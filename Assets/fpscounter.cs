using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class fpscounter : MonoBehaviour
{



    // Update is called once per frame
    void Update()
    {
        GetComponent<TextMeshProUGUI>().text = "FPS : "+(int)(1.0f / Time.deltaTime) + "";
    }
}
