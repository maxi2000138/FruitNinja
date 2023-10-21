Shader "Custom/UIBlurSurfaceShader"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" { }
        _BlurAmount ("Blur Amount", Range (0, 20)) = 15.0
        _TintColor ("Tint Color", Color) = (.5, .5, .5, 1)
        _Brightness ("Brightness", Range (0, 10)) = 1.0
        _Intensity ("Intensity", Range(0, 5)) = 1.0
    }

    SubShader
    {
        Tags { "Queue" = "Overlay" }
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
        fixed _Intensity;

        // Функция для вычисления весов Гауссовского размытия
        fixed GaussianWeight(fixed x, fixed sigma)
        {
            return exp(-(x * x) / (2 * sigma * sigma));
        }

        // Гауссовское размытие с двухпроходным подходом
        fixed4 GaussianBlur(sampler2D tex, float2 uv, float2 texelSize)
        {
            fixed4 colorSum = 0;

            // Стандартное отклонение Гауссиана
            fixed sigma = _BlurAmount;

            // Вычисление весов Гауссовского размытия для горизонтального прохода
            fixed weightsH[201];
            fixed totalWeightH = 0;
            for (int i = 0; i < 201; i++)
            {
                weightsH[i] = GaussianWeight(fixed(i) - 100, sigma);
                totalWeightH += weightsH[i];
            }

            // Горизонтальный проход
            for (int i = 0; i < 201; i++)
            {
                fixed2 offsetH = texelSize * (fixed(i) - 100);
                colorSum += tex2D(tex, uv + offsetH) * (weightsH[i] / totalWeightH);
            }

            // Вычисление весов Гауссовского размытия для вертикального прохода
            fixed weightsV[201];
            fixed totalWeightV = 0;
            for (int i = 0; i < 201; i++)
            {
                weightsV[i] = GaussianWeight(fixed(i) - 100, sigma);
                totalWeightV += weightsV[i];
            }

            // Вертикальный проход
            colorSum = 0;
            for (int i = 0; i < 201; i++)
            {
                fixed2 offsetV = texelSize * (fixed(i) - 100);
                colorSum += tex2D(tex, uv + offsetV) * (weightsV[i] / totalWeightV);
            }

            return colorSum;
        }

        void surf(Input IN, inout SurfaceOutput o)
        {
            fixed4 col = tex2D(_MainTex, IN.uv_MainTex);
            fixed2 texelSize = 1.0 / _ScreenParams.xy;

            fixed4 sum = GaussianBlur(_MainTex, IN.uv_MainTex, texelSize);

            // Применение цвета, яркости и интенсивности
            col.rgb = lerp(col.rgb, sum.rgb, _Intensity) * _TintColor.rgb * _Brightness;
            col.a = sum.a;

            o.Albedo = col.rgb;
            o.Alpha = col.a;
        }
        ENDCG
    }
}
