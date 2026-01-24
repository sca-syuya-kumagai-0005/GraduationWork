using UnityEngine;
using System.Collections;
using System.Linq;

public class Sinner017 : MonoBehaviour
{
    [Header("=== Piero Fade Settings ===")]
    [SerializeField] private SpriteRenderer piero;  // ← ピエロのSpriteRenderer
    [SerializeField] private float startAlpha = 0f; // 開始の透明度
    [SerializeField] private float endAlpha = 1f;   // 終了の透明度
    [SerializeField] private float fadeDuration = 1f; // フェード時間

    [Header("=== Effect Settings ===")]
    [SerializeField] private GameObject[] effects;


    [Header("=== Move Settings ===")]
    [SerializeField] private Transform outLine;
    [SerializeField] private Transform[] pathPoints;
    [SerializeField] private float moveDuration = 1f;
    [SerializeField] private float totalRunTime = 10f;

    [Header("=== Blink Settings ===")]
    [SerializeField] private float blinkInterval = 0.2f;
    [SerializeField] private int blinkCount = 4;

    [Header("=== Rotation Settings ===")]
    [SerializeField] private float rotateDuration = 1f;
    [SerializeField] private float startAngle = 0f;
    [SerializeField] private float endAngle = 360f;

    [SerializeField] private float waitAfterFirstRotation = 0.5f;  // 追加

    private bool isStopped = false;
    private Transform lastStoppedTarget;

    [SerializeField] private GameObject endEffects;
    [SerializeField]
    private GameObject piero1;

    [SerializeField] private AudioManager audioManager;
    [SerializeField] private AudioClip sinner_017;

    void Update()
    {
        if (Input.GetMouseButtonDown(2))
            isStopped = true;
    }


    public void OnEnable()
    {
        audioManager = GameObject.Find("Audio").GetComponent<AudioManager>();
        audioManager.PlaySE(sinner_017);
        isStopped = false;

        // リセット処理
        piero1.gameObject.SetActive(true);

        // OutLine を pathPoints[0] に合わせる
        if (pathPoints != null && pathPoints.Length > 0)
        {
            outLine.position = pathPoints[0].position;
        }

        // エフェクト非表示
        foreach (var fx in effects)
        {
            if (fx != null)
                fx.SetActive(false);
        }

        // pathPoints のオブジェクト非表示
        foreach (var p in pathPoints)
        {
            if (p != null)
                p.gameObject.SetActive(false);
            p.localRotation = Quaternion.Euler(0, 0, 0);
        }

        // OutLine 自体も非表示
        outLine.gameObject.SetActive(false);



        // 元の処理（内容は一切変更なし)
        StartCoroutine(FadePiero());
        SetRandomWinLose();
        StartCoroutine(MoveLoop());
    }

    public void OnDisable()
    {
        isStopped = true;
    }



    public void SetRandomWinLose()
    {
        // 念のため全部非表示にする
        foreach (var p in pathPoints)
        {
            Transform win = p.Find("CardIcon_Win");
            Transform lose = p.Find("CardIcon_Lose");

            if (win) win.gameObject.SetActive(false);
            if (lose) lose.gameObject.SetActive(false);
        }

        // 3つをランダムにシャッフル
        Transform[] shuffled = pathPoints.OrderBy(x => Random.value).ToArray();

        // 最初の2つをWINにする（当たり）
        for (int i = 0; i < 2; i++)
        {
            Transform win = shuffled[i].Find("CardIcon_Win");
            if (win) win.gameObject.SetActive(true);
        }

        // 最後の1つをLOSEにする（はずれ）
        Transform loseObj = shuffled[2].Find("CardIcon_Lose");
        if (loseObj) loseObj.gameObject.SetActive(true);
        endEffects.SetActive(false);
    }

    private IEnumerator FadePiero()
    {
        float timer = 0f;

        Color c = piero.color;
        c.a = startAlpha;
        piero.color = c;

        while (timer < fadeDuration)
        {
            float t = timer / fadeDuration;
            float a = Mathf.Lerp(startAlpha, endAlpha, t);

            c.a = a;
            piero.color = c;

            timer += Time.deltaTime;
            yield return null;
        }

        // 最終値をセット
        c.a = endAlpha;
        piero.color = c;
        yield return new WaitForSeconds(0.15f);

        // フェード完了後の処理（ここから)

        // エフェクト全表示
        foreach (var fx in effects)
        {
            if (fx != null)
                fx.SetActive(true);
        }

        yield return new WaitForSeconds(0.2f);

        // pathPoints のオブジェクト全表示
        foreach (var p in pathPoints)
        {
            if (p != null)
                p.gameObject.SetActive(true);
        }
        outLine.gameObject.SetActive(true);
    }



    private IEnumerator MoveLoop()
    {
        yield return new WaitForSeconds(1.25f);
        float timer = 0f;
        int index = 0;

        while (timer < totalRunTime)
        {
            if (isStopped)
                break;

            outLine.position = pathPoints[index].position;

            timer += moveDuration;
            yield return new WaitForSeconds(moveDuration);

            index = (index + 1) % pathPoints.Length;
        }

        lastStoppedTarget = GetClosestTarget();

        // --- WIN / LOSE 判定 ---
        Transform win = lastStoppedTarget.Find("CardIcon_Win");
        Transform lose = lastStoppedTarget.Find("CardIcon_Lose");

        if (win != null && win.gameObject.activeSelf)
            Debug.Log("あたり");
        else if (lose != null && lose.gameObject.activeSelf)
            Debug.Log("はずれ");

        // --- 点滅 ---
        yield return StartCoroutine(BlinkOutLine());

        // --- 当たった1つだけ回転 ---
        StartCoroutine(RotateObject(lastStoppedTarget, rotateDuration));
        yield return StartCoroutine(RotateObject(outLine, rotateDuration));

        // --- 待機 ---
        yield return new WaitForSeconds(waitAfterFirstRotation);

        // --- 他の全オブジェクトを回転 → 完了するまで待つ ---
        yield return StartCoroutine(RotateAllOthers(lastStoppedTarget));
        yield return new WaitForSeconds(.5f);

        // --- 全部回転完了後にエフェクト ---
        endEffects.SetActive(true);
        yield return new WaitForSeconds(0.2f);

        piero1.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }

    private IEnumerator RotateAllOthers(Transform excludeTarget)
    {
        float halfDuration = rotateDuration / 2f;

        // コルーチン管理配列
        Coroutine[] routines = new Coroutine[pathPoints.Length];
        int count = 0;

        foreach (var p in pathPoints)
        {
            if (p != excludeTarget)
            {
                routines[count] = StartCoroutine(RotateObject(p, halfDuration));
                count++;
            }
        }

        // rotateDuration/2f の間待てば全員終わる
        yield return new WaitForSeconds(halfDuration);
    }


    private IEnumerator BlinkOutLine()
    {
        for (int i = 0; i < blinkCount; i++)
        {
            outLine.gameObject.SetActive(false);
            yield return new WaitForSeconds(blinkInterval);

            outLine.gameObject.SetActive(true);
            yield return new WaitForSeconds(blinkInterval);
        }
        outLine.gameObject.SetActive(true);

    }


    private Transform GetClosestTarget()
    {
        Transform nearest = null;
        float minDist = float.MaxValue;

        foreach (var p in pathPoints)
        {
            float d = Vector3.Distance(outLine.position, p.position);
            if (d < minDist)
            {
                minDist = d;
                nearest = p;
            }
        }
        return nearest;
    }


    // 指定時間でY回転する汎用版
    private IEnumerator RotateObject(Transform target, float duration)
    {
        float timer = 0f;

        while (timer < duration)
        {
            float t = timer / duration;
            float angle = Mathf.Lerp(startAngle, endAngle, t);

            target.localRotation = Quaternion.Euler(0, angle, 0);

            timer += Time.deltaTime;
            yield return null;
        }

        target.localRotation = Quaternion.Euler(0, endAngle, 0);


    }
}
