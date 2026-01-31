using UnityEngine;
public class ItemID_003 : Sinner
{
    private int genesisPhase;
    private const float timeLimit = 30.0f;
    private float timer;
    private string[] itemNames = new string[9]
    {
        "",
        "–¾‚©‚è‚ÆˆÃ‚ª‚è",
        "“Vã‚Æ“V‰º‚Ì…",
        "”É‰h‚·‚é—¤‚ÆŠC",
        "¯‚Æ“ú‚ÆŒ‚Ì—Ö",
        "—ƒ‚Æ•h‚Ì•Õ‚­–½",
        "‘´‚Ì–Í‘¢‚Æ’˜",
        "j•Ÿ‚·‚×‚«‹x‘§",
        "V‘n¢",
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
        sinnerName = "¢ŠE‚ÅÅ‰‚É‹ó‚¢‚½ŒŠ";
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
        spriteRenderer.color = new Color(0.25f, 1.0f, 0.15f);
        if (genesisPhase == 8)
        {
            player.Health -= 666;
            return;
        }
        if (deliveryItems[itemID] == Mood.Exception)
        {
            int heal = 10 * genesisPhase;
            player.Heal(heal);
            string text = genesisPhase + "“ú–ÚB\n" + itemNames[genesisPhase];
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
            text = "‘ã‚ÍŠù‚Éx•¥‚í‚ê‚Ü‚µ‚½B";
            announceManager.MakeAnnounce(text);
        }
        else
        {
            text = sinnerName + "FˆÙí”­¶B";
            announceManager.MakeAnnounce(text);
        }
    }
}
