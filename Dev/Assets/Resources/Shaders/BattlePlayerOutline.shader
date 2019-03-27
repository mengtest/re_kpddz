Shader "Custom/BattlePlayerOutline" {
	Properties {
		_Color ("Main Color", Color) = (.5,.5,.5,1)
		_MainTex ("Base (RGB)", 2D) = "white" { }
		_Outline("Outline width", Range(0.0, 0.03)) = .005
	}
	CGINCLUDE
		#include "UnityCG.cginc"
		
		struct appdata {
			float4 vertex : POSITION;
			float3 normal : NORMAL;
			half4 texcoord : TEXCOORD0;
		};
		
		//顶点返回
		struct v2f {
			float4 pos : POSITION;
			half2 uv : TEXCOORD0;
		};
		
		//uniform, applied by Program
		uniform fixed4 _Color;
		uniform sampler2D _MainTex;
		uniform float _Outline;
		float4 _MainTex_ST;

		//描边部分
		v2f vert_outline(appdata v) {
			v2f o;
			o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
			float3 norm = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);
			float2 offset = TransformViewToProjection(norm.xy);
			o.pos.xy += offset * o.pos.z * _Outline;
			return o;
		}

		//遮挡描边
		fixed4 frag_outline_cover(v2f i) :COLOR{
			return fixed4(0.0, 1.0, 1.0, 0.5);
		}
		
		//正常描边
		fixed4 frag_outline_normal(v2f i) : COLOR{
			return fixed4(0.0, 0.0, 0.0, 1.0);
		}
	ENDCG

	SubShader {
		//遮挡描边
		Pass {
			Cull Front
			ZWrite Off
			ZTest GEqual
			Blend SrcAlpha OneMinusSrcAlpha // Normal
			
			CGPROGRAM
			#pragma vertex vert_outline
			#pragma fragment frag_outline_cover
			ENDCG
		}
		
		//遮挡中间像素
		Pass{
			Cull Back
			ZWrite Off
			ZTest GEqual
			AlphaTest Off
			Blend SrcAlpha SrcAlpha // Normal
			Color (0.0, 1, 1, 0.0)
		}
		
		//正常描边
		Pass{
			Cull Back
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha // Normal
			CGPROGRAM
			#pragma vertex vert_outline
			#pragma fragment frag_outline_normal
			ENDCG
		}
		
		//正常纹理绘制
		Pass {
			Cull Back
			ZWrite On
			ZTest LEqual
			Blend SrcAlpha OneMinusSrcAlpha // Normal

			SetTexture[_MainTex]{
				ConstantColor[_Color]
				Combine texture * constant
			}
		}
	}
	Fallback "Diffuse"
}