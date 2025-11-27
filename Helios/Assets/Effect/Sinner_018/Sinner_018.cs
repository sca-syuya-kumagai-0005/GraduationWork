using UnityEngine;
using System.Collections;

public class Sinner_018 : MonoBehaviour
{
    [Header("=== Target Settings ===")]
    [SerializeField] private SpriteRenderer targetSprite;      // アルファフェード対象
    [SerializeField] private Transform targetTransform;        // 回転対象

    [Header("=== Fade Settings ===")]
    [SerializeField, Range(0f, 1f)] private float fromAlpha = 0f;
    [SerializeField, Range(0f, 1f)] private float toAlpha = 1f;
    [SerializeField] private float fadeDuration = 1.0f;

    [Header("=== Rotation Settings ===")]
    [SerializeField] private Vector3 fromRotation = Vector3.zero;
    [SerializeField] private Vector3 toRotation = new Vector3(0f, 0f, 360f);
    [SerializeField] private float rotationDuration = 1.0f;

    private Coroutine playRoutine;

    private void OnEnable()
    {
        Play();
    }

    public void Play()
    {
        // 進行中のコルーチンを停止
        if (playRoutine != null)
            StopCoroutine(playRoutine);
        if (targetTransform != null)
            targetTransform.localRotation = Quaternion.Euler(fromRotation);
        // フェード → 回転 の順で行うメイン処理
        playRoutine = StartCoroutine(PlaySequence());
    }

    private IEnumerator PlaySequence()
    {
        // フェードを先に実行し、終わるまで待つ
        yield return StartCoroutine(FadeCoroutine());
        yield return new WaitForSeconds(0.25f);
        // 次に回転を実行
        yield return StartCoroutine(RotateCoroutine());
        yield return new WaitForSeconds(0.25f);
        //フェードアウト
        yield return StartCoroutine(FadeOutCoroutine());
        yield return new WaitForSeconds(0.25f);
        gameObject.SetActive(false);
    }

    private IEnumerator FadeCoroutine()
    {
        if (targetSprite == null)
            yield break;

        SetAlpha(fromAlpha);

        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / fadeDuration);

            float a = Mathf.Lerp(fromAlpha, toAlpha, t);
            SetAlpha(a);

            yield return null;
        }

        SetAlpha(toAlpha);
    }

    private void SetAlpha(float a)
    {
        if (targetSprite == null) return;

        Color c = targetSprite.color;
        c.a = a;
        targetSprite.color = c;
    }

    private IEnumerator RotateCoroutine()
    {
        if (targetTransform == null)
            yield break;

        targetTransform.localRotation = Quaternion.Euler(fromRotation);

        float timer = 0f;
        while (timer < rotationDuration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / rotationDuration);

            Quaternion start = Quaternion.Euler(fromRotation);
            Quaternion end = Quaternion.Euler(toRotation);

            targetTransform.localRotation = Quaternion.Lerp(start, end, t);

            yield return null;
        }

        targetTransform.localRotation = Quaternion.Euler(toRotation);
    }

    private IEnumerator FadeOutCoroutine()
    {
        if (targetSprite == null)
            yield break;

        // 最初は toAlpha から開始
        SetAlpha(toAlpha);

        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / fadeDuration);

            // 逆方向に補間
            float a = Mathf.Lerp(toAlpha, fromAlpha, t);
            SetAlpha(a);

            yield return null;
        }

        // 最終的に fromAlpha
        SetAlpha(fromAlpha);
    }


}
