Shader "Custom/SimpleWater"
{
    Properties
    {
        _Color("Water Color", Color) = (0,0.5,1,1)
        _MainTex("Wave Texture", 2D) = "white" {}
        _WaveSpeed("Wave Speed", Float) = 0.5
        _WaveStrength("Wave Strength", Float) = 0.1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

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
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float _WaveSpeed;
            float _WaveStrength;

          v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                // Apply tiling/offset first
                float2 uv = TRANSFORM_TEX(v.uv, _MainTex);

                // Simple wave movement
                float wave = sin(v.vertex.x * 2 + _Time.x * _WaveSpeed) * _WaveStrength;
                o.uv = uv + wave;

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 tex = tex2D(_MainTex, i.uv);
                return tex * _Color;
            }
            ENDCG
        }
    }
}
