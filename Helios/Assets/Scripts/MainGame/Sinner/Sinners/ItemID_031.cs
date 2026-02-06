using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
public class ItemID_031 : Sinner
{
    private SinnerDistribute distribute;
    List<Sinner> allSinners;
    float timeLimit;
    float timer;
    int count;
    GameObject qliphothCounter;
    LastBossCount lastBossCount;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timeLimit = 90.0f;
        secureClass = SecureClass.Nulla;
        liskClass = LiskClass.Oblivara;
        probabilitys = new float[8] { 200.0f, 200.0f, 200.0f, 200.0f, 200.0f, 200.0f, 200.0f, 200.0f };
        sinnerID = "ItemID_031";
        sinnerName = "UnknownΔ";
        LoadSprite("ID031");
        LoadQliphothRanpage();
        LoadQliphothCounter();
        effect = effectObjectParent.transform.GetChild(27).gameObject;

        progressGraph = GameObject.Find("ProgressGraph").GetComponent<ProgressGraph>();
        distribute = GameObject.Find("Map").gameObject.GetComponent<SinnerDistribute>();
        allSinners = new List<Sinner>();
        StartCoroutine(GetAllSinner());
        timer = 0.0f;
        count = 0;
    }
    // Update is called once per frame
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timeLimit)
        {
            timeLimit = 150.0f;
            timer -= timeLimit;
            AddQliphothRanpage();
        }
        if(lastBossCount != null)
        {
            int c = 0;
            List<Sinner> tmpList = new List<Sinner>();
            for (int i = 0; i < allSinners.Count; i++)
            {
                if (allSinners[i].IsQliphothRanpage) c++;
                tmpList.Add(allSinners[i]);
            }
            if (lastBossCount.GetTimer <= 0.0f || c == 0)
            {
                for (int i = 0; i < tmpList.Count; i++) tmpList[i].DeleteRanpage();
                count += c;
                StartCoroutine(lastBossCount.FadeOut());
                lastBossCount = null;
            }
        }
        if (count >= 2)
            AbnormalPhenomenon();
    }
    public override void ReceiptDeliveryInformation(int itemID, int deliveryProcessID, int deliveryLineID)
    {
        if (progressGraph.SinnerList.Count <= 1)
        {
            base.ReceiptDeliveryInformation(itemID, deliveryProcessID, deliveryLineID);
        }
        else
        {
            announceManager.MakeAnnounce("他シナーへの配達を全て終わらせてください。");
            announceManager.MakeAnnounce("Sinner_031の軋む音が響く……。");
            count++;

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
    }
    public override void AbnormalPhenomenon()
    {
        //全ての異常において共通で起きる事があれば↓を変更
        base.AbnormalPhenomenon();
        count = 0;
        StartCoroutine(Beam());
    }
    private IEnumerator Beam()
    {
        bool isEnd = false;
        float intervalCount = 0.0f;
        const float beamInterval = 5.0f;
        float timer = 0.0f;
        const float timeLimit = 180.0f;
        while (!isEnd)
        {
            timer += Time.deltaTime;
            intervalCount += Time.deltaTime;
            if (intervalCount >= beamInterval)
            {
                intervalCount -= beamInterval;
                Debug.Log("ビーム！！！！！！！！！！");
            }
            if (timer >= timeLimit) isEnd = true;
            yield return null;
        }
    }

    private void AddQliphothRanpage()
    {
        int lotteryCount = 3;
        List<Sinner> tmpList = new List<Sinner>();
        for(int i = 0; i < lotteryCount; i++)
        {
            int rand = Random.Range(0, allSinners.Count);
            allSinners[rand].QliphothRanpage();
            tmpList.Add(allSinners[rand]);
            allSinners.Remove(allSinners[rand]);
        }
        allSinners.AddRange(tmpList);
        GameObject UICanvas = GameObject.Find("UI");
        GameObject counter=
        Instantiate(qliphothCounter, qliphothCounter.transform.position, Quaternion.identity, UICanvas.transform);
        lastBossCount = counter.transform.GetComponent<LastBossCount>();
    }
    private IEnumerator GetAllSinner()
    {
        yield return new WaitForEndOfFrame();
        for (int i = 0; i < distribute.GetSinnerHousedObjects.Length - 1; i++)
        {
            for (int j = 0; j < distribute.GetSinnerHousedObjects[i].Count; j++)
            {
                allSinners.Add(distribute.GetSinnerHousedObjects[i][j].GetComponent<Sinner>());
            }
        }
    }
    private void LoadQliphothCounter()
    {
        string path = "QliphothCounter";
        Addressables.LoadAssetAsync<GameObject>(path).Completed += handle =>
        {
            if (handle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
            {
                qliphothCounter = handle.Result;
            }
            else
            {
                Debug.LogError($"Failed to load sprite at path: {path}");
            }
        };
    }

    public IEnumerator WeakingMood(Mood mood,string sinnerName)
    {
        if (mood == Mood.Exception) yield break;
        if (sinnerName == this.sinnerName) yield break;
        bool isEnd = false;
        float t = 0.0f;
        while (!isEnd)
        {
            probabilitys[(int)mood] = 0.0f;
            t += Time.deltaTime;
            if (t >= 15.0f) isEnd = true;
            yield return null;
        }
        probabilitys[(int)mood] = 200.0f;
    }
}