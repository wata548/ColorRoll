Shader "Custom/SurfacePainter"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color1("BrushColor1", Color) = (1, 0, 0, 1)
        _Pos1("BrushPos1", Vector) = (1, 1, 0, 0)
        _Radius1("Radius1", Float) = 0.01

        _Color2("BrushColor2", Color) = (0, 0, 1, 1)
        _Pos2("BrushPos2", Vector) = (1, 1, 0, 0)
        _Radius2("Radius2", Float) = 0.01
    }
    SubShader
    {
        Pass
        {
            ZTest Always
            Cull Off
            ZWrite Off
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float _Radius1;
            float2 _Pos1;
            float4 _Color1;
            
            float _Radius2;
            float2 _Pos2;
            float4 _Color2;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float alpha1 = saturate(1 - length(i.uv - _Pos1) / _Radius1);
                float alpha2 = saturate(1 - length(i.uv - _Pos2) / _Radius2);
                fixed4 curColor = tex2D(_MainTex, i.uv);
                fixed4 result;
                if (alpha1 < alpha2) {
                    result = lerp(curColor, _Color2, alpha2);
                }
                else 
                    result = lerp(curColor, _Color1, alpha1);
                return result;
            }
            ENDCG
        }
    }
}