using UnityEngine;
using UnityEngine.SceneManagement;

public class Musicscript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        Application.targetFrameRate = 60;
        SceneManager.LoadScene("GameScene");

    }
}
