using UnityEngine;
using UnityEngine.UI;

public class Phone : MonoBehaviour
{
    AudioManager audioManager;
    [SerializeField] Slider musicBar;
    [SerializeField] MusicList musicList;
    public AudioClip[] queue;
    public int nowNumber { get; private set; }

    private void Start()
    {
        audioManager = Locator<AudioManager>.Instance;
    }

    private void OnEnable()
    {
        Locator<Phone>.Bind(this);
    }

    private void OnDisable()
    {
        Locator<Phone>.Unbind(this);
    }

    private void LateUpdate()
    {
        musicBar.value = audioManager.bgmAudio.time / audioManager.bgmAudio.clip.length;
    }

    public void SetQueue(AudioClip[] _clips)
    {
        AudioClip clip = Locator<AudioManager>.Instance.GetNowBGM();
        queue = new AudioClip[_clips.Length];
        _clips.CopyTo(queue, 0);
        for (int i = 0; i < queue.Length; i++)
        {
            if (queue[i] == clip)
            {
                nowNumber = i;
                Debug.Log(i);
                break;
            }
        }
    }

    public void SetRandomQueue()
    {
        AudioClip _clip = queue[0];
        queue[0] = queue[nowNumber];
        queue[nowNumber] = _clip;
        nowNumber = 0;
        for (int i = queue.Length - 1; i > 1; i--)
        {
            int randomIndex = UnityEngine.Random.Range(1, i + 1);
            AudioClip temp = queue[i];
            queue[i] = queue[randomIndex];
            queue[randomIndex] = temp;
        }
    }

    public void ChangeBGM(int _num)
    {
        musicList.ChangeMusicNameText(queue[_num].name);
        Locator<AudioManager>.Instance.PlayBGM(queue[_num]);
    }

    public void NextMusic()
    {
        nowNumber = (nowNumber + 1) % queue.Length;
        ChangeBGM(nowNumber);
    }

    public void BackMusic()
    {
        nowNumber = (nowNumber - 1 < 0) ? queue.Length - 1 : (nowNumber - 1) % queue.Length;
        ChangeBGM(nowNumber);
    }
}
