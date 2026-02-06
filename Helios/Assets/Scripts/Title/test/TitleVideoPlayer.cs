using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Video;

public class TitleVideoPlayer : MonoBehaviour
{
    [SerializeField] VideoPlayer player;
    [SerializeField] AudioSource audioSource;
    private void Start()
    {
        player.loopPointReached += LoopPointReached;
    }

    void LoopPointReached(VideoPlayer _vp)
    {
        Locator<TitleManager>.Instance.BackTitleSelect();
    }

    [Button]
    public void PlayVideo()
    {
        audioSource.volume = Locator<AudioManager>.Instance.GetVolume(Audio.MASTER);
        player.Play();
    }
    [Button]
    public void StopVideo()
    {
        player.Stop();
    }
}
