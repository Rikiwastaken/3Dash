using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttons : MonoBehaviour
{

    private Transform musicHolder;

    private void Start()
    {
        musicHolder = Musicscript.instance.transform;
        for(int i = 0; i < musicHolder.childCount; i++)
        {
            if(Musicscript.instance.currentmusicindex!=i)
            {
                musicHolder.GetChild(i).GetComponent<AudioSource>().mute = true;
            }
            else
            {
                musicHolder.GetChild(i).GetComponent<AudioSource>().mute = false;
            }
        }
    }

    public void Onquit()
    {
        Application.Quit();
    }

    public void OnRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ChangeMusic()
    {
        musicHolder.GetChild(Musicscript.instance.currentmusicindex).GetComponent<AudioSource>().mute = true;

        Musicscript.instance.currentmusicindex++;
        if(Musicscript.instance.currentmusicindex >= musicHolder.childCount)
        {
            Musicscript.instance.currentmusicindex = 0;
        }

        musicHolder.GetChild(Musicscript.instance.currentmusicindex).GetComponent<AudioSource>().mute = false;
    }

    public void Mute()
    {
        if (GameObject.Find("Music1").GetComponent<AudioSource>().volume == 1f)
        {
            GameObject.Find("Music1").GetComponent<AudioSource>().volume = 0f;
            GameObject.Find("Music2").GetComponent<AudioSource>().volume = 0f;
        }
        else
        {
            GameObject.Find("Music1").GetComponent<AudioSource>().volume = 1f;
            GameObject.Find("Music2").GetComponent<AudioSource>().volume = 1f;
        }
    }

    public void Pause()
    {
        if(Time.timeScale > 0f)
        {

        Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
}
