
Shader "Custom/ImprovedWater"
{
    Properties
    {
        _Color("Water Color", Color) = (0.2, 0.4, 0.6, 1)

        _MainTex("Wave Texture 1", 2D) = "bump" {}
        _WaveScale1("Wave Scale 1", Range(0.1, 5.0)) = 1.0
        _WaveStrength1("Wave Strength 1", Range(0.001, 0.1)) = 0.01
        _WaveSpeed1("Wave Speed 1 (XY)", Vector) = (0.01, 0.02, 0, 0)

        _SecondTex("Wave Texture 2 (Different Scale/Speed)", 2D) = "bump" {}
        _WaveScale2("Wave Scale 2", Range(0.1, 5.0)) = 0.5
        _WaveStrength2("Wave Strength 2", Range(0.001, 0.1)) = 0.01
        _WaveSpeed2("Wave Speed 2 (XY)", Vector) = (-0.02, 0.015, 0, 0)

        _Shininess("Shininess", Range(0.01, 3)) = 0.3
        _SpecularColor("Specular Color", Color) = (1,1,1,1)

        _ShallowColor("Shallow Color", Color) = (0.2, 0.8, 1, 1)
        _DeepColor("Deep Color", Color) = (0.0, 0.3, 0.5, 1)
        _DepthFactor("Depth Factor", Range(0.1, 5.0)) = 1.0
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 200

        Blend SrcAlpha OneMinusSrcAlpha
        Cull Back
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "UnityLightingCommon.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
                float3 worldNormal : TEXCOORD2;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _SecondTex;
            float4 _Color;
            float _WaveScale1;
            float _WaveStrength1;
            float2 _WaveSpeed1;
            float _WaveScale2;
            float _WaveStrength2;
            float2 _WaveSpeed2;
            float _Shininess;
            float4 _SpecularColor;
            float4 _ShallowColor;
            float4 _DeepColor;
            float _DepthFactor;

            float calculateDisplacement(float2 uv, sampler2D tex, float scale, float2 speed, float strength)
            {
                float2 scrolledUV = uv * scale + _Time.x * speed;
                float displacement = tex2Dlod(tex, float4(scrolledUV, 0, 0)).r;
                return (displacement - 0.5) * strength;
            }

            v2f vert(appdata v)
            {
                v2f o;

                float2 uv1 = v.uv * _WaveScale1;
                float2 uv2 = v.uv * _WaveScale2;

                float disp1 = calculateDisplacement(uv1, _MainTex, 1, _WaveSpeed1, _WaveStrength1);
                float disp2 = calculateDisplacement(uv2, _SecondTex, 1, _WaveSpeed2, _WaveStrength2);
                float totalDisp = disp1 + disp2;

                v.vertex.xyz += v.normal * totalDisp;

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.worldNormal = normalize(UnityObjectToWorldNormal(v.normal));
                o.uv = v.uv;

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Combine animated normals from wave textures
                float3 waveNormal = UnpackNormal(tex2D(_MainTex, i.uv * 2 + _Time.x * _WaveSpeed1));
                float3 waveNormal2 = UnpackNormal(tex2D(_SecondTex, i.uv * 3 - _Time.x * _WaveSpeed2));
                float3 combinedNormal = normalize(i.worldNormal + (waveNormal + waveNormal2) * 0.3);

                // Basic lighting
                float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
                float3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPos);
                float3 halfDir = normalize(lightDir + viewDir);

                float NdotL = max(0, dot(combinedNormal, lightDir));
                float NdotH = max(0, dot(combinedNormal, halfDir));
                float spec = pow(NdotH, 100 * _Shininess);

                float3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz;
                float3 color = (_Color.rgb * (ambient + NdotL)) + (_SpecularColor.rgb * spec);

                // Depth-based color tint
                float depth = saturate((i.worldPos.y + 1.0) * _DepthFactor);
                float3 depthColor = lerp(_DeepColor.rgb, _ShallowColor.rgb, depth);
                color = lerp(color, depthColor, 0.5);

                return fixed4(color, _Color.a);
            }
            ENDCG
        }
    }
}
