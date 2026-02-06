using UnityEngine;
public class ItemID_003 : Sinner
{
    private int genesisPhase;
    private const float timeLimit = 30.0f;
    private float timer;
    private string[] itemNames = new string[9]
    {
        "",
        "ñæÇ©ÇËÇ∆à√Ç™ÇË",
        "ìVè„Ç∆ìVâ∫ÇÃêÖ",
        "î…âhÇ∑ÇÈó§Ç∆äC",
        "êØÇ∆ì˙Ç∆åéÇÃó÷",
        "óÉÇ∆ïhÇÃï’Ç≠ñΩ",
        "ë¥ÇÃñÕë¢Ç∆íÅèò",
        "èjïüÇ∑Ç◊Ç´ãxëß",
        "êVënê¢",
    };
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        secureClass = SecureClass.Secra;
        liskClass = LiskClass.Oblivara;
        probabilitys = new float[8] { 100.0f, 100.0f, 100.0f, 100.0f, 100.0f, 100.0f, 100.0f, 100.0f };
        int rand = Random.Range(0, deliveryItems.Length);
        deliveryItems[rand] = Mood.Exception;
        sinnerID = "ItemID_003";
        sinnerName = "ê¢äEÇ≈ç≈èâÇ…ãÛÇ¢ÇΩåä";
        LoadSprite("ID003");
        effect = effectObjectParent.transform.GetChild(2).gameObject;
        genesisPhase = 1;
        timer = 0.0f;
    }
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timeLimit)
        {
            AbnormalPhenomenon();
            timer = 0.0f;
        }
    }
    public override void ReceiptDeliveryInformation(int itemID, int deliveryProcessID, int deliveryLineID)
    {
        progressGraph.SinnerList.Remove(sinnerName);
        DeleteRanpage();
        spriteRenderer.color = new Color(0.25f, 1.0f, 0.15f);

        ItemID_031 delta = GameObject.FindAnyObjectByType<ItemID_031>();
        StartCoroutine(delta.WeakingMood(deliveryItems[itemID], sinnerName));
        if (genesisPhase == 8)
        {
            player.Health -= 666;
            return;
        }
        if (deliveryItems[itemID] == Mood.Exception)
        {
            int heal = 10 * genesisPhase;
            player.Heal(heal);
            string text = genesisPhase + "ì˙ñ⁄ÅB\n" + itemNames[genesisPhase];
            announceManager.MakeAnnounce(text);
            genesisPhase++;
        }
        else
        {
            AbnormalPhenomenon();
        }    
        progressGraph.AddProgress();
        Destroy(gameObject.transform.Find("DestinationPin(Clone)").gameObject);
        timer = 0.0f;

        DeliveryProgressCheck();
    }
    public override void AbnormalPhenomenon()
    {
        int damage = 5 * genesisPhase;
        player.Health -= damage;
        string text;
        if (timer >= timeLimit)
        {
            text = "ë„èûÇÕä˘Ç…éxï•ÇÌÇÍÇ‹ÇµÇΩÅB";
            announceManager.MakeAnnounce(text);
        }
        else
        {
            text = sinnerName + "ÅFàŸèÌî≠ê∂ÅB";
            announceManager.MakeAnnounce(text);
        }
    }
}
