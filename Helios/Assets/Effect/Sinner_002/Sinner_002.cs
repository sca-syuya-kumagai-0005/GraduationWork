using System.Collections;
using UnityEngine;

public class Sinner_002 : MonoBehaviour
{
    [Header("=== 対象設定 ===")]
    [SerializeField] private Transform[] targets;
    [SerializeField] private Vector3[] moveDirections;
    [SerializeField] private float moveDuration = 1f;

    [Header("=== スケール設定 ===")]
    [SerializeField] private Transform[] scaleTargets;
    [SerializeField] private Vector3 targetScale = Vector3.one;
    [SerializeField] private float waitAfterMove = 0.5f;
    [SerializeField] private float scaleDuration = 0.5f;
    [SerializeField] private float scaleDistance = 0.5f;

    [SerializeField] private Vector3[] startPositions;  // ← local座標として扱う
    [SerializeField] private GameObject loveEffect;

    private Vector3[] endPositions;
    private Vector3[] initialScales;

    private void OnEnable()
    {
        loveEffect.SetActive(false);
        Initialize();
        PlayAnimation();
    }

    private void Initialize()
    {
        if (targets == null || targets.Length == 0) return;

        // moveDirections 補正
        if (moveDirections.Length < targets.Length)
        {
            System.Array.Resize(ref moveDirections, targets.Length);
            for (int i = 0; i < moveDirections.Length; i++)
                if (moveDirections[i] == Vector3.zero) moveDirections[i] = Vector3.right;
        }

        if (scaleTargets == null || scaleTargets.Length == 0)
            scaleTargets = targets;

        // === End位置をlocal基準で算出 ===
        endPositions = new Vector3[targets.Length];
        initialScales = new Vector3[scaleTargets.Length];

        for (int i = 0; i < targets.Length; i++)
        {
            endPositions[i] = startPositions[i] + moveDirections[i];   // local position移動
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
            targets[i].localPosition = startPositions[i];         // localPosition に変更
            scaleTargets[i].localScale = initialScales[i];
        }
    }

    private IEnumerator MoveSequence()
    {
        loveEffect.SetActive(true);
        yield return new WaitForSeconds(scaleDistance);
        loveEffect.SetActive(false);
        for (int i = 0; i < targets.Length; i++)
        {
            StartCoroutine(MoveAndScale(targets[i], scaleTargets[i],
                startPositions[i], endPositions[i]));
        }
    }

    private IEnumerator MoveAndScale
        (Transform moveTarget, Transform scaleTarget, Vector3 startPos, Vector3 endPos)
    {
        float t = 0;

        // === localPositionで移動 ===
        while (t < moveDuration)
        {
            float rate = t / moveDuration;
            moveTarget.localPosition = Vector3.Lerp(startPos, endPos, Mathf.SmoothStep(0, 1, rate));
            t += Time.deltaTime;
            yield return null;
        }
        moveTarget.localPosition = endPos;

        // 待機
        yield return new WaitForSeconds(waitAfterMove);

        // === スケール ===
        Vector3 s0 = scaleTarget.localScale;
        float st = 0;

        while (st < scaleDuration)
        {
            scaleTarget.localScale = Vector3.Lerp(s0, targetScale, Mathf.SmoothStep(0, 1, st / scaleDuration));
            st += Time.deltaTime;
            yield return null;
        }
        scaleTarget.localScale = targetScale;
    }
}
