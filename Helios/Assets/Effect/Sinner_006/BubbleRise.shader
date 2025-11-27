Shader "Custom/BubbleOutline"
{
    Properties
    {
        _BubbleColor("Bubble Outline Color", Color) = (1,1,1,1)
        _BubbleCount("Bubble Count", Float) = 20
        _Speed("Rise Speed", Float) = 1.5
        _SizeMin("Bubble Size Min", Float) = 0.05
        _SizeMax("Bubble Size Max", Float) = 0.2
        _Sway("Sway Amount", Float) = 0.2
        _OutlineThickness("Outline Thickness", Float) = 0.02

        _Alpha("Overall Alpha", Range(0,1)) = 1.0   // ’Ç‰Á
    }

        SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float4 pos : SV_POSITION;
                float2 uv  : TEXCOORD0;
            };

            float4 _BubbleColor;
            float _BubbleCount;
            float _Speed;
            float _SizeMin;
            float _SizeMax;
            float _Sway;
            float _OutlineThickness;
            float _Alpha;   // ’Ç‰Á

            float hash(float2 p)
            {
                return frac(sin(dot(p, float2(12.9898, 78.233))) * 43758.5453);
            }

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float bubble_outline(float2 uv, float2 center, float radius, float thickness)
            {
                float d = length(uv - center);
                float edge = abs(d - radius);
                return smoothstep(thickness, thickness * 0.6, edge);
            }

            float4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv;
                float time = _Time.y * _Speed;

                float alpha = 0.0;

                for (int b = 0; b < _BubbleCount; b++)
                {
                    float randX = hash(float2(b, 0.1));
                    float randT = hash(float2(b, 3.7));

                    float y = frac(time + randT);
                    float x = randX;

                    float2 center = float2(x, y);

                    float size = lerp(_SizeMin, _SizeMax, hash(float2(b, 5.3)));

                    center.x += sin((time + randT) * 6.0 + b) * _Sway;

                    float outline = bubble_outline(uv, center, size, _OutlineThickness);
                    alpha += outline;
                }

                alpha = saturate(alpha);

                float4 col = _BubbleColor;
                col.a = alpha * _Alpha;   // “§–¾“x‚ðæŽZ

                return col;
            }

            ENDHLSL
        }
    }
}
