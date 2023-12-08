using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//public TMP_Text text;

public class ScoreFunction: MonoBehaviour
{
    int score = 10000;
    int done  = 0;
    // Start is called before the first frame update
    void Start()
    {
        TextMeshPro textObj = GetComponent<TextMeshPro>();
        textObj.SetText("");
    }

    // Update is called once per frame
    void Update()
    {
        GameObject pc = GameObject.Find("plaqueCounter");//.reducePlaqueCount();
		plaqueCount plaque = pc.GetComponent<plaqueCount>();
        int plaque_count  = plaque.getPlaqueCount();

        if (plaque_count == 0 && done == 0) {
            GameObject plane = GameObject.Find("ScorePlane");
            plane.transform.position = new Vector3((float)3, (float) 3.3,(float) -1.7);

            // GETTTTT THE RIGHT NAMES
            GameObject timer = GameObject.Find("Text (TMP)");//.reducePlaqueCount();
		    TimerFunction timer_time = timer.GetComponent<TimerFunction>();
            int time         = timer_time.getTimeInSecs();

		    GameObject gumCount    = GameObject.Find("gumCounter");//.reducePlaqueCount();
		    gumTouchCount touchObj = gumCount.GetComponent<gumTouchCount>();
            int touches            = touchObj.getTouchCount();

            score -= time * 35;
            score -= touches * 250;

           GameObject scoreText = GameObject.Find("ScoreTMP");//.reducePlaqueCount();
           TextMeshPro textObj = scoreText.GetComponent<TextMeshPro>();
            textObj.SetText("Gum touches: {0}\nTime: {1} seconds\nScore: {2}", (int)touches, (int)time, (int)score);
            done = -1;
        }


    }
}
