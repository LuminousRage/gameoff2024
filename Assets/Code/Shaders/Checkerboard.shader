Shader "Custom/Checkerboard"
{
    Properties
    {
		// Default purple
        _Color1 ("Color", Color) = (0.8, 0.53, 0.6, 1)

		// Default green
        _Color2 ("Color", Color) = (0.66, 1, 0, 1)

		_BlockSize("Block Size", Range(1, 10)) = 1
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
			float3 worldPos;
        };

        fixed4 _Color1;
        fixed4 _Color2;
		int _BlockSize;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
			int sum = 0;

			IN.worldPos += 0.5;	

			sum += abs(floor(IN.worldPos.x + 0.5)) / _BlockSize;
			sum += abs(floor(IN.worldPos.y + 0.5)) / _BlockSize;
			sum += abs(floor(IN.worldPos.z + 0.5)) / _BlockSize;

            bool useAlternateColour = sum % 2;

			fixed4 outColor = _Color1;

			if (useAlternateColour){
				outColor = _Color1;
			} else {
				outColor = _Color2;
			}

            o.Albedo = outColor.rgb;
            o.Alpha = outColor.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
