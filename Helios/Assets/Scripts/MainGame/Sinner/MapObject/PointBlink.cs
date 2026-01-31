using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PointBlink : EasingMethods
{
    private SpriteRenderer spriteRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        StartCoroutine(Blink());
    }

private IEnumerator Blink()
    {
        int counter = 0;
        while (true)
        {
            bool isEnd = false;
            float timer = 0.0f;
            float defaultScale = 0.25f;
            float motionedScale = 1.5f;
            Color defaultColor = Color.red;
            Color motionedColor = Color.clear;
            while (!isEnd)
            {

                if (counter % 10 == 0)
                {
                    motionedScale = 15.0f;
                    defaultColor = new Color(25.5f, 23.0f, 6.4f)/2;
                }
                else
                {
                    motionedScale = 1.5f;
                    defaultColor = Color.red;
                }
                timer += Time.deltaTime;
                transform.localScale = Vector3.one
                    * (defaultScale + (motionedScale - defaultScale) * EaseOutCirc(timer));
                spriteRenderer.color =
                    defaultColor + (motionedColor - defaultColor) * EaseInCubic(timer);

                if (timer > 1.0f) isEnd = true;
                yield return null;
            }
            counter++;
        }
    }
}
