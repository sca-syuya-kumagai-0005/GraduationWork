using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UnmaskScale : MonoBehaviour
{
    [Header("Scale")]
    [Tooltip("現在のスケールに対する拡大倍率")]
    public float scaleMultiplier = 1.2f;

    [Header("Time")]
    public float delay = 5f;
    public float duration = 1f;

    [Header("Step Evaluation")]
    public GameObject[] targets;
    public float evaluateInterval = 15f;

    [SerializeField] private Image[] clickBlock;

    [Header("Mask Follow")]
    [SerializeField] private Image unMask;
    [SerializeField] private Transform character;
    private Transform house;    // Sprite
    private Transform load;    // Sprite
    [SerializeField] private Canvas canvas;
    [SerializeField] private Camera targetCamera;
    [SerializeField] private Sprite[] maskSprite;

    private Coroutine scaleCoroutine;
    private Coroutine evaluateCoroutine;
    private Coroutine followCoroutine;
    private Coroutine followLoad;

    private int currentIndex = 0;

    int count = 0;
    int count1 = 0;
    int count2 = 0;
    //========================
    // MG側から呼ぶ開始API
    //========================
    public void StartByState(TutorialMG.TutorialState state)
    {
        StopAll(); // 念のため全部止める
        for (int i = 0; i < clickBlock.Length; i++)
        {
            clickBlock[i].enabled = false;
        }
        switch (state)
        {
            case TutorialMG.TutorialState.Click:
                unMask.sprite = maskSprite[count]; 
                house = GameObject.Find("9_26_52").transform;
                unMask.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
                followCoroutine = StartCoroutine(FollowHouse());
                scaleCoroutine = StartCoroutine(ScaleLoop());
                break;

            case TutorialMG.TutorialState.DeliveryExplanation:
                unMask.sprite = maskSprite[1];
                unMask.transform.position = new Vector3(0f, 25f, 925f);
                unMask.transform.localScale = new Vector3(4.6f, 1.25f, 1f);
                if (clickBlock[0].enabled)
                    clickBlock[0].enabled = false;
                else
                    clickBlock[0].enabled = true;


                break;

            case TutorialMG.TutorialState.DocumentCheck:
                unMask.sprite = maskSprite[0];
                UnMaskToWorld();
                unMask.transform.position = new Vector3(-570f, -450f, 925f);
                unMask.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
                scaleCoroutine = StartCoroutine(ScaleLoop());
                evaluateCoroutine = StartCoroutine(EvaluateCoroutine());
                break;

            case TutorialMG.TutorialState.OpenDocument:
                count++;
                switch(count)
                {
                    case 1:
                        unMask.sprite = unMask.sprite = maskSprite[2];
                        clickBlock[count].enabled = true;
                        character.position = new Vector3(0f, -350f, 925f);
                        unMask.transform.position = new Vector3(-740f, -56f, 925f);
                        unMask.transform.localScale = new Vector3(1.25f, 4.1f, 1f);

                        break;
                    case 2:
                        clickBlock[count].enabled = true;
                        character.position = new Vector3(150f, -350f, 925f);
                        unMask.transform.position = new Vector3(-360f, 32.5f, 925f);
                        unMask.transform.localScale = new Vector3(2.75f, 2.5f, 1f);
                        break;

                    case 3:
                        clickBlock[2].enabled = false;
                        unMask.transform.position = new Vector3(-360f, 0f, 925f);
                        unMask.transform.localScale = new Vector3(2.75f, 0.35f, 1f);
                        break;

                    case 4:
                       
                        character.position = new Vector3(150f, -350f, 925f);
                        break;
                    case 5:
                        character.position = new Vector3(-750f, -350f, 925f);
                        unMask.transform.position = new Vector3(437f, 440f, 925f);
                        unMask.transform.localScale = new Vector3(2.8f, 0.3f, 1f);
                        break;
                    case 6:
                        unMask.transform.position = new Vector3(435f, 358f, 925f);
                        unMask.transform.localScale = new Vector3(2.75f, 0.6f, 1f);
                        break;
                    case 7:
                        unMask.transform.position = new Vector3(380f, -87f, 925f);
                        unMask.transform.localScale = new Vector3(3.75f, 4.4f, 1f);
                        break;
                    case 8:
                        unMask.transform.position = new Vector3(212f, 6f, 925f);
                        unMask.transform.localScale = new Vector3(1.9f, 1.05f, 1f);
                        break;
                    case 9:
                        clickBlock[3].enabled = true;
                        unMask.sprite = maskSprite[0];
                        unMask.transform.position = new Vector3(780f, -14.5f, 925f);
                        unMask.transform.localScale = new Vector3(0.7f, 0.7f, 1f);
                        break;
                    case 10:
                        
                        unMask.sprite = maskSprite[2];
                        unMask.transform.position = new Vector3(-740f, -458f, 925f);
                        unMask.transform.localScale = new Vector3(1.25f, 0.5f, 1f);
                        break;
                }

                break;

            case TutorialMG.TutorialState.OneMoreHouseClick:
                unMask.sprite = maskSprite[0];
                house = GameObject.Find("9_26_52").transform;
                unMask.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
                followCoroutine = StartCoroutine(FollowHouse());
                break;

            case TutorialMG.TutorialState.TypeSelection:
                count1++;
                switch(count1)
                {
                    case 1:
                        unMask.sprite = maskSprite[4];
                        unMask.transform.position = new Vector3(-72.5f, 35f, 925f);
                        unMask.transform.localScale = new Vector3(0.9f, 0.4f, 1f);
                        break;
                    case 2:
                        unMask.sprite = maskSprite[5];
                        unMask.transform.position = new Vector3(-287f, -120f, 925f);
                        unMask.transform.localScale = new Vector3(1.6f, 0.55f, 1f);
                        break;
                    case 3:
                        unMask.sprite = maskSprite[6];
                        unMask.transform.position = new Vector3(-319.5f, -418.5f, 925f);
                        unMask.transform.localScale = new Vector3(1.85f, 0.7f, 1f);
                        break;
                }
                break;
            case TutorialMG.TutorialState.PushButton:
                unMask.sprite = maskSprite[0];
                unMask.transform.position = new Vector3(-830f, -424f, 925f);
                unMask.transform.localScale = new Vector3(1.35f, 1.35f, 1f);
                break;

            case TutorialMG.TutorialState.DrawLine:
                unMask.sprite = maskSprite[7];
                load = GameObject.Find("3_28_52").transform;
                unMask.transform.localScale = new Vector3(5.25f, 5.25f, 5.25f);
                followLoad = StartCoroutine(FollowLoad());
                break;

            case TutorialMG.TutorialState.EndDrawLine:
                unMask.sprite = maskSprite[6];
                unMask.transform.position = new Vector3(-319.5f, -418.5f, 925f);
                unMask.transform.localScale = new Vector3(1.85f, 0.7f, 1f);
                break;

            case TutorialMG.TutorialState.ProgressGauge:

                count2++;
                switch(count2)
                {
                    case 1:
                        unMask.sprite = maskSprite[0];
                        unMask.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
                        unMask.transform.position = new Vector3(810f, -390f, 925f);
                        break;
                    case 2:
                        unMask.transform.localScale = new Vector3(1.7f, 1.7f, 1.7f);
                        unMask.transform.position = new Vector3(810f, -390f, 925f);
                        break;
                    case 3:
                        unMask.sprite = maskSprite[8];
                        unMask.transform.localScale = new Vector3(1.9f, 1.9f, 1.9f);
                        break;
                }
               
                break;
            
        }
    }

    //========================
    // 停止API
    //========================
    public void StopAll()
    {
        if (scaleCoroutine != null) StopCoroutine(scaleCoroutine);
        if (evaluateCoroutine != null) StopCoroutine(evaluateCoroutine);
        if (followCoroutine != null) StopCoroutine(followCoroutine);
        if (followLoad != null) StopCoroutine(followLoad);

        scaleCoroutine = null;
        evaluateCoroutine = null;
        followCoroutine = null;
        followLoad = null;
    }

    //========================
    // MaskをHouseに毎フレーム追従
    //========================
    IEnumerator FollowHouse()
    {
        if (unMask == null || house == null || canvas == null)
            yield break;

        if (targetCamera == null)
            targetCamera = Camera.main;

        RectTransform canvasRect = canvas.GetComponent<RectTransform>();

        while (true)
        {
            //  ワールド → スクリーン座標
            Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(
                targetCamera,
                house.position
            );

            //  スクリーン → Canvasローカル座標
            Vector2 localPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRect,
                screenPos,
                canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : targetCamera,
                out localPos
            );

            //  UIに反映
            unMask.transform.localPosition = localPos;

            yield return null;
        }
    }

    IEnumerator FollowLoad()
    {
        if (unMask == null || load == null || canvas == null)
            yield break;

        if (targetCamera == null)
            targetCamera = Camera.main;

        RectTransform canvasRect = canvas.GetComponent<RectTransform>();

        while (true)
        {
            //  ワールド → スクリーン座標
            Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(
                targetCamera,
                load.position
            );

            //  スクリーン → Canvasローカル座標
            Vector2 localPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRect,
                screenPos,
                canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : targetCamera,
                out localPos
            );

            //  UIに反映
            unMask.transform.localPosition = localPos;

            yield return null;
        }
    }


    public Vector3 UnMaskToWorld()
    {
        if (unMask == null || canvas == null)
            return Vector3.zero;

        if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            // Overlayはカメラ不要
            Vector3 worldPos = canvas.transform.TransformPoint(unMask.transform.localPosition);
            worldPos.z = house.position.z;
            return worldPos;
        }
        else
        {
            // ScreenSpaceCamera / WorldSpace
            // Canvas Transform を使って直接 World に変換
            Vector3 worldPos = canvas.transform.TransformPoint(unMask.transform.localPosition);
            worldPos.z = house.position.z; // houseのZに合わせる
            return worldPos;
        }
    }






    //========================
    // スケール反復（現在スケール基準）
    //========================
    IEnumerator ScaleLoop()
    {
        yield return new WaitForSeconds(delay);

        Vector3 baseScale = transform.localScale;
        Vector3 targetScale = baseScale * scaleMultiplier;

        while (true)
        {
            yield return Scale(baseScale, targetScale);
            yield return Scale(targetScale, baseScale);
        }
    }

    IEnumerator Scale(Vector3 from, Vector3 to)
    {
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            transform.localScale = Vector3.Lerp(from, to, time / duration);
            yield return null;
        }

        transform.localScale = to;
    }

    //========================
    // 評価処理
    //========================
    IEnumerator EvaluateCoroutine()
    { 
        for (int i = 0; i < targets.Length; i ++)
        {
            targets[i].SetActive(false);
        }
        currentIndex = 0;

        while (currentIndex < targets.Length)
        {
            yield return new WaitForSeconds(evaluateInterval);

            GameObject obj = targets[currentIndex];
            if (obj != null)
            {
                obj.SetActive(true);

                UnmaskScale scale = obj.GetComponent<UnmaskScale>();
                if (scale != null)
                {
                    scale.StartByState(TutorialMG.TutorialState.DocumentCheck);
                }
            }

            currentIndex++;
        }
    }


}
