using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemyscript : MonoBehaviour
{

    public float basespeed;
    private float basez;
    public float multiplier=1f;

    public float basex;
    public float basey;


    // Start is called before the first frame update
    void Start()
    {

        basez = GameObject.Find("playercube").GetComponent<Playescript>().basez;


        if(transform.position ==Vector3.zero)
        {
            transform.position = new Vector3(basex, basey, basez);
        }

        multiplier = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Rigidbody>().velocity = new Vector3(0,0,-basespeed)*multiplier;

        if(transform.position.x==0 && transform.position.y==0)
        {
            Destroy(gameObject);
        }
        if (GameObject.Find("playercube").GetComponent<Playescript>().lives <= 0)
        {
            GetComponent<Rigidbody>().velocity= Vector3.zero;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.transform.GetComponent<Enemyscript>())
        {
            if (!other.transform.CompareTag("Player"))
            {
                if (GameObject.Find("playercube").GetComponent<Playescript>().lives > 0)
                {
                    GameObject.Find("playercube").GetComponent<Playescript>().score++;
                }
                Destroy(this.gameObject);
            }
            else
            {
                if(GameObject.Find("playercube").GetComponent<Playescript>().lives>0 && GameObject.Find("playercube").GetComponent<Playescript>().blinking<=0)
                {
                    GameObject.Find("playercube").GetComponent<Playescript>().lives--;
                    GameObject.Find("playercube").GetComponent<Playescript>().StartIFrame();
                }
                Destroy(this.gameObject);
            }
        }
        
    }
}
