using UnityEngine;
using UnityEngine.UI;
public class Tutorial : MonoBehaviour
{
    [SerializeField] Sprite[] tutorialImage;
    int day;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        day = GameObject.Find("SaveManager").gameObject.GetComponent<SaveDataManager>().Days;
        Debug.Log(day);
        this.gameObject.SetActive(day==1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Close()
    {
        this.gameObject.SetActive(false);
    }

    public void Next()
    {
        
    }
} 
