Shader "Custom/Medusa"
{
    Properties
    {
        [NoScaleOffset]_mainTex ("Base (RGB)", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "Queue" = "Transparent"}
        
        CGINCLUDE

        #include "UnityCG.cginc"
    
        // vert输入数据
        struct appdata {
            float4 vertex : POSITION;   // 位置
            float4 color : COLOR;     // 法线
            float2 uv : TEXCOORD0;      // 纹理坐标
        };

        struct v2f {
            float4 pos : SV_POSITION;
            half2 uv : TEXCOORD0;
            fixed4 color : COLOR;
        };

        // 属性数据
        sampler2D _mainTex;

        // 顶点函数
        v2f vert (appdata v)
        {
            v2f o;
            o.pos = UnityObjectToClipPos(v.vertex);
            o.uv = v.uv;
            o.color = v.color;

            return o;
        }

        // 像素函数
        fixed4 frag (v2f i) : SV_Target
        {
            fixed4 col = tex2D(_mainTex, i.uv);
            col = fixed4(col.xyz, i.color.w);
            return col;
        }

        ENDCG

        Pass
        {
            Cull Off
			ZWrite On
			ZTest Always 
			
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            ENDCG
        }

        Pass
        {
            Cull Off
			ZWrite On
			ZTest LEqual 
			Blend SrcAlpha OneMinusSrcAlpha
			
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            ENDCG
        }
    }
}