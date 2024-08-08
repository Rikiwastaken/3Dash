using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttons : MonoBehaviour
{
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
        if(GameObject.Find("Music1").GetComponent<AudioSource>().mute==true)
        {
            GameObject.Find("Music1").GetComponent<AudioSource>().mute = false;
            GameObject.Find("Music2").GetComponent<AudioSource>().mute = true;
        }
        else
        {
            GameObject.Find("Music1").GetComponent<AudioSource>().mute = true;
            GameObject.Find("Music2").GetComponent<AudioSource>().mute = false;
        }
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
}
