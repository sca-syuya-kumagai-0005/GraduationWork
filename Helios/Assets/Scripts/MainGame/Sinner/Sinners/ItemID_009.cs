using UnityEngine;
using UnityEngine.UIElements;
public class ItemID_009 : Sinner
{
    private GameObject mapObject;
    private SinnerDistribute distribute;
    private TimeLine timeLine;
    bool isAbnormality;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        secureClass = SecureClass.Catra;
        liskClass = LiskClass.Zerath;
        probabilitys = new float[8] { 10.0f, 10.0f, 20.0f, 100.0f, 150.0f, 50.0f, 250.0f, 0.0f };
        sinnerID = "ItemID_009";
        sinnerName = "朽ちた天馬";
        LoadSprite("ID009");
        LoadSinnerIconObject();
        effect = effectObjectParent.transform.GetChild(8).gameObject;

        distribute = GameObject.Find("Map").gameObject.GetComponent<SinnerDistribute>();
        timeLine = GameObject.Find("ClockObject").GetComponent<TimeLine>();
        isAbnormality = false;
    }
    // Update is called once per frame
    private void Update()
    {
        if(!isAbnormality)
        {
            if (timeLine.TimeStateAccess == TimeLine.TimeState.Night && deliveryCount == 0)
            {
                isAbnormality = true;
                AbnormalPhenomenon();
            }
        }
    }
    public override void ReceiptDeliveryInformation(int itemID, int deliveryProcessID, int deliveryLineID)
    {
        progressGraph.SinnerList.Remove(sinnerName);
        DeleteRanpage();
        spriteRenderer.color = new Color(0.25f, 1.0f, 0.15f);

        ItemID_031 delta = GameObject.FindAnyObjectByType<ItemID_031>();
        if (delta != null)
            StartCoroutine(delta.WeakingMood(deliveryItems[itemID], sinnerName));

        if (deliveryItems[7] != Mood.Exception)
        {
            if (timeLine.TimeStateAccess == TimeLine.TimeState.Night || deliveryProcessID == 0)
            {
                AbnormalPhenomenon();
                player.Health -= (int)DamageLevel.Minor;
                progressGraph.AddProgress();
                Destroy(gameObject.transform.Find("DestinationPin(Clone)").gameObject);
                return;
            }
            bool notPassedZoo = false;
            if (!specifyingDeliveryRoutes.DeleveryData[deliveryLineID].Contains((int)Map.MapObjectID.ZOO))
            {
                IncreaseProbabilitys(90.0f);
                notPassedZoo = true;
            }
            if (notPassedZoo) IncreaseProbabilitys(-90.0f);
            base.ReceiptDeliveryInformation(itemID, deliveryProcessID, deliveryLineID);
        }
        else
        {
            if (deliveryItems[itemID] == Mood.Exception)
            {
                Release(sinnerName);
            }
            progressGraph.AddProgress();
            Destroy(gameObject.transform.Find("DestinationPin(Clone)").gameObject);
        }

        StartCoroutine(DeliveryProgressCheck());
    }
    public override void AbnormalPhenomenon()
    {        
        //全ての異常において共通で起きる事があれば↓を変更
        base.AbnormalPhenomenon();

        mapObject = Instantiate(sinnerIconObject, Vector3.zero, Quaternion.identity, transform.parent.parent);
        CelestialSteed celestialSteed = mapObject.AddComponent<CelestialSteed>();
        celestialSteed.SetSprite = sinnerSprite;
        mapObject.name = "朽ちた天馬";

        isAbnormality = true;

        deliveryItems[7] = Mood.Exception;
    }
    public override void Release(string name)
    {
        Destroy(mapObject);
        deliveryItems[7] = Mood.Trust;
        announceManager.MakeAnnounce(sinnerName + "の異常は解消されました。");
        base.Release(sinnerName);
    }
}