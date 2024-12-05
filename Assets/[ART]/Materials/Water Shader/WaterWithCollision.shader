Shader "Custom/WaterWithCollision"
{
    Properties
    {
        // Properties for water color, texture, and other parameters
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Main Texture", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _ScrollSpeedX ("Scroll Speed X", Range(-1, 1)) = 0.1
        _ScrollSpeedY ("Scroll Speed Y", Range(-1, 1)) = 0.1
        _WaveIntensity ("Wave Intensity", Range(0, 1)) = 0.5
        _FoamColor ("Foam Color", Color) = (1,1,1,1)
        _FoamThreshold ("Foam Threshold", Range(0, 1)) = 0.5
        _RippleColor ("Ripple Color", Color) = (1,1,1,1)
        _RippleThreshold ("Ripple Threshold", Range(0, 1)) = 0.5
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
            float _ScrollSpeedX;
            float _ScrollSpeedY;
            float _WaveIntensity;
            float4 _FoamColor;
            float _FoamThreshold;
            float4 _RippleColor;
            float _RippleThreshold;

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

                fixed4 waterColor = tex2D(_MainTex, uv + waveDistortion);

                // Check for foam generation
                if (FoamDetected(uv))
                {
                    waterColor.rgb += _FoamColor.rgb;
                }

                // Check for ripple generation
                if (RippleDetected(uv))
                {
                    waterColor.rgb += _RippleColor.rgb;
                }

                waterColor.rgb *= _Color.rgb;

                // Apply alpha blending for transparency
                waterColor.a *= _Color.a;

                return waterColor;
            }

            bool FoamDetected(float2 uv)
            {
                // Implement collision detection logic for foam generation
                // For example, check if the current UV coordinate intersects with another object
                // You can use Unity's built-in functions like tex2Dproj to sample nearby pixels
                // and detect collisions.
                return false; // Placeholder, replace with actual logic
            }

            bool RippleDetected(float2 uv)
            {
                // Implement collision detection logic for ripple generation
                // Similar to foam generation, check if the current UV coordinate intersects with another object
                return false; // Placeholder, replace with actual logic
            }

            ENDCG
        }
    }
    FallBack "Transparent/Diffuse"
}
