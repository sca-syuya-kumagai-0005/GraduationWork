using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class TutorialButton : MonoBehaviour
{
    public static bool IsTutorial = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        IsTutorial = false;
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

                Debug.Log(hit.gameObject.name);
                if(hit.gameObject.name== "TutoriaObject")
                {
                    IsTutorial = true;
                    SceneManager.LoadScene("MainScene");
                }
            }
        }
    }
}
