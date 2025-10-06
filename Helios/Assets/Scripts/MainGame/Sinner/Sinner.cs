using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using static KumagaiLibrary.Unity.EventSet;

public class Sinner : MonoBehaviour
{
    private Player player;
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
    protected Sprite sinnerSprite;//画像
    protected int deliveryCount;//配達された回数
    protected ResidenceCertificate residenceCertificate;
    protected Mood[] deliveryItems = new Mood[moods];//自身が配達されうる荷物

    private AnnounceManager announceManager;
    protected GameObject effect;
    protected float effectTimer;
    private ProgressGraph progressGraph;

    private GameStateSystem gameState;
    private void Awake()
    {
        residenceCertificate = GameObject.Find("ResidenceCertificate").GetComponent<ResidenceCertificate>();
        announceManager = GameObject.Find("AnnounceCenter").GetComponent<AnnounceManager>();
        SetEventType(down, OnClick, gameObject);
        player = GameObject.Find("Player").GetComponent<Player>();
        SetDeliveryItems();
        transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.4f, 0, 1);
        progressGraph = GameObject.Find("ProgressGraph").GetComponent<ProgressGraph>();
        gameState = GameObject.Find("GameState").GetComponent<GameStateSystem>();
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
    private IEnumerator EffectStop(float time)
    {
        yield return new WaitForSeconds(time);
        effect.SetActive(false);
    }
    /// <summary>
    /// 配達員が建物に到着した時に呼ぶ
    /// </summary>
    virtual public void ReceiveDeliveryItem(int itemID)
    {
        ReceivedItemID = itemID;
        string str = sinnerID + "に「" + deliveryItems[itemID] + "」の配達が完了しました。";
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
    /// 異常発生時に呼ぶ仮想関数
    /// </summary>
    virtual protected void AbnormalPhenomenon()
    {
        string str = sinnerID + "[" + sinnerName + "]:異常発生。\n早急な鎮圧を推奨。";
        announceManager.MakeAnnounce(str);
        effect.SetActive(true);
        StartCoroutine(EffectStop(effectTimer));
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
    /// 自身がクリックされた時にイベントとして呼ばれる関数
    /// </summary>
    private void OnClick()
    {
        gameState.GameState = GameStateSystem.State.DeliveryPreparation;
        SetInformation();
    }

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
}
