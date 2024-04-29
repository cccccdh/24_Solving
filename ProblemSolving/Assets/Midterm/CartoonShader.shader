Shader "Unlit/CartoonShader"
{
    Properties
    {
        _DiffuseColor("DiffuseColor", Color) = (1, 1, 1, 1)
        _LightDirection("LightDirection", Vector) = (0,1,0,0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

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
                float4 vertex : SV_POSITION;
                float3 normal : NORMAL;
                float3 viewDir : TEXCOORD0;
            };

            float4 _DiffuseColor;
            float4 _LightDirection;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = normalize(UnityObjectToWorldNormal(v.normal));
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float3 lightDir = normalize(_LightDirection);
                float3 normal = normalize(i.normal);

                float diffuseIntensity = max(dot(normal, lightDir), 0);

                fixed4 cartoonColor;
                if (diffuseIntensity > 0.95)
                {
                    cartoonColor = fixed4(1, 1, 1, 1);
                }
                else if (diffuseIntensity > 0.5)
                {
                    cartoonColor = fixed4(0.8, 0.8, 0.8, 1);
                }
                else if (diffuseIntensity > 0.15)
                {
                    cartoonColor = fixed4(0.6, 0.6, 0.6, 1);
                }
                else
                {
                    cartoonColor = fixed4(0.4, 0.4, 0.4, 1);
                }
                fixed4 cartoon = cartoonColor * _DiffuseColor;

                return cartoon;
            }
            ENDCG
        }
    }
}
