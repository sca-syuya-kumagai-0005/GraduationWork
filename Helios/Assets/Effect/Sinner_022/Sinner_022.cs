using UnityEngine;
using System.Collections;

public class Sinner_022 : MonoBehaviour
{
    [Header("=== Ripple Material ===")]
    public Material rippleMaterial;

    private int maxWaves = 10;
    private Vector4[] waveCount;
    private float[] waveStartTimes;
    private int activeCount = 0;

    [Header("=== 水滴ローカル座標 ===")]
    [SerializeField] private Vector3 dropLocalPosition = new Vector3(0f, -1.8f, 0f);

    void Awake()
    {
        waveCount = new Vector4[maxWaves];
        waveStartTimes = new float[maxWaves];
    }

    void OnEnable()
    {
        activeCount = 0;

        rippleMaterial.SetFloat("_RippleStrength", 0.1f);
        rippleMaterial.SetFloat("_TimeScale", 0.5f);
    }

    void Update()
    {
        rippleMaterial.SetInt("_ActiveWaveCount", activeCount);
        rippleMaterial.SetVectorArray("_WaveStartPos", waveCount);
        rippleMaterial.SetFloatArray("_WaveStartTime", waveStartTimes);
    }

    //====================================
    // Animation Event から呼ぶ関数
    //====================================
    public void PlayDropRipple()
    {
        StopAllCoroutines();
        StartCoroutine(DropRipple());
    }

    IEnumerator DropRipple()
    {
        // 落下前の「間」
        yield return new WaitForSeconds(0.3f);

        // ローカル → ワールド → スクリーン
        Vector3 worldPos = transform.TransformPoint(dropLocalPosition);
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);

        // 水滴が落ちた瞬間
        rippleMaterial.SetFloat("_RippleStrength", 0.45f);
        rippleMaterial.SetFloat("_TimeScale", 0.7f);

        AddRipple(screenPos);
    }

    //====================================
    // Ripple 処理
    //====================================
    void AddRipple(Vector2 screenPos)
    {
        if (activeCount >= maxWaves)
            activeCount = 0;

        Vector2 uv = new Vector2(
            screenPos.x / Screen.width,
            screenPos.y / Screen.height
        );

        uv = new Vector2(1f - uv.x, 1f - uv.y);

        waveCount[activeCount] = new Vector4(uv.x, uv.y, 0, 0);
        waveStartTimes[activeCount] = Time.time;
        activeCount++;
    }
}
