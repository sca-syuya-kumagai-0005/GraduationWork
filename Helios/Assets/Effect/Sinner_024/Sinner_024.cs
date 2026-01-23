using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;
using TMPro;

public class VolumeLiftAnimator : MonoBehaviour
{
    [Header("Volume")]
    [SerializeField] private Volume volume;

    [Header("Lift Animation")]
    [SerializeField] private Vector4 startLift;
    [SerializeField] private Vector4 endLift;
    [SerializeField] private float duration = 1f;

    [Header("UI")]
    [SerializeField] private TMP_Text inputText;

    [Header("Anime")]
    [SerializeField] private Animator animationChess;
    [SerializeField] private GameObject volumeCamera;
    [SerializeField] private LiftGammaGain liftGammaGain;

    [SerializeField] private GameObject[] objects;
    [SerializeField] private GameObject canvas;

    // チェックメイト入力用
    private readonly string secretWord = "checkmate";
    private int currentIndex = 0;

    void OnEnable()
    {
        volumeCamera.SetActive(true);
        animationChess.enabled = false;
        ResetInput();
        canvas.SetActive(true);
        // ここが重要
        if (!volume.profile.TryGet(out liftGammaGain))
        {
            Debug.LogError("LiftGammaGain が Volume に存在しません");
            return;
        }

        liftGammaGain.lift.value = startLift;
        inputText.text = "";

        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].SetActive(false);
        }
    }


    void Update()
    {
        foreach (char c in Input.inputString)
        {
            // 英字以外は無視（return しない）
            if (!char.IsLetter(c))
                continue;

            char inputChar = char.ToLower(c);

            // 範囲外防止
            if (currentIndex >= secretWord.Length)
            {
                ResetInput();
                return;
            }

            // 正しい次の文字か？
            if (inputChar == secretWord[currentIndex])
            {
                currentIndex++;

                // 表示に「蓄積」
                inputText.text += inputChar;

                // 完全一致
                if (currentIndex == secretWord.Length)
                {
                    Debug.Log("チェックメイト");

                    // 成功演出
                    animationChess.enabled = true;
                }
            }
            else
            {
                // 違う文字が来た瞬間にリセット
                Debug.Log("リセット");
                ResetInput();
            }
        }
    }

    void ResetInput()
    {
        currentIndex = 0;
        inputText.text = "";
    }

    public void End()
    {
        volumeCamera.SetActive(false);
        inputText.text = "";
        animationChess.enabled = false;
    }

    public void Play()
    {
        StopAllCoroutines();
        StartCoroutine(AnimateLift());
    }

    IEnumerator AnimateLift()
    {
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float lerp = Mathf.Clamp01(t / duration);
            liftGammaGain.lift.value = Vector4.Lerp(startLift, endLift, lerp);
            yield return null;
        }

        liftGammaGain.lift.value = endLift;
    }
}
