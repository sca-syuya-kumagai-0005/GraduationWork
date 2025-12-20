using UnityEngine;
public class ItemID_004 : Sinner
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        secureClass = SecureClass.Secra;
        liskClass = LiskClass.Lumenis;
        probabilitys = new float[8] { 0.0f, 0.0f, 100.0f, 100.0f, 75.0f, 0.0f, 50.0f, 0.0f };
        sinnerID = "ItemID_004";
        sinnerName = "…–Ê‚Æ–¾‹¾";
        LoadSprite("ID004");
        //effect = GameObject.Find("Effect").transform.Find("Ripple_004").gameObject;
        //GetEffectObject(false);
        effectTimer = 5.0f;
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
