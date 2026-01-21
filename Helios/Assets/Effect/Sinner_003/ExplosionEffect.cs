using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ExplosionEffect : MonoBehaviour
{
    [Header("爆散の強さ設定 (バッチ数 = 配列長)")]
    public float[] explosionForces;

    [Header("回転の強さ設定 (バッチごと)")]
    public float[] rotationForces;

    [Header("物理挙動設定")]
    public float drag = 2f;
    public float angularDrag = 2f;

    [Header("方向ランダム設定")]
    public float directionRandomness = 0.3f;
    public float upwardBias = 0.2f;
    public float cameraBias = 0.3f;

    [Header("カメラ参照")]
    public Camera targetCamera;

    [Header("爆発後の公転設定")]
    public float orbitSpeed = 50f;
    [Tooltip("true=左回転 / false=右回転")]
    public bool orbitLeft = true;

    [Header("吸収設定")]
    public float absorbSpeed = 5f;
    public float shrinkSpeed = 2f;

    [Header("タイミング設定")]
    public float revealInterval = 1.0f;
    public float explodeDelay = 1.5f;
    public float absorbDelay = 2.0f;

    private List<Transform> children;
    private List<List<Transform>> batches;
    private bool hasExploded = false;

    [Header("ステージ非表示,サブカメラの停止")]
    //public GameObject gameScene;
    public Camera subCamera;

    // --- 管理用 ---
    private int activeOrbits = 0;   // 現在吸収中のオブジェクト数
    private bool absorbFinished = false; // 終了処理が一度だけになるよう制御

    void Start()
    {
        if (targetCamera == null) targetCamera = Camera.main;

        children = new List<Transform>();
        foreach (Transform child in transform)
        {
            children.Add(child);

            Rigidbody rb = child.GetComponent<Rigidbody>();
            if (rb == null) rb = child.gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.useGravity = false;
            rb.linearDamping = drag;
            rb.angularDamping = angularDrag;

            var rends = child.GetComponentsInChildren<Renderer>(true);
            foreach (var r in rends) r.enabled = false;
        }

        children = children.OrderByDescending(c => (c.position - transform.position).sqrMagnitude).ToList();

        int totalBatches = Mathf.Max(1, explosionForces.Length);
        batches = new List<List<Transform>>();
        int countPerBatch = Mathf.CeilToInt((float)children.Count / totalBatches);

        for (int b = 0; b < totalBatches; b++)
        {
            int start = b * countPerBatch;
            int end = Mathf.Min(start + countPerBatch, children.Count);
            if (start < end)
                batches.Add(children.GetRange(start, end - start));
            else
                batches.Add(new List<Transform>());
        }

        // 自動進行開始
        StartCoroutine(AutoExplosionSequence());
    }

    IEnumerator AutoExplosionSequence()
    {
        for (int i = 0; i < batches.Count; i++)
        {
            RevealBatch(i);
            yield return new WaitForSeconds(revealInterval);
        }

        yield return new WaitForSeconds(explodeDelay);
        ExplodeAllBatches();
        hasExploded = true;

        yield return new WaitForSeconds(absorbDelay);
        StartAbsorbAll();
    }

    private void RevealBatch(int batchIndex)
    {
        foreach (var t in batches[batchIndex])
        {
            var rends = t.GetComponentsInChildren<Renderer>(true);
            foreach (var r in rends) r.enabled = true;
        }
    }

    private void ExplodeAllBatches()
    {
        subCamera.enabled = false;
        //gameScene.SetActive(false);

        int totalBatches = batches.Count;
        float aspect = 16f / 9f;

        for (int b = 0; b < totalBatches; b++)
        {
            float baseForce = (explosionForces.Length > b) ? explosionForces[b] : explosionForces[explosionForces.Length - 1];
            float rotForceBase = (rotationForces.Length > b) ? rotationForces[b] : rotationForces[rotationForces.Length - 1];

            foreach (var t in batches[b])
            {
                if (t == null) continue;

                Rigidbody rb = t.GetComponent<Rigidbody>();
                rb.isKinematic = false;
                rb.useGravity = false;
                rb.linearDamping = drag;
                rb.angularDamping = angularDrag;

                Vector3 localDir = t.position - transform.position;
                localDir.x /= aspect;
                Vector3 outwardDir = localDir.normalized;

                Vector3 randomDir = Random.onUnitSphere * directionRandomness;
                Vector3 biasedDir = outwardDir * (1f - directionRandomness) + randomDir + Vector3.up * upwardBias;

                Vector3 cameraDir = (t.position - targetCamera.transform.position).normalized;
                biasedDir.z = Random.Range(-1f, 1f);

                Vector3 finalDir = Vector3.Lerp(biasedDir.normalized, cameraDir, cameraBias).normalized;

                float forceWithRandom = baseForce * Random.Range(0.9f, 1.1f);
                rb.AddForce(finalDir * forceWithRandom, ForceMode.Impulse);

                Vector3 randomTorque = Random.insideUnitSphere * forceWithRandom * rotForceBase;
                rb.AddTorque(randomTorque, ForceMode.Impulse);

                var orbit = t.gameObject.AddComponent<OrbitAndAbsorb>();
                orbit.Initialize(this, transform.position, orbitSpeed, orbitLeft ? 1 : -1);
            }
        }
    }

    private void StartAbsorbAll()
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

    // --- 吸収完了通知（子から呼ばれる） ---
    public void NotifyAbsorbFinished()
    {
        if (absorbFinished) return; // すでに処理済みならスキップ

        activeOrbits--;
        if (activeOrbits <= 0)
        {
            absorbFinished = true; // 一度だけにする
            subCamera.enabled = false;
            //gameScene.SetActive(true);
        }
    }

    // ---------------- 子オブジェクトのクラス ----------------
    public class OrbitAndAbsorb : MonoBehaviour
    {
        private ExplosionEffect owner;
        private Vector3 center;
        private float speed;
        private int direction;

        private bool absorbing = false;
        private float absorbSpeed;
        private float shrinkSpeed;
        private Vector3 initialScale;

        public void Initialize(ExplosionEffect effect, Vector3 c, float s, int d)
        {
            owner = effect;
            center = c;
            speed = s;
            direction = d;
            initialScale = transform.localScale;
        }

        public void StartAbsorb(Vector3 target, float speed, float shrink)
        {
            center = target;
            absorbing = true;
            absorbSpeed = speed;
            shrinkSpeed = shrink;
            initialScale = transform.localScale;
        }

        void Update()
        {
            transform.RotateAround(center, Vector3.up, speed * direction * Time.deltaTime);

            if (absorbing)
            {
                transform.position = Vector3.MoveTowards(transform.position, center, absorbSpeed * Time.deltaTime);
                transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, Time.deltaTime * shrinkSpeed);

                if (transform.localScale.magnitude < 0.01f)
                {
                    gameObject.SetActive(false);
                    if (owner != null)
                    {
                        owner.NotifyAbsorbFinished();
                    }
                }
            }
        }
    }
}
