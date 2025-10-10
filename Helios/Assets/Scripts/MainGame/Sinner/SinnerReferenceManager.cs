using UnityEngine;
using static KumagaiLibrary.Unity.EventSet;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] GameObject sinnerReference;
    [SerializeField] GameObject backButton;
    [SerializeField] GameObject map;
    [SerializeField] GameObject brokker;
    const string downKey = "Down";

    struct SinnerReferenceInfomation
    {
        string sinnerName;
        string sinnerType;
        string riskLevel;
        string explanatoryText;
        int sinnerNumber;
    }

    [SerializeField] SinnerReferenceInfomation[] sinnerReferences;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetEventType(downKey, SinnerReferencePointerDown, this.gameObject);
        SetEventType(downKey,BackButtonPointerDown,backButton);
    }

    private void Update()
    {
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
}
