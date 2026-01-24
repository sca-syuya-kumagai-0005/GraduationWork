using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ExplosionEffect : MonoBehaviour
{
    [Header("爆散の強さ")]
    public float[] explosionForces;
    public float[] rotationForces;

    [Header("物理")]
    public float drag = 2f;
    public float angularDrag = 2f;

    [Header("カメラ")]
    public Camera targetCamera;
    public Camera subCamera;

    [Header("公転・吸収")]
    public float orbitSpeed = 50f;
    public bool orbitLeft = true;
    public float absorbSpeed = 5f;
    public float shrinkSpeed = 2f;

    [Header("タイミング")]
    public float revealInterval = 1f;
    public float explodeDelay = 1.5f;
    public float absorbDelay = 2f;

    List<Transform> children = new();
    List<List<Transform>> batches = new();

    int activeOrbits;
    bool absorbFinished;
    Coroutine sequence;

    [SerializeField] private AudioManager audioManager;
    [SerializeField] private AudioClip sinner_003;

    // =======================
    // 表示されたら毎回リセット＆再生
    // =======================
    void OnEnable()
    {
        audioManager = GameObject.Find("Audio").GetComponent<AudioManager>();
        audioManager.PlaySE(sinner_003);
        Initialize();
        sequence = StartCoroutine(AutoExplosionSequence());

    }

    void OnDisable()
    {
        if (sequence != null)
            StopCoroutine(sequence);

        Cleanup();
    }

    // =======================
    // 初期化（完全リセット）
    // =======================
    void Initialize()
    {
        absorbFinished = false;
        activeOrbits = 0;

        if (targetCamera == null)
            targetCamera = Camera.main;

        children.Clear();

        foreach (Transform child in transform)
        {
            children.Add(child);

            // 初期Transform保存
            var init = child.GetComponent<PieceInitializer>();
            if (init == null)
                init = child.gameObject.AddComponent<PieceInitializer>();

            init.ResetToInitial(); // 位置0にしない（元に戻す）

            // Rigidbody 初期化
            var rb = child.GetComponent<Rigidbody>();
            if (rb == null) rb = child.gameObject.AddComponent<Rigidbody>();

            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
            rb.useGravity = false;
            rb.linearDamping = drag;
            rb.angularDamping = angularDrag;

            // Orbit 削除
            var orbit = child.GetComponent<OrbitAndAbsorb>();
            if (orbit != null) Destroy(orbit);

            // 最初は必ず非表示
            child.gameObject.SetActive(false);
        }

        // 距離順でバッチ分け
        children = children
            .OrderByDescending(c => (c.position - transform.position).sqrMagnitude)
            .ToList();

        batches.Clear();
        int batchCount = Mathf.Max(1, explosionForces.Length);
        int perBatch = Mathf.CeilToInt((float)children.Count / batchCount);

        for (int i = 0; i < batchCount; i++)
        {
            int start = i * perBatch;
            int end = Mathf.Min(start + perBatch, children.Count);
            batches.Add(start < end ? children.GetRange(start, end - start) : new());
        }
    }

    void Cleanup()
    {
        foreach (Transform c in transform)
        {
            var orbit = c.GetComponent<OrbitAndAbsorb>();
            if (orbit != null) Destroy(orbit);
        }
    }

    // =======================
    // 演出シーケンス
    // =======================
    IEnumerator AutoExplosionSequence()
    {
        for (int i = 0; i < batches.Count; i++)
        {
            foreach (var t in batches[i])
                t.gameObject.SetActive(true);

            yield return new WaitForSeconds(revealInterval);
        }

        yield return new WaitForSeconds(explodeDelay);
        Explode();

        yield return new WaitForSeconds(absorbDelay);
        StartAbsorb();
    }

    void Explode()
    {
        if (subCamera != null)
            subCamera.enabled = false;

        for (int b = 0; b < batches.Count; b++)
        {
            float force = explosionForces[Mathf.Min(b, explosionForces.Length - 1)];
            float rot = rotationForces[Mathf.Min(b, rotationForces.Length - 1)];

            foreach (var t in batches[b])
            {
                var rb = t.GetComponent<Rigidbody>();
                rb.isKinematic = false;

                Vector3 dir = (t.position - transform.position).normalized;
                rb.AddForce(dir * force, ForceMode.Impulse);
                rb.AddTorque(Random.insideUnitSphere * rot, ForceMode.Impulse);

                var orbit = t.gameObject.AddComponent<OrbitAndAbsorb>();
                orbit.Initialize(this, transform.position, orbitSpeed, orbitLeft ? 1 : -1);
            }
        }
    }

    void StartAbsorb()
    {
        activeOrbits = 0;

        foreach (var c in children)
        {
            var orbit = c.GetComponent<OrbitAndAbsorb>();
            if (orbit != null)
            {
                activeOrbits++;
                orbit.StartAbsorb(transform.position, absorbSpeed, shrinkSpeed);
            }
        }
    }

    // 全ピース吸収完了時
    public void NotifyAbsorbFinished()
    {
        if (absorbFinished) return;

        activeOrbits--;

        if (activeOrbits <= 0)
        {
            absorbFinished = true;

            // 演出完了 → 自身を非表示
            gameObject.SetActive(false);
        }
    }

    // =======================
    // 初期Transform保存
    // =======================
    class PieceInitializer : MonoBehaviour
    {
        Vector3 pos;
        Quaternion rot;
        Vector3 scale;
        bool cached;

        void Awake()
        {
            Cache();
        }

        void Cache()
        {
            if (cached) return;
            pos = transform.localPosition;
            rot = transform.localRotation;
            scale = transform.localScale;
            cached = true;
        }

        public void ResetToInitial()
        {
            Cache();
            transform.localPosition = pos;
            transform.localRotation = rot;
            transform.localScale = scale;
        }
    }

    // =======================
    // Orbit & Absorb
    // =======================
    public class OrbitAndAbsorb : MonoBehaviour
    {
        ExplosionEffect owner;
        Vector3 center;
        float speed;
        int dir;

        bool absorbing;
        float absorbSpeed;
        float shrinkSpeed;

        public void Initialize(ExplosionEffect o, Vector3 c, float s, int d)
        {
            owner = o;
            center = c;
            speed = s;
            dir = d;
            absorbing = false;
        }

        public void StartAbsorb(Vector3 c, float speed, float shrink)
        {
            center = c;
            absorbing = true;
            absorbSpeed = speed;
            shrinkSpeed = shrink;
        }

        void Update()
        {
            transform.RotateAround(center, Vector3.up, speed * dir * Time.deltaTime);

            if (!absorbing) return;

            transform.position = Vector3.MoveTowards(transform.position, center, absorbSpeed * Time.deltaTime);
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, Time.deltaTime * shrinkSpeed);

            if (transform.localScale.magnitude < 0.01f)
            {
                owner.NotifyAbsorbFinished();
                Destroy(this);
                gameObject.SetActive(false);
            }
        }
    }
}
