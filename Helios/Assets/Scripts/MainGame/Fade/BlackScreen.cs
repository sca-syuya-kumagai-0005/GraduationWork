using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[DefaultExecutionOrder(-1)]
public class BlackScreen : EasingMethods
{
    private float fadeInTime;
    Image image;
    AudioManager audioManager;
    private void Start()
    {
        fadeInTime = 0.5f;
        image = gameObject.GetComponent<Image>();
        audioManager = GameObject.Find("Audio").GetComponent<AudioManager>();
        StartCoroutine(FadeIn());
    }
    public IEnumerator FadeOut()
    {
        gameObject.SetActive(true);
        float t = 0.0f;
        float timerLate = 2.5f;
        bool isEnd = false;
        Color color = Color.clear;
        while (!isEnd)
        {
            t += Time.deltaTime / timerLate;
            color = Color.clear + Color.black * EaseInCubic(t);
            image.color = color;
            if (t >= 1.0f) isEnd = true;
            yield return null;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sceneName">ëJà⁄ÇµÇΩÇ¢ÉVÅ[Éìñº</param>
    /// <returns></returns>
    public IEnumerator FadeOut(string sceneName)
    {
        gameObject.SetActive(true);
        float t = 0.0f;
        float timerLate = 2.5f;
        bool isEnd = false;
        Color color = Color.clear;
        while (!isEnd)
        {
            t += Time.deltaTime / timerLate;
            color = Color.clear + Color.black * EaseInCubic(t);
            image.color = color;
            if (t >= 1.0f) isEnd = true;
            yield return null;
        }
        SceneManager.LoadScene(sceneName);
    }

    public IEnumerator FadeIn()
    {
        audioManager.FadeInBGM(fadeInTime);
        gameObject.SetActive(true);
        float t = 0.0f;
        float timerLate = fadeInTime;
        bool isEnd = false;
        Color color = Color.black;
        while (!isEnd)
        {
            t += Time.deltaTime / timerLate;
            color = Color.black - Color.black * EaseInExpo(t);
            image.color = color;
            if (t >= 1.0f) isEnd = true;
            yield return null;
        }
        gameObject.SetActive(false);
    }

    public void OnClick()
    {
        gameObject.SetActive(true);
        audioManager.StopSE();
        audioManager.FadeOutBGM(1.0f);
        StartCoroutine(FadeOut("MainScene"));
    }
}
