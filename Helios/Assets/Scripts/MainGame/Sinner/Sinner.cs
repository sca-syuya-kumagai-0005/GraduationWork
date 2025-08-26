using UnityEngine;

public class Sinner : MonoBehaviour
{
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
    int[] probabilitys = new int[(int)Moods.Max];

    virtual protected void AbnormalPhenomenon()
    {

    }
    protected void Damage()
    {
        Debug.Log("Damage");
        AbnormalPhenomenon();
    }

    protected void Lottery(int value, Moods mood)
    {
        if (value < 100)
        {
            int rand = Random.Range(0, 100);
            if (rand < value) Damage();
        }
        int min = 100;
        int max = 150;
        const int add = 50;
        const int death = 350;
        while (max >= death)
        {
            if (min <= value && value < max)
            {
                Damage();
                break;
            }
            min = max;
            max += add;
        }
    }
}
