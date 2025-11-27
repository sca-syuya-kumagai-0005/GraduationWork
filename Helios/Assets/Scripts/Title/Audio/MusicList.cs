using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MusicList : MonoBehaviour
{
    [SerializeField] GameObject contents;
    RectTransform contentsRect;
    Transform contentsTransform;
    [SerializeField] GameObject musicPrefab;
    List<GameObject> musics = new List<GameObject>();
    ScrollRect myScrollRect;
    RectTransform myRectTransform;
    Vector2 defaultSize;
    [SerializeField] Text musicNameText;

    private void Awake()
    {
        contentsRect = contents.GetComponent<RectTransform>();
        contentsTransform = contents.transform;
        myScrollRect = GetComponent<ScrollRect>();
        myRectTransform = GetComponent<RectTransform>();
        defaultSize = myRectTransform.sizeDelta;
        myRectTransform.sizeDelta = new Vector2(defaultSize.x, 0);
        this.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        Locator<MusicList>.Bind(this);
    }

    private void OnDisable()
    {
        Locator<MusicList>.Unbind(this);
    }

    //本実装関数
    //public void InstansMusic(AudioClip _audioClip)
    //{
    //    GameObject obj = Instantiate(musicPrefab, viewPortTransform);
    //    obj.GetComponent<MusicButton>().SetAudioCiip(_audioClip);
    //    obj.name = _audioClip.name;
    //    musics.Add(obj);
    //}

    public void instans()
    {
        this.gameObject.SetActive(true);
        myRectTransform.DOSizeDelta(defaultSize, 0.25f);
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
    public void DeleteMusic(string _text)
    {
        musicNameText.text = _text;
        myRectTransform.DOSizeDelta(new Vector2(defaultSize.x, 0), 0.25f).OnComplete(() =>
        {
            foreach (GameObject obj in musics)
            {
                Destroy(obj);
            }
            musics.Clear();
            this.gameObject.SetActive(false);
        });
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
