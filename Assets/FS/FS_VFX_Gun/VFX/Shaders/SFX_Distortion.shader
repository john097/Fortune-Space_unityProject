// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "QFX/SFX/Distortion"
{
	Properties
	{
		[HDR]_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
		_MainTex ("Particle Texture", 2D) = "white" {}
		_InvFade ("Soft Particles Factor", Range(0.01,3.0)) = 1.0
		_CutOutA("CutOut (A)", 2D) = "white" {}
		_DistortionTexture("Distortion Texture", 2D) = "bump" {}
		_Scale("Scale", Range( 0 , 4)) = 0
		_Distortion("Distortion", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
	}

	Category 
	{
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane"  }

		
		SubShader
		{
			Blend SrcAlpha OneMinusSrcAlpha
			ColorMask RGB
			Cull Off
			Lighting Off 
			ZWrite Off
			GrabPass{ }

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
					float4 ase_texcoord3 : TEXCOORD3;
				};
				
				uniform sampler2D _MainTex;
				uniform fixed4 _TintColor;
				uniform float4 _MainTex_ST;
				uniform sampler2D_float _CameraDepthTexture;
				uniform float _InvFade;
				uniform sampler2D _GrabTexture;
				uniform float _Distortion;
				uniform sampler2D _DistortionTexture;
				uniform float4 _DistortionTexture_ST;
				uniform float _Scale;
				uniform sampler2D _CutOutA;
				uniform float4 _CutOutA_ST;
						inline float4 ASE_ComputeGrabScreenPos( float4 pos )
						{
							#if UNITY_UV_STARTS_AT_TOP
							float scale = -1.0;
							#else
							float scale = 1.0;
							#endif
							float4 o = pos;
							o.y = pos.w * 0.5f;
							o.y = ( pos.y - o.y ) * _ProjectionParams.x * scale + o.y;
							return o;
						}
				

				v2f vert ( appdata_t v  )
				{
					v2f o;
					UNITY_SETUP_INSTANCE_ID(v);
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
					float4 clipPos = UnityObjectToClipPos(v.vertex);
					float4 screenPos = ComputeScreenPos(clipPos);
					o.ase_texcoord3 = screenPos;
					

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

					float2 uv_MainTex = i.texcoord * _MainTex_ST.xy + _MainTex_ST.zw;
					float4 temp_output_4_0 = ( _TintColor * tex2D( _MainTex, uv_MainTex ) );
					float2 uv_DistortionTexture = i.texcoord * _DistortionTexture_ST.xy + _DistortionTexture_ST.zw;
					float4 screenPos = i.ase_texcoord3;
					float4 ase_grabScreenPos = ASE_ComputeGrabScreenPos( screenPos );
					float4 ase_grabScreenPosNorm = ase_grabScreenPos / ase_grabScreenPos.w;
					float4 screenColor1 = tex2D( _GrabTexture, ( float4( ( _Distortion * UnpackNormal( tex2D( _DistortionTexture, (uv_DistortionTexture*_Scale + 0.0) ) ) ) , 0.0 ) + ase_grabScreenPosNorm ).xy );
					
					float2 uv_CutOutA = i.texcoord * _CutOutA_ST.xy + _CutOutA_ST.zw;
					

					fixed4 col = ( temp_output_4_0 + screenColor1 );
					col.a = ( _TintColor.a * tex2D( _CutOutA, uv_CutOutA ).a );
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
12;447;1899;586;2874.834;447.8848;2.521678;True;False
Node;AmplifyShaderEditor.RangedFloatNode;15;-1506.549,211.7213;Float;False;Property;_Scale;Scale;2;0;Create;0;4;0;4;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;16;-1492.031,84.10633;Float;False;0;6;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScaleAndOffsetNode;13;-1209.002,194.1268;Float;False;3;0;FLOAT2;0,0;False;1;FLOAT;1.0;False;2;FLOAT;0.0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-966.2645,62.80555;Float;False;Property;_Distortion;Distortion;3;0;Create;0;0.01;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;6;-984.8704,166.3647;Float;True;Property;_DistortionTexture;Distortion Texture;1;0;Create;None;a268ab862991c4743a9281c69bb2c36a;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TemplateShaderPropertyNode;3;-1051.247,-189.7433;Float;False;_MainTex;0;5;SAMPLER2D;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GrabScreenPosition;23;-927.5345,394.7149;Float;False;0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;-679.986,113.4676;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT3;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;5;-759.0173,-193.2219;Float;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;22;-486.1479,114.956;Float;False;2;2;0;FLOAT3;0.0,0,0,0;False;1;FLOAT4;0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TemplateShaderPropertyNode;2;-699.7006,-380.6992;Float;False;_TintColor;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;26;-543.074,500.35;Float;True;Property;_CutOutA;CutOut (A);0;0;Create;None;6c3d1d97f81c782499976d55869ba522;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;-337.9227,-207.3038;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ScreenColorNode;1;-343.4307,106.2691;Float;False;Global;_GrabScreen0;Grab Screen 0;0;0;Create;Object;-1;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;-143.6791,-17.35193;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;29;-231.1533,383.9169;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;30;-126.6422,-136.8045;Float;False;2;2;0;COLOR;0.0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMasterNode;25;35.88515,-18.02395;Float;False;True;2;Float;ASEMaterialInspector;0;6;QFX/SFX/Distortion;2f9571191a522e14fb54d07a8dd014f9;QFX/SFX/Templates/Particles Alpha Blended Alpha Source;2;SrcAlpha;OneMinusSrcAlpha;0;One;One;Off;True;True;True;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT;0.0;False;2;FLOAT3;0,0,0;False;0
WireConnection;13;0;16;0
WireConnection;13;1;15;0
WireConnection;6;1;13;0
WireConnection;9;0;8;0
WireConnection;9;1;6;0
WireConnection;5;0;3;0
WireConnection;22;0;9;0
WireConnection;22;1;23;0
WireConnection;4;0;2;0
WireConnection;4;1;5;0
WireConnection;1;0;22;0
WireConnection;24;0;4;0
WireConnection;24;1;1;0
WireConnection;29;0;2;4
WireConnection;29;1;26;4
WireConnection;30;0;4;0
WireConnection;30;1;1;0
WireConnection;25;0;30;0
WireConnection;25;1;29;0
ASEEND*/
//CHKSM=E96ECEC5C90235C801D7E5335568D3A1EF552289