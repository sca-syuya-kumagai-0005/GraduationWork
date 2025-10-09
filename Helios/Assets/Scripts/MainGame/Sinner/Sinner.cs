using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using static KumagaiLibrary.Unity.EventSet;

public class Sinner : MonoBehaviour
{
    protected Player player;
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

    protected enum SinnerType
    {
        Humanoid,
        Abnormal,
        Area,
        Concept
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
    protected SinnerType sinnerType;//�V�i�[�̎��
    protected Sprite sinnerSprite;//�摜
    protected int deliveryCount;//�z�B���ꂽ��
    protected ResidenceCertificate residenceCertificate;
    protected Mood[] deliveryItems = new Mood[moods];//���g���z�B���ꂤ��ו�
    protected int deliveryProcessID;
    protected int deliveryLineID;

    protected AnnounceManager announceManager;
    protected GameObject effect;
    protected float effectTimer;
    protected ProgressGraph progressGraph;

    private GameStateSystem gameState;
    private void Awake()
    {
        residenceCertificate = GameObject.Find("ResidenceCertificate").GetComponent<ResidenceCertificate>();
        announceManager = GameObject.Find("AnnounceCenter").GetComponent<AnnounceManager>();
        SetEventType(down, OnClick, gameObject);
        player = GameObject.Find("Player").GetComponent<Player>();
        SetDeliveryItems();
        progressGraph = GameObject.Find("ProgressGraph").GetComponent<ProgressGraph>();
        gameState = GameObject.Find("GameState").GetComponent<GameStateSystem>();
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
    private IEnumerator EffectStop(float time)
    {
        yield return new WaitForSeconds(time);
        effect.SetActive(false);
    }
    /// <summary>
    /// �z�B���������ɓ����������ɌĂ�
    /// </summary>
    virtual public void ReceiveDeliveryInformation(int itemID,int deliveryProcessID,int deliveryLineID)
    {
        ReceivedItemID = itemID;
        this.deliveryProcessID = deliveryProcessID;
        this.deliveryLineID = deliveryLineID;
        string str = sinnerID + "�Ɂu" + deliveryItems[itemID] + "�v�̔z�B���������܂����B";
        announceManager.MakeAnnounce(str);
        int damage = Lottery();
        if (damage != 0)
        {
            AbnormalPhenomenon();
            player.fluctuationHealth(-damage);
        }
        progressGraph.AddProgress();
    }
    /// <summary>
    /// �ُ픭�����ɌĂԉ��z�֐�
    /// </summary>
    virtual protected void AbnormalPhenomenon()
    {
        string str = sinnerID + "[" + sinnerName + "]:�ُ픭���B\n�����Ɏ��ӂւ̑��Q���m�F���Ă��������B";
        announceManager.MakeAnnounce(str);
        effect.SetActive(true);
        StartCoroutine(EffectStop(effectTimer));
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
        gameState.GameState = GameStateSystem.State.DeliveryPreparation;
        SetInformation();
    }
    /// <summary>
    /// Adressable�ŉ摜��ǂݍ��ފ֐�
    /// </summary>
    /// <param name="path">�t�@�C���p�X</param>
    protected void LoadSprite(string path)
    {
        Addressables.LoadAssetAsync<Sprite>(path).Completed += handle =>
        {
            if (handle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
            {
                sinnerSprite = handle.Result;
            }
            else
            {
                Debug.LogError($"Failed to load sprite at path: {path}");
            }
        };
    }
    /// <summary>
    /// �ُ픭���m�����㏸�����鎞�ɌĂԊ֐�
    /// </summary>
    /// <param name="Increase">�㏸���̐��l</param>
    protected void IncreaseProbabilitys(float Increase)
    {
        for(int i=0;i<probabilitys.Length;i++)
        {
            probabilitys[i] += Increase;
        }
    }
}
