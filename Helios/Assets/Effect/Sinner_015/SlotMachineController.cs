using UnityEngine;

public class SlotMachineController : MonoBehaviour
{
    [SerializeField] private Sinner_015 slotMachine;
    [SerializeField] private GameObject sinner_015;

    // カスタムシンボルスプライト（Inspectorで割り当て）
    [SerializeField] private Sprite bicycleSprite;
    [SerializeField] private Sprite diceSprite;
    [SerializeField] private Sprite deliverySprite;
    [SerializeField] private Sprite reaperSprite;
    [SerializeField] private Sprite trackSprite;


    // ★ Inspectorで編集する通常の probability
    [Header("Probability (Inspector)")]
    [SerializeField] private float bicycleProbabilities = 0.4f;
    [SerializeField] private float deliveryProbabilities = 0.5f;
    [SerializeField] private float diceProbabilities = 0.5f;
    [SerializeField] private float reaperProbabilities = 0.1f;
    [SerializeField] private float trackProbabilities = 0.5f;

    // ★ Spin時のターゲットシンボル
    [Header("Spin Target Settings")]
    [SerializeField] private Sinner_015.SlotSymbol targetSymbol = Sinner_015.SlotSymbol.Random;

    private void Start()
    {
        if (slotMachine == null)
            slotMachine = FindObjectOfType<Sinner_015>();

        slotMachine.OnReaperStopCallback = OnReaperHit;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpinSlot();
        }
    }

    private void SpinSlot()
    {
        sinner_015.SetActive(true);
        // ★ probabilityArray は null のまま渡す
        // Sinner_015 側で「nullなら probability を使う」ようにする
        Sinner_015.SymbolData[] customTable = new Sinner_015.SymbolData[]
        {
            new Sinner_015.SymbolData {
                symbol = Sinner_015.SlotSymbol.Bicycle,
                sprite = bicycleSprite,
                probability = bicycleProbabilities,
                probabilityArray = null
            },

            new Sinner_015.SymbolData {
                symbol = Sinner_015.SlotSymbol.Delivery,
                sprite = deliverySprite,
                probability = deliveryProbabilities,
                probabilityArray = null
            },

            new Sinner_015.SymbolData {
                symbol = Sinner_015.SlotSymbol.Dice,
                sprite = diceSprite,
                probability = diceProbabilities,
                probabilityArray = null
            },

            new Sinner_015.SymbolData {
                symbol = Sinner_015.SlotSymbol.Reaper,
                sprite = reaperSprite,
                probability = reaperProbabilities,
                probabilityArray = null
            },

            new Sinner_015.SymbolData {
                symbol = Sinner_015.SlotSymbol.Track,
                sprite = trackSprite,
                probability = trackProbabilities,
                probabilityArray = null
            },
        };

        slotMachine.SpinWithCustomSettings(customTable, targetSymbol);
    }

    private void OnReaperHit()
    {
        Debug.Log("[SlotMachine] Reaperが止まりました！演出開始！");
    }
}
