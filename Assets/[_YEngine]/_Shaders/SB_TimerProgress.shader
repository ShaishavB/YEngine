Shader "SB/iOS/NoLight/TimerProgress" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_ProgresssTex("Timer Effect Tex (A)",2D) = "white"{}
		_Procgress("Progress", Range(0,1.01)) = 0
	}
	SubShader{
	
		Pass{
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#include "UnityCG.cginc"
			
			uniform sampler2D _MainTex;
			uniform sampler2D _ProgresssTex;
			uniform fixed _Procgress; 
			
			half4 frag (v2f_img i) : COLOR {
				half4 texFinal;
			    half4 texMain = tex2D(_MainTex,i.uv);
			    half texProgress = tex2D(_ProgresssTex,i.uv).a;
				texFinal = texMain;

				if(texProgress < _Procgress){
					texFinal.a = 0;
				}
   	            return texFinal;
            }
			
			ENDCG
		}
	}
	
}
