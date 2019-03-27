Shader "Custom/BumpColorSpec_normal" {
	Properties{
		_MainTint("Diffuse Tint", Color) = (1,1,1,1)
		_MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
		_NormalIntensity("Normal Map Intensity", Range(0,10)) = 1
		_NormalMap ("Normal Map (RGB)", 2D) = "bump" {}  //bump
		_SpecPower("Specular Power", Range(0.1, 120)) = 3
		_SpecTint("Specular Tint", Color) = (1,1,1,1)
		_SpecMap ("Spec map (RGB)", 2D) = "white" {}
		_Gloss ("Gloss", Range(0.1, 120)) = 3
	}

	SubShader {
		//Tags { "RenderType"="Opaque" }
		//LOD 200
		
		CGPROGRAM
		//使用自定义光照模型
		#pragma surface surf CustomPhong

		sampler2D _MainTex;
		sampler2D _NormalMap;
		sampler2D _SpecMap;
		float _NormalIntensity;
		float4 _MainTint;
		float _SpecPower;
		float4 _SpecTint;
		float _Gloss;

		//输出
		struct SurfaceCustomOutput
		{
			fixed3 Albedo;
			fixed3 Normal;
			fixed3 Emission;
			fixed3 SpecularColor;
			half Specular;
			fixed Gloss;
			fixed Alpha;
		};
		
		//输入
		struct Input {
			float2 uv_MainTex;
			float2 uv_NormalTex;
			float2 uv_SpecularMask;
		};

		//自定义光照模型
		inline fixed4 LightingCustomPhong(SurfaceCustomOutput s, fixed3 lightDir, half3 viewDir, fixed atten)
		{
			//计算漫反射和反射向量
			float diff = dot(s.Normal, lightDir);
			float3 reflectionVector = normalize(2.0f * s.Normal * diff - lightDir);

			//计算Phong向量
			float spec = pow(max(0.0f, dot(reflectionVector, viewDir)), _SpecPower) * s.Specular;
			float3 finalSpec = s.SpecularColor * spec * _SpecTint.rgb;

			//最终颜色
			fixed4 c;
			c.rgb = (s.Albedo * _LightColor0.rgb * diff) + (_LightColor0.rgb * finalSpec) * (atten * 2);
			c.rgb = _MainTint.rgb * c.rgb;
			c.a = saturate(s.Alpha + _LightColor0.a * _SpecColor.a * spec * atten);;

			return c;
		}

		inline fixed4 LightingBlinnPhongHardsurfaceFront(SurfaceCustomOutput s, fixed3 lightDir, fixed3 viewDir, fixed atten)
		{
			fixed3 h = normalize(lightDir + viewDir);

			fixed diff = dot(s.Normal, lightDir);
			fixed difftrans = abs(diff) * 1 - s.Alpha;
			diff = max(diff, difftrans);

			half nh = dot(s.Normal, h);
			nh = max(0, nh);

			half spec = pow(nh, s.Specular*128.0) * s.Gloss * _Gloss;

			fixed4 c;
			c.rgb = (s.Albedo * _LightColor0.rgb * diff + _LightColor0.rgb * _SpecColor.rgb * spec) * (atten * 2);
			c.a = saturate(s.Alpha + _LightColor0.a * _SpecColor.a * spec * atten);
			return c;
		}

		//主函数
		void surf (Input IN, inout SurfaceCustomOutput o) {
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			fixed3 normalMap = UnpackNormal(tex2D(_NormalMap, IN.uv_NormalTex));
			normalMap = fixed3(normalMap.x*_NormalIntensity, normalMap.y*_NormalIntensity, normalMap.z*_NormalIntensity);
			float4 specMask = tex2D(_SpecMap, IN.uv_SpecularMask);

			//赋值
			o.Albedo = c.rgb; 
			o.Normal = normalMap.rgb;
			o.Specular = specMask.g;
			o.SpecularColor = specMask.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
 