Shader"Unlit/ToonShader"
{
    Properties
    {
        _Color("Color", COLOR) = (1,1,1,1)
        _AColor("Shadow Color", COLOR) = (0,0,0,1)
        _MainTex("Texture", 2D) = "white" {}
        _Brightness("Brightness", Range(0,1)) = 0.3
        _Strength("Strength", Range(0,1)) = 0.5
        _Detail("Detail", Range(0,1)) = 0.3
        _ShadowMap("ShadowMap", 2D) = "black" {}
    }
        SubShader
        {
            Tags { "RenderType" = "Opaque" }
            LOD 100

            Pass
            {

                Cull Back
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                // make fog work
                #pragma multi_compile_fog

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
                                UNITY_FOG_COORDS(1)
                    float4 vertex : SV_POSITION;
                    half3 worldNormal : NORMAL;
                };

                sampler2D _MainTex;
                float4 _MainTex_ST;
                float _Brightness;
                float _Strength;
                float4 _Color;
                float4 _AColor;
                float _Detail;
                sampler2D _shadowMap;

                float SampleShadowMap(sampler2D shadowMap, float3 lightDir)
                {
                    // Get the shadow map coordinates.
                    float2 shadowMapUV = lightDir.xy * 0.5 + 0.5;

                    // Sample the shadow map.
                    float shadow = tex2D(shadowMap, shadowMapUV);

                    // Return the shadow value.
                    return shadow;
                }

                float Toon(float3 normal, float3 lightDir)
                {
                    float NdotL = max(0.0, dot(normalize(normal), normalize(lightDir)));

                    float shadowTerm = 1 - SampleShadowMap(_shadowMap, lightDir);
    
                    return floor(NdotL / _Detail) * shadowTerm;
                    //return NdotL;
                }

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    o.worldNormal = UnityObjectToWorldNormal(v.normal);
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    // sample the texture
                    fixed4 col = tex2D(_MainTex, i.uv) * _Color;
                    float _brightness = Toon(i.worldNormal, _WorldSpaceLightPos0.xyz) * _Strength;
                    col *= _brightness * _Color + (1 - _brightness) * _AColor + _Brightness; // Add shadows
                    return col;
                }
                ENDCG
            }
            
            //Pass for Casting Shadows 
            Pass 
            {
                Name "CastShadow"
                Tags { "LightMode" = "ShadowCaster" }
    
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma multi_compile_shadowcaster
                #include "UnityCG.cginc"
    
                struct v2f 
                { 
                    V2F_SHADOW_CASTER;
                };
    
                v2f vert( appdata_base v )
                {
                    v2f o;
                    TRANSFER_SHADOW_CASTER(o)
                    return o;
                }
    
                float4 frag( v2f i ) : COLOR
                {
                    SHADOW_CASTER_FRAGMENT(i)
                }
                ENDCG
            }

        }
}