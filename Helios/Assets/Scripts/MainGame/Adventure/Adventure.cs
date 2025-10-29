using System.Collections;
using System.Collections.Generic;
using KumagaiLibrary.Unity;
using UnityEngine;
using UnityEngine.UI;

public class Adventure : MonoBehaviour
{
    [SerializeField]
    private TextAsset[] storyCsvFiles;
    [SerializeField]
    private Sprite[] backGroundSprites;
    private Image backGround;
    [SerializeField]
    private Sprite characterSprites;
    private Image[] characters;
    private Text message;
    private const float textSpeed = 0.1f;
    private GameObject arrow;

    private List<string[]> csvText = new List<string[]>();
    private int lines;
    private int days;
    private float timer;
    private bool isComplete;

    private struct Command
    {
        public const string CharacterSet = "CharacterSet";
        public const string CharacterRemove = "CharacterRemove";
        public const string CharacterSpeak = "CharacterSpeak";
    }
    private enum CharacterQuota
    {
        First=0,
        Second,
        Third,
    }
    private const byte column_command = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SaveDataManager saveDataManager = GameObject.Find("SaveManager").GetComponent<SaveDataManager>();
        message = GameObject.Find("MessageBox").gameObject.GetComponent<Text>();
        days = saveDataManager.Days;
        timer = 0.0f;
        isComplete = false;
        csvText = CsvManager.Read(storyCsvFiles[0]);
        lines = 0;
    }

    // Update is called once per frame
    void Update()
    {
        string[] str = csvText[lines][column_command].Split("_");

        string commanType = str[0];
        string commandTarget = str[1];
        switch (commanType)
        {
            case Command.CharacterSet:
                break;
            case Command.CharacterRemove:
                break;
            case Command.CharacterSpeak:
                break;
        }
    }

    /// <summary>
    /// 文字送りの矢印のモーション。
    /// </summary>
    /// <param name="t">時間軸。</param>
    private void ArrowFloating(float t)
    {
        const float arrowDefaultPositionY = 0.0f;
        const float arrowAddPositionY = 5.0f;
        arrow.transform.localPosition = new Vector3
            (0, arrowDefaultPositionY + arrowAddPositionY * Mathf.Abs(Mathf.Sin(t * 2.0f)), 0);
    }
    /// <summary>
    /// テキストウィンドウの文字送り。
    /// </summary>
    /// <param name="t">時間軸。</param>
    private IEnumerator ViewText(string sceanerio, float t)
    {

        //テキストの合計文字数*一文字あたりの時間で合計時間を計算
        float fullTextTimer = sceanerio.Length * textSpeed;

        //時間軸が合計時間を超えたらこの関数内での時間軸は止める
        if (fullTextTimer <= t) t = fullTextTimer;

        //合計文字数に、時間軸が合計時間の何割なのかをかけ、今が何文字目なのかを計算
        int nowChars = (int)(sceanerio.Length * (t / fullTextTimer));

        //テキストボックスを初期化
        message.text = "";
        for (int i = 0; i < nowChars; i++)
            message.text += sceanerio[i];//計算で出した現在の文字数分だけ一文字ずつtextに記述

        //全ての文字を記述したら、モーションが終わってるフラグを立てる(textの方が長いため)
        if (message.text == sceanerio) isComplete = true;
        else isComplete = false;
        yield return null;
    }
    private IEnumerator ChangeBackGround(Sprite sprite)
    {
        //演出あるならどうぞ
        gameObject.GetComponent<Image>().sprite = sprite;
        yield return null;
    }
}
