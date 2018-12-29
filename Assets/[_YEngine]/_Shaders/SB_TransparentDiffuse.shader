
Shader "SB/iOS/NoLight/TransparentDiffuse" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB) A (Alpha)", 2D) = "white" {}
	}
	SubShader {
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
		Lighting Off 
		ZWrite on
		Fog { Color (0,0,0,0) }
		Pass{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma fragmentoption ARB_fog_exp2
			#include "UnityCG.cginc"
			
			uniform sampler2D _MainTex;
			uniform float4 _Color; 

			half4 frag (v2f_img i) : COLOR {
				half4 texFinal;
			    half4 texMain = tex2D(_MainTex,i.uv);
				texFinal = texMain * _Color;
	            return texFinal;
            }
			ENDCG
		}
	} 
	FallBack "Diffuse"
}
