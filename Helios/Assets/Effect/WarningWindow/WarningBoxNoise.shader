Shader "Custom/WarningBoxNoise"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Tint("Tint Color", Color) = (1,1,1,1)
        _Alpha("Alpha", Range(0,1)) = 1.0
        _NoiseStrength("Noise Strength", Range(0,0.2)) = 0.08
        _NoiseSpeed("Noise Speed", Float) = 1.5
        _BlockWidth("Block Width", Range(0.01,0.3)) = 0.05
    }

        SubShader
        {
            Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
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
                float4 _Tint;
                float _Alpha;
                float _NoiseStrength;
                float _NoiseSpeed;
                float _BlockWidth;

                struct appdata
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f
                {
                    float4 pos : SV_POSITION;
                    float2 uv : TEXCOORD0;
                };

                // 2Dハッシュ（安定した疑似乱数）
                float hash(float2 p)
                {
                    return frac(sin(dot(p, float2(12.9898,78.233))) * 43758.5453);
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

                    // 横方向ボックスノイズ
                    float blockIndex = floor(uv.y / _BlockWidth);
                    float n = hash(float2(blockIndex, _Time.y * _NoiseSpeed));
                    uv.x += (n - 0.5) * _NoiseStrength;

                    fixed4 col = tex2D(_MainTex, uv);

                    // Tint適用
                    col.rgb *= _Tint.rgb;

                    // α値調整
                    col.a *= tex2D(_MainTex, i.uv).a; // 元テクスチャのαを保持
                    col.a *= _Alpha;                 // 外部で指定したαを掛ける

                    return col;
                }

                ENDCG
            }
        }
}
