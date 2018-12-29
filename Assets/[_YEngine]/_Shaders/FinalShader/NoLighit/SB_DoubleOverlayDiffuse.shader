
Shader "SB/iOS/NoLight/OverlayDiffuse" {
	Properties {
		_OverlayTex ("Overlay Layer (RGB) A (Alpha)", 2D) = "white" {}
		_MainTex ("Bottam Layer (RGB) A (Alpha)", 2D) = "white" {}
	}
	SubShader {
		Tags {"RenderType" = "Geometry" "Queue" = "Geometry"}
		Lighting Off 
		ZWrite on
		Fog { Color (0,0,0,0) }
		Pass{
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest

				#include "UnityCG.cginc"
				
				struct appdata_t {
					float4 vertex : POSITION;
					fixed4 color : COLOR;
					float2 texcoord : TEXCOORD0;
				};

				struct v2f {
					float4 vertex : POSITION;
					fixed4 color : COLOR;
					float2 texcoord : TEXCOORD0;
				};

				sampler2D _MainTex;
				sampler2D _OverlayTex;
				
				uniform float4 _MainTex_ST;
				uniform float4 _OverlayTex_ST;

				v2f vert (appdata_t v) {
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
					return o;
				}
				
				fixed4 frag (v2f i) : COLOR {
					// Get the raw texture value
					float4 texBottamLayer = tex2D(_MainTex, i.texcoord);
					float4 texOverlay = tex2D(_OverlayTex,i.texcoord);
					
					// Declare the output structure
					fixed4 output;
					if(texBottamLayer.r < 0.5){
						output.r = texOverlay.r * texBottamLayer.r * 2 ;
					} else {
						output.r = 1 - 2 * (1 - texBottamLayer.r) * (1 - texOverlay.r);
					}
					if(texBottamLayer.g < 0.5){
						output.g = texOverlay.g * texBottamLayer.g * 2 ;
					} else {
						output.g = 1 - 2 * (1 - texBottamLayer.g) * (1 - texOverlay.g);
					}
					if(texBottamLayer.b < 0.5){
						output.b = texOverlay.b * texBottamLayer.b * 2 ;
					} else {
						output.b = 1 - 2 * (1 - texBottamLayer.b) * (1 - texOverlay.b);
					}
					if(texBottamLayer.a < 0.5){
						output.a = texOverlay.a * texBottamLayer.a * 2 ;
					} else {
						output.a = 1 - 2 * (1 - texBottamLayer.a) * (1 - texOverlay.a);
					}
					
					output = lerp(texOverlay,output,texBottamLayer.a);
					output = lerp(texBottamLayer,output,texOverlay.a);
				
					return output;
				}
			ENDCG
		}
	} 
}					