using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Playescript : MonoBehaviour
{

    public PlayerInput PlayerInput;

    public InputActionReference movement;
    public InputActionReference start;

    public int speedmode;

    private int level=1;
    

    public float xlimit;
    public float ylimit;

    private Vector2 movementinput;

    private Vector3 nextpos;

    public float multiplicateur;

    private int spawncdcntr;
    public int timebetweenspawn;

    public GameObject EnemyCube;

    // Start is called before the first frame update
    void Start()
    {
        movement.action.performed += OnMovementChange;
        nextpos = transform.position;
        Application.targetFrameRate = 120;
    }

    public void OnMovementChange(InputAction.CallbackContext context)
    {
        movementinput = context.ReadValue<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position,nextpos) < 0.01f)
        {
            transform.position = nextpos;
        }

        if(transform.position == nextpos)
        {
            if(nextpos.y==-ylimit && movementinput.y>0)
            {
                nextpos = new Vector3(nextpos.x, ylimit, transform.position.z);
            }
            else if (nextpos.y == ylimit && movementinput.y < 0)
            {
                nextpos = new Vector3(nextpos.x, -ylimit, transform.position.z);
            }
            if (nextpos.x == -xlimit && movementinput.x > 0)
            {
                nextpos = new Vector3(xlimit, nextpos.y, transform.position.z);
            }
            else if (nextpos.x == xlimit && movementinput.x < 0)
            {
                nextpos = new Vector3(-xlimit,nextpos.y, transform.position.z);
            }
        }
        else
        {
            GetComponent<Rigidbody>().velocity=((nextpos - transform.position)*multiplicateur);
        }

        if(spawncdcntr==0)
        {
            SpawnEnemywave(level);
            spawncdcntr = (int)(timebetweenspawn/(Time.deltaTime*level));
        }
        else
        {
            spawncdcntr--;
        }

    }

    private void SpawnEnemywave(int level)
    {
        if (level == 1)
        {
            GameObject newcube = Instantiate(EnemyCube);
            int rdint = Random.Range(-1, 1);
            if(rdint == -1)
            {
                newcube.GetComponent<Enemyscript>().basex = -0.6f;
            }
            else if(rdint == 1)
            {
                newcube.GetComponent<Enemyscript>().basex = 0.6f;
            }
            else
            {
                newcube.GetComponent<Enemyscript>().basex = 0f;
            }
            rdint = Random.Range(-1, 1);
            if (rdint == -1)
            {
                newcube.GetComponent<Enemyscript>().basey = -0.6f;
            }
            else if (rdint == 1)
            {
                newcube.GetComponent<Enemyscript>().basey = 0.6f;
            }
            else
            {
                newcube.GetComponent<Enemyscript>().basey = 0f;
            }
        }
        
    }
}
