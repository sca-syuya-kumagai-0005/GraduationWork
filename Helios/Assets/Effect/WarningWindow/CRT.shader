Shader "Custom/CRT_Fixed"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Line("ScanLine Strength", Range(0,1)) = 0.2
        _Tint("Tint Color", Color) = (1,1,1,1)
        _Distortion("Horizontal Distortion", Range(0, 0.05)) = 0.01
        _Noise("Noise Strength", Range(0,0.2)) = 0.05
        _ScanFreq("Scanline Frequency", Float) = 800
        _NoiseSpeed("Noise Speed", Float) = 1.0
    }

        SubShader
        {
            Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                sampler2D _MainTex;
                float4 _MainTex_ST;
                float _Line;
                float4 _Tint;
                float _Distortion;
                float _Noise;
                float _ScanFreq;
                float _NoiseSpeed;

                struct appdata {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f {
                    float4 pos : SV_POSITION;
                    float2 uv : TEXCOORD0;
                };

                static inline float hash21(float2 p)
                {
                    p = frac(p * float2(127.1, 311.7));
                    p += dot(p, p + 34.345);
                    return frac(sin(p.x * 43758.5453 + p.y * 24634.634));
                }

                v2f vert(appdata v)
                {
                    v2f o;
                    o.pos = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    float2 uv = i.uv;

                    uv.x += sin(uv.y * 3.14159) * _Distortion;

                    float n = hash21(uv * 100.0 + float2(_Time.y * _NoiseSpeed, 0.0));
                   
                    float noiseOffset = (n - 0.5) * 2.0 * _Noise;
                    uv.x += noiseOffset;

                   
                    fixed4 col = tex2D(_MainTex, uv);

                   
                    float scan = sin(uv.y * _ScanFreq) * _Line;
                    col.rgb -= scan;

                  
                    col.rgb *= _Tint.rgb;

                    return col;
                }

                ENDCG
            }
        }
}
