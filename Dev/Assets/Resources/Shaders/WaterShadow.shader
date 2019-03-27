Shader "Custom/WaterShadow"{
	Properties {
		_Color("Main Color",Color)=(1,1,1,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Cutoff("Base Alpha cutoff",Range(0,0.9))=0.2
	}
	SubShader 
	{
		Tags { Queue=Transparent }
		LOD 200

		Lighting Off
		ZTest Off
		//ZWrite Off
		Cull off
		Blend SrcAlpha OneMinusSrcAlpha//One Zero  //SrcAlpha OneMinusSrcAlpha
		LOD 5

		Pass
		{
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#include "UnityCG.cginc"

		struct appdata_t
		{
			float4 vertex:POSITION;
			float4 color:COLOR;
			float2 texcoord:TEXCOORD;
		};

		struct v2f
		{
			float4 vertex:SV_POSITION;
			float4 color:COLOR;
			float2 texcoord:TEXCOORD;
		};
		sampler2D _MainTex;
		float4 _MainTex_ST;
		float _Cutoff;

		v2f vert(appdata_t v)
		{
			v2f o;
			o.vertex = mul(UNITY_MATRIX_MVP,v.vertex);
			o.color=v.color;
			o.texcoord=TRANSFORM_TEX(v.texcoord,_MainTex);
			return o;
		}

		float4 _Color;
		half4 frag(v2f i):SV_Target
		{
			half4 col = tex2D(_MainTex,i.texcoord);
			if(col.a<_Cutoff)
			{
				clip(col.a-_Cutoff);
			}
			else
			{
				col.rgb=col.rgb*float3(0,0,0);
				col.rgb=col.rgb+_Color;
				col.a=_Color.a;
			}
			return col;
		}
		ENDCG
		}
	} 
	FallBack "Diffuse"
}