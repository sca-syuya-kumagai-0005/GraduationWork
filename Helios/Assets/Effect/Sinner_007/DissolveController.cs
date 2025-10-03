using UnityEngine;
using System.Collections;

public class DissolveController : MonoBehaviour
{
    [Header("対象マテリアル")]
    public Material dissolveMaterial;

    [Header("共通設定")]
    public float startDelay = 0f;  // ← 共通の遅延

    [Header("Dissolve設定")]
    public float startDissolve = 0f;
    public float endDissolve = 1f;
    public float dissolveDuration = 2f;

    [Header("EdgeWidth設定")]
    public float startEdgeWidth = 0.05f;
    public float endEdgeWidth = 0.2f;
    public float edgeWidthDuration = 2f;

    [Header("TextDissolve設定")]
    public float startTextDissolve = 1f;
    public float endTextDissolve = 0f;
    public float dissolveTextDuration = 1f;

    [Header("回転対象")]
    [SerializeField] private Transform targetObject; // 角度を操作する対象

    [Header("回転設定")]
    public float rotateDuration = 1f;   // 回転にかける秒数
    public float startAngle = 0f;       // 回転開始角度（Y軸）
    public float endAngle = 180f;       // 回転終了角度（Y軸）

    [Header("テキスト")]
    [SerializeField] private SpriteRenderer nameText_007;

    private Coroutine mainRoutine;

    private void OnEnable()
    {
        // 初期値リセット
        if (dissolveMaterial != null)
        {
            dissolveMaterial.SetFloat("_Dissolve", startDissolve);
            dissolveMaterial.SetFloat("_EdgeWidth", startEdgeWidth);
        }

        if (nameText_007 != null && nameText_007.material != null)
        {
            nameText_007.material.SetFloat("_Amount", startTextDissolve);
        }

        if (targetObject != null)
            targetObject.rotation = Quaternion.Euler(0, startAngle, 0);

        // 実行開始
        PlaySequence();
    }

    private void OnDisable()
    {
        if (mainRoutine != null)
        {
            StopCoroutine(mainRoutine);
            mainRoutine = null;
        }
    }

    public void PlaySequence()
    {
        if (mainRoutine != null) StopCoroutine(mainRoutine);
        mainRoutine = StartCoroutine(MainProcess());
    }

    private IEnumerator MainProcess()
    {
        // 1. Start → End へ回転
        yield return Rotate(startAngle, endAngle, rotateDuration);

        // 2. Dissolve と EdgeWidth と TextDissolve を同時にスタート
        yield return RunDissolveAndEdge();

        // 3. 少し待ってから、End → Start へ回転して戻す
        yield return new WaitForSeconds(0.25f);
        yield return Rotate(endAngle, startAngle, rotateDuration);

        // 4. すべて終わったら自分を非アクティブ化
        gameObject.SetActive(false);
    }

    private IEnumerator Rotate(float from, float to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float angle = Mathf.Lerp(from, to, t);
            if (targetObject != null)
                targetObject.rotation = Quaternion.Euler(0, angle, 0);
            yield return null;
        }
        if (targetObject != null)
            targetObject.rotation = Quaternion.Euler(0, to, 0);
    }

    private IEnumerator RunDissolveAndEdge()
    {
        Coroutine dissolveRoutine = StartCoroutine(
            AnimateFloat(dissolveMaterial, "_Dissolve", startDissolve, endDissolve, dissolveDuration, startDelay));

        Coroutine edgeRoutine = StartCoroutine(
            AnimateFloat(dissolveMaterial, "_EdgeWidth", startEdgeWidth, endEdgeWidth, edgeWidthDuration, startDelay));

        Coroutine dissolveTextRoutine = null;
        if (nameText_007 != null && nameText_007.material != null)
        {
            dissolveTextRoutine = StartCoroutine(
                AnimateFloat(nameText_007.material, "_Amount", startTextDissolve, endTextDissolve, dissolveTextDuration, startDelay));
        }

        // 全部終わるまで待つ
        yield return dissolveRoutine;
        yield return edgeRoutine;
        if (dissolveTextRoutine != null) yield return dissolveTextRoutine;
    }

    private IEnumerator AnimateFloat(Material mat, string property, float from, float to, float duration, float delay = 0f)
    {
        if (mat == null) yield break;

        if (delay > 0f)
            yield return new WaitForSeconds(delay);

        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float value = Mathf.Lerp(from, to, t / duration);
            mat.SetFloat(property, value);
            yield return null;
        }
        mat.SetFloat(property, to);
    }
}
