using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
public class ItemID_003 : Sinner
{
    private int genesisPhase;
    private const float timeRimit = 30.0f;
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
        effect = GameObject.Find("Effect").transform.Find("").gameObject;
        GetEffectObject(true);
        genesisPhase = 1;
        timer = 0.0f;
    }
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timeRimit)
        {
            AbnormalPhenomenon();
            timer = 0.0f;
        }
    }
    public override void ReceiptDeliveryInformation(int itemID, int deliveryProcessID, int deliveryLineID)
    {
        if (genesisPhase == 8)
        {
            player.Health -= 666;
            return;
        }
        if (deliveryItems[itemID] == Mood.Exception)
        {
            int heal = 10 * genesisPhase;
            player.Health += heal;
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
    }
    protected override void AbnormalPhenomenon()
    {
        int damage = 5 * genesisPhase;
        player.Health -= damage;
        string text;
        if (timer >= timeRimit)
        {
            text = "‘ã‚ÍŠù‚Éx•¥‚í‚ê‚Ü‚µ‚½B";
            announceManager.MakeAnnounce(text);
        }
        else
        {
            text = sinnerID + "FˆÙí”­¶B";
            announceManager.MakeAnnounce(text);
        }
    }
}
