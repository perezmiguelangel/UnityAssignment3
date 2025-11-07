using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController audioInstance;
    public float volume = 0.5f;

    public AudioSource musicSource;
    public AudioSource effectsSource;


    public AudioClip walkClip;
    public AudioClip damageClip;
    public AudioClip uiClip;
    public AudioClip swordClip;
    public AudioClip buttonClip;
    public AudioClip playerDeathClip;
    public AudioClip bossDeathClip;



    void Awake()
    {
        if (audioInstance == null)
        {
            audioInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }
    
    public void playClip(string s)
    {
        if (s == "save")
        {
            effectsSource.PlayOneShot(uiClip);
        }
        else if (s == "walk")
        {
            effectsSource.PlayOneShot(walkClip);
        }
        else if (s == "damage")
        {
            effectsSource.PlayOneShot(damageClip);
        }
        else if (s == "sword")
        {
            effectsSource.PlayOneShot(swordClip);
        }
        else if (s == "button")
        {
            effectsSource.PlayOneShot(buttonClip);
        }
        else if (s == "playerDeath")
        {
            effectsSource.PlayOneShot(playerDeathClip);
        }
        else if(s == "bossDeath")
        {
            effectsSource.PlayOneShot(bossDeathClip);
        }
    }

    public void SetVolume(float x)
    {
        volume = x;
    }


}
