Shader "Hex3D/Particles/Glow/AddFog" 
{
	Properties 
	{
		_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
		_MainTex ("Particle Texture", 2D) = "white" {}
		_AddIntensity ("Add Intensity", Range(0, 5)) = 2.0
		
		_MKGlowColor ("Glow Color", Color) = (1,1,1,1)
		_MKGlowPower ("Glow Power", Range(0.0,5.0)) = 2.5
		_MKGlowOffSet ("Glow Width ", Range(0.0,0.15)) = 0.0
		_MKGlowTex ("Glow Texture", 2D) = "black" {}
		_MKGlowTexColor ("Glow Texture Color", Color) = (1,1,1,1)
		_MKGlowTexStrength ("Glow Texture Strength ", Range(0.0,5.0)) = 1.0
	}

	Category 
	{
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="MKGlow" }
		Blend SrcAlpha One
		AlphaTest Greater .01
		ColorMask RGB
		Cull Off Lighting Off ZWrite Off Fog { Color (0,0,0,0) }
		
		SubShader 
		{
			Pass 
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile_particles
				#pragma multi_compile_fog

				#include "UnityCG.cginc"

				sampler2D _MainTex;
				fixed4 _TintColor;
				float _AddIntensity;
				
				sampler2D _MKGlowTex;
				half _MKGlowTexStrength;
				fixed4 _MKGlowTexColor;
				
				struct appdata_t 
				{
					float4 vertex : POSITION;
					fixed4 color : COLOR;
					float2 texcoord : TEXCOORD0;
				};

				struct v2f 
				{
					float4 vertex : SV_POSITION;
					fixed4 color : COLOR;
					float2 texcoord : TEXCOORD0;
					UNITY_FOG_COORDS(1)
				};
				
				float4 _MainTex_ST;

				v2f vert (appdata_t v)
				{
					v2f o;
					o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
					o.color = v.color;
					o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
					UNITY_TRANSFER_FOG(o,o.vertex);
					return o;
				}
				
				fixed4 frag (v2f i) : SV_Target
				{
					fixed4 col = tex2D(_MainTex, i.texcoord) * i.color * _AddIntensity * _TintColor;
					fixed4 d = tex2D(_MKGlowTex, i.texcoord) * _MKGlowTexColor;
		 			col += (d * _MKGlowTexStrength);
		 			
					UNITY_APPLY_FOG_COLOR(i.fogCoord, col, fixed4(0,0,0,0));
					return col;
				}
				ENDCG 
			}
		}	
	}
}
