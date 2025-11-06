using UnityEngine;
using UnityEngine.SceneManagement;


public class GameController : MonoBehaviour
{
    public static GameController gcInstance;

    //Using custom time to control flow, mainly for pausing 
    public float gameTime { get; set; }
    public string level = "Level1";

    //Audio, vol: 0 -> 1
    public AudioSource audioSource;
    public float volume;
    
    
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
        audioSource.Play();
        SceneManager.LoadScene("MainMenu");
    }

    void Update()
    {

    }


    //Scene Control
    public void LoadScene(string str)
    {
        level = "Level1";
        SceneManager.LoadScene(str);
        
    }
    public void LoadScene()
    {
        Debug.Log("LoadScene level=" + level);
        SceneManager.LoadScene(level);
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

    //Sound
    public void setVolume(float x)
    {
        AudioListener.volume = x;
    }

    public void saveVolume()
    {
        PlayerPrefs.SetFloat("volume", volume);
    }

    public void resetPlayerPref()
    {

    }
    public void savePlayerPref()
    {
        
    }

}
