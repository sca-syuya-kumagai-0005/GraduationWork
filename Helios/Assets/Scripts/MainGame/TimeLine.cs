using UnityEngine;

public class TimeLine : MonoBehaviour
{
    private float timeLine;
    public float GetTimeLine {  get { return timeLine; } }
    private float[] times = new float[3];


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        times = new float[3]
        {
            210.0f,210.0f,180.0f
        };
    }

    // Update is called once per frame
    void Update()
    {
        timeLine += Time.deltaTime;
    }

    protected void timerReset()
    {
        timeLine = 0.0f;

    }
}
