using UnityEngine;

public class Sinner_014 : MonoBehaviour
{
    [SerializeField] private Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private AudioManager audioManager;
    [SerializeField] private AudioClip sinner_014;

    // Update is called once per frame
    public void onAnimationEnd()
    {
        anim.Play("Sinner_014", 0, 0);
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        audioManager = GameObject.Find("Audio").GetComponent<AudioManager>();
        audioManager.PlaySE(sinner_014);
    }
}
