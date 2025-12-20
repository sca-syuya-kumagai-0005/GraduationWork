using System.Collections;
using UnityEngine;

public class DeliveryProcess : EasingMethods
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        StartCoroutine(SlideIn());
    }

    private IEnumerator SlideIn()
    {
        bool isEnd = false;
        float t = 0.0f;
        float timeRate = 0.5f;
        float defaultPosX;
        float tergetPosX = -105.0f;
        Vector3 pos = transform.localPosition;
        pos.x = 1330.0f;
        transform.localPosition = pos;
        defaultPosX = pos.x;
        while (!isEnd)
        {
            t += Time.deltaTime/timeRate;
            float addPosX = tergetPosX - defaultPosX;
            pos.x = defaultPosX + addPosX * EaseOutCirc(EaseOutCirc(t));
            transform.localPosition = pos;
            if(t>1.0f) isEnd = true;
            yield return null;
        }
    }
}
