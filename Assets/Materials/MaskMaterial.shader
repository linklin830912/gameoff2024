Shader "Custom/GranularNoise"
{
    Properties
    {
        _MainColor ("Main Color", Color) = (1, 1, 1, 1)
        _NoiseScale ("Noise Scale", Float) = 50.0
        _Radius ("Radius", Float) = 1.0
        _DarknessStrength ("Darkness Strength", Float) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200
        Blend SrcAlpha OneMinusSrcAlpha
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
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
                float3 worldNormal : TEXCOORD1;
                float3 worldViewDir : TEXCOORD2;
            };

            float4 _MainColor;
            float _NoiseScale;
            float _Radius;
            float _DarknessStrength;

            // Vertex function
            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv * _NoiseScale;

                // Transform the normal and view direction to world space
                o.worldNormal = normalize(mul(v.normal, (float3x3)unity_ObjectToWorld));
                o.worldViewDir = normalize(_WorldSpaceCameraPos - mul(v.vertex, unity_ObjectToWorld).xyz);

                return o;
            }

            // Simple pseudo-random noise function
            float random(float2 uv)
            {
                return frac(sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453);
            }

            // Fragment function to apply the noise effect
            float4 frag(v2f i) : SV_Target
            {
                // Generate noise value based on UV coordinates
                float noiseValue = random(i.uv);

                // Apply noise value to the main color for a granular effect
                float4 color = _MainColor ;
                float2 center = float2(0.5, 0.5);
                float distance = length(i.uv - center);
                float darknessFactor = smoothstep(_Radius, 0.0, distance);

                // Calculate the dot product between normal and view direction
                float dotNV = dot(i.worldNormal, i.worldViewDir);
                
                // Map the dot product from [-1, 1] to [0, 1] for easier interpolation
                float intensity = saturate((dotNV + 1) * 0.5);


                return fixed4(color.xyz,  intensity*intensity*intensity*noiseValue);
                
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
