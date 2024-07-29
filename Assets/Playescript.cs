using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

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

    public GameObject wallprefab;

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

    public float basez;

    private int lastindex;

    public GameObject gameover;

    private List<GameObject> walllist;

    public int blinking;

    public int blinkingcnt;
    private bool inv;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    // Start is called before the first frame update
    void Start()
    {
        movement.action.performed += OnMovementChange;
        start.action.performed += OnStartPress;
        nextpos = transform.position;
        generatematerials();
        walllist = new List<GameObject>();

    }

    public void OnMovementChange(InputAction.CallbackContext context)
    {
        movementinput = context.ReadValue<Vector2>();
    }

    public void OnStartPress(InputAction.CallbackContext context)
    {
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {

        if(blinking>0)
        {
            blinking--;
            if (blinkingcnt > 0)
            {
                blinkingcnt--;
                
            }
            if (blinkingcnt == 0)
            {
                blinkingcnt = (int)(0.2f / Time.deltaTime);
                inv=!inv;
            }
        }
        else
        {
            blinkingcnt = 0;
            inv=false;
        }

        if(inv)
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }



        if(lives<=0)
        {
            gameover.SetActive(true);
        }

        if(lastlvl !=level && lives>0)
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

        if (spawncdcntr==0 && lives>0)
        {
            generation++;
            SpawnEnemywave(level);
            spawncdcntr = (int)(timebetweenspawn/(Time.deltaTime));
            if(level>2 && level<=15)
            {
                spawncdcntr = (int)(spawncdcntr / level) * 2;
            }
            else if(level>15)
            {
                spawncdcntr = (int)(spawncdcntr / 15) * 2;
            }

        }
        else
        {
            spawncdcntr--;
        }

        level = (int)(generation / 10) + 2;

    }

    public void StartIFrame()
    {
        blinking =(int)(2/Time.deltaTime);
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

        if (transform.position == nextpos && lives>0)
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
        
        int rdint = UnityEngine.Random.Range(-1, 2);
        Vector3 newposition = new Vector3(0, 0, basez);
        if (rdint == -1)
        {
            newposition.x = -0.6f;
        }
        else if (rdint == 1)
        {
            newposition.x = 0.6f;
        }
        else
        {
            newposition.x = 0f;
        }
        rdint = UnityEngine.Random.Range(-1, 2);
        if (rdint == -1)
        {
            newposition.y = -0.6f;
        }
        else if (rdint == 1)
        {
            newposition.y = 0.6f;
        }
        else if (newposition.x != 0f)
        {
            newposition.y = 0f;
        }
        else
        {
            newposition.y = 0.6f;
        }
        GameObject newcube = Instantiate(EnemyCube, newposition,Quaternion.identity);
        return newcube;
    }

    private List<GameObject> SpawnALineOf2()
    {
        List<GameObject> list = new List<GameObject>();
        Vector3 newposition = new Vector3(0, 0, basez);
        int rdint = UnityEngine.Random.Range(0, 3);
        if (rdint == 0)
        {
            newposition.x = -0.6f;
        }
        else if (rdint == 1)
        {
            newposition.x = 0.6f;
        }
        else
        {
            newposition.x = 0f;
        }
        rdint = UnityEngine.Random.Range(0, 3);
        if (rdint == 0)
        {
            newposition.y = -0.6f;
        }
        else if (rdint == 1)
        {
            newposition.y = 0.6f;
        }
        else if (newposition.x != 0f)
        {
            newposition.y = 0f;
        }
        else
        {
            newposition.y = 0.6f;
        }

        GameObject newcube = Instantiate(EnemyCube, newposition, Quaternion.identity);

        list.Add(newcube);

        Vector3 otherposition = new Vector3(0, 0, basez);

        rdint = UnityEngine.Random.Range(0, 2);

        if(rdint == 0)
        {
            otherposition.x = newposition.x;


            rdint = UnityEngine.Random.Range(0, 2);
            if(newposition.y ==-0.6f)
            {
                if(rdint==0)
                {
                    otherposition.y = 0f;
                }
                else
                {
                    otherposition.y = 0.6f;
                }
            }
            else if (newposition.y == 0.6f)
            {
                if (rdint == 0)
                {
                    otherposition.y = 0f;
                }
                else
                {
                    otherposition.y = -0.6f;
                }
            }
            else
            {
                if (rdint == 0)
                {
                    otherposition.y = 0.6f;
                }
                else
                {
                    otherposition.y = -0.6f;
                }
            }

        }
        else
        {
            otherposition.y = newposition.y;

            rdint = UnityEngine.Random.Range(0, 2);
            if (newposition.x == -0.6f)
            {
                if (rdint == 0 && otherposition.y !=0f)
                {
                    otherposition.x = 0f;
                }
                else
                {
                    otherposition.x = 0.6f;
                }
            }
            else if (newposition.x == 0.6f)
            {
                if (rdint == 0 && otherposition.y != 0f)
                {
                    otherposition.x = 0f;
                }
                else
                {
                    otherposition.x = -0.6f;
                }
            }
            else
            {
                if (rdint == 0)
                {
                    otherposition.x = 0.6f;
                }
                else
                {
                    otherposition.x = -0.6f;
                }
            }
        }

        GameObject othercube = Instantiate(EnemyCube, otherposition, Quaternion.identity);


        list.Add(othercube);

        return list;
    }

    private List<GameObject> SpawnFullLine()
    {
        var list = new List<GameObject>();

        int rdint = UnityEngine.Random.Range(0, 4);

        if(rdint == 0)
        {
            Vector3 newposition = new Vector3(0.6f, 0.6f, basez);
            GameObject obj1 = Instantiate(EnemyCube, newposition, Quaternion.identity);

            newposition = new Vector3(0.6f, 0f, basez);
            GameObject obj2 = Instantiate(EnemyCube, newposition, Quaternion.identity);

            newposition = new Vector3(0.6f, -0.6f, basez);
            GameObject obj3 = Instantiate(EnemyCube, newposition, Quaternion.identity);

            list.Add(obj1);
            list.Add(obj2);
            list.Add(obj3);

        }
        else if (rdint == 1)
        {

            Vector3 newposition = new Vector3(-0.6f, 0.6f, basez);
            GameObject obj1 = Instantiate(EnemyCube, newposition, Quaternion.identity);

            newposition = new Vector3(-0.6f, 0f, basez);
            GameObject obj2 = Instantiate(EnemyCube, newposition, Quaternion.identity);

            newposition = new Vector3(-0.6f, -0.6f, basez);
            GameObject obj3 = Instantiate(EnemyCube, newposition, Quaternion.identity);


            list.Add(obj1);
            list.Add(obj2);
            list.Add(obj3);

        } else if (rdint == 2)
        {

            Vector3 newposition = new Vector3(0.6f, 0.6f, basez);
            GameObject obj1 = Instantiate(EnemyCube, newposition, Quaternion.identity);

            newposition = new Vector3(0, 0.6f, basez);
            GameObject obj2 = Instantiate(EnemyCube, newposition, Quaternion.identity);

            newposition = new Vector3(-0.6f, 0.6f, basez);
            GameObject obj3 = Instantiate(EnemyCube, newposition, Quaternion.identity);


            list.Add(obj1);
            list.Add(obj2);
            list.Add(obj3);

        }
        else
        {
            Vector3 newposition = new Vector3(0.6f, -0.6f, basez);
            GameObject obj1 = Instantiate(EnemyCube, newposition, Quaternion.identity);

            newposition = new Vector3(0f, -0.6f, basez);
            GameObject obj2 = Instantiate(EnemyCube, newposition, Quaternion.identity);

            newposition = new Vector3(-0.6f, -0.6f, basez);
            GameObject obj3 = Instantiate(EnemyCube, newposition, Quaternion.identity);


            list.Add(obj1);
            list.Add(obj2);
            list.Add(obj3);

        }

        return list;
    }

    private void MoveWall(int level, float speed)
    {
        float scale = (float)(300 - level) / 300;
        GameObject newwall = Instantiate(wallprefab, new Vector3(0, 0, basez), Quaternion.identity);
        newwall.transform.localScale= new Vector3 (scale, scale, scale);
        int Randint = UnityEngine.Random.Range(0, materials.Count);

        while(Randint == lastindex)
        {
            Randint = UnityEngine.Random.Range(0, materials.Count);
        }
        
        lastindex = Randint;

        for (int i = 0; i < newwall.transform.childCount; i++)
        {
            newwall.transform.GetChild(i).GetComponent<Renderer>().material = materials[Randint];
        }
        newwall.GetComponent<wallscript>().speed = speed;
        newwall.GetComponent<wallscript>().moving = true;

        if(walllist.Count >= 5)
        {
            Destroy(walllist[0]);
            
        }

        walllist.Add(newwall);

    }

    private void generatematerials()
    {
        for(int i = 0; i < materials.Count; i++)
        {
            float randr = UnityEngine.Random.Range(0f, 1f);
            float randg = UnityEngine.Random.Range(0f, 1f);
            float randb = UnityEngine.Random.Range(0f, 1f);

            Material newmat = new Material(materials[i]);
            newmat.SetColor("_Color", new Color(randr, randg, randb,0f));

            materials[i]=newmat;
        }
    }

}
