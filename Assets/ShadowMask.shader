Shader"Custom/ObscureVision"
{
    Properties
    {
        _Radius ("Radius", Range(0, 1)) = 0.3 // Controls the size of the visible area
        _PlayerPosition ("Player Screen Position", Vector) = (0.5, 0.5, 0, 0) // Normalized screen position
        _DarkColor ("Dark Color", Color) = (0, 0, 0, 1) // Color for the obscured area, with full alpha
    }

    SubShader
    {
        Tags { "Queue" = "Overlay" "RenderType" = "Transparent" } // Make sure it renders last
        LOD 100

        Cull Off

        Pass
        {
            ZWrite Off // We disable depth writing to ensure transparency is handled properly

            Blend SrcAlpha
            OneMinusSrcAlpha // Enable alpha blending for transparency

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            // Include necessary shader functions
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

            // Shader properties
            float4 _PlayerPosition; // Player's screen position (normalized)
            float _Radius; // Radius of the visible circle
            float4 _DarkColor; // Color for the obscured areas (black)

            // Vertex function
            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            // Fragment shader (pixel-level calculation)
            fixed4 frag(v2f i) : SV_Target
            {
                            // Calculate the distance from each pixel to the player's position
                float distanceFromPlayer = distance(i.uv, _PlayerPosition.xy);

                            // If the pixel is outside the visible radius, fully obscure it
                if (distanceFromPlayer > _Radius)
                {
                    return _DarkColor; // Apply the dark color (solid black with full alpha)
                }
                else
                {
                                // Return transparent for the visible circle around the player
                    return fixed4(0, 0, 0, 0); // Transparent
                }
            }
            ENDCG
        }
    }
    
    // Fallback to ensure shader works correctly in case of failure
    FallBack"Unlit/Transparent" 
}
