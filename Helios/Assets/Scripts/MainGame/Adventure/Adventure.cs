using KumagaiLibrary.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Adventure : EasingMethods
{
    [SerializeField]
    private TextAsset[] storyCsvFiles;
    [SerializeField]
    private Sprite[] backGroundSprites;
    private Image backGround;
    [SerializeField]
    private Sprite[] characterSprites;
    [SerializeField]
    private Image[] characters;
    private Vector3[] defaultPosition = new Vector3[3]
    {
        new Vector3(0,-700,0),
        new Vector3(-400,-700,0),
        new Vector3(400,-700,0)
    };
    private string[] characterNames;
    private bool[] characterIsSetting;
    private Text nameBox;
    private Text messageBox;
    private const float textSpeed = 0.1f;
    private GameObject arrow;
    private BlackScreen blackScreen;

    private List<string[]> csvData = new List<string[]>();
    private int lines;
    private int days;
    private float speakTiming;
    private bool isComplete;

    private const char lineBreakCommand = '|';
    private enum Command
    {
        SetCharacter = 0,
        RemoveCharacter, 
        SetSpeakTiming,
        Speak, 
        PlayBGM,
        StopBGM,
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

    private AudioManager audioManager;
    [SerializeField]
    private AudioClip[] audioClip;

    private bool isMassageSkipped;

    private List<string> textLog = new List<string>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SaveDataManager saveDataManager = GameObject.Find("SaveManager").GetComponent<SaveDataManager>();
        //saveDataManager.Save();
        backGround = GameObject.Find("BackGround").gameObject.GetComponent<Image>();
        blackScreen = GameObject.Find("BlackScreen").gameObject.GetComponent<BlackScreen>();
        nameBox = GameObject.Find("NameBox").gameObject.GetComponent<Text>();
        messageBox = GameObject.Find("MessageBox").gameObject.GetComponent<Text>();
        days = saveDataManager.Days;
        isComplete = false;
        csvData = CsvManager.Read(storyCsvFiles[days]);
        lines = 0;
        speakTiming = 0;
        characterNames = new string[4];
        characterIsSetting = new bool[characters.Length];
        for (int i = 0; i < characters.Length; i++)
        {
            characterIsSetting[i] = false;
            characters[i].color = Color.clear;
        } 
        nameBox.name = "";
        messageBox.name = "";
        audioManager = GameObject.Find("Audio").gameObject.GetComponent<AudioManager>();
        //audioManager.PlayBGM(audioClip[0]);
        StartCoroutine(StartWait());
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
            {
               StartCoroutine(blackScreen.FadeOut("MainScene"));
            }
        }
        else
        {
            if (isComplete)
            {
                Debug.Log(lines + "行目読み込み：Command=" + commandType);
                isComplete = false;
                isMassageSkipped = false;
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

                    case nameof(Command.PlayBGM):
                        StartCoroutine(PlayBGM(csvData[lines][column_Text]));
                        break;

                    case nameof(Command.StopBGM):
                        StartCoroutine(StopBGM());
                        break;

                    default:
                        StartCoroutine(ViewText("システム", lines + "行目で例外、または未対応コマンドが確認されました。"));
                        break;
                }
                lines++;
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            isMassageSkipped = true;
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

        int quotaNumber = (int)ConvertStringToQuota(quota);
        characterNames[quotaNumber] = csvData[lines][column_Text];

        bool quotaIsD = false;
        switch (characterNames[quotaNumber])
        {
            case "黒影　零司":
                characters[quotaNumber].sprite = characterSprites[0];
                break;
            case "知鳴　朔人":
                characters[quotaNumber].sprite = characterSprites[1];
                break;
            case "色織　叶芽":
                characters[quotaNumber].sprite = characterSprites[2];
                break;
            default:
                quotaIsD = true;
                break;
        }
        if (!quotaIsD )
        {
            characterIsSetting[quotaNumber] = true;
            float addPos = 100.0f;
            characters[quotaNumber].transform.localPosition = defaultPosition[quotaNumber];

            characters[quotaNumber].color = Color.clear;
            while (!isEnd)
            {
                timer += Time.deltaTime / timeLate;
                float pos = (defaultPosition[quotaNumber].x + addPos) - addPos * EaseOutCubic(timer);
                characters[quotaNumber].transform.localPosition = new Vector3(pos, defaultPosition[quotaNumber].y, 0);
                characters[quotaNumber].color = Color.clear + Color.white * EaseOutCubic(timer);
                if (timer >= 1.0f) isEnd = true;
                yield return null;
            }
            characters[quotaNumber].color = Color.white;
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
        bool quotaIsD = false;
        int quotaNumber = (int)ConvertStringToQuota(quota);
        if (quotaNumber == (int)CharacterQuota.D) quotaIsD = true;
        if (!quotaIsD)
        {
            float addPos = 100.0f;
            characters[quotaNumber].transform.localPosition = defaultPosition[quotaNumber];
            characters[quotaNumber].color = Color.white;
            while (!isEnd)
            {
                timer += Time.deltaTime / timeLate;
                float pos = defaultPosition[quotaNumber].x - addPos * EaseOutCubic(timer);
                characters[quotaNumber].transform.localPosition = new Vector3(pos, characters[quotaNumber].transform.localPosition.y, 0);
                characters[quotaNumber].color = Color.white - Color.white * EaseOutCubic(timer);
                if (timer >= 1.0f) isEnd = true;
                yield return null;
            }
            characterIsSetting[quotaNumber] = false;
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
        Debug.Log("Speak呼び出し");
        string name = "";
        CharacterQuota quota = ConvertStringToQuota(quotaCommand);
        float[] defPosY = new float[characters.Length];
        for (int i = 0; i < defPosY.Length; i++) defPosY[i] = characters[i].transform.position.y;
        switch (quota)
        {
            case CharacterQuota.Else:
                name = "SystemError";
                break;
            default:
                name = characterNames[(int)quota];
                break;
        }

        for (int i = 0; i < characters.Length; i++)
        {
            Vector3 pos = new Vector3(characters[i].transform.localPosition.x ,defPosY[i]);
            if (i == (int)quota)
            {
                characters[i].color = Color.white;
                pos = defaultPosition[i] + new Vector3(0, 30.0f);
                characters[i].transform.localPosition = pos;
            }
            else
            {
                if (characterIsSetting[i])
                {
                    Color color = Color.white / 2;
                    color.a = 1.0f;
                    characters[i].color = color;
                    pos = defaultPosition[i];
                    characters[i].transform.localPosition = pos;
                }
                else
                {
                    characters[i].color = Color.clear;
                }
            }
        }

        nameBox.text = name;
        //テキストボックスを初期化
        messageBox.text = "";
        yield return new WaitForSeconds(speakTiming);

        for (int i = 0; i < text.Length; i++)
        {
            if (text[i] != lineBreakCommand)
                messageBox.text += text[i];
            else messageBox.text += '\n';
            if (!isMassageSkipped)
            {
                audioManager.PlaySE(audioClip[0]);
                yield return new WaitForSeconds(textSpeed);
            }
        }
        textLog.Add(messageBox.text);
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

    private IEnumerator StartWait()
    {
        yield return new WaitForSeconds(3.0f);
        isComplete = true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sprite"></param>
    /// <returns></returns>
private IEnumerator PlayBGM(string bgm)
    {
        AudioClip ac = null;
        switch (bgm)
        {
            case "ADV_everyday":
                ac = audioClip[1];
                break;

            case "ADV_danger":
                ac = audioClip[2];
                break;

            case "ADV_despair":
                ac = audioClip[3];
                break;

            case "ADV_wonder":
                ac = audioClip[4];
                break;
        }
        audioManager.PlayBGM(ac);
        audioManager.FadeInBGM(0.5f);
        yield return new WaitForSeconds(0.5f);
        isComplete = true;
    }
    private IEnumerator StopBGM()
    {
        audioManager.FadeOutBGM(0.5f);
        yield return new WaitForSeconds(0.5f);
        isComplete = true;
    }
}
