using UnityEngine;

/// <summary>
/// 複数のターゲットを指定スケール間で1回だけ往復させるアニメーション。
/// 戻る時にディレイ対応。表示/非表示切替で毎回最初から再生可能。
/// </summary>
public class Sinner_012 : MonoBehaviour
{
    [Header("=== Target Objects ===")]
    [SerializeField] private Transform[] targetObjects;

    [Header("=== Scale Animation Settings ===")]
    [SerializeField] private Vector3 startScale = Vector3.one;
    [SerializeField] private Vector3 endScale = Vector3.one * 2f;

    [Tooltip("start → end の時間（秒）")]
    [SerializeField] private float duration = 1.5f;

    [Tooltip("endScale に到達してから戻り始めるまでのディレイ時間")]
    [SerializeField] private float delayAtEnd = 0.5f;

    private float time;
    private bool isReturning = false;
    private float delayTimer = 0f;
    private bool isFinished = false;

    private void OnEnable()
    {
        ResetAnimationState();
    }

    private void Update()
    {
        if (isFinished || targetObjects == null || targetObjects.Length == 0)
            return;

        if (!isReturning)
        {
            // A → B
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / duration);

            ApplyScaleToAll(Vector3.Lerp(startScale, endScale, t));

            if (t >= 1f)
            {
                // B に到達 → ディレイ開始
                isReturning = true;
                delayTimer = 0f;
            }
        }
        else
        {
            // B → A のディレイ
            delayTimer += Time.deltaTime;
            if (delayTimer < delayAtEnd) return;

            // B → A
            time -= Time.deltaTime;
            float t = Mathf.Clamp01(time / duration);

            ApplyScaleToAll(Vector3.Lerp(startScale, endScale, t));

            if (t <= 0f)
            {
                // 完全に startScale に戻す
                ApplyScaleToAll(startScale);

                // 1回だけなので終了
                isFinished = true;
                gameObject.SetActive(false);
            }
        }
    }

    private void ApplyScaleToAll(Vector3 scale)
    {
        for (int i = 0; i < targetObjects.Length; i++)
        {
            if (targetObjects[i] != null)
                targetObjects[i].localScale = scale;
        }
    }


    private void ResetAnimationState()
    {
        time = 0f;
        isReturning = false;
        delayTimer = 0f;
        isFinished = false;

        ApplyScaleToAll(startScale);
        
    }
}
