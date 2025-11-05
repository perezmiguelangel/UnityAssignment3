using UnityEngine;
using UnityEngine.SceneManagement;


public class GameController : MonoBehaviour
{
    public static GameController gcInstance;

    //Using custom time to control flow, mainly for pausing 
    public float gameTime { get; set; }
    public int level;
    
    void Awake()
    {
        if (gcInstance == null)
        {
            gcInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        gameTime = 1f;
    }

    void Start()
    {

    }

    void Update()
    {

    }


    //Scene Control
    public void LoadScene(string str)
    {
        SceneManager.LoadScene(str);
    }

    //Clear functions to manage gameTime
    public void pauseGame()
    {
        gameTime = 0f;
    }
    public void resumeGame()
    {
        gameTime = 1f;
    }
}
