using System.Collections;
using UnityEngine;

public class TestScript : EasingMethods
{
    SpriteRenderer spriteRenderer;
    float rotationZ;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rotationZ = 0.0f;
        StartCoroutine(Rotate());
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
                Vector3 rot = new Vector3(0, 0, rotZ);
                gameObject.transform.localRotation = Quaternion.Euler(rot);


                if (t > 1.0f) isEnd = true;
                yield return null;
            }
        }
    }
}
