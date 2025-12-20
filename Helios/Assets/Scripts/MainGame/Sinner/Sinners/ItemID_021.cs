using UnityEngine;
public class ItemID_021 : Sinner
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        secureClass = SecureClass.Hazra;
        liskClass = LiskClass.Lumenis;
        probabilitys = new float[8] { 200.0f, 85.0f, 30.0f, 100.0f, 25.0f, 70.0f, 150.0f, 20.0f };
        sinnerID = "ItemID_021";
        sinnerName = "Œã‰÷‚Ì—…j”Õ";
        LoadSprite("ID021");
        effect = effectObjectParent.transform.GetChild(20).gameObject;
    }
    // Update is called once per frame
    void Update()
    {

    }
    protected override void AbnormalPhenomenon()
    {
        //‘S‚Ä‚ÌˆÙí‚É‚¨‚¢‚Ä‹¤’Ê‚Å‹N‚«‚é–‚ª‚ ‚ê‚Î«‚ğ•ÏX
        base.AbnormalPhenomenon();

        //‚»‚ê‚¼‚ê‚Ìˆ—‚Í‚±‚±‚É‘‚­
    }
}
