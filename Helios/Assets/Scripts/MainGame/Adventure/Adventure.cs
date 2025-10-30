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
    private const byte column_Text = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SaveDataManager saveDataManager = GameObject.Find("SaveManager").GetComponent<SaveDataManager>();
        message = GameObject.Find("MessageBox").gameObject.GetComponent<Text>();
        days = saveDataManager.Days;
        isComplete = false;
        csvText = CsvManager.Read(storyCsvFiles[0]);
        lines = 0;
    }

    // Update is called once per frame
    void Update()
    {
        string commanType;
        string commandTarget;
        {
            string[] data = csvText[lines][column_command].Split("_");
            const int commandTypeAdress = 0;
            const int commandTargetAdress = 1;
            commanType = data[commandTypeAdress];
            commandTarget = data[commandTargetAdress];
        }

        if (isComplete)
        {
            isComplete = false;
            switch (commanType)
            {
                case Command.CharacterSet:
                    CharacterFadein();
                    break;
                case Command.CharacterRemove:
                    break;
                case Command.CharacterSpeak:
                    break;
            }
        }
    }
    /// <summary>
    /// CharacterSet��������Ă�����ĂԁB
    /// </summary>
    /// <returns></returns>
    private IEnumerator CharacterFadein()
    {
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
    }

    /// <summary>
    /// ��������̖��̃��[�V�����B
    /// </summary>
    /// <param name="t">���Ԏ��B</param>
    private void ArrowFloating(float t)
    {
        const float arrowDefaultPositionY = 0.0f;
        const float arrowAddPositionY = 5.0f;
        arrow.transform.localPosition = new Vector3
            (0, arrowDefaultPositionY + arrowAddPositionY * Mathf.Abs(Mathf.Sin(t * 2.0f)), 0);
    }

    /// <summary>
    /// �e�L�X�g�E�B���h�E�̕�������B
    /// </summary>
    /// <param name="t">���Ԏ��B</param>
    private IEnumerator ViewText(string sceanerio, float t)
    {

        //�e�L�X�g�̍��v������*�ꕶ��������̎��Ԃō��v���Ԃ��v�Z
        float fullTextTimer = sceanerio.Length * textSpeed;

        //���Ԏ������v���Ԃ𒴂����炱�̊֐����ł̎��Ԏ��͎~�߂�
        if (fullTextTimer <= t) t = fullTextTimer;

        //���v�������ɁA���Ԏ������v���Ԃ̉����Ȃ̂��������A�����������ڂȂ̂����v�Z
        int nowChars = (int)(sceanerio.Length * (t / fullTextTimer));

        //�e�L�X�g�{�b�N�X��������
        message.text = "";
        for (int i = 0; i < nowChars; i++)
            message.text += sceanerio[i];//�v�Z�ŏo�������݂̕������������ꕶ������text�ɋL�q

        //�S�Ă̕������L�q������A���[�V�������I����Ă�t���O�𗧂Ă�(text�̕�����������)
        if (message.text == sceanerio) isComplete = true;
        else isComplete = false;
        yield return null;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sprite"></param>
    /// <returns></returns>
    private IEnumerator ChangeBackGround(Sprite sprite)
    {
        //���o����Ȃ�ǂ���
        gameObject.GetComponent<Image>().sprite = sprite;
        yield return null;
    }
}
