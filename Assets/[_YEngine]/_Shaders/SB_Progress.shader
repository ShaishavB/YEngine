Shader "SB/iOS/NoLight/ProgressBar" { 
	Properties {
		_MainTex ("Main Tex (RGBA)", 2D) = "black" {}
		_OverlayTex ("OverLay Tex (RGBA)", 2D) = "white" {}
		_Progress ("Progress", Range(0.0,1.0)) = 0.0
	}

	SubShader {
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			
			uniform sampler2D _MainTex;
			uniform sampler2D _OverlayTex;
			
			uniform float _Progress;
			
			struct v2f {
				float4 pos : POSITION;
				float2 uv : TEXCOORD0;
			};
			
			v2f vert (appdata_base v) {
				v2f o;
				o.pos = UnityObjectToClipPos (v.vertex);
				o.uv = TRANSFORM_UV(0);
				return o;
			}
			half4 frag( v2f i ) : COLOR {
				half4 texFinal;
				half4 texMain = tex2D(_MainTex, i.uv);
				half4 texOver = tex2D(_OverlayTex, i.uv);
				float b = i.uv.x > _Progress;
		        texFinal = lerp(texOver, texMain, b);
				
				return texFinal;
			}
			ENDCG
		}
	}
}