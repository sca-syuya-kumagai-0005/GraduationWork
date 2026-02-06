using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LastBossCount : EasingMethods
{
    [SerializeField]
    private Text[] timerText;
    private float timer;
    [SerializeField]
    private float timeLimit;
    public float GetTimer { get { return timeLimit - timer; }}
    //private string digits;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(FadeIn());
        timer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        float t = timeLimit - timer;
        string digit = null;
        int value = (int)(t * 100);
        int digit1 = value / 100;
        int digit2 = value / 10 - value / 100 * 10;
        int digit3 = value - value / 10 * 10;
        if (t >= 10.0f)
        {
            digit = digit1.ToString() + ":" + digit2.ToString() + digit3.ToString();
        }
        else
        {
            digit = "0" + digit1.ToString() + ":" + digit2.ToString() + digit3.ToString();
        }
        if (t <= 0.0f) digit = "00:00";
        timerText[0].text = digit;
        timerText[1].text = digit;
    }

    private IEnumerator FadeIn()
    {
        bool isEnd = false;
        float t = 0.0f;
        float defaultPositionY = 700.0f;
        float motionedPositionY = 450.0f;
        while (!isEnd)
        {
            t += Time.deltaTime;
            float addPos = (motionedPositionY - defaultPositionY) * EaseOutQuart(t);
            transform.localPosition = new Vector3(transform.localPosition.x, defaultPositionY + addPos, 0);
            if (t >= 1.0f) isEnd = true;
            yield return null;
        }
    }
    public IEnumerator FadeOut()
    {
        bool isEnd = false;
        float t = 0.0f;
        float defaultPositionY = 450.0f;
        float motionedPositionY = 700.0f;
        while (!isEnd)
        {
            t += Time.deltaTime / 0.5f;
            float addPos = (motionedPositionY - defaultPositionY) * EaseInQuart(t);
            transform.localPosition = new Vector3(transform.localPosition.x, defaultPositionY + addPos, 0);
            if (t >= 1.0f) isEnd = true;
            yield return null;
        }
        Destroy(gameObject);
    }
}
