using System.Collections;
using UnityEngine;
public class WaveMove_OneDirection : MonoBehaviour
{
    [Header("=== 波移動設定 ===")]
    [Tooltip("移動方向（例：右方向なら (1,0,0)）")]
    public Vector3 moveDirection = Vector3.right;

    [Tooltip("進行方向に対して垂直な波の高さ")]
    public float waveAmplitude = 0.5f;

    [Tooltip("波の回数（上下回数）")]
    public int waveCount = 3;

    [Tooltip("全体の移動距離（進行方向の合計距離）")]
    public float moveDistance = 5.0f;

    [Tooltip("目的地に到達するまでの時間（秒）")]
    public float moveDuration = 2.0f;

    [Tooltip("開始までの遅延時間")]
    public float startDelay = 0f;

    [Tooltip("移動の緩急（Easeカーブ）")]
    public AnimationCurve ease = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private Vector3 startPos;

    private void OnEnable()
    {
        startPos = transform.localPosition;
        StartCoroutine(WaveMoveRoutine());
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

            // 一方向の移動
            float forwardMove = Mathf.Lerp(0f, moveDistance, easedT);

            // 指定回数分の波を生成（波CountでSin波の回数を決定）
            float wave = Mathf.Sin(t * Mathf.PI * 2f * waveCount) * waveAmplitude;

            // 垂直方向を求める（dirに対して直角方向）
            Vector3 perpendicular = new Vector3(-dir.y, dir.x, 0f);

            // 合成
            Vector3 pos = startPos + dir * forwardMove + perpendicular * wave;
            transform.localPosition = pos;

            yield return null;
        }

        // 最終位置に補正
        transform.localPosition = startPos + dir * moveDistance;
    }
}
