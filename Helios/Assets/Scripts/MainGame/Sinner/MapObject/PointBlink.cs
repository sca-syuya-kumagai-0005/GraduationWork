using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PointBlink : EasingMethods
{
    Player player;
    private SpriteRenderer spriteRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("PlayerObject").GetComponent<Player>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        StartCoroutine(Blink());
    }

private IEnumerator Blink()
    {
        int counter = 0;
        while (true)
        {
            counter++;
            bool isEnd = false;
            float timer = 0.0f;
            float defaultScale = 0.25f;
            float motionedScale = 1.5f;
            if (counter == 10)
            {
                player.Health -= 10;
                motionedScale = 15f;
            }
            Color defaultColor = Color.red;
            Color motionedColor = Color.clear;
            while (!isEnd)
            {
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
}
