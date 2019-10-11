Shader "Custom/Monster"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", 2D) = "black" {}
		_BumpMap("Bumpmap", 2D) = "bump" {}
		_EmissionColor("Color", Color) = (0,0,0)
		_EmissionMap("Emission", 2D) = "white" {}
		_OcclusionMap("Occlusion Map", 2D) = "white" {}
		[Toggle] _Tint("Tint", Float) = 0
		_TintColor("Tint Color", Color) = (0,0,0,0)
		_TintAmount("Tint Amount", Range (0, 1)) = 0
	}
		SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _BumpMap;
		sampler2D _EmissionMap;
		sampler2D _OcclusionMap;
		sampler2D _Metallic;

		struct Input
		{
			float2 uv_MainTex;
			float2 uv_BumpMap;
			float2 uv_Metallic;
		};

		half _Glossiness;
		float _Tint;
		float _TintAmount;
		fixed4 _EmissionColor;
		fixed4 _Color;
		fixed4 _TintColor;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			// Albedo comes from a texture tinted by color
			fixed4 occ = tex2D(_OcclusionMap, IN.uv_MainTex);
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color * occ;
			if (_Tint != 0)
				c = c + _TintColor * _TintAmount;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = tex2D(_Metallic, IN.uv_Metallic);
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
			o.Emission = tex2D(_EmissionMap, IN.uv_MainTex).rgb * _EmissionColor.rgb;
		}
		ENDCG
	}
		FallBack "Diffuse"
}
