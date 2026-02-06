using System.Collections;
using UnityEngine;
public class ItemID_005 : Sinner
{
    private bool isRampage = false;
    private TimeLine timeLine;
    private float abnormalityDuration;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        secureClass = SecureClass.Catra;
        liskClass = LiskClass.Lumenis;
        probabilitys = new float[8] { 0.0f, 100.0f, 125.0f, 100.0f, 75.0f, 0.0f, 25.0f, 100.0f };
        sinnerID = "ItemID_005";
        sinnerName = "あなたの為の天象儀";
        LoadSprite("ID005");
        effect = effectObjectParent.transform.GetChild(4).gameObject;
        timeLine = GameObject.Find("ClockObject").GetComponent<TimeLine>();
        abnormalityDuration = 0.0f;
    }
    public override void ReceiptDeliveryInformation(int itemID, int deliveryProcessID, int deliveryLineID)
    {
        abnormalityDuration = 45.0f;
        float increase;//各条件に対する確率増加量
        if (itemID == (int)Mood.Anticipation || itemID == (int)Mood.Trust)
        {
            isRampage = true;
        }
        else if (itemID == (int)Mood.Anger || itemID == (int)Mood.Disgust)
        {
            increase = 100.0f;
            IncreaseProbabilitys(increase);
            abnormalityDuration = 90.0f;
        }
        const int processType_Truck = 0;
        if (deliveryProcessID == processType_Truck)
        {
            increase = 80.0f;
            IncreaseProbabilitys(increase);
        }
        base.ReceiptDeliveryInformation(itemID, deliveryProcessID, deliveryLineID);
    }
    public override void AbnormalPhenomenon()
    {
        //全ての異常において共通で起きる事があれば↓を変更
        base.AbnormalPhenomenon();

        //それぞれの処理はここに書く
        //朝に固定する処理を書く
        if (!timeLine.AbnormalityList.Contains(sinnerName))
            timeLine.AddAbnormalityList(sinnerName);
        timeLine.TimeStateAccess = TimeLine.TimeState.Morning;
        StartCoroutine(Release());
    }

    private IEnumerator Release()
    {
        float timer = 0.0f;
        while (abnormalityDuration > timer)
        {
            if(!isRampage)
            timer += Time.deltaTime;
            yield return null;
        }
        timeLine.RemoveAbnormalityList(sinnerName);
        base.Release(sinnerName);
    }

    public override void Release(string name)
    {

    }
}
