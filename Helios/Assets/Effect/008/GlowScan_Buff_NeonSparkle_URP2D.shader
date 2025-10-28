Shader "Custom/GlowScan_Buff_NeonSparkle_URP2D"
{
    Properties
    {
        _MainTex("Sprite Texture", 2D) = "white" {}
        _ScanSpeed("Scan Speed", Float) = 0.3
        _ScanWidth("Scan Width", Range(0,0.5)) = 0.06
        _ScanCount("Scan Count", Int) = 2
        _ScanColor("Scan Color", Color) = (1.0,0.7,0.9,1.0)

        _NeonMode("Neon Mode (0=Off,1=On)", Int) = 1
        _GlowWidth("Glow Width", Range(0,0.5)) = 0.12
        _GlowIntensity("Glow Intensity", Range(0,3)) = 2.0
        _GlowAlpha("Glow Alpha", Range(0,1)) = 0.6

        _Interval("Interval Time", Float) = 0.5

        _WaveAmp("Wave Amplitude", Float) = 0.015
        _WaveFreq("Wave Frequency", Float) = 15.0
        _WaveSpeed("Wave Speed", Float) = 1.5

        _SparkFreq("Spark Frequency", Float) = 30.0
        _SparkSpeed("Spark Speed", Float) = 10.0
        _SparkIntensity("Spark Intensity", Float) = 1.0

        _ScanMinY("Scan Min Y", Range(0,1)) = 0.0
        _ScanMaxY("Scan Max Y", Range(0,1)) = 1.0
    }

        SubShader
        {
            Tags { "RenderType" = "Transparent" "Queue" = "Transparent" "IgnoreProjector" = "True" "SRPDefaultUnlit" = "True" }

            Pass
            {
                Name "FORWARD"
                Tags { "LightMode" = "SRPDefaultUnlit" }

                Blend SrcAlpha OneMinusSrcAlpha
                ZWrite Off

                HLSLPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

                sampler2D _MainTex;
                float4 _MainTex_ST;

                float _ScanSpeed;
                float _ScanWidth;
                int _ScanCount;
                float4 _ScanColor;

                int _NeonMode;
                float _GlowWidth;
                float _GlowIntensity;
                float _GlowAlpha;

                float _Interval;

                float _WaveAmp;
                float _WaveFreq;
                float _WaveSpeed;

                float _SparkFreq;
                float _SparkSpeed;
                float _SparkIntensity;

                float _ScanMinY;
                float _ScanMaxY;

                struct Attributes
                {
                    float4 positionOS : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct Varyings
                {
                    float4 positionCS : SV_POSITION;
                    float2 uv : TEXCOORD0;
                };

                Varyings vert(Attributes IN)
                {
                    Varyings OUT;
                    OUT.positionCS = TransformObjectToHClip(IN.positionOS.xyz);
                    OUT.uv = IN.uv;
                    return OUT;
                }

                half4 frag(Varyings IN) : SV_Target
                {
                    float2 uv = IN.uv;
                    float scanRange = max(0.001, _ScanMaxY - _ScanMinY);
                    float uvYInRange = (uv.y - _ScanMinY) / scanRange;

                    // 高さ範囲外は透明
                    if (uv.y < _ScanMinY || uv.y > _ScanMaxY)
                        return half4(0,0,0,0);

                    float cycleTime = _Interval + 1.0;
                    float tCycle = fmod(_Time.y * _ScanSpeed, cycleTime);
                    float scanPhase = saturate(tCycle / 1.0);

                    float intensity = 0;

                    for (int n = 0; n < _ScanCount; n++)
                    {
                        float scanPos = fmod(scanPhase + (float)n / _ScanCount, 1.0);
                        float wave = sin(uv.x * _WaveFreq + _Time.y * _WaveSpeed) * _WaveAmp;
                        float distLine = abs(uvYInRange - (scanPos + wave));

                        float lineIntensity = 1.0 - smoothstep(0, _ScanWidth, distLine);

                        // キラキラ演出
                        float spark = sin(uv.x * _SparkFreq + _Time.y * _SparkSpeed + n) * 0.5 + 0.5;
                        lineIntensity += spark * lineIntensity * _SparkIntensity;

                        intensity += lineIntensity;

                        // ネオン光（Emission）
                        if (_NeonMode == 1)
                        {
                            float glow = 1.0 - smoothstep(0, _ScanWidth + _GlowWidth, distLine);
                            lineIntensity += glow * _GlowIntensity * 2.0;
                        }
                    }

                    intensity = saturate(intensity);

                    // 上下端フェード
                    float fadeEdge = smoothstep(0.0, _ScanWidth, uvYInRange) * (1.0 - smoothstep(1.0 - _ScanWidth, 1.0, uvYInRange));
                    intensity *= fadeEdge;

                    half4 col = _ScanColor;

                    if (_NeonMode == 1)
                    {
                        col.a = saturate(_GlowAlpha * intensity);
                        col.rgb += col.rgb * intensity * _GlowIntensity * 2.0; // Bloomで光る
                    }
                    else
                    {
                        col.a = _GlowAlpha * intensity;
                    }

                    return col;
                }
                ENDHLSL
            }
        }
}
