

Shader "SB/iOS/AlahaReflection" { 
	Properties {
 		_Color ("Main Color", Color) = (1,1,1,0)
 		_MainTex ("Base Texture", 2D) = "white" {}
        _Cube  ("Reflation Map (RGB)",2D) = "_Skybox" {TexGen SphereMap}
		_Shininess ("Shininess", Range (0.01, 1)) = 0.7
    }
	SubShader { 
    	Tags { 
	    	"LightMode" = "Vertex" 
	    	"Queue"="Transparent"
	    	"RenderType"="Transparent"
    	}
		Material {
			Diffuse [_Color]
			Ambient [_Color]
	        Emission [_Color]				
			Specular [_Color]
			Shininess [_Shininess]
		}

		Lighting On
		SeparateSpecular On
		Pass {
			Blend SrcAlpha OneMinusSrcAlpha
			SetTexture [_MainTex] {Combine texture, texture }
		}
		Pass {
			Blend DstAlpha One
			SetTexture [_] {Combine One-texture, one-texture }
			SetTexture [_Cube] {
				combine texture, previous }
		}
	}
	FallBack "Reflective/Diffuse"
	
}