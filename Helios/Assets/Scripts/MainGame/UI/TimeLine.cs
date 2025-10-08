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
    [SerializeField]private AnnounceManager announceManager;
    Player player;
    private GameStateSystem gameState;

    private SaveDataManager saveDataManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        saveDataManager = GameObject.Find("SaveManager").GetComponent<SaveDataManager>();
        player = GameObject.Find("Player").GetComponent<Player>();
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
                announceManager.MakeAnnounce("����ł͖{���̋Ɩ����J�n���܂��B");
                gameState.GameState= GameStateSystem.State.Wait;
                timeLine = 0.0f;
            }
        }
        if (timeLine > totalTimes[0] - 90.0f && !announced[0])
        {
            announceManager.MakeAnnounce("�u���v�I����1��30�b�O�ł��B");
            announced[0] = true;
        }
        if (timeLine > totalTimes[1] - 90.0f && !announced[1])
        {
            announceManager.MakeAnnounce("�u���v�I����1��30�b�O�ł��B");
            announced[1] = true;
        }
        if (timeLine > totalTimes[2] - 90.0f && !announced[2])
        {
            announceManager.MakeAnnounce("�u��v�I����1��30�b�O�ł��B");
            announced[2] = true;
        }
        if (timeLine > totalTimes[2]) timeLine = 0.0f;
    }

    public void NextDay()
    {
        saveDataManager.Days++;
        gameState.GameState = GameStateSystem.State.End;
        timeLine = 0.0f;
        player.formatting();
        announceManager.MakeAnnounce("�{���̋Ɩ��͈ȏ�ƂȂ�܂��B\n�����l�ł����B");
    }
}
