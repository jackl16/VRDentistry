using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gumTouchCount : MonoBehaviour
{
    int touchCount = 0;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Text textObj = GetComponent<Text>();
        textObj.text = "Gum Touch Counter: " + touchCount;

    }

    public int getTouchCount()
    {
        return touchCount;
    }

    public void addTouchCount()
    {
        touchCount++;
    }
}
