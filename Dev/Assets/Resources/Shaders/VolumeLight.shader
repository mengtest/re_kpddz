Shader "Custom/VolumeLight"
{
    Properties
    {
        [NoScaleOffset]_mainTex ("Main", 2D) = "white" {}
        [NoScaleOffset]_maskTex ("Mask", 2D) = "white" {}
        _speed ("Speed (more big more slow)", float) = 20.0
        _brightness ("Brightness", float) = 2.0
    }

    SubShader
    {
        Tags { "Queue" = "Transparent" }

        Pass
        {
            Name "VolumeLight"
            Cull Off
            ZWrite Off
            ZTest LEqual 
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #include "UnityCG.cginc"

            #pragma vertex vert_img  // vertex用系统提供的默认处理函数
            #pragma fragment frag
            
            // 属性数据
            sampler2D _mainTex;
            sampler2D _maskTex;
            float _speed;
            float _brightness;

            // 像素函数
            fixed4 frag (v2f_img i) : SV_Target
            {
                half2 uv = i.uv;
                fixed scrollX = sin(_Time.y/_speed);
                uv += fixed2(scrollX, 0.0); 
                fixed4 col = tex2D(_mainTex, uv);
                col *= _brightness;
                fixed4 colMask = tex2D(_maskTex, i.uv);

                half a = (1-colMask.a) * (1-colMask.a);
                return fixed4(col.r, col.g, col.b, a); 
            }
            
            ENDCG
        }
    }
}