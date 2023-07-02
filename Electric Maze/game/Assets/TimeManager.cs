using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timeFont;
    int seconds;
    int minuts=0;
    float updateTimer = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TimeUp();
    }


    void TimeUp()
    {
        updateTimer += Time.deltaTime;
        if(updateTimer>=1)
        {
            seconds++;
            if(seconds>=60)
            {
                minuts++;
                seconds = 0;
            }
            updateTimer = 0;
        }
        timeFont.SetText("Time: " + string.Format("{00:00} {1:00}",minuts,seconds));

    }
}
