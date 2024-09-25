using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class fpscounter : MonoBehaviour
{

    public float fps1;
    public float fps2;
    public float fps3;
    public float fps4;
    public float fps5;

    // Update is called once per frame
    void FixedUpdate()
    {

        if(fps1 ==0f)
        {
            fps1 = 1f/Time.unscaledDeltaTime;
        }
        else if(fps2 ==0f)
        {
            fps2 = 1f/Time.unscaledDeltaTime;
        }
        else if (fps3 ==0f)
        {
            fps3 = 1f / Time.unscaledDeltaTime;
        }
        else if (fps4 ==0f)
        {
            fps4 = 1f/Time.unscaledDeltaTime;
        }
        else if ( fps5 ==0f)
        {
            fps5 = 1f / Time.unscaledDeltaTime;
        }
        else
        {
            fps5 = fps4;
            fps4 = fps3;
            fps3 = fps2;
            fps2 = fps1;
            fps1 = 1f / Time.unscaledDeltaTime;
        }

        float averaged = (fps1+fps2+fps3+fps4+fps5) / 5f;

        GetComponent<TextMeshProUGUI>().text = "FPS : "+(int)(averaged) + "";
    }
}
