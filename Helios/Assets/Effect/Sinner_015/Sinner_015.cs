using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sinner_015 : MonoBehaviour
{
    // ================== ENUM ==================
    public enum SlotSymbol { Bicycle, Delivery, Dice, Reaper, Track, Random }

    // ================== SYMBOL DATA ==================
    [System.Serializable]
    public class SymbolData
    {
        public SlotSymbol symbol;
        public Sprite sprite;

        // 配列対応（外部用）
        public float[] probabilityArray;

        // Inspector用の単体値
        public float probability = 0.2f;
    }



    // ================== INSPECTOR SETTINGS ==================
    [Header("=== Visibility Control ===")]
    [SerializeField] private bool isVisible = true;

    [Header("=== Reel Parents ===")]
    [SerializeField] private Transform[] parents;

    [Header("=== Symbol Prefab ===")]
    [SerializeField] private GameObject symbolPrefab;

    [Header("=== Symbol Table ===")]
    [SerializeField] private SymbolData[] symbolTable;

    [Header("=== Stop Settings ===")]
    [SerializeField] private SlotSymbol target;

    [Header("=== Spin Settings ===")]
    [SerializeField] private int symbolCount = 10;
    [SerializeField] private float symbolSpacing = 1.5f;
    [SerializeField] private float scrollSpeed = 3f;
    [SerializeField] private float spinDuration = 3f;

    [Header("=== Fade Settings ===")]
    [SerializeField, Range(0f, 1f)] private float startAlpha = 0f;
    [SerializeField, Range(0f, 1f)] private float endAlpha = 1f;
    [SerializeField] private float duration = 1f;
    [SerializeField] private float reelDuration = 1f;
    [SerializeField] private float spinDelay = 0f;

    [Header("Target Renderers")]
    [SerializeField] private List<SpriteRenderer> renderers = new List<SpriteRenderer>();

    [Header("Target Reels")]
    [SerializeField] private Transform[] targetReels;

    [Header("=== Spin Start Delay ===")]
    [SerializeField] private float manualSpinDelay = 0f;
    [SerializeField] private bool overrideSpinDelay = false;

    [Header("=== Reaper Count ===")]
    public int ReaperCount { get; private set; } = 0;

    [Header("=== Reel Stop Delay Settings ===")]
    [SerializeField] private float[] laneDelays = new float[3] { 0f, 0.3f, 0.6f };

    [Header("=== Center Object Animation ===")]
    [SerializeField] private List<Transform> centerObjects = new List<Transform>();
    [SerializeField] private Vector3 startScale = Vector3.one;
    [SerializeField] private Vector3 endScale = Vector3.one * 1.5f;
    [SerializeField] private float scaleDuration = 1.0f;
    private int stoppedReelCount = 0;

    [Header("=== Center Object Rotation ===")]
    [SerializeField] private Vector3 rotationStart = new Vector3(0f, 0f, -15f);
    [SerializeField] private Vector3 rotationEnd = new Vector3(0f, 0f, 15f);
    [SerializeField] private float rotationDuration = 1.0f;
    [SerializeField] private int rotationLoopCount = 2;

    [SerializeField] private GameObject reaperParticle;
    [SerializeField] private GameObject generalPurposeParticle;
    [SerializeField] private float waitBeforeFade = 0f;

    [System.Serializable]
    public class FadeSettings
    {
        public string propertyName = "_Fade";
        public float start = 0f;
        public float end = 1f;
        public float duration = 2f;
        public float delay = 0f;
    }

    [Header("=== フェード設定 ===")]
    [SerializeField] private FadeSettings[] fadeSettings;
    [SerializeField] private Material[] fadeMaterials;
    private int fadeProgressIndex = 0;

    [Header("=== リバースフェード設定 ===")]
    [SerializeField]
    private FadeSettings[] reverseFadeSettings = new FadeSettings[]
    {
        new FadeSettings { propertyName = "_Feather", start = 0.8f, end = 0.1f, duration = 2f },
        new FadeSettings { propertyName = "_Fade", start = 1f, end = 0f, duration = 2f }
    };



    // 外部から与えられる確率配列（優先）
    // インデックスは SlotSymbol の enum 値に対応（例: probabilityArray[(int)SlotSymbol.Bicycle]）
    public float[] probabilityArray;

    // ================== REEL INSTANCE ==================
    private Reel[] reels;

    // 外部から登録できるコールバック（任意）
    public Action OnReaperStopCallback;

    // 統一した OnReaperStop 実装（重複を解消）

    [SerializeField] private AudioManager audioManager;
    [SerializeField] private AudioClip sinner_015;
    public void OnReaperStop()
    {
        ReaperCount++;
        Debug.Log($"[SlotMachine] Reaper Hit! Total = {ReaperCount}");
        OnReaperStopCallback?.Invoke();
    }

    // ================== CENTRAL SYMBOL MANAGEMENT ==================
    public void AddCenterObject(Transform symbol)
    {
        if (symbol == null) return;
        centerObjects.Add(symbol);
        Debug.Log($"[SlotMachine] CenterObject added: {symbol.name}");
    }

    // ---------------- External API ----------------
    public void SetSymbolTable(SymbolData[] table)
    {
        if (table == null || table.Length == 0) return;
        symbolTable = table;

        if (reels != null)
        {
            foreach (var r in reels)
            {
                r.UpdateSymbolTable(symbolTable);
            }
        }
    }

    public void SetTargetSymbol(SlotSymbol target)
    {
        this.target = target;
    }

    public void SetProbabilityArray(float[] arr)
    {
        probabilityArray = arr;
    }

    public void SpinWithCustomSettings(SymbolData[] table, SlotSymbol targetSymbol, float[] externalProbabilityArray = null)
    {
        if (externalProbabilityArray != null)
            SetProbabilityArray(externalProbabilityArray);

        SetSymbolTable(table);
        SetTargetSymbol(targetSymbol);
        //StartCoroutine(AnimationSequence());

        //if (reels != null)
        //    foreach (var r in reels)
        //        StartCoroutine(ReelUpdateLoop(r));

        //if (fadeMaterials != null)
        //{
        //    for (int i = 0; i < fadeMaterials.Length; i++)
        //    {
        //        if (fadeMaterials[i] == null) continue;
        //        if (fadeMaterials[i].HasProperty("_Fade")) fadeMaterials[i].SetFloat("_Fade", 0);
        //        if (fadeMaterials[i].HasProperty("_Feather")) fadeMaterials[i].SetFloat("_Feather", 0.5f);
        //    }
        //}
    }

    public void SetMachineActive(bool active)
    {
        gameObject.SetActive(active);

        if (active)
        {
            if (reels != null)
            {
                foreach (var r in reels)
                    r.Initialize();
            }
        }
    }

    public void ReelFade()
    {
        if (fadeSettings != null && fadeProgressIndex < fadeSettings.Length && fadeMaterials != null && fadeMaterials.Length > 0)
        {
            int materialIndex = Mathf.Clamp(fadeProgressIndex, 0, fadeMaterials.Length - 1);
            StartCoroutine(AnimateProperty(fadeSettings[fadeProgressIndex], fadeMaterials[materialIndex], true));
            fadeProgressIndex++;
        }
    }

    public void OnReelStopped()
    {
        stoppedReelCount++;
        if (stoppedReelCount >= 3)
        {
            if (target == SlotSymbol.Random)
            {
                StartCoroutine(FadeOutAfterDelay());
            }
            else if (target == SlotSymbol.Reaper)
            {
                StartCoroutine(ScaleAllCenterObjects());
            }
            else
            {
                if (generalPurposeParticle != null) generalPurposeParticle.SetActive(true);
                StartCoroutine(ScaleAllCenterObjects());
            }
        }
    }

    // ================== UNITY CALLBACKS ==================
    private void Start()
    {
        if (parents == null || parents.Length != 3)
        {
            Debug.LogError("Parentsは3つ指定してください");
            return;
        }

        reels = new Reel[3];
        for (int i = 0; i < 3; i++)
            reels[i] = new Reel(parents[i], symbolPrefab, symbolTable, symbolCount, symbolSpacing, this);

        foreach (var sr in renderers) SetAlpha(sr, startAlpha);
        if (fadeMaterials != null)
        {
            for (int i = 0; i < fadeMaterials.Length; i++)
            {
                if (fadeMaterials[i] == null) continue;
                if (fadeMaterials[i].HasProperty("_Fade")) fadeMaterials[i].SetFloat("_Fade", 0);
                if (fadeMaterials[i].HasProperty("_Feather")) fadeMaterials[i].SetFloat("_Feather", 0.5f);
            }
        }
    }

    private void OnEnable()
    {
        audioManager = GameObject.Find("Audio").GetComponent<AudioManager>();
        audioManager.PlaySE(sinner_015);

        fadeProgressIndex = 0;
        ReaperCount = 0;
        foreach (var sr in renderers) SetAlpha(sr, startAlpha);

        if (reels != null)
            foreach (var r in reels) r.Initialize();

        StopAllCoroutines();
        StartCoroutine(AnimationSequence());

        if (reels != null)
            foreach (var r in reels)
                StartCoroutine(ReelUpdateLoop(r));

        if (fadeMaterials != null)
        {
            for (int i = 0; i < fadeMaterials.Length; i++)
            {
                if (fadeMaterials[i] == null) continue;
                if (fadeMaterials[i].HasProperty("_Fade")) fadeMaterials[i].SetFloat("_Fade", 0);
                if (fadeMaterials[i].HasProperty("_Feather")) fadeMaterials[i].SetFloat("_Feather", 0.5f);
            }
        }
    }

    private void FixedUpdate()
    {
        if (reels == null) return;
        foreach (var reel in reels)
            reel?.UpdateLogic();
    }

    private IEnumerator ReelUpdateLoop(Reel reel)
    {
        while (true)
        {
            reel.UpdateLogic();
            yield return null;
        }
    }

    // ================== SPIN CONTROL ==================
    public void SpinAll()
    {
        stoppedReelCount = 0;
        centerObjects.Clear();

        if (laneDelays == null || laneDelays.Length < 3)
            laneDelays = new float[3] { 0f, 0.3f, 0.6f };

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

    // ================== HELPER FUNCTIONS ==================
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

    // ---------------- Fade & Animation Coroutines ----------------
    private IEnumerator AnimateProperty(FadeSettings settings, Material targetMaterial, bool forward)
    {
        if (settings.delay > 0f) yield return new WaitForSeconds(settings.delay);
        if (targetMaterial == null || !targetMaterial.HasProperty(settings.propertyName)) yield break;

        float from = forward ? settings.start : settings.end;
        float to = forward ? settings.end : settings.start;
        float time = 0f;

        while (time < settings.duration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / settings.duration);
            float value = Mathf.Lerp(from, to, t);
            targetMaterial.SetFloat(settings.propertyName, value);
            yield return null;
        }

        targetMaterial.SetFloat(settings.propertyName, to);
    }

    private IEnumerator AnimatePropertyFromCurrent(FadeSettings settings, Material targetMaterial)
    {
        if (settings.delay > 0f) yield return new WaitForSeconds(settings.delay);
        if (targetMaterial == null || !targetMaterial.HasProperty(settings.propertyName)) yield break;

        float from = targetMaterial.GetFloat(settings.propertyName);
        float to = settings.end;
        float time = 0f;

        while (time < settings.duration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / settings.duration);
            float value = Mathf.Lerp(from, to, t);
            targetMaterial.SetFloat(settings.propertyName, value);
            yield return null;
        }

        targetMaterial.SetFloat(settings.propertyName, to);
    }

    private IEnumerator FadeOutAfterDelay()
    {
        yield return new WaitForSeconds(waitBeforeFade);

        if (reverseFadeSettings != null && fadeMaterials != null)
        {
            foreach (var material in fadeMaterials)
            {
                if (material == null) continue;
                foreach (var settings in reverseFadeSettings)
                {
                    if (material.HasProperty(settings.propertyName))
                        StartCoroutine(AnimatePropertyFromCurrent(settings, material));
                }
            }
        }

        if (targetReels != null)
        {
            foreach (var reel in targetReels)
            {
                if (reel != null)
                    StartCoroutine(FadeReelAlphaOut(reel));
            }
        }

        yield return StartCoroutine(FadeOutCoroutine());
         gameObject.SetActive(false);
    }

    private void ResetSymbolTransform(Transform t)
    {
        if (t == null) return;
        t.localScale = Vector3.one * 0.15f;
        t.localRotation = Quaternion.Euler(Vector3.zero);
    }

    private IEnumerator ScaleAllCenterObjects()
    {
        if (centerObjects.Count == 0) yield break;

        foreach (var obj in centerObjects)
            if (obj != null) obj.localScale = startScale;

        float time = 0f;
        while (time < scaleDuration)
        {
            time += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, time / scaleDuration);
            foreach (var obj in centerObjects)
                if (obj != null) obj.localScale = Vector3.Lerp(startScale, endScale, t);
            yield return null;
        }

        foreach (var obj in centerObjects)
            if (obj != null) obj.localScale = endScale;

        StartCoroutine(RotateAllCenterObjects());
    }

    private IEnumerator RotateAllCenterObjects()
    {
        if (centerObjects.Count == 0) yield break;

        float time = 0f;
        while (time < rotationDuration)
        {
            time += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, time / rotationDuration);
            foreach (var obj in centerObjects)
                if (obj != null) obj.localRotation = Quaternion.Euler(Vector3.Lerp(Vector3.zero, rotationEnd, t));
            yield return null;
        }

        for (int loop = 1; loop < rotationLoopCount; loop++)
        {
            yield return RotateBetween(rotationEnd, rotationStart);
            yield return RotateBetween(rotationStart, rotationEnd);

            if (loop == rotationLoopCount - 1 && target == SlotSymbol.Reaper)
                if (reaperParticle != null) reaperParticle.SetActive(true);
        }

        if (centerObjects.Count > 0)
            yield return RotateBetween(centerObjects[0].localRotation.eulerAngles, Vector3.zero);

        yield return ScaleBackAllCenterObjects();

        if (generalPurposeParticle != null) generalPurposeParticle.SetActive(false);
        StartCoroutine(FadeOutAfterDelay());
    }

    private IEnumerator RotateBetween(Vector3 from, Vector3 to)
    {
        float time = 0f;
        while (time < rotationDuration)
        {
            time += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, time / rotationDuration);
            foreach (var obj in centerObjects)
                if (obj != null) obj.localRotation = Quaternion.Euler(Vector3.Lerp(from, to, t));
            yield return null;
        }
    }

    private IEnumerator ScaleBackAllCenterObjects()
    {
        if (centerObjects.Count == 0) yield break;

        float time = 0f;
        while (time < scaleDuration)
        {
            time += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, time / scaleDuration);
            foreach (var obj in centerObjects)
                if (obj != null) obj.localScale = Vector3.Lerp(endScale, startScale, t);
            yield return null;
        }

        foreach (var obj in centerObjects)
            if (obj != null) obj.localScale = startScale;

        foreach (var obj in centerObjects)
            obj.GetComponent<SpriteRenderer>().sortingOrder = 4;
    }

    private IEnumerator FadeCoroutine()
    {
        if (reaperParticle != null) reaperParticle.SetActive(false);
        if (generalPurposeParticle != null) generalPurposeParticle.SetActive(false);

        float time = 0f;
        foreach (var sr in renderers)
            ResetSymbolTransform(sr.transform);

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            foreach (var sr in renderers)
                if (sr != null)
                {
                    var c = sr.color;
                    c.a = Mathf.Lerp(startAlpha, endAlpha, t);
                    sr.color = c;
                }
            yield return null;
        }

        foreach (var sr in renderers)
            if (sr != null)
            {
                var c = sr.color;
                c.a = endAlpha;
                sr.color = c;
            }
    }

    private IEnumerator FadeOutCoroutine()
    {
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            foreach (var sr in renderers)
                if (sr != null)
                {
                    var c = sr.color;
                    c.a = Mathf.Lerp(endAlpha, startAlpha, t);
                    sr.color = c;
                }
            yield return null;
        }

        foreach (var sr in renderers)
        if (sr != null)
        {
                var c = sr.color;
                c.a = startAlpha;
                sr.color = c;
        }
       
    }

    private IEnumerator AnimationSequence()
    {
        // --- フェード1：リール全体 ---
        if (targetReels != null)
        {
            foreach (var reel in targetReels)
            {
                if (reel != null)
                    StartCoroutine(FadeReelAlpha(reel, startAlpha, endAlpha, reelDuration));
            }
        }

        // --- フェード2：個別レンダラー ---
        yield return StartCoroutine(FadeCoroutine());

        // --- フェード完了後に回転 ---
        float delay = overrideSpinDelay ? manualSpinDelay : spinDelay;
        yield return new WaitForSeconds(delay);

        SpinAll();
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
                if (sr != null)
                {
                    var c = sr.color;
                    c.a = alpha;
                    sr.color = c;
                }
            yield return null;
        }

        foreach (var sr in children)
            if (sr != null)
            {
                var c = sr.color;
                c.a = toAlpha;
                sr.color = c;
            }
    }

    public IEnumerator FadeReelAlphaOut(Transform reel, float fromAlpha = 0f, float toAlpha = 1f, float duration = 1f)
    {
        SpriteRenderer[] children = reel.GetComponentsInChildren<SpriteRenderer>();
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            float alpha = Mathf.Lerp(toAlpha, fromAlpha, t);
            foreach (var sr in children)
                if (sr != null)
                {
                    var c = sr.color;
                    c.a = alpha;
                    sr.color = c;
                }
            yield return null;
        }

        foreach (var sr in children)
            if (sr != null)
            {
                var c = sr.color;
                c.a = fromAlpha;
                sr.color = c;
            }
    }

    // ================== REEL CLASS ==================
    private class Reel
    {
        private List<Transform> symbols = new();
        private Transform parent;
        private GameObject prefab;
        private SymbolData[] table;
        private int count;
        private float spacing;
        private Sinner_015 owner;

        private float scrollSpeed;
        private SlotSymbol targetSymbol;
        private float totalHeight;
        private bool isStopping = false;
        private bool changeSprite = false;
        public int reaperCount = 0;

        public Reel(Transform parent, GameObject prefab, SymbolData[] table, int count, float spacing, Sinner_015 owner)
        {
            this.parent = parent;
            this.prefab = prefab;
            this.table = table;
            this.count = count;
            this.spacing = spacing;
            this.owner = owner;

            for (int i = 0; i < count; i++)
            {
                var obj = UnityEngine.Object.Instantiate(prefab, parent);
                obj.transform.localPosition = new Vector3(0, -i * spacing + 4, 0);
                var sr = obj.GetComponent<SpriteRenderer>();
                sr.sprite = GetSpriteFromEnum(RandomSymbol());
                owner.SetAlpha(sr, owner.startAlpha);
                owner.AddRenderer(sr);
                symbols.Add(obj.transform);
            }

            totalHeight = spacing * count;
        }

        public void UpdateSymbolTable(SymbolData[] newTable)
        {
            if (newTable == null || newTable.Length == 0) return;
            table = newTable;
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
                sr.sortingOrder = 4;
            }
        }

        public void StartSpin(float speed, float delay, SlotSymbol target)
        {
            scrollSpeed = speed;
            targetSymbol = target;
            isStopping = false;
            changeSprite = false;
            owner.StartCoroutine(StopAfterRoutine(delay));
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

        public void UpdateLogic()
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
                    owner.StartCoroutine(SnapToTarget());
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

            Transform closest = GetCenterSymbol();
            var sr = closest.GetComponent<SpriteRenderer>();
            sr.sprite = GetSpriteFromEnum(targetSymbol);

            float centerOffset = 0f;
            closest.localPosition = new Vector3(0, centerOffset, 0);

            for (int i = 0; i < symbols.Count; i++)
            {
                Transform s = symbols[i];
                if (s == closest) continue;

                int indexOffset = i - symbols.IndexOf(closest);
                float y = centerOffset - indexOffset * spacing;

                if (y < -totalHeight / 2f) y += totalHeight;
                if (y > totalHeight / 2f) y -= totalHeight;

                s.localPosition = new Vector3(0, y, 0);
            }

            sr.sortingOrder = 8;
            owner.ReelFade();

            float overshoot = -1f;
            float duration = 0.2f;

            closest.localPosition += new Vector3(0, overshoot, 0);

            float t = 0f;
            while (t < duration)
            {
                t += Time.deltaTime;
                float n = Mathf.SmoothStep(0, 1, t / duration);
                float y = Mathf.Lerp(centerOffset + overshoot, centerOffset, n);
                closest.localPosition = new Vector3(0, y, 0);
                yield return null;
            }

            closest.localPosition = new Vector3(0, centerOffset, 0);

            changeSprite = false;

            if (sr.sprite != null && sr.sprite.name == "grim reaper_0")
            {
                reaperCount++;
                owner.OnReaperStop();
            }

            owner.AddCenterObject(closest);
            owner.OnReelStopped();
        }

        // --- Probabilities: owner.probabilityArray (優先) -> SymbolData.probabilityArray[0] (次) -> default
        private float GetProbabilityFor(SymbolData d)
        {
            if (d == null) return 0f;

            // 1) 外部配列が有効なら enum index で取得（範囲チェック）
            if (owner.probabilityArray != null && owner.probabilityArray.Length > (int)d.symbol)
            {
                float v = owner.probabilityArray[(int)d.symbol];
                if (v > 0f) return v;
            }

            // 2) SymbolData.probabilityArray が設定されていれば最初の要素を使う
            if (d.probabilityArray != null && d.probabilityArray.Length > 0)
                return d.probabilityArray[0];

            // 3) フォールバック値（等確率の一部として扱う）
            return 0f;
        }

        public SlotSymbol RandomSymbol()
        {
            float total = 0;
            foreach (var d in table)
            {
                // 配列がある場合は配列の最初の値を使う、なければ単体 probability
                float p = (d.probabilityArray != null && d.probabilityArray.Length > 0) ? d.probabilityArray[0] : d.probability;
                if (d.symbol != SlotSymbol.Random) total += p;
            }

            float r = UnityEngine.Random.Range(0f, total);
            float sum = 0;
            foreach (var d in table)
            {
                if (d.symbol == SlotSymbol.Random) continue;
                float p = (d.probabilityArray != null && d.probabilityArray.Length > 0) ? d.probabilityArray[0] : d.probability;
                sum += p;
                if (r <= sum) return d.symbol;
            }

            return table[0].symbol;
        }


        private Sprite GetSpriteFromEnum(SlotSymbol symbol)
        {
            if (table == null) return null;
            foreach (var data in table)
                if (data != null && data.symbol == symbol) return data.sprite;
            Debug.LogError($"Sprite not found for symbol: {symbol}");
            return null;
        }
    }
}
