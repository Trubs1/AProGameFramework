///description: 卡通渲染(光照分层,轮廓描边)
///author:Trubs (WQ)
///Date:2019/04/19

Shader "Custom/CelShadingForward"
{
	Properties
	{
		_Color("Color", Color) = (1, 1, 1, 1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}

		_LitLevel1("LiteLevel1", float) = -1
		_LitLevel2("LiteLevel2", float) = 0.0
		_LitLevel3("LiteLevel3", float) = 0.4

		_OutlineCol("OutlineCol", Color) = (1,0,0,1)  
        _OutlineFactor("OutlineFactor", Range(0,1)) = 0.1  

	}
	 	 
	SubShader
	{
		Tags
		{
		    "RenderType" = "Opaque"
		}
		LOD 200
		 		 
		CGPROGRAM
		#pragma surface surf CelShadingForward
		#pragma target 3.0	 
		 
		sampler2D _MainTex;
		fixed4 _Color;
		float _LitLevel1;
		float _LitLevel2;
		float _LitLevel3;

		half4 LightingCelShadingForward(SurfaceOutput s, half3 lightDir, half atten)
		{
			//通常情况下一个像素所接收到的光源强度是光源方向与法线方向之间的点积（NdotL）
			half NdotL = dot(s.Normal, lightDir);			 

			if (NdotL <= _LitLevel1)
                NdotL = _LitLevel1;
            else if (NdotL <= _LitLevel2)
                NdotL = _LitLevel2;
            else if (NdotL <= _LitLevel3)
                NdotL = _LitLevel3;
            else
                NdotL = 1;


			//NdotL = 1 + clamp(floor(NdotL), -1, 0); //只有亮黑暗两个乘次
			//NdotL = smoothstep(0, 0.025f, NdotL);
			 		 
			half4 c;
			c.rgb = s.Albedo * _LightColor0.rgb * (NdotL * atten * 2);
			c.a = s.Alpha;		 
			 
			return c;
		}
	 

		struct Input
		{
			float2 uv_MainTex;
		};
		 
		 
		void surf(Input IN, inout SurfaceOutput o)
		{
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG


		//轮廓描边:沿法线挤出一点，只输出描边的颜色  
        Pass  
        {              
            Cull Front  //剔除正面，只渲染背面，对于大多数模型适用，不过如果需要背面的，就有问题了  

			CGPROGRAM  

            #pragma vertex vert  
            #pragma fragment frag  

            #include "UnityCG.cginc"  

            fixed4 _OutlineCol;  
            float _OutlineFactor;  

			struct v2f  
            {  

                float4 pos : SV_POSITION;  

            };  

			v2f vert(appdata_full v)  
            {  
                v2f o;  
                //在vertex阶段，每个顶点按照法线的方向偏移一部分，不过这种会造成近大远小的透视问题  
                //v.vertex.xyz += v.normal * _OutlineFactor;  
                o.pos = UnityObjectToClipPos(v.vertex); 
                //将法线方向转换到视空间  
                float3 vnormal = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);  
                //将视空间法线xy坐标转化到投影空间，只有xy需要，z深度不需要了  
                float2 offset = TransformViewToProjection(vnormal.xy);  
                //在最终投影阶段输出进行偏移操作  
                o.pos.xy += offset * _OutlineFactor;  
                return o;  
            }                

            fixed4 frag(v2f i) : SV_Target  
            {  
                //这个Pass直接输出描边颜色  

                return _OutlineCol;  
            }  
            
			ENDCG  
        }
	}
	FallBack "Diffuse"
}