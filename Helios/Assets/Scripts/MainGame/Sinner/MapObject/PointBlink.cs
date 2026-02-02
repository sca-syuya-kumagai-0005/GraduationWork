using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class PointBlink : EasingMethods
{
    private SpriteRenderer spriteRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    public IEnumerator Blink(bool isShockWave)
    {
        bool isEnd = false;
        float timer = 0.0f;
        float defaultScale = 0.25f;
        float motionedScale = 1.5f;
        Color defaultColor = Color.red;
        Color motionedColor = Color.clear;

        while (!isEnd)
        {
            if (isShockWave)
            {
                motionedScale = 10.0f;
                defaultColor = new Color(25.5f, 23.0f, 6.4f) / 2;
            }

            timer += Time.deltaTime;
            transform.localScale = Vector3.one
                * (defaultScale + (motionedScale - defaultScale) * EaseOutCirc(timer));
            spriteRenderer.color =
                defaultColor + (motionedColor - defaultColor) * EaseInCubic(timer);

            if (timer > 1.0f) isEnd = true;
            yield return null;
        }
    }
}
