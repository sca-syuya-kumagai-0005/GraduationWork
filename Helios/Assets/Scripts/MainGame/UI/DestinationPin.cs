using System.Collections;
using UnityEngine;

public class DestinationPin : EasingMethods
{
    [SerializeField]
    private GameObject pin;
    [SerializeField]
    private GameObject ring;
    [SerializeField]
    private SpriteRenderer color;

    private void Start()
    {
        StartCoroutine(Pin());
    }

    private IEnumerator Pin()
    {
        float t = 0.0f;
        bool isEnd = false;
        float timeLate = 0.5f;
        const float defPosY = 3.0f;
        while (!isEnd)
        {
            t += Time.deltaTime / timeLate;
            float posY = defPosY - defPosY * EaseOutCubic(t);
            pin.transform.localPosition = new Vector3(0, posY + 1, 0);

            if (t >= 1.0f) isEnd = true;
            yield return null;
        }
        StartCoroutine(Rotation());
        StartCoroutine(Hovering());
    }
    private IEnumerator Rotation()
    {
        float t = 0.0f;
        float timeLate = 5f;
        while (true)
        {
            t += Time.deltaTime / timeLate;
            pin.transform.rotation = Quaternion.Euler(0, t * 180, 0);

            yield return null;
        }
    }
    private IEnumerator Hovering()
    {
        while (true)
        {
            StartCoroutine(Ring());
            float t = 0.0f;
            bool isEnd = false;
            float timeLate = 1.0f;
            float posY;
            while (!isEnd)
            {
                t += Time.deltaTime / timeLate;
                posY = Mathf.Abs(Mathf.Sin(t / 2 * Mathf.PI) / 5);
                pin.transform.localPosition = new Vector3(0, posY + 1, 0);

                if (t >= 2.0f) isEnd = true;
                yield return null;
            }
        }
    }
    private IEnumerator Ring()
    {
            float t = 0.0f;
            bool isEnd = false;
            float timeLate = 1.0f;
            const float motScl = 0.3f;
            while (!isEnd)
            {
                t += Time.deltaTime / timeLate;
                float scl = motScl * EaseOutCirc(t);
                ring.transform.localScale = Vector3.one * scl;
                Color col = Color.white;
                col.a = 1.0f - t;
                color.color = col;
                if (t >= 1.0f) isEnd = true;
                yield return null;
            }
    }
}
