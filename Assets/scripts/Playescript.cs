using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Playescript : MonoBehaviour
{

    public PlayerInput PlayerInput;

    public InputActionReference movement;
    public InputActionReference start;

    public int speedmode;

    public int level;
    

    public float xlimit;
    public float ylimit;

    private Vector2 movementinput;

    private Vector3 nextpos;

    public float multiplicateur;

    private int spawncdcntr;
    public float timebetweenspawn;

    public GameObject EnemyCube;

    public GameObject BombCube;

    public GameObject LifeCube;

    public GameObject ShieldCube;

    public GameObject wallprefab;

    public int score;

    public int lives = 3;

    private int lastCD;

    public TextMeshProUGUI scoretxt;

    public TextMeshProUGUI livestxt;

    public TextMeshProUGUI leveltxt;

    public int generation;

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

    private int lastwall;

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
            if(level<=35)
            {
                MoveWall(level, getmultiplicator());
            }
            else
            {
                MoveWall(level, 35 / 1.8f + 2.2f);
            }
            

        }


        scoretxt.text = "Score : " + score;

        livestxt.text = lives + " lives";

        leveltxt.text = "Level : " + (level-1);

        ManageMovement();

        if (spawncdcntr==0 && lives>0)
        {
            if(level<10)
            {
                generation+=2;
            }
            else
            {
                generation++;
            }
            SpawnEnemywave(level);
            spawncdcntr = (int)(timebetweenspawn/(Time.deltaTime));
            if(level>2 && level<=5)
            {
                spawncdcntr = (int)(spawncdcntr / level) * 2;
            }
            else if(level>7)
            {
                spawncdcntr = (int)(spawncdcntr / 7) * 2;
            }

        }
        else
        {
            spawncdcntr--;
        }

        level = (int)(generation / 9) + 2;

    }

    public void StartIFrame(float time)
    {
        blinking =(int)(time/Time.deltaTime);
    }

    private float getmultiplicator()
    {
        float mult = 0f;

        if (level <= 3)
        {
            mult = 1f;
        }
        else if (level <= 5)
        {
            mult = 1.1f;
        }
        else if (level <= 6)
        {
            mult = 1.2f;
        }
        else if (level >= 7 && level < 15)
        {
            mult = 1.3f;
        }
        else if (level >= 15 && level < 25)
        {
            mult = 1.4f;
        }
        else if (level >= 25 && level < 35)
        {
            mult = 1.5f;
        }
        else
        {
            mult = level/70 + 1.1f;
        }

            return mult;
    }

    private void SpawnEnemywave(int level)
    {
        if (level <= 2)
        {
            GameObject newcube = SpawnACube();


            if (level==2)
            {
                newcube.GetComponent<Enemyscript>().multiplier = getmultiplicator();
            }
            
        }

        else if(level==3 || level==4)
        {
            int rdint = UnityEngine.Random.Range(0, 2);
            if(rdint == 0)
            {
                GameObject newcube = SpawnACube();
                newcube.GetComponent<Enemyscript>().multiplier = getmultiplicator();
            }
            else
            {
                List<GameObject> newcubelist = SpawnALineOf2();
                newcubelist[0].GetComponent<Enemyscript>().multiplier = getmultiplicator();
                newcubelist[1].GetComponent<Enemyscript>().multiplier = getmultiplicator();
            }

        }

        else if( level == 5 || level == 6)
        {
            int rdint = UnityEngine.Random.Range(0, 3);
            if (rdint == 0)
            {
                GameObject newcube = SpawnACube();
                newcube.GetComponent<Enemyscript>().multiplier = getmultiplicator();
                
            }
            else if (rdint == 1)
            {
                List<GameObject> newcubelist = SpawnALineOf2();
                newcubelist[0].GetComponent<Enemyscript>().multiplier = getmultiplicator();
                newcubelist[1].GetComponent<Enemyscript>().multiplier = getmultiplicator();
            }
            else if (rdint == 2)
            {
                List<GameObject> newcubelist = SpawnFullLine();
                newcubelist[0].GetComponent<Enemyscript>().multiplier = getmultiplicator();
                newcubelist[1].GetComponent<Enemyscript>().multiplier = getmultiplicator();
                newcubelist[2].GetComponent<Enemyscript>().multiplier = getmultiplicator();
            }
        }

        else if( level >=7 && level<15)
        {

            int rdint = UnityEngine.Random.Range(0, 3);
            if (rdint == 0)
            {
                GameObject newcube = SpawnACube();
                newcube.GetComponent<Enemyscript>().multiplier = getmultiplicator();

            }
            else if (rdint == 1)
            {
                List<GameObject> newcubelist = SpawnALineOf2();
                newcubelist[0].GetComponent<Enemyscript>().multiplier = getmultiplicator();
                newcubelist[1].GetComponent<Enemyscript>().multiplier = getmultiplicator();
            }
            else if (rdint == 2)
            {
                List<GameObject> newcubelist = SpawnFullLine();
                newcubelist[0].GetComponent<Enemyscript>().multiplier = getmultiplicator();
                newcubelist[1].GetComponent<Enemyscript>().multiplier = getmultiplicator();
                newcubelist[2].GetComponent<Enemyscript>().multiplier = getmultiplicator();
            }
        }

        else if (level >= 15)
        {
           

            int rdint = UnityEngine.Random.Range(0, 6);
            if (rdint == 0)
            {
                GameObject newcube = SpawnACube();
                newcube.GetComponent<Enemyscript>().multiplier = getmultiplicator();
                
            }
            else if (rdint == 1)
            {
                List<GameObject> newcubelist = SpawnALineOf2();
                newcubelist[0].GetComponent<Enemyscript>().multiplier = getmultiplicator();
                newcubelist[1].GetComponent<Enemyscript>().multiplier = getmultiplicator();
            }
            else if (rdint == 2)
            {
                List<GameObject> newcubelist = SpawnFullLine();
                newcubelist[0].GetComponent<Enemyscript>().multiplier = getmultiplicator();
                newcubelist[1].GetComponent<Enemyscript>().multiplier = getmultiplicator();
                newcubelist[2].GetComponent<Enemyscript>().multiplier = getmultiplicator();
            }
            else
            {
                List<GameObject> newcubelist = SpawnMultiple(4);
                

                for (int i = 0; i < newcubelist.Count; i++)
                {
                    newcubelist[i].GetComponent<Enemyscript>().multiplier = getmultiplicator();
                }
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

        rdint = UnityEngine.Random.Range(0, 150);
        GameObject newcube=null;
        if (rdint==33)
        {
            newcube = Instantiate(LifeCube, newposition, Quaternion.identity);
        }
        else if(rdint==50)
        {
            newcube = Instantiate(BombCube, newposition, Quaternion.identity);
        }
        else if(rdint == 66)
        {
            newcube = Instantiate(ShieldCube, newposition, Quaternion.identity);
        }
        else
        {
            newcube = Instantiate(EnemyCube, newposition, Quaternion.identity);

        }

        newcube.transform.position = newposition;
        newcube.transform.SetParent(GameObject.Find("Enemies").transform);
        return newcube;
    }
    private List<GameObject> SpawnMultiple(int number)
    {

        List<GameObject> list = new List<GameObject>();
        int rdint = UnityEngine.Random.Range(0, 4);

        Vector3 blacklistedpos = Vector3.zero;

        if(rdint==0)
        {
            blacklistedpos = new Vector3(-0.6f, -0.6f, basez);
        }
        else if (rdint == 1)
        {
            blacklistedpos = new Vector3(-0.6f, 0.6f, basez);
        }
        else if (rdint == 2)
        {
            blacklistedpos = new Vector3(0.6f, -0.6f, basez);
        }
        else
        {
            blacklistedpos = new Vector3(0.6f, 0.6f, basez);
        }


        while (list.Count <= number)
        {
            GameObject newcube = SpawnACube();

            bool different = true;

            blacklistedpos = new Vector3(blacklistedpos.x, blacklistedpos.y, newcube.transform.position.z);

            if (different && Vector3.Distance(blacklistedpos, newcube.transform.position)>=0.05f)
            {
                list.Add(newcube);
            }
            else
            {
                Destroy(newcube);
            }

            
        }

        return list;

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
        newcube.transform.SetParent(GameObject.Find("Enemies").transform);

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
        othercube.transform.SetParent(GameObject.Find("Enemies").transform);


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

            obj1.transform.SetParent(GameObject.Find("Enemies").transform);
            obj2.transform.SetParent(GameObject.Find("Enemies").transform);
            obj3.transform.SetParent(GameObject.Find("Enemies").transform);

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

            obj1.transform.SetParent(GameObject.Find("Enemies").transform);
            obj2.transform.SetParent(GameObject.Find("Enemies").transform);
            obj3.transform.SetParent(GameObject.Find("Enemies").transform);

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

            obj1.transform.SetParent(GameObject.Find("Enemies").transform);
            obj2.transform.SetParent(GameObject.Find("Enemies").transform);
            obj3.transform.SetParent(GameObject.Find("Enemies").transform);

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

            obj1.transform.SetParent(GameObject.Find("Enemies").transform);
            obj2.transform.SetParent(GameObject.Find("Enemies").transform);
            obj3.transform.SetParent(GameObject.Find("Enemies").transform);

            list.Add(obj1);
            list.Add(obj2);
            list.Add(obj3);

        }
        
        return list;
    }

    private void MoveWall(int level, float speed)
    {
        lastwall++;
        float scale = 0.9f;
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
        newwall.GetComponent<wallscript>().ID = lastwall;

        if (walllist.Count >= 5)
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
            newmat.SetColor("_Color", new UnityEngine.Color(randr, randg, randb,0f));

            materials[i]=newmat;
        }
    }

}
