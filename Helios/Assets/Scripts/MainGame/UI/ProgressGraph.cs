using UnityEngine;
using UnityEngine.UI;

public class ProgressGraph : MonoBehaviour
{
    [SerializeField]
    private Image image;
    private float progress = 0;
    public float GetProgres { get {  return progress; } }
    private int days = 1;
    private int norm;//ÉmÉãÉ}ÅAÇªÇÃÇ§ÇøëùÇ¶ï˚ïœÇ¶ÇÈ
    private TimeLine timeLine;
    private GameStateSystem gameState;
    private BlackScreen blackScreen;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        norm = days + 1;
        timeLine = GameObject.Find("Clock").GetComponent<TimeLine>();
        gameState = GameObject.Find("GameState").GetComponent<GameStateSystem>();
        blackScreen = GameObject.Find("BlackScreen").GetComponent<BlackScreen>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState.GameState != GameStateSystem.State.End)
        {
            image.fillAmount = progress;
            if (progress >= 1.0f)
            {
                timeLine.NextDay();
                gameState.GameState = GameStateSystem.State.End;
                StartCoroutine(blackScreen.FadeOut("MainScene"));
            }
        }
    }
    public void AddProgress()
    {
        progress += 1.0f / norm;
    }
}
