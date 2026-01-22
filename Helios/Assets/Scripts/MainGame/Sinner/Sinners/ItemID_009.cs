using UnityEngine;
using UnityEngine.UIElements;
public class ItemID_009 : Sinner
{
    private GameObject sinnerIconObject;
    private TimeLine timeLine;
    bool isAbnormality;
    GameObject[] plot = new GameObject[9];
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        secureClass = SecureClass.Catra;
        liskClass = LiskClass.Zerath;
        probabilitys = new float[8] { 10.0f, 10.0f, 20.0f, 100.0f, 150.0f, 50.0f, 250.0f, 0.0f };
        sinnerID = "ItemID_009";
        sinnerName = "ÊúΩ„Å°„ÅüÂ§©È¶¨";
        LoadSprite("ID009");
        LoadSinnerObject();
        effect = effectObjectParent.transform.GetChild(8).gameObject;

        for(int i = 0; i < plot.Length; i++)
        {
            plot[i] = GameObject.Find("Address_" + i);
        }
        timeLine = GameObject.Find("ClockObject").GetComponent<TimeLine>();
        isAbnormality = false;
    }
    // Update is called once per frame
    private void Update()
    {
        if(!isAbnormality)
        {
            if (timeLine.TimeState == TimeLine.TimeStates.Night && deliveryCount == 0)
            {
                isAbnormality = true;
                AbnormalPhenomenon();
            }
        }
    }
    public override void ReceiptDeliveryInformation(int itemID, int deliveryProcessID, int deliveryLineID)
    {
        if (timeLine.TimeState == TimeLine.TimeStates.Night || deliveryProcessID == 0)
        {
            AbnormalPhenomenon();
            return;
        } 
        bool notPassedZoo = false;
        if (specifyingDeliveryRoutes.DeleveryData[deliveryLineID].Contains((int)Map.MapObjectID.ZOO))
        {
            IncreaseProbabilitys(90.0f);
            notPassedZoo = true;
        }
        if (notPassedZoo) IncreaseProbabilitys(-90.0f);
        base.ReceiptDeliveryInformation(itemID, deliveryProcessID, deliveryLineID);
    }
    protected override void AbnormalPhenomenon()
    {
        //ÂÖ®„Å¶„ÅÆÁï∞Â∏∏„Å´„Åä„ÅÑ„Å¶ÂÖ±ÈÄö„ÅßËµ∑„Åç„Çã‰∫ã„Åå„ÅÇ„Çå„Å∞‚Üì„ÇíÂ§âÊõ¥
        base.AbnormalPhenomenon();

<<<<<<< HEAD
        //„Åù„Çå„Åû„Çå„ÅÆÂá¶ÁêÜ„ÅØ„Åì„Åì„Å´Êõ∏„Åè
        //„ÄåÂÆöÊúüÁöÑ„Å´Ë°ùÊíÉÊ≥¢„Çí„Ç§„É≥„Çπ„Çø„É≥„Çπ„Åô„ÇãÈ¶¨„Äç„Çí„Ç§„É≥„Çπ„Çø„É≥„Çπ„Åô„Çã

        Instantiate(sinnerIconObject, Vector3.zero, Quaternion.identity, transform.parent.parent);
=======
        //ÇªÇÍÇºÇÍÇÃèàóùÇÕÇ±Ç±Ç…èëÇ≠
        //ÅuíËä˙ìIÇ…è’åÇîgÇÉCÉìÉXÉ^ÉìÉXÇ∑ÇÈînÅvÇÉCÉìÉXÉ^ÉìÉXÇ∑ÇÈ
        Instantiate(sinnerIconObject, transform.parent);
>>>>>>> parent of 337848b (Revert "‰∏ÄÊó¶„Ç¢„ÉÉ„Éó")
    }
}