using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemyscript : MonoBehaviour
{

    public float basespeed;
    private float basez;
    public float multiplier;

    public float basex;
    public float basey;

    public string power;


    // Start is called before the first frame update
    void Start()
    {

        basez = GameObject.Find("playercube").GetComponent<Playescript>().basez;


        if(transform.position ==Vector3.zero)
        {
            transform.position = new Vector3(basex, basey, basez);
        }

        if(transform.childCount >0)
        {
            int rdint = UnityEngine.Random.Range(0, 100);
            if (rdint == 10)
            {
                transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                transform.GetChild(0).gameObject.SetActive(false);
            }
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GetComponent<Rigidbody>().velocity = new Vector3(0,0,-basespeed)*multiplier;

        if (transform.position.x==0 && transform.position.y==0)
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
        if(!other.transform.GetComponent<Enemyscript>() && other.transform.tag!="wall")
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
                if(power!="")
                {
                    if(power=="shield")
                    {
                        GameObject.Find("playercube").GetComponent<Playescript>().StartIFrame(6f);
                        InstantiateText("Shield");
                    }
                    else if(power=="bomb")
                    {
                        InstantiateText("Bomb");
                        GameObject.Find("playercube").GetComponent<Playescript>().bombheld += 1;
                    }
                    else if (power == "life")
                    {
                        GameObject.Find("playercube").GetComponent<Playescript>().lives++;
                        InstantiateText("+1 Life");
                    }
                }
                else if(GameObject.Find("playercube").GetComponent<Playescript>().lives>0 && GameObject.Find("playercube").GetComponent<Playescript>().blinking<=0)
                {
                    GameObject.Find("playercube").GetComponent<Playescript>().lives--;
                    GameObject.Find("playercube").GetComponent<Playescript>().StartIFrame(2f);
                }
                Destroy(this.gameObject);
            }
        }
        
    }

    void InstantiateText(string texttoshow)
    {
        GameObject newtext = GameObject.Find("TextObject");

        newtext.transform.GetChild(0).gameObject.SetActive(true);

        newtext.GetComponentInChildren<TextMeshProUGUI>().text = texttoshow;


        if(texttoshow== "Shield")
        {
            newtext.transform.Find("Shield").gameObject.SetActive(true);
        }
        else if (texttoshow == "Bomb")
        {
            newtext.transform.Find("Buster").gameObject.SetActive(true);
        }
        else
        {
            newtext.transform.Find("Speed").gameObject.SetActive(true);
        }

        newtext.GetComponent<textscript>().frametofade = (int)(3/Time.deltaTime);

    }


}



