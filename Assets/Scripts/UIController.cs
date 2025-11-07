using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject worldTextCanvas;
    public TextMeshProUGUI movement;
    public TextMeshProUGUI jump;
    public TextMeshProUGUI health;
    public PlayerController playerController;
    public GameObject pauseCanvas;
    public Button Reset;
    public Button SaveExit;
    public AudioController audioController;

    void Start()
    {
        Reset.onClick.AddListener(onResetClicked);
        SaveExit.onClick.AddListener(onExitClicked);
        pauseCanvas.SetActive(false);
    }
    public void onResetClicked()
    {
        //Loads saved game level
        audioController.playClip("button");
        GameController.gcInstance.LoadScene();
    }
    public void onExitClicked()
    {
        audioController.playClip("button");
        GameController.gcInstance.savePlayerPref();
        GameController.gcInstance.LoadScene("MainMenu");
    }

    void Awake()
    {
        jump.gameObject.SetActive(false);
    }

    public void SetActive(bool x, string y)
    {
        if (y == "MoveText")
        {
            movement.gameObject.SetActive(x);
        }
        else if (y == "JumpText")
        {
            jump.gameObject.SetActive(x);
        }
    }

    public void pause()
    {
        GameController.gcInstance.pauseGame();
        worldTextCanvas.SetActive(false);
        pauseCanvas.SetActive(true);
    }
    public void resume()
    {
        GameController.gcInstance.resumeGame();
        worldTextCanvas.SetActive(true);
        pauseCanvas.SetActive(false);
    }

    void Update()
    {
        health.text = "Health: " + playerController.playerHealth;
    }
}
