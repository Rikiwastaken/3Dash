using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemyscript : MonoBehaviour
{

    public float basespeed;
    public float basez;
    private float multiplier=1f;

    public float basex;
    public float basey;


    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(basex,basey,basez);
        multiplier = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Rigidbody>().velocity = new Vector3(0,0,-basespeed)*multiplier;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(this.gameObject);
    }
}
