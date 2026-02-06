using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.Collections.Unicode;
public class ItemID_017 : Sinner
{
    bool isAbnormality;
    int longBuffCount;
    AudioClip[] audioClip=new AudioClip[6];
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        secureClass = SecureClass.Vigil;
        liskClass = LiskClass.Lumenis;
        probabilitys = new float[8] { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 35.0f, 0.0f };
        sinnerID = "ItemID_017";
        sinnerName = "マッハピエロ";
        LoadSprite("ID017");
        effect = effectObjectParent.transform.GetChild(16).gameObject;
        for (int i = 0; i < audioClip.Length; i++) 
        {
            string str = (i + 1).ToString();
            audioClip[i] = Resources.Load<AudioClip>("SinnersSE/Sinner017/shuttleRun_" + str);
            Debug.Log(audioClip[i]);
        }
        longBuffCount = 0;
        isAbnormality = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (!isAbnormality)
        {
            float speedBuff = 1.0f;
            for (int i = 0; i < longBuffCount; i++)
            {
                speedBuff *= 1.1f;
            }
            specifyingDeliveryRoutes.ClownSpeed = speedBuff;
        }
        if (Input.GetKeyDown(KeyCode.K)) Release(sinnerName);
    }
    public override void ReceiptDeliveryInformation(int itemID, int deliveryProcessID, int deliveryLineID)
    {
        if(isAbnormality)
        {
            if (deliveryProcessID == 2 && specifyingDeliveryRoutes.PassArea.Count() >= 5)
                Release(sinnerName);
        }
        bool isNotPark = false;
        if (!specifyingDeliveryRoutes.DeleveryData[deliveryLineID].Contains((int)Map.MapObjectID.PARK))
        {
            isNotPark = true;
            IncreaseProbabilitys(50.0f);
        }
        if (itemID == (int)Mood.Fear)
        {
            longBuffCount = 0;
            base.ReceiptDeliveryInformation(itemID, deliveryProcessID, deliveryLineID);
        }
        else
        {
            progressGraph.SinnerList.Remove(sinnerName);
            DeleteRanpage();
            spriteRenderer.color = new Color(0.25f, 1.0f, 0.15f);

            ItemID_031 delta = GameObject.FindAnyObjectByType<ItemID_031>();
            if (delta != null)
                StartCoroutine(delta.WeakingMood(deliveryItems[itemID], sinnerName));

            const int deadLine = 4;
            string[] texts = new string[deadLine]
            {
                "全配送ラインが加速しました。",
                "全配送ラインが更に加速しました。",
                "全配送ラインが異常なまでに加速しました。",
                "全配送ラインが人体の限界まで加速しました。",
            };
            announceManager.MakeAnnounce(texts[longBuffCount]);
            longBuffCount++;
            if (longBuffCount == deadLine)
            {
                announceManager.MakeAnnounce("全配送ラインが加速に耐えられず四散しました。");
                announceManager.MakeAnnounce("ピエロは楽しそうに笑っています。");
                player.Health -= 100;
            }

            progressGraph.AddProgress();
            Destroy(gameObject.transform.Find("DestinationPin(Clone)").gameObject);
            StartCoroutine(DeliveryProgressCheck());
        }
        if (isNotPark) IncreaseProbabilitys(-50.0f);
    }
    public override void AbnormalPhenomenon()
    {
        //全ての異常において共通で起きる事があれば↓を変更
        base.AbnormalPhenomenon();
        //それぞれの処理はここに書く
        isAbnormality = true;
        StartCoroutine(ShuttleRun());
    }
    public override void Release(string name)
    {
        isAbnormality = false;
        base.Release(sinnerName);
    }

    private IEnumerator ShuttleRun()
    {
        longBuffCount = 0;
        float[] waitTimes = new float[6]
        {
            2.3f,
            9.064f,
            8.385f,
            7.784f,
            7.726f,
            6.818f
        };
        audioManager.PlaySE(audioClip[0]);
        yield return new WaitForSeconds(waitTimes[0]);
        while (true)
        {
            int buffCount = 0;
            for (int i = 1; i < 6; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    audioManager.PlaySE(audioClip[i]);
                    buffCount++;
                    yield return new WaitForSeconds(waitTimes[i]);

                    float speedBuff = 1.0f;
                    for(int c = 0; c < buffCount; c++)
                    {
                        speedBuff *= 1.05f;
                    }
                    specifyingDeliveryRoutes.ClownSpeed = speedBuff;
                    if (!isAbnormality) yield break;
                }
            }
            for(int i = 0; i < specifyingDeliveryRoutes.IsDriving.Length; i++)
            {
                if (specifyingDeliveryRoutes.IsDriving[i])
                    specifyingDeliveryRoutes.Breaking[i] = true;
            }
        }
    }
}
