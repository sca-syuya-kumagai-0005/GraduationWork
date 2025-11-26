using UnityEngine;
using UnityEngine.UI;

public class MusicButton : MonoBehaviour
{
    private Button button;
    AudioClip audioClip;
    [SerializeField] Text musicNameText;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(playMusic);
    }

    //public void SetAudioCiip(AudioClip _audioClip)
    //{
    //    audioClip = _audioClip;
    //    musicNameText.text = _audioClip.name;
    //}

    public void SetAudioCiip(string _s)
    {
        musicNameText.text = _s;
    }

    public void playMusic()
    {
        Locator<MusicList>.Instance.DeleteMusic();
        //Locator<AudioManager>.Instance.PlayBGM(audioClip);
    }
}
