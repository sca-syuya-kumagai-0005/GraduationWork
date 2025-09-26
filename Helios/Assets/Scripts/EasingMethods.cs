using Unity.VisualScripting;
using UnityEngine;
using System;

public class EasingMethods : MonoBehaviour
{
    protected enum EasingsPattern
    {
        None = 0,
        EaseInSine,
        EaseOutSine,
        EaseInOutSine,
        EaseInQuad,
        EaseOutQuad,
        EaseInOutQuad,
        EaseInCubic,
        EaseOutCubic,
        EaseInOutCubic,
        EaseInQuart,
        EaseOutQuart,
        EaseInOutQuart,
        EaseInQuint,
        EaseOutQuint,
        EaseInOutQuint,
        EaseInExpo,
        EaseOutExpo,
        EaseInOutExpo,
        EaseInCirc,
        EaseOutCirc,
        EaseInOutCirc,
        EaseInBack,
        EaseOutBack,
        EaseInOutBack,
        EaseInElastic,
        EaseOutElastic,
        EaseInOutElastic,
        EaseInBounce,
        EaseOutBounce,
        EaseInOutBounce,
        SIZE
    }

    protected Func<float, float>[] easingFunc = new Func<float, float>[(int)EasingsPattern.SIZE]
    {
        None,
        EaseInSine,
        EaseOutSine,
        EaseInOutSine,
        EaseInQuad,
        EaseOutQuad,
        EaseInOutQuad,
        EaseInCubic,
        EaseOutCubic,
        EaseInOutCubic,
        EaseInQuart,
        EaseOutQuart,
        EaseInOutQuart,
        EaseInQuint,
        EaseOutQuint,
        EaseInOutQuint,
        EaseInExpo,
        EaseOutExpo,
        EaseInOutExpo,
        EaseInCirc,
        EaseOutCirc,
        EaseInOutCirc,
        EaseInBack,
        EaseOutBack,
        EaseInOutBack,
        EaseInElastic,
        EaseOutElastic,
        EaseInOutElastic,
        EaseInBounce,
        EaseOutBounce,
        EaseInOutBounce,
    };  

    private const float c1 = 1.70158f;
    private const float c2 = c1 * 1.525f;
    private const float c3 = c1 + 1;
    private const float c4 = (2 * Mathf.PI) / 3;
    private const float c5 = (2 * Mathf.PI) / 4.5f;

    protected static float None(float t)
    {
        return t;
    }
    protected static float EaseInSine(float t)
    {
        return t <= 0 ? 0
            : t < 1.0 ? 1 - Mathf.Cos((t * Mathf.PI) / 2)
            : 1.0f;
    }

    protected static float EaseOutSine(float t)
    {
        return t <= 0 ? 0
            : t < 1.0 ? Mathf.Sin((t * Mathf.PI) / 2)
            : 1.0f;
    }

    protected static float EaseInOutSine(float t)
    {
        return t <= 0 ? 0
            : t < 1.0 ? -(Mathf.Cos(Mathf.PI * t) - 1) / 2
            : 1.0f;
    }
    protected static float EaseInQuad(float t)
    {
        return t <= 0 ? 0
           : t < 1.0 ? t * t
           : 1.0f;
    }

    protected static float EaseOutQuad(float t)
    {
        return t <= 0 ? 0
            : t < 1.0 ? 1 - (1 - t) * (1 - t)
            : 1.0f;
    }

    protected static float EaseInOutQuad(float t)
    {
        return t < 0 ? 0
            : t < 0.5 ? 2 * t * t
            : t < 1.0 ? 1 - Mathf.Pow(-2 * t + 2, 2) / 2
            : 1.0f;
    }

    protected static float EaseInCubic(float t)
    {
        return t < 0 ? 0
             : t < 1.0 ? t * t * t
             : 1.0f;
    }

    protected static float EaseOutCubic(float t)
    {
        return t < 0 ? 0
            : t < 1.0 ? 1 - Mathf.Pow(1 - t, 3)
            : 1.0f;
    }

    protected static float EaseInOutCubic(float t)
    {
        return t < 0 ? 0
            : t < 0.5 ? 4 * t * t * t
            : t < 1.0 ? 1 - Mathf.Pow(-2 * t + 2, 3) / 2
            : 1.0f;
    }

    protected static float EaseInQuart(float t)
    {
        return t < 0 ? 0
            : t < 1.0 ? t * t * t * t
            : 1.0f;
    }

    protected static float EaseOutQuart(float t)
    {
        return t < 0 ? 0
           : t < 1.0 ? 1 - Mathf.Pow(1 - t, 4)
           : 1.0f;
    }

    protected static float EaseInOutQuart(float t)
    {
        return t < 0 ? 0
            : t < 0.5 ? 8 * t * t * t * t
            : t < 1.0 ? 1 - Mathf.Pow(-2 * t + 2, 4) / 2
            : 1.0f;
    }

    protected static float EaseInQuint(float t)
    {
        return t < 0 ? 0
            : t < 1.0 ? t * t * t * t * t
            : 1.0f;
    }

    protected static float EaseOutQuint(float t)
    {
        return t < 0 ? 0
            : t < 1.0 ? 1 - Mathf.Pow(1 - t, 5)
            : 1.0f;
    }

    protected static float EaseInOutQuint(float t)
    {
        return t < 0 ? 0
            : t < 0.5 ? 16 * t * t * t * t * t
            : t < 1.0 ? -Mathf.Pow(-2 * t + 2, 5) / 2
            : 1.0f;
    }

    protected static float EaseInExpo(float t)
    {
        return t <= 0 ? 0
            : t < 1.0 ? Mathf.Pow(2, 10 * t - 10)
            : 1.0f;
    }

    protected static float EaseOutExpo(float t)
    {
        return t < 0 ? 0
            : t < 1.0 ? 1 - Mathf.Pow(2, -10 * t)
            : 1;
    }

    protected static float EaseInOutExpo(float t)
    {
        return t < 0 ? 0
            : 1 == t ? 1
            : t < 0.5 ? Mathf.Pow(2, 20 * t - 10) / 2
            : (2 - Mathf.Pow(2, -20 * t + 10)) / 2;
    }

    protected static float EaseInCirc(float t)
    {
        return t < 0 ? 0
            : t < 1.0 ? 1 - Mathf.Sqrt(1 - Mathf.Pow(t, 2))
            : 1.0f;
    }

    protected static float EaseOutCirc(float t)
    {
        return t < 0 ? 0
            : t < 1.0 ? Mathf.Sqrt(1 - Mathf.Pow(t - 1, 2))
            : 1.0f;
    }

    protected static float EaseInOutCirc(float t)
    {
        return t < 0 ? 0
            : t < 0.5 ? (1 - Mathf.Sqrt(1 - Mathf.Pow(2 * t, 2))) / 2
            : t < 1.0 ? (Mathf.Sqrt(1 - Mathf.Pow(-2 * t + 2, 2)) + 1) / 2
            : 1.0f;
    }

    protected static float EaseInBack(float t)
    {
        return t < 0 ? 0
            : t < 1.0 ? c3 * t * t * t - c1 * t * t
            : 1.0f;
    }

    protected static float EaseOutBack(float t)
    {
        return t < 0 ? 0
            : t < 1.0 ? 1 + c3 * Mathf.Pow(t - 1, 3) + c1 * Mathf.Pow(t - 1, 2)
            : 1.0f;
    }

    protected static float EaseInOutBack(float t)
    {
        return t < 0 ? 0
            : t < 0.5 ? (Mathf.Pow(2 * t, 2) * ((c2 + 1) * 2 * t - c2)) / 2
            : t < 1.0 ? (Mathf.Pow(2 * t - 2, 2) * ((c2 + 1) * (t * 2 - 2) + c2) + 2) / 2
            : 1.0f;
    }

    protected static float EaseInElastic(float t)
    {
        return t <= 0 ? 0
            : 1 <= 0 ? 1
            : -Mathf.Pow(2, 10 * t - 10) * Mathf.Sin((t * 10 - 10.75f) * c4);
    }

    protected static float EaseOutElastic(float t)
    {
        return t <= 0 ? 0
            : 1 <= t ? 1
            : Mathf.Pow(2, -10 * t) * Mathf.Sin((t * 10 - 0.75f) * c4) + 1;
    }

    protected static float EaseInOutElastic(float t)
    {
        return t <= 0 ? 0
            : 1 <= t ? 1
            : t < 0.5 ? -(Mathf.Pow(2, 20 * t - 10) * Mathf.Sin((20 * t - 11.125f) * c5)) / 2
            : (Mathf.Pow(2, -20 * t + 10) * Mathf.Sin((20 * t - 11.125f) * c5)) / 2 + 1;
    }

    protected static float EaseInBounce(float t)
    {
        return t < 0 ? 0 :
            t < 1.0 ? 1 - EaseOutBounce(1 - t)
            : 1;
    }

    protected static float EaseOutBounce(float t)
    {
        const float n1 = 7.5625f;
        const float d1 = 2.75f;
        if (t < 1 / d1) return n1 * t * t;
        else if (t < 2 / d1) return n1 * (t -= 1.5f / d1) * t + 0.75f;
        else if (t < 2.5 / d1) return n1 * (t -= 2.25f / d1) * t + 0.9375f;
        else return n1 * (t -= 2.625f / d1) * t + 0.984375f;
    }

    protected static float EaseInOutBounce(float t)
    {
        return t < 0 ? 0
            : t < 0.5 ? (1 - EaseOutBounce(1 - 2 * t)) / 2
            : t < 1.0 ? (1 + EaseOutBounce(2 * t - 1)) / 2
            : 1.0f;
    }

}
