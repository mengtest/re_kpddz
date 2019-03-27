// Upgrade NOTE: commented out 'float4 unity_LightmapST', a built-in variable
// Upgrade NOTE: commented out 'sampler2D unity_Lightmap', a built-in variable
// Upgrade NOTE: replaced tex2D unity_Lightmap with UNITY_SAMPLE_TEX2D

Shader "Custom/CoverSemitransparent(ligtmap)" {
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
	_Color("Main Color", COLOR) = (1.0,1.0,1.0,1)
	}
		SubShader{
		Tags{ "Queue" = "Geometry+1" "RenderType" = "Opaque" }
		Blend SrcAlpha OneMinusSrcAlpha
		Lighting Off

		CGINCLUDE
#include "UnityCG.cginc"

		//vert -> frag
	struct v2f {
		float4 pos : SV_POSITION;
		half2 uv0 : TEXCOORD0;
		half2 uv1 : TEXCOORD1;
	};

	struct appdata_lightmap {
		float4 vertex : POSITION;
		half2 texcoord0 : TEXCOORD0;
		half2 texcoord1 : TEXCOORD1;
	};

	//光照贴图数据
#ifndef LIGHTMAP_OFF
	// sampler2D unity_Lightmap;
	// float4 unity_LightmapST;
#endif

	//贴图数据
	sampler2D _MainTex;
	float4 _MainTex_ST;
	fixed4 _Color;

	////////////////////////////////////////////////////////////////////////////////////////////////

	//顶点处理函数
	v2f vert(appdata_lightmap i)
	{
		v2f o;
		o.pos = mul(UNITY_MATRIX_MVP, i.vertex);
		o.uv0 = TRANSFORM_TEX(i.texcoord0, _MainTex);

		//计算光照贴图UV
#ifndef LIGHTMAP_OFF
		o.uv1 = i.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
#endif
		return o;
	}

	//片段处理
	fixed4 frag(v2f i) : COLOR
	{
		fixed4 color = tex2D(_MainTex, i.uv0);
		color *= _Color;

		//读取光照贴图数据
#ifndef LIGHTMAP_OFF
		fixed4 colorLightingMap = UNITY_SAMPLE_TEX2D(unity_Lightmap, i.uv1);
		color.rgb *= DecodeLightmap(colorLightingMap);
#endif
		return color;
	}
		ENDCG

		Pass {
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
			ENDCG
	}

	}
		//FallBack "Diffuse"
}
