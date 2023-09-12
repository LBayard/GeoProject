Shader "World Political Map 2D/Unlit Texture 16K" {
 
Properties {
    _Color ("Color", Color) = (1,1,1)
	_TexTL ("Tex TL", 2D) = "white" {}
	_TexTR ("Tex TR", 2D) = "white" {}
	_TexBL ("Tex BL", 2D) = "white" {}
	_TexBR ("Tex BR", 2D) = "white" {}
}
 
SubShader {
  	Tags { "RenderType"="Opaque" }
		Offset 50,50
		Pass {
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest
				
				#include "UnityCG.cginc"
				
				sampler2D _TexTL;
				sampler2D _TexTR;
				sampler2D _TexBL;
				sampler2D _TexBR;

        
				struct appdata {
					float4 vertex : POSITION;
					float2 texcoord: TEXCOORD0;
					UNITY_VERTEX_INPUT_INSTANCE_ID
				};
				
				struct v2f {
					float4 pos : SV_POSITION;
					float2 uv : TEXCOORD0;
					UNITY_VERTEX_INPUT_INSTANCE_ID
					UNITY_VERTEX_OUTPUT_STEREO
				};
        
				v2f vert (appdata v) {
					v2f o;
					UNITY_SETUP_INSTANCE_ID(v);
					UNITY_INITIALIZE_OUTPUT(v2f, o);
					UNITY_TRANSFER_INSTANCE_ID(v, o);
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
					o.pos = UnityObjectToClipPos(v.vertex);
                    // Push back
                    #if UNITY_REVERSED_Z
                        o.pos.z -= 0.0005;
                    #else
                        o.pos.z += 0.0005;
                    #endif
					o.uv				= v.texcoord;
					return o;
				 }
				
                fixed4 frag (v2f i) : SV_Target {
					UNITY_SETUP_INSTANCE_ID(i);
					UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
					fixed4 color;
                    float mip = -8;
                    // compute Earth pixel color
                    if (i.uv.x<0.5) {
                        if (i.uv.y>0.5) {
                            color = tex2Dlod(_TexTL, float4(i.uv.x * 2.0, (i.uv.y - 0.5) * 2.0, 0, mip));
                        } else {
                            color = tex2Dlod(_TexBL, float4(i.uv.x * 2.0, i.uv.y * 2.0, 0, mip));
                        }
                    } else {
                        if (i.uv.y>0.5) {
                            color = tex2Dlod(_TexTR, float4((i.uv.x - 0.5) * 2.0f, (i.uv.y - 0.5) * 2.0, 0, mip));
                        } else {
                            color = tex2Dlod(_TexBR, float4((i.uv.x - 0.5) * 2.0f, i.uv.y * 2.0, 0, mip));
                        }
                    }
                    return color;
                }
			
			ENDCG
		}
	}
}