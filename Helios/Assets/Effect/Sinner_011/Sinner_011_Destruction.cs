using UnityEngine;
using System.Collections;

public sealed class Sinner_011_Destruction : MonoBehaviour
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

    [SerializeField] private GameObject boom;
    [SerializeField] private GameObject explosion;

    [Header("=== scaleTarget回転設定 ===")]
    [SerializeField] private Vector3 scaleStartRotation = Vector3.zero;
    [SerializeField] private Vector3 scaleEndRotation = new Vector3(0, 0, 180);
    [SerializeField] private float scaleRotateDuration = 1f;
    [SerializeField] private bool scaleRotateEase = true;
    [SerializeField] private float scaleRotateReturnDelay = 0.3f;
    [SerializeField] private float scaleRotateReturnDuration = 1f;

    [Header("=== scaleTarget位置移動設定 ===")]
    [SerializeField] private Vector3 scaleStartPosition = Vector3.zero;
    [SerializeField] private Vector3 scaleEndPosition = new Vector3(0, 1, 0);
    [SerializeField] private float scaleMoveDuration = 1f;
    [SerializeField] private bool scaleMoveEase = true;
    [SerializeField] private float scaleMoveReturnDelay = 0.3f;
    [SerializeField] private float scaleMoveReturnDuration = 1f;

    // 爆発時Cylinder用設定
    [Header("=== Cylinder 爆発設定 ===")]
    [SerializeField] private float explodeForce = 8f;
    [SerializeField] private float explodeUpForce = 2f;
    [SerializeField] private float explodeRadius = 2f;
    [SerializeField] private float cylinderLifeTime = 3f;
    [SerializeField] private float cylinderTorqueStrength = 10f; // 回転の強さ

    private Rigidbody cylinderRb;
    [Header("=== 爆発後復元設定 ===")]
    [SerializeField] private Transform cylinderOriginalParent;  // 復元先親
    [SerializeField] private Vector3 postExplosionLocalPosition = Vector3.zero;
    [SerializeField] private Vector3 postExplosionLocalRotation = Vector3.zero;

    [SerializeField] private AudioManager audioManager;
    [SerializeField] private AudioClip sinner_011Boom;

    private void OnEnable()
    {

        // 爆発前の親とローカル座標を復元
        if (cylinderRotateTarget != null)
        {
            if (cylinderOriginalParent != null)
                cylinderRotateTarget.SetParent(cylinderOriginalParent);

            cylinderRotateTarget.localPosition = postExplosionLocalPosition;
            cylinderRotateTarget.localRotation = Quaternion.Euler(postExplosionLocalRotation);
        }

        if (scaleTarget == null) scaleTarget = transform;
        if (rotateTarget == null) rotateTarget = transform;
        if (moveTarget == null) moveTarget = transform;

        // Transform 初期化
        scaleTarget.localScale = Vector3.zero;
        rotateTarget.localRotation = Quaternion.identity;
        moveTarget.localPosition = startPosition;
        scaleTarget.localRotation = Quaternion.identity;
        if (cylinderRotateTarget != null)
        {
            cylinderRotateTarget.localRotation = Quaternion.Euler(cylinderStartRotation);
            cylinderOriginalParent = cylinderRotateTarget.parent;

            cylinderRb = cylinderRotateTarget.GetComponent<Rigidbody>();
            if (cylinderRb == null)
                cylinderRb = cylinderRotateTarget.gameObject.AddComponent<Rigidbody>();

            cylinderRb.linearVelocity = Vector3.zero;
            cylinderRb.angularVelocity = Vector3.zero;
            cylinderRb.useGravity = false; // ←普段は重力オフ
            cylinderRb.mass = 0.2f;
        }

        if (customRotateTarget != null)
            customRotateTarget.localRotation = Quaternion.Euler(customStartRotation);

        if (moveObjects != null && startPositions != null)
        {
            for (int i = 0; i < moveObjects.Length; i++)
            {
                if (moveObjects[i] != null)
                    moveObjects[i].localPosition = (startPositions.Length > i) ? startPositions[i] : moveObjects[i].localPosition;
            }
        }

        boom.SetActive(false);
        explosion.SetActive(false);
        audioManager = GameObject.Find("Audio").GetComponent<AudioManager>();
        audioManager.PlaySE(sinner_011Boom);
        StartAction();
    }

    private void OnDisable()
    {
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
        yield return new WaitForSeconds(0.15f);
        yield return StartCoroutine(MultiMoveRoutine());
        yield return new WaitForSeconds(0.1f);
        yield return StartCoroutine(CylinderRotateRoutine());
        yield return new WaitForSeconds(0.1f);
        yield return StartCoroutine(ReverseRotateRoutine());
        yield return new WaitForSeconds(0.1f);
        yield return StartCoroutine(CustomRotateRoutine());
        yield return new WaitForSeconds(0.2f);

        Coroutine move = StartCoroutine(MoveObjectOnceRoutine());
        Coroutine scale = StartCoroutine(GunScaleRoutine());
        yield return scale;
        yield return move;

        yield return new WaitForSeconds(0.5f);

        StartCoroutine(CylinderExplodeRoutine());
        StartCoroutine(ScaleRotateRoutine());
        StartCoroutine(CustomRotateReverseRoutine());
    }

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
        float endZ = cylinderEndRotation.z;
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

    private IEnumerator CustomRotateReverseRoutine()
    {
        if (customRotateTarget == null) yield break;

        Quaternion startRot = Quaternion.Euler(customStartRotation);
        Quaternion endRot = Quaternion.Euler(customEndRotation);

        // 今度は end から start に回す
        customRotateTarget.localRotation = endRot;
        float t = 0f;

        while (t < customRotateDuration)
        {
            t += Time.deltaTime;
            float n = customRotateEase ? Mathf.SmoothStep(0, 1, t / customRotateDuration) : t / customRotateDuration;
            customRotateTarget.localRotation = Quaternion.Lerp(endRot, startRot, n);
            yield return null;
        }
        customRotateTarget.localRotation = startRot;
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

    private IEnumerator ScaleRotateRoutine()
    {
        Quaternion startRot = Quaternion.Euler(scaleStartRotation);
        Quaternion endRot = Quaternion.Euler(scaleEndRotation);
        Vector3 startPos = scaleStartPosition;
        Vector3 endPos = scaleEndPosition;

        scaleTarget.localRotation = startRot;
        scaleTarget.localPosition = startPos;

        float t = 0f;

        while (t < scaleRotateDuration)
        {
            t += Time.deltaTime;

            float n = scaleRotateEase
                ? Mathf.SmoothStep(0f, 1f, t / scaleRotateDuration)
                : (t / scaleRotateDuration);
            scaleTarget.localRotation = Quaternion.Lerp(startRot, endRot, n);

            float nm = scaleMoveEase
                ? Mathf.SmoothStep(0f, 1f, t / scaleMoveDuration)
                : (t / scaleMoveDuration);
            scaleTarget.localPosition = Vector3.Lerp(startPos, endPos, nm);

            yield return null;
        }

        scaleTarget.localRotation = endRot;
        scaleTarget.localPosition = endPos;
    }

    private IEnumerator CylinderExplodeRoutine()
    {
        if (cylinderRotateTarget == null || cylinderRb == null) yield break;

        cylinderRotateTarget.SetParent(this.transform);

        cylinderRb.linearVelocity = Vector3.zero;
        cylinderRb.angularVelocity = Vector3.zero;

        cylinderRb.useGravity = true; // ←爆発時だけ重力オン

        Vector3 left = -Vector3.right;
        Vector2 randomCircle = Random.insideUnitCircle.normalized;
        Vector3 randomDir = new Vector3(randomCircle.x, Random.Range(-0.2f, 0.5f), randomCircle.y);
        Vector3 finalDir = (left * 0.7f + randomDir * 0.3f).normalized;

        cylinderRb.AddForce(finalDir * explodeForce + Vector3.up * explodeUpForce, ForceMode.Impulse);

        Vector3 randomTorque = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
        ).normalized * cylinderTorqueStrength;
        cylinderRb.AddTorque(randomTorque, ForceMode.Impulse);

        boom.SetActive(true);
        explosion.SetActive(true);

        yield return new WaitForSeconds(cylinderLifeTime);

        cylinderRotateTarget.SetParent(cylinderOriginalParent);
        cylinderRb.useGravity = false; // ←終了後は重力オフに戻す
    }


}
