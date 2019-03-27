Shader "Custom/NLitCommonObject" {
	Properties {
		_Color ("Main Color", Color) = (.5,.5,.5,1)
		_MainTex ("Base (RGB)", 2D) = "white" { }
	}
	CGINCLUDE
		#include "UnityCG.cginc"
		#pragma keepalpha
		struct appdata {
			float4 vertex : POSITION;
			float3 normal : NORMAL;
			half4 texcoord : TEXCOORD0;
		};
		
		//顶点返回
		struct v2f {
			float4 pos : POSITION;
			fixed4 color : COLOR; //顶点颜色
			half2 uv : TEXCOORD0;
		};
		
		//uniform, applied by Program
		uniform fixed4 _Color;
		uniform sampler2D _MainTex;
		float4 _MainTex_ST;

		v2f vert(appdata v) {
			v2f o;
			o.pos = mul( UNITY_MATRIX_MVP, v.vertex);;
			o.color = _Color;
			o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
			return o;
		}

	ENDCG
	SubShader {
		Tags{ "Queue" = "Geometry" "RenderType" = "Opaque" }
		Pass {
			Lighting Off
			Cull Back

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			fixed4 frag(v2f i) :COLOR {
				fixed4 texCol = tex2D(_MainTex, i.uv);
				clip(texCol.a - 0.5);
				return texCol;
			}

			ENDCG
		}

	}
	Fallback "Diffuse"
}