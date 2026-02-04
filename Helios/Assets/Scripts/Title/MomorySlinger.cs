using System.Collections.Generic;
using System.Runtime.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MomorySlinger : MonoBehaviour
{
    [SerializeField] SlingerMove slingerMove;
    int nowSlinger;
    int nowSinner;
    int maxSinner;//csvから要素数を代入
    [SerializeField] Text[] sinnerNameTexts;
    const int right = 2;
    const int left = 2;
    [SerializeField, Header("シナーメモリー関連")] GameObject explanatoryTextObj;
    List<GameObject> explanatoryList;
    [SerializeField] ScrollRect listScrollRect;
    [SerializeField] GameObject contentsObj;
    RectTransform contentsRect;
    [SerializeField] Text sinnerName;
    [SerializeField] Image secureImage;
    [SerializeField] Image liskImage;
    [SerializeField] Sprite[] secureSprites;
    [SerializeField] Sprite[] liskSprites;
    [SerializeField] Text[] emotions = new Text[8];
    [SerializeField] SinnerMemoryData[] sinnerMemoryDatas;

    private void Awake()
    {
        nowSlinger = 0;
        nowSinner = 0;
        maxSinner = sinnerMemoryDatas.Length;
        //for (int i = 0; i < sinnerNameTexts.Length; i++)
        //{
        //    int num = (i < right) ? i : maxSinner - (sinnerNameTexts.Length - i);
        //    sinnerNameTexts[i].text = sinnerMemoryDatas[num].name;
        //}
        contentsRect = contentsObj.GetComponent<RectTransform>();
    }

    private void Start()
    {
        explanatoryList = new List<GameObject>();
        Set();
    }

    private void LateUpdate()
    {
        InputSlingerMove();
        if (Input.GetKeyDown(KeyCode.Return))
        {
            AudioManager a = Locator<AudioManager>.Instance;
            Debug.Log(a.GetVolume(Audio.BGM));
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = 0; i < sinnerMemoryDatas[nowSinner].explanatoryTexts.Length;i++)
            {
                GameObject obj = Instantiate(explanatoryTextObj, contentsObj.transform);
                obj.GetComponent<DataText>().SetText(sinnerMemoryDatas[nowSinner].explanatoryTexts[i].headingText, sinnerMemoryDatas[nowSinner].explanatoryTexts[i].Text);
                explanatoryList.Add(obj);
            }
            ContentSizeChange();
        }
    }

    void InputSlingerMove()
    {
        if (slingerMove.isMove) return;
        float mouse = Input.mouseScrollDelta.y;
        float axis = Input.GetAxisRaw("Horizontal");
        if (axis != 0 || mouse != 0f)
        {
            mouse /= Mathf.Abs(mouse);
            float f = (mouse != 0f) ? mouse : -axis;
            int a = (f < 0) ? right : left;
            int num = (a * (int)f + nowSlinger + sinnerNameTexts.Length) % sinnerNameTexts.Length;
            int nameNum = (a * (int)f + nowSinner + maxSinner) % maxSinner;
            sinnerNameTexts[num].text = sinnerMemoryDatas[nameNum].name;
            nowSlinger = (nowSlinger + (int)f < 0) ? sinnerNameTexts.Length - 1 : (nowSlinger + (int)f) % sinnerNameTexts.Length;
            nowSinner = (nowSinner + (int)f < 0) ? maxSinner - 1 : (nowSinner + (int)f) % maxSinner;
            StartCoroutine(slingerMove.Move(f));
        }
    }

    void Set()
    {
        ///─────表示情報更新─────///
        sinnerName.text = sinnerMemoryDatas[nowSinner].name;
        secureImage.sprite = secureSprites[(int)sinnerMemoryDatas[nowSinner].secureClass];
        liskImage.sprite = liskSprites[(int)sinnerMemoryDatas[nowSinner].liskClass];
        for (int i = 0; i < sinnerMemoryDatas[nowSinner].emotionValue.Length;i++)
        {
            emotions[i].text = sinnerMemoryDatas[nowSinner].emotionValue[i] + "%";
        }
        for (int i = 0; i < sinnerMemoryDatas[nowSinner].explanatoryTexts.Length;i++)
        {
            GameObject obj = Instantiate(explanatoryTextObj, contentsObj.transform);
            obj.GetComponent<DataText>().SetText(sinnerMemoryDatas[nowSinner].explanatoryTexts[i].headingText, sinnerMemoryDatas[nowSinner].explanatoryTexts[i].Text);
            explanatoryList.Add(obj);
        }
        ContentSizeChange();
    }

    /// <summary>
    /// スクロールビューのコンテンツ部分のサイズを変更する
    /// </summary>
    void ContentSizeChange()
    {
        const float sizeY = 220.0f;
        contentsRect.sizeDelta = new Vector2(contentsRect.sizeDelta.x, sizeY * explanatoryList.Count);
        listScrollRect.verticalNormalizedPosition = 1f;
    }
}
