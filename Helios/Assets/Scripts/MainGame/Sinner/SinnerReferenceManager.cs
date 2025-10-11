using UnityEngine;
using UnityEngine.UI;
using static KumagaiLibrary.Unity.EventSet;

public class SinnerReferenceManager : MonoBehaviour
{
    [SerializeField] private GameObject sinnerReference;
    [SerializeField] private GameObject backButton;
    [SerializeField] private GameObject map;
    [SerializeField] private GameObject brokker;
    [SerializeField] private Text sinnerNameText;
    [SerializeField] private Text sinnerTypeText;
    [SerializeField] private Text riskLevelText;
    [SerializeField] private Text[] explanatoryText;

    const string downKey = "Down";


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetEventType(downKey, SinnerReferencePointerDown, this.gameObject);
        SetEventType(downKey, BackButtonPointerDown, backButton);
    }

    private void Update()
    {
        brokker.transform.position = transform.position + map.transform.localPosition;

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
        sinnerTypeText.text = type;
        riskLevelText.text = level;
        for (int i = 0; i < explanatory.Length; i++)
        {
            explanatoryText[i].text = explanatory[i];

        }
    }
}