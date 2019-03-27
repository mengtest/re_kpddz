Shader "Custom/PlayerDebuff" {
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
			float4 texcoord : TEXCOORD0;
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
		half _AlphaPercentage;

		/**
		 * Photoshop 柔光效果
		 *
		 * 参数：src 对应上面图层颜色 B
		 *		dest 对应下面图层颜色 A
		 *
		 * B<=128 则 C=(A×B)/128+(A/255)^2×(255-2B) 
		 * B>128  则 C=(A×B反相)/128+sqrt(A/255)×(2B-255)
		 *
		 * 返回：计算后的颜色分量
		 */
		fixed softLigt(fixed src, fixed dest)
		{
			fixed c;
			if (src <= 0.5)
			{
				c = (src * dest) / 0.5 + pow(dest, 2) * (1.0-2*src);
			}
			else
			{
				c = ((1.0-src) * dest)/0.5 + sqrt(dest) * (2*src - 1.0);
			}

			return c;
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
				
				half fValue = max(0.0, _AlphaPercentage);
				half alpha = texCol.a * fValue;

				//DEBUFF
				fixed r = softLigt(i.color.r, texCol.r);
				fixed g = softLigt(i.color.g, texCol.g);
				fixed b = softLigt(i.color.b, texCol.b);

				texCol.rgb = fixed3(r,g,b);
				return fixed4(texCol.rgb, alpha);
			}
			ENDCG
			
		}
	}
	Fallback "Diffuse"
}