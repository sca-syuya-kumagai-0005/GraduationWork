using UnityEngine;
public class ItemID_016 : Sinner
{
    bool inCamera;
    float timer;
    const float timeLimit = 5.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = 0.0f;
        inCamera = false;
        BoxCollider col =
        gameObject.AddComponent<BoxCollider>();
        col.isTrigger = true;
        Debug.Log(gameObject.name);
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
        if (inCamera)
        {
            timer += Time.deltaTime;
        }
        if (timer >= timeLimit)
        {
            timer -= timeLimit;
            AbnormalPhenomenon();
        }
    }
    public override void AbnormalPhenomenon()
    {
        //‘S‚Ä‚ÌˆÙí‚É‚¨‚¢‚Ä‹¤’Ê‚Å‹N‚«‚é–‚ª‚ ‚ê‚Î«‚ğ•ÏX
        base.AbnormalPhenomenon();

        //‚»‚ê‚¼‚ê‚Ìˆ—‚Í‚±‚±‚É‘‚­
    }

    private void OnTriggerEnter(Collider other)
    {
        inCamera = true;
    }

    private void OnTriggerExit(Collider other)
    {
        timer = 0.0f;
        inCamera = false;
    }
}
