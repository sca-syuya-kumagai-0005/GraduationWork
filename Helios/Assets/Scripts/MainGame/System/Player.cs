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
    private Phase lastPhase;

    [SerializeField]
    private AudioClip[] emergencyClips;
    private AudioManager audioManager;
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
        warningColor = warning.transform.GetChild(0).GetComponent<WarningColor>();
        warningLine = warning.transform.GetChild(0).GetComponent<WarningLine>();
        phase = 0;
        lastPhase = 0;
        audioManager = GameObject.Find("Audio").GetComponent<AudioManager>();
        audioManager.PlayBGM(emergencyClips[0]);
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) health -= 10;
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

        if (lastPhase != phase)
        {
            int[] colorNumber = new int[5] { 3, 2, 1, 4, 0 };
            warningColor.colorType = (WarningColorType)colorNumber[(int)phase];
            StartCoroutine(WarningTape());
            StartCoroutine(EmergencyAudioChenge());
        }
        lastPhase = phase;
    }

    private IEnumerator EmergencyAudioChenge()
    {
        float timer = 0.5f;
        audioManager.FadeOutBGM(timer);
        yield return new WaitForSeconds(timer);
        audioManager.PlayBGM(emergencyClips[(int)phase]);
        audioManager.FadeInBGM(timer);
    }
    private IEnumerator WarningTape()
    {
        warningLine.SetState(WarningLine.WarningState.In);
        yield return new WaitForSeconds(5);
        warningLine.SetState(WarningLine.WarningState.Out);
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
            timer += Time.deltaTime;
            float addPos = (motionedPos - defaultPos) * EaseInOutCubic(timer);
            restartButton.transform.localPosition = new Vector3(0, defaultPos + addPos+50, 0);
            if (timer >= 1.0f) isEnd = true;
            yield return null;
        }
        transform.localPosition = new Vector3(0, motionedPos, 0);
    }
}
