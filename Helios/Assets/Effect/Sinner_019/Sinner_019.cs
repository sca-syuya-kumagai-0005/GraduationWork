using UnityEngine;

public class Sinner_019 : MonoBehaviour
{
    [Header("表示してから非表示に戻すまでの秒数")]
    public float visibleDuration = 2f;

    private Coroutine hideCoroutine;

    private void OnEnable()
    {
        // 表示されたらカウント開始
        if (hideCoroutine != null) StopCoroutine(hideCoroutine);
        hideCoroutine = StartCoroutine(AutoHide());
    }

    private System.Collections.IEnumerator AutoHide()
    {
        yield return new WaitForSeconds(visibleDuration);
        gameObject.SetActive(false);
        hideCoroutine = null;
    }
}
