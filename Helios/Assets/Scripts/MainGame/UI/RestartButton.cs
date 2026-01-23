using UnityEngine;

public class RestartButton : MonoBehaviour
{
    private bool onCursor;
    [SerializeField]
    private float onCursorScale;
    [SerializeField]
    private float defaultScale;
    [SerializeField]
    private float transScaleTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        onCursor = false;
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
    }

    public void OnCursor()
    {
        onCursor = true;
    }
    public void OffCursor()
    {
        onCursor= false;
    }
}
