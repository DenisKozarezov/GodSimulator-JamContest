Shader "Sprites/Outline2D"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _OutlineWidth("Outline Width", Range(0, 15)) = 1
        _OutlineColor("Outline Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
            "PreviewType" = "Plane"
            "CanUseSpriteAtlas" = "True"
        }
        LOD 100

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 texcoord : TEXCOORD0;
            };

            half _OutlineWidth;
            fixed4 _OutlineColor;
            sampler2D _MainTex;
            float4 _MainTex_TexelSize;

            v2f vert (appdata_t IN)
            {
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.texcoord = IN.texcoord;
                return OUT;
            }

            fixed4 frag(v2f IN) : COLOR
            {
                fixed4 c = tex2D(_MainTex, IN.texcoord);
                c.rgb *= c.a;
                half4 outlineC = _OutlineColor;
                outlineC.rgb *= outlineC.a;                

                if (_OutlineWidth > 0 && c.a != 0)
                {
                    fixed pixelUp = tex2D(_MainTex, IN.texcoord + fixed2(0, _MainTex_TexelSize.y) * _OutlineWidth).a;
                    fixed pixelDown = tex2D(_MainTex, IN.texcoord - fixed2(0, _MainTex_TexelSize.y) * _OutlineWidth).a;
                    fixed pixelRight = tex2D(_MainTex, IN.texcoord + fixed2(_MainTex_TexelSize.x, 0) * _OutlineWidth).a;
                    fixed pixelLeft = tex2D(_MainTex, IN.texcoord - fixed2(_MainTex_TexelSize.x, 0) * _OutlineWidth).a;
                    return lerp(outlineC, c, ceil(pixelUp * pixelDown * pixelRight * pixelLeft));
                }
                else return c;
            }
            ENDCG
        }
    }
}
