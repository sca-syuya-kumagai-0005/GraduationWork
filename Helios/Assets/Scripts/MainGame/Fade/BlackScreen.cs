using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[DefaultExecutionOrder(-1)]
public class BlackScreen : EasingMethods
{
    Image image;
    private void Start()
    {
        image = gameObject.GetComponent<Image>();
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
        gameObject.SetActive(true);
        float t = 0.0f;
        float timerLate = 2.5f;
        bool isEnd = false;
        Color color = Color.black;
        while (!isEnd)
        {
            t += Time.deltaTime / timerLate;
            color = Color.black - Color.black * EaseOutSine(t);
            image.color = color;
            if (t >= 1.0f) isEnd = true;
            yield return null;
        }
        gameObject.SetActive(false);
    }
}
