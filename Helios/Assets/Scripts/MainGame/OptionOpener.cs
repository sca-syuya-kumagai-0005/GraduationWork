using UnityEngine;
using UnityEngine.EventSystems;

public class OptionOpener : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private GameObject option;
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

            //Debug.Log("Raycast hits:");
            foreach (var hit in results)
            {
                if(hit.gameObject.name == "Option")
                {
                    Instantiate(option);
                }
            }
        }
    }


}
