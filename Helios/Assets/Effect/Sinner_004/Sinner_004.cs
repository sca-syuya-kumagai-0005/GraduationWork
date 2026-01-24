using UnityEngine;
using System.Collections;

public class Sinner_004 : MonoBehaviour
{
    public Material rippleMaterial;
    private int maxWaves = 10;

    private Vector4[] waveCount;
    private float[] waveStartTimes;
    private int activeCount = 0;

    [Header("背景フェード対象")]
    [SerializeField] private SpriteRenderer backGroundSprite; // スプライトのColorを操作

    [Header("背景フェード設定")]
    public float fadeDuration = 1f;      // α変化にかける秒数
    public float startAlpha = 0.75f;     // フェードイン後の到達α値
    public float endAlpha = 0f;          // フェードアウト後のα値

    [SerializeField] private AudioManager audioManager;
    [SerializeField] private AudioClip sinner_004;

    void OnEnable()
    {
        audioManager = GameObject.Find("Audio").GetComponent<AudioManager>();
        
        // 毎回リセット
        waveCount = new Vector4[maxWaves];
        waveStartTimes = new float[maxWaves];
        activeCount = 0;
        rippleMaterial.SetFloat("_RippleStrength", 0.1f);
        rippleMaterial.SetFloat("_TimeScale", 0.5f);
        // 背景透明化してスタート
        if (backGroundSprite != null)
        {
            Color c = backGroundSprite.color;
            backGroundSprite.color = new Color(c.r, c.g, c.b, 0f);
        }

        // コルーチン開始
        StartCoroutine(FadeAndRippleRoutine());
    }

    void OnDisable()
    {
        // オブジェクトが無効化されたときにコルーチンを止める
        StopAllCoroutines();
    }

    void Update()
    {
        rippleMaterial.SetInt("_ActiveWaveCount", activeCount);
        rippleMaterial.SetVectorArray("_WaveStartPos", waveCount);
        rippleMaterial.SetFloatArray("_WaveStartTime", waveStartTimes);
    }

    void AddRipple(Vector2 screenPos)
    {
        if (activeCount >= maxWaves) activeCount = 0;

        Vector2 uv = new Vector2(screenPos.x / Screen.width, screenPos.y / Screen.height);
        uv = new Vector2(1f - uv.x, 1f - uv.y);

        waveCount[activeCount] = new Vector4(uv.x, uv.y, 0, 0);
        waveStartTimes[activeCount] = Time.time;
        activeCount++;
    }

    IEnumerator FadeAndRippleRoutine()
    {
        // 背景スプライトのフェードイン処理
        Color baseColor = backGroundSprite.color;
        float startAlphaNow = baseColor.a;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / fadeDuration);

            float newAlpha = Mathf.Lerp(startAlphaNow, this.startAlpha, t);
            backGroundSprite.color = new Color(baseColor.r, baseColor.g, baseColor.b, newAlpha);

            yield return null;
        }
        audioManager.PlaySE(sinner_004);
        // フェード完了後に波紋ループ開始
        yield return StartCoroutine(RippleAndFadeOut());
        
    }

    IEnumerator RippleAndFadeOut()
    {
        int count = 0;

        // ランダムな位置に波紋
        while (count < maxWaves - 3)
        {
            Vector2 randomPos = new Vector2(
                Random.Range(0f, Screen.width),
                Random.Range(0f, Screen.height)
            );

            AddRipple(randomPos);
            count++;
            yield return new WaitForSeconds(0.15f);
        }

        // 最後に中央に波紋
        yield return new WaitForSeconds(1f);
        rippleMaterial.SetFloat("_RippleStrength", 0.5f);
        rippleMaterial.SetFloat("_TimeScale", 0.6f);
        Vector2 center = new Vector2(Screen.width / 2, Screen.height / 2);
        AddRipple(center);

        yield return new WaitForSeconds(1.25f);

        // フェードアウト処理
        Color baseColor = backGroundSprite.color;
        float startAlphaNow = baseColor.a;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / fadeDuration);

            float newAlpha = Mathf.Lerp(startAlphaNow, endAlpha, t);
            backGroundSprite.color = new Color(baseColor.r, baseColor.g, baseColor.b, newAlpha);
            yield return null;
        }

        // 自分の親オブジェクトを非表示
        if (transform.parent != null)
        {
            transform.parent.gameObject.SetActive(false);
        }
    }
}
