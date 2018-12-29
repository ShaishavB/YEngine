

Shader "SB/iOS/AlphaBlend" {
	Properties {
 		_Color ("Main Color", Color) = (1,1,1,0)
 		_MainTex ("Base Texture", 2D) = "white" {}
		_Shininess ("Shininess", Range (0.01, 1)) = 0.7
	}

	SubShader {
		Tags { "RenderType" = "Geometry" "Queue" = "Transparent"}
		Blend SrcAlpha OneMinusSrcAlpha
		Material {
			Diffuse [_Color]
			Ambient [_Color]
        	Emission [_Color]				
			Shininess [_Shininess]
			Specular [_Color]
		} 
		Lighting On
		SeparateSpecular On
		Pass{
			SetTexture [_MainTex] {
			constantcolor [_Color]
			combine texture * constant}
		}
	}
	FallBack "Diffuse"
}

