// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Shader created with Shader Forge v1.05 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.05;sub:START;pass:START;ps:flbk:Transparent/Cutout/Diffuse,lico:1,lgpr:1,nrmq:1,limd:1,uamb:True,mssp:True,lmpd:True,lprd:False,rprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:0,bsrc:0,bdst:1,culm:2,dpts:2,wrdp:True,dith:0,ufog:True,aust:True,igpj:False,qofs:0,qpre:2,rntp:3,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:0,x:34000,y:32640,varname:node_0,prsc:2|diff-322-OUT,spec-3-OUT,gloss-270-OUT,normal-2-RGB,transm-7-OUT,lwrap-6-OUT,clip-1-A,voffset-394-OUT;n:type:ShaderForge.SFN_Tex2d,id:1,x:33572,y:32631,ptovrint:False,ptlb:Diffuse,ptin:_Diffuse,varname:_Diffuse,prsc:2,tex:66321cc856b03e245ac41ed8a53e0ecc,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:2,x:33572,y:32818,ptovrint:False,ptlb:Normal,ptin:_Normal,varname:_Normal,prsc:2,tex:cb6c5165ed180c543be39ed70e72abc8,ntxv:3,isnm:True;n:type:ShaderForge.SFN_Vector1,id:3,x:33777,y:32641,varname:node_3,prsc:2,v1:0.2;n:type:ShaderForge.SFN_Vector3,id:6,x:33572,y:33075,varname:node_6,prsc:2,v1:0.9,v2:0.9,v3:0.8;n:type:ShaderForge.SFN_Vector3,id:7,x:33572,y:32976,varname:node_7,prsc:2,v1:0.9,v2:1,v3:0.5;n:type:ShaderForge.SFN_Vector1,id:270,x:33777,y:32701,varname:node_270,prsc:2,v1:0.4;n:type:ShaderForge.SFN_VertexColor,id:321,x:33330,y:32501,varname:node_321,prsc:2;n:type:ShaderForge.SFN_Multiply,id:322,x:33777,y:32508,varname:node_322,prsc:2|A-330-OUT,B-1-RGB;n:type:ShaderForge.SFN_Lerp,id:330,x:33572,y:32478,varname:node_330,prsc:2|A-331-OUT,B-337-OUT,T-321-B;n:type:ShaderForge.SFN_Vector1,id:331,x:33330,y:32356,varname:node_331,prsc:2,v1:1;n:type:ShaderForge.SFN_Vector3,id:337,x:33330,y:32410,varname:node_337,prsc:2,v1:0.9632353,v2:0.8224623,v3:0.03541304;n:type:ShaderForge.SFN_VertexColor,id:389,x:32886,y:33347,varname:node_389,prsc:2;n:type:ShaderForge.SFN_NormalVector,id:391,x:33073,y:33231,prsc:2,pt:False;n:type:ShaderForge.SFN_Time,id:392,x:33073,y:33586,varname:node_392,prsc:2;n:type:ShaderForge.SFN_Sin,id:393,x:33479,y:33548,varname:node_393,prsc:2|IN-413-OUT;n:type:ShaderForge.SFN_Multiply,id:394,x:33729,y:33420,cmnt:Wind animation,varname:node_394,prsc:2|A-562-OUT,B-389-R,C-393-OUT,D-403-OUT;n:type:ShaderForge.SFN_Vector1,id:403,x:33479,y:33699,varname:node_403,prsc:2,v1:0.02;n:type:ShaderForge.SFN_Add,id:413,x:33298,y:33548,varname:node_413,prsc:2|A-519-OUT,B-392-T;n:type:ShaderForge.SFN_Multiply,id:519,x:33073,y:33457,varname:node_519,prsc:2|A-389-B,B-520-OUT;n:type:ShaderForge.SFN_Pi,id:520,x:32919,y:33494,varname:node_520,prsc:2;n:type:ShaderForge.SFN_Add,id:561,x:33294,y:33171,varname:node_561,prsc:2|A-563-OUT,B-391-OUT;n:type:ShaderForge.SFN_Normalize,id:562,x:33479,y:33280,varname:node_562,prsc:2|IN-561-OUT;n:type:ShaderForge.SFN_Vector3,id:563,x:33073,y:33131,cmnt:Wind direction,varname:node_563,prsc:2,v1:1,v2:0.5,v3:0.5;proporder:1-2;pass:END;sub:END;*/

Shader "Custom/Animated Vegetation/Complex" {
    Properties {
        _Diffuse ("Diffuse", 2D) = "white" {}
		_WindParam ("Wind Set", Vector) = (1,.5,.5,.2) // 风的参数
    }	
    SubShader {
        Tags {
            "Queue"="AlphaTest"
            "RenderType"="TransparentCutout"
        }
        Pass {
            Name "ForwardBase"
            Tags {
                "LightMode"="ForwardBase"
            }
            Cull Off
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma exclude_renderers gles xbox360 ps3 flash 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform sampler2D _Diffuse; 
			uniform float4 _Diffuse_ST;
            uniform float _WindStrength;
			uniform float4 _WindParam;
			
			//uniform sampler2D _Normal; uniform float4 _Normal_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 binormalDir : TEXCOORD4;
                float4 vertexColor : COLOR;
                LIGHTING_COORDS(5,6)
             
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.normalDir = mul(unity_ObjectToWorld, float4(v.normal,0)).xyz;
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.binormalDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                float4 node_392 = _Time + _TimeEditor;
                //v.vertex.xyz += (normalize((float3(1,0.5,0.5)+v.normal))*o.vertexColor.r*sin(((o.vertexColor.b*3.141592654)+node_392.g))*0.02);
				v.vertex.xyz += (normalize((_WindParam.xyz+v.normal))*o.vertexColor.r*sin(((o.vertexColor.b*3.141592654)+node_392.g))*_WindParam.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }

			//片段着色器
            fixed4 frag(VertexOutput i) : COLOR {
                float4 _Diffuse_var = tex2D(_Diffuse,TRANSFORM_TEX(i.uv0, _Diffuse));
                clip(_Diffuse_var.a - 0.5); //
               
                float3 diffuse = tex2D(_Diffuse, TRANSFORM_TEX(i.uv0, _Diffuse));
				// Final Color:
                float3 finalColor = diffuse;// + specular;w
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Transparent/Cutout/Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
