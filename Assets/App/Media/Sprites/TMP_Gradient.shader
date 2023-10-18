Shader "Custom/TextGradientShader"
{
    Properties
    {
        _MainTex ("Font Texture", 2D) = "white" { }
        _Color ("Text Color", Color) = (.5,.5,.5,1)
        _GradientStartColor ("Gradient Start Color", Color) = (1, 0, 0, 1)
        _GradientMidColor ("Gradient Mid Color", Color) = (0, 1, 0, 1)
        _GradientEndColor ("Gradient End Color", Color) = (0, 0, 1, 1)
        _Angle ("Gradient Angle", Range(0, 360)) = 45.0
    }

    SubShader
    {
        Tags { "Queue" = "Overlay" }
        LOD 100

        CGPROGRAM
        #pragma surface surf Lambert

        struct Input
        {
            float2 uv_MainTex;
        };

        sampler2D _MainTex;
        fixed _Angle;
        fixed4 _Color;
        fixed4 _GradientStartColor;
        fixed4 _GradientMidColor;
        fixed4 _GradientEndColor;

        void surf(Input IN, inout SurfaceOutput o)
        {
            // ... ваш код текстуры ...
            o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb * _Color.rgb;
            
            // Calculate gradient color based on angle
            float angle = _Angle * 0.0174533; // Convert degrees to radians
            float2 dir = normalize(float2(cos(angle), sin(angle)));
            float4 uv = float4(IN.uv_MainTex, 0, 1); // Добавляем z и w компоненты
            float2 gradientDir = float2(cos(angle + 1.5708), sin(angle + 1.5708)); // 1.5708 is 90 degrees in radians
            float gradient = dot(uv.xy, gradientDir);

            // Interpolate between colors based on gradient
            float4 gradientColor = lerp(_GradientStartColor, _GradientMidColor, saturate(gradient));
            o.Albedo = lerp(o.Albedo, gradientColor.rgb, saturate(gradient));
        }
        ENDCG
    }
}
