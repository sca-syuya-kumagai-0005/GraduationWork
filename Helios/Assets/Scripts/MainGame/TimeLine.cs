using UnityEngine;

public class TimeLine : MonoBehaviour
{
    private float timeLine;
    public float GetTimeLine {  get { return timeLine; } }
    private float[] times = new float[3];
    private float[] totalTimes = new float[3];
    private bool[] announced = new bool[3];
    private AnnounceManager announceManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
    }

    // Update is called once per frame
    void Update()
    {
        timeLine += Time.deltaTime;
        if (timeLine > totalTimes[0] - 90.0f && !announced[0])
        {
            announceManager.MakeAnnounce("「朝」終了の1分30秒前です。");
            announced[0] = true;
        }
        if (timeLine > totalTimes[1] - 90.0f && !announced[1])
        {
            announceManager.MakeAnnounce("「昼」終了の1分30秒前です。");
            announced[1] = true;
        }
        if (timeLine > totalTimes[2] - 90.0f && !announced[2])
        {
            announceManager.MakeAnnounce("「夜」終了の1分30秒前です。");
            announced[2] = true;
        }
    }

    protected void timerReset()
    {
        timeLine = 0.0f;
    }
}
