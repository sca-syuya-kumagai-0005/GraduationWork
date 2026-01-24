using UnityEngine;

public class SinnerAnimation : MonoBehaviour
{
    [Header("Deactivate Target")]
    [SerializeField] private GameObject target;

    [SerializeField] private AudioManager audioManager;
    [SerializeField] private AudioClip sinner_009;
    [SerializeField] private AudioClip sinner_013;
    [SerializeField] private AudioClip sinner_014;
    [SerializeField] private AudioClip sinner_023;

    public void Sinner009()
    {
        audioManager = GameObject.Find("Audio").GetComponent<AudioManager>();
        audioManager.PlaySE(sinner_009);
    }

    public void Sinner013()
    {
        audioManager = GameObject.Find("Audio").GetComponent<AudioManager>();
        audioManager.PlaySE(sinner_013);
    }

    public void Sinner014()
    {
        audioManager = GameObject.Find("Audio").GetComponent<AudioManager>();
        audioManager.PlaySE(sinner_014);
    }

    public void Sinner023()
    {
        audioManager = GameObject.Find("Audio").GetComponent<AudioManager>();
        audioManager.PlaySE(sinner_023);
    }

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
