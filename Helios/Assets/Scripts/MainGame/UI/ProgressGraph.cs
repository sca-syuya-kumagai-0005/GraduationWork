using UnityEngine;
using UnityEngine.UI;

public class ProgressGraph : MonoBehaviour
{
    Image image;
    private float progress = 0;
    public float GetProgres { get {  return progress; } }
    private int days = 1;
    private int norm;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        image = GetComponent<Image>();
        norm = days + 1;
    }

    // Update is called once per frame
    void Update()
    {
        image.fillAmount = progress;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            AddProgress();
        }
    }
    private void AddProgress()
    {
        progress += 1.0f / norm;
    }
}
