using UnityEngine;
using UnityEngine.UI;

public class MusicButton : MonoBehaviour
{
    private Button button;
    int num;
    [SerializeField] Text musicNameText;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(playMusic);
    }

    public void SetAudioCiip(int _num ,string _audioClipName)
    {
        num = _num;
        musicNameText.text = _audioClipName;
    }

    public void playMusic()
    {
        Locator<MusicList>.Instance.DeleteMusic(musicNameText.text);
        Locator<MusicQueue>.Instance.ChangeBGM(num);
    }
}
