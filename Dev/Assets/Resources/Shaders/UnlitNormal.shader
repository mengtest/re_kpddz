// Unlit alpha-blended shader.
// - no lighting
// - no lightmap support
// - no per-material color

Shader "Custom/UnlitNormal" {
Properties {
	_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
}

SubShader {
	Tags {"Queue"="Geometry" "IgnoreProjector"="True" "RenderType"="Opaque"}
	LOD 100
	
	Pass {
		Lighting Off
		ZTest LEqual
		SetTexture [_MainTex] { combine texture } 
	}
}
}
