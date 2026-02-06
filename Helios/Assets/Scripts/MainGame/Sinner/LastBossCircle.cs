using System.Collections;
using UnityEngine;

public class LastBossCircle : EasingMethods
{
    GameObject[] targetSpites = new GameObject[3];
    private float time;
    float rotationZ;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rotationZ = 0.0f;
        time = 0.325f;
        targetSpites[0] = transform.GetChild(0).gameObject;
        targetSpites[1] = transform.GetChild(1).gameObject;
        targetSpites[2] = transform.GetChild(2).gameObject;
        StartCoroutine(Rotate());
        StartCoroutine(Blink());
        //spriteRenderer = GetComponent<SpriteRenderer>();    
        //spriteRenderer.color = new Color(100.0f, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        rotationZ += Time.deltaTime * 90.0f;
    }

    private IEnumerator Rotate()
    {
        while (true)
        {
            bool isEnd = false;
            float t = 0.0f;
            while (!isEnd)
            {
                t += Time.deltaTime / 1.5f;
                float rotZ = rotationZ + 2 * 270 * EaseInOutCubic(t);
                Vector3 rot = new Vector3(0,0, rotZ);
                targetSpites[1].gameObject.transform.localRotation = Quaternion.Euler(rot);
                targetSpites[2].gameObject.transform.localRotation = Quaternion.Euler(-rot);


                if (t > 1.0f) isEnd = true;
                yield return null;
            }
        }
    }

    private IEnumerator Blink()
    {
        float t = 0.0f;
        SpriteRenderer sr = targetSpites[0].GetComponent<SpriteRenderer>();
        Color color = Color.white;
        while (true)
        {
            t += Time.deltaTime / time;
            color.a = Mathf.Sin(t) / 2 + 0.5f;
            sr.color = color;
            yield return null;
        }
    }
}
