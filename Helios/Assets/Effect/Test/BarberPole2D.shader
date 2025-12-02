Shader "Unlit/WarningStripeHDR"
{
    Properties
    {
        _Color1("Color1 (HDR)", Color) = (3,3,0,1)
        _Color2("Color2 (HDR)", Color) = (0,0,0,1)

        _Emission("Glow Power", Range(0,25)) = 1
        _Speed("Scroll Speed", float) = 1
        _StripeWidth("Stripe Width", float) = 8
        _Angle("Stripe Angle", float) = 45

        _Alpha("Alpha", Range(0,1)) = 1     //追加
    }

        SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
        ZWrite Off

        // 透過
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            float _Speed;
            float _StripeWidth;
            float _Angle;
            float _Emission;
            float _Alpha;
            float4 _Color1;
            float4 _Color2;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float rad = radians(_Angle);

                float2 uvRot = float2(
                    i.uv.x * cos(rad) - i.uv.y * sin(rad),
                    i.uv.x * sin(rad) + i.uv.y * cos(rad)
                );

                float scroll = uvRot.y + _Time.y * _Speed;
                float stripe = floor(scroll * _StripeWidth);

                float4 col = (fmod(stripe, 2) == 0) ? _Color1 : _Color2;

                // Emissionで光、Alphaで透明度
                float3 glow = col.rgb * _Emission;

                return float4(glow, _Alpha); //ポイント
            }
            ENDCG
        }
    }
}
