Shader "Custom/005_Fade_Shader"
{
    Properties
    {
        _Fade("Fade Amount", Range(0,1)) = 0
        _Feather("Feather (Softness)", Range(0.001,1.0)) = 0.3
        _Power("Gradient Power", Range(0.1,5)) = 1
    }
        SubShader
    {
        Tags { "Queue" = "Overlay" "RenderType" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            float _Fade;
            float _Feather;
            float _Power;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float y = i.uv.y;

            float alpha = smoothstep(1.0 - _Fade - _Feather, 1.0 - _Fade, y);
            alpha = pow(alpha, _Power);

            return fixed4(0,0,0,alpha);
        }
        ENDCG
    }
    }
}
