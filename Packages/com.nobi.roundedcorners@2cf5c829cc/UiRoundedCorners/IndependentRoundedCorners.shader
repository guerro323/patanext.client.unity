﻿Shader "UI/RoundedCorners/IndependentRoundedCorners" {
    
    Properties {
        [HideInInspector] _MainTex ("Texture", 2D) = "white" {}
        
        // --- Mask support ---
        [HideInInspector] _StencilComp ("Stencil Comparison", Float) = 8
        [HideInInspector] _Stencil ("Stencil ID", Float) = 0
        [HideInInspector] _StencilOp ("Stencil Operation", Float) = 0
        [HideInInspector] _StencilWriteMask ("Stencil Write Mask", Float) = 255
        [HideInInspector] _StencilReadMask ("Stencil Read Mask", Float) = 255
        [HideInInspector] _ColorMask ("Color Mask", Float) = 15
        [HideInInspector] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
        // Definition in Properties section is required to Mask works properly
        _r ("r", Vector) = (0,0,0,0)
        _halfSize ("halfSize", Vector) = (0,0,0,0)
        _rect2props ("rect2props", Vector) = (0,0,0,0)
        _outline ("outline", Float) = 0
        // ---
    }
    
    SubShader {
        Tags { 
            "RenderType"="Transparent"
            "Queue"="Transparent" 
        }
        
        // --- Mask support ---
        Stencil {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }    
        Cull Off
        Lighting Off
        ZTest [unity_GUIZTestMode]
        ColorMask [_ColorMask]
        // ---
        
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

        Pass {
            CGPROGRAM
            
            #include "UnityCG.cginc"
            #include "SDFUtils.cginc"
            #include "ShaderSetup.cginc"
            
            #pragma vertex vert
            #pragma fragment frag
            
            float4 _r;
            float _outline;
            float4 _halfSize;
            float4 _rect2props;
            sampler2D _MainTex;
            
            fixed4 frag (v2f i) : SV_Target {
                float alpha = CalcAlphaForIndependentCorners(i.uv, _halfSize.xy, _rect2props, _r);
                if (_outline > 0)
                {
                    float4 newR = _halfSize;
                    newR.xy -= _outline;

                    float2 uv = i.uv;
                    uv = (uv - 0.5) * (_halfSize / newR) + 0.5;
                    
                    alpha *= 1 - CalcAlphaForIndependentCorners(uv, _halfSize.xy - (_outline/2), _rect2props, _r - _outline);
                }
                
                return mixAlpha(tex2D(_MainTex, i.uv), i.color, alpha);
            }
            
            ENDCG
        }
    }
}
