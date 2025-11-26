using UnityEngine;
using UnityEngine.EventSystems;

public class Debugger : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // ç∂ÉNÉäÉbÉN
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = Input.mousePosition;

            var results = new System.Collections.Generic.List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            Debug.Log("Raycast hits:");
            foreach (var hit in results)
            {
                Debug.Log("Hit: " + hit.gameObject.name);
            }
        }
    }
}
