using UnityEngine;
using System.Collections;

public class DissolveController : MonoBehaviour
{
    [Header("�Ώۃ}�e���A��")]
    public Material dissolveMaterial;

    [Header("���ʐݒ�")]
    public float startDelay = 0f;  // �� ���ʂ̒x��

    [Header("Dissolve�ݒ�")]
    public float startDissolve = 0f;
    public float endDissolve = 1f;
    public float dissolveDuration = 2f;

    [Header("EdgeWidth�ݒ�")]
    public float startEdgeWidth = 0.05f;
    public float endEdgeWidth = 0.2f;
    public float edgeWidthDuration = 2f;

    [Header("TextDissolve�ݒ�")]
    public float startTextDissolve = 1f;
    public float endTextDissolve = 0f;
    public float dissolveTextDuration = 1f;

    [Header("��]�Ώ�")]
    [SerializeField] private Transform targetObject; // �p�x�𑀍삷��Ώ�

    [Header("��]�ݒ�")]
    public float rotateDuration = 1f;   // ��]�ɂ�����b��
    public float startAngle = 0f;       // ��]�J�n�p�x�iY���j
    public float endAngle = 180f;       // ��]�I���p�x�iY���j

    [Header("�e�L�X�g")]
    [SerializeField] private SpriteRenderer nameText_007;

    private Coroutine mainRoutine;

    private void OnEnable()
    {
        // �����l���Z�b�g
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

        // ���s�J�n
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
        // 1. Start �� End �։�]
        yield return Rotate(startAngle, endAngle, rotateDuration);

        // 2. Dissolve �� EdgeWidth �� TextDissolve �𓯎��ɃX�^�[�g
        yield return RunDissolveAndEdge();

        // 3. �����҂��Ă���AEnd �� Start �։�]���Ė߂�
        yield return new WaitForSeconds(0.25f);
        yield return Rotate(endAngle, startAngle, rotateDuration);

        // 4. ���ׂďI������玩�����A�N�e�B�u��
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

        // �S���I���܂ő҂�
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
