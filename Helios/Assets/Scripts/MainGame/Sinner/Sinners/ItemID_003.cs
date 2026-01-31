using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
public class ItemID_003 : Sinner
{
    private int genesisPhase;
    private const float timeLimit = 30.0f;
    private float timer;
    private string[] itemNames = new string[9]
    {
        "",
        "明かりと暗がり",
        "天上と天下の水",
        "繁栄する陸と海",
        "星と日と月の輪",
        "翼と鰭の遍く命",
        "其の模造と秩序",
        "祝福すべき休息",
        "新創世",
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
        sinnerName = "世界で最初に空いた穴";
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
            string text = genesisPhase + "日目。\n" + itemNames[genesisPhase];
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

        if (progressGraph.SinnerList.Count != 0 && gameState.GameState != GameStateSystem.State.End)
        {
            announceManager.MakeAnnounce("本日のノルマは達成しましたが、全シナーへの配達が未完了です。");
            announceManager.MakeAnnounce("全シナーへの配達を完了し本日の業務を終了して下さい。");
        }
    }
    public override void AbnormalPhenomenon()
    {
        int damage = 5 * genesisPhase;
        player.Health -= damage;
        string text;
        if (timer >= timeLimit)
        {
            text = "代償は既に支払われました。";
            announceManager.MakeAnnounce(text);
        }
        else
        {
            text = sinnerID + "：異常発生。";
            announceManager.MakeAnnounce(text);
        }
    }
}
