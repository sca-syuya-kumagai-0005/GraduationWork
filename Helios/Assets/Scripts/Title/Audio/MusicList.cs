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
    [SerializeField] ScrollRect listScrollRect;
    [SerializeField] RectTransform listRectTransform;
    Vector2 defaultSize;
    [SerializeField] Text musicNameText;
    [SerializeField] MusicNameMover musicNameMover;
    [SerializeField] AudioClip[] playList; // 曲のリスト
    Phone phone;

    private void Awake()
    {
        contentsRect = contents.GetComponent<RectTransform>();
        contentsTransform = contents.transform;
        defaultSize = listRectTransform.sizeDelta;
        listRectTransform.sizeDelta = new Vector2(defaultSize.x, 0);
    }

    private void Start()
    {
        musicNameText.text = Locator<AudioManager>.Instance.GetNowBGM().name;
        phone = Locator<Phone>.Instance;
        phone.SetQueue(playList);
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
        if (musics.Count != 0)
        {
            DeleteMusic();
            return;
        }
        this.gameObject.SetActive(true);
        listRectTransform.DOSizeDelta(defaultSize, 0.25f);
        for (int i = (phone.nowNumber + 1) % phone.queue.Length; i != phone.nowNumber; i = (i + 1) % phone.queue.Length)
        {
            InstansMusic(i, phone.queue[i].name);
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
    public void DeleteMusic()
    {
        //ここでボタン系を再稼働
        listRectTransform.DOSizeDelta(new Vector2(defaultSize.x, 0), 0.25f).OnComplete(() =>
        {
            foreach (GameObject obj in musics)
            {
                Destroy(obj);
            }
            musics.Clear();
        });
    }

    /// <summary>
    /// スクロールビューのコンテンツ部分のサイズを変更する
    /// </summary>
    public void ContentSizeChange()
    {
        const float sizeY = 50.0f;
        contentsRect.sizeDelta = new Vector2(contentsRect.sizeDelta.x, sizeY * musics.Count);
        listScrollRect.verticalNormalizedPosition = 1f;
    }

    public void ChangeMusicNameText(string _text)
    {
        musicNameText.text = _text;
        musicNameMover.PositionReset();
    }

    public void SetQueue()
    {
        phone.SetQueue(playList);
    }

    public void RandomQueue()
    {
        phone.SetRandomQueue();
    }
}
