Shader "Custom/ECGWaveShader"
{
    Properties
    {
        _LineColor("Line Color", Color) = (0, 1, 0, 1)
        _BackgroundColor("Background Color", Color) = (0, 0, 0, 1)
        _LineWidth("Line Width", Float) = 2.0
        _Speed("Speed", Float) = 1.0
        _Alpha("Alpha", Range(0,1)) = 1.0

        _CenterLineColor("Center Line Color", Color) = (1, 1, 1, 0.5)
        _CenterLineWidth("Center Line Width", Float) = 1.0
        _CenterLineAlpha("Center Line Alpha", Range(0,1)) = 0.5
        _GlowWidth("Center Line Glow Width", Float) = 0.01

        _WaveCount("Wave Count", Float) = 5.0
        _WaveWidth("Wave Width", Float) = 1.0
        _WaveHeight("Wave Height (Max)", Float) = 0.2
        _WaveHeightRandomMax("Wave Height Random Max", Float) = 0.05
        _RandomRange("Random Range", Float) = 0.4

        _BackgroundVisible("Show Background", Float) = 1.0
        _CenterLineNoiseStrength("Center Line Noise Strength", Float) = 0.005
        _CenterLineNoiseSpeed("Center Line Noise Speed", Float) = 1.0
    }

    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            float4 _LineColor;
            float4 _BackgroundColor;
            float _LineWidth;
            float _Speed;
            float _Alpha;

            float4 _CenterLineColor;
            float _CenterLineWidth;
            float _CenterLineAlpha;
            float _GlowWidth;

            float _WaveCount;
            float _WaveWidth;
            float _WaveHeight;
            float _WaveHeightRandomMax;
            float _RandomRange;

            float _BackgroundVisible;
            float _CenterLineNoiseStrength;
            float _CenterLineNoiseSpeed;

            float hash(float x)
            {
                return frac(sin(x * 12.9898) * 43758.5453);
            }

            float PWave(float x, float center, float width, float height)
            {
                float dx = (x - center) / width;
                return height * exp(-dx * dx);
            }

            float QRSComplex(float x, float center, float wQ, float wR, float wS, float hQ, float hR, float hS)
            {
                float q = -hQ * exp(-pow((x - (center - wQ)), 2) * 100);
                float r = hR * exp(-pow((x - center), 2) * 100);
                float s = -hS * exp(-pow((x - (center + wS)), 2) * 100);
                return q + r + s;
            }

            float TWave(float x, float center, float width, float height)
            {
                float dx = (x - center) / width;
                return height * exp(-dx * dx);
            }

            float ECG(float x, float time)
            {
                float totalLength = _WaveCount;
                float scaledX = x * totalLength;

                float cycleIndex = floor(scaledX);
                float localX = frac(scaledX);

                float randValP = hash(cycleIndex + 1.0);
                float randHeightP = hash(cycleIndex * 10.0 + 1.0);
                float pHeight = 0.1 * _WaveHeight + (lerp(-_RandomRange, _RandomRange, randValP)) * 0.1 * _WaveHeight + (randHeightP - 0.5) * _WaveHeightRandomMax;

                float randValT = hash(cycleIndex + 2.0);
                float randHeightT = hash(cycleIndex * 10.0 + 2.0);
                float tHeight = 0.3 * _WaveHeight + (lerp(-_RandomRange, _RandomRange, randValT)) * 0.2 * _WaveHeight + (randHeightT - 0.5) * _WaveHeightRandomMax;

                float randValR = hash(cycleIndex + 3.0);
                float randHeightR = hash(cycleIndex * 10.0 + 3.0);
                float rHeight = 1.2 * _WaveHeight + (lerp(-_RandomRange, _RandomRange, randValR)) * 0.2 * _WaveHeight + (randHeightR - 0.5) * _WaveHeightRandomMax;

                float offsetP = lerp(-_RandomRange, _RandomRange, randValP) * _WaveWidth;
                float offsetT = lerp(-_RandomRange, _RandomRange, randValT) * _WaveWidth;

                float p = PWave(localX, 0.1 + offsetP * 0.05, 0.03 * _WaveWidth, pHeight);
                float qrs = QRSComplex(localX, 0.2,
                    0.015 * _WaveWidth, 0.02 * _WaveWidth, 0.015 * _WaveWidth,
                    0.5 * _WaveHeight, rHeight, 0.7 * _WaveHeight);
                float t = TWave(localX, 0.4 + offsetT * 0.1, 0.05 * _WaveWidth, tHeight);

                return p + qrs + t;
            }

            float GaussianBlur(float dist, float width)
            {
                float sigma = width * 0.5;
                return exp(-(dist * dist) / (2.0 * sigma * sigma));
            }

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv;
                float time = _Time.y * _Speed;

                float scrollSpeed = 0.2;
                float ecgValue = ECG(uv.x + time * scrollSpeed, time);

                float baseLine = 0.45;

                float noise = sin(_Time.y * _CenterLineNoiseSpeed + uv.x * 10.0) * _CenterLineNoiseStrength;
                float centerLineY = baseLine + noise;

                float waveY = baseLine + ecgValue;

                float distWave = abs(uv.y - waveY);
                float alphaLineWave = smoothstep(_LineWidth * 0.002, 0.0, distWave);
                float4 lineColorWave = float4(_LineColor.rgb, _Alpha) * alphaLineWave;

                float distCenter = abs(uv.y - centerLineY);
                float alphaLineCenter = smoothstep(_CenterLineWidth * 0.002, 0.0, distCenter);
                float glowAlpha = GaussianBlur(distCenter, _GlowWidth) * _CenterLineAlpha;

                float4 lineColorCenter = float4(_CenterLineColor.rgb, alphaLineCenter * _CenterLineAlpha);
                float4 glowColor = float4(_CenterLineColor.rgb, glowAlpha * 0.3);
                float4 centerLineFinal = lineColorCenter + glowColor;
                centerLineFinal.a = saturate(centerLineFinal.a);

                float4 innerColor = float4(0, 0, 0, 0);
                if ((waveY > centerLineY && uv.y > centerLineY && uv.y < waveY) ||
                    (waveY < centerLineY && uv.y < centerLineY && uv.y > waveY))
                {
                    float t = abs(uv.y - centerLineY) / abs(waveY - centerLineY);
                    float alphaInner = pow(1.0 - t, 2.0);
                    innerColor = float4(_LineColor.rgb, _Alpha * 0.4) * alphaInner;
                }

                float4 waveAndInner = saturate(lineColorWave + innerColor);
                float4 finalColor = lerp(_BackgroundColor, waveAndInner, waveAndInner.a);

                float4 output = lerp(finalColor, waveAndInner, 1.0 - _BackgroundVisible);

                return lerp(output, centerLineFinal, centerLineFinal.a);
            }
            ENDHLSL
        }
    }
}
