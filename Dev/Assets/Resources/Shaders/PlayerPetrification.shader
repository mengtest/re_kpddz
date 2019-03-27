Shader "Custom/PlayerPetrification" {
	Properties {
		_Color ("Main Color", Color) = (.5,.5,.5,1)
		_MainTex ("Base (RGB)", 2D) = "white" { }
		_AlphaPercentage ("Alpha Percentage", Float) = 1.0
	}
	CGINCLUDE
		#include "UnityCG.cginc"
		
		struct appdata {
			float4 vertex : POSITION;
			float3 normal : NORMAL;
			half4 texcoord : TEXCOORD0;
		};
		
		//顶点返回
		struct v2f {
			float4 pos : POSITION;
			fixed4 color : COLOR; //顶点颜色
			half2 uv : TEXCOORD0;
		};
		
		//uniform, applied by Program
		uniform fixed4 _Color;
		uniform sampler2D _MainTex;
		float4 _MainTex_ST;
		fixed _AlphaPercentage;

		/**
		 * RGB->HSV
		 *
		 * 返回float3{r, g, b}
		 */
		fixed3 rgb2hsv(fixed r, fixed g, fixed b)
		{
			fixed3 rlt;
			fixed h, s, v;
			fixed fMax = max(r, max(g, b));
			fixed fMin = min(r, min(g, b));
			fixed fDiff = fMax - fMin;
			if (r == fMax) h = (g-b)/fDiff;
			if (g == fMax) h = 2 + (b-r)/fDiff;
			if (b == fMax) h = 4 + (r-g)/fDiff;

			h= h*60;
			if (h<0) h+=360;
			v = fMax;
			s = fDiff/fMax;

			rlt.x = h;
			rlt.y = s;
			rlt.z = v;

			return rlt;
		}

		/**
		 * HSV->RGB
		 *
		 * 返回float3{h, s, v}
		 */
		fixed3 hsv2rgb(fixed h, fixed s, fixed v)
		{
			fixed3 rlt;
			fixed r, g, b;
			int i;
			fixed f, p, q, t;

			h /= 60;
			i = floor(h);
			f = h-i;
			p = v * (1 - s);
			q = v * (1 - s*f);
			t = v * (1 - s*(1-f));

			if (i==0) rlt = fixed3(v, t, p);
			if (i==1) rlt = fixed3(q, v, p);
			if (i==2) rlt = fixed3(p, v, t);
			if (i==3) rlt = fixed3(p, q, v);
			if (i==4) rlt = fixed3(t, p, v);
			if (i==5) rlt = fixed3(v, p, q);

			return rlt;
		}

		v2f vert(appdata v) {
			v2f o;
			o.pos = mul( UNITY_MATRIX_MVP, v.vertex);;
			o.color = _Color;
			half fValue = max(0.0, _AlphaPercentage);
			half alpha = o.color.a * fValue;
			o.color.a = fixed4(o.color.rgb, alpha); 
			o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
			return o;
		}
	ENDCG
	SubShader {
		Pass {
			Cull Off
			Blend SrcAlpha OneMinusSrcAlpha // Normal
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			fixed4 frag(v2f i) :COLOR {
				fixed4 texCol = tex2D(_MainTex, i.uv);
				clip(texCol.a-0.5);

				fixed fValue = max(0.0, _AlphaPercentage);
				fixed alpha = texCol.a * fValue;

				//float grey = dot(texCol.rgb, float3(0.299, 0.587, 0.114));  
				//texCol.rgb = float3(grey, grey, grey);

				//去色
				fixed3 hsvColor = rgb2hsv(texCol.r, texCol.g, texCol.b);
				fixed3 rbgColor = hsv2rgb(0.0, 0.0, hsvColor.z);

				return float4(rbgColor.rgb, alpha);
			}
			ENDCG
			
		}
	}
	Fallback "Diffuse"
}