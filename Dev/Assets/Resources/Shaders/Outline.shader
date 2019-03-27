Shader "Custom/Outline" {
	Properties {
		_Color ("Main Color", Color) = (.5,.5,.5,1)
		_OutlineColor ("Outline Color", Color) = (0,0,0,1)
		_Outline ("Outline width", Range (0.0, 0.03)) = .005
		_MainTex ("Base (RGB)", 2D) = "white" { }
	}
	CGINCLUDE
		#include "UnityCG.cginc"
		struct appdata {
			float4 vertex : POSITION;
			float3 normal : NORMAL;
		};

		//顶点返回
		struct v2f {
			float4 pos : POSITION;
		};
		
		uniform fixed4 _Color;
		uniform fixed _Outline;
		uniform fixed4 _OutlineColor;

		//顶点处理
		v2f vert(appdata v) {
			v2f o;
			o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
			float3 norm   = mul ((float3x3)UNITY_MATRIX_IT_MV, v.normal);
			float2 offset = TransformViewToProjection(norm.xy);
			o.pos.xy += offset * o.pos.z * _Outline;
			return o;
		}

		//片段处理
		fixed4 frag(v2f i) :COLOR{
			_OutlineColor.a = _Color.a;
			return _OutlineColor;
		}
	ENDCG

	SubShader {
		Tags{ "Queue" = "Geometry" "RenderType" = "Opaque" }

		Pass {
			Name "OUTLINEBASE"
			Cull Front
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha // Normal

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			ENDCG
		}
		Pass {
			Name "BASE"
			Cull Back
			ZWrite On
			ZTest LEqual
			Blend SrcAlpha OneMinusSrcAlpha
			
			SetTexture[_MainTex]{
				ConstantColor[_Color]
				Combine texture * constant, texture * constant
			}
		}
	}
	Fallback "Diffuse"
}