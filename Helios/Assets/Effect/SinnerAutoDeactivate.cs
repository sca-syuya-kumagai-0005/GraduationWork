using UnityEngine;
using System.Collections;

public class SinnerAutoDeactivate : MonoBehaviour
{
    [Header("Deactivate Settings")]
    [SerializeField] private GameObject target;          // è¡ÇµÇΩÇ¢ëŒè€Åiñ¢éwíËÇ»ÇÁé©ï™Åj
    [SerializeField] private float deactivateTime = 5f;  // è¡Ç¶ÇÈÇ‹Ç≈ÇÃïbêî

    [SerializeField] private AudioManager audioManager;
    [SerializeField] private AudioClip sinner_006;

    private void OnEnable()
    {
        audioManager = GameObject.Find("Audio").GetComponent<AudioManager>();
        audioManager.PlaySE(sinner_006);
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
    public void SE()
    {

    }
}
