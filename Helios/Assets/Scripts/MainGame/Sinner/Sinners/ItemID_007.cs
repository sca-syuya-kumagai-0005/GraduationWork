using UnityEngine;
public class ItemID_007 : Sinner
{
    //�z�����C����
    const int lines = 4;
    //��������
    const float timeLimit = 30.0f;
    //������
    const float increase = 25.0f;
    float[] addProbabilitys = new float[lines];
    //�e���C���̃^�C�}�[
    float[] timer = new float[lines];
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        secureClass = SecureClass.Vigil;
        liskClass = LiskClass.Dravex;
        probabilitys = new float[8] { 25.0f, 25.0f, 25.0f, 0.0f, 25.0f, 25.0f, 0.0f, 125.0f };
        sinnerID = "ItemID_007";
        sinnerName = "�`��杂͍��炩��";
        sinnerTypeList.Add(SinnerType.Humanoid);
        LoadSprite("atokaraireru");
        effect = GameObject.Find("Effect").transform.Find("").gameObject;
        GetEffectObject(false);
        KumagaiLibrary.Dictionary.Support.AddArray(specifyingDeliveryRoutes.SinnerDebuff, sinnerName, false);
    }
    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < lines; i++)
        {
            if (specifyingDeliveryRoutes.SinnerDebuff[deliveryLineID][sinnerName])
            {
                timer[i] += Time.deltaTime;
                if (specifyingDeliveryRoutes.IsDriving[i]) timer[i] = 0.0f;
                if (timer[i] >= timeLimit)
                {
                    timer[i] = 0.0f;
                    addProbabilitys[i] += increase;
                    if (Random.Range(0, 100) < addProbabilitys[i])
                        AbnormalPhenomenon();
                }
            }
        }
    }
    public override void ReceiptDeliveryInformation(int itemID, int deliveryProcessID, int deliveryLineID)
    {
        ReceivedItemID = itemID;
        this.deliveryProcessID = deliveryProcessID;
        this.deliveryLineID = deliveryLineID;
        string str = sinnerID + "�Ɂu" + deliveryItems[itemID] + "�v�̔z�B���������܂����B";
        announceManager.MakeAnnounce(str);
        int damage = Lottery(probabilitys[deliveryProcessID] + addProbabilitys[deliveryProcessID]);
        if (damage != 0)
        {
            AbnormalPhenomenon();
            player.Health -= damage;
        }
        progressGraph.AddProgress();
        Destroy(gameObject.transform.Find("DestinationPin(Clone)").gameObject);
    }
    protected override void AbnormalPhenomenon()
    {
        //�S�Ăُ̈�ɂ����ċ��ʂŋN���鎖������΁���ύX
        base.AbnormalPhenomenon();

        //���ꂼ��̏����͂����ɏ���
        //sDR�ł��̃t���O�����Ă��獬���𔻒�
        for(int i = 0; i < lines; i++)
        {
            if (specifyingDeliveryRoutes.IsDriving[i])
                specifyingDeliveryRoutes.SinnerDebuff[i][sinnerName] = true;
        }
    }
}
