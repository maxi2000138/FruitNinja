Shader "Custom/SoftBlur"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1, 1, 1, 1)
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
        _BlurSize("Blur Size", Range(0.0, 0.1)) = 0.05
        _TintColor ("Tint Color", Color) = (1, 1, 1, 1)
    }

    SubShader
    {
        Tags
        { 
            "Queue"="Overlay" 
            "RenderType"="Transparent" 
            "IgnoreProjector"="True" 
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ PIXELSNAP_ON
            #include "UnityCG.cginc"
            
            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos      : POSITION;
                fixed4 color    : COLOR;
                float2 uv       : TEXCOORD0;
            };
            
            fixed4 _Color;
            fixed4 _TintColor;

            v2f vert(appdata_t IN)
            {
                v2f OUT;
                OUT.pos = UnityObjectToClipPos(IN.vertex);
                OUT.uv = IN.texcoord;
                OUT.color = IN.color * _Color;
                #ifdef PIXELSNAP_ON
                OUT.pos = UnityPixelSnap (OUT.pos);
                #endif

                return OUT;
            }

            sampler2D _MainTex;
            float _BlurSize;

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 sum = fixed4(0.0, 0.0, 0.0, 0.0);
                float weightSum = 0.0;

                // Gaussian weights for softer blur
                float weights[9] = {0.03, 0.07, 0.12, 0.18, 0.2, 0.18, 0.12, 0.07, 0.03};

                for (int j = -4; j <= 4; j++)
                {
                    float weight = weights[j + 4];
                    sum += tex2D(_MainTex, i.uv + float2(0, j * _BlurSize)) * weight;
                    weightSum += weight;
                }

                sum /= weightSum;

                // Apply tint color
                sum.rgb *= _TintColor.rgb;

                return sum;
            }
            ENDCG
        }
    }
}
