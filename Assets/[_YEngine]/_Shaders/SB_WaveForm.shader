
Shader "SB/iOS/NoLight/WaveForm" { 
	Properties {
		_MainTex ("WaveForm Tex (RGBA)", 2D) = "white" {}
		_BackTex ("Background Tex (RGBA)", 2D) = "black" {}
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
			uniform sampler2D _BackTex;
			
			uniform float4 _Color;
			
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
				half4 texOver = tex2D(_BackTex, i.uv);
		        texFinal = lerp(texOver,texMain, texMain.a);
				return texFinal;
			}
			ENDCG
		}
	}
}