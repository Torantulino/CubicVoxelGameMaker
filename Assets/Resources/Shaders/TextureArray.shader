Shader "Custom/Sample2DArrayTexture"
{
	Properties
	{
		_TexArr("Texture", 2DArray) = "" {}
	}
	SubShader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// DX10/OpenGLES3 Target needed for Texture Arrays
			// Shader Model 3.5 Minimum
			#pragma target 3.5

			#include "UnityCG.cginc"

			// vertex input: position, UV
        	struct appdata {
            	float4 vertex : POSITION;
            	float4 texcoord : TEXCOORD0;
        	};

			struct v2f
			{
				float3 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			// Vertex Shader
			// Called once for each vertex
			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv.xyz = v.texcoord.xyz;
				return o;
			}

			UNITY_DECLARE_TEX2DARRAY(_TexArr);

			// Fragment Shader
			// Called for every pixel
			half4 frag(v2f i) : SV_Target
			{
				return UNITY_SAMPLE_TEX2DARRAY(_TexArr, i.uv);
			}
			ENDCG
		}
	}
}