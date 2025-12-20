using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;
using static KumagaiLibrary.Unity.EventSet;


using Unity.VisualScripting;

public class SinnerReferenceManager : SinnerReferenceBase
{
    [SerializeField] private GameObject sinnerReference;
    [SerializeField] private GameObject backButton;
    [SerializeField] private GameObject map;
    [SerializeField] private GameObject brokker;
    [SerializeField] private GameObject nextButton;
    [SerializeField] private GameObject scrollContent;
    [SerializeField] private GameObject viewPort;
    [SerializeField] private GameObject[] reference;
    [SerializeField] private Image viewPortImage;
    [SerializeField] private Image icon;
    [SerializeField] private Sprite[] sortButtonBlue;
    [SerializeField] private Sprite[] sortButtonOrange;
    [SerializeField] private Text selectSinnerName;
    [SerializeField] private Text sinnerNameText;
    [SerializeField] private Text sinnerTypeText;
    [SerializeField] private Text riskLevelText;
    [SerializeField] private Text[] conditionText;
    [SerializeField] private Text abnormalText;
    [SerializeField] private Text[] referenceMaterials;
    [SerializeField] private Text apperanceText;
    [SerializeField] private Text exeplanationText;
    [SerializeField] float selectSinnerNameTextWidth;
    int count;

    [SerializeField] 
   // [SerializeField] GameObject[] page;
   
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
        sinnerInfomations.Sort((a, b) => a.id.CompareTo(b.id));//シナーの情報を格納しているListの順番がバラバラなので一度シナー番号順に並べる
        for (int i=0;i<sinnerInfomations.Count;i++)
        {
            switch(sinnerInfomations[i].risk)
            {
                case (SinnerRisk.LUMENIS):
                    {
                        if (riskFlags.lumenis)
                        {
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
            if (displaySinners.Count <= 0)
            {
                sinnerInfomations[i].thisObject.SetActive(true);
                Debug.Log(displaySinners.Count);
                continue;
            }
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

    private void OnEnable()
    {
        count=0;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetEventType(downKey, SinnerReferencePointerDown, this.gameObject);
        SetEventType(downKey, BackButtonPointerDown, backButton);
        viewPortImage=viewPort.GetComponent<Image>();
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
        float scroll=Input.mouseScrollDelta.y;
        viewPortImage.raycastTarget=scroll!=0;
        if (Input.GetMouseButtonDown(0)) // 左クリック
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = Input.mousePosition;

            var results = new System.Collections.Generic.List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);
            if(results.Count>0)SortFlagSwitch(results[0].gameObject);

        }
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

    public void SetData(string name, string type, string level, string[] condition,string abnormal,Sprite iconImage,string extorior,string[] referenceMaterial,string apprance,string exeplanation)
    {
        sinnerNameText.text = name;
        selectSinnerName.text = name;
        float size = selectSinnerName.text.Length;
        selectSinnerName.fontSize = (int)(selectSinnerNameTextWidth / size); 
        Debug.Log((int)(size));
        sinnerTypeText.text = type;
        apperanceText.text=apprance;
        exeplanationText.text=exeplanation;
        riskLevelText.text = level;

        for (int i = 0; i < condition.Length; i++)
        {
            conditionText[i].text = condition[i];
        }
        abnormalText.text = abnormal; 
        icon.sprite=iconImage;
        for (int i = 0; i < referenceMaterials.Length; i++)
        {
            referenceMaterials[i].text = referenceMaterial[i];
        }
            
        
        Debug.Log("シナーの資料を選択されたシナーに更新しました");
    }


    public void NextPage()
    {
        pageCount++;
        //pageCount %= page.Length;
    }

    private void SortFlagSwitch(GameObject click)
    {
          
        switch (click.name)
        {
            
            case (SinnerType.SECRA):
                {
                    typeFlags.secra=!typeFlags.secra;
                    Image image = click.GetComponent<Image>();
                    if (typeFlags.secra) image.sprite = sortButtonOrange[0];
                    else image.sprite = sortButtonBlue[0];
                    break;
                }
            case (SinnerType.VIGIL):
                {
                    typeFlags.vigil = !typeFlags.vigil;
                    Image image = click.GetComponent<Image>();
                    if (typeFlags.vigil) image.sprite = sortButtonOrange[1];
                    else image.sprite = sortButtonBlue[1];
                    break;
                }
            case (SinnerType.HAZRA):
                {
                    typeFlags.hazra = !typeFlags.hazra;
                    Image image = click.GetComponent<Image>();
                    if (typeFlags.hazra) image.sprite = sortButtonOrange[2];
                    else image.sprite = sortButtonBlue[2];
                    break;
                }
            case (SinnerType.CATRA):
                {
                    typeFlags.catra = !typeFlags.catra;
                    Image image = click.GetComponent<Image>();
                    if (typeFlags.catra) image.sprite = sortButtonOrange[3];
                    else image.sprite = sortButtonBlue[3];
                    break;
                }
            case (SinnerType.NULLA):
                {
                    typeFlags.nulla = !typeFlags.nulla;
                    Image image = click.GetComponent<Image>();
                    if (typeFlags.nulla) image.sprite = sortButtonOrange[4];
                    else image.sprite = sortButtonBlue[4];
                    break;
                }
            case (SinnerRisk.LUMENIS):
                {
                    riskFlags.lumenis = !riskFlags.lumenis;
                    Image image = click.GetComponent<Image>();
                    if (riskFlags.lumenis) image.sprite = sortButtonOrange[5];
                    else image.sprite = sortButtonBlue[5];
                    break;
                }

            case (SinnerRisk.VELGRA):
                {
                    riskFlags.velgra = !riskFlags.velgra;
                    Image image = click.GetComponent<Image>();
                    if (riskFlags.velgra) image.sprite = sortButtonOrange[6];
                    else image.sprite = sortButtonBlue[6];
                    break;
                }
            case (SinnerRisk.DRAVEX):
                {
                    riskFlags.dravex = !riskFlags.dravex;
                    Image image = click.GetComponent<Image>();
                    if (riskFlags.dravex) image.sprite = sortButtonOrange[7];
                    else image.sprite = sortButtonBlue[7];
                    break;
                }
            case (SinnerRisk.ZERATH):
                {
                    riskFlags.zerath = !riskFlags.zerath;
                    Image image = click.GetComponent<Image>();
                    if (riskFlags.zerath) image.sprite = sortButtonOrange[8];
                    else image.sprite = sortButtonBlue[8];
                    break;
                }
            case (SinnerRisk.OBLIVARA):
                {
                    riskFlags.oblivara = !riskFlags.oblivara;
                    Image image = click.GetComponent<Image>();
                    if (riskFlags.oblivara) image.sprite = sortButtonOrange[9];
                    else image.sprite = sortButtonBlue[9];
                    break;
                }
            case ("ReverceButton"):
                {
                    count++;
                    count %= 2;
                    reference[count].SetActive(true);
                    reference[(count + 1) % 2].SetActive(false);
                    Debug.Log("Debug");
                    break;
                }
        }

    }


}