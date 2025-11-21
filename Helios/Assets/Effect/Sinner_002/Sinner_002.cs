using System.Collections;
using UnityEngine;

public class Sinner_002 : MonoBehaviour
{
    [Header("=== 対象設定 ===")]
    [SerializeField] private Transform[] targets;          // 移動対象
    [SerializeField] private Vector3[] moveDirections;     // 各対象の移動方向（長さ＝距離を含む）
    [SerializeField] private float moveDuration = 1f;      // 移動にかかる秒数

    [Header("=== スケール設定 ===")]
    [SerializeField] private Transform[] scaleTargets;     // スケール対象
    [SerializeField] private Vector3 targetScale = Vector3.one;  // 最終スケール
    [SerializeField] private float waitAfterMove = 0.5f;         // 移動後に待つ時間
    [SerializeField] private float scaleDuration = 0.5f;         // スケールにかける時間
    [SerializeField] private float scaleDistance = 0.5f;         // 遅延

    [SerializeField] private Vector3[] startPositions;
    private Vector3[] endPositions;
    private Vector3[] initialScales;

    private void OnEnable()
    {
        Initialize();
        PlayAnimation();
    }

    /// <summary>
    /// 初期化（座標・スケール・ターゲット補完）
    /// </summary>
    private void Initialize()
    {
        if (targets == null || targets.Length == 0)
        {
            Debug.LogWarning("targets が設定されていません。");
            return;
        }

        // moveDirections の補完
        if (moveDirections == null || moveDirections.Length < targets.Length)
        {
            System.Array.Resize(ref moveDirections, targets.Length);
            for (int i = 0; i < moveDirections.Length; i++)
                if (moveDirections[i] == Vector3.zero) moveDirections[i] = Vector3.right;
        }

        // scaleTargets の補完（指定されていない場合は targets と同じ）
        if (scaleTargets == null || scaleTargets.Length == 0)
        {
            scaleTargets = targets;
        }
        else if (scaleTargets.Length < targets.Length)
        {
            Debug.LogWarning("scaleTargets の数が targets より少ないため、自動で補完します。");
            System.Array.Resize(ref scaleTargets, targets.Length);
            for (int i = 0; i < targets.Length; i++)
                if (scaleTargets[i] == null) scaleTargets[i] = targets[i];
        }

        endPositions = new Vector3[targets.Length];
        initialScales = new Vector3[scaleTargets.Length];
        for (int i = 0; i < targets.Length; i++)
        {
            //startPositions[i] = targets[i].position;
            endPositions[i] = startPositions[i] + moveDirections[i];
            initialScales[i] = scaleTargets[i].localScale;
        }
    }
    public void PlayAnimation()
    {
        ResetState();
        StartCoroutine(MoveSequence());
    }

    private void ResetState()
    {
        for (int i = 0; i < targets.Length; i++)
        {
            targets[i].position = startPositions[i];
            scaleTargets[i].localScale = initialScales[i];
        }
    }

    private IEnumerator MoveSequence()
    {
        yield return new WaitForSeconds(scaleDistance);
        for (int i = 0; i < targets.Length; i++)
        {
            StartCoroutine(MoveAndScale(targets[i], scaleTargets[i], startPositions[i], endPositions[i]));
        }
        yield return null;
    }

    private IEnumerator MoveAndScale(Transform moveTarget, Transform scaleTarget, Vector3 startPos, Vector3 endPos)
    {
        // === 移動 ===
        float time = 0f;
        while (time < moveDuration)
        {
            float t = time / moveDuration;
            moveTarget.position = Vector3.Lerp(startPos, endPos, Mathf.SmoothStep(0, 1, t));
            time += Time.deltaTime;
            yield return null;
        }
        moveTarget.position = endPos;

        // === 待機 ===
        yield return new WaitForSeconds(waitAfterMove);

        // === スケール ===
        Vector3 initialScale = scaleTarget.localScale;
        float scaleTime = 0f;
        while (scaleTime < scaleDuration)
        {
            float t = scaleTime / scaleDuration;
            scaleTarget.localScale = Vector3.Lerp(initialScale, targetScale, Mathf.SmoothStep(0, 1, t));
            scaleTime += Time.deltaTime;
            yield return null;
        }
        scaleTarget.localScale = targetScale;
    }
}
