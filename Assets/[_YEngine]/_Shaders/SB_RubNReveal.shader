

Shader "SB/iOS/NoLight/RubNReveal" { 
	Properties {
        _MainTex ("Main Question Image (RGB)", 2D) = "white" {}
        _Alpha ("Cover Alpha (A)", 2D) = "white" {}
        _FogTex ("Fog Effect Texture (RGBA)", 2D) = "white" {}
    }
    Category{
		SubShader { 
			Pass {
				Tags {"RenderType" = "Geometry" "Queue" = "Geometry"}
				Lighting Off 
				ZWrite on
				Fog { Color (0,0,0,0) }

				CGPROGRAM
		            #pragma vertex vert_img
		            #pragma fragment frag
		            #pragma fragmentoption ARB_precision_hint_fastest
		            #pragma fragmentoption ARB_fog_exp2
	
		            #include "UnityCG.cginc"
		            uniform sampler2D _FogTex;
		            uniform sampler2D _MainTex;
		            uniform sampler2D _Alpha;
	
	   				half4 frag (v2f_img i) : COLOR {
						half4 texFinal;
					    half4 texMain = tex2D(_MainTex,i.uv);
					    half4 texFog = tex2D(_FogTex, i.uv) ;
					    half4 texAlpha = tex2D(_Alpha,i.uv);
					    
		                texFinal =  lerp(texMain,texFog, texAlpha.a);
		                return texFinal;
		            }
		        ENDCG
	        }
        }
	}
}