// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "QFX/SFX/Sparks"
{
	Properties
	{
		[HDR]_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
		_MainTex ("Particle Texture", 2D) = "white" {}
		_InvFade ("Soft Particles Factor", Range(0.01,3.0)) = 1.0
		[HDR]_ColorColor("Color Color", Color) = (1,0.6993915,0.4264706,1)
		[HDR]_ColorEdge("Color Edge", Color) = (1,0.9801217,0.2794118,1)
		_Mix("Mix", Range( 0 , 1)) = 100
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
	}

	Category 
	{
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane"  }

		
		SubShader
		{
			Blend One One
			ColorMask RGB
			Cull Off
			Lighting Off 
			ZWrite Off

			Pass {
			
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma target 2.0
				#pragma multi_compile_particles
				#pragma multi_compile_fog


				#include "UnityCG.cginc"

				struct appdata_t 
				{
					float4 vertex : POSITION;
					fixed4 color : COLOR;
					float4 texcoord : TEXCOORD0;
					float3 velocity : TEXCOORD1;
					UNITY_VERTEX_INPUT_INSTANCE_ID
				};

				struct v2f 
				{
					float4 vertex : SV_POSITION;
					fixed4 color : COLOR;
					float4 texcoord : TEXCOORD0;
					UNITY_FOG_COORDS(1)
					#ifdef SOFTPARTICLES_ON
					float4 projPos : TEXCOORD2;
					#endif
					UNITY_VERTEX_OUTPUT_STEREO
				};
				
				uniform sampler2D _MainTex;
				uniform fixed4 _TintColor;
				uniform float4 _MainTex_ST;
				uniform sampler2D_float _CameraDepthTexture;
				uniform float _InvFade;
				uniform float4 _ColorEdge;
				uniform float4 _ColorColor;
				uniform float _Mix;

				v2f vert ( appdata_t v  )
				{
					v2f o;
					UNITY_SETUP_INSTANCE_ID(v);
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

					v.vertex.xyz +=  float3( 0, 0, 0 ) ;
					o.vertex = UnityObjectToClipPos(v.vertex);
					#ifdef SOFTPARTICLES_ON
						o.projPos = ComputeScreenPos (o.vertex);
						COMPUTE_EYEDEPTH(o.projPos.z);
					#endif
					o.color = v.color;
					o.texcoord = v.texcoord;
					o.texcoord.xy = TRANSFORM_TEX(v.texcoord,_MainTex);
					UNITY_TRANSFER_FOG(o,o.vertex);
					return o;
				}

				fixed4 frag ( v2f i  ) : SV_Target
				{
					#ifdef SOFTPARTICLES_ON
						float sceneZ = LinearEyeDepth (SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)));
						float partZ = i.projPos.z;
						float fade = saturate (_InvFade * (sceneZ-partZ));
						i.color.a *= fade;
					#endif

					float4 lerpResult11 = lerp( _ColorEdge , _ColorColor , 0.7);
					float2 uv_MainTex = i.texcoord * _MainTex_ST.xy + _MainTex_ST.zw;
					float4 tex2DNode3 = tex2D( _MainTex, uv_MainTex );
					float lerpResult2 = lerp( tex2DNode3.r , tex2DNode3.g , _Mix);
					

					fixed4 col = ( i.color * ( lerpResult11 * lerpResult2 ) * _TintColor );
					col.a = _TintColor.a;
					UNITY_APPLY_FOG(i.fogCoord, col);
					return col;
				}
				ENDCG 
			}
		}	
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=14201
7;646;1904;387;1166.016;230.8227;1;True;False
Node;AmplifyShaderEditor.TemplateShaderPropertyNode;1;-933.2864,-203.662;Float;True;_MainTex;0;5;SAMPLER2D;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;3;-667.9739,-205.2309;Float;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;12;-301.8963,-520.7354;Float;False;Constant;_Float0;Float 0;3;0;Create;0.7;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;10;-358.1793,-688.8041;Float;False;Property;_ColorColor;Color Color;0;1;[HDR];Create;1,0.6993915,0.4264706,1;1.286765,3.770791,5,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;9;-347.8384,-879.3872;Float;False;Property;_ColorEdge;Color Edge;1;1;[HDR];Create;1,0.9801217,0.2794118,1;1.375,2.768966,5.5,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;4;-651.5112,22.01427;Float;False;Property;_Mix;Mix;2;0;Create;100;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;2;-296.8578,-178.218;Float;True;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;11;-18.7881,-708.2486;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.VertexColorNode;15;184.2965,-477.7604;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;129.9478,-200.7994;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateShaderPropertyNode;27;146.0878,99.50719;Float;False;_TintColor;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;8;-542.9359,-454.0258;Float;True;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;1,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.VertexToFragmentNode;26;-1092.098,-560.1993;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TemplateVertexDataNode;25;-1252.339,-560.1991;Float;False;velocity;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleDivideOpNode;6;-890.573,-454.3583;Float;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0.0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;407.1189,-223.1144;Float;True;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMasterNode;24;691.2996,-220.9645;Float;False;True;2;Float;ASEMaterialInspector;0;7;QFX/SFX/Sparks;86b8b41743f1e744e92179f38c8256f5;QFX/SFX/Templates/Particles Alpha Blended Velocity Vertex Data;4;One;One;0;One;Zero;Off;True;True;True;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT;0.0;False;2;FLOAT3;0,0,0;False;0
WireConnection;3;0;1;0
WireConnection;2;0;3;1
WireConnection;2;1;3;2
WireConnection;2;2;4;0
WireConnection;11;0;9;0
WireConnection;11;1;10;0
WireConnection;11;2;12;0
WireConnection;13;0;11;0
WireConnection;13;1;2;0
WireConnection;8;0;6;0
WireConnection;26;0;25;0
WireConnection;6;0;26;0
WireConnection;14;0;15;0
WireConnection;14;1;13;0
WireConnection;14;2;27;0
WireConnection;24;0;14;0
WireConnection;24;1;27;4
ASEEND*/
//CHKSM=66302A13302791C2498442A488335B2268C2804F