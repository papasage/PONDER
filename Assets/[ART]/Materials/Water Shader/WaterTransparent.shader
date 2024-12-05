Shader "Custom/WaterTransparent"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Main Texture", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _ScrollSpeedX ("Scroll Speed X", Range(-1, 1)) = 0.1
        _ScrollSpeedY ("Scroll Speed Y", Range(-1, 1)) = 0.1
        _WaveIntensity ("Wave Intensity", Range(0, 1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 200
        
        Blend SrcAlpha OneMinusSrcAlpha // Set blend mode to support transparency

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            float _ScrollSpeedX;
            float _ScrollSpeedY;
            float _WaveIntensity;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv;
                float2 offset = float2(_ScrollSpeedX * _Time.y, _ScrollSpeedY * _Time.x);
                uv += offset;
                float waveDistortion = sin(uv.x * 10 + _Time.y) + cos(uv.y * 10 + _Time.x);
                waveDistortion *= _WaveIntensity;

                fixed4 col = tex2D(_MainTex, uv + waveDistortion);
                col *= _Color;
                return col;
            }
            ENDCG
        }
    }
    FallBack "Transparent/Diffuse"
}
