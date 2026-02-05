using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DataText : MonoBehaviour
{
    [SerializeField] Text headerText;
    [SerializeField] Text dataText;
    [SerializeField] ScrollRect listScrollRect;
    [SerializeField] RectTransform contentsRect;
    [SerializeField] TextMover textMover;

    public void SetText(string _header, string _data)
    {
        headerText.text = _header;
        dataText.text = _data;
        StartCoroutine(ContentSizeChange());
    }

    IEnumerator ContentSizeChange()
    {
        yield return null;
        contentsRect.sizeDelta = new Vector2(contentsRect.sizeDelta.x, headerText.rectTransform.sizeDelta.y + dataText.rectTransform.sizeDelta.y);
        listScrollRect.verticalNormalizedPosition = 1f;
        textMover.PositionReset();
    }
}
