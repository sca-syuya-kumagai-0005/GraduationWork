using UnityEngine;

public class Sinner : MonoBehaviour
{
    protected enum SecureClass
    {
        Secra,
        Vigil,
        Hazra,
        Catra,
        Nulla
    }
    protected enum LiskClass
    {
        Lumenis,
        Velgra,
        Dravex,
        Zerath,
        Oblivara
    }
    protected enum Moods
    {
        Joy = 0,
        Anticipation,
        Anger,
        Disgust,
        Sadness,
        Surprise,
        Fear,
        Trust,
        Max
    }
    protected enum EmergencyPhase
    {
        First,
        Second,
        Third,
        Death
    }
    protected EmergencyPhase phase;
    protected int[] probabilitys = new int[(int)Moods.Max];
    protected Moods mood;
    protected SecureClass secureClass;
    protected LiskClass liskClass;
    protected int damege;
    virtual protected void AbnormalPhenomenon()
    {
        Debug.Log("àŸèÌî≠ê∂");
    }
    protected void Damage(int damege)
    {
        Debug.Log(damege+"Damage");
    }

    protected EmergencyPhase Lottery()
    {
        EmergencyPhase phase;
        if (probabilitys[(int)mood] < 100)
        {
            int rand = Random.Range(0, 100);
            if (rand < probabilitys[(int)mood])
            {
                phase = EmergencyPhase.First;
                return phase;
            }
        }
        int min = 100;
        int max = 150;
        const int add = 50;
        const int death = 350;
        while (max >= death)
        {
            if (min <= probabilitys[(int)mood] && probabilitys[(int)mood] < max)
            {
                phase= EmergencyPhase.Second;
                return phase;
            }
            min = max;
            max += add;
        }
        phase = EmergencyPhase.Death;
        return phase;
    }
}
