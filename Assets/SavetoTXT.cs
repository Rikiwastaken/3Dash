using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavetoTXT : MonoBehaviour
{
    private Transform Ennemis;

    private List<Transform> EnnemisList = new List<Transform>();

    public int stringtimer;

    void Start()
    {
        Ennemis = GameObject.Find("Enemies").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(stringtimer==0)
        {
            //SaveJSON(CreateString());

            stringtimer = 5;
        }
        else
        {
            stringtimer--;
        }
    }

    string CreateString()
    {

        string res = "{\n";
        int nbchildren = Ennemis.childCount;
        Transform playertransform = GameObject.Find("playercube").transform;
        EnnemisList = new List<Transform>();
        res += "\"joueur\":{\"coordinates\":[" + playertransform.position.x + "," + playertransform.position.y + "," + playertransform.position.z + "],\"lives\":"+playertransform.GetComponent<Playescript>().lives+",\"bombs\":" + playertransform.GetComponent<Playescript>().bombheld +",\"score\":"+playertransform.GetComponent<Playescript>().score+ "},\n";
        for (int i = 0; i < nbchildren; i++)
        {
            Transform t = Ennemis.GetChild(i);
            res += "\"e"+i+ "\":{\"coordinates\":[" + t.position.x + "," + t.position.y + "," + t.position.z+"],\"velocity\":["+t.GetComponent<Rigidbody>().velocity.x+ "," + t.GetComponent<Rigidbody>().velocity.y +"," + t.GetComponent<Rigidbody>().velocity.z+"]";

            if (t.GetComponent<Enemyscript>().movedirection == 1)
            {
                res += ",\"state\":\"x\"";
            }
            else if (t.GetComponent<Enemyscript>().movedirection == 2)
            {
                res += ",\"state\":\"y\"";
            }
            else if (t.GetComponent<Enemyscript>().power!="")
            {
                res += ",\"state\":\""+ t.GetComponent<Enemyscript>().power + "\"";
            }
            else
            {
                res += ",\"state\":\"NA\"";
            }

            res += "}";
            if(i!=nbchildren-1)
            {
                res += ",";
            }
            res += "\n";
        }
        res += "}";
        return res;
    }

    void SaveJSON(string JSONtosave)
    {
        System.IO.File.WriteAllText(Application.persistentDataPath + "/Ennemis.json", JSONtosave);
    }

}
