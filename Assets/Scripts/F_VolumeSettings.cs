using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class F_VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider myMusicSlider;
    [SerializeField] private Slider mySFXSlider;

    private void Start()
    {

        if (PlayerPrefs.HasKey("musicVolumen"))
        {
            LoadVolumen();
        }
        else
        {
            SetMusicVolumen();
            SetSfxVolumen();
        }
    }

    public void SetMusicVolumen()
    {
        float volumen = myMusicSlider.value;
        if (volumen == 0)
        {
            audioMixer.SetFloat("Music", -80);
        }
        else
        {
            audioMixer.SetFloat("Music", Mathf.Log10(volumen) * 20);
        }
        PlayerPrefs.SetFloat("musicVolumen", volumen);
    }

    public void SetSfxVolumen()
    {

        float volumen = mySFXSlider.value;
        if (volumen == 0)
        {
            audioMixer.SetFloat("Sonido", -80);
        }
        else
        {
            audioMixer.SetFloat("Sonido", Mathf.Log10(volumen) * 20);
        }
        PlayerPrefs.SetFloat("sonidoVolumen", volumen);
    }

    private void LoadVolumen()
    {
        myMusicSlider.value = PlayerPrefs.GetFloat("musicVolumen");
        SetMusicVolumen();
        mySFXSlider.value = PlayerPrefs.GetFloat("sonidoVolumen");
        SetSfxVolumen();

    }
}
