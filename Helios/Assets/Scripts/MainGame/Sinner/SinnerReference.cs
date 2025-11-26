
using UnityEngine;
using UnityEngine.UI;
using static KumagaiLibrary.Unity.EventSet;

public class SinnerReference : MonoBehaviour
{
    [SerializeField] private TextAsset sinnerCsv;
    


    GameObject reference;
    SinnerReferenceManager manager;
    Text nameText;
    

    private void Start()
    {
        reference = GameObject.Find("ReferenceData").gameObject;
        manager = reference.GetComponent<SinnerReferenceManager>();
        SetEventType("Down", PointerDown, this.gameObject);
        nameText=this.gameObject.transform.GetChild(0).GetComponent<Text>();

    }


    private void Upadate()
    {
       
    }
    public void PointerDown()
    {
        //Debug.Log(sinnerName);
        //manager.SetData(sinnerName,type,risk,explanatory);
    }

   
}
