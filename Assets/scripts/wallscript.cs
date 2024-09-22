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
            //GetComponent<Rigidbody>().velocity = new Vector3(0,0,-5-speed);
            GetComponent<Rigidbody>().velocity = new Vector3(0, 0, -10 * speed);

        }


        if(this.transform.position.z<=-75)
        {
            GameObject[] walls = GameObject.FindGameObjectsWithTag("wall");
            int nbwalls=0;
            foreach (GameObject wall in walls)
            {
                if(wall.GetComponent<wallscript>() != null)
                {
                    nbwalls++;
                }
            }
            if(nbwalls > 3)
            {
                Destroy(this.gameObject);
            }
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
        if(other.transform.GetComponent<wallscript>() != null)
        {
            if(Mathf.Abs(other.transform.position.z-transform.position.z)<=1f)
            {
                if (other.GetComponent<wallscript>().ID < GetComponent<wallscript>().ID)
                {
                    Destroy(other.gameObject);
                }
            }
        }
    }
}
