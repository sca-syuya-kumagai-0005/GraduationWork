Shader "Custom/2DLightExpandDissolve"
{
    Properties
    {
        _Color("Light Color", Color) = (1,1,1,1)
        _Radius("Radius", Float) = 0.0
        _Feather("Feather", Float) = 0.2
        _DissolveTex("Dissolve Pattern", 2D) = "white" {}
        _Dissolve("Dissolve Amount", Range(0,1)) = 0.0
        _EdgeColor("Edge Color", Color) = (1,0.8,0.3,1)
        _EdgeWidth("Edge Width", Range(0,1)) = 0.05
    }
        SubShader
        {
            Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off ZWrite Off

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                float4 _Color;
                float _Radius;
                float _Feather;
                sampler2D _DissolveTex;
                float4 _DissolveTex_ST;
                float _Dissolve;
                float4 _EdgeColor;
                float _EdgeWidth;

                struct appdata
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f
                {
                    float4 vertex : SV_POSITION;
                    float2 uv : TEXCOORD0;
                };

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _DissolveTex);
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    float2 center = float2(0.5, 0.5);
                    float dist = distance(i.uv, center);

                    // 基本の光フェード
                    float alpha = smoothstep(_Radius, _Radius - _Feather, dist);

                    // ディゾルブノイズ
                    float noise = tex2D(_DissolveTex, i.uv).r;
                    float edge = smoothstep(_Dissolve, _Dissolve - _EdgeWidth, noise);

                    // 内側の光
                    float inner = step(_Dissolve, noise);

                    // エッジカラー追加
                    float4 col = lerp(_Color, _EdgeColor, edge - inner);

                    col.a *= alpha * edge;
                    return col;
                }
                ENDCG
            }
        }
}
