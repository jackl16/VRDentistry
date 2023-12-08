using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlaqueShrinkScript : MonoBehaviour
{
	public enum EFFECT_TYPE { CONSTANT, VISCOUS, SPRING, FRICTION, VIBRATE };


	// Public, User-Adjustable Settings
	public EFFECT_TYPE effectType = EFFECT_TYPE.VISCOUS; //!< Which type of effect occurs within this zone?
	[Range(0.0f, 1.0f)] public double Gain = 0.333f;
	[Range(0.0f, 1.0f)] public double Magnitude = 0.333f;
	[Range(1.0f, 1000.0f)] public double Frequency = 200.0f;
	private double Duration = 1.0f;
	public Vector3 Position = Vector3.zero;
	public Vector3 Direction = Vector3.up;


	float shrinkRate = 0.90f;
	// Keep track of the Haptic Devices
	HapticPlugin[] devices;
	bool[] inTheZone;       //Is the stylus in the effect zone?
	Vector3[] devicePoint;  // Current location of stylus
	float[] delta;          // Distance from stylus to zone collider.
	int[] FXID;             // ID of the effect.  (Per device.)

	
	// These are the user adjustable vectors, converted to world-space. 
	private Vector3 focusPointWorld = Vector3.zero;
	private Vector3 directionWorld = Vector3.up;


							// Start is called before the first frame update
	void Start()
    {
		//Initialize the list of haptic devices.
		devices = (HapticPlugin[])Object.FindObjectsOfType(typeof(HapticPlugin));
		inTheZone = new bool[devices.Length];
		devicePoint = new Vector3[devices.Length];
		delta = new float[devices.Length];
		FXID = new int[devices.Length];


		// Generate an OpenHaptics effect ID for each of the devices.
		for (int ii = 0; ii < devices.Length; ii++)
		{
			inTheZone[ii] = false;
			devicePoint[ii] = Vector3.zero;
			delta[ii] = 0.0f;
			FXID[ii] = HapticPlugin.effects_assignEffect(devices[ii].configName);
		}
		GetComponent<MeshRenderer>().material.color = new Color((float)0.87, (float)0.65, (float)0.09, (float)0.5);
	}

	// Update is called once per frame
	void Update()
    {
        Collider collider = gameObject.GetComponent<Collider>();
        if (collider == null)
        {
            Debug.LogError("This Haptic Effect Zone requires a collider");
            return;
        }

		// Update the World-Space vectors
		focusPointWorld = transform.TransformPoint(Position);
		directionWorld = transform.TransformDirection(Direction);

		// Update the effect seperately for each haptic device.
		for (int ii = 0; ii < devices.Length; ii++)
		{
			HapticPlugin device = devices[ii];
			bool oldInTheZone = inTheZone[ii];
			int ID = FXID[ii];

			// If a haptic effect has not been assigned through Open Haptics, assign one now.
			if (ID == -1)
			{
				FXID[ii] = HapticPlugin.effects_assignEffect(devices[ii].configName);
				ID = FXID[ii];

				if (ID == -1) // Still broken?
				{
					Debug.LogError("Unable to assign Haptic effect.");
					continue;
				}
			}

			// Determine if the stylus is in the "zone". 
			Vector3 StylusPos = device.stylusPositionWorld; //World Coordinates
			Vector3 CP = collider.ClosestPoint(StylusPos);  //World Coordinates
			devicePoint[ii] = CP;
			delta[ii] = (CP - StylusPos).magnitude;

			//If the stylus is within the Zone, The ClosestPoint and the Stylus point will be identical.
			if (delta[ii] <= Mathf.Epsilon)
			{
				inTheZone[ii] = true;

				// Convert from the World coordinates to coordinates relative to the haptic device.
				Vector3 focalPointDevLocal = device.transform.InverseTransformPoint(focusPointWorld);
				Vector3 rotationDevLocal = device.transform.InverseTransformDirection(directionWorld);
				double[] pos = { focalPointDevLocal.x, focalPointDevLocal.y, focalPointDevLocal.z };
				double[] dir = { rotationDevLocal.x, rotationDevLocal.y, rotationDevLocal.z };

				double Mag = Magnitude;

				if (device.isInSafetyMode())
					Mag = 0;

				// Send the current effect settings to OpenHaptics.
				HapticPlugin.effects_settings(
					device.configName,
					ID,
					Gain,
					Mag,
					Frequency,
					pos,
					dir);
				HapticPlugin.effects_type(
					device.configName,
					ID,
					(int)effectType);

			}
			else
			{
				inTheZone[ii] = false;

				// Note : If the device is not in the "Zone", there is no need to update the effect settings.
			}

			// If the on/off state has changed since last frame, send a Start or Stop event to OpenHaptics
			if (oldInTheZone != inTheZone[ii])
			{
				if (inTheZone[ii])
				{
					Vector3 scale = transform.localScale;
					transform.localScale = scale * shrinkRate;

					//float sizeX=GetComponent<Collider>().bounds.size;
					float size= GetComponent<MeshRenderer>().bounds.size.x;
					// GetComponent<MeshRenderer>().material.SetColor("_Color", Color.red);
					//size = renderer.bounds.size;
					bool check = transform.localScale.x < -5;


					HapticPlugin.effects_startEffect(device.configName, ID);
					Debug.Log("Collider Size : " + transform.localScale.x);
					Debug.Log("Collider Size : " + check);
					if (transform.localScale.x < 0.07)
					{
						//HapticPlugin.effects_stopEffect(device.configName, ID);
						//Destroy(gameObject);
						transform.localScale = scale *0;
						GameObject pc = GameObject.Find("plaqueCounter");//.reducePlaqueCount();
						plaqueCount plaque = pc.GetComponent<plaqueCount>();
						plaque.reducePlaqueCount();
					}
				}
				else
				{
					// GetComponent<MeshRenderer>().material.color = new Color( (float)0.87, (float)0.65, (float)0.09, (float)0.5);
					HapticPlugin.effects_stopEffect(device.configName, ID);
				}
			}

		}



    }

   
}
