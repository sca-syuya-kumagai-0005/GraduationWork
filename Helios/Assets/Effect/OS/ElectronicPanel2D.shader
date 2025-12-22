Shader "Custom/ElectronicPanel2D"
{
    Properties
    {
        _MainTex("Sprite Texture", 2D) = "white" {}
        _PanelColor("Panel Color", Color) = (0.2, 0.8, 1, 1)

        _GlobalGlow("Global Glow", Range(0,2)) = 0.6

        _ScanStrength("Scan Line Strength", Range(0,0.3)) = 0.08
        _ScanDensity("Scan Line Density", Range(50,500)) = 200
        _EdgeGlow("Edge Glow", Range(0,1)) = 0.35
    }

        SubShader
        {
            Tags
            {
                "Queue" = "Transparent"
                "RenderType" = "Transparent"
                "IgnoreProjector" = "True"
                "RenderPipeline" = "UniversalPipeline"
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

                struct Attributes
                {
                    float4 positionOS : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct Varyings
                {
                    float4 positionHCS : SV_POSITION;
                    float2 uv : TEXCOORD0;
                };

                TEXTURE2D(_MainTex);
                SAMPLER(sampler_MainTex);

                float4 _PanelColor;
                float _GlobalGlow;
                float _ScanStrength;
                float _ScanDensity;
                float _EdgeGlow;

                Varyings vert(Attributes IN)
                {
                    Varyings OUT;
                    OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                    OUT.uv = IN.uv;
                    return OUT;
                }

                half4 frag(Varyings IN) : SV_Target
                {
                    half4 tex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv);

                    // 透明部分保持
                    if (tex.a <= 0) discard;

                    // ベースカラー
                    half3 color = tex.rgb * _PanelColor.rgb;

                    // ===== 全体グロー =====
                    color += color * _GlobalGlow;

                    // スキャンライン（静的）
                    float scan = sin(IN.uv.y * _ScanDensity) * _ScanStrength;
                    color -= scan;

                    // エッジグロー
                    float2 centeredUV = abs(IN.uv - 0.5) * 2;
                    float edge = saturate(max(centeredUV.x, centeredUV.y));
                    color += edge * _EdgeGlow * _PanelColor.rgb;

                    return half4(color, tex.a * _PanelColor.a);
                }
                ENDHLSL
            }
        }
}
