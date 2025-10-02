using UnityEngine;
using System.Collections;

public class RippleControllerMulti : MonoBehaviour
{
    public Material rippleMaterial;
    private int maxWaves = 10;

    private Vector4[] waveCount;
    private float[] waveStartTimes;
    private int activeCount = 0;

    [Header("�w�i�t�F�[�h�Ώ�")]
    [SerializeField] private SpriteRenderer backGroundSprite; // �X�v���C�g��Color�𑀍�

    [Header("�w�i�t�F�[�h�ݒ�")]
    public float fadeDuration = 1f;      // ���ω��ɂ�����b��
    public float startAlpha = 0.75f;     // �t�F�[�h�C����̓��B���l
    public float endAlpha = 0f;          // �t�F�[�h�A�E�g��̃��l

    void OnEnable()
    {
        // ���񃊃Z�b�g
        waveCount = new Vector4[maxWaves];
        waveStartTimes = new float[maxWaves];
        activeCount = 0;

        // �w�i���������ăX�^�[�g
        if (backGroundSprite != null)
        {
            Color c = backGroundSprite.color;
            backGroundSprite.color = new Color(c.r, c.g, c.b, 0f);
        }

        // �R���[�`���J�n
        StartCoroutine(FadeAndRippleRoutine());
    }

    void OnDisable()
    {
        // �I�u�W�F�N�g�����������ꂽ�Ƃ��ɃR���[�`�����~�߂�
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
        // �w�i�X�v���C�g�̃t�F�[�h�C������
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

        // �t�F�[�h������ɔg�䃋�[�v�J�n
        yield return StartCoroutine(RippleAndFadeOut());
    }

    IEnumerator RippleAndFadeOut()
    {
        int count = 0;

        // �����_���Ȉʒu�ɔg��
        while (count < maxWaves - 1)
        {
            Vector2 randomPos = new Vector2(
                Random.Range(0f, Screen.width),
                Random.Range(0f, Screen.height)
            );

            AddRipple(randomPos);
            count++;
            yield return new WaitForSeconds(0.3f);
        }

        // �Ō�ɒ����ɔg��
        yield return new WaitForSeconds(0.7f);
        Vector2 center = new Vector2(Screen.width / 2, Screen.height / 2);
        AddRipple(center);

        yield return new WaitForSeconds(0.5f);

        // �t�F�[�h�A�E�g����
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

        // �����̐e�I�u�W�F�N�g���\��
        if (transform.parent != null)
        {
            transform.parent.gameObject.SetActive(false);
        }
    }
}
