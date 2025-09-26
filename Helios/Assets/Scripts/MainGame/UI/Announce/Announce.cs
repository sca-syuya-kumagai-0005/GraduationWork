using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Announce : EasingMethods
{
    private float positionY;
    private Vector3 inPosition = new Vector3(0, 0, 0);
    private Vector3 outPosition = new Vector3(-500.0f,0,0);
    Vector3 defaultPosition;
    bool isPushing = false;
    private void Start()
    {
        defaultPosition = transform.localPosition;
        positionY = defaultPosition.y;
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
        float motionLate = 0.5f;
        while (!isEnd)
        {
            float posX = inPosition.x - outPosition.x * EaseOutCirc(t);
            transform.localPosition = defaultPosition + new Vector3(posX, 0, 0);
            if (t > 1.0f) isEnd = true;
            t += (Time.deltaTime / motionLate);
            yield return null;
        }
        //transform.localPosition = defaultPosition - outPosition;
    }
    public IEnumerator FadeOut()
    {
        bool isEnd = false;
        float t = 0.0f;
        float motionLate = 0.25f;
        while (!isEnd)
        {
            float posX = outPosition.x * t;
            transform.localPosition = new Vector3(posX, transform.localPosition.y, 0);
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
        float motionLate = 0.5f;
        float beforePosY = transform.localPosition.y;
        while (!isEnd)
        {
            float posY = beforePosY + 190.0f * EaseOutCirc(t);
            transform.localPosition = new Vector3(transform.localPosition.x, posY, 0);
            if (t >= 1.0f) isEnd = true;
            t += Time.deltaTime / motionLate;
            yield return null;
        }
        isPushing = false;
    }
}
