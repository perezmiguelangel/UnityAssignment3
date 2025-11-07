using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    //Main
    public Canvas mainCanvas;
    public Button startButton;
    public Button loadButton;
    public Button settingsButton;
    public Button exitButton;

    //Settings
    public Canvas settingsCanvas;
    public Slider soundSlider;
    public Button soundResetButtton;
    public Button resetButton;
    public Button saveButton;
    public Button backButton;

    


    void Start()
    {
        settingsCanvas.gameObject.SetActive(false);
        startButton.onClick.AddListener(onStartClicked);
        loadButton.onClick.AddListener(onLoadClicked);
        settingsButton.onClick.AddListener(onSettingsClicked);
        exitButton.onClick.AddListener(onExitClicked);

        soundSlider.onValueChanged.AddListener(onVolumeChange);
        soundResetButtton.onClick.AddListener(onSoundResetClicked);
        resetButton.onClick.AddListener(onResetClicked);
        saveButton.onClick.AddListener(onSaveClicked);
        backButton.onClick.AddListener(onBackClicked);

    }

    public void onStartClicked()
    {
        //Loads saved game level
        AudioController.audioInstance.playClip("button");
        GameController.gcInstance.LoadScene();
    }
    public void onLoadClicked()
    {
        AudioController.audioInstance.playClip("button");
        GameController.gcInstance.loadPlayerPref();
    }
    public void onSettingsClicked()
    {
        AudioController.audioInstance.playClip("button");
        mainCanvas.gameObject.SetActive(false);
        settingsCanvas.gameObject.SetActive(true);
    }
    public void onExitClicked()
    {
        AudioController.audioInstance.playClip("button");
        Application.Quit();
    }

    public void onVolumeChange(float x)
    {
        AudioController.audioInstance.playClip("button");
        GameController.gcInstance.setVolume(x);
        Debug.Log("vol:" + x);
    }
    public void onSoundResetClicked()
    {
        AudioController.audioInstance.playClip("button");
        GameController.gcInstance.setVolume(0.5f);
        GameController.gcInstance.saveVolume();
    }
    public void onResetClicked()
    {
        AudioController.audioInstance.playClip("button");
        GameController.gcInstance.resetPlayerPref();
    }
    public void onSaveClicked()
    {
        AudioController.audioInstance.playClip("button");
        GameController.gcInstance.savePlayerPref();
    }
    public void onBackClicked()
    {
        AudioController.audioInstance.playClip("button");
        settingsCanvas.gameObject.SetActive(false);
        mainCanvas.gameObject.SetActive(true);
    }


}
