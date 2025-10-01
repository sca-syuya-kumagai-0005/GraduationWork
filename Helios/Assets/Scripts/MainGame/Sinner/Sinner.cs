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
    protected string sinnerID;
    protected string sinnerName;
    protected Sprite sinnerSprite;
    protected int deliveryCount;
    protected int damege;
    protected ResidenceCertificate residenceCertificate;
    protected Moods[] deliveryItems = new Moods[8];

    /// <summary>
    /// îzíBàıÇ™åöï®Ç…ìûíÖÇµÇΩéûÇ…åƒÇ‘
    /// </summary>
    public int GiveDeliveryItem { set { mood = (Moods)value; } }
    private void Awake()
    {
        residenceCertificate = GameObject.Find("ResidenceCertificate").GetComponent<ResidenceCertificate>();
    }
    virtual protected void AbnormalPhenomenon(string objectName)
    {
        Debug.Log(objectName + ":àŸèÌî≠ê∂");
    }
    protected void Damage(int damege)
    {
        Debug.Log(damege + "Damage");
    }
    protected void SetInformation()
    {
        residenceCertificate.SetSinnerName = sinnerName;
        residenceCertificate.SetSinnerID = sinnerID;
        residenceCertificate.SetSinnerImage = sinnerSprite;
        residenceCertificate.SetSecureClass = secureClass.ToString();
        residenceCertificate.SetLiskClass = liskClass.ToString();
        residenceCertificate.SetDeliveryItems = (int[])deliveryItems.Clone();
        Debug.Log("SECURE:" + secureClass + "\nLISK:" + liskClass);
        residenceCertificate.gameObject.SetActive(true);
    }
    protected EmergencyPhase Lottery()
    {
        EmergencyPhase phase = EmergencyPhase.First;
        if (probabilitys[(int)mood] < 100)
        {
            int rand = UnityEngine.Random.Range(0, 100);
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
        return phase;
    }
}
