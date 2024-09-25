using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class textscript : MonoBehaviour
{
    // Start is called before the first frame update

    public int frametofade;

    // Update is called once per frame
    void FixedUpdate()
    {
        if(frametofade > 0)
        {
            frametofade--;
        }
        if(frametofade <= 0)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.Find("Shield").gameObject.SetActive(false);
            transform.Find("Speed").gameObject.SetActive(false);
            transform.Find("Buster").gameObject.SetActive(false);
        }
    }
}
