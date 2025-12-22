Shader "Custom/BoxNoise2D_FullyRandom_Glow_HDR_Blur_Texture"
{
    Properties
    {
        // ===== Texture =====
        _BoxTex("Box Texture", 2D) = "white" {}

    // ===== Box Control =====
    _BoxCountMin("Box Count Min", Range(0,50)) = 5
    _BoxCountMax("Box Count Max", Range(0,50)) = 15
    _SwitchSpeed("Switch Speed", Range(0.1,100)) = 1

    _BoxSizeMin("Box Size Min", Range(0.02,0.3)) = 0.05
    _BoxSizeMax("Box Size Max", Range(0.02,0.5)) = 0.15

        // ===== Blur =====
        _BlurSize("Edge Blur Size", Range(0.0,0.1)) = 0.02

        // ===== Emission =====
        _GlobalEmission("Global Emission", Range(0,5)) = 1

        _Emission1("Emission 1", Range(0,10)) = 3
        _Emission2("Emission 2", Range(0,10)) = 3
        _Emission3("Emission 3", Range(0,10)) = 3
        _Emission4("Emission 4", Range(0,10)) = 3
        _Emission5("Emission 5", Range(0,10)) = 3

        // ===== Color Tint =====
        _Color1("Color 1", Color) = (0,1,1,1)
        _Color2("Color 2", Color) = (0.2,0.8,1,1)
        _Color3("Color 3", Color) = (0.5,0.9,1,1)
        _Color4("Color 4", Color) = (0.3,0.6,1,1)
        _Color5("Color 5", Color) = (0.1,0.4,0.8,1)

        // ===== Alpha =====
        _AlphaMin("Alpha Min", Range(0,1)) = 0.2
        _AlphaMax("Alpha Max", Range(0,1)) = 0.8
    }

        SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
            "RenderPipeline" = "UniversalPipeline"
        }

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

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

            TEXTURE2D(_BoxTex);
            SAMPLER(sampler_BoxTex);

            float _BoxCountMin, _BoxCountMax;
            float _SwitchSpeed;
            float _BoxSizeMin, _BoxSizeMax;
            float _BlurSize;

            float _GlobalEmission;
            float _Emission1, _Emission2, _Emission3, _Emission4, _Emission5;

            float4 _Color1,_Color2,_Color3,_Color4,_Color5;
            float _AlphaMin,_AlphaMax;

            struct ColorEmission
            {
                float4 color;
                float emission;
            };

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                return OUT;
            }

            float hash(float n)
            {
                return frac(sin(n) * 43758.5453);
            }

            ColorEmission PickColorEmission(float r)
            {
                ColorEmission ce;

                if (r < 0.2) { ce.color = _Color1; ce.emission = _Emission1; }
                else if (r < 0.4) { ce.color = _Color2; ce.emission = _Emission2; }
                else if (r < 0.6) { ce.color = _Color3; ce.emission = _Emission3; }
                else if (r < 0.8) { ce.color = _Color4; ce.emission = _Emission4; }
                else { ce.color = _Color5; ce.emission = _Emission5; }

                return ce;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float timeStep = floor(_Time.y * _SwitchSpeed);
                int boxCount = (int)lerp(_BoxCountMin, _BoxCountMax, hash(timeStep * 9.91));

                half4 result = half4(0,0,0,0);

                for (int i = 0; i < 50; i++)
                {
                    if (i >= boxCount) break;

                    float seed = i * 13.1 + timeStep * 17.7;

                    float2 pos = float2(hash(seed + 1.1), hash(seed + 2.2));
                    float size = lerp(_BoxSizeMin, _BoxSizeMax, hash(seed + 3.3));

                    float2 d = abs(IN.uv - pos);
                    float edgeX = size - d.x;
                    float edgeY = size - d.y;

                    if (edgeX > 0 && edgeY > 0)
                    {
                        float edgeFade = saturate(min(edgeX, edgeY) / max(_BlurSize, 0.0001));

                        float2 boxUV = (IN.uv - (pos - size)) / (size * 2);

                        float4 texCol = SAMPLE_TEXTURE2D(_BoxTex, sampler_BoxTex, boxUV);

                        ColorEmission ce = PickColorEmission(hash(seed + 4.4));

                        float4 col = texCol * ce.color;

                        col.a *= lerp(_AlphaMin, _AlphaMax, hash(seed + 5.5)) * edgeFade;

                        float3 emission =
                            col.rgb *
                            ce.emission *
                            _GlobalEmission *
                            edgeFade;

                        col.rgb += emission;

                        result = col;
                    }
                }

                return result;
            }
            ENDHLSL
        }
    }
}
