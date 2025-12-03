Shader "Custom/NeonExternalGlowStrong"
{
    Properties
    {
        _MainTex ("Sprite", 2D) = "white" {}
        _Color ("Color", Color) = (2,1,4,1)
        _GlowSize ("Glow Size", Range(0,3)) = 1
        _GlowIntensity ("Glow Intensity", Range(0,8)) = 4
        _CoreBoost ("Core Brightness", Range(1,4)) = 1.5
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }

        // ============================
        // PASS 1: OUTER GLOW (8 •ûŒü)
        // ============================
        Pass
        {
            Blend SrcAlpha One

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float _GlowSize;
            float _GlowIntensity;

            struct appdata { float4 vertex:POSITION; float2 uv:TEXCOORD0; };
            struct v2f { float4 pos:SV_POSITION; float2 uv:TEXCOORD0; };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = TransformObjectToHClip(v.vertex.xyz);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                float2 dirs[8] = {
                    float2( 1, 0), float2(-1, 0),
                    float2( 0, 1), float2( 0,-1),
                    float2( 1, 1), float2(-1, 1),
                    float2( 1,-1), float2(-1,-1)
                };

                float glow = 0;

                for (int n = 0; n < 8; n++)
                {
                    glow += tex2D(_MainTex, i.uv + dirs[n] * _GlowSize).a;
                }

                glow = glow * _GlowIntensity;

                float4 col = _Color;
                col.rgb *= glow;
                col.a = glow * 0.7;

                return col;
            }
            ENDHLSL
        }

        // ============================
        // PASS 2: MAIN SPRITE
        // ============================
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag2
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float _CoreBoost;

            struct appdata { float4 vertex:POSITION; float2 uv:TEXCOORD0; };
            struct v2f { float4 pos:SV_POSITION; float2 uv:TEXCOORD0; };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = TransformObjectToHClip(v.vertex.xyz);
                o.uv = TRANSFORM_TEX(v.uv,_MainTex);
                return o;
            }

            float4 frag2(v2f i) : SV_Target
            {
                float4 c = tex2D(_MainTex, i.uv) * _Color;

                // ƒpƒlƒ‹–{‘Ì‚ð‹­’²
                c.rgb *= _CoreBoost;
                return c;
            }
            ENDHLSL
        }
    }
}
