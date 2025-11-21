using System.Collections;
using UnityEngine;

public class WaveMove_OneDirection : MonoBehaviour
{
    [Header("=== 波移動設定 ===")]
    public Vector3 moveDirection = Vector3.right;
    public float waveAmplitude = 0.5f;
    public int waveCount = 3;
    public float moveDistance = 5.0f;
    public float moveDuration = 2.0f;
    public float startDelay = 0f;
    public AnimationCurve ease = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private Vector3 initialPos;   // ← 初回のみ保存する位置（不変）
    private Coroutine moveRoutine;

    private void Awake()
    {
        initialPos = transform.localPosition;  // ← 1度だけ保存
    }

    private void OnEnable()
    {
        // 表示ごとに開始位置へ戻す
        transform.localPosition = initialPos;

        if (moveRoutine != null)
            StopCoroutine(moveRoutine);

        moveRoutine = StartCoroutine(WaveMoveRoutine());
    }

    private IEnumerator WaveMoveRoutine()
    {
        if (startDelay > 0f)
            yield return new WaitForSeconds(startDelay);

        float elapsed = 0f;
        Vector3 dir = moveDirection.normalized;

        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / moveDuration);
            float easedT = ease.Evaluate(t);

            float forwardMove = Mathf.Lerp(0f, moveDistance, easedT);

            float wave = Mathf.Sin(t * Mathf.PI * 2f * waveCount) * waveAmplitude;

            Vector3 perpendicular = new Vector3(-dir.y, dir.x, 0f);

            Vector3 pos = initialPos + dir * forwardMove + perpendicular * wave;
            transform.localPosition = pos;

            yield return null;
        }

        transform.localPosition = initialPos + dir * moveDistance;
    }
}
