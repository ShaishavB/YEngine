

Shader "SB/iOS/NoLight/WTWLetters" {
	Properties {
	    _Color ("Main Color", Color) = (1,1,1,1)
	    _BGColor ("Back Ground Color", Color) = (1,1,1,1)
	    _MainTex ("Letters[MainTex] (RGBA)", 2D) = "white" {}
	    _BGTex ("BG Texture (RGBA)", 2D) = "white" {}
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
			uniform sampler2D _BGTex;
			uniform float4 _Color; 
			uniform float4 _BGColor; 

			half4 frag (v2f_img i) : COLOR {
				half4 texFinal;
			    half4 texMain = tex2D(_MainTex,i.uv);
			    texMain *=  _Color;
			    
			    half4 texSecond = tex2D(_BGTex,i.uv);
			    texSecond *= _BGColor;
			    
			    texFinal = lerp(texSecond, texMain, texMain.a);
	            return texFinal;
            }
			ENDCG
		}
	}
}