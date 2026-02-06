using System.Collections;
using UnityEngine;

public class Beam : EasingMethods
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnEnable()
    {
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.useGravity = false;
        StartCoroutine(LaserBeam());
    }

    private IEnumerator LaserBeam()
    {
        bool isEnd = false;
        float timer = 0.0f;
        float timeLate = 0.5f;
        while (!isEnd)
        {
            timer += Time.deltaTime / timeLate;
            Vector3 scale = transform.localScale;
            scale.x = 25 * EaseInOutCirc(timer);
            transform.localScale = scale;
            if (timer >= 1.0f) isEnd = true;
            yield return null;
        }
        transform.localScale = new Vector3(25.0f, 500.0f, 25.0f);
        isEnd = false;
        timer = 0.0f;
        timeLate = 1.5f;
        while (!isEnd)
        {
            timer += Time.deltaTime / timeLate;
            Vector3 scale = transform.localScale;
            scale.x = 25.0f - 25.0f * EaseInOutCirc(timer);
            transform.localScale = scale;
            if (timer >= 1.0f) isEnd = true;
            yield return null;
        }
        Destroy(gameObject);
    }
}
