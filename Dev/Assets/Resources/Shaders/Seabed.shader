// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/Seabed"
{
    Properties
    {
        [NoScaleOffset]_mainTex ("Base (RGB)", 2D) = "white" {}
        _bumpMap ("Bump (RGB)", 2D) = "bump" {}
        _intensity ("Intensity", float) = 0.005
    }

    SubShader
    {
        Tags { "Queue" = "Background" "RenderType" = "Opaque"}
        
        Pass
        {
            Cull Back
			
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile CAMERA_ORTHOGONAL CAMERA_PERSPECTIVE
           
            #include "UnityCG.cginc"

            // vert输入数据
            struct appdata {
                float4 vertex : POSITION;   // 位置
                float3 normal : NORMAL;     // 法线
                float4 tangent : TANGENT;    // 切线
                float2 uv : TEXCOORD0;      // 纹理坐标
            };

            struct v2f {
                float4 pos : SV_POSITION;
                half2 uv : TEXCOORD0;
                float4 screenPos : TEXCOORD5;
            };

            // 属性数据
            sampler2D _mainTex;
            sampler2D _bumpMap;
            float _intensity;

            // 顶点函数
            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.screenPos = o.pos;
                return o;
            }

            // 像素函数
            fixed4 frag (v2f i) : SV_Target
            {
                // same as from previous shader...
                float2 uv = i.uv;
                uv.x += _Time.y*0.01;

                half3 tnormal = UnpackNormal(tex2D(_bumpMap, uv)); 
#ifdef CAMERA_ORTHOGONAL
                i.screenPos = float4( i.screenPos.xy , 0, 0 );
#else
                i.screenPos = float4( i.screenPos.xy / i.screenPos.w , 0, 0 ); /// 
#endif
                float2 sceneUVs = i.screenPos.xy*0.5+0.5 + (tnormal.rgb.rg * _intensity);
                sceneUVs = saturate(sceneUVs);
                return tex2D(_mainTex, sceneUVs);
            }
   
            ENDCG
        }
    }
}