using System.Collections;
using UnityEngine;

public class TrailController : MonoBehaviour
{
    [Header("=== �Ώېݒ� ===")]
    [SerializeField] private Transform[] targets;          // �ړ��Ώ�
    [SerializeField] private Vector3[] moveDirections;     // �e�Ώۂ̈ړ������i�������������܂ށj
    [SerializeField] private float moveDuration = 1f;      // �ړ��ɂ�����b��

    [Header("=== �X�P�[���ݒ� ===")]
    [SerializeField] private Transform[] scaleTargets;     // �X�P�[���Ώ�
    [SerializeField] private Vector3 targetScale = Vector3.one;  // �ŏI�X�P�[��
    [SerializeField] private float waitAfterMove = 0.5f;         // �ړ���ɑ҂���
    [SerializeField] private float scaleDuration = 0.5f;         // �X�P�[���ɂ����鎞��
    [SerializeField] private float scaleDistance = 0.5f;         //�x��


    private Vector3[] startPositions;
    private Vector3[] endPositions;

    private void Start()
    {
        if (targets == null || targets.Length == 0)
        {
            Debug.LogWarning("targets ���ݒ肳��Ă��܂���B");
            return;
        }

        // moveDirections �̕⊮
        if (moveDirections == null || moveDirections.Length < targets.Length)
        {
            System.Array.Resize(ref moveDirections, targets.Length);
            for (int i = 0; i < moveDirections.Length; i++)
                if (moveDirections[i] == Vector3.zero) moveDirections[i] = Vector3.right;
        }

        // scaleTargets �̕⊮�i�w�肳��Ă��Ȃ��ꍇ�� targets �Ɠ����j
        if (scaleTargets == null || scaleTargets.Length == 0)
        {
            scaleTargets = targets;
        }
        else if (scaleTargets.Length < targets.Length)
        {
            Debug.LogWarning("scaleTargets �̐��� targets ��菭�Ȃ����߁A�����ŕ⊮���܂��B");
            System.Array.Resize(ref scaleTargets, targets.Length);
            for (int i = 0; i < targets.Length; i++)
                if (scaleTargets[i] == null) scaleTargets[i] = targets[i];
        }

        // ���W����
        startPositions = new Vector3[targets.Length];
        endPositions = new Vector3[targets.Length];
        for (int i = 0; i < targets.Length; i++)
        {
            startPositions[i] = targets[i].position;
            endPositions[i] = startPositions[i] + moveDirections[i]; // �� normalized�폜�A�x�N�g���̒������g�p
        }

        StartCoroutine(MoveSequence());
    }

    private IEnumerator MoveSequence()
    {
        yield return new WaitForSeconds(scaleDistance); 
        for (int i = 0; i < targets.Length; i++)
        {
            StartCoroutine(MoveAndScale(targets[i], scaleTargets[i], startPositions[i], endPositions[i]));
        }
        yield return null;
    }

    private IEnumerator MoveAndScale(Transform moveTarget, Transform scaleTarget, Vector3 startPos, Vector3 endPos)
    {
        // === �ړ� ===
        float time = 0f;
        while (time < moveDuration)
        {
            float t = time / moveDuration;
            moveTarget.position = Vector3.Lerp(startPos, endPos, Mathf.SmoothStep(0, 1, t));
            time += Time.deltaTime;
            yield return null;
        }
        moveTarget.position = endPos;

        // === �ҋ@ ===
        yield return new WaitForSeconds(waitAfterMove);

        // === �X�P�[�� ===
        Vector3 initialScale = scaleTarget.localScale;
        float scaleTime = 0f;
        while (scaleTime < scaleDuration)
        {
            float t = scaleTime / scaleDuration;
            scaleTarget.localScale = Vector3.Lerp(initialScale, targetScale, Mathf.SmoothStep(0, 1, t));
            scaleTime += Time.deltaTime;
            yield return null;
        }
        scaleTarget.localScale = targetScale;
    }
}
