using UnityEngine;
using UnityEngine.SceneManagement;

public class Musicscript : MonoBehaviour
{

    public int timefordistrotion;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        Application.targetFrameRate = 60;
        SceneManager.LoadScene("GameScene");

    }

    private void FixedUpdate()
    {
        if(Application.targetFrameRate!=60)
        {
            Application.targetFrameRate = 60;
        }
        if(timefordistrotion >0)
        {
            timefordistrotion--;
            foreach (Transform t in transform)
            {
                t.GetComponent<AudioSource>().pitch = 0.9f + Time.timeScale / 10;
            }
        }
        else
        {
            foreach (Transform t in transform)
            {
                t.GetComponent<AudioSource>().pitch = 1f;
            }
        }
        
    }
}
