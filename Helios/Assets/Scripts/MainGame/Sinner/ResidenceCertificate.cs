using UnityEngine;
using UnityEngine.UI;

public class ResidenceCertificate : Sinner
{
    private GameObject information;
    private Image sinnerImage;
    public Sprite SetSinnerImage { set { sinnerImage.sprite = value; } }
    private int[] deliveryItems = new int[8];
    public int[] SetDeliveryItems { set { deliveryItems = value; } }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetEventType(down, OnClick);
        sinnerImage=transform.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnClick()
    {
        Debug.Log("èZñØï[:ÉNÉäÉbÉNÇ≥ÇÍÇΩ");
    }
}
