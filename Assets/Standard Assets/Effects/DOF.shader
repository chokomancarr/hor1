Shader "Hidden/DOF"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_DepthTex ("Texture", 2D) = "white" {}
		_focal ("Focal", Range(0, 1)) = 0.1
		_aper ("Aperture", Range(0.01, 1)) = 0.3
		_cut ("Cutoff", Range(0, 0.5)) = 0
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass //set depth texture
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			sampler2D _CameraDepthTexture;
			fixed _focal;
			fixed _aper;
			float pixelW;
			float pixelH;


			fixed4 frag (v2f i) : SV_Target
			{
				half d = Linear01Depth(UNITY_SAMPLE_DEPTH(tex2D(_CameraDepthTexture, i.uv)));
				float depth = ((d - _focal)/_aper + 0.5);

				return fixed4(d, depth, 2*abs(depth-0.5), tex2D(_MainTex, i.uv).a);
			}
			ENDCG
		}
		Pass //blur64
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			sampler2D _DepthTex;
			fixed _focal;
			fixed _aper;
			fixed _cut;
			fixed _depthNearMul;
			fixed _depthDiffMul;
			fixed _depthDiffOff;
			float pixelW;
			float pixelH; 


			fixed4 frag (v2f i) : SV_Target
			{
				//blur pixel based on abs(depth - _focal)
				const float w[64] = {

					0.000082,	0.000167,	0.000306,	0.000502,	0.000738,	0.000971,	0.001146,	0.001211,
					0.000167,	0.000341,	0.000625,	0.001026,	0.001509,	0.001987,	0.002344,	0.002476,
					0.000306,	0.000625,	0.001146,	0.001881,	0.002765,	0.003641,	0.004294,	0.004537,
					0.000502,	0.001026,	0.001881,	0.003086,	0.004537,	0.005975,	0.007047,	0.007446,
					0.000738,	0.001509,	0.002765,	0.004537,	0.00667,	0.008783,	0.01036,	0.010946,
					0.000971,	0.001987,	0.003641,	0.005975,	0.008783,	0.011566,	0.013643,	0.014414,
					0.001146,	0.002344,	0.004294,	0.007047,	0.01036,	0.013643,	0.016092,	0.017002,
					0.001211,	0.002476,	0.004537,	0.007446,	0.010946,	0.014414,	0.017002,	0.017965,
				};

				fixed4 buffer = fixed4(0, 0, 0, 0);
				fixed4 col0 = tex2D(_MainTex, i.uv);
				fixed4 depth0 = tex2D(_DepthTex, i.uv);
				[unroll(225)]
				for (int x = 0; x < 15; x++) {
					int xx = 7-abs(x-7);
					for (int y = 0; y < 15; y++) {
						int yy = 7-abs(y-7);
						float2 uv1 = i.uv + float2(pixelW * (x - 7), pixelH * (y - 7));
						fixed4 depth = tex2D(_DepthTex, uv1);
						fixed4 col = tex2D(_MainTex, uv1);
						//buffer += lerp(col0, col, 2*abs(depth.g-0.5))*w[8*yy + xx];
						buffer += lerp(col0, col, depth.b * clamp(_depthDiffOff+(depth0.r - depth.r)/_depthDiffMul , 0, clamp(max(depth0.b, (0.5-depth.g)/_depthNearMul), 0, 1)))*w[8*yy + xx];
					}
				}

				return buffer;
			}
			ENDCG
		}

		Pass //show alpha
		{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			sampler2D _MainTex;

			fixed4 frag (v2f_img i) : SV_Target
			{
				fixed4 t = tex2D(_MainTex, i.uv);
				return fixed4 (t.g,t.g,t.g,t.a);
			}
			ENDCG
		}
	}
}
