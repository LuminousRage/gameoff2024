Shader "Custom/ColorSwapper"
{
    Properties
    {
        _NewColor ("New Color", Color) = (0.8, 0.53, 0.6, 1)
		_MainTex("Texture", 2D) = "white" {}
        _Transparency ("Transparency", Range(0, 1)) = 1.0
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows alpha:fade

        #pragma target 3.0

        struct Input
        {
            float2 uv_MainTex;
        };

        fixed4 _NewColor;
        sampler2D _MainTex;
        float _Transparency;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float4 originalColour = tex2D(_MainTex, IN.uv_MainTex);

            if (originalColour.b >= 0.98){
                o.Albedo = _NewColor.rgb;
            } else {
                o.Albedo = originalColour.rgb;
            }

            o.Alpha = _Transparency;
        }
        ENDCG
    }
    FallBack "Transparent"
}