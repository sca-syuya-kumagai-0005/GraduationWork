using UnityEditor.UIElements;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private ECGWaveCircleController waveCircleController;
    private int health;
    private const int maxHealth = 100;
    public int Health { get { return health; }}

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

    public void fluctuationHealth(int value)
    {
        health += value;
    }
    private void Start()
    {
        formatting();
    }
    private void Update()
    {
        phase = 0;
        for (int i = 0; i < phaseLine.Length; i++)
        {
            if (phaseLine[i] >= health) phase++;
        }
        waveCircleController.targetState = (ECGWaveCircleController.WaveState)phase;
    }
}
