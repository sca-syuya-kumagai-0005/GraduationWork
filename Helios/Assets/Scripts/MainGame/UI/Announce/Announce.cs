using System.Collections;
using UnityEngine;

public class Announce : EasingMethods
{
    private const float inPositionX = 0.0f;
    private const float outPositionX = -500.0f;
    Vector3 defaultPosition;
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
            float posX = inPositionX - outPositionX * EaseOutCirc(t);
            transform.localPosition = defaultPosition + new Vector3(posX, 0, 0);
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
