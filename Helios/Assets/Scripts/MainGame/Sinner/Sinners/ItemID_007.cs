using UnityEngine;
public class ItemID_007 : Sinner
{
    //配送ライン数
    const int lines = 4;
    //制限時間
    const float timeLimit = 30.0f;
    //増加量
    const float increase = 25.0f;
    float[] addProbabilitys = new float[lines];
    //各ラインのタイマー
    float[] timer = new float[lines];
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        secureClass = SecureClass.Vigil;
        liskClass = LiskClass.Dravex;
        probabilitys = new float[8] { 25.0f, 25.0f, 25.0f, 0.0f, 25.0f, 25.0f, 0.0f, 125.0f };
        sinnerID = "ItemID_007";
        sinnerName = "冒険譚は高らかに";
        sinnerTypeList.Add(SinnerType.Humanoid);
        LoadSprite("ID007");
        effect = effectObjectParent.transform.GetChild(6).gameObject;
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
        string str = sinnerID + "に「" + deliveryItems[itemID] + "」の配達が完了しました。";
        announceManager.MakeAnnounce(str);
        int damage = Lottery(probabilitys[deliveryProcessID] + addProbabilitys[deliveryProcessID]);
        if (damage != 0)
        {
            AbnormalPhenomenon();
            specifyingDeliveryRoutes.AbnormalCount[deliveryLineID]++;
            player.Health -= damage;
        }
        progressGraph.AddProgress();
        Destroy(gameObject.transform.Find("DestinationPin(Clone)").gameObject);
    }
    public override void AbnormalPhenomenon()
    {
        //全ての異常において共通で起きる事があれば↓を変更
        base.AbnormalPhenomenon();

        //それぞれの処理はここに書く
        //sDRでこのフラグ立ってたら混乱を判定
        for(int i = 0; i < lines; i++)
        {
            if (specifyingDeliveryRoutes.IsDriving[i])
            {
                specifyingDeliveryRoutes.SinnerDebuff[i][sinnerName] = true;
                specifyingDeliveryRoutes.ConfisonClickCount[i] = 50;
            }
        }
    }
}
