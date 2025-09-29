using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Announce : EasingMethods
{
    private const float inPositionX = 0.0f;
    private const float outPositionX = -500.0f;
    Vector3 defaultPosition;
    bool isPushing = false;
    private void Start()
    {
        defaultPosition = transform.localPosition;
        StartCoroutine(FadeIn());
    }

    public IEnumerator DeleteThisAnnounce()
    {
        StartCoroutine(FadeOut());
        while (isPushing)yield return null;
        yield return new WaitForSeconds(1.0f);
        Destroy(gameObject);
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
        while (!isEnd)
        {
            float posX = outPositionX * t;
            transform.localPosition = defaultPosition + new Vector3(posX, 0, 0);
            if (t >= 1.0f) isEnd = true;
            t += Time.deltaTime / motionLate;
            yield return null;
        }
    }

    public IEnumerator PushUp()
    {
        isPushing = true;
        bool isEnd = false;
        float t = 0.0f;
        const float motionLate = 0.5f;
        const float announceSize = 190.0f;
        float posY = 0.0f;
        while (!isEnd)
        {
            posY = announceSize * EaseOutCirc(t);
            transform.localPosition = defaultPosition + new Vector3(0, posY, 0);
            if (t >= 1.0f) isEnd = true;
            t += Time.deltaTime / motionLate;
            yield return null;
        }
        defaultPosition.y = defaultPosition.y + posY;
        isPushing = false;
    }
}
