Shader "Custom/UnderwaterAirBubble"
{
    Properties
    {
        _OutlineColor("Outline Color", Color) = (1,1,1,1)
        _OutlineThickness("Outline Thickness", Range(0,0.2)) = 0.04

        _RefractionStrength("Refraction Strength", Range(0,1)) = 0.25
        _DistortionNoise("Distortion Noise", Range(0,1)) = 0.35

        _BubbleCount("Bubble Count", Float) = 20
        _Speed("Rise Speed", Float) = 1.4
        _SizeMin("Min Size", Float) = 0.05
        _SizeMax("Max Size", Float) = 0.14
        _Sway("Sway", Float) = 0.12

        // 追加：全体の透明度
        _GlobalAlpha("Global Alpha", Range(0,1)) = 1.0
    }

        SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv     : TEXCOORD0;
            };

            struct v2f {
                float4 pos : SV_POSITION;
                float2 uv  : TEXCOORD0;
                float2 screenUV : TEXCOORD1;
            };

            float4 _OutlineColor;
            float _OutlineThickness;

            float _RefractionStrength;
            float _DistortionNoise;

            float _BubbleCount;
            float _Speed;
            float _SizeMin;
            float _SizeMax;
            float _Sway;

            // 追加
            float _GlobalAlpha;

            sampler2D _ScreenTex;
            float4 _ScreenTex_TexelSize;

            float hash(float2 p)
            {
                return frac(sin(dot(p, float2(12.9898,78.233))) * 43758.5453);
            }

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);

                float4 sp = ComputeScreenPos(o.pos);
                o.screenUV = sp.xy / sp.w;

                o.uv = v.uv;
                return o;
            }

            float bubble_outline(float dist, float radius, float thick)
            {
                float edge = abs(dist - radius);
                return smoothstep(thick, thick * 0.2, edge);
            }

            float2 bubble_distort(float2 uv, float2 center, float radius, float time)
            {
                float2 delta = uv - center;
                float d = length(delta) / radius;

                float wobble = sin(delta.x * 20 + time * 2.0) * 0.01 +
                               sin(delta.y * 25 - time * 1.4) * 0.01;

                float distort = (1.0 - d) * wobble * _DistortionNoise;

                return uv + delta * distort;
            }

            float4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv;
                float time = _Time.y * _Speed;

                float3 col = 0;
                float alpha = 0;

                for (int n = 0; n < _BubbleCount; n++)
                {
                    float randX = hash(float2(n, 0.2));
                    float randT = hash(float2(n, 5.7));

                    float y = frac(time + randT);
                    float x = randX;

                    float2 center = float2(x, y);
                    float size = lerp(_SizeMin, _SizeMax, hash(float2(n, 4.3)));

                    center.x += sin((time + randT) * 3.0 + n) * _Sway;

                    float dist = length(uv - center);

                    // Outline
                    float outline = bubble_outline(dist, size, _OutlineThickness);
                    if (outline > 0.001)
                    {
                        col += _OutlineColor.rgb * outline;
                        alpha += outline * 0.8;
                    }

                    // Interior
                    if (dist < size)
                    {
                        float2 dUV = bubble_distort(uv, center, size, time);
                        float4 screenCol = tex2D(_ScreenTex, dUV);

                        col += screenCol.rgb * _RefractionStrength * (1.0 - dist / size);
                        alpha += (1.0 - dist / size) * 0.15;
                    }
                }

                // ここに追加：全体の透明度を掛けて出力
                return float4(col, alpha * _GlobalAlpha);
            }
            ENDHLSL
        }
    }
}
