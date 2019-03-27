
Shader "Mobile/Transparent/Additive/Diffuse-SR" {
Properties {
	_MainTex ("Texture (RGB)", 2D) = "white" {}
	_ScrollX ("Scroll speed X", Float) = 1.0
	_ScrollY ("Scroll speed Y", Float) = 0.0
	_RotationStart("Rotation Start", Float) = 0
	_RotationX("Rotation center Y", Float) = 0.5	
	_RotationY("Rotation center X", Float) = 0.5
	_Rotation ("Rotation speed", Float) = 1.0
	_TintColor ("Main Color", Color) = (1,1,1,1)
	_MMultiplier ("Multiplier", Float) = 2.0	
}

	
SubShader {
	Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
	
	Blend SrcAlpha One
	Cull Off Lighting Off ZWrite Off Fog { Color (0,0,0,0) }
	LOD 100
		
	CGINCLUDE
	#include "UnityCG.cginc"
	sampler2D _MainTex;

	float4 _MainTex_ST;
	
	float _ScrollX;
	float _ScrollY;
	float _RotationStart;
	float _RotationX;
	float _RotationY;
	float _Rotation;
	float4 _TintColor;
	float _MMultiplier;
	
	struct v2f {
		float4 pos : SV_POSITION;
		float2 uv : TEXCOORD0;
		fixed4 color : TEXCOORD1;
	};

	float2 CalcUV(float2 uv, float tx, float ty, float rstart, float rx, float ry, float r)
	{
		float s = sin(r*_Time+rstart);
		float c = cos(r*_Time+rstart);
		
		float2x2 m = {c, -s, s, c};
		
		uv -= float2(rx, ry);
		uv = mul(uv, m);
		uv += float2(rx, ry);
		
		uv += frac(float2(tx, ty) * _Time);
		
		return uv;
	}
	
	v2f vert (appdata_full v)
	{
		v2f o;
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
		o.uv.xy = TRANSFORM_TEX(v.texcoord.xy,_MainTex);
		o.uv.xy = CalcUV(o.uv.xy, _ScrollX, _ScrollY, _RotationStart, _RotationX, _RotationY, _Rotation);
		o.color = _MMultiplier * _TintColor * v.color;
		
		return o;
	}
	ENDCG


	Pass {
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
//		#pragma fragmentoption ARB_precision_hint_fastest		
		fixed4 frag (v2f i) : COLOR
		{
			return tex2D (_MainTex, i.uv.xy)* i.color;
		}
		ENDCG 
	}	
}
}
