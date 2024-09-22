using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class wallscript : MonoBehaviour
{

    public bool moving;

    public float speed;

    public float ID;

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

        if(transform.localScale.x<=1)
        {
            transform.localScale = transform.localScale*1.00005f;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.name== "WallBehind")
        {
            GameObject[] walls = GameObject.FindGameObjectsWithTag("wall");
            foreach(GameObject wall in walls)
            {
                if(wall.GetComponent<wallscript>() != null)
                {
                    if (wall.GetComponent<wallscript>().ID < GetComponent<wallscript>().ID)
                    {
                        Destroy(wall);
                    }
                }
            }
        }
        else if(other.transform.GetComponent<wallscript>() != null)
        {
            if(Mathf.Abs(other.transform.position.x-transform.position.x)<=1f)
            {
                if (other.GetComponent<wallscript>().ID < GetComponent<wallscript>().ID)
                {
                    Destroy(other);
                }
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.transform.GetComponent<wallscript>() != null)
        {
            if (Mathf.Abs(other.transform.localScale.x - transform.localScale.x) <= 0.1f)
            {
                if (other.GetComponent<wallscript>().ID < GetComponent<wallscript>().ID)
                {
                    other.transform.localScale = other.transform.localScale / 1.00005f;
                }
            }
        }
    }
}
