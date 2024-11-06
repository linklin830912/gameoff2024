Shader "Unlit/NormalShader"
{
    Properties
    {
        _Color1("Front-Facing Color", Color) = (1, 0, 0, 1)
        _Color2("Side-Facing Color", Color) = (0, 1, 0, 1)
        _Color3("Edge-Facing Color", Color) = (0, 0, 1, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 worldNormal : TEXCOORD0;
                float3 worldViewDir : TEXCOORD1;
            };

            fixed4 _Color1;
            fixed4 _Color2;
            fixed4 _Color3;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);

                // Transform the normal and view direction to world space
                o.worldNormal = normalize(mul(v.normal, (float3x3)unity_ObjectToWorld));
                o.worldViewDir = normalize(_WorldSpaceCameraPos - mul(v.vertex, unity_ObjectToWorld).xyz);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Calculate the dot product between normal and view direction
                float dotNV = dot(i.worldNormal, i.worldViewDir);
                
                // Map the dot product from [-1, 1] to [0, 1] for easier interpolation
                float intensity = saturate((dotNV + 1) * 0.5);

                // Interpolate between colors based on intensity
                fixed3 color = lerp(_Color2.rgb, _Color3.rgb, intensity); // Edge colors
                color = lerp(_Color1.rgb, color, intensity); // Blend with front-facing color

                return fixed4(color, 1.0);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
