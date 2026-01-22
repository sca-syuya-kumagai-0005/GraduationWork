using UnityEngine;
using UnityEngine.UI;

public class PlayModeButton : MonoBehaviour
{
    enum Mode {
        REWIND,
        SHUFFLE,
        MAX,
    }
    Mode mode;
    [SerializeField] Sprite[] sprites;
    private Button button;
    MusicList musicList;
    Image myImage;

    void Start()
    {
        mode = Mode.REWIND;
        button = GetComponent<Button>();
        myImage = GetComponent<Image>();
        musicList = Locator<MusicList>.Instance;
        button.onClick.AddListener(ModeChange);
    }

    public void ModeChange()
    {
        mode = (Mode)(((int)mode + 1) % (int)Mode.MAX);
        if (mode == Mode.REWIND)
        {
            myImage.sprite = sprites[0];
            musicList.SetQueue();
        }
        else
        {
            myImage.sprite = sprites[1];
            musicList.RandomQueue();
        }
        
    }
}
