// Copyright (c) 2018 WangQiang(279980661@qq.com)
// author:Trubs (WQ)
// Date:2018/08/08

Shader "Unlit/FieldOfViewRenderer"
{
	Properties
	{
		_FieldColor ("FieldColor", Color) = (1,1,1,1)
		_AlphaTex("AlphaTex", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType" = "Transparent" }
		LOD 100

		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			Cull Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float4 color : COLOR;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 color : TEXCOORD1;
			};

			float4 _FieldColor;

			sampler2D _AlphaTex;
			float4 _AlphaTex_ST;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.color = v.color;

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = _FieldColor;
				return fixed4(i.color.rgb, tex2D(_AlphaTex, i.uv).r);
			}
			ENDCG
		}
	}
}
