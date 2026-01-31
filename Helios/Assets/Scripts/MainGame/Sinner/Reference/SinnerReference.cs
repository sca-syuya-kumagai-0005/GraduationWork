
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using static KumagaiLibrary.Unity.EventSet;

public class SinnerReference : SinnerReferenceBase
{

    
    [SerializeField]
    SinnerInfomation info;
    SpecifyingDeliveryRoutes sDR;
    GameObject reference;
    SinnerReferenceManager manager;
    Text nameText;
    char[] pool = {'!','"','#','$','%','&','(',')','=','-','^','~','|','@','[','{','`','+',';','*',':','}',']','<',',','>','.','?','/','_'};
    const string SINNER_006 = "ŒN‚Æ•à‚ñ‚¾ƒNƒ‰ƒQ";
    private void Start()
    {

        reference = GameObject.Find("ReferenceData").gameObject;
        manager = reference.GetComponent<SinnerReferenceManager>();
        sDR = GameObject.Find("Drivers").gameObject.GetComponent<SpecifyingDeliveryRoutes>();
        SetEventType("Down", PointerDown, this.gameObject);
        nameText = this.gameObject.transform.GetChild(0).GetComponent<Text>(); 
        nameText.text = (info.id+1).ToString()+" "+info.name;
        info.thisObject=this.gameObject;
        manager.sinnerInfomations.Add(info);
    }


    private void Upadate()
    {
       
    }
    public void PointerDown()
    {
        Debug.Log(info.name);
        // if (sDR.SinnerDebuff[0].ContainsKey(SINNER_006)|| sDR.SinnerDebuff[1].ContainsKey(SINNER_006)|| sDR.SinnerDebuff[2].ContainsKey(SINNER_006)|| sDR.SinnerDebuff[3].ContainsKey(SINNER_006))
        if (sDR.SinnerDebuff[0].ContainsKey(SINNER_006) && sDR.SinnerDebuff[0][SINNER_006])
        {
            string name=RandomChange(info.name.Length);
            string type= RandomChange(info.type.Length);
            string risk= RandomChange(info.risk.Length);
            string[] condition=new string[info.condition.Length];
            for(int i=0;i<info.condition.Length;i++)
            {
                condition[i] = RandomChange(info.condition[i].Length);
            }
            string apperance=RandomChange(info.apperance.Length);
            string abnormal=RandomChange(info.abnormal.Length);
            string[] overView=new string[info.overView.Length];
            for(int i=0;i<overView.Length;i++)
            {
                overView[i] = RandomChange(info.overView[i].Length);
            }
            string exeplanation = RandomChange(info.exeplanation.Length);
            manager.SetData(name, type, risk, condition, abnormal, info.icon, apperance, overView, apperance, exeplanation);
        }
        else
        {
            manager.SetData(info.name, info.type, info.risk, info.condition, info.abnormal, info.icon, info.apperance, info.overView, info.apperance, info.exeplanation);
        }
    }

    private string RandomChange(int size)
    {
        string str="";
        for(int i = 0;i<size;i++)
        {
            str+=pool[Random.Range(0,pool.Length)];
        }
        return str;
    }
   
}
