using UnityEngine;

public class ItemID_001 : Sinner
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        secureClass = SecureClass.Catra;
        liskClass = LiskClass.Oblivara;
        probabilitys = new float[] { 0.0f, 0.0f, 25.0f, 50.0f, 100.0f, 80.0f, 50.0f, 0.0f };
        AbnormalPhenomenon(ItemName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void AbnormalPhenomenon(string objectName)
    {
        //�S�Ăُ̈�ɂ����ċ��ʂŋN���鎖������΁���ύX
        base.AbnormalPhenomenon(objectName);

        //���ꂼ��̏����͂����ɏ���

    }
}
