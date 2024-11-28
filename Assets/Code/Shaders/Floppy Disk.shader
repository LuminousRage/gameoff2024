Shader "Custom/Checkerboard"
{
    Properties
    {
        _FloppyColor ("Floppy Disk Color", Color) = (0.8, 0.53, 0.6, 1)
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

        fixed4 _FloppyColor;
		sampler2D _MainTex;
		

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
			float4 originalColour = tex2D(_MainTex, IN.uv_MainTex);

			if (originalColour.b >= 0.98){
				o.Albedo = _FloppyColor.rgb;
				o.Alpha = _FloppyColor.a;
			} else {
				o.Albedo = originalColour.rgb;
				o.Alpha = originalColour.a;
			}

        }
        ENDCG
    }
    FallBack "Diffuse"
}
