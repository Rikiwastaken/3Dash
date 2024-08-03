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
            GetComponent<Rigidbody>().velocity = new Vector3(0,0,-5-speed);
            
        }


        if(this.transform.position.z<=-25)
        {
            Destroy(this);
        }

        for(int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<Renderer>().material.color.a < 1f)
            {
                Color col = transform.GetChild(i).GetComponent<Renderer>().material.color;
                Color newcol = new Color(col.r,col.g,col.b,col.a+0.1f);
                transform.GetChild(i).GetComponent<Renderer>().material.color = newcol;
            }
        }
        if(GameObject.Find("playercube").GetComponent<Playescript>().lives <= 0)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
}
