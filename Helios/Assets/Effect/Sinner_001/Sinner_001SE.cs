using UnityEngine;

public class Sinner_001SE : MonoBehaviour
{
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private AudioClip sinner_001;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnEnable()
    {
        audioManager = GameObject.Find("Audio").GetComponent<AudioManager>();
        audioManager.PlaySE(sinner_001);
    }
}
