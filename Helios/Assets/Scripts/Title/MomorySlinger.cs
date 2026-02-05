using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MomorySlinger : MonoBehaviour
{
    [SerializeField] SaveDataManager saveDataManager;
    bool[] memory;
    [SerializeField] SlingerMove slingerMove;
    int nowSlinger;
    int nowSinner;
    int maxSinner;
    [SerializeField] Text[] sinnerNameTexts;
    const int right = 2;
    const int left = 2;
    [SerializeField, Header("シナーメモリー関連")] GameObject explanatoryTextObj;
    List<GameObject> explanatoryList;
    [SerializeField] ScrollRect listScrollRect;
    [SerializeField] GameObject contentsObj;
    RectTransform contentsRect;
    [SerializeField] GameObject conditionTextObj;
    List<GameObject> conditionList;
    [SerializeField] GameObject conditionObj;
    [SerializeField] Text sinnerName;
    [SerializeField] Image sinnerImage;
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
        for (int i = 0; i < sinnerNameTexts.Length; i++)
        {
            int num = (i < right) ? i : maxSinner - (sinnerNameTexts.Length - i);
            sinnerNameTexts[i].text = sinnerMemoryDatas[num].name;
        }
        contentsRect = contentsObj.GetComponent<RectTransform>();
        memory = new bool[maxSinner];
        for (int i = 0; i < maxSinner; i++)
        {
            memory[i] = saveDataManager.Memory[i];
            if (i == maxSinner - 1) memory[i] = saveDataManager.Memory[saveDataManager.Memory.Length - 1];
        }
    }

    private void Start()
    {
        explanatoryList = new List<GameObject>();
        conditionList = new List<GameObject>();
        Set();
    }

    private void Update()
    {
        InputSlingerMove();
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
            StartCoroutine(Anim((int)f));
        }
    }

    IEnumerator Anim(float _dir)
    {
        yield return StartCoroutine(slingerMove.Move(_dir));
        Set();
    }

    void Set()
    {
        DeliteList();
        ///─────表示情報更新─────///
        //名前
        sinnerName.text = sinnerMemoryDatas[nowSinner].name;
        //クラス
        secureImage.sprite = secureSprites[(int)sinnerMemoryDatas[nowSinner].secureClass];
        liskImage.sprite = liskSprites[(int)sinnerMemoryDatas[nowSinner].liskClass];
        //シナー画像
        sinnerImage.sprite = sinnerMemoryDatas[nowSinner].sinnerImage;
        //配達確率
        for (int i = 0; i < sinnerMemoryDatas[nowSinner].emotionValue.Length; i++)
        {
            emotions[i].text = sinnerMemoryDatas[nowSinner].emotionValue[i] + "%";
        }
        //説明文
        for (int i = 0; i < sinnerMemoryDatas[nowSinner].explanatoryTexts.Length; i++)
        {
            if (sinnerMemoryDatas[nowSinner].explanatoryTexts[i].isMemory && !memory[i]) continue;
            GameObject obj = Instantiate(explanatoryTextObj, contentsObj.transform);
            obj.GetComponent<DataText>().SetText(sinnerMemoryDatas[nowSinner].explanatoryTexts[i].headingText, sinnerMemoryDatas[nowSinner].explanatoryTexts[i].Text);
            explanatoryList.Add(obj);
        }
        //スクロール更新
        ContentSizeChange();
        //条件文
        for (int i = 0; i < sinnerMemoryDatas[nowSinner].conditionalTexts.Length; i++)
        {
            GameObject obj = Instantiate(conditionTextObj, conditionObj.transform);
            string text;
            if (sinnerMemoryDatas[nowSinner].conditionalTexts[i].isMemory)
            {
                text = (!memory[i]) ? sinnerMemoryDatas[nowSinner].conditionalTexts[i].Text : sinnerMemoryDatas[nowSinner].conditionalTexts[i].housedText;
            }
            else
            {
                text = sinnerMemoryDatas[nowSinner].conditionalTexts[i].Text;
            }
            obj.GetComponent<ConditionText>().SetText(text);
            conditionList.Add(obj);
        }
    }

    void DeliteList()
    {
        foreach (GameObject obj in explanatoryList) Destroy(obj);
        explanatoryList.Clear();

        foreach (GameObject obj in conditionList) Destroy(obj);
        conditionList.Clear();
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
