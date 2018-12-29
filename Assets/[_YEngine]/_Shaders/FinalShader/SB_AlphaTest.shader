Shader "SB/iOS/AlphaTest" {
	Properties {
		_MainTex ("Base Texture", 2D) = "white" {}
		_CutOff("Cutoff Value",Range(0.00,1)) = 0.04
		_Color("Base Color",color) = (0,0,0,0)
	}

	SubShader {
		Alphatest Greater [_CutOff]
		Tags {"RenderType" = "Geometry" "Queue" = "AlphaTest"}

		BindChannels{
			bind "vertex", vertex
			Bind "Color", color
			bind "texcoord1", texcoord
		}

		Pass{
			SetTexture [_MainTex] {
				ConstantColor [_Color]
				combine texture  * constant }
		}
	}
}
