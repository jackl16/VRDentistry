using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//public TMP_Text text;

public class TimerFunction : MonoBehaviour
{
  
    double minutes = 0;
    double secondsOne = 0;
    double secondsTen = 0;
    // Start is called before the first frame update
    void Start()
    {
        TextMeshPro textObj = GetComponent<TextMeshPro>();
        textObj.SetText("0:00");
        //textObj.SetText("The first number is {0} and the 2nd is {1:2} and the 3rd is {3:0}.", 4, 6.345f, 3.5f);

        //string clockText=GetComponent<TMPro.TextMeshProUGUI>().text();
        //  text.SetText("0");
        //clockText = 0;
    }

    // Update is called once per frame
    void Update()
    {
        secondsOne = secondsOne + 0.001;///0.016;
        if (secondsOne >= 10)
        {

            secondsTen = secondsTen + 1;
            secondsOne = 0;
        }
        if (secondsTen >= 6)
        {

            minutes = minutes + 1;
            secondsTen= 0;
        }
        TextMeshPro textObj = GetComponent<TextMeshPro>();
        textObj.SetText("{0}:{1}{2}", (int)minutes, (int)secondsTen, (int)secondsOne);
    }

    public int getTimeInSecs()
    {
        return (int)(60*minutes+10*secondsTen+secondsOne);
    }
}
