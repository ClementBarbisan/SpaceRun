Shader "Lit/Explose"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color("Color", Color) = (1, 1, 1, 1)
        _Value("Value", Float) = 0
    }
    SubShader
    {
        
        Pass
        {
            Tags {"LightMode"="ForwardBase"}
        
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma geometry geom
            #pragma multi_compile_instancing
            #include "UnityCG.cginc"
            #include "UnityLightingCommon.cginc"
            struct appdata
             {
                 float4 vertex   : POSITION;  // The vertex position in model space.
                 float3 normal   : NORMAL;    // The vertex normal in model space.
                 float4 texcoord : TEXCOORD0; // The first UV coordinate.
             };
            struct g2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                fixed4 diff : COLOR0;
                UNITY_VERTEX_OUTPUT_STEREO
            };
            struct v2g
            {
                float2 uv : TEXCOORD0;
                fixed4 diff : COLOR0;
                float4 vertex : SV_POSITION;
                float3 normal : NORMAL;
                UNITY_VERTEX_INPUT_INSTANCE_ID 
            };
            
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Value;
            v2g vert (appdata v)
            {
                v2g o;
               
                o.vertex = v.vertex;
                o.uv = v.texcoord;
               
                half3 worldNormal = UnityObjectToWorldNormal(v.normal);
                half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
                o.diff = nl * _LightColor0;
                o.normal = worldNormal;
                o.diff.rgb += ShadeSH9(half4(worldNormal,1));
                
                return o;
            }

             [maxvertexcount(3)]
            void geom(triangle v2g IN[3], inout TriangleStream<g2f> triStream)
            {
                g2f o;
    UNITY_SETUP_INSTANCE_ID(IN[0]); //Insert
                UNITY_INITIALIZE_OUTPUT(g2f, o); //Insert
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o); //Insert
                for(int i = 0; i < 3; i++)
                {
                    o.vertex = UnityObjectToClipPos(IN[i].vertex + float4(IN[i].normal, 0.0) * _Value);
                    o.vertex += o.vertex * _Value;
                    UNITY_TRANSFER_FOG(o,o.vertex);
                    o.uv = TRANSFORM_TEX(IN[i].uv, _MainTex);
                    o.diff = IN[i].diff;
                    triStream.Append(o);
                }
 
                triStream.RestartStrip();
            }
           

            fixed4 frag (g2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                col *= i.diff;
                return col;
            }
            ENDCG
        }
    }
}