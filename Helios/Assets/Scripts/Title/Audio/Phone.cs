using UnityEngine;
using UnityEngine.UI;

public class Phone : MonoBehaviour
{
    AudioManager audioManager;
    [SerializeField] Slider musicBar;
    [SerializeField] MusicList musicList;

    private void Start()
    {
        audioManager = Locator<AudioManager>.Instance;
    }

    private void LateUpdate()
    {
        musicBar.value = audioManager.bgmAudio.time / audioManager.bgmAudio.clip.length;
    }

    public void RepeatMusic()
    {

    }
}
