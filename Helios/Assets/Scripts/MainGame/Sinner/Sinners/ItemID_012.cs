using UnityEngine;
public class ItemID_012 : Sinner
{
    private bool isAbnormality;
    private bool isWalk;
    private bool isBycicle;

    private TimeLine timeLine;

    private float timer;
    private float timeLimit;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        secureClass = SecureClass.Secra;
        liskClass = LiskClass.Velgra;
        probabilitys = new float[8] { 0.0f, 0.0f, 60.0f, 50.0f, 30.0f, 0.0f, 10.0f, 100.0f };
        sinnerID = "ItemID_012";
        sinnerName = "人に住く狐";
        LoadSprite("ID012");
        timeLine = GameObject.Find("ClockObject").GetComponent<TimeLine>();
        effect = effectObjectParent.transform.GetChild(11).gameObject;

        isAbnormality = false;
        isWalk = false;
        isBycicle = false;

        timer = 0.0f;
        timeLimit = 90.0f;
    }

    private void Update()
    {
        if (isAbnormality)
        {
            timer += Time.deltaTime;
            if (timer > timeLimit)
            {
                timer -= timeLimit;
                if (timeLine.TimeStateAccess == TimeLine.TimeState.Night) player.Heal(3);
                else
                {
                    if (player.Health > 3)
                        player.Health -= 3;
                    else player.Health = 1;
                }
            }
        }
    }

    public override void ReceiptDeliveryInformation(int itemID, int deliveryProcessID, int deliveryLineID)
    {
        if (specifyingDeliveryRoutes.DeleveryData[deliveryLineID].Contains((int)Map.MapObjectID.SHIRINE_LEFT)|| specifyingDeliveryRoutes.DeleveryData[deliveryLineID].Contains((int)Map.MapObjectID.SHIRINE_RIGHT))
        {
            isAbnormality = false;
            timer = 0.0f;
        }

        if (deliveryProcessID == 2)
        {
            isWalk = true;
            IncreaseProbabilitys(-45.0f);
        }
        if (deliveryProcessID == 1)
        {
            isBycicle = true;
            IncreaseProbabilitys(40.0f);
        }
        base.ReceiptDeliveryInformation(itemID, deliveryProcessID, deliveryLineID);

        if (isWalk)
        {
            IncreaseProbabilitys(45.0f);
            isWalk = false;
        }
        if (isBycicle)
        {
            IncreaseProbabilitys(-40.0f);
            isBycicle = false;
        }
    }
    public override void AbnormalPhenomenon()
    {
        //全ての異常において共通で起きる事があれば↓を変更
        base.AbnormalPhenomenon();

        //それぞれの処理はここに書く
        isAbnormality = true;
    }
}
