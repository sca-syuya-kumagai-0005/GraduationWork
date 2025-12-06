using System.Collections;
using System.Collections.Generic;
using KumagaiLibrary.Unity;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    private string[] characterNames=new string[3];
    private Text nameBox;
    private Text messageBox;
    private const float textSpeed = 0.1f;
    private GameObject arrow;

    private List<string[]> csvData = new List<string[]>();
    private int lines;
    private int days;
    private float speakTiming;
    private bool isComplete;

    private enum Command
    {
        SetCharacter = 0,
        RemoveCharacter, 
        SetSpeakTiming,
        Speak,
        End
    }
    private enum CharacterQuota
    {
        A=0,
        B,
        C,
        D,
        Else
    }
    private const byte column_command = 0;
    private const byte column_Text = 1;

    private bool isSkiped;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SaveDataManager saveDataManager = GameObject.Find("SaveManager").GetComponent<SaveDataManager>();
        backGround = GameObject.Find("BackGround").gameObject.GetComponent<Image>();
        nameBox = GameObject.Find("NameBox").gameObject.GetComponent<Text>();
        messageBox = GameObject.Find("MessageBox").gameObject.GetComponent<Text>();
        days = saveDataManager.Days;
        isComplete = true;
        csvData = CsvManager.Read(storyCsvFiles[days]);
        lines = 0;
        speakTiming = 0;
        nameBox.name = "";
        messageBox.name = "";
    }

    // Update is called once per frame
    void Update()
    {
        string commandType = null;
        string commandTarget = null;
        string[] data = csvData[lines][column_command].Split("_");
        const int commandTypeAdress = 0;
        const int commandTargetAdress = 1;
        commandType = data[commandTypeAdress];
        const int only = 1;
        if (!(data.Length == only))
        {
            commandTarget = data[commandTargetAdress];
        }
        //Debug.Log(commandType+":"+commandTarget);
        if (csvData[lines][column_command] == nameof(Command.End))
        {
            if (isComplete)
                ChangeScene();
        }
        else
        if (isComplete)
        {
            Debug.Log(lines + "行目読み込み：Command=" + commandType);
            isComplete = false;
            switch (commandType)
            {
                case nameof(Command.SetCharacter):
                    StartCoroutine(CharacterFadein(commandTarget));
                    break;

                case nameof(Command.RemoveCharacter):
                    StartCoroutine(CharacterFadeOut(commandTarget));
                    break;

                case nameof(Command.SetSpeakTiming):
                    SetWaitTime();
                    break;

                case nameof(Command.Speak):
                    StartCoroutine(ViewText(data[commandTargetAdress], csvData[lines][column_Text]));
                    break;

                default:
                    StartCoroutine(ViewText("システム", lines + "行目で例外、または未対応コマンドが確認されました。"));
                    break;
            }
            lines++;
        }
        if(Input.GetMouseButtonDown(0))
        {
            isSkiped = true;
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
    /// CharacterSetが書かれていたら呼ぶ。
    /// </summary>
    /// <returns></returns>
    private IEnumerator CharacterFadein(string quota)
    {
        Debug.Log("SetCharacter呼び出し");
        bool isEnd = false;
        float timer = 0.0f;
        float timeLate = 1.0f;

        characterNames[(int)ConvertStringToQuota(quota)] = csvData[lines][column_Text];
        while (!isEnd)
        {
            timer += Time.deltaTime / timeLate;
            if (timer >= 1.0f) isEnd = true;
            yield return null;
        }
        isComplete = true;
        Debug.Log("SetCharacter完了:");
    }

    private IEnumerator CharacterFadeOut(string quota)
    {
        Debug.Log("RemoveCharacter呼び出し");
        bool isEnd = false;
        float timer = 0.0f;
        float timeLate = 1.0f;

        while (!isEnd)
        {
            timer += Time.deltaTime / timeLate;
            if (timer >= 1.0f) isEnd = true;
            yield return null;
        }
        isComplete = true;
        Debug.Log("RemoveCharacter完了:");
    }

    private void SetWaitTime()
    {
        if (float.TryParse(csvData[lines][column_Text], out var num))
            speakTiming = num;
        else Debug.Log("値が不適切です");
        isComplete = true;
    }
    /// <summary>
    /// テキストウィンドウの文字送りを行う関数。
    /// </summary>
    private IEnumerator ViewText(string quotaCommand,string text)
    {
        isSkiped = false;
        Debug.Log("Speak呼び出し");
        string name = "";
        CharacterQuota quota = ConvertStringToQuota(quotaCommand);
        switch (quota)
        {
            case CharacterQuota.Else:
                name = "SystemError";
                break;
            default:
                name = characterNames[(int)quota];
                break;
        }
        nameBox.text = name;
        //テキストボックスを初期化
        messageBox.text = "";
        yield return new WaitForSeconds(speakTiming);

        for(int i = 0; i < text.Length; i++)
        {
            messageBox.text += text[i];
            if (isSkiped)
            {
                i = text.Length;
                messageBox.text = text;
            }
            yield return new WaitForSeconds(textSpeed);
        }
        Debug.Log("Speak完了:");

        while (!isComplete)
        {
            yield return null;
            Debug.Log("クリック待機中");
            if (Input.GetMouseButtonDown(0)) isComplete = true;
        }
        nameBox.text = "";
        messageBox.text = "";
    }

    private CharacterQuota ConvertStringToQuota(string quota)
    {
        CharacterQuota ret = CharacterQuota.Else;
        switch (quota)
        {
            case nameof(CharacterQuota.A):
                ret = CharacterQuota.A;
                break;
            case nameof(CharacterQuota.B):
                ret = CharacterQuota.B;
                break;
            case nameof(CharacterQuota.C):
                ret = CharacterQuota.C;
                break;
            case nameof(CharacterQuota.D):
                ret = CharacterQuota.D;
                break;
        }
        return ret;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sprite"></param>
    /// <returns></returns>
    private IEnumerator ChangeBackGround(Sprite sprite)
    {
        //演出あるならどうぞ
        gameObject.GetComponent<Image>().sprite = sprite;
        yield return null;
    }

    private void ChangeScene()
    {
        SceneManager.LoadScene("MainScene");
    }
}
