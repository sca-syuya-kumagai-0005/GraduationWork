using UnityEngine;
using UnityEngine.UI;

public class ProgressGraph : MonoBehaviour
{
    [SerializeField]
    private Image image;
    private float progress = 0;
    public float GetProgres { get {  return progress; } }
    private int days = 1;
    private int norm;//ƒmƒ‹ƒ}A‚»‚Ì‚¤‚¿‘‚¦•û•Ï‚¦‚é
    private TimeLine timeLine;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        norm = days + 1;
        timeLine = GameObject.Find("Clock").GetComponent<TimeLine>();
    }

    // Update is called once per frame
    void Update()
    {
        image.fillAmount = progress;
        if (progress >= 1.0f)
        {
            timeLine.NextDay();
        }
    }
    public void AddProgress()
    {
        progress += 1.0f / norm;
    }
}
