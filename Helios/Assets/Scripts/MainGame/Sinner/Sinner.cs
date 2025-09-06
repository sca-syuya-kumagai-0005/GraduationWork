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
    protected Moods mood;
    protected float[] probabilitys = new float[(int)Moods.Max];
    protected SecureClass secureClass;
    protected LiskClass liskClass;
    protected string ItemName;
    protected int deliveryCount;
    protected int damege;
    virtual protected void AbnormalPhenomenon(string objectName)
    {
        Debug.Log(objectName + "àŸèÌî≠ê∂");
    }
    protected void Damage(int damege)
    {
        Debug.Log(damege + "Damage");
    }

    protected EmergencyPhase Lottery()
    {
        EmergencyPhase phase = EmergencyPhase.First;
        if (probabilitys[(int)mood] < 100)
        {
            int rand = Random.Range(0, 100);
            if (rand < probabilitys[(int)mood])
            {
                phase = EmergencyPhase.First;
            }
        }
        else if (100 < probabilitys[(int)mood] || probabilitys[(int)mood] < 200)
        {
            phase = EmergencyPhase.First;
        }
        else if (200 < probabilitys[(int)mood] || probabilitys[(int)mood] < 300)
        {
            phase = EmergencyPhase.Second;
        }
        else if (300 < probabilitys[(int)mood] || probabilitys[(int)mood] < 350)
        {
            phase = EmergencyPhase.Third;
        }
        else
        {
            phase = EmergencyPhase.Death;
        }
        //int min = 100;
        //int max = 150;
        //const int add = 50;
        //const int death = 350;
        //while (max >= death)
        //{
        //    if (min <= probabilitys[(int)mood] && probabilitys[(int)mood] < max)
        //    {
        //        phase= EmergencyPhase.Second;
        //        return phase;
        //    }
        //    min = max;
        //    max += add;
        //}
        return phase;
    }
}
