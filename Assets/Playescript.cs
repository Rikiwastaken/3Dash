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
    

    public float xlimit;
    public float ylimit;

    private Vector2 movementinput;

    private Vector3 nextpos;

    public float multiplicateur;



    // Start is called before the first frame update
    void Start()
    {
        movement.action.performed += OnMovementChange;
        nextpos = transform.position;
    }

    public void OnMovementChange(InputAction.CallbackContext context)
    {
        Debug.Log("dzqqzdqd");
        movementinput = context.ReadValue<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position,nextpos) < 0.01f)
        {
            transform.position = nextpos;
        }

        Debug.Log(movementinput);

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



    }
}
