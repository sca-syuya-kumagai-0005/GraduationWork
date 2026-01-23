using System.Collections.Generic;
using UnityEngine;

public class TimeLine : MonoBehaviour
{
    [SerializeField]
    private float timer;
    public float GetTimer {  get { return timer; } }
    private float[] times = new float[3];
    private float[] totalTimes = new float[3];
    private bool[] announced = new bool[3];
    public enum TimeState
    {
        Morning,
        Noon,
        Night,
        Abnormal
    }
    private TimeState timeState;
    public TimeState TimeStateAccess {  get { return timeState; } set { timeState = value; } }

    [SerializeField]private AnnounceManager announceManager;
    private GameStateSystem gameState;

    private SaveDataManager saveDataManager;
    [SerializeField]
    private List<string> abnormalityList;
    public List<string> AbnormalityList { get { return abnormalityList; }set { abnormalityList = value; } }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        saveDataManager = GameObject.Find("SaveManager").GetComponent<SaveDataManager>();
        announceManager = GameObject.Find("AnnounceCenter").GetComponent<AnnounceManager>();
        times = new float[3]
        {
            10.0f,10.0f,180.0f
        };
        totalTimes = new float[3]
        {
            times[0],
            times[0]+times[1],
            times[0]+times[1]+times[2]
        };
        timeState = TimeState.Morning;
        gameState = GameObject.Find("GameState").GetComponent<GameStateSystem>();
        abnormalityList = new List<string>();
    }
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (gameState.GameState == GameStateSystem.State.Start)
        {
            if (timer > 1.5f)
            {
                announceManager.MakeAnnounce("それでは本日の業務を開始します。");
                gameState.GameState = GameStateSystem.State.Wait;
                timer = 0.0f;
            }
        }
        if (!announced[0])
        {
            if (timer > totalTimes[0])
            {
                announced[0] = true;
                if (abnormalityList.Count == 0)
                {
                    announceManager.MakeAnnounce("「昼」になりました。");
                    timeState = TimeState.Noon;
                }
            }
        }
        if (!announced[1])
        {
            if (timer > totalTimes[1])
            {
                announced[1] = true;
                if (abnormalityList.Count == 0)
                {
                    announceManager.MakeAnnounce("「夜」になりました。");
                    timeState = TimeState.Night;
                }
            }
        }
        if (!announced[2])
        {
            if (timer > totalTimes[2])
            {
                announced[2] = true;
                if (abnormalityList.Count == 0)
                {
                    announceManager.MakeAnnounce("「朝」になりました。");
                    timeState = TimeState.Morning;
                }
            }
        }
        if (timer > totalTimes[2])
        {
            timer = 0.0f;
            for (int i = 0; i < times.Length; i++) announced[i] = false;
        }
    }

    public void NextDay()
    {
        saveDataManager.Days++;
        gameState.GameState = GameStateSystem.State.End;
        timer = 0.0f;
        announceManager.MakeAnnounce("本日の業務は以上となります。\nお疲れ様でした。");
    }

    public void AddAbnormalityList(string sinnerName)
    {
        abnormalityList.Add(sinnerName);
    }
    public void RemoveAbnormalityList(string sinnerName)
    {
        abnormalityList.Remove(sinnerName);
    }
}
