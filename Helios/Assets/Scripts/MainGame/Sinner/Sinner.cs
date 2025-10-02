using UnityEngine;
using UnityEngine.Rendering;
using static KumagaiLibrary.Unity.EventSet;

public class Sinner : MonoBehaviour
{
    protected enum SecureClass
    {
        Secra,
        Vigil,
        Hazra,
        Catra,
        Nulla
    }
    protected enum LiskClass
    {
        Lumenis,
        Velgra,
        Dravex,
        Zerath,
        Oblivara
    }
    protected enum Mood
    {
        /// <summary>
        /// ���
        /// </summary>
        Joy = 0,
        /// <summary>
        /// ����
        /// </summary>
        Anticipation,
        /// <summary>
        /// �{��
        /// </summary>
        Anger,
        /// <summary>
        /// ����
        /// </summary>
        Disgust,
        /// <summary>
        /// �߂���
        /// </summary>
        Sadness,
        /// <summary>
        /// ����
        /// </summary>
        Surprise,
        /// <summary>
        /// ����
        /// </summary>
        Fear,
        /// <summary>
        /// �M��
        /// </summary>
        Trust,
        /// <summary>
        /// ����ו�
        /// </summary>
        Exception
    }
    protected enum DamageLevel
    {
        None = 0,
        Minor = 10,
        Moderate = 20,
        Enormous = 30,
        Death = 100
    }
    private const int moods = 8;//����̐�
    protected int ReceivedItemID;//�󂯎��ו��̃A�C�e���ԍ�
    protected float[] probabilitys = new float[moods];//���ꂼ��̊m��
    protected SecureClass secureClass;//���e�N���X
    protected LiskClass liskClass;//���X�N�N���X
    protected string sinnerID;//�V�i�[�ԍ�
    protected string sinnerName;//�V�i�[��
    protected Sprite sinnerSprite;//�摜
    protected int deliveryCount;//�z�B���ꂽ��
    protected ResidenceCertificate residenceCertificate;
    protected Mood[] deliveryItems = new Mood[moods];//���g���z�B���ꂤ��ו�

    private void Awake()
    {
        residenceCertificate = GameObject.Find("ResidenceCertificate").GetComponent<ResidenceCertificate>();
        SetEventType(down, OnClick, gameObject);
        SetDeliveryItems();
    }

    /// <summary>
    /// �z�B���ꂤ��ו��̏�����
    /// </summary>
    protected void SetDeliveryItems()
    {
        deliveryItems = new Mood[8] 
        {
            Mood.Joy, 
            Mood.Anticipation, 
            Mood.Anger, 
            Mood.Disgust, 
            Mood.Sadness, 
            Mood.Surprise, 
            Mood.Fear, 
            Mood.Trust, 
        };
    }

    /// <summary>
    /// �z�B���������ɓ����������ɌĂ�
    /// </summary>
    virtual public void ReceiveDeliveryItem(int itemID)
    {
        ReceivedItemID = itemID;
        int damage = Lottery();
        if (damage != 0)
        {
            AbnormalPhenomenon();
            Damage(damage);
        }
    }
    /// <summary>
    /// �ُ픭�����ɌĂԉ��z�֐�
    /// </summary>
    virtual protected void AbnormalPhenomenon()
    {
        string str = sinnerID + "[" + sinnerName + "]:" + KumagaiLibrary.String.ColorChanger("�ُ픭��", "purple");
        Debug.Log(str);
    }
    /// <summary>
    /// ���炩�̌����Ń_���[�W����������ꍇ�ĂԊ֐�
    /// </summary>
    /// <param name="damege"></param>
    protected void Damage(int damege)
    {
        Debug.Log(KumagaiLibrary.String.ColorChanger(damege.ToString(), "purple") + "�_���[�W");
    }
    /// <summary>
    /// �z�B�\�Ɏ��g�̏���n�����ɌĂԊ֐�
    /// </summary>
    protected void SetInformation()
    {
        residenceCertificate.SetSinnerName = sinnerName;
        residenceCertificate.SetSinnerID = sinnerID;
        residenceCertificate.SetSinnerImage = sinnerSprite;
        residenceCertificate.SetSecureClass = secureClass.ToString();
        residenceCertificate.SetLiskClass = liskClass.ToString();
        residenceCertificate.SetDeliveryItems = (int[])deliveryItems.Clone();
        residenceCertificate.gameObject.SetActive(true);
    }
    /// <summary>
    /// �z�B�������A���I���s�����ɌĂԊ֐�
    /// </summary>
    /// <returns></returns>
    protected int Lottery()
    {
        DamageLevel damageLevel = DamageLevel.None;
        if (probabilitys[ReceivedItemID] < 100)
        {
            int rand = Random.Range(0, 100);
            if (rand < probabilitys[ReceivedItemID])
            {
                damageLevel = DamageLevel.Minor;
            }
        }
        else if (100 <= probabilitys[ReceivedItemID] && probabilitys[ReceivedItemID] < 200)
        {
            damageLevel = DamageLevel.Minor;
        }
        else if (200 <= probabilitys[ReceivedItemID] && probabilitys[ReceivedItemID] < 300)
        {
            damageLevel = DamageLevel.Moderate;
        }
        else if (300 <= probabilitys[ReceivedItemID] && probabilitys[ReceivedItemID] < 350)
        {
            damageLevel = DamageLevel.Enormous;
        }
        else
        {
            damageLevel = DamageLevel.Death;
        }
        return (int)damageLevel;
    }
    /// <summary>
    /// ���g���N���b�N���ꂽ���ɃC�x���g�Ƃ��ČĂ΂��֐�
    /// </summary>
    private void OnClick()
    {
        Debug.Log(sinnerName + ":�N���b�N���ꂽ");
        SetInformation();
    }
}
