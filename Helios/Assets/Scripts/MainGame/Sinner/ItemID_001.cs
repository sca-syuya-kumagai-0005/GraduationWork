using UnityEngine;
using UnityEngine.UI;

public class ItemID_001 : Sinner
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        secureClass = SecureClass.Catra;
        liskClass = LiskClass.Oblivara;
        probabilitys = new float[8] { 0.0f, 0.0f, 25.0f, 50.0f, 100.0f, 80.0f, 50.0f, 0.0f };
        sinnerID = "ItemID_001";
        sinnerName = "�����݂̑�";
        sinnerSprite = Resources.Load<Sprite>("");//�����ɉ摜��
        deliveryItems = new Moods[8] { Moods.Joy, Moods.Anticipation, Moods.Anger, Moods.Disgust, Moods.Sadness, Moods.Surprise, Moods.Fear, Moods.Trust, };
        SetEventType(down, OnClick);
    }
    // Update is called once per frame
    void Update()
    {

    }

    private void OnClick()
    {
        Debug.Log(sinnerName + ":�N���b�N���ꂽ");
        SetInformation();
    }
    protected override void AbnormalPhenomenon(string objectName)
    {
        //�S�Ăُ̈�ɂ����ċ��ʂŋN���鎖������΁���ύX
        base.AbnormalPhenomenon(objectName);

        //���ꂼ��̏����͂����ɏ���

    }
}
