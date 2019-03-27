// Unlit alpha-blended shader.
// - no lighting
// - no lightmap support
// - no per-material color

Shader "Custom/UnlitTransparentWithoutCull" {
Properties {
	_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
}

SubShader {
	Tags {"Queue"="Transparent-500" "IgnoreProjector"="True" "RenderType"="Transparent"}
	LOD 100
	
	Cull Off
	ZWrite Off
	ZTest LEqual
	Blend SrcAlpha OneMinusSrcAlpha 

	Pass {
		Lighting Off
		SetTexture [_MainTex] { combine texture } 
	}
}
}
