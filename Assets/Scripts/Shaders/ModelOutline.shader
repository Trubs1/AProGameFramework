Shader "Custom/ModelOutline" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0

		_OutlineCol("OutlineCol", Color) = (1,0,0,1)  
        _OutlineFactor("OutlineFactor", Range(0,1)) = 0.1  

	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200


        //沿法线挤出一点，只输出描边的颜色  
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


		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
