using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressGraph : MonoBehaviour
{
    [SerializeField]
    private Image image;
    private int progress;
    public float GetProgres { get {  return progress; } }
    private int norm;//ÉmÉãÉ}ÅAÇªÇÃÇ§ÇøëùÇ¶ï˚ïœÇ¶ÇÈ
    private TimeLine timeLine;
    private GameStateSystem gameState;
    private BlackScreen blackScreen;
    private SaveDataManager saveData;
    [SerializeField]
    private List<string> sinnerList;
    public List<string> SinnerList { get { return sinnerList; } set { sinnerList = value; } }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sinnerList = new List<string>();
        progress = 0;
        image.fillAmount = 0;
        saveData = GameObject.Find("SaveManager").GetComponent<SaveDataManager>();
        norm = (int)Math.Ceiling(saveData.Days * 1.3);
        timeLine = GameObject.Find("ClockObject").GetComponent<TimeLine>();
        gameState = GameObject.Find("GameState").GetComponent<GameStateSystem>();
        blackScreen = GameObject.Find("BlackScreen").GetComponent<BlackScreen>();
    }

    // Update is called once per frame
    void Update()
    {
        if (progress >= norm && sinnerList.Count == 0)
        {
            if (gameState.GameState != GameStateSystem.State.End)
            {
                saveData.Save();
                timeLine.NextDay();
                gameState.GameState = GameStateSystem.State.End;
                StartCoroutine(blackScreen.FadeOut("Adventure"));
            }
        }
    }
    public void AddProgress()
    {
        progress++;
        float late = (float)(progress * 10 / norm) / 10;
        image.fillAmount = late;
    }
}
