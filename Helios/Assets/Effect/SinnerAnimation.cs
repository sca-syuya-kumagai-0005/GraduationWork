using UnityEngine;

public class SinnerAnimation : MonoBehaviour
{
    [Header("Deactivate Target")]
    [SerializeField] private GameObject target;

    public void Deactivate()
    {
        if (target != null)
        {
            target.SetActive(false);
        }
        else
        {
            // target–¢w’è‚Ìê‡‚Í©•ª©g‚ğ”ñ•\¦
            gameObject.SetActive(false);
        }
    }
}
