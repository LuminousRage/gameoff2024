Shader "Custom/TextureWrapper"
{
    Properties
    {
		_MainTex("Texture", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        struct Input
        {
			float2 uv_MainTex;
        };

        fixed4 _NewColor;
		sampler2D _MainTex;
		

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float2 offset = float2(_Time.y, 0);
            float2 target_uv = IN.uv_MainTex + offset;

            target_uv.x %= 1;


			float4 colour = tex2D(_MainTex, target_uv);

			o.Albedo = colour.rgb;
			o.Alpha = colour.a;

        }
        ENDCG
    }
    FallBack "Diffuse"
}