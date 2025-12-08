Shader "Custom/RGBHorizontalGlitch_Safe_LineShift_Alpha"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Intensity("RGB Glitch Amount", Range(0,1)) = 1
        _BurstNoise("RGB Horizontal Shift", Range(-200,200)) = 40
        _Speed("Noise Speed", Range(0,50)) = 15

        _LineShift("Line Shift Strength", Range(0,300)) = 120
        _LineDensity("Glitch Line Density", Range(50,1000)) = 300

        _Alpha("Alpha", Range(0,1)) = 1   // ★追加：透明度制御
    }

        SubShader
        {
            Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                sampler2D _MainTex;
                float4 _MainTex_ST;

                float _Intensity;
                float _BurstNoise;
                float _Speed;

                float _LineShift;
                float _LineDensity;

                float _Alpha; // ★ Alphaコントロール変数

                struct appdata { float4 vertex:POSITION; float2 uv:TEXCOORD0; };
                struct v2f { float4 pos:SV_POSITION; float2 uv:TEXCOORD0; };

                v2f vert(appdata v)
                {
                    v2f o;
                    o.pos = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv,_MainTex);
                    return o;
                }

                float randLine(float v) { return frac(sin(v * 1234.567 + _Time.y * _Speed) * 99999.0); }

                fixed4 frag(v2f i) : SV_Target
                {
                    // --- RGBシフト ---
                    float n = randLine(i.uv.y * 800.0);
                    float s = (n - 0.5) * _BurstNoise * _Intensity;

                    float2 uvR = clamp(i.uv + float2(s,0), 0,1);
                    float2 uvG = clamp(i.uv,               0,1);
                    float2 uvB = clamp(i.uv + float2(-s,0),0,1);

                    fixed4 cR = tex2D(_MainTex, uvR);
                    fixed4 cG = tex2D(_MainTex, uvG);
                    fixed4 cB = tex2D(_MainTex, uvB);

                    fixed4 rgb = fixed4(cR.r, cG.g, cB.b, cG.a);

                    // --- 横帯ノイズ ---
                    float band = floor(i.uv.y * _LineDensity);
                    float distort = (randLine(band) - 0.5) * _LineShift;

                    float2 uvBand = i.uv + float2(distort / 1024.0, 0);
                    fixed4 bandC = tex2D(_MainTex, uvBand);

                    fixed4 outColor = lerp(rgb, bandC, 0.8);

                    // ★ 最終的にアルファを適用
                    outColor.a *= _Alpha;

                    return outColor;
                }
                ENDCG
            }
        }
}
