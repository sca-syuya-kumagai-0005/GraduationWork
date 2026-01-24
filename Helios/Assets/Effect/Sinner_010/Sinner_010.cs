using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Sinner_010 : MonoBehaviour
{
    [Header("対象マテリアル配列")]
    [SerializeField] private Material[] _materials;

    [Header("DotColor プロパティ名")]
    [SerializeField] private string _colorPropertyName = "_DotColor";

    [Header("アルファ設定")]
    [SerializeField, Range(0f, 1f)] private float _fromAlpha = 0f;
    [SerializeField, Range(0f, 1f)] private float _toAlpha = 1f;
    [SerializeField] private float _duration = 1f;
    [SerializeField] private float _stagger = 0.1f;

    [Header("2番目のマテリアルだけ半分にする")]
    [SerializeField] private bool _secondIsHalfAlpha = true;

    [Header("Ease設定")]
    [SerializeField] private bool _useEaseInOut = true;

    private readonly List<Coroutine> _runningCoroutines = new();
    private bool _started;

    [SerializeField] private AudioManager audioManager;
    [SerializeField] private AudioClip sinner_010;
    [SerializeField] private AudioClip sinner_010SE;

    private void OnEnable()
    {
        // 再表示された時に再スタートできるようにする
        _started = false;
        ResetAlpha();
        TryStart();
        audioManager = GameObject.Find("Audio").GetComponent<AudioManager>();
        audioManager.PlaySE(sinner_010);
        audioManager.PlaySE(sinner_010SE);
    }

    private void OnBecameVisible()
    {
        TryStart();
    }

    private void TryStart()
    {
        if (_started || !gameObject.activeInHierarchy) return;
        _started = true;
        StartAnimation();
    }

    private void StartAnimation()
    {
        if (_materials == null || _materials.Length == 0) return;

        ResetAlpha();

        for (int i = 0; i < _materials.Length; i++)
        {
            var mat = _materials[i];
            if (mat == null) continue;

            var coroutine = StartCoroutine(AnimateMaterialAlpha(mat, i * _stagger, i));
            _runningCoroutines.Add(coroutine);
        }
    }

    private void ResetAlpha()
    {
        foreach (var mat in _materials)
        {
            if (mat == null || !mat.HasProperty(_colorPropertyName)) continue;
            var c = mat.GetColor(_colorPropertyName);
            c.a = 0f;
            mat.SetColor(_colorPropertyName, c);
        }
    }

    private IEnumerator AnimateMaterialAlpha(Material mat, float startDelay, int index)
    {
        if (startDelay > 0f)
            yield return new WaitForSeconds(startDelay);

        var baseColor = mat.HasProperty(_colorPropertyName)
            ? mat.GetColor(_colorPropertyName)
            : Color.white;

        float elapsed = 0f;

        while (elapsed < _duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / Mathf.Max(0.0001f, _duration));

            if (_useEaseInOut)
                t = Mathf.SmoothStep(0f, 1f, t);

            float alpha = Mathf.Lerp(_fromAlpha, _toAlpha, t);

            if (_secondIsHalfAlpha && index == 1)
                alpha *= 0.5f;

            var c = baseColor;
            c.a = alpha;
            mat.SetColor(_colorPropertyName, c);

            yield return null;
        }

        float finalAlpha = _toAlpha;
        if (_secondIsHalfAlpha && index == 1)
            finalAlpha *= 0.5f;

        var final = baseColor;
        final.a = finalAlpha;
        mat.SetColor(_colorPropertyName, final);
    }

    private void OnDisable()
    {
        // 停止＆初期化
        foreach (var c in _runningCoroutines)
        {
            if (c != null) StopCoroutine(c);
        }
        _runningCoroutines.Clear();

        ResetAlpha();
        _started = false;
    }
}
