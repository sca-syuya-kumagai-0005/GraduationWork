using UnityEngine;
using System.Collections;

public class MouseCheck : MonoBehaviour
{
    public enum TimeState
    {
        Morning,
        Noon,
        Night,
        Warning
    }

    [Header("State")]
    public TimeState currentState;
    public TimeState targetState; // 目標状態

    [Header("Movement Settings")]
    [SerializeField] private Transform target;
    [SerializeField] private float speed = 2f;
    [SerializeField] private Vector3 startPos = new Vector3(0, 9.5f, 0);
    [SerializeField] private Vector3 endPos = new Vector3(0, 7f, 0);
    [SerializeField] private GameObject[] icons;

    [Header("Rotation Time Settings")]
    [SerializeField] private float timeFor90 = 1f;
    [SerializeField] private float timeFor180 = 2f;
    [SerializeField] private float timeFor270 = 3f;

    private bool isMouseOver = false;
    private bool isRotating = false;

    private void Update()
    {
        MoveObject();

        // 自動回転監視
        if (!isRotating && currentState != targetState)
        {
            StartCoroutine(RotateTo(targetState));
        }
    }

    private void OnMouseEnter()
    {
        isMouseOver = true;
    }

    private void OnMouseExit()
    {
        isMouseOver = false;
    }

    private void MoveObject()
    {
        // マウスが乗っていて、かつマウス操作が一切されていない時だけ降りる
        bool canMoveDown = isMouseOver && !IsAnyMouseInput();

        Vector3 targetPos = canMoveDown ? endPos : startPos;

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPos,
            speed * Time.deltaTime);
    }


    private IEnumerator RotateTo(TimeState next)
    {
        isRotating = true;

        float startZ = target.localEulerAngles.z;
        float endZ = 45f + (int)next * 90f;

        // 右回り固定 (時計回り)
        float angleDiff = endZ - startZ;
        if (angleDiff < 0) angleDiff += 360f; // 右回り方向に補正

        float absDiff = Mathf.Abs(angleDiff);

        // 角度差によって回転時間を指定
        float duration = 1f;
        if (Mathf.Approximately(absDiff, 90f)) duration = timeFor90;
        else if (Mathf.Approximately(absDiff, 180f)) duration = timeFor180;
        else if (Mathf.Approximately(absDiff, 270f)) duration = timeFor270;

        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / duration);

            float z = Mathf.Lerp(startZ, startZ + angleDiff, t);
            target.localRotation = Quaternion.Euler(0, 0, z);

            // アイコンも逆回転
            for (int i = 0; i < icons.Length; i++)
            {
                icons[i].transform.localRotation = Quaternion.Euler(0, 0, -z);
            }

            yield return null;
        }

        target.localRotation = Quaternion.Euler(0, 0, startZ + angleDiff);
        currentState = next;
        isRotating = false;
    }



    private void RotateIcons(float parentZ)
    {
        for (int i = 0; i < icons.Length; i++)
        {
            icons[i].transform.localRotation = Quaternion.Euler(0, 0, -parentZ);
        }
    }

    private bool IsAnyMouseInput()
    {
        return Input.GetMouseButton(0) ||   // 左クリック
               Input.GetMouseButton(1) ||   // 右クリック
               Input.GetMouseButton(2) ||   // 中クリック
               Mathf.Abs(Input.mouseScrollDelta.y) > 0f; // ホイール
    }

}
