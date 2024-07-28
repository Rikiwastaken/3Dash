using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public float timebetweenspawn;

    public GameObject EnemyCube;

    public int score;

    public int lives = 3;

    private int lastCD;

    public TextMeshProUGUI scoretxt;

    public TextMeshProUGUI livestxt;

    public TextMeshProUGUI leveltxt;

    private int generation;

    public List<Material> materials;

    private int lastlvl;

    private bool wall1next;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    // Start is called before the first frame update
    void Start()
    {
        movement.action.performed += OnMovementChange;
        nextpos = transform.position;
        
    }

    public void OnMovementChange(InputAction.CallbackContext context)
    {
        movementinput = context.ReadValue<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {

        if(lastlvl !=level)
        {
            lastlvl = level;
            if(level==1)
            {
                MoveWall(level,1f);
            }
            else if(level==2 || level ==3)
            {
                MoveWall(level,2f);
            }
            else if (level == 4 || level == 5)
            {
                MoveWall(level, 3f);
            }
            else if (level == 6)
            {
                MoveWall(level, 4f);
            }
            else if(level >= 7)
            {
                float mult = level / 2 + 1;
                MoveWall(level, mult);
            }

        }


        scoretxt.text = "Score : " + score;

        livestxt.text = lives + " lives";

        leveltxt.text = "Level : " + (level-1);

        ManageMovement();

        if (spawncdcntr==0)
        {
            generation++;
            SpawnEnemywave(level);
            spawncdcntr = (int)(timebetweenspawn/(Time.deltaTime));
            if(level>2)
            {
                spawncdcntr = (int)(spawncdcntr / level) * 2;
            }

        }
        else
        {
            spawncdcntr--;
        }

        level = (int)(generation / 10) + 2;

    }

    private void SpawnEnemywave(int level)
    {
        if (level <= 2)
        {
            GameObject newcube = SpawnACube();


            if (level==2)
            {
                newcube.GetComponent<Enemyscript>().multiplier = 2f;
            }
            
        }

        if(level==3 || level==4)
        {
            int rdint = UnityEngine.Random.Range(0, 2);
            if(rdint == 0)
            {
                GameObject newcube = SpawnACube();
                if(level==3)
                {
                    newcube.GetComponent<Enemyscript>().multiplier = 2f;
                    
                }
                else
                {
                    newcube.GetComponent<Enemyscript>().multiplier = 3f;
                    
                }
            }
            else
            {
                List<GameObject> newcubelist = SpawnALineOf2();
                if (level == 3)
                {
                    newcubelist[0].GetComponent<Enemyscript>().multiplier = 2f;
                    newcubelist[1].GetComponent<Enemyscript>().multiplier = 2f;
                }
                else
                {
                    newcubelist[0].GetComponent<Enemyscript>().multiplier = 3f;
                    newcubelist[0].GetComponent<Enemyscript>().multiplier = 3f;
                    
                }
            }

        }

        if( level == 5 || level == 6)
        {
            int rdint = UnityEngine.Random.Range(0, 3);
            if (rdint == 0)
            {
                GameObject newcube = SpawnACube();
                if (level == 5)
                {
                    newcube.GetComponent<Enemyscript>().multiplier = 3f;
                }
                else
                {
                    newcube.GetComponent<Enemyscript>().multiplier = 4f;
                }
            }
            else if (rdint == 1)
            {
                List<GameObject> newcubelist = SpawnALineOf2();
                if (level == 5)
                {
                    newcubelist[0].GetComponent<Enemyscript>().multiplier = 3f;
                    newcubelist[1].GetComponent<Enemyscript>().multiplier = 3f;
                }
                else
                {
                    newcubelist[0].GetComponent<Enemyscript>().multiplier = 4f;
                    newcubelist[1].GetComponent<Enemyscript>().multiplier = 4f;
                }
            }
            else if (rdint == 1)
            {
                List<GameObject> newcubelist = SpawnFullLine();
                if (level == 5)
                {
                    newcubelist[0].GetComponent<Enemyscript>().multiplier = 3f;
                    newcubelist[1].GetComponent<Enemyscript>().multiplier = 3f;
                    newcubelist[2].GetComponent<Enemyscript>().multiplier = 3f;
                }
                else
                {
                    newcubelist[0].GetComponent<Enemyscript>().multiplier = 4f;
                    newcubelist[1].GetComponent<Enemyscript>().multiplier = 4f;
                    newcubelist[2].GetComponent<Enemyscript>().multiplier = 4f;
                }
            }
            if(level==5)
            {
                
            }
            else
            {
                
            }
        }

        if( level >=7)
        {
            float mult = level/2 +1;

            int rdint = UnityEngine.Random.Range(0, 3);
            if (rdint == 0)
            {
                GameObject newcube = SpawnACube();
                newcube.GetComponent<Enemyscript>().multiplier = mult;
            }
            else if (rdint == 1)
            {
                List<GameObject> newcubelist = SpawnALineOf2();
                newcubelist[0].GetComponent<Enemyscript>().multiplier = mult;
                newcubelist[1].GetComponent<Enemyscript>().multiplier = mult;
            }
            else if (rdint == 1)
            {
                List<GameObject> newcubelist = SpawnFullLine();
                newcubelist[0].GetComponent<Enemyscript>().multiplier = mult;
                newcubelist[1].GetComponent<Enemyscript>().multiplier = mult;
                newcubelist[2].GetComponent<Enemyscript>().multiplier = mult;
            }
        }

    }

    private void ManageMovement()
    {
        if (Vector3.Distance(transform.position, nextpos) < 0.01f)
        {
            transform.position = nextpos;
        }

        if (transform.position == nextpos)
        {
            if (nextpos.y == -ylimit && movementinput.y > 0)
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
                nextpos = new Vector3(-xlimit, nextpos.y, transform.position.z);
            }
        }
        else
        {
            GetComponent<Rigidbody>().velocity = ((nextpos - transform.position) * multiplicateur);
        }
    }

    private GameObject SpawnACube()
    {
        GameObject newcube = Instantiate(EnemyCube);
        int rdint = UnityEngine.Random.Range(-1, 2);
        Debug.Log(rdint);
        if (rdint == -1)
        {
            newcube.GetComponent<Enemyscript>().basex = -0.6f;
        }
        else if (rdint == 1)
        {
            newcube.GetComponent<Enemyscript>().basex = 0.6f;
        }
        else
        {
            newcube.GetComponent<Enemyscript>().basex = 0f;
        }
        rdint = UnityEngine.Random.Range(-1, 2);
        if (rdint == -1)
        {
            newcube.GetComponent<Enemyscript>().basey = -0.6f;
        }
        else if (rdint == 1)
        {
            newcube.GetComponent<Enemyscript>().basey = 0.6f;
        }
        else if (newcube.GetComponent<Enemyscript>().basex != 0f)
        {
            newcube.GetComponent<Enemyscript>().basey = 0f;
        }
        else
        {
            newcube.GetComponent<Enemyscript>().basey = 0.6f;
        }
        return newcube;
    }

    private List<GameObject> SpawnALineOf2()
    {
        List<GameObject> list = new List<GameObject>();
        GameObject newcube = Instantiate(EnemyCube);
        int rdint = UnityEngine.Random.Range(0, 3);
        if (rdint == 0)
        {
            newcube.GetComponent<Enemyscript>().basex = -0.6f;
        }
        else if (rdint == 1)
        {
            newcube.GetComponent<Enemyscript>().basex = 0.6f;
        }
        else
        {
            newcube.GetComponent<Enemyscript>().basex = 0f;
        }
        rdint = UnityEngine.Random.Range(0, 3);
        if (rdint == 0)
        {
            newcube.GetComponent<Enemyscript>().basey = -0.6f;
        }
        else if (rdint == 1)
        {
            newcube.GetComponent<Enemyscript>().basey = 0.6f;
        }
        else if (newcube.GetComponent<Enemyscript>().basex != 0f)
        {
            newcube.GetComponent<Enemyscript>().basey = 0f;
        }
        else
        {
            newcube.GetComponent<Enemyscript>().basey = 0.6f;
        }
        list.Add(newcube);


        GameObject othercube = Instantiate(EnemyCube);

        rdint = UnityEngine.Random.Range(0, 2);

        if(rdint == 0)
        {
            othercube.GetComponent<Enemyscript>().basex = newcube.GetComponent<Enemyscript>().basex;


            rdint = UnityEngine.Random.Range(0, 2);
            if(newcube.GetComponent<Enemyscript>().basey==-0.6f)
            {
                if(rdint==0)
                {
                    othercube.GetComponent<Enemyscript>().basey = 0f;
                }
                else
                {
                    othercube.GetComponent<Enemyscript>().basey = 0.6f;
                }
            }
            else if (newcube.GetComponent<Enemyscript>().basey == 0.6f)
            {
                if (rdint == 0)
                {
                    othercube.GetComponent<Enemyscript>().basey = 0f;
                }
                else
                {
                    othercube.GetComponent<Enemyscript>().basey = -0.6f;
                }
            }
            else
            {
                if (rdint == 0)
                {
                    othercube.GetComponent<Enemyscript>().basey = 0.6f;
                }
                else
                {
                    othercube.GetComponent<Enemyscript>().basey = -0.6f;
                }
            }

        }
        else
        {
            othercube.GetComponent<Enemyscript>().basey = newcube.GetComponent<Enemyscript>().basey;

            rdint = UnityEngine.Random.Range(0, 2);
            if (newcube.GetComponent<Enemyscript>().basex == -0.6f)
            {
                if (rdint == 0 && othercube.GetComponent<Enemyscript>().basey !=0f)
                {
                    othercube.GetComponent<Enemyscript>().basex = 0f;
                }
                else
                {
                    othercube.GetComponent<Enemyscript>().basex = 0.6f;
                }
            }
            else if (newcube.GetComponent<Enemyscript>().basex == 0.6f)
            {
                if (rdint == 0 && othercube.GetComponent<Enemyscript>().basey != 0f)
                {
                    othercube.GetComponent<Enemyscript>().basex = 0f;
                }
                else
                {
                    othercube.GetComponent<Enemyscript>().basex = -0.6f;
                }
            }
            else
            {
                if (rdint == 0)
                {
                    othercube.GetComponent<Enemyscript>().basex = 0.6f;
                }
                else
                {
                    othercube.GetComponent<Enemyscript>().basex = -0.6f;
                }
            }
        }

        list.Add(othercube);

        return list;
    }

    private List<GameObject> SpawnFullLine()
    {
        var list = new List<GameObject>();

        int rdint = UnityEngine.Random.Range(0, 4);

        if(rdint == 0)
        {
            GameObject obj1 = Instantiate(EnemyCube);
            obj1.GetComponent<Enemyscript>().basex = 0.6f;
            obj1.GetComponent<Enemyscript>().basey = -0.6f;

            GameObject obj2 = Instantiate(EnemyCube);
            obj2.GetComponent<Enemyscript>().basex = 0.6f;
            obj2.GetComponent<Enemyscript>().basey = 0f;

            GameObject obj3 = Instantiate(EnemyCube);
            obj3.GetComponent<Enemyscript>().basex = 0.6f;
            obj3.GetComponent<Enemyscript>().basey = 0.6f;

            list.Add(obj1);
            list.Add(obj2);
            list.Add(obj3);

        }
        else if (rdint == 1)
        {
            GameObject obj1 = Instantiate(EnemyCube);
            obj1.GetComponent<Enemyscript>().basex = -0.6f;
            obj1.GetComponent<Enemyscript>().basey = -0.6f;

            GameObject obj2 = Instantiate(EnemyCube);
            obj2.GetComponent<Enemyscript>().basex = -0.6f;
            obj2.GetComponent<Enemyscript>().basey = 0f;

            GameObject obj3 = Instantiate(EnemyCube);
            obj3.GetComponent<Enemyscript>().basex = -0.6f;
            obj3.GetComponent<Enemyscript>().basey = 0.6f;

            list.Add(obj1);
            list.Add(obj2);
            list.Add(obj3);

        } else if (rdint == 2)
        {
            GameObject obj1 = Instantiate(EnemyCube);
            obj1.GetComponent<Enemyscript>().basey = 0.6f;
            obj1.GetComponent<Enemyscript>().basex = -0.6f;

            GameObject obj2 = Instantiate(EnemyCube);
            obj2.GetComponent<Enemyscript>().basey = 0.6f;
            obj2.GetComponent<Enemyscript>().basex = 0f;

            GameObject obj3 = Instantiate(EnemyCube);
            obj3.GetComponent<Enemyscript>().basey = 0.6f;
            obj3.GetComponent<Enemyscript>().basex = 0.6f;
            list.Add(obj1);
            list.Add(obj2);
            list.Add(obj3);

        }
        else
        {
            GameObject obj1 = Instantiate(EnemyCube);
            obj1.GetComponent<Enemyscript>().basey = -0.6f;
            obj1.GetComponent<Enemyscript>().basex = -0.6f;

            GameObject obj2 = Instantiate(EnemyCube);
            obj2.GetComponent<Enemyscript>().basey = -0.6f;
            obj2.GetComponent<Enemyscript>().basex = 0f;

            GameObject obj3 = Instantiate(EnemyCube);
            obj3.GetComponent<Enemyscript>().basey = -0.6f;
            obj3.GetComponent<Enemyscript>().basex = 0.6f;

            list.Add(obj1);
            list.Add(obj2);
            list.Add(obj3);

        }

        return list;
    }

    private void MoveWall(int level, float speed)
    {
        GameObject wall1 = GameObject.Find("wall1");

        GameObject wall2 = GameObject.Find("wall2");


        if(!wall1next)
        {
            wall2.transform.position = new Vector3(0, 0, wall1.transform.position.z+25);
            for (int i = 0; i < wall2.transform.childCount; i++)
            {
                wall2.transform.GetChild(i).GetComponent<Renderer>().material = materials[level];
            }
            wall1next = true;
        }
        else
        {
            wall1.transform.position = new Vector3(0, 0, wall2.transform.position.z + 25);
            for (int i = 0; i < wall1.transform.childCount; i++)
            {
                wall1.transform.GetChild(i).GetComponent<Renderer>().material = materials[level];
            }
            wall1next= false;
        }
        wall1.GetComponent<wallscript>().speed = speed;
        wall1.GetComponent<wallscript>().moving = true;
        wall2.GetComponent<wallscript>().speed = speed;
        wall2.GetComponent<wallscript>().moving = true;
       

    }

}
