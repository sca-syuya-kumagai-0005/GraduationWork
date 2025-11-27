using UnityEngine;

public class Sinner_014 : MonoBehaviour
{
    [SerializeField] private Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called once per frame
    public void onAnimationEnd()
    {
        anim.Play("Sinner_014", 0, 0);
        gameObject.SetActive(false);
    }
}
