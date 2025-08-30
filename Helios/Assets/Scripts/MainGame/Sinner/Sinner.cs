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
        Debug.Log("Damage");
        AbnormalPhenomenon();
    }

    protected void Lottery()
    {
        if (probabilitys[(int)mood] < 100)
        {
            int rand = Random.Range(0, 100);
            if (rand < probabilitys[(int)mood]) Damage(damege);
        }
        int min = 100;
        int max = 150;
        const int add = 50;
        const int death = 350;
        while (max >= death)
        {
            if (min <= probabilitys[(int)mood] && probabilitys[(int)mood] < max)
            {
                Damage(damege);
                return;
            }
            min = max;
            max += add;
        }
        Damage(damege);
    }
}
