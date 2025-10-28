using System.Collections;
using UnityEngine;

/// <summary>
/// 現在の位置から順番に「相対距離」で波打ちながら移動するスクリプト
/// </summary>
public class WaveMove_MultiPositions : MonoBehaviour
{
    [Header("=== 経路設定 ===")]
    [Tooltip("順番に移動する相対距離（ローカル座標系）")]
    public Vector3[] moveOffsets;

    [Header("=== 波移動設定 ===")]
    [Tooltip("波の振幅（上下幅）")]
    public float waveAmplitude = 0.5f;

    [Tooltip("波の回数（1移動中の上下回数）")]
    public int waveCount = 3;

    [Tooltip("波の開始位相（度数法）\n0=中心, 90=上から, 270=下から開始")]
    [Range(0f, 360f)] public float waveStartPhase = 90f;

    [Tooltip("1区間あたりの移動時間（秒）")]
    public float moveDuration = 2.0f;

    [Tooltip("開始までの遅延時間")]
    public float startDelay = 0f;

    [Tooltip("移動の緩急（Easeカーブ）")]
    public AnimationCurve ease = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Tooltip("最後まで移動したら最初に戻るか")]
    public bool loop = false;

    private Coroutine moveCoroutine;

    private void OnEnable()
    {
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);

        moveCoroutine = StartCoroutine(WaveMoveRoutine());
    }

    private IEnumerator WaveMoveRoutine()
    {
        if (moveOffsets == null || moveOffsets.Length == 0)
            yield break;

        if (startDelay > 0f)
            yield return new WaitForSeconds(startDelay);

        // 出発点は現在位置（ワールド座標）
        Vector3 currentPos = transform.position;

        do
        {
            for (int i = 0; i < moveOffsets.Length; i++)
            {
                Vector3 startPos = currentPos;
                Vector3 targetPos = startPos + moveOffsets[i];

                // 進行方向と波の垂直方向
                Vector3 dir = (targetPos - startPos).normalized;
                Vector3 perpendicular = Vector3.Cross(dir, Vector3.forward);

                float elapsed = 0f;
                while (elapsed < moveDuration)
                {
                    elapsed += Time.deltaTime;
                    float t = Mathf.Clamp01(elapsed / moveDuration);
                    float easedT = ease.Evaluate(t);

                    // 線形補間
                    Vector3 pos = Vector3.Lerp(startPos, targetPos, easedT);

                    // 波打ち追加
                    float phaseRad = waveStartPhase * Mathf.Deg2Rad;
                    float wave = Mathf.Sin(t * Mathf.PI * 2f * waveCount + phaseRad) * waveAmplitude;
                    pos += perpendicular * wave;

                    transform.position = pos;
                    yield return null;
                }

                // 終点補正
                transform.position = targetPos;
                currentPos = targetPos;
            }

            if (loop)
                currentPos = transform.position; // 現在位置からまた次ループへ

        } while (loop);
    }
}
