using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using static KumagaiLibrary.Unity.EventSet;
using static KumagaiLibrary.Unity.CsvManager;


using Unity.VisualScripting;

public class SinnerReferenceManager : SinnerReferenceBase
{
    [SerializeField] private GameObject sinnerReference;
    [SerializeField] private GameObject backButton;
    [SerializeField] private GameObject map;
    [SerializeField] private GameObject brokker;
    [SerializeField] private Text selectSinnerName;
    [SerializeField] private Text sinnerNameText;
    [SerializeField] private Text sinnerTypeText;
    [SerializeField] private Text riskLevelText;
    [SerializeField] private Text[] explanatoryText;
    [SerializeField] GameObject nextButton;
   // [SerializeField] GameObject[] page;
    [SerializeField] GameObject scrollContent;
    //[SerializeField]List<GameObject> contents;
   // [SerializeField] TextAsset[] sinnerDatas;
    int pageCount = 0;

    const string downKey = "Down";
    [SerializeField]
    Risk riskFlags;

    [SerializeField]
    Type typeFlags;
  

    

  

    const int SINNER_MAX= 31;
    public List<SinnerInfomation> sinnerInfomations=new List<SinnerInfomation>();//シナーのデータ全て
    [SerializeField]
    List<SinnerInfomation> displaySinners;//ソートによって表示するシナー
    //List<string> readData;
    List<SinnerInfomation> SinnerSort()
    {
        return new List<SinnerInfomation>();
    }

    public void CheckSortList()
    {
        List<SinnerInfomation> displaySinners = new List<SinnerInfomation>();//表示しない要素をまとめたList
        Debug.Log(sinnerInfomations.Count);
        Debug.Log(riskFlags.lumenis);
        sinnerInfomations.Sort((a, b) => a.id.CompareTo(b.id));//シナーの情報を格納しているListの順番がバラバラなので一度シナー番号順に並べる
        for (int i=0;i<sinnerInfomations.Count;i++)
        {
            switch(sinnerInfomations[i].risk)
            {
                case (SinnerRisk.LUMENIS):
                    {
                        if (riskFlags.lumenis)
                        {
                            Debug.Log("LUMENIS");
                            displaySinners.Add(sinnerInfomations[i]);
                            continue;
                        }
                        break;
                    }
                case (SinnerRisk.VELGRA):
                    {
                        if (riskFlags.velgra)
                        {
                            displaySinners.Add(sinnerInfomations[i]);
                            continue;
                        }
                        break;
                    }
                case (SinnerRisk.DRAVEX):
                    {
                        if (riskFlags.dravex)
                        {
                            displaySinners.Add(sinnerInfomations[i]);
                            continue;
                        }
                        break;
                    }
                case (SinnerRisk.ZERATH):
                    {
                        if (riskFlags.zerath)
                        {
                            displaySinners.Add(sinnerInfomations[i]);
                            continue;
                        }
                        break;
                    }
                case (SinnerRisk.OBLIVARA):
                    {
                        if (riskFlags.oblivara)
                        {
                            displaySinners.Add(sinnerInfomations[i]);
                            continue;
                        }
                        break;
                    }
            }

            switch(sinnerInfomations[i].type)
            {
                case (SinnerType.SECRA):
                    {
                        if (typeFlags.secra)
                        {
                            Debug.Log("SECRA");
                            displaySinners.Add(sinnerInfomations[i]);
                            continue;
                        }
                        break;
                    }
                case (SinnerType.VIGIL):
                    {
                        if (typeFlags.vigil)
                        {
                            displaySinners.Add(sinnerInfomations[i]);
                            continue;
                        }
                        break;
                    }
                case (SinnerType.HAZRA):
                    {
                        if (typeFlags.hazra)
                        {
                            displaySinners.Add(sinnerInfomations[i]);
                            continue;
                        }
                        break;
                    }
                case (SinnerType.CATRA):
                    {
                        if (typeFlags.catra)
                        {
                            displaySinners.Add(sinnerInfomations[i]);
                            continue;
                        }
                        break;
                    }
                case (SinnerType.NULLA):
                    {
                        if (typeFlags.nulla)
                        {
                            displaySinners.Add(sinnerInfomations[i]);
                            continue;
                        }
                        break;
                    }
            }
        }

        for(int i=0;i<sinnerInfomations.Count;i++)
        {
            sinnerInfomations[i].thisObject.SetActive(false);
            for (int j=0;j<displaySinners.Count;j++)
            {
                
                if (i == displaySinners[j].id)
                {
                    displaySinners[j].thisObject.SetActive(true);
                }
            }
        }
    }

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetEventType(downKey, SinnerReferencePointerDown, this.gameObject);
        SetEventType(downKey, BackButtonPointerDown, backButton);
        //KumagaiLibrary.Unity.EventSet.SetEventType("PointerDown", NextPage, nextButton);
        //for(int i=0;i<sinnerDatas.Length;i++)
        //{
        //    SinnerInfomation sinnerData;
        //    sinnerData.id = i;
        //    List<string[]> data = Read(sinnerDatas[i]);
        //    sinnerData.name = data[0][0];
        //    sinnerData.type = data[1][0];
        //    sinnerData.risk = data[2][0];
        //    sinnerData.abnormal = data[3][0];
        //    sinnerData.condition = data[4];
        //    sinnerData.appearance = data[5][0];
        //    sinnerData.explanation = data[6][0];
        //    sinnerData.interview = data[7][0];
        //    sinnerData.appendix = data[8][0];
        //    Debug.Log("name"+sinnerData.name);
        //    Debug.Log("type"+sinnerData.type);
        //    Debug.Log("risk" + sinnerData.risk);
        //    Debug.Log("abnormal" + sinnerData.abnormal);
        //    for(int n=0;n<sinnerData.condition.Length;n++)
        //    {
        //        Debug.Log("condition"+n+sinnerData.condition[n]);
        //    }
        //    Debug.Log("apperance" + data[5][0]);
        //    Debug.Log("explanation" + data[6][0]);
        //    Debug.Log(sinnerData.interview);
        //    Debug.Log(sinnerData.appendix);
            

        //}
        
        //for(int i=0;i<scrollContent.transform.childCount;i++)
        //{
        //    contents.Add(scrollContent.transform.GetChild(i).gameObject);
        //    GameObject obj = contents[i];
        //    //obj.transform.localPosition= new Vector3(obj.transform.localPosition.x,-(i*100+100),0);
        //}
        //this.gameObject.SetActive(false);
        
    }

    private void Update()
    {
        brokker.transform.position = transform.position + map.transform.localPosition;
        CheckSortList();
     
    }
    // Update is called once per frame

    public void SinnerReferencePointerDown()
    {
        sinnerReference.SetActive(true);
        brokker.SetActive(true);
    }

    public void BackButtonPointerDown()
    {
        sinnerReference.SetActive(false);
        brokker.SetActive(false);
    }

    public void SetData(string name, string type, string level, string[] explanatory)
    {
        sinnerNameText.text = name;
        selectSinnerName.text = name;
        sinnerTypeText.text = type;
        riskLevelText.text = level;
        for (int i = 0; i < explanatory.Length; i++)
        {
            explanatoryText[i].text = explanatory[i];
        }
        Debug.Log("シナーの資料を選択されたシナーに更新しました");
    }


    public void NextPage()
    {
        pageCount++;
        //pageCount %= page.Length;
    }


}