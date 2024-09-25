using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class Slomo : MonoBehaviour
{
    public int slomoduration;

    public int slomocounter;

    public InputActionReference slowmo;
    public bool slowmopressed;

    public float mintimescale;

    public Slider slider;

    public int zerocd;

    void Start()
    {
        slomocounter = slomoduration;
        slowmo.action.performed += OnSlowmoPress;
        slider.maxValue = slomoduration;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        slider.value = slomocounter;
        if (!slowmo.action.IsPressed())
        {
            slowmopressed = false;
        }


        if(slowmopressed && slomocounter>0)
        {


            if(Time.timeScale>mintimescale)
            {
                Time.timeScale -= 0.01f;
                Time.fixedDeltaTime = 0.02F * Time.timeScale;
            }
            slomocounter--;
        }
        else
        {
            if (Time.timeScale < 1f)
            {
                Time.timeScale += 0.02f;
                Time.fixedDeltaTime = 0.02F * Time.timeScale;
            }
            if(Time.timeScale>1f)
            {
                Time.timeScale = 1f;
                Time.fixedDeltaTime = 0.02F * Time.timeScale;
            }
        }
        if(slomocounter==0 && zerocd==0)
        {
            zerocd = 100;
        }

        if(!slowmopressed && slomocounter<slomoduration)
        {
            if(zerocd>0)
            {
                zerocd--;
            }
            if(zerocd==0)
            {
                slomocounter++;
            }
            
        }


    }

    public void OnSlowmoPress(InputAction.CallbackContext context)
    {
        slowmopressed = true;
        int rdint = Random.Range(0, 30);
        if (rdint == 20)
        {
            if (GameObject.Find("Music"))
            {
                GameObject.Find("Music").GetComponent<Musicscript>().timefordistrotion = 200;
            }

        }
    }
}
