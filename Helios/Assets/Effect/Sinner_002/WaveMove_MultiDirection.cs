using System.Collections;
using UnityEngine;

/// <summary>
/// ���݂̈ʒu���珇�ԂɁu���΋����v�Ŕg�ł��Ȃ���ړ�����X�N���v�g
/// </summary>
public class WaveMove_MultiPositions : MonoBehaviour
{
    [Header("=== �o�H�ݒ� ===")]
    [Tooltip("���ԂɈړ����鑊�΋����i���[�J�����W�n�j")]
    public Vector3[] moveOffsets;

    [Header("=== �g�ړ��ݒ� ===")]
    [Tooltip("�g�̐U���i�㉺���j")]
    public float waveAmplitude = 0.5f;

    [Tooltip("�g�̉񐔁i1�ړ����̏㉺�񐔁j")]
    public int waveCount = 3;

    [Tooltip("�g�̊J�n�ʑ��i�x���@�j\n0=���S, 90=�ォ��, 270=������J�n")]
    [Range(0f, 360f)] public float waveStartPhase = 90f;

    [Tooltip("1��Ԃ�����̈ړ����ԁi�b�j")]
    public float moveDuration = 2.0f;

    [Tooltip("�J�n�܂ł̒x������")]
    public float startDelay = 0f;

    [Tooltip("�ړ��̊ɋ}�iEase�J�[�u�j")]
    public AnimationCurve ease = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Tooltip("�Ō�܂ňړ�������ŏ��ɖ߂邩")]
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

        // �o���_�͌��݈ʒu�i���[���h���W�j
        Vector3 currentPos = transform.position;

        do
        {
            for (int i = 0; i < moveOffsets.Length; i++)
            {
                Vector3 startPos = currentPos;
                Vector3 targetPos = startPos + moveOffsets[i];

                // �i�s�����Ɣg�̐�������
                Vector3 dir = (targetPos - startPos).normalized;
                Vector3 perpendicular = Vector3.Cross(dir, Vector3.forward);

                float elapsed = 0f;
                while (elapsed < moveDuration)
                {
                    elapsed += Time.deltaTime;
                    float t = Mathf.Clamp01(elapsed / moveDuration);
                    float easedT = ease.Evaluate(t);

                    // ���`���
                    Vector3 pos = Vector3.Lerp(startPos, targetPos, easedT);

                    // �g�ł��ǉ�
                    float phaseRad = waveStartPhase * Mathf.Deg2Rad;
                    float wave = Mathf.Sin(t * Mathf.PI * 2f * waveCount + phaseRad) * waveAmplitude;
                    pos += perpendicular * wave;

                    transform.position = pos;
                    yield return null;
                }

                // �I�_�␳
                transform.position = targetPos;
                currentPos = targetPos;
            }

            if (loop)
                currentPos = transform.position; // ���݈ʒu����܂������[�v��

        } while (loop);
    }
}
