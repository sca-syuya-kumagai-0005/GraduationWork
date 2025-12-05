using UnityEngine;
public class ItemID_020 : Sinner
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        secureClass = SecureClass.Vigil;
        liskClass = LiskClass.Zerath;
        probabilitys = new float[8] { 95.0f, 85.0f, 65.0f, 75.0f, 35.0f, 100.0f, 0.0f, 70.0f };
        sinnerID = "ItemID_020";
        sinnerName = "‰Î‰Ô‚Ì–‚—";
        LoadSprite("atokaraireru");
        //effect = GameObject.Find("Effect").transform.Find("Fog_001_VFX").gameObject;
        //GetEffectObject(true);
        effectTimer = 6.5f;
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
