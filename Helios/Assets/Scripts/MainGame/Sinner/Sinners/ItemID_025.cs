using System.Linq;
using UnityEngine;
public class ItemID_025 : Sinner
{
    private TimeLine timeLine;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        secureClass = SecureClass.Catra;
        liskClass = LiskClass.Lumenis;
        probabilitys = new float[8] { 10.0f, 50.0f, 30.0f, 10.0f, 10.0f, 60.0f, 50.0f, 30.0f };
        sinnerID = "ItemID_025";
        sinnerName = "";
        LoadSprite("atokaraireru");
        effect = effectObjectParent.transform.GetChild(24).gameObject;
        timeLine = GameObject.Find("ClockObject").GetComponent<TimeLine>();
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
