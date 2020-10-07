// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom02"
{
	Properties
	{
		[HDR]_Emissivecolor("Emissive color", Color) = (0,0,0,0)
		_Timescale("Time scale", Range( 0 , 10)) = 0
		_Emissivemask("Emissive mask", 2D) = "white" {}
		_Scroll_SPEED("Scroll_SPEED", Range( 0.1 , 10)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float4 _Emissivecolor;
		uniform float _Timescale;
		uniform sampler2D _Emissivemask;
		uniform float _Scroll_SPEED;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float mulTime2 = _Time.y * _Timescale;
			float temp_output_7_0 = sin( mulTime2 );
			float2 temp_cast_0 = (_Scroll_SPEED).xx;
			float2 panner16 = ( 1.0 * _Time.y * temp_cast_0 + i.uv_texcoord);
			o.Emission = ( _Emissivecolor * ( ( temp_output_7_0 + 1.0 ) * 0.5 ) * tex2D( _Emissivemask, panner16 ).r ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17700
197;331;1280;647;1845.459;413.48;2.121505;True;True
Node;AmplifyShaderEditor.RangedFloatNode;6;-1288.856,-109.195;Inherit;False;Property;_Timescale;Time scale;1;0;Create;True;0;0;False;0;0;2.14;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;2;-957.8558,-106.195;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;15;-1097.77,469.0006;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;17;-1142.678,686.45;Inherit;False;Property;_Scroll_SPEED;Scroll_SPEED;3;0;Create;True;0;0;False;0;0;1;0.1;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;11;-585.4566,311.0128;Inherit;False;554;304;(Sin(x)+1)/2;2;9;10;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SinOpNode;7;-792.1567,32.11278;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;9;-535.4567,362.0128;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;16;-851.9581,609.9049;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;14;-246.6569,-195.7011;Inherit;True;Property;_Emissivemask;Emissive mask;2;0;Create;True;0;0;False;0;-1;None;61c0b9c0523734e0e91bc6043c72a490;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;13;-743.8558,-222.195;Inherit;False;419.0002;161;1.0-fract(x);2;3;5;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-266.4568,361.0128;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;1;-228.1001,-488.9;Inherit;False;Property;_Emissivecolor;Emissive color;0;1;[HDR];Create;True;0;0;False;0;0,0,0,0;20.10119,81.27871,166.9272,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;12;-593.8564,-18.08722;Inherit;False;248;303;abs(sin(x));1;8;;1,1,1,1;0;0
Node;AmplifyShaderEditor.AbsOpNode;8;-543.8564,31.91278;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FractNode;3;-693.8558,-171.195;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;5;-511.8557,-172.195;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;125.2999,-208.2;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;388.8,-255.4;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Custom02;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;2;0;6;0
WireConnection;7;0;2;0
WireConnection;9;0;7;0
WireConnection;16;0;15;0
WireConnection;16;2;17;0
WireConnection;14;1;16;0
WireConnection;10;0;9;0
WireConnection;8;0;7;0
WireConnection;3;0;2;0
WireConnection;5;0;3;0
WireConnection;4;0;1;0
WireConnection;4;1;10;0
WireConnection;4;2;14;1
WireConnection;0;2;4;0
ASEEND*/
//CHKSM=3BC7CF9F3227177F95352B944416435FF517DBA6