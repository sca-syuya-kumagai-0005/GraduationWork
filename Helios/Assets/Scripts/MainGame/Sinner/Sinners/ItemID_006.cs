using UnityEngine;
public class ItemID_006 : Sinner
{
    float timer;
    const float timeLimit = 60.0f;
    float increase;

    bool isAbnormality;//そのうち資料側のフラグを直接変更する
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        secureClass = SecureClass.Catra;
        liskClass = LiskClass.Oblivara;
        probabilitys = new float[8] { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f };
        sinnerID = "ItemID_006";
        sinnerName = "君と歩んだクラゲ";
        LoadSprite("atokaraireru");
        //effect = GameObject.Find("Effect").transform.Find("Ripple_004").gameObject;
        //GetEffectObject(false);
        effectTimer = 5.0f;
        isAbnormality = false;
    }
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= timeLimit)
        {
            timer = 0.0f;
            increase = 10.0f;
            IncreaseProbabilitys(increase);
        }
    }
    public override void ReceiptDeliveryInformation(int itemID, int deliveryProcessID, int deliveryLineID)
    {
        const int process_Truck = 0;
        if (deliveryProcessID == process_Truck)
        {
            increase = 50.0f;
            IncreaseProbabilitys(increase);
        }

        ReceivedItemID = itemID;
        this.deliveryProcessID = deliveryProcessID;
        this.deliveryLineID = deliveryLineID;
        string str = sinnerID + "に「" + deliveryItems[itemID] + "」の配達が完了しました。";
        announceManager.MakeAnnounce(str);
        //水族館を通ってなかったら
        //if (true)
        {
            float probability = Random.Range(0, 101);
            int damage = Lottery(probability + probabilitys[deliveryLineID]);
            if (damage != 0)
            {
                AbnormalPhenomenon();
                player.Health -= damage;
            }
        }
        progressGraph.AddProgress();
        Destroy(gameObject.transform.Find("DestinationPin(Clone)").gameObject);
    }
    protected override void AbnormalPhenomenon()
    {
        //全ての異常において共通で起きる事があれば↓を変更
        base.AbnormalPhenomenon();

        //それぞれの処理はここに書く
        isAbnormality = true;
    }
}
