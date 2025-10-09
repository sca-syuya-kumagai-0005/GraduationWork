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
        sinnerName = "���Ȃ��ׂ̈̓V�ۋV";
        LoadSprite("atokaraireru");
        effect = GameObject.Find("Effect").transform.Find("").gameObject;

        timeLine = GameObject.Find("Clock").GetComponent<TimeLine>();
        abnormalityDuration = 0.0f;
    }
    // Update is called once per frame
    void Update()
    {
        abnormalityDuration -= Time.deltaTime;
        if (abnormalityDuration >= 0.0f)
        {

        }
    }
    public override void ReceiveDeliveryInformation(int itemID, int deliveryProcessID, int deliveryLineID)
    {
        float increase;//�e�����ɑ΂���m��������
        if (itemID == (int)Mood.Anticipation || itemID == (int)Mood.Trust)
        {
            isRampage = true;
        }
        else if (itemID == (int)Mood.Anger || itemID == (int)Mood.Disgust)
        {
            increase = 100.0f;
            IncreaseProbabilitys(increase);
        }
        const int processType_Truck = 0;
        if (deliveryProcessID == processType_Truck)
        {
            increase = 80.0f;
            IncreaseProbabilitys(increase);
        }
        base.ReceiveDeliveryInformation(itemID, deliveryProcessID, deliveryLineID);
    }
    protected override void AbnormalPhenomenon()
    {
        //�S�Ăُ̈�ɂ����ċ��ʂŋN���鎖������΁���ύX
        base.AbnormalPhenomenon();

        //���ꂼ��̏����͂����ɏ���
    }
}
