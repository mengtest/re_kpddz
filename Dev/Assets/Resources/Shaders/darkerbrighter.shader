Shader "wii/darkerbrighter"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_shadowAlpha("Shadow alpha", Float) = 0.5
		_modelAlpha("Model alpha", Float) = 1.0
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" "Queue"="Transparent"}
		LOD 100
		Cull off
		Blend SrcAlpha OneMinusSrcAlpha

		//阴影
		Pass
		{
			Stencil{
				Ref 1
				Comp Greater
				Pass IncrSat
				Fail Keep
			}

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			float4x4 _World2Ground;
			float4x4 _Ground2World;
			float3 _litDir;
			float _shadowAlpha;

			float4 vert(float4 vertex: POSITION) : SV_POSITION
			{
				// 将光照方向和顶点变换为投影面的坐标系内进行计算
				//float3 litDir = _litDir.xyz;
				float3 litDir = float3(0.4, -0.5, 0.1); // WorldSpaceLightDir(vertex); //_litDir.xyz;
				litDir = mul(_World2Ground,float4(litDir,0)).xyz;
				litDir = normalize(litDir);
				float4 vt;
				vt = mul(unity_ObjectToWorld, vertex);
				vt = mul(_World2Ground,vt);
				//vt.y = 0.01;

				// 计算投影平面上的坐标
				vt.x = vt.x - (vt.y / litDir.y)*litDir.x;
				vt.z = vt.z - (vt.y / litDir.y)*litDir.z;
				vt.y = 0.02;

				// 最后变换为投影空间坐标
				vt = mul(_Ground2World,vt);
				vt = mul(unity_WorldToObject,vt);
				return mul(UNITY_MATRIX_MVP, vt);
			}

			float4 frag(void) : COLOR
			{
				return float4(0.0, 0.0, 0.0, _shadowAlpha);
			}

			ENDCG
		}  // end pas  阴影



		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _modelAlpha;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float4 c0 = tex2D(_MainTex, i.uv).rgba;
				float4 c1 = tex2D(_MainTex, i.uv - float2(1, 0)).rgba;
				float4 c2 = tex2D(_MainTex, i.uv - float2(0, 1)).rgba;
				float4 c3 = tex2D(_MainTex, i.uv + float2(1, 0)).rgba;
				float4 c4 = tex2D(_MainTex, i.uv + float2(0, 1)).rgba;

				float red = c0.r;
				float blue = c0.b;
				float green = c0.g;

				float red2 = (c1.r + c2.r + c3.r + c4.r) / 4;
				float blue2 = (c1.b + c2.b + c3.b + c4.b) / 4;
				float green2 = (c1.g + c2.g + c3.g + c4.g) / 4;

				if (red2 > 0.3)
					red = c0.r + c0.r / 2;
				else
					red = c0.r - c0.r / 2;

				if (green2 >  0.3)
					green = c0.g + c0.g / 2;
				else
					green = c0.g - c0.g / 2;


				if (blue2  >  0.3)
					blue = c0.b + c0.b / 2;
				else
					blue = c0.b - c0.b / 2;

				float4 ocol0 = float4(red, green, blue, c0.a*_modelAlpha); //

				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, ocol0);
				return ocol0;
			}
			ENDCG
		}
	}
}
