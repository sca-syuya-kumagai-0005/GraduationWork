using UnityEngine;
public class ItemID_002 : Sinner
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        secureClass = SecureClass.Secra;
        liskClass = LiskClass.Velgra;
        probabilitys = new float[8] { 0.0f, 0.0f, 200.0f, 200.0f, 100.0f, 0.0f, 350.0f, 25.0f };
        sinnerID = "ItemID_002";
        sinnerName = "�g����";
        LoadSprite("atokaraireru");
        effect = GameObject.Find("Effect").transform.Find("Fog_001_VFX").gameObject;
    }
    // Update is called once per frame
    void Update()
    {

    }
    protected override void AbnormalPhenomenon()
    {
        //�S�Ăُ̈�ɂ����ċ��ʂŋN���鎖������΁���ύX
        base.AbnormalPhenomenon();

        //���ꂼ��̏����͂����ɏ���
    }
}
