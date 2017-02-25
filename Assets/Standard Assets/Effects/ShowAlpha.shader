Shader "Hidden/ShowAlpha" {

Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
}

SubShader {
	Pass {
		ZTest Always Cull Off ZWrite Off
		Fog { Mode off }
				
CGPROGRAM
#pragma vertex vert_img
#pragma fragment frag
#include "UnityCG.cginc"

uniform sampler2D _MainTex;

fixed4 frag (v2f_img i) : SV_Target
{
	return tex2D(_MainTex, i.uv).a;
}
ENDCG

	}
}

Fallback off

}