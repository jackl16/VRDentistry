using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class plaqueCount : MonoBehaviour
{
    
    
    int plaqueNum=0;
    // Start is called before the first frame update
    void Start()
    {
        //gameObject[] gos = GameObject[]; 
        var gos = GameObject.FindGameObjectsWithTag("plaque");
        plaqueNum = gos.Length;

    }

    // Update is called once per frame
    void Update()
    {
        Text textObj = GetComponent<Text>();
        textObj.text = "Plaque Remaining: "+plaqueNum;

    }

    public int getPlaqueCount()
    {
        return plaqueNum;
    }

    public void reducePlaqueCount()
    {
        plaqueNum--;
    }
}
