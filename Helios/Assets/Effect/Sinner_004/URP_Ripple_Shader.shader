Shader "Custom/URP_Ripple"
{
    Properties
    {
        _BackgroundTex("Background", 2D) = "white" {}
        _TimeScale("Speed", Float) = 0.1
        _RippleStrength("Strength", Float) = 0.05
        _WaveFrequency("Wave Frequency", Float) = 15.0
        _WaveDecay("Wave Decay", Float) = 1.5
        _BlurSize("Blur Size", Range(0,5)) = 1.0
    }

        SubShader
        {
            Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
            Blend SrcAlpha OneMinusSrcAlpha

            Pass
            {
                HLSLPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

                #define MAX_WAVES 10   //同時に出せる波の最大数

                struct Attributes { float4 positionOS : POSITION; float2 uv : TEXCOORD0; };
                struct Varyings { float4 positionHCS : SV_POSITION; float2 uv : TEXCOORD0; };

                sampler2D _BackgroundTex;
                float4 _BackgroundTex_TexelSize;

                float _TimeScale, _RippleStrength, _WaveFrequency, _WaveDecay, _BlurSize;

                //波の情報
                float4 _WaveStartPos[MAX_WAVES];   // xy: UV座標
                float  _WaveStartTime[MAX_WAVES];  // 開始時間
                int    _ActiveWaveCount;           // 有効な波の数

                Varyings vert(Attributes v)
                {
                    Varyings o;
                    o.positionHCS = TransformObjectToHClip(v.positionOS);
                    o.uv = v.uv;
                    return o;
                }

                half4 frag(Varyings i) : SV_Target
                {
                    float2 uv = i.uv;
                    float rippleSum = 0.0;

                    // --- 全ての波を合成 ---
                    for (int idx = 0; idx < _ActiveWaveCount; idx++)
                    {
                        float2 dir = uv - _WaveStartPos[idx].xy;
                        float dist = length(dir);

                        float t = _Time.y - _WaveStartTime[idx];
                        if (t < 0) continue;

                        float radius = t * _TimeScale;
                        float wave = sin(dist * _WaveFrequency - radius * _WaveFrequency);

                        float attenuationDist = exp(-_WaveDecay * (dist + radius));
                        float attenuationTime = exp(-0.5 * t);
                        float ripple = wave * attenuationDist * attenuationTime;

                        float ring = smoothstep(-0.02, 0.02, dist - radius);
                        ripple *= (1.0 - ring);

                        rippleSum += ripple;
                    }

                    // --- 屈折 ---
                    float2 dirMain = uv - 0.5; // 方向ベクトル（適当）
                    float2 tangent = float2(-dirMain.y, dirMain.x);
                    float2 offset = normalize(dirMain) * rippleSum * 0.7 + tangent * rippleSum * 0.3;
                    float2 refractUV = uv + offset * _RippleStrength;

                    // --- ブラー ---
                    float blurAmount = abs(rippleSum) * _BlurSize;
                    half4 col = tex2D(_BackgroundTex, refractUV);
                    col += tex2D(_BackgroundTex, refractUV + float2(_BackgroundTex_TexelSize.x, 0) * blurAmount);
                    col += tex2D(_BackgroundTex, refractUV + float2(-_BackgroundTex_TexelSize.x, 0) * blurAmount);
                    col += tex2D(_BackgroundTex, refractUV + float2(0, _BackgroundTex_TexelSize.y) * blurAmount);
                    col += tex2D(_BackgroundTex, refractUV + float2(0, -_BackgroundTex_TexelSize.y) * blurAmount);
                    col /= 5.0;

                    col.a = saturate(abs(rippleSum) * 2.0);

                    return col;
                }
                ENDHLSL
            }
        }
}
