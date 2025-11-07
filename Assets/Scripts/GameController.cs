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
    public bool isPaused;
    public Vector3 playerPosition;
    
    
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
        isPaused = false;
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
        isPaused = true;
    }
    public void resumeGame()
    {
        gameTime = 1f;
        isPaused = false;
    }

    //Sound
    public void setVolume(float x)
    {
        volume = x;
        AudioListener.volume = x;
    }

    public void saveVolume()
    {
        PlayerPrefs.SetFloat("volume", volume);
    }

    public void resetPlayerPref()
    {
        volume = 0.5f;
        setVolume(volume);
        playerPosition = new Vector3(-8, -3, 0);
        savePlayerPref();
    }
    public void savePlayerPref()
    {
        PlayerPrefs.SetFloat("Volume", volume);
        PlayerPrefs.SetFloat("PosX", playerPosition.x);
        PlayerPrefs.SetFloat("PosY", playerPosition.y);
        PlayerPrefs.Save();
    }
    public void loadPlayerPref()
    {
        volume = PlayerPrefs.GetFloat("Volume");
        playerPosition.x = PlayerPrefs.GetFloat("PosX");
        playerPosition.y = PlayerPrefs.GetFloat("PosY");
        setVolume(volume);
    }

}
