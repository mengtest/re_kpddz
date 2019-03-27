Shader "Unlit/GaussBlur"
{
	Properties
	{
		_MainTex ("_uiBackgroundTexture", 2D) = "white"{}
		_BlurRadius ("_BlurRadius", Range(1,15) ) = 6.0
		_Sigma ("Standard deviation", float) = 3.0
	}
	SubShader
	{
		Tags { "Queue"="Transparent" }

		// Grab the screen behind the object into _BackgroundTexture
		/*Grabpass
		{
			"_uiBackgroundTexture"
		}*/

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
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _Sigma;
			float _BlurRadius;
			
			// 高斯分布函数
			float GetGaussianDistribution( float x, float y) 
			{
				float g = 1.0f / sqrt( 2.0f * 3.141592654f * _Sigma * _Sigma );
				return g * exp( -(x * x + y * y) / (2 * _Sigma * _Sigma) );
			}
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv; //TRANSFORM_TEX(v.uv, _uiBackgroundTexture);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float weightTotal = 0;
				for (int x=-_BlurRadius; x<=_BlurRadius; x++)
			    {
			        for(int y=-_BlurRadius; y<=_BlurRadius; y++)
			        {
			            weightTotal += GetGaussianDistribution(x/640.0, y/640.0);
			        }
			    }

				float4 col = float4(0.0, 0.0, 0.0, 0.0);
			    for (int xx=-_BlurRadius; xx<=_BlurRadius; xx++)
			    {
			        for (int yy=-_BlurRadius; yy<=_BlurRadius; yy++)
			        {
			            float weight = GetGaussianDistribution(xx/ 640.0, yy/640.0) / weightTotal;
					
			            float4 color = tex2D(_MainTex, i.uv + float2(xx/ 640.0, yy/640.0));
			            color = color * weight;
			            col += color;
			        }
			    }
				
			    return col; //tex2D(_uiBackgroundTexture, i.uv);
			}
			ENDCG
		}
	}
}
