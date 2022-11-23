Shader "Custom/PixelArtV2"
{
    Properties
    {
        [HideInInspector] _MainTex ("Texture", 2D) = "white" {}
        [HideInInspector] _RatioY("Ratio Y", float) = 1
        [HideInInspector] _RatioX("Ratio X", float) = 1
        
        [KeywordEnum(B8, B16, B32)] _Bits("Bits",float)=0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Transparent"}
        Blend SrcAlpha OneMinusSrcAlpha
        LOD 100

        Pass
        { 
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma shader_feature _BITS_B8 _BITS_B16 _BITS_B32
            #include "UnityCG.cginc"

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

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _RatioX;
            float _RatioY;
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed pixelSample = 0;
                #if _BITS_B8
                    pixelSample = 16;
                #elif _BITS_B16
                    pixelSample = 32;
                #else 
                    pixelSample = 64;
                #endif


                half bitX = _RatioX / pixelSample;
                half bitY = _RatioY / pixelSample;

                float2 uv = float2((int)(i.uv.x / bitX) * bitX, (int)(i.uv.y / bitY) * bitY);


                // sample the texture
                fixed4 col = tex2D(_MainTex, uv);
                return col;
            }
            ENDCG
        }
    }
}
