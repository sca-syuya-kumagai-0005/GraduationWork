using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RestartButton : MonoBehaviour
{
    private bool onCursor;
    [SerializeField]
    private float onCursorScale;
    [SerializeField]
    private float defaultScale;
    [SerializeField]
    private float transScaleTime;
    [SerializeField]
    private Image[] buttonImages;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        onCursor = false;
        StartCoroutine(Blink());
    }

    // Update is called once per frame
    void Update()
    {
        if (onCursor)
        {
            float addScale = onCursorScale - defaultScale;
            if (transform.localScale.x < onCursorScale)
            {
                transform.localScale += Vector3.one * addScale * Time.deltaTime / transScaleTime;
            }
            else
            {
                transform.localScale = Vector3.one * onCursorScale;
            }
        }
        else
        {
            float addScale = defaultScale - onCursorScale;
            if (transform.localScale.x > defaultScale)
            {
                transform.localScale += Vector3.one * addScale * Time.deltaTime / transScaleTime;
            }
            else
            {
                transform.localScale = Vector3.one * defaultScale;
            }
        }

        buttonImages[1].gameObject.SetActive(!onCursor);
    }

    public void OnCursor()
    {
        onCursor = true;
        buttonImages[0].color = Color.red;
    }
    public void OffCursor()
    {
        onCursor= false;
        buttonImages[0].color = Color.white;
    }

    private IEnumerator Blink()
    {
        float t = 0.0f;
        while (true)
        {
            t += Time.deltaTime / 0.25f;
            float a = Mathf.Sin(t) / 2 + 0.5f;
            buttonImages[1].color = new Color(1, 1, 1, a);
            yield return null;
        }
    }
}
