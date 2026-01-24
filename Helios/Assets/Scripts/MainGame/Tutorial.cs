using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
public class Tutorial : MonoBehaviour
{
    [SerializeField] Sprite[] tutorialSprite;
    int day;
    [SerializeField]float delayTime;
    [SerializeField]Image tutorialImage;
    int pageCount = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tutorialImage.sprite = tutorialSprite[0];
        day = GameObject.Find("SaveManager").gameObject.GetComponent<SaveDataManager>().Days;
        StartCoroutine(Delay());
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // ç∂ÉNÉäÉbÉN
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = Input.mousePosition;

            var results = new System.Collections.Generic.List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            //Debug.Log("Raycast hits:");
            foreach (var hit in results)
            {
                if(hit.gameObject.name=="NextButton")
                {
                    Next();
                }
                
                if(hit.gameObject.name=="BackButton")
                {
                    Back();
                }

                if(hit.gameObject.name=="CloseButton")
                {
                    Close();
                }
            }
        }
    }

    public void Close()
    {
        this.gameObject.SetActive(false);
    }

    private void Next()
    {
        pageCount++;
        pageCount%=tutorialSprite.Length;
        tutorialImage.sprite = tutorialSprite[pageCount];
    }

    private void Back()
    {
        pageCount--;
        if(pageCount==-1)
        {
            pageCount = tutorialSprite.Length-1;
        }
        tutorialImage.sprite = tutorialSprite[pageCount];
    }
    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(delayTime);
        for (int i = 0; i < this.gameObject.transform.childCount; i++)
        {
            this.gameObject.transform.GetChild(i).gameObject.SetActive(day==1);
        }
    }
} 
