using UnityEngine;
public class ItemID_002 : Sinner
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        secureClass = SecureClass.Secra;
        liskClass = LiskClass.Velgra;
        probabilitys = new float[8] { 0.0f, 0.0f, 200.0f, 200.0f, 100.0f, 0.0f, 350.0f, 25.0f };
        sinnerID = "ItemID_002";
        sinnerName = "�g����";
        sinnerType = SinnerType.Humanoid;
        LoadSprite("atokaraireru");
        effect = GameObject.Find("Effect").transform.Find("Fog_001_VFX").gameObject;
        KumagaiLibrary.Dictionary.Support.AddArray(specifyingDeliveryRoutes.SinnerDebuff, sinnerName, false);
    }
    // Update is called once per frame
    void Update()
    {

    }
    public override void ReceiveDeliveryInformation(int itemID, int deliveryProcessID, int deliveryLineID)
    {
        float increase;
        const int processType_Truck = 0;
        if (deliveryProcessID == processType_Truck)
        {
            increase = 25.0f;
            IncreaseProbabilitys(increase);
        }
        base.ReceiveDeliveryInformation(itemID, deliveryProcessID, deliveryLineID);
    }
    protected override void AbnormalPhenomenon()
    {
        //�S�Ăُ̈�ɂ����ċ��ʂŋN���鎖������΁���ύX
        base.AbnormalPhenomenon();

        //���ꂼ��̏����͂����ɏ���
        specifyingDeliveryRoutes.SinnerDebuff[deliveryLineID][sinnerName] = true;
    }
}
