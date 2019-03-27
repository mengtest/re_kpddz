Shader "Custom/SelfEmission" 
{
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _RimColor ("Rim Color", Color) = (1, 1, 1, 1)
        _RimWidth ("Rim Width", Float) = 0.7
		_RimIntensity("Rim Intensity", Float) = 1.0
		_RimAlphaIntensity("Rim  Alpha Intensity", Float) = 1.5
		_Color("Main Color", Color) = (1,1,1,1)

    }
    SubShader {
        Pass {
       		Lighting Off
			Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                struct appdata 
                {
                    float4 vertex : POSITION;
                    float3 normal : NORMAL;
                    float2 texcoord : TEXCOORD0;
                };

                struct v2f 
                {
                    float4 pos : SV_POSITION;
                    float2 uv : TEXCOORD0;
                    fixed4 color : COLOR;
                };

                uniform float4 _MainTex_ST;
                uniform fixed4 _RimColor;
                float _RimWidth;
				float _RimIntensity;
				float _RimAlphaIntensity;

                v2f vert (appdata_base v) {
                    v2f o;
                    o.pos = mul (UNITY_MATRIX_MVP, v.vertex);

                    float3 viewDir = normalize(ObjSpaceViewDir(v.vertex));
					//边缘高亮
					float dotProduct = 1 - dot(v.normal, viewDir);

					//边缘透明度最低
					o.color = smoothstep(1 - _RimWidth, 1.0, dotProduct);;
					o.color *= _RimColor;

					o.color.rgb *= _RimIntensity;
					o.color.a = pow(o.color.a, _RimAlphaIntensity);
					
                    o.uv = v.texcoord.xy;
                    return o;
                }

                uniform sampler2D _MainTex;
				uniform fixed4 _Color;

                fixed4 frag(v2f i) : COLOR {
                    fixed4 texcol = tex2D(_MainTex, i.uv);
					texcol *= _Color;
                    texcol.rgb += i.color.rgb;
					texcol.a =i.color.a;
                    return texcol;
                }
            ENDCG
        }
    }
}