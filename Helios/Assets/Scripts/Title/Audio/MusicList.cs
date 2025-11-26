using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicList : MonoBehaviour
{
    [SerializeField] GameObject contents;
    RectTransform contentsRect;
    Transform contentsTransform;
    [SerializeField] GameObject musicPrefab;
    List<GameObject> musics = new List<GameObject>();
    ScrollRect myScrollRect;

    private void Awake()
    {
        contentsRect = contents.GetComponent<RectTransform>();
        contentsTransform = contents.transform;
        myScrollRect = GetComponent<ScrollRect>();
    }

    private void OnEnable()
    {
        Locator<MusicList>.Bind(this);
    }

    private void OnDisable()
    {
        Locator<MusicList>.Unbind(this);
    }

    //public void InstansMusic(AudioClip _audioClip)
    //{
    //    GameObject obj = Instantiate(musicPrefab, viewPortTransform);
    //    obj.GetComponent<MusicButton>().SetAudioCiip(_audioClip);
    //    obj.name = _audioClip.name;
    //    musics.Add(obj);
    //}

    public void instans()
    {
        for (int i = 0; i < 15;i++)
        {
            InstansMusic(i.ToString());
        }
        ContentSizeChange();
    }

    public void InstansMusic(string _audioClip)
    {
        GameObject obj = Instantiate(musicPrefab, contentsTransform);
        obj.GetComponent<MusicButton>().SetAudioCiip(_audioClip);
        obj.name = _audioClip;
        musics.Add(obj);
    }

    /// <summary>
    /// 表示中のリストを削除する
    /// </summary>
    public void DeleteMusic()
    {
        foreach (GameObject obj in musics)
        {
            Destroy(obj);
        }
        musics.Clear();
    }

    /// <summary>
    /// スクロールビューのコンテンツ部分のサイズを変更する
    /// </summary>
    public void ContentSizeChange()
    {
        const float sizeY = 50.0f;
        contentsRect.sizeDelta = new Vector2(contentsRect.sizeDelta.x, sizeY * musics.Count);
        myScrollRect.verticalNormalizedPosition = 1f;
    }
}
