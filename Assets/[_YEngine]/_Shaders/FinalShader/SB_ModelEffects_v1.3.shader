

Shader "SB/iOS/ModelEffects_v1.3" { 
	Properties {
		_Color ("Color", Color) = (1,1,1,1) 
		_SpecColor ("Specular Color", Color) = (1,1,1,1) 

        _ColorTex("Texture (RGB)", 2D) = "white" {}
        _MainTex ("Vinyl RGBA", 2D) = "white" {}
        _EffectTex ("R-(Empty) G-(Reflaction Alpha) B-(DepthMap)", 2D) = "white" {}
		_Cube  ("Reflation Map (RGB)",2D) = "_Skybox" {TexGen CubeReflect}
		_Shininess ("Shininess", Range(0.01,1)) = 0

    }
		SubShader { 
			Pass {
				Tags {"RenderType" = "Geometry" "Queue" = "Geometry" }
			
				Name "BASE"
				BindChannels {
					Bind "Vertex", vertex
					Bind "texcoord", texcoord0 // lightmap uses 2nd uv
					Bind "texcoord1", texcoord1 // main uses 1st uv
				}

				CGPROGRAM
		            #pragma vertex vert_img
		            #pragma fragment frag
		            #pragma fragmentoption ARB_precision_hint_fastest
		            #pragma fragmentoption ARB_fog_exp2
	
		            #include "UnityCG.cginc"
	
		            uniform sampler2D _MainTex;
		            uniform sampler2D _ColorTex;
		            uniform sampler2D _EffectTex;
	
	   				half4 frag (v2f_img i) : COLOR {
						half4 texFinal;
						
					    half4 texEffect = tex2D(_EffectTex,i.uv);
	
					    half4 texColor = tex2D(_ColorTex, i.uv) ;
					    half4 texVinyl = tex2D(_MainTex,i.uv);
	
		                texVinyl =  lerp(texColor, texVinyl, texVinyl.a); 

						texFinal = (texVinyl * texEffect.b);

						texFinal.a = texEffect.g;
		                return texFinal;
		            }
		        ENDCG
	        }
		        
        Pass {
			Name "REFLECT"
			Tags { "LightMode" = "Vertex" "RenderType" = "Geometry" "Queue" = "Geometry"}
			ZWrite Off
			Blend DstColor OneMinusSrcAlpha

			Material {
				Shininess [_Shininess]
				Specular [_SpecColor]
			} 
			Lighting On
			SeparateSpecular On

			SetTexture [_] {Combine One-texture, one-texture }
			SetTexture [_Cube] {
				combine texture, previous }
		}
	}
}