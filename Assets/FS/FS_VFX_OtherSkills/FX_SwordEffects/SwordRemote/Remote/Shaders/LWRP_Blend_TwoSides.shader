// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ERB/LWRP/Particles/Blend_TwoSides"
{
    Properties
    {
		_Cutoff("Mask Clip Value", Float) = 0.5
		_MainTex("Main Tex", 2D) = "white" {}
		_Mask("Mask", 2D) = "white" {}
		_Noise("Noise", 2D) = "white" {}
		_SpeedMainTexUVNoiseZW("Speed MainTex U/V + Noise Z/W", Vector) = (0,0,0,0)
		_FrontFacesColor("Front Faces Color", Color) = (0,0.2313726,1,1)
		_BackFacesColor("Back Faces Color", Color) = (0.1098039,0.4235294,1,1)
		_Emission("Emission", Float) = 2
		[Toggle]_UseFresnel("Use Fresnel?", Float) = 0
		[Toggle]_SeparateFresnel("SeparateFresnel", Float) = 0
		_SeparateEmission("Separate Emission", Float) = 2
		_FresnelColor("Fresnel Color", Color) = (1,1,1,1)
		_Fresnel("Fresnel", Float) = 1
		_FresnelEmission("Fresnel Emission", Float) = 1
		[Toggle]_UseCustomData("Use Custom Data?", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
    }

    SubShader
    {
		

        Tags { "RenderPipeline"="LightweightPipeline" "RenderType"="TransparentCutout" "Queue"="Transparent" "PreviewType"="Plane" }
        Cull Off
		HLSLINCLUDE
		#pragma target 3.0
		ENDHLSL

		
        Pass
        {
            Tags { "LightMode"="LightweightForward" }
            Name "Base"

            Blend SrcAlpha OneMinusSrcAlpha
			ZWrite On
			ZTest LEqual
			Offset 0,0
			ColorMask RGBA
			

            HLSLPROGRAM
            #define _RECEIVE_SHADOWS_OFF 1

            #pragma prefer_hlslcc gles
            #pragma shader_feature _SAMPLE_GI

            // -------------------------------------
            // Unity defined keywords
			#ifdef ASE_FOG
            #pragma multi_compile_fog
			#endif

            #pragma multi_compile_instancing
            
            #pragma vertex vert
            #pragma fragment frag

            #define ASE_SRP_VERSION 50702
            #define _AlphaClip 1

            #include "Packages/com.unity.render-pipelines.lightweight/ShaderLibrary/Core.hlsl"
        	#include "Packages/com.unity.render-pipelines.lightweight/ShaderLibrary/Lighting.hlsl"
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/UnityInstancing.hlsl"
        	#include "Packages/com.unity.render-pipelines.lightweight/ShaderLibrary/ShaderGraphFunctions.hlsl"

			float _SeparateFresnel;
			float _UseFresnel;
			float4 _FrontFacesColor;
			float _Fresnel;
			float _FresnelEmission;
			float4 _FresnelColor;
			float4 _BackFacesColor;
			float _Emission;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _SpeedMainTexUVNoiseZW;
			float _SeparateEmission;
			sampler2D _Mask;
			float4 _Mask_ST;
			sampler2D _Noise;
			float4 _Noise_ST;
			float _UseCustomData;
			float _Cutoff;

            struct GraphVertexInput
            {
                float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct GraphVertexOutput
            {
                float4 position : POSITION;
				#ifdef ASE_FOG
				float fogCoord : TEXCOORD0;
				#endif
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_color : COLOR;
				float4 ase_texcoord3 : TEXCOORD3;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO
            };
			
            GraphVertexOutput vert (GraphVertexInput v)
            {
                GraphVertexOutput o = (GraphVertexOutput)0;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				float3 ase_worldPos = mul(GetObjectToWorldMatrix(), v.vertex).xyz;
				o.ase_texcoord1.xyz = ase_worldPos;
				float3 ase_worldNormal = TransformObjectToWorldNormal(v.ase_normal);
				o.ase_texcoord2.xyz = ase_worldNormal;
				
				o.ase_color = v.ase_color;
				o.ase_texcoord3 = v.ase_texcoord;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord1.w = 0;
				o.ase_texcoord2.w = 0;
				float3 vertexValue =  float3( 0, 0, 0 ) ;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
				v.vertex.xyz = vertexValue; 
				#else
				v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal =  v.ase_normal ;
                o.position = TransformObjectToHClip(v.vertex.xyz);
				#ifdef ASE_FOG
				o.fogCoord = ComputeFogFactor( o.position.z );
				#endif
                return o;
            }

            half4 frag (GraphVertexOutput IN ) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);
				float3 ase_worldPos = IN.ase_texcoord1.xyz;
				float3 ase_worldViewDir = ( _WorldSpaceCameraPos.xyz - ase_worldPos );
				ase_worldViewDir = normalize(ase_worldViewDir);
				float3 ase_worldNormal = IN.ase_texcoord2.xyz;
				float fresnelNdotV95 = dot( ase_worldNormal, ase_worldViewDir );
				float fresnelNode95 = ( 0.0 + 1.0 * pow( abs(1.0 - fresnelNdotV95), _Fresnel ) );
				float dotResult87 = dot( ase_worldNormal , ase_worldViewDir );
				float4 lerpResult91 = lerp( lerp(_FrontFacesColor,( ( _FrontFacesColor * ( 1.0 - fresnelNode95 ) ) + ( _FresnelEmission * _FresnelColor * fresnelNode95 ) ),_UseFresnel) , _BackFacesColor , (1.0 + (sign( dotResult87 ) - -1.0) * (0.0 - 1.0) / (1.0 - -1.0)));
				float2 uv0_MainTex = IN.ase_texcoord3.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				float2 appendResult21 = (float2(_SpeedMainTexUVNoiseZW.x , _SpeedMainTexUVNoiseZW.y));
				float4 tex2DNode105 = tex2D( _MainTex, ( uv0_MainTex + ( appendResult21 * _Time.y ) ) );
				
				float2 uv_Mask = IN.ase_texcoord3.xy * _Mask_ST.xy + _Mask_ST.zw;
				float4 uv0_Noise = IN.ase_texcoord3;
				uv0_Noise.xy = IN.ase_texcoord3.xy * _Noise_ST.xy + _Noise_ST.zw;
				float2 appendResult22 = (float2(_SpeedMainTexUVNoiseZW.z , _SpeedMainTexUVNoiseZW.w));
				float4 _Vector0 = float4(0,1,-19,20);
				float4 temp_cast_1 = (_Vector0.x).xxxx;
				float4 temp_cast_2 = (_Vector0.y).xxxx;
				float4 temp_cast_3 = (_Vector0.z).xxxx;
				float4 temp_cast_4 = (_Vector0.w).xxxx;
				float4 clampResult135 = clamp( (temp_cast_3 + (( tex2D( _Mask, uv_Mask ) * tex2D( _Noise, ( (uv0_Noise).xy + ( _Time.y * appendResult22 ) + uv0_Noise.w ) ) * lerp(1.0,uv0_Noise.z,_UseCustomData) ) - temp_cast_1) * (temp_cast_4 - temp_cast_3) / (temp_cast_2 - temp_cast_1)) , float4( 0,0,0,0 ) , float4( 1,1,1,1 ) );
				
		        float3 Color = lerp(( lerpResult91 * _Emission * IN.ase_color * tex2DNode105 * IN.ase_color.a ),( ( lerpResult91 + ( _FresnelColor * tex2DNode105 * _SeparateEmission ) ) * _Emission * IN.ase_color * IN.ase_color.a ),_SeparateFresnel).rgb;
		        float Alpha = clampResult135.r;
		        float AlphaClipThreshold = _Cutoff;
         #if _AlphaClip
                clip(Alpha - AlphaClipThreshold);
        #endif

				#ifdef ASE_FOG
				Color = MixFog( Color, IN.fogCoord );
				#endif
                return half4(Color, Alpha);
            }
            ENDHLSL
        }
		
        Pass
        {
			
            Name "DepthOnly"
            Tags { "LightMode"="DepthOnly" }

            ZWrite On
			ZTest LEqual
			ColorMask 0

            HLSLPROGRAM
            #define _RECEIVE_SHADOWS_OFF 1

            // Required to compile gles 2.0 with standard srp library
            #pragma prefer_hlslcc gles
            #pragma target 2.0

            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing

            #pragma vertex vert
            #pragma fragment frag

            #define ASE_SRP_VERSION 50702
            #define _AlphaClip 1

            #include "Packages/com.unity.render-pipelines.lightweight/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.lightweight/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.lightweight/ShaderLibrary/ShaderGraphFunctions.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

			sampler2D _Mask;
			float4 _Mask_ST;
			sampler2D _Noise;
			float4 _Noise_ST;
			float4 _SpeedMainTexUVNoiseZW;
			float _UseCustomData;
			float _Cutoff;

			struct GraphVertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

            struct VertexOutput
            {
                float4 clipPos : SV_POSITION;
				float4 ase_texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO
            };

			
			VertexOutput vert( GraphVertexInput v  )
			{
					VertexOutput o = (VertexOutput)0;
					UNITY_SETUP_INSTANCE_ID(v);
					UNITY_TRANSFER_INSTANCE_ID(v, o);
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
					o.ase_texcoord = v.ase_texcoord;
					float3 vertexValue =  float3(0,0,0) ;	
					#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
					#else
					v.vertex.xyz += vertexValue;
					#endif
					v.ase_normal =  v.ase_normal ;
					o.clipPos = TransformObjectToHClip(v.vertex.xyz);
					return o;
			}

            half4 frag( VertexOutput IN  ) : SV_TARGET
            {
                UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);
				float2 uv_Mask = IN.ase_texcoord.xy * _Mask_ST.xy + _Mask_ST.zw;
				float4 uv0_Noise = IN.ase_texcoord;
				uv0_Noise.xy = IN.ase_texcoord.xy * _Noise_ST.xy + _Noise_ST.zw;
				float2 appendResult22 = (float2(_SpeedMainTexUVNoiseZW.z , _SpeedMainTexUVNoiseZW.w));
				float4 _Vector0 = float4(0,1,-19,20);
				float4 temp_cast_0 = (_Vector0.x).xxxx;
				float4 temp_cast_1 = (_Vector0.y).xxxx;
				float4 temp_cast_2 = (_Vector0.z).xxxx;
				float4 temp_cast_3 = (_Vector0.w).xxxx;
				float4 clampResult135 = clamp( (temp_cast_2 + (( tex2D( _Mask, uv_Mask ) * tex2D( _Noise, ( (uv0_Noise).xy + ( _Time.y * appendResult22 ) + uv0_Noise.w ) ) * lerp(1.0,uv0_Noise.z,_UseCustomData) ) - temp_cast_0) * (temp_cast_3 - temp_cast_2) / (temp_cast_1 - temp_cast_0)) , float4( 0,0,0,0 ) , float4( 1,1,1,1 ) );
				

				float Alpha = clampResult135.r;
				float AlphaClipThreshold = _Cutoff;

         #if _AlphaClip
        		clip(Alpha - AlphaClipThreshold);
        #endif
                return 0;
            }
            ENDHLSL
        }
		
    }
    Fallback "Hidden/InternalErrorShader"
	
}
/*ASEBEGIN
Version=16900
446;73;948;656;1078.258;709.8981;2.876209;True;False
Node;AmplifyShaderEditor.Vector4Node;15;-1416.635,615.4911;Float;False;Property;_SpeedMainTexUVNoiseZW;Speed MainTex U/V + Noise Z/W;4;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;22;-1052.689,775.7031;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TimeNode;17;-1085.113,644.0539;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;29;-839.9114,656.8193;Float;False;0;14;4;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;23;-836.114,821.5425;Float;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ComponentMaskNode;59;-589.8,683.1187;Float;False;True;True;False;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;27;-264.7057,749.2278;Float;False;3;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;104;-265.9941,869.9862;Float;False;Constant;_Float0;Float 0;13;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;131;-71.56631,960.1508;Float;False;Property;_UseCustomData;Use Custom Data?;14;0;Create;True;0;0;False;0;0;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;14;-117.4146,709.8865;Float;True;Property;_Noise;Noise;3;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;13;-117.0276,524.4487;Float;True;Property;_Mask;Mask;2;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;70;278.0829,449.9118;Float;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.Vector4Node;134;279.941,574.2953;Float;False;Constant;_Vector0;Vector 0;13;0;Create;True;0;0;False;0;0,1,-19,20;0,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;133;527.5223,551.6103;Float;False;5;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;1,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;51;292.3858,257.3008;Float;False;5;5;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;127;-644.2001,-367.2286;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;52;-16.89706,-122.9107;Float;False;Property;_Emission;Emission;7;0;Create;True;0;0;False;0;2;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;135;715.8577,551.5114;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,1;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;105;-120.1585,209.7414;Float;True;Property;_MainTex;Main Tex;1;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;143;-44.30498,400.0178;Float;False;Property;_SeparateEmission;Separate Emission;10;0;Create;True;0;0;False;0;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;91;2.611719,-247.528;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;146;-302.9539,160.1816;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WorldNormalVector;86;-830.5074,-82.69998;Float;False;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WireNode;142;-868.9249,183.4537;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;145;628.366,-28.36714;Float;False;4;4;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;3;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;132;525.4827,727.519;Float;False;Property;_Cutoff;Mask Clip Value;0;0;Create;False;0;0;False;0;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;141;289.5909,107.5137;Float;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;85;-824.4177,57.76734;Float;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DynamicAppendNode;21;-1055.23,553.4234;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.VertexColorNode;32;-46.08871,-45.01809;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ToggleSwitchNode;147;819.7252,252.6802;Float;False;Property;_SeparateFresnel;SeparateFresnel;9;0;Create;True;0;0;False;0;0;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;96;-1323.113,-190.9255;Float;False;Property;_Fresnel;Fresnel;12;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SignOpNode;88;-454.7349,1.959959;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;31;-872.3895,-547.056;Float;False;Property;_FrontFacesColor;Front Faces Color;5;0;Create;True;0;0;False;0;0,0.2313726,1,1;0.5,0.5,0.5,1;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;93;-1135.806,-434.952;Float;False;Property;_FresnelColor;Fresnel Color;11;0;Create;True;0;0;False;0;1,1,1,1;0.5,0.5,0.5,1;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;97;-861.7213,-380.977;Float;False;3;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;126;-870.2397,-235.7427;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;144;472.3789,-151.9794;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;98;-1141.312,-518.009;Float;False;Property;_FresnelEmission;Fresnel Emission;13;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;130;-315.7727,-324.7298;Float;False;Property;_UseFresnel;Use Fresnel?;8;0;Create;True;0;0;False;0;0;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;-838.5569,554.7645;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TFHCRemapNode;89;-292.2313,-16.20071;Float;False;5;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;1;False;3;FLOAT;1;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;26;-312.7807,532.4018;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.FresnelNode;95;-1165.254,-267.3749;Float;False;Standard;WorldNormal;ViewDir;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;106;-612.1661,450.9655;Float;False;0;105;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DotProductOpNode;87;-622.2883,-9.652705;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;92;-327.1501,-194.7354;Float;False;Property;_BackFacesColor;Back Faces Color;6;0;Create;True;0;0;False;0;0.1098039,0.4235294,1,1;0.5,0.5,0.5,1;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;123;-481.3663,-229.8227;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;149;1128.107,475.859;Float;False;False;2;Float;ASEMaterialInspector;0;1;Hidden/Templates/LightWeightSRPUnlit;e2514bdcf5e5399499a9eb24d175b9db;True;ShadowCaster;0;1;ShadowCaster;0;False;False;False;True;0;False;-1;False;False;False;False;False;True;3;RenderPipeline=LightweightPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;2;0;False;False;False;False;True;False;False;False;False;0;False;-1;False;True;1;False;-1;False;False;True;1;LightMode=ShadowCaster;False;0;Hidden/InternalErrorShader;0;0;Standard;0;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;148;1128.107,485.6747;Float;False;True;2;Float;;0;3;ERB/LWRP/Particles/Blend_TwoSides;e2514bdcf5e5399499a9eb24d175b9db;True;Base;0;0;Base;5;False;False;False;True;2;False;-1;False;False;False;False;False;True;4;RenderPipeline=LightweightPipeline;RenderType=TransparentCutout=RenderType;Queue=Transparent=Queue=0;PreviewType=Plane;True;2;0;True;2;5;False;-1;10;False;-1;0;1;False;-1;0;False;-1;False;False;False;True;True;True;True;True;0;False;-1;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;0;False;-1;True;0;False;-1;True;False;0;False;-1;0;False;-1;True;1;LightMode=LightweightForward;False;0;Hidden/InternalErrorShader;0;0;Standard;3;Vertex Position,InvertActionOnDeselection;1;Receive Shadows;0;Built-in Fog;0;0;3;True;False;True;False;5;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;150;1128.107,475.859;Float;False;False;2;Float;ASEMaterialInspector;0;1;Hidden/Templates/LightWeightSRPUnlit;e2514bdcf5e5399499a9eb24d175b9db;True;DepthOnly;0;2;DepthOnly;0;False;False;False;True;0;False;-1;False;False;False;False;False;True;3;RenderPipeline=LightweightPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;2;0;False;False;False;False;True;False;False;False;False;0;False;-1;False;True;1;False;-1;True;3;False;-1;False;True;1;LightMode=DepthOnly;True;0;0;Hidden/InternalErrorShader;0;0;Standard;0;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;0
WireConnection;22;0;15;3
WireConnection;22;1;15;4
WireConnection;23;0;17;2
WireConnection;23;1;22;0
WireConnection;59;0;29;0
WireConnection;27;0;59;0
WireConnection;27;1;23;0
WireConnection;27;2;29;4
WireConnection;131;0;104;0
WireConnection;131;1;29;3
WireConnection;14;1;27;0
WireConnection;70;0;13;0
WireConnection;70;1;14;0
WireConnection;70;2;131;0
WireConnection;133;0;70;0
WireConnection;133;1;134;1
WireConnection;133;2;134;2
WireConnection;133;3;134;3
WireConnection;133;4;134;4
WireConnection;51;0;91;0
WireConnection;51;1;52;0
WireConnection;51;2;32;0
WireConnection;51;3;105;0
WireConnection;51;4;32;4
WireConnection;127;0;31;0
WireConnection;127;1;126;0
WireConnection;135;0;133;0
WireConnection;105;1;26;0
WireConnection;91;0;130;0
WireConnection;91;1;92;0
WireConnection;91;2;89;0
WireConnection;146;0;142;0
WireConnection;142;0;93;0
WireConnection;145;0;144;0
WireConnection;145;1;52;0
WireConnection;145;2;32;0
WireConnection;145;3;32;4
WireConnection;141;0;146;0
WireConnection;141;1;105;0
WireConnection;141;2;143;0
WireConnection;21;0;15;1
WireConnection;21;1;15;2
WireConnection;147;0;51;0
WireConnection;147;1;145;0
WireConnection;88;0;87;0
WireConnection;97;0;98;0
WireConnection;97;1;93;0
WireConnection;97;2;95;0
WireConnection;126;0;95;0
WireConnection;144;0;91;0
WireConnection;144;1;141;0
WireConnection;130;0;31;0
WireConnection;130;1;123;0
WireConnection;24;0;21;0
WireConnection;24;1;17;2
WireConnection;89;0;88;0
WireConnection;26;0;106;0
WireConnection;26;1;24;0
WireConnection;95;3;96;0
WireConnection;87;0;86;0
WireConnection;87;1;85;0
WireConnection;123;0;127;0
WireConnection;123;1;97;0
WireConnection;148;0;147;0
WireConnection;148;1;135;0
WireConnection;148;2;132;0
ASEEND*/
//CHKSM=A692F5477042FF4FB8849365E3599FDC64397C11