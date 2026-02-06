using UnityEngine;
using UnityEngine.EventSystems;

public class TutorialOpener : MonoBehaviour
{

    [SerializeField] private GameObject tutorial;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
            
            
            foreach (var hit in results)
            {
                if (hit.gameObject.name == "TutorialButton")
                {
                    Debug.Log(hit.gameObject.name);
                    tutorial.gameObject.SetActive(true);
                }

            }
        }
    }
}
