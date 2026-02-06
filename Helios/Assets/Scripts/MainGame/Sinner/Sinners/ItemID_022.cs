using System.Collections;
using UnityEngine;
public class ItemID_022 : Sinner
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        secureClass = SecureClass.Nulla;
        liskClass = LiskClass.Oblivara;
        probabilitys = new float[8] { 350.0f, 350.0f, 350.0f, 350.0f, 350.0f, 350.0f, 350.0f, 350.0f };
        sinnerID = "ItemID_022";
        sinnerName = "後奏のベル";
        LoadSprite("ID022");
        effect = effectObjectParent.transform.GetChild(21).gameObject;
        progressGraph.SinnerList.Remove(sinnerName);
        spriteRenderer.color = new Color(0.25f, 1.0f, 0.15f);
    }
    public override void ReceiptDeliveryInformation(int itemID, int deliveryProcessID, int deliveryLineID)
    {
        StartCoroutine(Deth());
    }
    public override void Release(string name)
    {

    }

    private IEnumerator Deth()
    {
        bool isEnd = false;
        float t = 0.0f;
        float[] intervals = new float[15]
        {
            0.0f,1.0f,0.9f,0.8f,0.8f,
            0.7f,0.6f,0.5f,0.4f,0.3f,
            0.1f,0.1f,0.1f,0.1f,0.5f,
        };
        string[] texts = new string[15]
        {
            "『■■■■』に配達が\n完了しました。",
            "『■■■■』に配達が\n完了しました。",
            "『■奏■■』に配達が\n完了■ました。",
            "『■奏■ル』に配達が\n完了■ま■た。",
            "『後奏■ル』に配達が\n完了し■■まし■。",
            "『後奏ベル』に配達が\n完了■■し■い■た。",
            "『後奏ベル』に配達が\n完了し■■■い■た。",
            "『後奏ベル』に配達が\n完了■■■ま■■し■。",
            "『後奏ベル』に配達が\n完了■■■■ま■■た。",
            "『後奏ベル』に配達が\n完了■てし■■ま■た。",
            "『後奏ベル』に配達が\n完了してし■いま■た。",
            "『後奏ベル』に配達が\n完了し■しまいました。",
            "『後奏ベル』に配達が\n完了してしまいました。",
            "『後奏ベル』に配達が\n完了してしまいました。",
            "『後奏ベル』に配達が\n完了してしまいました。",
        };
        audioManager.FadeOutBGM(3.0f);
        for (int i = 0; i < intervals.Length; i++)
        {
            yield return new WaitForSeconds(intervals[i]);
            announceManager.MakeAnnounce(texts[i]);
            if (i == 5)
            {
                effect.SetActive(true);
                player.Health -= 666;
            }
        }
    }
}
