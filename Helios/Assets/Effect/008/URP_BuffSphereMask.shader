Shader "Custom/URP_BuffSphereMask"
{
    Properties
    {
        _MainTex("Sprite Texture", 2D) = "white" {}
        _BaseColor("Base Color", Color) = (0.2,0.6,1,1)
        _GlowColor("Glow Color", Color) = (1,0.5,1,1)
        _GlowIntensity("Glow Intensity", Float) = 2.0

        _SphereCount("Sphere Count", Int) = 20
        _SphereSize("Sphere Size", Range(0.01,0.2)) = 0.05
        _SphereSpeed("Sphere Speed", Range(0.1,2.0)) = 0.5

        _TimeScale("Time Scale", Float) = 1.0
    }

        SubShader
        {
            Tags{
                "Queue" = "Transparent"
                "RenderType" = "Transparent"
                "IgnoreProjector" = "True"
                "CanUseSpriteAtlas" = "True"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off

            Pass
            {
                HLSLPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

                struct Attributes {
                    float4 positionOS : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct Varyings {
                    float4 positionHCS : SV_POSITION;
                    float2 uv : TEXCOORD0;
                };

                // === テクスチャ設定 ===
                TEXTURE2D(_MainTex);
                SAMPLER(sampler_MainTex);
                float4 _MainTex_ST;

                // === プロパティ ===
                float4 _BaseColor;
                float4 _GlowColor;
                float _GlowIntensity;
                int _SphereCount;
                float _SphereSize;
                float _SphereSpeed;
                float _TimeScale;

                // === 頂点処理 ===
                Varyings vert(Attributes IN)
                {
                    Varyings OUT;
                    OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                    OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                    return OUT;
                }

                // === 疑似ランダム ===
                float hash21(float2 p)
                {
                    p = frac(p * float2(123.34, 345.45));
                    p += dot(p, p + 34.345);
                    return frac(p.x * p.y);
                }

                // === フラグメント ===
                half4 frag(Varyings IN) : SV_Target
                {
                    float2 uv = IN.uv;
                    float t = _Time.y * _TimeScale;

                    // スプライトの形状マスク
                    float4 mainTex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);
                    if (mainTex.a < 0.01)
                    {
                        return half4(0, 0, 0, 0);
                    }

                    float alpha = 0.0;
                    float3 col = _BaseColor.rgb * mainTex.rgb;

                    // ==== 泡生成ループ ====
                    for (int i = 0; i < _SphereCount; i++)
                    {
                        float2 seed = float2(i * 17.123, i * 91.123);

                        // ベース位置（xはランダム固定）
                        float baseX = hash21(seed);

                        // 上昇速度
                        float speed = _SphereSpeed * (0.5 + hash21(seed + 2.0));
                        float y = frac(t * speed + hash21(seed + 3.0));

                        // ★ ゆらゆら揺れ（左右サイン波）
                        float swayAmp = 0.05 + hash21(seed + 4.0) * 0.05;    // 揺れ幅（個体差）
                        float swayFreq = 2.0 + hash21(seed + 5.0) * 2.0;     // 揺れ速度（個体差）
                        float x = baseX + sin((t + i) * swayFreq) * swayAmp;

                        float2 pos = float2(x, y);
                        float d = distance(uv, pos);

                        float size = _SphereSize * (0.5 + hash21(seed + 1.0));

                        // フェードアウト（上に行くほど薄く）
                        float fade = smoothstep(0.9, 0.7, y);

                        float sphere = smoothstep(size, size * 0.5, d);
                        float intensity = (1.0 - d / size) * 0.8 * fade;

                        alpha += sphere * intensity;
                        col += _GlowColor.rgb * sphere * _GlowIntensity * fade;
                    }

                    return half4(col, saturate(alpha) * mainTex.a);
                }
                ENDHLSL
            }
        }
}
