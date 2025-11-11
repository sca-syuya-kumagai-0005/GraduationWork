using Unity.VisualScripting;
using UnityEngine;

public class TimeLine : MonoBehaviour
{
    [SerializeField]
    private GameObject clock;
    [SerializeField]
    private Sprite[] clockSprites;
    private float timeLine;
    public float GetTimeLine {  get { return timeLine; } }
    private float[] times = new float[3];
    private float[] totalTimes = new float[3];
    private bool[] announced = new bool[3];

    public enum TimeStates
    {
        Morning,
        Noon,
        Night,
        Abnormal
    }
    private TimeStates timeState;
    public TimeStates TimeState {  get { return timeState; } set { timeState = value; } }

    [SerializeField]private AnnounceManager announceManager;
    private GameStateSystem gameState;

    private SaveDataManager saveDataManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        saveDataManager = GameObject.Find("SaveManager").GetComponent<SaveDataManager>();
        announceManager = GameObject.Find("AnnounceCenter").GetComponent<AnnounceManager>();
        times = new float[3]
        {
            210.0f,210.0f,180.0f
        };
        totalTimes = new float[3]
        {
            times[0],
            times[0]+times[1],
            times[0]+times[1]+times[2]
        };
        gameState = GameObject.Find("GameState").GetComponent<GameStateSystem>();
    }
    // Update is called once per frame
    void Update()
    {
        timeLine += Time.deltaTime;
        if (gameState.GameState == GameStateSystem.State.Start)
        {
            if (timeLine > 1.5f)
            {
                announceManager.MakeAnnounce("それでは本日の業務を開始します。");
                gameState.GameState= GameStateSystem.State.Wait;
                timeLine = 0.0f;
            }
        }
        if (timeLine > totalTimes[0] - 90.0f && !announced[0])
        {
            announceManager.MakeAnnounce("「朝」終了の1分30秒前です。");
            announced[0] = true;
            timeState = TimeState.Morning;
        }
        if (timeLine > totalTimes[1] - 90.0f && !announced[1])
        {
            announceManager.MakeAnnounce("「昼」終了の1分30秒前です。");
            announced[1] = true;
            timeState = TimeState.Noon;
        }
        if (timeLine > totalTimes[2] - 90.0f && !announced[2])
        {
            announceManager.MakeAnnounce("「夜」終了の1分30秒前です。");
            announced[2] = true;
            timeState = TimeState.Night;
        }
        if (timeLine > totalTimes[2]) timeLine = 0.0f;
    }

    public void NextDay()
    {
        saveDataManager.Days++;
        gameState.GameState = GameStateSystem.State.End;
        timeLine = 0.0f;
        announceManager.MakeAnnounce("本日の業務は以上となります。\nお疲れ様でした。");
    }
}
