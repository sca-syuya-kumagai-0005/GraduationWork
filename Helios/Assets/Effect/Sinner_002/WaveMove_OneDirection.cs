using System.Collections;
using UnityEngine;
public class WaveMove_OneDirection : MonoBehaviour
{
    [Header("=== �g�ړ��ݒ� ===")]
    [Tooltip("�ړ������i��F�E�����Ȃ� (1,0,0)�j")]
    public Vector3 moveDirection = Vector3.right;

    [Tooltip("�i�s�����ɑ΂��Đ����Ȕg�̍���")]
    public float waveAmplitude = 0.5f;

    [Tooltip("�g�̉񐔁i�㉺�񐔁j")]
    public int waveCount = 3;

    [Tooltip("�S�̂̈ړ������i�i�s�����̍��v�����j")]
    public float moveDistance = 5.0f;

    [Tooltip("�ړI�n�ɓ��B����܂ł̎��ԁi�b�j")]
    public float moveDuration = 2.0f;

    [Tooltip("�J�n�܂ł̒x������")]
    public float startDelay = 0f;

    [Tooltip("�ړ��̊ɋ}�iEase�J�[�u�j")]
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

            // ������̈ړ�
            float forwardMove = Mathf.Lerp(0f, moveDistance, easedT);

            // �w��񐔕��̔g�𐶐��i�gCount��Sin�g�̉񐔂�����j
            float wave = Mathf.Sin(t * Mathf.PI * 2f * waveCount) * waveAmplitude;

            // �������������߂�idir�ɑ΂��Ē��p�����j
            Vector3 perpendicular = new Vector3(-dir.y, dir.x, 0f);

            // ����
            Vector3 pos = startPos + dir * forwardMove + perpendicular * wave;
            transform.localPosition = pos;

            yield return null;
        }

        // �ŏI�ʒu�ɕ␳
        transform.localPosition = startPos + dir * moveDistance;
    }
}
