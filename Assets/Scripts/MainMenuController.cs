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
        GameController.gcInstance.LoadScene();
    }
    public void onLoadClicked()
    {
        GameController.gcInstance.loadPlayerPref();
    }
    public void onSettingsClicked()
    {
        mainCanvas.gameObject.SetActive(false);
        settingsCanvas.gameObject.SetActive(true);
    }
    public void onExitClicked()
    {
        Application.Quit();
    }

    public void onVolumeChange(float x)
    {
        GameController.gcInstance.setVolume(x);
        Debug.Log("vol:" + x);
    }
    public void onSoundResetClicked()
    {
        GameController.gcInstance.setVolume(0.5f);
        GameController.gcInstance.saveVolume();
    }
    public void onResetClicked()
    {
        GameController.gcInstance.resetPlayerPref();
    }
    public void onSaveClicked()
    {
        GameController.gcInstance.savePlayerPref();
    }
    public void onBackClicked()
    {
        settingsCanvas.gameObject.SetActive(false);
        mainCanvas.gameObject.SetActive(true);
    }


}
