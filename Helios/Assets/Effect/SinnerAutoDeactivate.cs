using UnityEngine;
using System.Collections;

public class SinnerAutoDeactivate : MonoBehaviour
{
    [Header("Deactivate Settings")]
    [SerializeField] private GameObject target;          // Á‚µ‚½‚¢‘ÎÛi–¢w’è‚È‚ç©•ªj
    [SerializeField] private float deactivateTime = 5f;  // Á‚¦‚é‚Ü‚Å‚Ì•b”

    private void OnEnable()
    {
        StartCoroutine(DeactivateAfterTime());
    }

    private IEnumerator DeactivateAfterTime()
    {
        yield return new WaitForSeconds(deactivateTime);

        if (target != null)
        {
            target.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
