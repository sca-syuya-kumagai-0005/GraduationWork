using System.Collections;
using UnityEngine;

public class Announce : EasingMethods
{
    private const float inPositionX = 0.0f;
    private const float outPositionX = -500.0f;
    private Vector3 defaultPosition;
    private float height;
    public float SetHeight { set { height = value; } }
    private void Start()
    {
        defaultPosition = transform.localPosition;
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        bool isEnd = false;
        float t = 0.0f;
        const float motionLate = 0.5f;
        while (!isEnd)
        {
            float addPosX = inPositionX - outPositionX * EaseOutCirc(t);
            defaultPosition.y=transform.localPosition.y;
            transform.localPosition = defaultPosition + new Vector3(addPosX, 0, 0);
            if (t > 1.0f) isEnd = true;
            t += (Time.deltaTime / motionLate);
            yield return null;
        }
        defaultPosition.x = transform.localPosition.x;
    }
    public IEnumerator FadeOut()
    {
        bool isEnd = false;
        float t = 0.0f;
        const float motionLate = 0.25f;
        defaultPosition.y = transform.localPosition.y;
        while (!isEnd)
        {
            float posX = outPositionX * t;
            transform.localPosition = defaultPosition + new Vector3(posX, 0, 0);
            if (t >= 1.0f) isEnd = true;
            t += Time.deltaTime / motionLate;
            yield return null;
        }
        Destroy(gameObject);
    }
}
