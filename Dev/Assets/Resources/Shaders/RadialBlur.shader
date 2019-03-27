//径向模糊后处理
Shader "Hidden/RadialBlur" {
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_fSampleDist("SampleDist", Float) = 1 //采样距离
		_fSampleStrength("SampleStrength", Float) = 2.2 //采样力度
		_fSampleCenterX("SampleCenterX", Float) = 0.5  //采样中心点X
		_fSampleCenterY("SampleCenterY", Float) = 0.5  //采样中心点Y
	}
	SubShader{
		ZWrite Off
		Blend Off
		Cull Off
		ZTest Off

		Pass
		{
			ZTest Always Cull Off ZWrite Off
			Fog{ Mode off }
			
			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata_t {
				float4 vertex : POSITION;
				half2 texcoord : TEXCOORD;
			};

			struct v2f {
				float4 vertex : POSITION;
				half2 texcoord : TEXCOORD;
			};

			float4 _MainTex_ST;

			v2f vert(appdata_t v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
				return o;
			}

			sampler2D _MainTex;
			float _fSampleDist;
			float _fSampleStrength;
			float _fSampleCenterX;
			float _fSampleCenterY;

			// some sample positions  
			static const float samples[6] =
			{
				-0.05,
				-0.03,
				-0.01,
				0.01,
				0.03,
				0.05,
			};

			fixed4 frag(v2f i) : SV_Target
			{

				//0.5,0.5屏幕中心
				half2 dir = half2(_fSampleCenterX, _fSampleCenterY) - i.texcoord;//从采样中心到uv的方向向量
				half2 texcoord = i.texcoord;
				half dist = length(dir);
				dir = normalize(dir);
				fixed4 color = tex2D(_MainTex, texcoord);

				fixed4 sum = 0.143*color;
				//    6次采样
				for (int i = 0; i < 6; ++i)
				{

					sum += 0.143*tex2D(_MainTex, texcoord + dir * samples[i] * _fSampleDist);
				}

				//求均值
				//sum /= 7.0f;


				//越离采样中心近的地方，越不模糊
				fixed t = saturate(dist * _fSampleStrength);

				//插值
				return lerp(color, sum, t);

			}

			ENDCG
		} //Pass
	} //SubShader
		
		Fallback off
}
