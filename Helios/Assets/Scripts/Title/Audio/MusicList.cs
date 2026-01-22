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
    [SerializeField] MusicNameMover musicNameMover;
    [SerializeField] AudioClip[] playList; // 曲のリスト
    MusicQueue musicQueue;

    private void Awake()
    {
        contentsRect = contents.GetComponent<RectTransform>();
        contentsTransform = contents.transform;
        myScrollRect = GetComponent<ScrollRect>();
        myRectTransform = GetComponent<RectTransform>();
        defaultSize = myRectTransform.sizeDelta;
        myRectTransform.sizeDelta = new Vector2(defaultSize.x, 0);
        musicQueue = Locator<MusicQueue>.Instance;
        musicQueue.SetQueue(playList);
    }

    private void Start()
    {
        musicNameText.text = Locator<AudioManager>.Instance.GetNowBGM().name;
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

    public void instans()
    {
        //ここでボタン系をストップ
        this.gameObject.SetActive(true);
        myRectTransform.DOSizeDelta(defaultSize, 0.25f);
        for (int i = (musicQueue.nowNumber + 1) % musicQueue.queue.Length; i != musicQueue.nowNumber; i = (i + 1) % musicQueue.queue.Length)
        {
            InstansMusic(i, musicQueue.queue[i].name);
        }
        ContentSizeChange();
    }

    //本実装関数
    public void InstansMusic(int _num, string _audioClipName)
    {
        GameObject obj = Instantiate(musicPrefab, contentsTransform);
        obj.GetComponent<MusicButton>().SetAudioCiip(_num, _audioClipName);
        obj.name = _audioClipName;
        musics.Add(obj);
    }

    /// <summary>
    /// 表示中のリストを削除する
    /// </summary>
    public void DeleteMusic(string _text)
    {
        //ここでボタン系を再稼働
        musicNameText.text = _text;
        musicNameMover.PositionReset();
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

    public void ChangeMusicNameText(string _text)
    {
        musicNameText.text = _text;
    }

    public void SetQueue()
    {
        musicQueue.SetQueue(playList);
    }

    public void RandomQueue()
    {
        musicQueue.SetRandomQueue();
    }
}
