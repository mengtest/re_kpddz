Shader "Custom/NLitTextureColorAlpha" {
	Properties{
		_Color("Main Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_MainTex("Base (RGB)", 2D) = "white" { }
	}

	SubShader{

		Tags { "Queue" = "Transparent" "RenderType" = "Opaque"  }  //"Queue" = "Transparent"

		Pass{
			Cull Back
			ZWrite Off
			ZTest LEqual 
			Blend SrcAlpha OneMinusSrcAlpha  //OneMinusSrcAlpha // Normal

			SetTexture[_MainTex]
			{
				ConstantColor [_Color]
				Combine texture * constant, texture * constant
			}
		}
	}

	Fallback "Diffuse"
}
