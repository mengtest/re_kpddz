//暂用，贴花效果目前有些问题，后面优化 @WP.Chu
Shader "Custom/SkillRangeDecal" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Color("Main Color", COLOR) = (1.0,1.0,1.0,1)
	}

	CGINCLUDE
		#include "UnityCG.cginc"
		
		struct appdata {
			float4 vertex : POSITION;
			half2 texcoord : TEXCOORD0;
		};

		//返回
		struct v2f {
			float4 pos : SV_POSITION;
			half2 uv : TEXCOORD0;
			
		};

		uniform sampler2D _MainTex;
		float4 _MainTex_ST;
		fixed4 _Color;

		v2f vert(appdata v) {
			v2f o;
			o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
			o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);  
			return o;
		}

		fixed4 frag(v2f i) : COLOR
		{
			fixed4 texColor = tex2D(_MainTex, i.uv);
			texColor *= _Color;
			return texColor;
		}

	ENDCG

	SubShader {
		//Tags { "Queue" = "Geometry-1" }
		
		Pass{
			ZWrite Off
			ZTest Off
			AlphaTest Off  //AlphaTest LEqual[_Cutoff]
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			ENDCG
		}
		
		
	} 
	FallBack "Diffuse"
}
