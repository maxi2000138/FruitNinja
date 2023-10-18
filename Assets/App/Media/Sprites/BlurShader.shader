Shader "Custom/UIBlurSurfaceShader"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" { }
        _BlurAmount ("Blur Amount", Range (0, 0.01)) = 2.0
        _TintColor ("Tint Color", Color) = (.5,.5,.5,1)
        _Brightness ("Brightness", Range (0, 10)) = 1.0
    }

    SubShader
    {
        Tags {"Queue" = "Overlay" }
        CGPROGRAM
        #pragma surface surf Lambert

        struct Input
        {
            float2 uv_MainTex;
        };

        sampler2D _MainTex;
        fixed _BlurAmount;
        fixed4 _TintColor;
        fixed _Brightness;

        void surf(Input IN, inout SurfaceOutput o)
        {
            // Размытие по горизонтали и вертикали
            fixed4 col = tex2D(_MainTex, IN.uv_MainTex);
            for (int i = 1; i <= 5; i++) {
                col += tex2D(_MainTex, IN.uv_MainTex + float2(i * _BlurAmount, 0));
                col += tex2D(_MainTex, IN.uv_MainTex - float2(i * _BlurAmount, 0));
                col += tex2D(_MainTex, IN.uv_MainTex + float2(0, i * _BlurAmount));
                col += tex2D(_MainTex, IN.uv_MainTex - float2(0, i * _BlurAmount));
            }
            col /= 21.0;

            // Применение цвета и яркости
            col.rgb = col.rgb * _TintColor.rgb * _Brightness;

            o.Albedo = col.rgb;
            o.Alpha = col.a;
        }
        ENDCG
    }
}
