using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotMachine3Lane : MonoBehaviour
{
    public enum SlotSymbol { Bicycle, Delivery, Dice, Reaper, Track, Random }

    [System.Serializable]
    public class SymbolData
    {
        public SlotSymbol symbol;
        public Sprite sprite;
        [Range(0f, 1f)] public float probability = 0.2f;
    }

    [Header("=== Visibility Control ===")]
    public bool isVisible = true;

    [Header("=== Reel Parents ===")]
    public Transform[] parents;

    [Header("=== Symbol Prefab ===")]
    public GameObject symbolPrefab;

    [Header("=== Symbol Table ===")]
    public SymbolData[] symbolTable;

    [Header("=== Stop Settings ===")]
    public SlotSymbol target;

    [Header("=== Spin Settings ===")]
    public int symbolCount = 10;
    public float symbolSpacing = 1.5f;
    public float scrollSpeed = 3f;
    public float spinDuration = 3f;

    private Reel[] reels;

    [Header("=== Fade Settings ===")]
    [Range(0f, 1f)] public float startAlpha = 0f;
    [Range(0f, 1f)] public float endAlpha = 1f;
    public float duration = 1f;
    public float reelDuration = 1f;
    public float spinDelay = 0f;

    [Header("Target Renderers")]
    public List<SpriteRenderer> renderers = new List<SpriteRenderer>();

    [Header("Target Reels")]
    public Transform[] targetReels;

    [Header("=== Spin Start Delay ===")]
    public float manualSpinDelay = 0f;
    public bool overrideSpinDelay = false;

    [Header("=== Reaper Count ===")]
    public int totalReaperCount = 0;

    // 追加: Reelがリーパー止めた通知
    public void OnReaperStop()
    {
        totalReaperCount++;
        Debug.Log($"[SlotMachine] Reaper Hit! Total = {totalReaperCount}");
    }

    // SetActive(true)でフェード→スロット開始
    void OnEnable()
    {
        totalReaperCount = 0;

        foreach (var sr in renderers)
            SetAlpha(sr, startAlpha);

        if (reels != null)
            foreach (var r in reels)
                r.Initialize();

        StopAllCoroutines();
        StartCoroutine(AnimationSequence());
    }

    void Start()
    {
        if (parents.Length != 3)
        {
            Debug.LogError("Parentsは3つ指定してください");
            return;
        }

        reels = new Reel[3];
        for (int i = 0; i < 3; i++)
            reels[i] = new Reel(parents[i], symbolPrefab, symbolTable, symbolCount, symbolSpacing, this);

        foreach (var sr in renderers)
            SetAlpha(sr, startAlpha);
    }

    public void StartSlotFromFade()
    {
        StopAllCoroutines();
        StartCoroutine(AnimationSequence());
    }

    public void SetVisibility(bool visible)
    {
        isVisible = visible;

        foreach (var sr in renderers)
            if (sr) sr.enabled = visible;

        if (visible && reels != null)
        {
            foreach (var reel in reels)
                reel.Initialize();

            StopAllCoroutines();
            StartCoroutine(AnimationSequence());
        }
    }

    private IEnumerator AnimationSequence()
    {
        foreach (var reel in targetReels)
            if (reel != null)
                StartCoroutine(FadeReelAlpha(reel, startAlpha, endAlpha, reelDuration));

        yield return StartCoroutine(FadeCoroutine());

        float delay = overrideSpinDelay ? manualSpinDelay : spinDelay;
        yield return new WaitForSeconds(delay);

        SpinAll();
    }

    private void SetAlpha(SpriteRenderer sr, float alpha)
    {
        if (!sr) return;
        var c = sr.color;
        c.a = alpha;
        sr.color = c;
    }

    public void AddRenderer(SpriteRenderer sr)
    {
        if (sr && !renderers.Contains(sr))
        {
            renderers.Add(sr);
            SetAlpha(sr, startAlpha);
        }
    }

    public IEnumerator FadeReelAlpha(Transform reel, float fromAlpha, float toAlpha, float duration)
    {
        SpriteRenderer[] children = reel.GetComponentsInChildren<SpriteRenderer>();
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            float alpha = Mathf.Lerp(fromAlpha, toAlpha, t);

            foreach (var sr in children)
                if (sr) { var c = sr.color; c.a = alpha; sr.color = c; }

            yield return null;
        }

        foreach (var sr in children)
            if (sr) { var c = sr.color; c.a = toAlpha; sr.color = c; }
    }

    private IEnumerator FadeCoroutine()
    {
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            foreach (var sr in renderers)
                if (sr) { var c = sr.color; c.a = Mathf.Lerp(startAlpha, endAlpha, t); sr.color = c; }

            yield return null;
        }

        foreach (var sr in renderers)
            if (sr) { var c = sr.color; c.a = endAlpha; sr.color = c; }
    }

    void Update()
    {
        foreach (var reel in reels)
            reel?.Update();
    }

    public void SpinAll()
    {
        float[] laneDelays = new float[3] { 0f, 0.3f, 0.6f };

        if (target == SlotSymbol.Random)
        {
            SlotSymbol[] results = new SlotSymbol[3];
            results[0] = reels[0].RandomSymbol();
            results[1] = reels[1].RandomSymbol();

            SlotSymbol r;
            do r = reels[2].RandomSymbol();
            while (r == results[0] && r == results[1]);
            results[2] = r;

            for (int i = 0; i < 3; i++)
                reels[i].StartSpin(scrollSpeed, spinDuration + laneDelays[i], results[i]);
        }
        else
        {
            for (int i = 0; i < 3; i++)
                reels[i].StartSpin(scrollSpeed, spinDuration + laneDelays[i], target);
        }
    }

    public void StopIndividual(int lane)
    {
        if (lane < 1 || lane > 3) return;
        reels[lane - 1].StopReel(target);
    }

    //================= REEL CLASS ======================
    private class Reel
    {
        private List<Transform> symbols = new();
        private Transform parent;
        private GameObject prefab;
        private SymbolData[] table;
        private int count;
        private float spacing;
        private SlotMachine3Lane owner;

        private MonoBehaviour coroutineOwner;
        private float scrollSpeed;
        private SlotSymbol targetSymbol;
        private float totalHeight;

        private bool isStopping = false;
        private bool changeSprite = false;
        public int reaperCount = 0;

        public Reel(Transform parent, GameObject prefab, SymbolData[] table, int count, float spacing, SlotMachine3Lane owner)
        {
            this.parent = parent;
            this.prefab = prefab;
            this.table = table;
            this.count = count;
            this.spacing = spacing;
            this.owner = owner;

            coroutineOwner = parent.GetComponent<MonoBehaviour>() ?? parent.gameObject.AddComponent<MonoBehaviourProxy>();

            for (int i = 0; i < count; i++)
            {
                var obj = Object.Instantiate(prefab, parent);
                obj.transform.localPosition = new Vector3(0, -i * spacing + 4, 0);
                var sr = obj.GetComponent<SpriteRenderer>();
                sr.sprite = GetSpriteFromEnum(RandomSymbol());
                owner.SetAlpha(sr, owner.startAlpha);
                owner.AddRenderer(sr);
                symbols.Add(obj.transform);
            }

            totalHeight = spacing * count;
        }

        public void Initialize()
        {
            changeSprite = false;
            isStopping = false;
            scrollSpeed = 0f;
            reaperCount = 0;

            for (int i = 0; i < symbols.Count; i++)
            {
                symbols[i].localPosition = new Vector3(0, -i * spacing + 4, 0);
                var sr = symbols[i].GetComponent<SpriteRenderer>();
                sr.sprite = GetSpriteFromEnum(RandomSymbol());
                owner.SetAlpha(sr, owner.startAlpha);
            }
        }

        public void StartSpin(float speed, float delay, SlotSymbol target)
        {
            scrollSpeed = speed;
            targetSymbol = target;
            isStopping = false;
            changeSprite = false;
            coroutineOwner.StartCoroutine(StopAfterRoutine(delay));
        }

        private IEnumerator StopAfterRoutine(float delay)
        {
            yield return new WaitForSeconds(delay);
            StopReel(targetSymbol);
        }

        public void StopReel(SlotSymbol symbol)
        {
            targetSymbol = symbol;
            isStopping = true;
        }

        public void Update()
        {
            if (scrollSpeed <= 0) return;

            foreach (var s in symbols)
            {
                if (!isStopping && !changeSprite)
                    s.localPosition -= new Vector3(0, scrollSpeed * Time.deltaTime, 0);

                if (s.localPosition.y < -totalHeight / 2f && !changeSprite)
                {
                    s.localPosition += new Vector3(0, totalHeight, 0);
                    if (!isStopping)
                        s.GetComponent<SpriteRenderer>().sprite = GetSpriteFromEnum(RandomSymbol());
                }
            }

            if (isStopping)
            {
                scrollSpeed -= Time.deltaTime * 8f;
                scrollSpeed = Mathf.Max(scrollSpeed, 0f);

                if (!changeSprite)
                {
                    changeSprite = true;
                    coroutineOwner.StartCoroutine(SnapToTarget());
                }
            }
        }

        public Transform GetCenterSymbol()
        {
            float centerOffset = -2f;
            Transform closest = symbols[0];
            float minDist = Mathf.Abs(symbols[0].localPosition.y - centerOffset);

            foreach (var s in symbols)
            {
                float d = Mathf.Abs(s.localPosition.y - centerOffset);
                if (d < minDist)
                {
                    minDist = d;
                    closest = s;
                }
            }
            return closest;
        }

        private IEnumerator SnapToTarget()
        {
            scrollSpeed = 0f;
            isStopping = false;

            float centerOffset = -2f;
            float halfHeight = totalHeight / 2f;

            Transform closest = GetCenterSymbol();
            var sr = closest.GetComponent<SpriteRenderer>();
            sr.sprite = GetSpriteFromEnum(targetSymbol);

            int centerIndex = symbols.IndexOf(closest);

            for (int i = 0; i < symbols.Count; i++)
            {
                int offsetIndex = i - centerIndex;
                float y = centerOffset - offsetIndex * spacing;

                if (y < -halfHeight) y += totalHeight;
                if (y > halfHeight) y -= totalHeight;

                symbols[i].localPosition = new Vector3(0f, y, 0f);
            }

            float overshoot = -0.2f;
            float duration = 0.15f;
            float time = 0f;

            foreach (var s in symbols)
                s.localPosition += new Vector3(0, overshoot, 0);

            while (time < duration)
            {
                time += Time.deltaTime;
                float t = Mathf.SmoothStep(0, 1, time / duration);
                foreach (var s in symbols)
                {
                    float targetY = s.localPosition.y - overshoot * t;
                    s.localPosition = new Vector3(0, targetY, 0);
                }
                yield return null;
            }

            for (int i = 0; i < symbols.Count; i++)
            {
                float y = Mathf.Round(symbols[i].localPosition.y / spacing) * spacing;

                if (Mathf.Abs(y - centerOffset) < spacing * 0.3f)
                    y = centerOffset;

                if (y < -halfHeight) y += totalHeight;
                if (y > halfHeight) y -= totalHeight;

                symbols[i].localPosition = new Vector3(0, y, 0);
            }

            changeSprite = false;

            //=== 中央リーパーカウント ===
            Debug.Log($"[STOP] {sr.sprite.name}");

            if (sr.sprite.name == "grim reaper_0")
            {
                reaperCount++;
                owner.OnReaperStop(); // 追加: 親に報告
            }

            Debug.Log($"Reel Reaper Count: {reaperCount}");
        }

        public SlotSymbol RandomSymbol()
        {
            float total = 0;
            foreach (var d in table)
                if (d.symbol != SlotSymbol.Random) total += d.probability;

            float r = Random.Range(0f, total), sum = 0;
            foreach (var d in table)
            {
                if (d.symbol == SlotSymbol.Random) continue;
                sum += d.probability;
                if (r <= sum) return d.symbol;
            }
            return table[0].symbol;
        }

        private Sprite GetSpriteFromEnum(SlotSymbol symbol)
        {
            foreach (var data in table)
                if (data.symbol == symbol) return data.sprite;
            Debug.LogError($"Sprite not found for symbol: {symbol}");
            return null;
        }
    }

    private class MonoBehaviourProxy : MonoBehaviour { }
}
