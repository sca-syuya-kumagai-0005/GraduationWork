using System;
using System.Linq;
using UnityEngine;

public class MusicQueue : MonoBehaviour
{
    public AudioClip[] queue {  get; private set; }
    public int nowNumber {  get; private set; }

    private void OnEnable()
    {
        Locator<MusicQueue>.Bind(this);
    }

    private void OnDisable()
    {
        Locator<MusicQueue>.Unbind(this);
    }

    public void SetQueue(AudioClip[] _clips)
    {
        AudioClip clip = _clips[nowNumber];
        queue = _clips;
        for (int i = 0;i < queue.Length;i++)
        {
            if (queue[i] == clip)
            {
                nowNumber = i;
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
        Locator<MusicList>.Instance.ChangeMusicNameText(queue[nowNumber].name);
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
