using UnityEngine;
public class ItemID_016 : Sinner
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        secureClass = SecureClass.Vigil;
        liskClass = LiskClass.Velgra;
        probabilitys = new float[8] { 100.0f, 100.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 100.0f };
        sinnerID = "ItemID_016";
        sinnerName = "¢ŠE‚Ì”`‚«‘‹";
        LoadSprite("ID016");
        effect = effectObjectParent.transform.GetChild(15).gameObject;
    }
    // Update is called once per frame
    void Update()
    {

    }
    public override void AbnormalPhenomenon()
    {
        //‘S‚Ä‚ÌˆÙí‚É‚¨‚¢‚Ä‹¤’Ê‚Å‹N‚«‚é–‚ª‚ ‚ê‚Î«‚ğ•ÏX
        base.AbnormalPhenomenon();

        //‚»‚ê‚¼‚ê‚Ìˆ—‚Í‚±‚±‚É‘‚­
    }
}
