using UnityEngine;

public class Sinner008SE : MonoBehaviour
{
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private AudioClip sinner_008;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnEnable()
    {
        audioManager = GameObject.Find("Audio").GetComponent<AudioManager>();
        audioManager.PlaySE(sinner_008);
    }
}
