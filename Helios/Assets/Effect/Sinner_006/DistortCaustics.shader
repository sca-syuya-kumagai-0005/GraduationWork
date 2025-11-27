Shader "Hidden/Underwater/DistortCaustics_Transparent"
{
    Properties
    {
        _MainTex("Base (RGB)", 2D) = "white" {}
        _NormalTex("Normal Map (RGBA)", 2D) = "bump" {}
        _CausticsTex("Caustics (RGBA)", 2D) = "white" {}
        _TimeScale("Time Scale", Float) = 1.0
        _DistortStrength("Distort Strength", Float) = 0.02
        _WaveAmplitude("Wave Amp", Float) = 0.06
        _CausticsIntensity("Caustics Intensity", Float) = 0.8
        _BlueTint("Blue Tint", Range(0,1)) = 0.25
        _ChromaticStrength("Chromatic Strength", Float) = 0.002
        _Alpha("Transparency", Range(0,1)) = 1.0
    }

        SubShader
        {
            Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }

            Pass
            {
                ZTest Always
                Cull Off
                ZWrite Off

            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appv {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            sampler2D _NormalTex;
            sampler2D _CausticsTex;

            float4 _MainTex_TexelSize;

            float _TimeScale;
            float _DistortStrength;
            float _WaveAmplitude;
            float _CausticsIntensity;
            float _BlueTint;
            float _ChromaticStrength;
            float _Alpha;

            v2f vert(appv v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float2 waveOffset(float2 uv, float t)
            {
                float3 n = tex2D(_NormalTex, uv * 2.0 + float2(t * 0.05, t * 0.03)).rgb;
                float2 nxy = (n.xy - 0.5) * 2.0;

                float big = sin((uv.x + t * 0.2) * 6.2831) * 0.5 +
                            sin((uv.y - t * 0.15) * 6.2831) * 0.25;

                float medium = sin((uv.x * 2.3 + t * 0.6) * 6.2831) * 0.15;

                float2 wave = float2(big, medium);

                return nxy * _DistortStrength + wave * _WaveAmplitude;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float t = _Time.y * _TimeScale;
                float2 uv = i.uv;

                float2 off = waveOffset(uv, t);

                float2 center = uv - 0.5;
                float radial = length(center);

                off += center * (sin(t * 1.3 - radial * 10.0) * 0.002);

                float2 uvR = uv + off * 1.0;
                float2 uvG = uv + off * 0.85;
                float2 uvB = uv + off * 0.7;

                float3 sceneCol;
                sceneCol.r = tex2D(_MainTex, uvR).r;
                sceneCol.g = tex2D(_MainTex, uvG).g;
                sceneCol.b = tex2D(_MainTex, uvB).b;

                float2 cuv = uv * 4.0 + float2(t * 0.5, t * 0.35);
                float3 caust = tex2D(_CausticsTex, cuv).rgb;

                float caustMask = saturate(dot(caust, float3(0.33,0.33,0.33)) * 2.0);
                sceneCol += caust * _CausticsIntensity * caustMask * (1.0 - radial);

                float3 blueShift = lerp(sceneCol, float3(0.0, 0.2, 0.5),
                                         _BlueTint * (0.5 + radial));
                sceneCol = lerp(sceneCol, blueShift, _BlueTint);

                sceneCol *= lerp(1.0, 0.6, pow(radial, 1.5));

                return float4(sceneCol, _Alpha);
            }

            ENDHLSL
        }
        }
}
