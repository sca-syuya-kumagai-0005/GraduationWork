using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class ItemID_008 : Sinner
{

    bool isAbnormality;
    AudioManager audioManager;
    float increase;
    float[] timer = new float[2]
    {
        0.0f,
        0.0f
    };
    float[] timeLimit = new float[2]
    {
        30.0f,
        10.0f
    };
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        secureClass = SecureClass.Catra;
        liskClass = LiskClass.Oblivara;
        probabilitys = new float[8] { 200.0f, 150.0f, 0.0f, 0.0f, 25.0f, 50.0f, 50.0f, 100.0f };
        sinnerID = "ItemID_008";
        sinnerName = "ó÷è•ÇÃéP";
        LoadSprite("ID008");
        effect = effectObjectParent.transform.GetChild(7).gameObject;
        for(int i = 0; i < specifyingDeliveryRoutes.SinnerDebuff.Length; i++)
        {
            specifyingDeliveryRoutes.SinnerDebuff[i].Add(sinnerName, false);
        }
        audioManager = GameObject.Find("Audio").gameObject.GetComponent<AudioManager>();
        isAbnormality = false;
    }
    // Update is called once per frame
    void Update()
    {
        if(isAbnormality)
        {
            timer[1] += Time.deltaTime;
        }
        bool addConfition = true;
        if (audioManager.GetVolume(Audio.MASTER) < 0.05f)
            addConfition = false;

        if (audioManager.GetVolume(Audio.BGM) < 0.05f && audioManager.GetVolume(Audio.SE) < 0.05f)
            addConfition = false;

        if (addConfition)
        {
            if (timer[1] >= timeLimit[1])
            {
                timer[1] -= timeLimit[1];
                List<int> normalLines = new List<int>();
                for (int i = 0; i < specifyingDeliveryRoutes.SinnerDebuff.Length; i++)
                {
                    if (!specifyingDeliveryRoutes.SinnerDebuff[i][sinnerName])
                        normalLines.Add(i);
                }
                int rand = Random.Range(0, normalLines.Count);
                AbnormalPhenomenon(normalLines[rand]);
            }
        }
    }
    public override void ReceiptDeliveryInformation(int itemID, int deliveryProcessID, int deliveryLineID)
    {
        if (specifyingDeliveryRoutes.DeleveryData[deliveryLineID].Contains((int)Map.MapObjectID.SCHOOL))
        {
            increase = 150.0f;
            IncreaseProbabilitys(increase);
        }
        base.ReceiptDeliveryInformation(itemID, deliveryProcessID, deliveryLineID);
        if (!isAbnormality)
        {
            StartCoroutine(CountDown(deliveryLineID));
        }
    }
    public override void AbnormalPhenomenon()
    {
        //ëSÇƒÇÃàŸèÌÇ…Ç®Ç¢Çƒã§í Ç≈ãNÇ´ÇÈéñÇ™Ç†ÇÍÇŒÅ´ÇïœçX
        base.AbnormalPhenomenon();

        //ÇªÇÍÇºÇÍÇÃèàóùÇÕÇ±Ç±Ç…èëÇ≠
        isAbnormality = true;
    }
    private void AbnormalPhenomenon(int lineID)
    {
        //ÇªÇÍÇºÇÍÇÃèàóùÇÕÇ±Ç±Ç…èëÇ≠
        specifyingDeliveryRoutes.SinnerDebuff[lineID][sinnerName] = true;
        specifyingDeliveryRoutes.ConfisonClickCount[lineID] = 50;
        specifyingDeliveryRoutes.TotalAbnormal++;
    }

    private IEnumerator CountDown(int lineNumber)
    {
        bool isEnd = false;
        while (!isEnd)
        {
            timer[0] += Time.deltaTime;
            if (!specifyingDeliveryRoutes.IsDriving[lineNumber]|| timeLimit[0] < timer[0])
            {
                isEnd = true;
            }
            yield return null;
        }
        if(timeLimit[0] < timer[0])AbnormalPhenomenon();
        timer[0] = 0.0f;
    }
}
