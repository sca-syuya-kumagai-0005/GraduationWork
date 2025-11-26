using UnityEngine;
using System.Collections;

public sealed class Sinner_011_Shot: MonoBehaviour
{

    [Header("=== 対象オブジェクト ===")]
    [SerializeField] private Transform scaleTarget;   // 拡大対象
    [SerializeField] private Transform rotateTarget;  // 回転対象
    [SerializeField] private Transform moveTarget;    // 単体移動対象

    [Header("=== 拡大設定 ===")]
    [SerializeField] private Vector3 endScale = Vector3.one;
    [SerializeField] private float scaleDuration = 1f;
    [SerializeField] private bool scaleEase = true;

    [Header("=== 回転設定 ===")]
    [SerializeField] private Vector3 endRotation = new Vector3(0, 0, 360);
    [SerializeField] private float rotateDuration = 1f;
    [SerializeField] private bool rotateEase = true;

    private Coroutine routine;

    [Header("=== 複数・連続移動設定 ===")]
    [SerializeField] private Transform[] moveObjects;
    [SerializeField] private Vector3[] startPositions;
    [SerializeField] private Vector3[] endPositions;
    [SerializeField] private float multiMoveDuration = 1f;
    [SerializeField] private float moveStaggerDelay = 0.2f;
    [SerializeField] private bool multiMoveEase = true;

    [Header("=== 個別回転設定 ===")]
    [SerializeField] private Transform cylinderRotateTarget;
    [SerializeField] private Vector3 cylinderStartRotation = Vector3.zero;
    [SerializeField] private Vector3 cylinderEndRotation = new Vector3(0, 0, 180);
    [SerializeField] private float cylinderRotateDuration = 1f;
    [SerializeField] private bool cylinderRotateEase = true;

    [Header("=== 単体移動設定 ===")]
    [SerializeField] private Vector3 startPosition = Vector3.zero;
    [SerializeField] private Vector3 endPosition = new Vector3(0, 2, 0);
    [SerializeField] private float moveDuration = 1f;
    [SerializeField] private bool moveEase = true;

    [Header("=== 個別回転設定 ===")]
    [SerializeField] private Transform customRotateTarget;
    [SerializeField] private Vector3 customStartRotation = Vector3.zero;
    [SerializeField] private Vector3 customEndRotation = new Vector3(0, 0, 180);
    [SerializeField] private float customRotateDuration = 1f;
    [SerializeField] private bool customRotateEase = true;

    [Header("=== 拡大設定 ===")]
    [SerializeField] private Vector3 gunEndScale = Vector3.one;
    [SerializeField] private float gunScaleDuration = 1f;
    [SerializeField] private bool gunScaleEase = true;

    [Header("=== 往復回転設定 ===")]
    [SerializeField] private Transform loopRotateTarget;
    [SerializeField] private Vector3 loopStartRotation = Vector3.zero;
    [SerializeField] private Vector3 loopEndRotation = new Vector3(0, 0, 180);
    [SerializeField] private float loopRotateDuration = 1f;
    [SerializeField] private int loopRotateCount = 2;
    [SerializeField] private AnimationCurve loopRotateCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("=== カメラ反動設定 ===")]
    [SerializeField] private Transform recoilCamera;
    [SerializeField] private Vector3 recoilAmount = new Vector3(-5f, 0, 0);
    [SerializeField] private float recoilReturnSpeed = 8f;
    [SerializeField] private AnimationCurve recoilCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("=== 副反動設定 ===")]
    [SerializeField] private Transform subRotateTarget;
    [SerializeField] private Vector3 subStartRotation = Vector3.zero;
    [SerializeField] private Vector3 subAddRotation = new Vector3(0, 0, 30f);
    [SerializeField] private float subRotateDuration = 0.1f;
    [SerializeField] private AnimationCurve subRotateCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("=== 副反動設定 ===")]
    [SerializeField] private Transform subRotateTarget2;
    [SerializeField] private Vector3 subStartRotation2 = Vector3.zero;
    [SerializeField] private Vector3 subAddRotation2 = new Vector3(0, 0, 30f);
    [SerializeField] private float subRotateDuration2 = 0.1f;
    [SerializeField] private AnimationCurve subRotateCurve2 = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [SerializeField] private ParticleSystem mazle;

    // =========================================================
    private void OnEnable()
    {
        if (scaleTarget == null) scaleTarget = transform;
        if (rotateTarget == null) rotateTarget = transform;
        if (moveTarget == null) moveTarget = transform;

        // ===  各ターゲット初期化 ===
        scaleTarget.localScale = Vector3.zero;
        rotateTarget.localRotation = Quaternion.identity;
        moveTarget.localPosition = startPosition;

        // Cylinder回転対象戻す
        if (cylinderRotateTarget != null)
            cylinderRotateTarget.localRotation = Quaternion.Euler(cylinderStartRotation);

        // 単体カスタム回転戻す
        if (customRotateTarget != null)
            customRotateTarget.localRotation = Quaternion.Euler(customStartRotation);

        // 往復ループ回転戻す
        if (loopRotateTarget != null)
            loopRotateTarget.localRotation = Quaternion.Euler(loopStartRotation);

        // 副反動(2つ)初期化
        if (subRotateTarget != null)
            subRotateTarget.localRotation = Quaternion.Euler(subStartRotation);

        if (subRotateTarget2 != null)
            subRotateTarget2.localRotation = Quaternion.Euler(subStartRotation2);

        // カメラ反動リセット
        if (recoilCamera != null)
            recoilCamera.localEulerAngles = Vector3.zero; // もしくは recoilCamera.localEulerAngles = loopStartRotation;

        // 複数移動オブジェクト初期化
        if (moveObjects != null && startPositions != null)
        {
            for (int i = 0; i < moveObjects.Length; i++)
            {
                if (moveObjects[i] != null)
                {
                    moveObjects[i].localPosition =
                        (startPositions.Length > i) ? startPositions[i] : moveObjects[i].localPosition;
                }
            }
        }

        StartAction();
    }

    private void OnDisable()
    {
        // コルーチン全停止
        if (routine != null)
        {
            StopCoroutine(routine);
            routine = null;
        }
        StopAllCoroutines();

    }


    public void StartAction()
    {
        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(ActionRoutine());
    }

    private IEnumerator DisableAfterDelay()
    {
        yield return new WaitForSeconds(5.0f);
        gameObject.SetActive(false);
    }

    private IEnumerator ActionRoutine()
    {
        StartCoroutine(DisableAfterDelay());

        yield return StartCoroutine(ScaleRoutine());
        yield return new WaitForSeconds(0.2f);
        yield return StartCoroutine(RotateRoutine());
        yield return new WaitForSeconds(0.2f);
        yield return StartCoroutine(MultiMoveRoutine());
        yield return new WaitForSeconds(0.2f);
        yield return StartCoroutine(CylinderRotateRoutine());
        yield return new WaitForSeconds(0.2f);
        yield return StartCoroutine(ReverseRotateRoutine());
        yield return new WaitForSeconds(0.1f);

        //Move と FOV 同時
        Coroutine move = StartCoroutine(MoveObjectOnceRoutine());
        Coroutine scale = StartCoroutine(GunScaleRoutine());
        yield return move;
        yield return scale;

        yield return new WaitForSeconds(0.1f);
        yield return StartCoroutine(CustomRotateRoutine());
        yield return new WaitForSeconds(0.2f);
        yield return StartCoroutine(LoopRotateRoutine());
        yield return new WaitForSeconds(0.2f);

        StartCoroutine(ScaleReverseRoutine());
    }

    // =========================================================
    //   Animation Coroutines
    // =========================================================
    private IEnumerator ScaleRoutine()
    {
        float t = 0f;
        while (t < scaleDuration)
        {
            t += Time.deltaTime;
            float n = scaleEase ? Mathf.SmoothStep(0, 1, t / scaleDuration) : t / scaleDuration;
            scaleTarget.localScale = Vector3.Lerp(Vector3.zero, endScale, n);
            yield return null;
        }
        scaleTarget.localScale = endScale;
    }

    private IEnumerator RotateRoutine()
    {
        Quaternion startRot = Quaternion.identity;
        Quaternion endRotQ = Quaternion.Euler(endRotation);
        float t = 0f;

        while (t < rotateDuration)
        {
            t += Time.deltaTime;
            float n = rotateEase ? Mathf.SmoothStep(0, 1, t / rotateDuration) : t / rotateDuration;
            rotateTarget.localRotation = Quaternion.Lerp(startRot, endRotQ, n);
            yield return null;
        }
        rotateTarget.localRotation = endRotQ;
    }

    private IEnumerator MoveObjectOnceRoutine()
    {
        moveTarget.localPosition = startPosition;
        float t = 0f;

        while (t < moveDuration)
        {
            t += Time.deltaTime;
            float n = moveEase ? Mathf.SmoothStep(0, 1, t / moveDuration) : t / moveDuration;
            moveTarget.localPosition = Vector3.Lerp(startPosition, endPosition, n);
            yield return null;
        }
        moveTarget.localPosition = endPosition;
    }

    private IEnumerator MultiMoveRoutine()
    {
        if (moveObjects == null || moveObjects.Length == 0) yield break;

        for (int i = 0; i < moveObjects.Length; i++)
        {
            StartCoroutine(SingleMoveRoutine(i));
            yield return new WaitForSeconds(moveStaggerDelay);
        }
    }

    private IEnumerator CylinderRotateRoutine()
    {
        if (cylinderRotateTarget == null) yield break;

        float startZ = cylinderStartRotation.z;
        float endZ = cylinderEndRotation.z; // 720,1080でもOK
        float t = 0f;

        while (t < cylinderRotateDuration)
        {
            t += Time.deltaTime;
            float n = cylinderRotateEase ? Mathf.SmoothStep(0, 1, t / cylinderRotateDuration) : t / cylinderRotateDuration;

            float z = Mathf.Lerp(startZ, endZ, n);
            cylinderRotateTarget.localRotation = Quaternion.Euler(0, 0, z);

            yield return null;
        }

        cylinderRotateTarget.localRotation = Quaternion.Euler(0, 0, endZ);
    }


    private IEnumerator SingleMoveRoutine(int i)
    {
        Transform obj = moveObjects[i];
        Vector3 startPos = (startPositions.Length > i) ? startPositions[i] : obj.localPosition;
        Vector3 endPos = (endPositions.Length > i) ? endPositions[i] : obj.localPosition;

        obj.localPosition = startPos;
        float t = 0f;

        while (t < multiMoveDuration)
        {
            t += Time.deltaTime;
            float n = multiMoveEase ? Mathf.SmoothStep(0, 1, t / multiMoveDuration) : t / multiMoveDuration;
            obj.localPosition = Vector3.Lerp(startPos, endPos, n);
            yield return null;
        }
        obj.localPosition = endPos;
    }

    private IEnumerator ReverseRotateRoutine()
    {
        Quaternion startRot = rotateTarget.localRotation;
        Quaternion endRot = Quaternion.Euler(Vector3.zero);
        float t = 0f;

        while (t < rotateDuration)
        {
            t += Time.deltaTime;
            float n = rotateEase ? Mathf.SmoothStep(0, 1, t / rotateDuration) : t / rotateDuration;
            rotateTarget.localRotation = Quaternion.Lerp(startRot, endRot, n);
            yield return null;
        }
        rotateTarget.localRotation = endRot;
    }

    private IEnumerator CustomRotateRoutine()
    {
        if (customRotateTarget == null) yield break;

        Quaternion startRot = Quaternion.Euler(customStartRotation);
        Quaternion endRot = Quaternion.Euler(customEndRotation);
        customRotateTarget.localRotation = startRot;
        float t = 0f;

        while (t < customRotateDuration)
        {
            t += Time.deltaTime;
            float n = customRotateEase ? Mathf.SmoothStep(0, 1, t / customRotateDuration) : t / customRotateDuration;
            customRotateTarget.localRotation = Quaternion.Lerp(startRot, endRot, n);
            yield return null;
        }
        customRotateTarget.localRotation = endRot;
    }

    private IEnumerator LoopRotateRoutine()
    {
        if (loopRotateTarget == null) yield break;

        Quaternion startRot = Quaternion.Euler(loopStartRotation);
        Quaternion endRot = Quaternion.Euler(loopEndRotation);
        loopRotateTarget.localRotation = startRot;

        Vector3 camDefaultRot = recoilCamera != null ? recoilCamera.localEulerAngles : Vector3.zero;

        for (int i = 0; i < loopRotateCount; i++)
        {
            // --- 行き ---
            float t1 = 0f;
            while (t1 < loopRotateDuration)
            {
                t1 += Time.deltaTime;
                float n = Mathf.Clamp01(t1 / loopRotateDuration);
                float curve = loopRotateCurve.Evaluate(n);

                loopRotateTarget.localRotation = Quaternion.Lerp(startRot, endRot, curve);

                if (recoilCamera != null)
                {
                    float rc = recoilCurve.Evaluate(n);
                    recoilCamera.localEulerAngles = camDefaultRot + recoilAmount * rc;
                }

                yield return null;
            }
            mazle.Play();
            StartCoroutine(SubRecoilRotate());
            StartCoroutine(SubRecoilRotate2());

            // --- 戻り ---
            float t2 = 0f;
            while (t2 < loopRotateDuration)
            {
                t2 += Time.deltaTime;
                float n = Mathf.Clamp01(t2 / loopRotateDuration);
                float curve = loopRotateCurve.Evaluate(n);

                loopRotateTarget.localRotation = Quaternion.Lerp(endRot, startRot, curve);

                if (recoilCamera != null)
                {
                    recoilCamera.localEulerAngles =
                        Vector3.Lerp(recoilCamera.localEulerAngles, camDefaultRot, Time.deltaTime * recoilReturnSpeed);
                }

                yield return null;
            }

            loopRotateTarget.localRotation = startRot;
            if (recoilCamera != null) recoilCamera.localEulerAngles = camDefaultRot;
        }
    }

    private IEnumerator SubRecoilRotate()
    {
        subRotateTarget.localRotation = Quaternion.Euler(subStartRotation);

        Quaternion start = Quaternion.Euler(subStartRotation);
        Quaternion end = Quaternion.Euler(subStartRotation + subAddRotation);

        float t = 0f;

        while (t < subRotateDuration)
        {
            t += Time.deltaTime;
            float n = subRotateCurve.Evaluate(Mathf.Clamp01(t / subRotateDuration));
            subRotateTarget.localRotation = Quaternion.Lerp(start, end, n);
            yield return null;
        }

        subRotateTarget.localRotation = end;
    }

    private IEnumerator SubRecoilRotate2()
    {
        subRotateTarget2.localRotation = Quaternion.Euler(subStartRotation2);

        Quaternion start = Quaternion.Euler(subStartRotation2);
        Quaternion end = Quaternion.Euler(subStartRotation2 + subAddRotation2);

        float t = 0f;

        while (t < subRotateDuration2)
        {
            t += Time.deltaTime;
            float n = subRotateCurve2.Evaluate(Mathf.Clamp01(t / subRotateDuration2));
            subRotateTarget2.localRotation = Quaternion.Lerp(start, end, n);
            yield return null;
        }

        subRotateTarget2.localRotation = end;
    }

    private IEnumerator ScaleReverseRoutine()
    {
        Vector3 start = endScale;
        Vector3 end = Vector3.zero;

        float t = 0f;
        while (t < scaleDuration)
        {
            t += Time.deltaTime;
            float n = scaleEase ? Mathf.SmoothStep(0, 1, t / scaleDuration) : t / scaleDuration;
            scaleTarget.localScale = Vector3.Lerp(start, end, n);
            yield return null;
        }
        scaleTarget.localScale = end;
    }


    private IEnumerator GunScaleRoutine()
    {
        float t = 0f;

        Vector3 startScale = scaleTarget.localScale;

        while (t < gunScaleDuration)
        {
            t += Time.deltaTime;
            float n = gunScaleEase ? Mathf.SmoothStep(0f, 1f, t / gunScaleDuration) : (t / gunScaleDuration);

            
            scaleTarget.localScale = Vector3.Lerp(startScale, gunEndScale, n);

            yield return null;
        }

        scaleTarget.localScale = gunEndScale;
    }


}
