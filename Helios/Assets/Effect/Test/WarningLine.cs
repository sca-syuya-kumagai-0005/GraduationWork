using System.Collections;
using UnityEngine;

public class WarningLine : MonoBehaviour
{
    public enum WarningState
    {
        In,
        Out,
        Idle
    }

    [Header("=== Current State ===")]
    public WarningState state = WarningState.In;

    private Coroutine currentProcess;

    [Header("=== First Wave ===")]
    public Transform[] warningLine;
    public Vector3[] startPositionsLine;

    [Header("=== Second Wave ===")]
    public Transform[] warningText;
    public Vector3[] startPositionsText;

    [Header("=== Movement ===")]
    public float[] moveAmounts = new float[2];
    public float duration = 1f;
    public float delayBetween = 0.2f;

    private int movingCount = 0;

    void Start()
    {
        SetStartPositions();
        TryStartProcess();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (state == WarningState.In)
                SetState(WarningState.Out);
            else if (state == WarningState.Out)
                SetState(WarningState.In);
        }
    }

    private void OnValidate()
    {
        if (!Application.isPlaying) return;
        TryStartProcess();
    }

    void TryStartProcess()
    {
        if (currentProcess != null)
            StopCoroutine(currentProcess);

        if (state == WarningState.In)
        {
            SetStartPositions();
            currentProcess = StartCoroutine(ProcessIn());
        }
        else if (state == WarningState.Out)
        {
            currentProcess = StartCoroutine(ProcessOut());
        }
    }

    public void SetState(WarningState newState)
    {
        state = newState;

        // 動作中の全コルーチン完全停止
        StopAllCoroutines();

        // movingCount初期化（これ大事）
        movingCount = 0;

        TryStartProcess();
    }

    public void SetStartPositions()
    {
        for (int i = 0; i < warningLine.Length; i++)
            warningLine[i].position = startPositionsLine[i];

        for (int i = 0; i < warningText.Length; i++)
            warningText[i].position = startPositionsText[i];
    }

    IEnumerator ProcessIn()
    {
        yield return StartCoroutine(MoveWaveLineIn());
        yield return new WaitForSeconds(0.25f);
        MoveWaveTextIn();
        yield return WaitFinish();
        state = WarningState.Idle;
    }

    IEnumerator MoveWaveLineIn()
    {
        Vector2[] directionsA = {
            new Vector2(+1, -1),
            new Vector2(-1, -1),
            new Vector2(-1, +1),
            new Vector2(+1, +1),
        };

        for (int i = 0; i < warningLine.Length; i++)
        {
            Vector3 target =
                warningLine[i].position +
                (Vector3)directionsA[i] * moveAmounts[0];

            movingCount++;
            StartCoroutine(MoveCoroutine(warningLine[i], target));
            yield return new WaitForSeconds(delayBetween);
        }
    }

    void MoveWaveTextIn()
    {
        MoveWaveTextInternal(+1);
    }

    IEnumerator ProcessOut()
    {
        MoveWaveLineOut();
        MoveWaveTextOut();
        yield return WaitFinish();
        state = WarningState.Idle;
    }

    void MoveWaveLineOut()
    {
        Vector2[] directionsA = {
            new Vector2(+1, -1),
            new Vector2(-1, -1),
            new Vector2(-1, +1),
            new Vector2(+1, +1),
        };

        for (int i = 0; i < warningLine.Length; i++)
        {
            Vector3 target =
                warningLine[i].position +
                (Vector3)directionsA[i] * moveAmounts[0] * -1;

            movingCount++;
            StartCoroutine(MoveCoroutine(warningLine[i], target));
        }
    }

    void MoveWaveTextOut()
    {
        MoveWaveTextInternal(-1);
    }

    void MoveWaveTextInternal(int dir)
    {
        Vector2[] directionsB = {
            new Vector2(-1, -1),
            new Vector2(-1, +1),
            new Vector2(+1, +1),
            new Vector2(+1, -1),
        };

        for (int i = 0; i < warningText.Length; i++)
        {
            Vector3 target =
                warningText[i].position +
                (Vector3)directionsB[i] * moveAmounts[1] * dir;

            movingCount++;
            StartCoroutine(MoveCoroutine(warningText[i], target));
        }
    }

    IEnumerator MoveCoroutine(Transform obj, Vector3 target)
    {
        Vector3 start = obj.position;
        float t = 0;

        while (t < duration)
        {
            t += Time.deltaTime;
            obj.position = Vector3.Lerp(start, target, t / duration);
            yield return null;
        }

        obj.position = target;
        movingCount--;
    }

    IEnumerator WaitFinish()
    {
        while (movingCount > 0)
            yield return null;
    }
}
