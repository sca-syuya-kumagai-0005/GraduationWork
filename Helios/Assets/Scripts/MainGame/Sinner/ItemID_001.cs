using UnityEngine;
using UnityEngine.UI;

public class ItemID_001 : Sinner
{
    private Image image;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        secureClass = SecureClass.Catra;
        liskClass = LiskClass.Oblivara;
        probabilitys = new float[] { 0.0f, 0.0f, 25.0f, 50.0f, 100.0f, 80.0f, 50.0f, 0.0f };
        ItemName = "憎しみの堕星";
        sinnerSprite = Resources.Load<Sprite>("");//ここに画像名
        //AbnormalPhenomenon(ItemName);
        gameObject.name = ItemName;
        SetEventType(down, OnClick);
    }
    // Update is called once per frame
    void Update()
    {

    }

    private void OnClick()
    {
        Debug.Log(ItemName+":クリックされた");
        residenceCertificate.SetSinnerImage=sinnerSprite;
    }
    protected override void AbnormalPhenomenon(string objectName)
    {
        //全ての異常において共通で起きる事があれば↓を変更
        base.AbnormalPhenomenon(objectName);

        //それぞれの処理はここに書く

    }
}
