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
        ItemName = "�����݂̑�";
        sinnerSprite = Resources.Load<Sprite>("");//�����ɉ摜��
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
        Debug.Log(ItemName+":�N���b�N���ꂽ");
        residenceCertificate.SetSinnerImage=sinnerSprite;
    }
    protected override void AbnormalPhenomenon(string objectName)
    {
        //�S�Ăُ̈�ɂ����ċ��ʂŋN���鎖������΁���ύX
        base.AbnormalPhenomenon(objectName);

        //���ꂼ��̏����͂����ɏ���

    }
}
