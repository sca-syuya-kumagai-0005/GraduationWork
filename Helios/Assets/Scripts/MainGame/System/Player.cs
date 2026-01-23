using KumagaiLibrary.Unity;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : EasingMethods
{
    [SerializeField]
    private GameObject restartPanel;
    private GameObject restartButton;
    private GameStateSystem gameStateSystem;
    [SerializeField]
    private ECGWaveCircleController waveCircleController;
    [SerializeField]
    private GameObject warning;
    private WarningColor warningColor;
    private WarningLine warningLine;
    private int health;
    private const int maxHealth = 100;
    [SerializeField]
    private Image hpGage;
    public int Health { get { return health; }set { health = value; } }

    private int[] phaseLine = new int[3] { 99, 66, 50 };
    private enum Phase
    {
        First=0,
        Second, 
        Third,
        Emergency
    }
    private Phase phase;
    public void formatting()
    {
        health = maxHealth;
    }
    public void Heal(int value)
    {
        if (health <= 0)
        health += value;
    }
    private void Start()
    {
        formatting();
        gameStateSystem = GameObject.Find("GameState").gameObject.GetComponent<GameStateSystem>();
        restartButton = restartPanel.transform.GetChild(1).gameObject;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) health = 0;
        if (health <= 0)
        {
            if (gameStateSystem.GameState != GameStateSystem.State.End)
            {
                restartPanel.transform.GetChild(0).gameObject.SetActive(true);
                StartCoroutine(FallRestartButton());
                gameStateSystem.GameState = GameStateSystem.State.End;
            }
        }
        if (health > maxHealth) health = maxHealth;
        phase = 0;
        for (int i = 0; i < phaseLine.Length; i++)
        {
            if (phaseLine[i] >= health) phase++;
        }
        waveCircleController.targetState = (ECGWaveCircleController.WaveState)phase;
        hpGage.fillAmount = (float)health / maxHealth;
    }

    private IEnumerator FallRestartButton()
    {
        float timer = 0.0f;
        bool isEnd = false;

        float defaultPos = 600.0f;
        float motionedPos = 500.0f;

        transform.localPosition = new Vector3(0, defaultPos, 0);
        while (!isEnd)
        {
            Debug.Log(timer);
            timer += Time.deltaTime;
            float addPos = (motionedPos - defaultPos) * EaseInOutCubic(timer);
            restartButton.transform.localPosition = new Vector3(0, defaultPos + addPos+50, 0);
            if (timer >= 1.0f) isEnd = true;
            yield return null;
        }
        transform.localPosition = new Vector3(0, motionedPos, 0);
    }
}
