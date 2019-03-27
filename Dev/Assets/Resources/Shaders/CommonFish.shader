Shader "Custom/CommonFish"
{
    Properties
    {
        [NoScaleOffset]_mainTex ("Base (RGB)", 2D) = "white" {}
        [NoScaleOffset]_causticTex ("Caustic", 2D) = "white" {} 
        _intencity ("Intencity", float) = 1.0
        _scrollSpeedX ("SpeedX", float) = 1.0
        _scrollSpeedY ("SpeedY", float) = 1.0
    }

    SubShader
    {
        Tags { "Queue" = "Geometry" "RenderType" = "Opaque"}
        
        Pass
        {
            Name "Fish"
            Cull Back
			
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile USING_CAUSTIC_OFF USING_CAUSTIC
           
            #include "UnityCG.cginc"

            // vert输入数据
            struct appdata {
                float4 vertex : POSITION;   // 位置
                float3 normal : NORMAL;     // 法线
                float2 uv : TEXCOORD0;      // 纹理坐标
            };

            struct v2f {
                float4 pos : SV_POSITION;
                half2 uvMain : TEXCOORD0;
                half causticInfo: NORMAL;
            };

            // 属性数据
            sampler2D _mainTex;

#ifdef USING_CAUSTIC
            sampler2D _causticTex;
            float _intencity;
            float _scrollSpeedX;
            float _scrollSpeedY;
#endif

            // 顶点函数
            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uvMain = v.uv;
                o.causticInfo = 0.0;

#ifdef USING_CAUSTIC
                half3 wNormal = normalize(UnityObjectToWorldNormal(v.normal));
                half3 wViewDir = normalize (WorldSpaceViewDir(v.vertex));
				half3 waterHorizNormal = half3(0.0, 1.0, 0.0);
				half angle2Water = dot(wNormal, waterHorizNormal);
                o.causticInfo = saturate(angle2Water);
#endif

                return o;
            }

            // 像素函数
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_mainTex, i.uvMain);

#ifdef USING_CAUSTIC 
                fixed scrollX = _scrollSpeedX * _Time.y;
                fixed scrollY = _scrollSpeedY * _Time.y;
                fixed2 scrollUV = i.uvMain;
                scrollUV += fixed2(0, scrollY); 
                fixed4 cstCol = tex2D(_causticTex, scrollUV);
                cstCol = i.causticInfo * _intencity * cstCol;
                col += cstCol;
#endif

                return col;
            }
   
            ENDCG
        }
    }
}