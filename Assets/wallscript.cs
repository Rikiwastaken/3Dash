using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class wallscript : MonoBehaviour
{

    public bool moving;

    public float speed;

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            GetComponent<Rigidbody>().velocity = new Vector3(0,0,-speed);
            
        }
    }
}
