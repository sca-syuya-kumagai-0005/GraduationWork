using UnityEngine;
using System.Collections;

public class Sinner_021 : MonoBehaviour
{
    public enum WENS
    {
        N,
        E,
        S,
        W
    }

    [Header("Effect")]
    [SerializeField] private GameObject hit;
    [SerializeField] private GameObject sparks;
    [SerializeField] private GameObject electrifiled;
    [SerializeField] private GameObject[] compassHit;

    [Header("Move")]
    [SerializeField] private Transform moveTarget;
    [SerializeField] private Vector3 startPos;
    [SerializeField] private Vector3 endPos;
    [SerializeField] private float moveDuration = 1f; //  移動時間

    [Header("Rotate")]
    [SerializeField] private Transform rotateTarget;
    [SerializeField] private WENS direction;
    public WENS Direction { set { direction = value; } }
    [SerializeField] private float baseAngle = 43f;
    [SerializeField] private Vector3 rotateAngle;
    [SerializeField] private float rotateDuration = 1f; //  回転時間

    [Header("Final Move (by WENS)")]
    [SerializeField] private Vector3 finalStart_N;
    [SerializeField] private Vector3 finalEnd_N;

    [SerializeField] private Vector3 finalStart_E;
    [SerializeField] private Vector3 finalEnd_E;

    [SerializeField] private Vector3 finalStart_S;
    [SerializeField] private Vector3 finalEnd_S;

    [SerializeField] private Vector3 finalStart_W;
    [SerializeField] private Vector3 finalEnd_W;

    [SerializeField] private float finalMoveDuration = 1f;

    [Header("Final Scale")]
    [SerializeField] private Vector3 finalScale = Vector3.one * 3f;

    
    int count = 0;


    private float currentZ;

    void OnEnable()
    {
        if (rotateTarget != null)
            rotateTarget.localRotation = Quaternion.Euler(0f, 0f, 226.5f);

        if (moveTarget != null)
            moveTarget.localScale = Vector3.one;

        count = 0;

        hit.SetActive(false);
        sparks.SetActive(false);
        electrifiled.SetActive(false);

        for (int i = 0; i < compassHit.Length; i++)
            compassHit[i].SetActive(false);

        if (rotateTarget != null)
            currentZ = rotateTarget.eulerAngles.z;

        StartCoroutine(Animation());
    }


    IEnumerator Animation()
    {
        yield return StartCoroutine(MoveRoutine());

        electrifiled.SetActive(true);
        yield return new WaitForSeconds(1f);

        sparks.SetActive(true);
        hit.SetActive(true);

        yield return StartCoroutine(RotateRoutine());

        yield return new WaitForSeconds(0.5f);
        // すべて終了後、WENSに応じた最終移動
        yield return StartCoroutine(FinalMoveRoutine());
    }


    //====================
    // Move
    //====================
    IEnumerator MoveRoutine()
    {
        float time = 0f;
        Vector3 start = startPos;

        while (time < moveDuration)
        {
            float t = time / moveDuration;
            moveTarget.position = Vector3.Lerp(start, endPos, t);
            time += Time.deltaTime;
            yield return null;
        }

        moveTarget.position = endPos;
    }

    IEnumerator RotateRoutine()
    {
        float time = 0f;

        float startZ = currentZ;
        float targetZ = startZ + GetTargetZDelta(); //  加算回転

        while (time < rotateDuration)
        {
            float t = time / rotateDuration;

            float z = Mathf.Lerp(startZ, targetZ, t);

            // Z軸のみ回転（X/Y完全固定）
            rotateTarget.rotation = Quaternion.Euler(0f, 0f, z);

            time += Time.deltaTime;
            yield return null;
        }

        currentZ = targetZ; //  累積更新
        rotateTarget.rotation = Quaternion.Euler(0f, 0f, currentZ);
    }


    float GetTargetZDelta()
    {
        float z = baseAngle;

        switch (direction)
        {
            case WENS.N: z += 0f; break;
            case WENS.E: z += 90f; break;
            case WENS.S: z += 180f; break;
            case WENS.W: z += 270f; break;
        }

        return z + rotateAngle.z; // 360超えOK
    }

    IEnumerator FinalMoveRoutine()
    {
        Vector3 start;
        Vector3 end;

        switch (direction)
        {
            case WENS.N:
                start = finalStart_N;
                end = finalEnd_N;
                count = 0; 
                break;
            case WENS.E:
                start = finalStart_E;
                end = finalEnd_E;
                count = 1;
                break;
            case WENS.S:
                start = finalStart_S;
                end = finalEnd_S;
                count = 2;
                break;
            case WENS.W:
                start = finalStart_W;
                end = finalEnd_W;
                count = 3;
                break;
            default:
                yield break;
        }

        float time = 0f;

        Vector3 startScale = moveTarget.localScale;
        Vector3 endScale = finalScale;

        while (time < finalMoveDuration)
        {
            float t = time / finalMoveDuration;

            // 位置移動
            moveTarget.position = Vector3.Lerp(start, end, t);

            // サイズ変更
            moveTarget.localScale = Vector3.Lerp(startScale, endScale, t);

            time += Time.deltaTime;
            yield return null;
        }

        // 最終保証
        moveTarget.position = end;
        moveTarget.localScale = endScale;

        yield return new WaitForSeconds(0.5f);

        compassHit[count].SetActive(true);

        yield return new WaitForSeconds(0.5f);

        this.gameObject.SetActive(false);

    }


}
