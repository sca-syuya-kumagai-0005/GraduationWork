using System.Collections;
using System.Collections.Generic;
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
        /// 喜び
        /// </summary>
        Joy = 0,
        /// <summary>
        /// 期待
        /// </summary>
        Anticipation,
        /// <summary>
        /// 怒り
        /// </summary>
        Anger,
        /// <summary>
        /// 嫌悪
        /// </summary>
        Disgust,
        /// <summary>
        /// 悲しみ
        /// </summary>
        Sadness,
        /// <summary>
        /// 驚き
        /// </summary>
        Surprise,
        /// <summary>
        /// 恐れ
        /// </summary>
        Fear,
        /// <summary>
        /// 信頼
        /// </summary>
        Trust,
        /// <summary>
        /// 特殊荷物
        /// </summary>
        Exception
    }

    protected enum SinnerType
    {
        /// <summary>
        /// 人型
        /// </summary>
        Humanoid,
    }
    protected enum DamageLevel
    {
        None = 0,
        Minor = 10,
        Moderate = 20,
        Enormous = 30,
        Death = 100
    }
    private const int moods = 8;//感情の数
    protected int ReceivedItemID;//受け取る荷物のアイテム番号
    protected float[] probabilitys = new float[moods];//それぞれの確率
    protected SecureClass secureClass;//収容クラス
    protected LiskClass liskClass;//リスククラス
    protected string sinnerID;//シナー番号
    protected string sinnerName;//シナー名
    protected List<SinnerType> sinnerTypeList;//シナーの種類
    protected Sprite sinnerSprite;//画像
    protected int deliveryCount;//配達された回数
    protected ResidenceCertificate residenceCertificate;
    protected Mood[] deliveryItems = new Mood[moods];//自身が配達されうる荷物
    protected int deliveryProcessID;
    protected int deliveryLineID;

    protected AnnounceManager announceManager;
    protected GameObject effect;
    protected ProgressGraph progressGraph;

    private GameStateSystem gameState;

    protected SpecifyingDeliveryRoutes specifyingDeliveryRoutes;
    protected Map.MapObjectID mapObjectID;

    protected GameObject effectObjectParent;

    protected GameObject sinnerIconObject;

    private AudioManager audioManager;
    private AudioClip[] audioClips = new AudioClip[3];
    private string[] audioFiles=new string[3]
    {
        "Delivery_Failed",
        "Delivery_Success",
        "Warning_Sound"
    };

    private void Awake()
    {
        sinnerTypeList = new List<SinnerType>();
        residenceCertificate = GameObject.Find("ResidenceCertificate").GetComponent<ResidenceCertificate>();
        announceManager = GameObject.Find("AnnounceCenter").GetComponent<AnnounceManager>();
        SetEventType(down, OnClick, gameObject);
        player = GameObject.Find("PlayerObject").GetComponent<Player>();
        SetDeliveryItems();
        progressGraph = GameObject.Find("ProgressGraph").GetComponent<ProgressGraph>();
        gameState = GameObject.Find("GameState").GetComponent<GameStateSystem>();
        specifyingDeliveryRoutes = GameObject.Find("Drivers").GetComponent<SpecifyingDeliveryRoutes>();
        transform.GetComponent<MapObjectRequest>().HaveSinner = true;
        effectObjectParent = GameObject.Find("SinnerALLEffect").gameObject;
        transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(0.925f, 0.52f, 0.0f);
        deliveryCount = 0;
        audioManager = GameObject.Find("Audio").GetComponent<AudioManager>();
        for (int i = 0; i < audioFiles.Length; i++)
        {
            audioClips[i] = Resources.Load<AudioClip>("SinnersSE/" + audioFiles[i]);
        }
    }

    /// <summary>
    /// 配達されうる荷物の初期化
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
    /// 配達員が到着した時に呼ぶ関数
    /// </summary>
    /// <param name="itemID">配達物の番号　0～8</param>
    /// <param name="deliveryProcessID">配達方法の番号　0～2</param>
    /// <param name="deliveryLineID">配達ラインの番号　0～3</param>
    virtual public void ReceiptDeliveryInformation(int itemID, int deliveryProcessID, int deliveryLineID)
    {
        ReceivedItemID = itemID;
        this.deliveryProcessID = deliveryProcessID;
        this.deliveryLineID = deliveryLineID;
        string str = sinnerID + "に「" + deliveryItems[itemID] + "」の配達が完了しました。";
        announceManager.MakeAnnounce(str);
        int damage = Lottery(deliveryLineID);
        if (damage != 0)
        {
            specifyingDeliveryRoutes.AbnormalCount[deliveryLineID]++;
            AbnormalPhenomenon();
            player.Health -= damage;

            audioManager.PlaySE(audioClips[1]);
        }
        else
        {
            audioManager.PlaySE(audioClips[0]);
        }
            deliveryCount++;
        progressGraph.AddProgress();
        Destroy(gameObject.transform.Find("DestinationPin(Clone)").gameObject);
    }
    /// <summary>
    /// 異常発生時に呼ぶ仮想関数
    /// </summary>
    virtual public void AbnormalPhenomenon()
    {
        specifyingDeliveryRoutes.TotalAbnormal++;
        string str = sinnerName + ":異常発生。\n直ちに損害を確認してください。";
        announceManager.MakeAnnounce(str);
        effect.SetActive(true);
        audioManager.PlaySE(audioClips[2]);
    }
    /// <summary>
    /// 配達表に自身の情報を渡す時に呼ぶ関数
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
    /// 配達完了時、抽選を行う時に呼ぶ関数
    /// </summary>
    /// <returns></returns>
    protected int Lottery(int deliveryLineID)
    {
        int debuff = 0;
        float probability = probabilitys[ReceivedItemID];

        if (specifyingDeliveryRoutes.SinnerDebuff[deliveryLineID].ContainsKey("紅い糸"))
        if (specifyingDeliveryRoutes.SinnerDebuff[deliveryLineID]["紅い糸"]) debuff += 50;

        DamageLevel damageLevel = DamageLevel.None;
        probability += debuff;
        if (probability < 100)
        {
            int rand = Random.Range(0, 100);
            if (rand < probabilitys[ReceivedItemID])
            {
                damageLevel = DamageLevel.Minor;
            }
        }
        else if (100 <= probability && probability < 200)
        {
            damageLevel = DamageLevel.Minor;
        }
        else if (200 <= probability && probability < 300)
        {
            damageLevel = DamageLevel.Moderate;
        }
        else if (300 <= probability && probability < 350)
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
    /// 配達完了時、抽選を行う時に呼ぶ関数
    /// </summary>
    /// <returns></returns>
    protected int Lottery(float probability)
    {
        int debuff = 0;

        if (specifyingDeliveryRoutes.SinnerDebuff[deliveryLineID].ContainsKey("紅い糸"))
            if (specifyingDeliveryRoutes.SinnerDebuff[deliveryLineID]["紅い糸"]) debuff += 50;

        DamageLevel damageLevel = DamageLevel.None;
        probability += debuff;
        if (probability < 100)
        {
            int rand = Random.Range(0, 100);
            if (rand < probabilitys[ReceivedItemID])
            {
                damageLevel = DamageLevel.Minor;
            }
        }
        else if (100 <= probability && probability < 200)
        {
            damageLevel = DamageLevel.Minor;
        }
        else if (200 <= probability && probability < 300)
        {
            damageLevel = DamageLevel.Moderate;
        }
        else if (300 <= probability && probability < 350)
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
    /// 自身がクリックされた時にイベントとして呼ばれる関数
    /// </summary>
    private void OnClick()
    {
        gameState.GameState = GameStateSystem.State.DeliveryPreparation;
        SetInformation();
    }
    /// <summary>
    /// Adressableで画像を読み込む関数
    /// </summary>
    /// <param name="path">ファイルパス</param>
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
    protected void LoadSinnerObject()
    {
        string path = "SinnerObject";
        Addressables.LoadAssetAsync<GameObject>(path).Completed += handle =>
        {
            if (handle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
            {
                sinnerIconObject = handle.Result;
            }
            else
            {
                Debug.LogError($"Failed to load sprite at path: {path}");
            }
        };
    }
    /// <summary>
    /// 異常発生確率を上昇させる時に呼ぶ関数
    /// </summary>
    /// <param name="Increase">上昇分の数値</param>
    protected void IncreaseProbabilitys(float Increase)
    {
        for (int i = 0; i < probabilitys.Length; i++)
        {
            probabilitys[i] += Increase;
        }
    }
    /// <summary>
    /// シナーの各クラスと番号を渡す関数
    /// </summary>
    /// <param name="_secureClass">管理クラスをここに参照渡しする</param>
    /// <param name="_liskClass">被害クラスをここに参照渡しする</param>
    /// <param name="_sinnerID">シナー番号をここに参照渡しする</param>
    public void GetSinnerInformation(int _secureClass,int _liskClass,int _sinnerID)
    {
        _secureClass = (int)secureClass;
        _liskClass = (int)liskClass;
        string str = sinnerID.Split(underbar)[1];
        _sinnerID = int.Parse(str);
    }
}
