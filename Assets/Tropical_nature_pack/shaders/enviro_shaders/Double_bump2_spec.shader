Shader "Enviro/DoubleBumped2_Specular"
{
	Properties 
	{
_MainTex("Base (RGB) Gloss (A)", 2D) = "white" {}
_DecalTex("Decal", 2D) = "white" {}
_BumpMap("Normalmap", 2D) = "bump" {}
_BumpMapDetail("Normal Detail", 2D) = "bump" {}
_SpecularColor("_SpecularColor", Color) = (1,1,1,1)
_Shininess("_Shininess", Range(0.01,1) ) = 0.5
_Color("Color", Color) = (1,1,1,1)

	}
	
	SubShader 
	{
		Tags
		{
"Queue"="Geometry"
"IgnoreProjector"="False"
"RenderType"="Opaque"

		}

		
Cull Back
ZWrite On
ZTest LEqual
ColorMask RGBA
Fog{
}


		CGPROGRAM
#pragma surface surf BlinnPhongEditor  vertex:vert
#pragma target 2.0


sampler2D _MainTex;
sampler2D _DecalTex;
sampler2D _BumpMap;
sampler2D _BumpMapDetail;
float4 _SpecularColor;
float _Shininess;
float4 _Color;

			struct EditorSurfaceOutput {
				half3 Albedo;
				half3 Normal;
				half3 Emission;
				half3 Gloss;
				half Specular;
				half Alpha;
				half4 Custom;
			};
			
			inline half4 LightingBlinnPhongEditor_PrePass (EditorSurfaceOutput s, half4 light)
			{
half3 spec = light.a * s.Gloss;
half4 c;
c.rgb = (s.Albedo * light.rgb + light.rgb * spec);
c.a = s.Alpha;
return c;

			}

			inline half4 LightingBlinnPhongEditor (EditorSurfaceOutput s, half3 lightDir, half3 viewDir, half atten)
			{
				half3 h = normalize (lightDir + viewDir);
				
				half diff = max (0, dot ( lightDir, s.Normal ));
				
				float nh = max (0, dot (s.Normal, h));
				float spec = pow (nh, s.Specular*128.0);
				
				half4 res;
				res.rgb = _LightColor0.rgb * diff;
				res.w = spec * Luminance (_LightColor0.rgb);
				res *= atten * 2.0;

				return LightingBlinnPhongEditor_PrePass( s, res );
			}

			inline half4 LightingBlinnPhongEditor_DirLightmap (EditorSurfaceOutput s, fixed4 color, fixed4 scale, half3 viewDir, bool surfFuncWritesNormal, out half3 specColor)
			{
				UNITY_DIRBASIS
				half3 scalePerBasisVector;
				
				half3 lm = DirLightmapDiffuse (unity_DirBasis, color, scale, s.Normal, surfFuncWritesNormal, scalePerBasisVector);
				
				half3 lightDir = normalize (scalePerBasisVector.x * unity_DirBasis[0] + scalePerBasisVector.y * unity_DirBasis[1] + scalePerBasisVector.z * unity_DirBasis[2]);
				half3 h = normalize (lightDir + viewDir);
			
				float nh = max (0, dot (s.Normal, h));
				float spec = pow (nh, s.Specular * 128.0);
				
				// specColor used outside in the forward path, compiled out in prepass
				specColor = lm * _SpecColor.rgb * s.Gloss * spec;
				
				// spec from the alpha component is used to calculate specular
				// in the Lighting*_Prepass function, it's not used in forward
				return half4(lm, spec);
			}
			
			struct Input {
				float2 uv_MainTex;
float2 uv_DecalTex;
float2 uv_BumpMap;
float2 uv_BumpMapDetail;

			};

			void vert (inout appdata_full v, out Input o) {
float4 VertexOutputMaster0_0_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_1_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_2_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_3_NoInput = float4(0,0,0,0);


			}
			

			void surf (Input IN, inout EditorSurfaceOutput o) {
				o.Normal = float3(0.0,0.0,1.0);
				o.Alpha = 1.0;
				o.Albedo = 0.0;
				o.Emission = 0.0;
				o.Gloss = 0.0;
				o.Specular = 0.0;
				o.Custom = 0.0;
				
float4 Sampled2D0=tex2D(_MainTex,IN.uv_MainTex.xy);
float4 Sampled2D3=tex2D(_DecalTex,IN.uv_DecalTex.xy);
float4 Multiply1=Sampled2D0 * Sampled2D3;
float4 Multiply3=Multiply1 * _Color;
float4 Tex2DNormal1=float4(UnpackNormal( tex2D(_BumpMap,(IN.uv_BumpMap.xyxy).xy)).xyz, 1.0 );
float4 Sampled2D1=tex2D(_BumpMapDetail,IN.uv_BumpMapDetail.xy);
float4 Add0=Tex2DNormal1 + Sampled2D1;
float4 Multiply0=Sampled2D3.aaaa * Sampled2D0.aaaa;
float4 Multiply2=Multiply0 * _SpecularColor;
float4 Master0_2_NoInput = float4(0,0,0,0);
float4 Master0_5_NoInput = float4(1,1,1,1);
float4 Master0_7_NoInput = float4(0,0,0,0);
float4 Master0_6_NoInput = float4(1,1,1,1);
o.Albedo = Multiply3;
o.Normal = Add0;
o.Specular = _Shininess.xxxx;
o.Gloss = Multiply2;

				o.Normal = normalize(o.Normal);
			}
		ENDCG
	}
	Fallback "Diffuse"
}