
using UnityEngine;
using UnityEngine.UI;
using static KumagaiLibrary.Unity.EventSet;

public class SinnerReference : SinnerReferenceBase
{

    
    [SerializeField]
    SinnerInfomation info;

    GameObject reference;
    SinnerReferenceManager manager;
    Text nameText;
    private void Start()
    {

        reference = GameObject.Find("ReferenceData").gameObject;
        manager = reference.GetComponent<SinnerReferenceManager>();
        SetEventType("Down", PointerDown, this.gameObject);
        nameText = this.gameObject.transform.GetChild(0).GetComponent<Text>(); 
        nameText.text = info.name;
        info.thisObject=this.gameObject;
        manager.sinnerInfomations.Add(info);    

    }


    private void Upadate()
    {
       
    }
    public void PointerDown()
    {
        Debug.Log(info.name);
        manager.SetData(info.name, info.type, info.risk, info.condition);
    }

   
}
