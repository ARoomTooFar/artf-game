Shader "NormalMap"
{
	Properties 
	{
_Diffuse("_Diffuse", 2D) = "white" {}
_NormalMap("_NormalMap", 2D) = "white" {}
_Alpha("_Alpha", Range(0,1) ) = 0.8176101

	}
	
	SubShader 
	{
		Tags
		{
"Queue"="Transparent"
"IgnoreProjector"="True"
"RenderType"="Transparent"

		}

		
Cull Back
ZWrite Off
ZTest LEqual
ColorMask RGBA
Fog{
}


			Alphatest Greater 0 ZWrite Off ColorMask RGB
	
	Pass {
		Name "FORWARD"
		Tags { "LightMode" = "ForwardBase" }
		Blend SrcAlpha OneMinusSrcAlpha
Program "vp" {
// Vertex combos: 3
//   opengl - ALU: 20 to 75
//   d3d9 - ALU: 21 to 78
SubProgram "opengl " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" }
Bind "vertex" Vertex
Bind "tangent" ATTR14
Bind "normal" Normal
Bind "texcoord" TexCoord0
Vector 13 [unity_Scale]
Vector 14 [_WorldSpaceCameraPos]
Vector 15 [_WorldSpaceLightPos0]
Matrix 5 [_Object2World]
Matrix 9 [_World2Object]
Vector 16 [unity_SHAr]
Vector 17 [unity_SHAg]
Vector 18 [unity_SHAb]
Vector 19 [unity_SHBr]
Vector 20 [unity_SHBg]
Vector 21 [unity_SHBb]
Vector 22 [unity_SHC]
Vector 23 [_Diffuse_ST]
Vector 24 [_NormalMap_ST]
"3.0-!!ARBvp1.0
# 44 ALU
PARAM c[25] = { { 1 },
		state.matrix.mvp,
		program.local[5..24] };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
MUL R1.xyz, vertex.normal, c[13].w;
DP3 R2.w, R1, c[6];
DP3 R0.x, R1, c[5];
DP3 R0.z, R1, c[7];
MOV R0.y, R2.w;
MUL R1, R0.xyzz, R0.yzzx;
MOV R0.w, c[0].x;
DP4 R2.z, R0, c[18];
DP4 R2.y, R0, c[17];
DP4 R2.x, R0, c[16];
MUL R0.y, R2.w, R2.w;
DP4 R3.z, R1, c[21];
DP4 R3.y, R1, c[20];
DP4 R3.x, R1, c[19];
ADD R2.xyz, R2, R3;
MAD R0.x, R0, R0, -R0.y;
MUL R3.xyz, R0.x, c[22];
MOV R1.xyz, vertex.attrib[14];
MUL R0.xyz, vertex.normal.zxyw, R1.yzxw;
MAD R1.xyz, vertex.normal.yzxw, R1.zxyw, -R0;
ADD result.texcoord[3].xyz, R2, R3;
MOV R0.xyz, c[14];
MOV R0.w, c[0].x;
DP4 R2.z, R0, c[11];
DP4 R2.x, R0, c[9];
DP4 R2.y, R0, c[10];
MAD R0.xyz, R2, c[13].w, -vertex.position;
MUL R2.xyz, R1, vertex.attrib[14].w;
MOV R1, c[15];
DP4 R3.z, R1, c[11];
DP4 R3.x, R1, c[9];
DP4 R3.y, R1, c[10];
DP3 result.texcoord[1].y, R0, R2;
DP3 result.texcoord[2].y, R2, R3;
DP3 result.texcoord[1].z, vertex.normal, R0;
DP3 result.texcoord[1].x, R0, vertex.attrib[14];
DP3 result.texcoord[2].z, vertex.normal, R3;
DP3 result.texcoord[2].x, vertex.attrib[14], R3;
MAD result.texcoord[0].zw, vertex.texcoord[0].xyxy, c[24].xyxy, c[24];
MAD result.texcoord[0].xy, vertex.texcoord[0], c[23], c[23].zwzw;
DP4 result.position.w, vertex.position, c[4];
DP4 result.position.z, vertex.position, c[3];
DP4 result.position.y, vertex.position, c[2];
DP4 result.position.x, vertex.position, c[1];
END
# 44 instructions, 4 R-regs
"
}

SubProgram "d3d9 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" }
Bind "vertex" Vertex
Bind "tangent" TexCoord2
Bind "normal" Normal
Bind "texcoord" TexCoord0
Matrix 0 [glstate_matrix_mvp]
Vector 12 [unity_Scale]
Vector 13 [_WorldSpaceCameraPos]
Vector 14 [_WorldSpaceLightPos0]
Matrix 4 [_Object2World]
Matrix 8 [_World2Object]
Vector 15 [unity_SHAr]
Vector 16 [unity_SHAg]
Vector 17 [unity_SHAb]
Vector 18 [unity_SHBr]
Vector 19 [unity_SHBg]
Vector 20 [unity_SHBb]
Vector 21 [unity_SHC]
Vector 22 [_Diffuse_ST]
Vector 23 [_NormalMap_ST]
"vs_3_0
; 47 ALU
dcl_position o0
dcl_texcoord0 o1
dcl_texcoord1 o2
dcl_texcoord2 o3
dcl_texcoord3 o4
def c24, 1.00000000, 0, 0, 0
dcl_position0 v0
dcl_tangent0 v1
dcl_normal0 v2
dcl_texcoord0 v3
mul r1.xyz, v2, c12.w
dp3 r2.w, r1, c5
dp3 r0.x, r1, c4
dp3 r0.z, r1, c6
mov r0.y, r2.w
mul r1, r0.xyzz, r0.yzzx
mov r0.w, c24.x
dp4 r2.z, r0, c17
dp4 r2.y, r0, c16
dp4 r2.x, r0, c15
mul r0.y, r2.w, r2.w
dp4 r3.z, r1, c20
dp4 r3.y, r1, c19
dp4 r3.x, r1, c18
add r1.xyz, r2, r3
mad r0.x, r0, r0, -r0.y
mul r2.xyz, r0.x, c21
add o4.xyz, r1, r2
mov r0.xyz, v1
mul r1.xyz, v2.zxyw, r0.yzxw
mov r0.xyz, v1
mad r0.xyz, v2.yzxw, r0.zxyw, -r1
mul r3.xyz, r0, v1.w
mov r0, c10
dp4 r4.z, c14, r0
mov r0, c9
mov r1.w, c24.x
mov r1.xyz, c13
dp4 r4.y, c14, r0
dp4 r2.z, r1, c10
dp4 r2.x, r1, c8
dp4 r2.y, r1, c9
mad r2.xyz, r2, c12.w, -v0
mov r1, c8
dp4 r4.x, c14, r1
dp3 o2.y, r2, r3
dp3 o3.y, r3, r4
dp3 o2.z, v2, r2
dp3 o2.x, r2, v1
dp3 o3.z, v2, r4
dp3 o3.x, v1, r4
mad o1.zw, v3.xyxy, c23.xyxy, c23
mad o1.xy, v3, c22, c22.zwzw
dp4 o0.w, v0, c3
dp4 o0.z, v0, c2
dp4 o0.y, v0, c1
dp4 o0.x, v0, c0
"
}

SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" }
"!!GLES

#ifdef VERTEX
attribute vec4 TANGENT;
uniform vec4 unity_Scale;
uniform vec4 unity_SHC;
uniform vec4 unity_SHBr;
uniform vec4 unity_SHBg;
uniform vec4 unity_SHBb;
uniform vec4 unity_SHAr;
uniform vec4 unity_SHAg;
uniform vec4 unity_SHAb;
uniform vec4 _WorldSpaceLightPos0;
uniform vec3 _WorldSpaceCameraPos;
uniform mat4 _World2Object;
uniform mat4 _Object2World;
uniform vec4 _NormalMap_ST;
uniform vec4 _Diffuse_ST;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1 = gl_Vertex.xyzw;
  vec4 tmpvar_3;
  tmpvar_3 = TANGENT.xyzw;
  vec3 tmpvar_5;
  tmpvar_5 = gl_Normal.xyz;
  vec4 tmpvar_7;
  tmpvar_7 = gl_MultiTexCoord0.xyzw;
  vec4 tmpvar_151;
  tmpvar_151.xy = ((tmpvar_7.xy * _Diffuse_ST.xy) + _Diffuse_ST.zw);
  tmpvar_151.zw = ((tmpvar_7.xy * _NormalMap_ST.xy) + _NormalMap_ST.zw).xy;
  mat3 tmpvar_71;
  tmpvar_71[0] = _Object2World[0].xyz;
  tmpvar_71[1] = _Object2World[1].xyz;
  tmpvar_71[2] = _Object2World[2].xyz;
  vec3 tmpvar_77;
  tmpvar_77 = (cross (tmpvar_5, tmpvar_3.xyz) * tmpvar_3.w);
  mat3 tmpvar_78;
  tmpvar_78[0].x = tmpvar_3.x;
  tmpvar_78[0].y = tmpvar_77.x;
  tmpvar_78[0].z = tmpvar_5.x;
  tmpvar_78[1].x = tmpvar_3.y;
  tmpvar_78[1].y = tmpvar_77.y;
  tmpvar_78[1].z = tmpvar_5.y;
  tmpvar_78[2].x = tmpvar_3.z;
  tmpvar_78[2].y = tmpvar_77.z;
  tmpvar_78[2].z = tmpvar_5.z;
  vec4 tmpvar_95;
  tmpvar_95.xyz = _WorldSpaceCameraPos.xyz;
  tmpvar_95.w = 1.0;
  vec4 tmpvar_98;
  tmpvar_98.xyz = (tmpvar_71 * (tmpvar_5 * unity_Scale.w)).xyz;
  tmpvar_98.w = 1.0;
  vec3 x2;
  vec3 x1;
  x1.x = dot (unity_SHAr, tmpvar_98);
  x1.y = (vec2(dot (unity_SHAg, tmpvar_98))).y;
  x1.z = (vec3(dot (unity_SHAb, tmpvar_98))).z;
  vec4 tmpvar_107;
  tmpvar_107 = (tmpvar_98.xyzz * tmpvar_98.yzzx);
  x2.x = dot (unity_SHBr, tmpvar_107);
  x2.y = (vec2(dot (unity_SHBg, tmpvar_107))).y;
  x2.z = (vec3(dot (unity_SHBb, tmpvar_107))).z;
  gl_Position = (gl_ModelViewProjectionMatrix * tmpvar_1).xyzw;
  gl_TexCoord[0] = tmpvar_151.xyzw;
  vec4 tmpvar_19;
  tmpvar_19.xyz = (tmpvar_78 * (((_World2Object * tmpvar_95).xyz * unity_Scale.w) - tmpvar_1.xyz)).xyz;
  tmpvar_19.w = 0.0;
  gl_TexCoord[1] = tmpvar_19;
  vec4 tmpvar_21;
  tmpvar_21.xyz = (tmpvar_78 * (_World2Object * _WorldSpaceLightPos0).xyz).xyz;
  tmpvar_21.w = 0.0;
  gl_TexCoord[2] = tmpvar_21;
  vec4 tmpvar_23;
  tmpvar_23.xyz = ((x1 + x2) + (unity_SHC.xyz * ((tmpvar_98.x * tmpvar_98.x) - (tmpvar_98.y * tmpvar_98.y)))).xyz;
  tmpvar_23.w = 0.0;
  gl_TexCoord[3] = tmpvar_23;
}


#endif
#ifdef FRAGMENT
struct v2f_vertex_lit {
    vec2 uv;
    vec4 diff;
    vec4 spec;
};
struct v2f_img {
    vec4 pos;
    vec2 uv;
};
struct appdata_img {
    vec4 vertex;
    vec2 texcoord;
};
struct SurfaceOutput {
    vec3 Albedo;
    vec3 Normal;
    vec3 Emission;
    float Specular;
    float Gloss;
    float Alpha;
};
struct EditorSurfaceOutput {
    vec3 Albedo;
    vec3 Normal;
    vec3 Emission;
    vec3 Gloss;
    float Specular;
    float Alpha;
};
struct appdata_full {
    vec4 vertex;
    vec4 tangent;
    vec3 normal;
    vec4 texcoord;
    vec4 texcoord1;
    vec4 color;
};
struct Input {
    vec2 uv_Diffuse;
    vec2 uv_NormalMap;
};
struct v2f_surf {
    vec4 pos;
    vec4 hip_pack0;
    vec3 viewDir;
    vec3 lightDir;
    vec3 vlight;
};
uniform sampler2D _Diffuse;
uniform vec4 _LightColor0;
uniform sampler2D _NormalMap;
vec4 UnpackNormal( in vec4 packednormal );
void surf( in Input IN, inout EditorSurfaceOutput o );
float Luminance( in vec3 c );
vec4 LightingBlinnPhongEditor_PrePass( in EditorSurfaceOutput s, in vec4 light );
vec4 LightingBlinnPhongEditor( in EditorSurfaceOutput s, in vec3 lightDir, in vec3 viewDir, in float atten );
vec4 frag_surf( in v2f_surf IN );
vec4 UnpackNormal( in vec4 packednormal ) {
    vec4 normal;
    normal.xy  = ((packednormal.wy  * 2.00000) - 1.00000);
    normal.z  = sqrt( ((1.00000 - (normal.x  * normal.x )) - (normal.y  * normal.y )) );
    return normal;
}
void surf( in Input IN, inout EditorSurfaceOutput o ) {
    vec4 Tex2D1;
    vec4 Tex2D0;
    vec4 UnpackNormal0;
    vec4 Master0_2_NoInput = vec4( 0.000000, 0.000000, 0.000000, 0.000000);
    vec4 Master0_3_NoInput = vec4( 0.000000, 0.000000, 0.000000, 0.000000);
    vec4 Master0_4_NoInput = vec4( 0.000000, 0.000000, 0.000000, 0.000000);
    vec4 Master0_6_NoInput = vec4( 1.00000, 1.00000, 1.00000, 1.00000);
    vec4 _Alpha;
    o.Albedo = vec3( 0.000000);
    o.Normal = vec3( 0.000000, 0.000000, 1.00000);
    o.Emission = vec3( 0.000000);
    o.Gloss = vec3( 0.000000);
    o.Specular = 0.000000;
    o.Alpha = 1.00000;
    Tex2D1 = texture2D( _Diffuse, IN.uv_Diffuse.xyxy .xy );
    Tex2D0 = texture2D( _NormalMap, IN.uv_NormalMap.xyxy .xy );
    UnpackNormal0 = UnpackNormal( Tex2D0);
    o.Albedo = vec3( Tex2D1);
    o.Normal = vec3( UnpackNormal0);
    o.Alpha = vec4( _Alpha);
}
float Luminance( in vec3 c ) {
    return dot( c, vec3( 0.220000, 0.707000, 0.0710000));
}
vec4 LightingBlinnPhongEditor_PrePass( in EditorSurfaceOutput s, in vec4 light ) {
    vec3 spec;
    vec4 c;
    spec = (light.w  * s.Gloss);
    c.xyz  = ((s.Albedo * light.xyz ) + (light.xyz  * spec));
    c.w  = (s.Alpha + Luminance( spec));
    return c;
}
vec4 LightingBlinnPhongEditor( in EditorSurfaceOutput s, in vec3 lightDir, in vec3 viewDir, in float atten ) {
    vec3 h;
    float diff;
    float nh;
    vec3 spec;
    vec4 res;
    viewDir = normalize( viewDir );
    h = normalize( (lightDir + viewDir) );
    diff = max( 0.000000, dot( s.Normal, lightDir));
    nh = max( 0.000000, dot( s.Normal, h));
    spec = (pow( nh, (s.Specular * 128.000)) * s.Gloss);
    res.xyz  = (_LightColor0.xyz  * ((diff * atten) * 2.00000));
    res.w  = float( (spec * Luminance( _LightColor0.xyz )));
    return LightingBlinnPhongEditor_PrePass( s, res);
}
vec4 frag_surf( in v2f_surf IN ) {
    Input surfIN;
    EditorSurfaceOutput o;
    float atten = 1.00000;
    vec4 c;
    surfIN.uv_Diffuse = IN.hip_pack0.xy ;
    surfIN.uv_NormalMap = IN.hip_pack0.zw ;
    o.Albedo = vec3( 0.000000);
    o.Emission = vec3( 0.000000);
    o.Specular = 0.000000;
    o.Alpha = 0.000000;
    surf( surfIN, o);
    c = LightingBlinnPhongEditor( o, IN.lightDir, normalize( vec3( IN.viewDir) ), atten);
    c.xyz  += (o.Albedo * IN.vlight);
    c.xyz  += o.Emission;
    c.w  = o.Alpha;
    return c;
}
void main() {
    vec4 xl_retval;
    v2f_surf xlt_IN;
    xlt_IN.pos = vec4(0.0);
    xlt_IN.hip_pack0 = vec4( gl_TexCoord[0]);
    xlt_IN.viewDir = vec3( gl_TexCoord[1]);
    xlt_IN.lightDir = vec3( gl_TexCoord[2]);
    xlt_IN.vlight = vec3( gl_TexCoord[3]);
    xl_retval = frag_surf( xlt_IN);
    gl_FragData[0] = vec4( xl_retval);
}
/* NOTE: GLSL optimization failed
0:84(13): error: type mismatch
*/

#endif
"
}

SubProgram "opengl " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" }
Bind "vertex" Vertex
Bind "tangent" ATTR14
Bind "normal" Normal
Bind "texcoord" TexCoord0
Bind "texcoord1" TexCoord1
Vector 13 [unity_Scale]
Vector 14 [_WorldSpaceCameraPos]
Matrix 9 [_World2Object]
Vector 15 [unity_LightmapST]
Vector 16 [_Diffuse_ST]
Vector 17 [_NormalMap_ST]
"3.0-!!ARBvp1.0
# 20 ALU
PARAM c[18] = { { 1 },
		state.matrix.mvp,
		program.local[5..17] };
TEMP R0;
TEMP R1;
TEMP R2;
MOV R0.xyz, vertex.attrib[14];
MUL R1.xyz, vertex.normal.zxyw, R0.yzxw;
MAD R0.xyz, vertex.normal.yzxw, R0.zxyw, -R1;
MUL R1.xyz, R0, vertex.attrib[14].w;
MOV R0.xyz, c[14];
MOV R0.w, c[0].x;
DP4 R2.z, R0, c[11];
DP4 R2.x, R0, c[9];
DP4 R2.y, R0, c[10];
MAD R0.xyz, R2, c[13].w, -vertex.position;
DP3 result.texcoord[1].y, R0, R1;
DP3 result.texcoord[1].z, vertex.normal, R0;
DP3 result.texcoord[1].x, R0, vertex.attrib[14];
MAD result.texcoord[0].zw, vertex.texcoord[0].xyxy, c[17].xyxy, c[17];
MAD result.texcoord[0].xy, vertex.texcoord[0], c[16], c[16].zwzw;
MAD result.texcoord[2].xy, vertex.texcoord[1], c[15], c[15].zwzw;
DP4 result.position.w, vertex.position, c[4];
DP4 result.position.z, vertex.position, c[3];
DP4 result.position.y, vertex.position, c[2];
DP4 result.position.x, vertex.position, c[1];
END
# 20 instructions, 3 R-regs
"
}

SubProgram "d3d9 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" }
Bind "vertex" Vertex
Bind "tangent" TexCoord2
Bind "normal" Normal
Bind "texcoord" TexCoord0
Bind "texcoord1" TexCoord1
Matrix 0 [glstate_matrix_mvp]
Vector 12 [unity_Scale]
Vector 13 [_WorldSpaceCameraPos]
Matrix 8 [_World2Object]
Vector 14 [unity_LightmapST]
Vector 15 [_Diffuse_ST]
Vector 16 [_NormalMap_ST]
"vs_3_0
; 21 ALU
dcl_position o0
dcl_texcoord0 o1
dcl_texcoord1 o2
dcl_texcoord2 o3
def c17, 1.00000000, 0, 0, 0
dcl_position0 v0
dcl_tangent0 v1
dcl_normal0 v2
dcl_texcoord0 v3
dcl_texcoord1 v4
mov r0.xyz, v1
mul r1.xyz, v2.zxyw, r0.yzxw
mov r0.xyz, v1
mad r0.xyz, v2.yzxw, r0.zxyw, -r1
mul r1.xyz, r0, v1.w
mov r0.xyz, c13
mov r0.w, c17.x
dp4 r2.z, r0, c10
dp4 r2.x, r0, c8
dp4 r2.y, r0, c9
mad r0.xyz, r2, c12.w, -v0
dp3 o2.y, r0, r1
dp3 o2.z, v2, r0
dp3 o2.x, r0, v1
mad o1.zw, v3.xyxy, c16.xyxy, c16
mad o1.xy, v3, c15, c15.zwzw
mad o3.xy, v4, c14, c14.zwzw
dp4 o0.w, v0, c3
dp4 o0.z, v0, c2
dp4 o0.y, v0, c1
dp4 o0.x, v0, c0
"
}

SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" }
"!!GLES

#ifdef VERTEX
attribute vec4 TANGENT;
uniform vec4 unity_Scale;
uniform vec4 unity_LightmapST;
uniform vec3 _WorldSpaceCameraPos;
uniform mat4 _World2Object;
uniform mat4 _Object2World;
uniform vec4 _NormalMap_ST;
uniform vec4 _Diffuse_ST;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1 = gl_Vertex.xyzw;
  vec4 tmpvar_3;
  tmpvar_3 = TANGENT.xyzw;
  vec3 tmpvar_5;
  tmpvar_5 = gl_Normal.xyz;
  vec4 tmpvar_7;
  tmpvar_7 = gl_MultiTexCoord0.xyzw;
  vec4 tmpvar_104;
  tmpvar_104.xy = ((tmpvar_7.xy * _Diffuse_ST.xy) + _Diffuse_ST.zw);
  tmpvar_104.zw = ((tmpvar_7.xy * _NormalMap_ST.xy) + _NormalMap_ST.zw).xy;
  vec3 tmpvar_58;
  tmpvar_58 = (cross (tmpvar_5, tmpvar_3.xyz) * tmpvar_3.w);
  mat3 tmpvar_59;
  tmpvar_59[0].x = tmpvar_3.x;
  tmpvar_59[0].y = tmpvar_58.x;
  tmpvar_59[0].z = tmpvar_5.x;
  tmpvar_59[1].x = tmpvar_3.y;
  tmpvar_59[1].y = tmpvar_58.y;
  tmpvar_59[1].z = tmpvar_5.y;
  tmpvar_59[2].x = tmpvar_3.z;
  tmpvar_59[2].y = tmpvar_58.z;
  tmpvar_59[2].z = tmpvar_5.z;
  vec4 tmpvar_72;
  tmpvar_72.xyz = _WorldSpaceCameraPos.xyz;
  tmpvar_72.w = 1.0;
  gl_Position = (gl_ModelViewProjectionMatrix * tmpvar_1).xyzw;
  gl_TexCoord[0] = tmpvar_104.xyzw;
  vec4 tmpvar_19;
  tmpvar_19.xyz = (tmpvar_59 * (((_World2Object * tmpvar_72).xyz * unity_Scale.w) - tmpvar_1.xyz)).xyz;
  tmpvar_19.w = 0.0;
  gl_TexCoord[1] = tmpvar_19;
  vec4 tmpvar_21;
  tmpvar_21.xy = ((gl_MultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw).xy;
  tmpvar_21.z = 0.0;
  tmpvar_21.w = 0.0;
  gl_TexCoord[2] = tmpvar_21;
}


#endif
#ifdef FRAGMENT
struct v2f_vertex_lit {
    vec2 uv;
    vec4 diff;
    vec4 spec;
};
struct v2f_img {
    vec4 pos;
    vec2 uv;
};
struct appdata_img {
    vec4 vertex;
    vec2 texcoord;
};
struct SurfaceOutput {
    vec3 Albedo;
    vec3 Normal;
    vec3 Emission;
    float Specular;
    float Gloss;
    float Alpha;
};
struct EditorSurfaceOutput {
    vec3 Albedo;
    vec3 Normal;
    vec3 Emission;
    vec3 Gloss;
    float Specular;
    float Alpha;
};
struct appdata_full {
    vec4 vertex;
    vec4 tangent;
    vec3 normal;
    vec4 texcoord;
    vec4 texcoord1;
    vec4 color;
};
struct Input {
    vec2 uv_Diffuse;
    vec2 uv_NormalMap;
};
struct v2f_surf {
    vec4 pos;
    vec4 hip_pack0;
    vec3 viewDir;
    vec2 hip_lmap;
};
uniform sampler2D _Diffuse;
uniform sampler2D _NormalMap;
uniform sampler2D unity_Lightmap;
vec4 UnpackNormal( in vec4 packednormal );
void surf( in Input IN, inout EditorSurfaceOutput o );
vec3 DecodeLightmap( in vec4 color );
vec4 frag_surf( in v2f_surf IN );
vec4 UnpackNormal( in vec4 packednormal ) {
    vec4 normal;
    normal.xy  = ((packednormal.wy  * 2.00000) - 1.00000);
    normal.z  = sqrt( ((1.00000 - (normal.x  * normal.x )) - (normal.y  * normal.y )) );
    return normal;
}
void surf( in Input IN, inout EditorSurfaceOutput o ) {
    vec4 Tex2D1;
    vec4 Tex2D0;
    vec4 UnpackNormal0;
    vec4 Master0_2_NoInput = vec4( 0.000000, 0.000000, 0.000000, 0.000000);
    vec4 Master0_3_NoInput = vec4( 0.000000, 0.000000, 0.000000, 0.000000);
    vec4 Master0_4_NoInput = vec4( 0.000000, 0.000000, 0.000000, 0.000000);
    vec4 Master0_6_NoInput = vec4( 1.00000, 1.00000, 1.00000, 1.00000);
    vec4 _Alpha;
    o.Albedo = vec3( 0.000000);
    o.Normal = vec3( 0.000000, 0.000000, 1.00000);
    o.Emission = vec3( 0.000000);
    o.Gloss = vec3( 0.000000);
    o.Specular = 0.000000;
    o.Alpha = 1.00000;
    Tex2D1 = texture2D( _Diffuse, IN.uv_Diffuse.xyxy .xy );
    Tex2D0 = texture2D( _NormalMap, IN.uv_NormalMap.xyxy .xy );
    UnpackNormal0 = UnpackNormal( Tex2D0);
    o.Albedo = vec3( Tex2D1);
    o.Normal = vec3( UnpackNormal0);
    o.Alpha = vec4( _Alpha);
}
vec3 DecodeLightmap( in vec4 color ) {
    return (2.00000 * color.xyz );
}
vec4 frag_surf( in v2f_surf IN ) {
    Input surfIN;
    EditorSurfaceOutput o;
    float atten = 1.00000;
    vec3 lmFull;
    vec4 c;
    surfIN.uv_Diffuse = IN.hip_pack0.xy ;
    surfIN.uv_NormalMap = IN.hip_pack0.zw ;
    o.Albedo = vec3( 0.000000);
    o.Emission = vec3( 0.000000);
    o.Specular = 0.000000;
    o.Alpha = 0.000000;
    surf( surfIN, o);
    lmFull = DecodeLightmap( texture2D( unity_Lightmap, IN.hip_lmap.xy ));
    c.xyz  = (o.Albedo * lmFull);
    c.w  = o.Alpha;
    c.xyz  += o.Emission;
    c.w  = o.Alpha;
    return c;
}
void main() {
    vec4 xl_retval;
    v2f_surf xlt_IN;
    xlt_IN.pos = vec4(0.0);
    xlt_IN.hip_pack0 = vec4( gl_TexCoord[0]);
    xlt_IN.viewDir = vec3( gl_TexCoord[1]);
    xlt_IN.hip_lmap = vec2( gl_TexCoord[2]);
    xl_retval = frag_surf( xlt_IN);
    gl_FragData[0] = vec4( xl_retval);
}
/* NOTE: GLSL optimization failed
0:81(13): error: type mismatch
*/

#endif
"
}

SubProgram "opengl " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "VERTEXLIGHT_ON" }
Bind "vertex" Vertex
Bind "tangent" ATTR14
Bind "normal" Normal
Bind "texcoord" TexCoord0
Vector 13 [unity_Scale]
Vector 14 [_WorldSpaceCameraPos]
Vector 15 [_WorldSpaceLightPos0]
Matrix 5 [_Object2World]
Matrix 9 [_World2Object]
Vector 16 [unity_4LightPosX0]
Vector 17 [unity_4LightPosY0]
Vector 18 [unity_4LightPosZ0]
Vector 19 [unity_4LightAtten0]
Vector 20 [unity_LightColor0]
Vector 21 [unity_LightColor1]
Vector 22 [unity_LightColor2]
Vector 23 [unity_LightColor3]
Vector 24 [unity_SHAr]
Vector 25 [unity_SHAg]
Vector 26 [unity_SHAb]
Vector 27 [unity_SHBr]
Vector 28 [unity_SHBg]
Vector 29 [unity_SHBb]
Vector 30 [unity_SHC]
Vector 31 [_Diffuse_ST]
Vector 32 [_NormalMap_ST]
"3.0-!!ARBvp1.0
# 75 ALU
PARAM c[33] = { { 1, 0 },
		state.matrix.mvp,
		program.local[5..32] };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
TEMP R4;
MUL R3.xyz, vertex.normal, c[13].w;
DP4 R0.x, vertex.position, c[6];
ADD R1, -R0.x, c[17];
DP3 R3.w, R3, c[6];
DP3 R4.x, R3, c[5];
DP3 R3.x, R3, c[7];
MUL R2, R3.w, R1;
DP4 R0.x, vertex.position, c[5];
ADD R0, -R0.x, c[16];
MUL R1, R1, R1;
MOV R4.z, R3.x;
MAD R2, R4.x, R0, R2;
MOV R4.w, c[0].x;
DP4 R4.y, vertex.position, c[7];
MAD R1, R0, R0, R1;
ADD R0, -R4.y, c[18];
MAD R1, R0, R0, R1;
MAD R0, R3.x, R0, R2;
MUL R2, R1, c[19];
MOV R4.y, R3.w;
RSQ R1.x, R1.x;
RSQ R1.y, R1.y;
RSQ R1.w, R1.w;
RSQ R1.z, R1.z;
MUL R0, R0, R1;
ADD R1, R2, c[0].x;
RCP R1.x, R1.x;
RCP R1.y, R1.y;
RCP R1.w, R1.w;
RCP R1.z, R1.z;
MAX R0, R0, c[0].y;
MUL R0, R0, R1;
MUL R1.xyz, R0.y, c[21];
MAD R1.xyz, R0.x, c[20], R1;
MAD R0.xyz, R0.z, c[22], R1;
MAD R1.xyz, R0.w, c[23], R0;
MUL R0, R4.xyzz, R4.yzzx;
DP4 R3.z, R0, c[29];
DP4 R3.y, R0, c[28];
DP4 R3.x, R0, c[27];
MUL R1.w, R3, R3;
MAD R0.x, R4, R4, -R1.w;
MOV R0.w, c[0].x;
DP4 R2.z, R4, c[26];
DP4 R2.y, R4, c[25];
DP4 R2.x, R4, c[24];
ADD R2.xyz, R2, R3;
MUL R3.xyz, R0.x, c[30];
ADD R3.xyz, R2, R3;
MOV R0.xyz, vertex.attrib[14];
MUL R2.xyz, vertex.normal.zxyw, R0.yzxw;
ADD result.texcoord[3].xyz, R3, R1;
MAD R1.xyz, vertex.normal.yzxw, R0.zxyw, -R2;
MOV R0.xyz, c[14];
DP4 R2.z, R0, c[11];
DP4 R2.x, R0, c[9];
DP4 R2.y, R0, c[10];
MAD R0.xyz, R2, c[13].w, -vertex.position;
MUL R2.xyz, R1, vertex.attrib[14].w;
MOV R1, c[15];
DP4 R3.z, R1, c[11];
DP4 R3.x, R1, c[9];
DP4 R3.y, R1, c[10];
DP3 result.texcoord[1].y, R0, R2;
DP3 result.texcoord[2].y, R2, R3;
DP3 result.texcoord[1].z, vertex.normal, R0;
DP3 result.texcoord[1].x, R0, vertex.attrib[14];
DP3 result.texcoord[2].z, vertex.normal, R3;
DP3 result.texcoord[2].x, vertex.attrib[14], R3;
MAD result.texcoord[0].zw, vertex.texcoord[0].xyxy, c[32].xyxy, c[32];
MAD result.texcoord[0].xy, vertex.texcoord[0], c[31], c[31].zwzw;
DP4 result.position.w, vertex.position, c[4];
DP4 result.position.z, vertex.position, c[3];
DP4 result.position.y, vertex.position, c[2];
DP4 result.position.x, vertex.position, c[1];
END
# 75 instructions, 5 R-regs
"
}

SubProgram "d3d9 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "VERTEXLIGHT_ON" }
Bind "vertex" Vertex
Bind "tangent" TexCoord2
Bind "normal" Normal
Bind "texcoord" TexCoord0
Matrix 0 [glstate_matrix_mvp]
Vector 12 [unity_Scale]
Vector 13 [_WorldSpaceCameraPos]
Vector 14 [_WorldSpaceLightPos0]
Matrix 4 [_Object2World]
Matrix 8 [_World2Object]
Vector 15 [unity_4LightPosX0]
Vector 16 [unity_4LightPosY0]
Vector 17 [unity_4LightPosZ0]
Vector 18 [unity_4LightAtten0]
Vector 19 [unity_LightColor0]
Vector 20 [unity_LightColor1]
Vector 21 [unity_LightColor2]
Vector 22 [unity_LightColor3]
Vector 23 [unity_SHAr]
Vector 24 [unity_SHAg]
Vector 25 [unity_SHAb]
Vector 26 [unity_SHBr]
Vector 27 [unity_SHBg]
Vector 28 [unity_SHBb]
Vector 29 [unity_SHC]
Vector 30 [_Diffuse_ST]
Vector 31 [_NormalMap_ST]
"vs_3_0
; 78 ALU
dcl_position o0
dcl_texcoord0 o1
dcl_texcoord1 o2
dcl_texcoord2 o3
dcl_texcoord3 o4
def c32, 1.00000000, 0.00000000, 0, 0
dcl_position0 v0
dcl_tangent0 v1
dcl_normal0 v2
dcl_texcoord0 v3
mul r3.xyz, v2, c12.w
dp4 r0.x, v0, c5
add r1, -r0.x, c16
dp3 r3.w, r3, c5
dp3 r4.x, r3, c4
dp3 r3.x, r3, c6
mul r2, r3.w, r1
dp4 r0.x, v0, c4
add r0, -r0.x, c15
mul r1, r1, r1
mov r4.z, r3.x
mad r2, r4.x, r0, r2
mov r4.w, c32.x
dp4 r4.y, v0, c6
mad r1, r0, r0, r1
add r0, -r4.y, c17
mad r1, r0, r0, r1
mad r0, r3.x, r0, r2
mul r2, r1, c18
mov r4.y, r3.w
rsq r1.x, r1.x
rsq r1.y, r1.y
rsq r1.w, r1.w
rsq r1.z, r1.z
mul r0, r0, r1
add r1, r2, c32.x
dp4 r2.z, r4, c25
dp4 r2.y, r4, c24
dp4 r2.x, r4, c23
rcp r1.x, r1.x
rcp r1.y, r1.y
rcp r1.w, r1.w
rcp r1.z, r1.z
max r0, r0, c32.y
mul r0, r0, r1
mul r1.xyz, r0.y, c20
mad r1.xyz, r0.x, c19, r1
mad r0.xyz, r0.z, c21, r1
mad r1.xyz, r0.w, c22, r0
mul r0, r4.xyzz, r4.yzzx
mul r1.w, r3, r3
dp4 r3.z, r0, c28
dp4 r3.y, r0, c27
dp4 r3.x, r0, c26
mad r1.w, r4.x, r4.x, -r1
mul r0.xyz, r1.w, c29
add r2.xyz, r2, r3
add r2.xyz, r2, r0
add o4.xyz, r2, r1
mov r0.xyz, v1
mul r1.xyz, v2.zxyw, r0.yzxw
mov r0.xyz, v1
mad r0.xyz, v2.yzxw, r0.zxyw, -r1
mul r3.xyz, r0, v1.w
mov r0, c10
dp4 r4.z, c14, r0
mov r0, c9
mov r1.w, c32.x
mov r1.xyz, c13
dp4 r4.y, c14, r0
dp4 r2.z, r1, c10
dp4 r2.x, r1, c8
dp4 r2.y, r1, c9
mad r2.xyz, r2, c12.w, -v0
mov r1, c8
dp4 r4.x, c14, r1
dp3 o2.y, r2, r3
dp3 o3.y, r3, r4
dp3 o2.z, v2, r2
dp3 o2.x, r2, v1
dp3 o3.z, v2, r4
dp3 o3.x, v1, r4
mad o1.zw, v3.xyxy, c31.xyxy, c31
mad o1.xy, v3, c30, c30.zwzw
dp4 o0.w, v0, c3
dp4 o0.z, v0, c2
dp4 o0.y, v0, c1
dp4 o0.x, v0, c0
"
}

SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "VERTEXLIGHT_ON" }
"!!GLES

#ifdef VERTEX
attribute vec4 TANGENT;
uniform vec4 unity_Scale;
uniform vec4 unity_SHC;
uniform vec4 unity_SHBr;
uniform vec4 unity_SHBg;
uniform vec4 unity_SHBb;
uniform vec4 unity_SHAr;
uniform vec4 unity_SHAg;
uniform vec4 unity_SHAb;
uniform vec3 unity_LightColor3;
uniform vec3 unity_LightColor2;
uniform vec3 unity_LightColor1;
uniform vec3 unity_LightColor0;
uniform vec4 unity_4LightPosZ0;
uniform vec4 unity_4LightPosY0;
uniform vec4 unity_4LightPosX0;
uniform vec4 unity_4LightAtten0;
uniform vec4 _WorldSpaceLightPos0;
uniform vec3 _WorldSpaceCameraPos;
uniform mat4 _World2Object;
uniform mat4 _Object2World;
uniform vec4 _NormalMap_ST;
uniform vec4 _Diffuse_ST;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1 = gl_Vertex.xyzw;
  vec4 tmpvar_3;
  tmpvar_3 = TANGENT.xyzw;
  vec3 tmpvar_5;
  tmpvar_5 = gl_Normal.xyz;
  vec4 tmpvar_7;
  tmpvar_7 = gl_MultiTexCoord0.xyzw;
  vec4 tmpvar_200;
  tmpvar_200.xy = ((tmpvar_7.xy * _Diffuse_ST.xy) + _Diffuse_ST.zw);
  tmpvar_200.zw = ((tmpvar_7.xy * _NormalMap_ST.xy) + _NormalMap_ST.zw).xy;
  mat3 tmpvar_94;
  tmpvar_94[0] = _Object2World[0].xyz;
  tmpvar_94[1] = _Object2World[1].xyz;
  tmpvar_94[2] = _Object2World[2].xyz;
  vec3 tmpvar_98;
  tmpvar_98 = (tmpvar_94 * (tmpvar_5 * unity_Scale.w));
  vec3 tmpvar_100;
  tmpvar_100 = (cross (tmpvar_5, tmpvar_3.xyz) * tmpvar_3.w);
  mat3 tmpvar_101;
  tmpvar_101[0].x = tmpvar_3.x;
  tmpvar_101[0].y = tmpvar_100.x;
  tmpvar_101[0].z = tmpvar_5.x;
  tmpvar_101[1].x = tmpvar_3.y;
  tmpvar_101[1].y = tmpvar_100.y;
  tmpvar_101[1].z = tmpvar_5.y;
  tmpvar_101[2].x = tmpvar_3.z;
  tmpvar_101[2].y = tmpvar_100.z;
  tmpvar_101[2].z = tmpvar_5.z;
  vec4 tmpvar_118;
  tmpvar_118.xyz = _WorldSpaceCameraPos.xyz;
  tmpvar_118.w = 1.0;
  vec4 tmpvar_121;
  tmpvar_121.xyz = tmpvar_98.xyz;
  tmpvar_121.w = 1.0;
  vec3 x2;
  vec3 x1;
  x1.x = dot (unity_SHAr, tmpvar_121);
  x1.y = (vec2(dot (unity_SHAg, tmpvar_121))).y;
  x1.z = (vec3(dot (unity_SHAb, tmpvar_121))).z;
  vec4 tmpvar_130;
  tmpvar_130 = (tmpvar_121.xyzz * tmpvar_121.yzzx);
  x2.x = dot (unity_SHBr, tmpvar_130);
  x2.y = (vec2(dot (unity_SHBg, tmpvar_130))).y;
  x2.z = (vec3(dot (unity_SHBb, tmpvar_130))).z;
  vec3 tmpvar_141;
  tmpvar_141 = (_Object2World * tmpvar_1).xyz;
  vec4 tmpvar_144;
  tmpvar_144 = (unity_4LightPosX0 - tmpvar_141.x);
  vec4 tmpvar_145;
  tmpvar_145 = (unity_4LightPosY0 - tmpvar_141.y);
  vec4 tmpvar_146;
  tmpvar_146 = (unity_4LightPosZ0 - tmpvar_141.z);
  vec4 tmpvar_150;
  tmpvar_150 = (((tmpvar_144 * tmpvar_144) + (tmpvar_145 * tmpvar_145)) + (tmpvar_146 * tmpvar_146));
  vec4 tmpvar_160;
  tmpvar_160 = (max (vec4(0.0, 0.0, 0.0, 0.0), ((((tmpvar_144 * tmpvar_98.x) + (tmpvar_145 * tmpvar_98.y)) + (tmpvar_146 * tmpvar_98.z)) * inversesqrt (tmpvar_150))) * 1.0/((1.0 + (tmpvar_150 * unity_4LightAtten0))));
  gl_Position = (gl_ModelViewProjectionMatrix * tmpvar_1).xyzw;
  gl_TexCoord[0] = tmpvar_200.xyzw;
  vec4 tmpvar_19;
  tmpvar_19.xyz = (tmpvar_101 * (((_World2Object * tmpvar_118).xyz * unity_Scale.w) - tmpvar_1.xyz)).xyz;
  tmpvar_19.w = 0.0;
  gl_TexCoord[1] = tmpvar_19;
  vec4 tmpvar_21;
  tmpvar_21.xyz = (tmpvar_101 * (_World2Object * _WorldSpaceLightPos0).xyz).xyz;
  tmpvar_21.w = 0.0;
  gl_TexCoord[2] = tmpvar_21;
  vec4 tmpvar_23;
  tmpvar_23.xyz = (((x1 + x2) + (unity_SHC.xyz * ((tmpvar_121.x * tmpvar_121.x) - (tmpvar_121.y * tmpvar_121.y)))) + ((((unity_LightColor0 * tmpvar_160.x) + (unity_LightColor1 * tmpvar_160.y)) + (unity_LightColor2 * tmpvar_160.z)) + (unity_LightColor3 * tmpvar_160.w))).xyz;
  tmpvar_23.w = 0.0;
  gl_TexCoord[3] = tmpvar_23;
}


#endif
#ifdef FRAGMENT
struct v2f_vertex_lit {
    vec2 uv;
    vec4 diff;
    vec4 spec;
};
struct v2f_img {
    vec4 pos;
    vec2 uv;
};
struct appdata_img {
    vec4 vertex;
    vec2 texcoord;
};
struct SurfaceOutput {
    vec3 Albedo;
    vec3 Normal;
    vec3 Emission;
    float Specular;
    float Gloss;
    float Alpha;
};
struct EditorSurfaceOutput {
    vec3 Albedo;
    vec3 Normal;
    vec3 Emission;
    vec3 Gloss;
    float Specular;
    float Alpha;
};
struct appdata_full {
    vec4 vertex;
    vec4 tangent;
    vec3 normal;
    vec4 texcoord;
    vec4 texcoord1;
    vec4 color;
};
struct Input {
    vec2 uv_Diffuse;
    vec2 uv_NormalMap;
};
struct v2f_surf {
    vec4 pos;
    vec4 hip_pack0;
    vec3 viewDir;
    vec3 lightDir;
    vec3 vlight;
};
uniform sampler2D _Diffuse;
uniform vec4 _LightColor0;
uniform sampler2D _NormalMap;
vec4 UnpackNormal( in vec4 packednormal );
void surf( in Input IN, inout EditorSurfaceOutput o );
float Luminance( in vec3 c );
vec4 LightingBlinnPhongEditor_PrePass( in EditorSurfaceOutput s, in vec4 light );
vec4 LightingBlinnPhongEditor( in EditorSurfaceOutput s, in vec3 lightDir, in vec3 viewDir, in float atten );
vec4 frag_surf( in v2f_surf IN );
vec4 UnpackNormal( in vec4 packednormal ) {
    vec4 normal;
    normal.xy  = ((packednormal.wy  * 2.00000) - 1.00000);
    normal.z  = sqrt( ((1.00000 - (normal.x  * normal.x )) - (normal.y  * normal.y )) );
    return normal;
}
void surf( in Input IN, inout EditorSurfaceOutput o ) {
    vec4 Tex2D1;
    vec4 Tex2D0;
    vec4 UnpackNormal0;
    vec4 Master0_2_NoInput = vec4( 0.000000, 0.000000, 0.000000, 0.000000);
    vec4 Master0_3_NoInput = vec4( 0.000000, 0.000000, 0.000000, 0.000000);
    vec4 Master0_4_NoInput = vec4( 0.000000, 0.000000, 0.000000, 0.000000);
    vec4 Master0_6_NoInput = vec4( 1.00000, 1.00000, 1.00000, 1.00000);
    vec4 _Alpha;
    o.Albedo = vec3( 0.000000);
    o.Normal = vec3( 0.000000, 0.000000, 1.00000);
    o.Emission = vec3( 0.000000);
    o.Gloss = vec3( 0.000000);
    o.Specular = 0.000000;
    o.Alpha = 1.00000;
    Tex2D1 = texture2D( _Diffuse, IN.uv_Diffuse.xyxy .xy );
    Tex2D0 = texture2D( _NormalMap, IN.uv_NormalMap.xyxy .xy );
    UnpackNormal0 = UnpackNormal( Tex2D0);
    o.Albedo = vec3( Tex2D1);
    o.Normal = vec3( UnpackNormal0);
    o.Alpha = vec4( _Alpha);
}
float Luminance( in vec3 c ) {
    return dot( c, vec3( 0.220000, 0.707000, 0.0710000));
}
vec4 LightingBlinnPhongEditor_PrePass( in EditorSurfaceOutput s, in vec4 light ) {
    vec3 spec;
    vec4 c;
    spec = (light.w  * s.Gloss);
    c.xyz  = ((s.Albedo * light.xyz ) + (light.xyz  * spec));
    c.w  = (s.Alpha + Luminance( spec));
    return c;
}
vec4 LightingBlinnPhongEditor( in EditorSurfaceOutput s, in vec3 lightDir, in vec3 viewDir, in float atten ) {
    vec3 h;
    float diff;
    float nh;
    vec3 spec;
    vec4 res;
    viewDir = normalize( viewDir );
    h = normalize( (lightDir + viewDir) );
    diff = max( 0.000000, dot( s.Normal, lightDir));
    nh = max( 0.000000, dot( s.Normal, h));
    spec = (pow( nh, (s.Specular * 128.000)) * s.Gloss);
    res.xyz  = (_LightColor0.xyz  * ((diff * atten) * 2.00000));
    res.w  = float( (spec * Luminance( _LightColor0.xyz )));
    return LightingBlinnPhongEditor_PrePass( s, res);
}
vec4 frag_surf( in v2f_surf IN ) {
    Input surfIN;
    EditorSurfaceOutput o;
    float atten = 1.00000;
    vec4 c;
    surfIN.uv_Diffuse = IN.hip_pack0.xy ;
    surfIN.uv_NormalMap = IN.hip_pack0.zw ;
    o.Albedo = vec3( 0.000000);
    o.Emission = vec3( 0.000000);
    o.Specular = 0.000000;
    o.Alpha = 0.000000;
    surf( surfIN, o);
    c = LightingBlinnPhongEditor( o, IN.lightDir, normalize( vec3( IN.viewDir) ), atten);
    c.xyz  += (o.Albedo * IN.vlight);
    c.xyz  += o.Emission;
    c.w  = o.Alpha;
    return c;
}
void main() {
    vec4 xl_retval;
    v2f_surf xlt_IN;
    xlt_IN.pos = vec4(0.0);
    xlt_IN.hip_pack0 = vec4( gl_TexCoord[0]);
    xlt_IN.viewDir = vec3( gl_TexCoord[1]);
    xlt_IN.lightDir = vec3( gl_TexCoord[2]);
    xlt_IN.vlight = vec3( gl_TexCoord[3]);
    xl_retval = frag_surf( xlt_IN);
    gl_FragData[0] = vec4( xl_retval);
}
/* NOTE: GLSL optimization failed
0:84(13): error: type mismatch
*/

#endif
"
}

}
Program "fp" {
// Fragment combos: 2
//   opengl - ALU: 6 to 15, TEX: 2 to 2
//   d3d9 - ALU: 4 to 13, TEX: 2 to 2
SubProgram "opengl " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" }
Vector 0 [_LightColor0]
Float 1 [_Alpha]
SetTexture 0 [_Diffuse] 2D
SetTexture 1 [_NormalMap] 2D
"3.0-!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 15 ALU, 2 TEX
PARAM c[3] = { program.local[0..1],
		{ 2, 1, 0 } };
TEMP R0;
TEMP R1;
TEMP R2;
TEX R0.yw, fragment.texcoord[0].zwzw, texture[1], 2D;
MAD R0.xy, R0.wyzw, c[2].x, -c[2].y;
MUL R0.z, R0.y, R0.y;
MAD R0.z, -R0.x, R0.x, -R0;
ADD R0.z, R0, c[2].y;
RSQ R0.z, R0.z;
RCP R0.z, R0.z;
DP3 R0.x, R0, fragment.texcoord[2];
MAX R0.w, R0.x, c[2].z;
TEX R0.xyz, fragment.texcoord[0], texture[0], 2D;
MUL R0.w, R0, c[2].x;
MUL R2.xyz, R0, fragment.texcoord[3];
MUL R1.xyz, R0.w, c[0];
MAD result.color.xyz, R0, R1, R2;
MOV result.color.w, c[1].x;
END
# 15 instructions, 3 R-regs
"
}

SubProgram "d3d9 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" }
Vector 0 [_LightColor0]
Float 1 [_Alpha]
SetTexture 0 [_Diffuse] 2D
SetTexture 1 [_NormalMap] 2D
"ps_3_0
; 13 ALU, 2 TEX
dcl_2d s0
dcl_2d s1
def c2, 2.00000000, -1.00000000, 1.00000000, 0.00000000
dcl_texcoord0 v0
dcl_texcoord2 v2.xyz
dcl_texcoord3 v3.xyz
texld r0.yw, v0.zwzw, s1
mad_pp r0.xy, r0.wyzw, c2.x, c2.y
mul_pp r0.z, r0.y, r0.y
mad_pp r0.z, -r0.x, r0.x, -r0
add_pp r0.z, r0, c2
rsq_pp r0.z, r0.z
rcp_pp r0.z, r0.z
dp3_pp r0.x, r0, v2
max_pp r0.w, r0.x, c2
texld r0.xyz, v0, s0
mul_pp r0.w, r0, c2.x
mul r2.xyz, r0, v3
mul r1.xyz, r0.w, c0
mad_pp oC0.xyz, r0, r1, r2
mov_pp oC0.w, c1.x
"
}

SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" }
"!!GLES"
}

SubProgram "opengl " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" }
Float 0 [_Alpha]
SetTexture 0 [_Diffuse] 2D
SetTexture 2 [unity_Lightmap] 2D
"3.0-!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 6 ALU, 2 TEX
PARAM c[2] = { program.local[0],
		{ 8 } };
TEMP R0;
TEMP R1;
TEX R0, fragment.texcoord[2], texture[2], 2D;
TEX R1.xyz, fragment.texcoord[0], texture[0], 2D;
MUL R0.xyz, R0.w, R0;
MUL R0.xyz, R0, R1;
MUL result.color.xyz, R0, c[1].x;
MOV result.color.w, c[0].x;
END
# 6 instructions, 2 R-regs
"
}

SubProgram "d3d9 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" }
Float 0 [_Alpha]
SetTexture 0 [_Diffuse] 2D
SetTexture 2 [unity_Lightmap] 2D
"ps_3_0
; 4 ALU, 2 TEX
dcl_2d s0
dcl_2d s1
dcl_2d s2
def c1, 8.00000000, 0, 0, 0
dcl_texcoord0 v0.xy
dcl_texcoord2 v1.xy
texld r0, v1, s2
texld r1.xyz, v0, s0
mul_pp r0.xyz, r0.w, r0
mul_pp r0.xyz, r0, r1
mul_pp oC0.xyz, r0, c1.x
mov_pp oC0.w, c0.x
"
}

SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" }
"!!GLES"
}

}
	}
	Pass {
		Name "FORWARD"
		Tags { "LightMode" = "ForwardAdd" }
		ZWrite Off Blend One One Fog { Color (0,0,0,0) }
		Blend SrcAlpha One
Program "vp" {
// Vertex combos: 5
//   opengl - ALU: 26 to 35
//   d3d9 - ALU: 29 to 38
SubProgram "opengl " {
Keywords { "POINT" }
Bind "vertex" Vertex
Bind "tangent" ATTR14
Bind "normal" Normal
Bind "texcoord" TexCoord0
Vector 17 [unity_Scale]
Vector 18 [_WorldSpaceCameraPos]
Vector 19 [_WorldSpaceLightPos0]
Matrix 5 [_Object2World]
Matrix 9 [_World2Object]
Matrix 13 [_LightMatrix0]
Vector 20 [_Diffuse_ST]
Vector 21 [_NormalMap_ST]
"3.0-!!ARBvp1.0
# 34 ALU
PARAM c[22] = { { 1 },
		state.matrix.mvp,
		program.local[5..21] };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
MOV R1.xyz, c[18];
MOV R1.w, c[0].x;
MOV R0.xyz, vertex.attrib[14];
DP4 R2.z, R1, c[11];
DP4 R2.y, R1, c[10];
DP4 R2.x, R1, c[9];
MAD R2.xyz, R2, c[17].w, -vertex.position;
MUL R1.xyz, vertex.normal.zxyw, R0.yzxw;
MAD R1.xyz, vertex.normal.yzxw, R0.zxyw, -R1;
MOV R0, c[19];
MUL R1.xyz, R1, vertex.attrib[14].w;
DP4 R3.z, R0, c[11];
DP4 R3.x, R0, c[9];
DP4 R3.y, R0, c[10];
MAD R0.xyz, R3, c[17].w, -vertex.position;
DP3 result.texcoord[1].y, R0, R1;
DP3 result.texcoord[1].z, vertex.normal, R0;
DP3 result.texcoord[1].x, R0, vertex.attrib[14];
DP4 R0.w, vertex.position, c[8];
DP4 R0.z, vertex.position, c[7];
DP4 R0.x, vertex.position, c[5];
DP4 R0.y, vertex.position, c[6];
DP3 result.texcoord[2].y, R1, R2;
DP3 result.texcoord[2].z, vertex.normal, R2;
DP3 result.texcoord[2].x, vertex.attrib[14], R2;
DP4 result.texcoord[3].z, R0, c[15];
DP4 result.texcoord[3].y, R0, c[14];
DP4 result.texcoord[3].x, R0, c[13];
MAD result.texcoord[0].zw, vertex.texcoord[0].xyxy, c[21].xyxy, c[21];
MAD result.texcoord[0].xy, vertex.texcoord[0], c[20], c[20].zwzw;
DP4 result.position.w, vertex.position, c[4];
DP4 result.position.z, vertex.position, c[3];
DP4 result.position.y, vertex.position, c[2];
DP4 result.position.x, vertex.position, c[1];
END
# 34 instructions, 4 R-regs
"
}

SubProgram "d3d9 " {
Keywords { "POINT" }
Bind "vertex" Vertex
Bind "tangent" TexCoord2
Bind "normal" Normal
Bind "texcoord" TexCoord0
Matrix 0 [glstate_matrix_mvp]
Vector 16 [unity_Scale]
Vector 17 [_WorldSpaceCameraPos]
Vector 18 [_WorldSpaceLightPos0]
Matrix 4 [_Object2World]
Matrix 8 [_World2Object]
Matrix 12 [_LightMatrix0]
Vector 19 [_Diffuse_ST]
Vector 20 [_NormalMap_ST]
"vs_3_0
; 37 ALU
dcl_position o0
dcl_texcoord0 o1
dcl_texcoord1 o2
dcl_texcoord2 o3
dcl_texcoord3 o4
def c21, 1.00000000, 0, 0, 0
dcl_position0 v0
dcl_tangent0 v1
dcl_normal0 v2
dcl_texcoord0 v3
mov r0.w, c21.x
mov r0.xyz, c17
dp4 r1.z, r0, c10
dp4 r1.y, r0, c9
dp4 r1.x, r0, c8
mad r3.xyz, r1, c16.w, -v0
mov r0.xyz, v1
mul r1.xyz, v2.zxyw, r0.yzxw
mov r0.xyz, v1
mad r1.xyz, v2.yzxw, r0.zxyw, -r1
mul r2.xyz, r1, v1.w
mov r0, c10
dp4 r4.z, c18, r0
mov r0, c9
dp4 r4.y, c18, r0
mov r1, c8
dp4 r4.x, c18, r1
mad r0.xyz, r4, c16.w, -v0
dp3 o2.y, r0, r2
dp3 o2.z, v2, r0
dp3 o2.x, r0, v1
dp4 r0.w, v0, c7
dp4 r0.z, v0, c6
dp4 r0.x, v0, c4
dp4 r0.y, v0, c5
dp3 o3.y, r2, r3
dp3 o3.z, v2, r3
dp3 o3.x, v1, r3
dp4 o4.z, r0, c14
dp4 o4.y, r0, c13
dp4 o4.x, r0, c12
mad o1.zw, v3.xyxy, c20.xyxy, c20
mad o1.xy, v3, c19, c19.zwzw
dp4 o0.w, v0, c3
dp4 o0.z, v0, c2
dp4 o0.y, v0, c1
dp4 o0.x, v0, c0
"
}

SubProgram "gles " {
Keywords { "POINT" }
"!!GLES

#ifdef VERTEX
attribute vec4 TANGENT;
uniform vec4 unity_Scale;
uniform vec4 _WorldSpaceLightPos0;
uniform vec3 _WorldSpaceCameraPos;
uniform mat4 _World2Object;
uniform mat4 _Object2World;
uniform vec4 _NormalMap_ST;
uniform mat4 _LightMatrix0;
uniform vec4 _Diffuse_ST;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1 = gl_Vertex.xyzw;
  vec4 tmpvar_3;
  tmpvar_3 = TANGENT.xyzw;
  vec3 tmpvar_5;
  tmpvar_5 = gl_Normal.xyz;
  vec4 tmpvar_7;
  tmpvar_7 = gl_MultiTexCoord0.xyzw;
  vec4 tmpvar_98;
  tmpvar_98.xy = ((tmpvar_7.xy * _Diffuse_ST.xy) + _Diffuse_ST.zw);
  tmpvar_98.zw = ((tmpvar_7.xy * _NormalMap_ST.xy) + _NormalMap_ST.zw).xy;
  vec3 tmpvar_43;
  tmpvar_43 = (cross (tmpvar_5, tmpvar_3.xyz) * tmpvar_3.w);
  mat3 tmpvar_44;
  tmpvar_44[0].x = tmpvar_3.x;
  tmpvar_44[0].y = tmpvar_43.x;
  tmpvar_44[0].z = tmpvar_5.x;
  tmpvar_44[1].x = tmpvar_3.y;
  tmpvar_44[1].y = tmpvar_43.y;
  tmpvar_44[1].z = tmpvar_5.y;
  tmpvar_44[2].x = tmpvar_3.z;
  tmpvar_44[2].y = tmpvar_43.z;
  tmpvar_44[2].z = tmpvar_5.z;
  vec4 tmpvar_61;
  tmpvar_61.xyz = _WorldSpaceCameraPos.xyz;
  tmpvar_61.w = 1.0;
  gl_Position = (gl_ModelViewProjectionMatrix * tmpvar_1).xyzw;
  gl_TexCoord[0] = tmpvar_98.xyzw;
  vec4 tmpvar_19;
  tmpvar_19.xyz = (tmpvar_44 * (((_World2Object * _WorldSpaceLightPos0).xyz * unity_Scale.w) - tmpvar_1.xyz)).xyz;
  tmpvar_19.w = 0.0;
  gl_TexCoord[1] = tmpvar_19;
  vec4 tmpvar_21;
  tmpvar_21.xyz = (tmpvar_44 * (((_World2Object * tmpvar_61).xyz * unity_Scale.w) - tmpvar_1.xyz)).xyz;
  tmpvar_21.w = 0.0;
  gl_TexCoord[2] = tmpvar_21;
  vec4 tmpvar_23;
  tmpvar_23.xyz = (_LightMatrix0 * (_Object2World * tmpvar_1)).xyz;
  tmpvar_23.w = 0.0;
  gl_TexCoord[3] = tmpvar_23;
}


#endif
#ifdef FRAGMENT
struct v2f_vertex_lit {
    vec2 uv;
    vec4 diff;
    vec4 spec;
};
struct v2f_img {
    vec4 pos;
    vec2 uv;
};
struct appdata_img {
    vec4 vertex;
    vec2 texcoord;
};
struct SurfaceOutput {
    vec3 Albedo;
    vec3 Normal;
    vec3 Emission;
    float Specular;
    float Gloss;
    float Alpha;
};
struct EditorSurfaceOutput {
    vec3 Albedo;
    vec3 Normal;
    vec3 Emission;
    vec3 Gloss;
    float Specular;
    float Alpha;
};
struct appdata_full {
    vec4 vertex;
    vec4 tangent;
    vec3 normal;
    vec4 texcoord;
    vec4 texcoord1;
    vec4 color;
};
struct Input {
    vec2 uv_Diffuse;
    vec2 uv_NormalMap;
};
struct v2f_surf {
    vec4 pos;
    vec4 hip_pack0;
    vec3 lightDir;
    vec3 viewDir;
    vec3 _LightCoord;
};
uniform sampler2D _Diffuse;
uniform vec4 _LightColor0;
uniform sampler2D _LightTexture0;
uniform sampler2D _NormalMap;
vec4 UnpackNormal( in vec4 packednormal );
void surf( in Input IN, inout EditorSurfaceOutput o );
float Luminance( in vec3 c );
vec4 LightingBlinnPhongEditor_PrePass( in EditorSurfaceOutput s, in vec4 light );
vec4 LightingBlinnPhongEditor( in EditorSurfaceOutput s, in vec3 lightDir, in vec3 viewDir, in float atten );
vec4 frag_surf( in v2f_surf IN );
vec4 UnpackNormal( in vec4 packednormal ) {
    vec4 normal;
    normal.xy  = ((packednormal.wy  * 2.00000) - 1.00000);
    normal.z  = sqrt( ((1.00000 - (normal.x  * normal.x )) - (normal.y  * normal.y )) );
    return normal;
}
void surf( in Input IN, inout EditorSurfaceOutput o ) {
    vec4 Tex2D1;
    vec4 Tex2D0;
    vec4 UnpackNormal0;
    vec4 Master0_2_NoInput = vec4( 0.000000, 0.000000, 0.000000, 0.000000);
    vec4 Master0_3_NoInput = vec4( 0.000000, 0.000000, 0.000000, 0.000000);
    vec4 Master0_4_NoInput = vec4( 0.000000, 0.000000, 0.000000, 0.000000);
    vec4 Master0_6_NoInput = vec4( 1.00000, 1.00000, 1.00000, 1.00000);
    vec4 _Alpha;
    o.Albedo = vec3( 0.000000);
    o.Normal = vec3( 0.000000, 0.000000, 1.00000);
    o.Emission = vec3( 0.000000);
    o.Gloss = vec3( 0.000000);
    o.Specular = 0.000000;
    o.Alpha = 1.00000;
    Tex2D1 = texture2D( _Diffuse, IN.uv_Diffuse.xyxy .xy );
    Tex2D0 = texture2D( _NormalMap, IN.uv_NormalMap.xyxy .xy );
    UnpackNormal0 = UnpackNormal( Tex2D0);
    o.Albedo = vec3( Tex2D1);
    o.Normal = vec3( UnpackNormal0);
    o.Alpha = vec4( _Alpha);
}
float Luminance( in vec3 c ) {
    return dot( c, vec3( 0.220000, 0.707000, 0.0710000));
}
vec4 LightingBlinnPhongEditor_PrePass( in EditorSurfaceOutput s, in vec4 light ) {
    vec3 spec;
    vec4 c;
    spec = (light.w  * s.Gloss);
    c.xyz  = ((s.Albedo * light.xyz ) + (light.xyz  * spec));
    c.w  = (s.Alpha + Luminance( spec));
    return c;
}
vec4 LightingBlinnPhongEditor( in EditorSurfaceOutput s, in vec3 lightDir, in vec3 viewDir, in float atten ) {
    vec3 h;
    float diff;
    float nh;
    vec3 spec;
    vec4 res;
    viewDir = normalize( viewDir );
    h = normalize( (lightDir + viewDir) );
    diff = max( 0.000000, dot( s.Normal, lightDir));
    nh = max( 0.000000, dot( s.Normal, h));
    spec = (pow( nh, (s.Specular * 128.000)) * s.Gloss);
    res.xyz  = (_LightColor0.xyz  * ((diff * atten) * 2.00000));
    res.w  = float( (spec * Luminance( _LightColor0.xyz )));
    return LightingBlinnPhongEditor_PrePass( s, res);
}
vec4 frag_surf( in v2f_surf IN ) {
    Input surfIN;
    EditorSurfaceOutput o;
    vec3 lightDir;
    vec4 c;
    surfIN.uv_Diffuse = IN.hip_pack0.xy ;
    surfIN.uv_NormalMap = IN.hip_pack0.zw ;
    o.Albedo = vec3( 0.000000);
    o.Emission = vec3( 0.000000);
    o.Specular = 0.000000;
    o.Alpha = 0.000000;
    surf( surfIN, o);
    lightDir = IN.lightDir;
    lightDir = normalize( lightDir );
    c = LightingBlinnPhongEditor( o, lightDir, normalize( vec3( IN.viewDir) ), texture2D( _LightTexture0, vec2( vec2( dot( IN._LightCoord, IN._LightCoord)))).w );
    c.w  = o.Alpha;
    return c;
}
void main() {
    vec4 xl_retval;
    v2f_surf xlt_IN;
    xlt_IN.pos = vec4(0.0);
    xlt_IN.hip_pack0 = vec4( gl_TexCoord[0]);
    xlt_IN.lightDir = vec3( gl_TexCoord[1]);
    xlt_IN.viewDir = vec3( gl_TexCoord[2]);
    xlt_IN._LightCoord = vec3( gl_TexCoord[3]);
    xl_retval = frag_surf( xlt_IN);
    gl_FragData[0] = vec4( xl_retval);
}
/* NOTE: GLSL optimization failed
0:85(13): error: type mismatch
*/

#endif
"
}

SubProgram "opengl " {
Keywords { "DIRECTIONAL" }
Bind "vertex" Vertex
Bind "tangent" ATTR14
Bind "normal" Normal
Bind "texcoord" TexCoord0
Vector 9 [unity_Scale]
Vector 10 [_WorldSpaceCameraPos]
Vector 11 [_WorldSpaceLightPos0]
Matrix 5 [_World2Object]
Vector 12 [_Diffuse_ST]
Vector 13 [_NormalMap_ST]
"3.0-!!ARBvp1.0
# 26 ALU
PARAM c[14] = { { 1 },
		state.matrix.mvp,
		program.local[5..13] };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
MOV R1.xyz, c[10];
MOV R1.w, c[0].x;
MOV R0.xyz, vertex.attrib[14];
DP4 R2.z, R1, c[7];
DP4 R2.y, R1, c[6];
DP4 R2.x, R1, c[5];
MAD R2.xyz, R2, c[9].w, -vertex.position;
MUL R1.xyz, vertex.normal.zxyw, R0.yzxw;
MAD R1.xyz, vertex.normal.yzxw, R0.zxyw, -R1;
MOV R0, c[11];
MUL R1.xyz, R1, vertex.attrib[14].w;
DP4 R3.z, R0, c[7];
DP4 R3.y, R0, c[6];
DP4 R3.x, R0, c[5];
DP3 result.texcoord[1].y, R3, R1;
DP3 result.texcoord[2].y, R1, R2;
DP3 result.texcoord[1].z, vertex.normal, R3;
DP3 result.texcoord[1].x, R3, vertex.attrib[14];
DP3 result.texcoord[2].z, vertex.normal, R2;
DP3 result.texcoord[2].x, vertex.attrib[14], R2;
MAD result.texcoord[0].zw, vertex.texcoord[0].xyxy, c[13].xyxy, c[13];
MAD result.texcoord[0].xy, vertex.texcoord[0], c[12], c[12].zwzw;
DP4 result.position.w, vertex.position, c[4];
DP4 result.position.z, vertex.position, c[3];
DP4 result.position.y, vertex.position, c[2];
DP4 result.position.x, vertex.position, c[1];
END
# 26 instructions, 4 R-regs
"
}

SubProgram "d3d9 " {
Keywords { "DIRECTIONAL" }
Bind "vertex" Vertex
Bind "tangent" TexCoord2
Bind "normal" Normal
Bind "texcoord" TexCoord0
Matrix 0 [glstate_matrix_mvp]
Vector 8 [unity_Scale]
Vector 9 [_WorldSpaceCameraPos]
Vector 10 [_WorldSpaceLightPos0]
Matrix 4 [_World2Object]
Vector 11 [_Diffuse_ST]
Vector 12 [_NormalMap_ST]
"vs_3_0
; 29 ALU
dcl_position o0
dcl_texcoord0 o1
dcl_texcoord1 o2
dcl_texcoord2 o3
def c13, 1.00000000, 0, 0, 0
dcl_position0 v0
dcl_tangent0 v1
dcl_normal0 v2
dcl_texcoord0 v3
mov r0.w, c13.x
mov r0.xyz, c9
dp4 r1.z, r0, c6
dp4 r1.y, r0, c5
dp4 r1.x, r0, c4
mad r3.xyz, r1, c8.w, -v0
mov r0.xyz, v1
mul r1.xyz, v2.zxyw, r0.yzxw
mov r0.xyz, v1
mad r1.xyz, v2.yzxw, r0.zxyw, -r1
mul r2.xyz, r1, v1.w
mov r0, c6
dp4 r4.z, c10, r0
mov r0, c5
mov r1, c4
dp4 r4.y, c10, r0
dp4 r4.x, c10, r1
dp3 o2.y, r4, r2
dp3 o3.y, r2, r3
dp3 o2.z, v2, r4
dp3 o2.x, r4, v1
dp3 o3.z, v2, r3
dp3 o3.x, v1, r3
mad o1.zw, v3.xyxy, c12.xyxy, c12
mad o1.xy, v3, c11, c11.zwzw
dp4 o0.w, v0, c3
dp4 o0.z, v0, c2
dp4 o0.y, v0, c1
dp4 o0.x, v0, c0
"
}

SubProgram "gles " {
Keywords { "DIRECTIONAL" }
"!!GLES

#ifdef VERTEX
attribute vec4 TANGENT;
uniform vec4 unity_Scale;
uniform vec4 _WorldSpaceLightPos0;
uniform vec3 _WorldSpaceCameraPos;
uniform mat4 _World2Object;
uniform vec4 _NormalMap_ST;
uniform vec4 _Diffuse_ST;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1 = gl_Vertex.xyzw;
  vec4 tmpvar_3;
  tmpvar_3 = TANGENT.xyzw;
  vec3 tmpvar_5;
  tmpvar_5 = gl_Normal.xyz;
  vec4 tmpvar_7;
  tmpvar_7 = gl_MultiTexCoord0.xyzw;
  vec4 tmpvar_91;
  tmpvar_91.xy = ((tmpvar_7.xy * _Diffuse_ST.xy) + _Diffuse_ST.zw);
  tmpvar_91.zw = ((tmpvar_7.xy * _NormalMap_ST.xy) + _NormalMap_ST.zw).xy;
  vec3 tmpvar_41;
  tmpvar_41 = (cross (tmpvar_5, tmpvar_3.xyz) * tmpvar_3.w);
  mat3 tmpvar_42;
  tmpvar_42[0].x = tmpvar_3.x;
  tmpvar_42[0].y = tmpvar_41.x;
  tmpvar_42[0].z = tmpvar_5.x;
  tmpvar_42[1].x = tmpvar_3.y;
  tmpvar_42[1].y = tmpvar_41.y;
  tmpvar_42[1].z = tmpvar_5.y;
  tmpvar_42[2].x = tmpvar_3.z;
  tmpvar_42[2].y = tmpvar_41.z;
  tmpvar_42[2].z = tmpvar_5.z;
  vec4 tmpvar_59;
  tmpvar_59.xyz = _WorldSpaceCameraPos.xyz;
  tmpvar_59.w = 1.0;
  gl_Position = (gl_ModelViewProjectionMatrix * tmpvar_1).xyzw;
  gl_TexCoord[0] = tmpvar_91.xyzw;
  vec4 tmpvar_19;
  tmpvar_19.xyz = (tmpvar_42 * (_World2Object * _WorldSpaceLightPos0).xyz).xyz;
  tmpvar_19.w = 0.0;
  gl_TexCoord[1] = tmpvar_19;
  vec4 tmpvar_21;
  tmpvar_21.xyz = (tmpvar_42 * (((_World2Object * tmpvar_59).xyz * unity_Scale.w) - tmpvar_1.xyz)).xyz;
  tmpvar_21.w = 0.0;
  gl_TexCoord[2] = tmpvar_21;
}


#endif
#ifdef FRAGMENT
struct v2f_vertex_lit {
    vec2 uv;
    vec4 diff;
    vec4 spec;
};
struct v2f_img {
    vec4 pos;
    vec2 uv;
};
struct appdata_img {
    vec4 vertex;
    vec2 texcoord;
};
struct SurfaceOutput {
    vec3 Albedo;
    vec3 Normal;
    vec3 Emission;
    float Specular;
    float Gloss;
    float Alpha;
};
struct EditorSurfaceOutput {
    vec3 Albedo;
    vec3 Normal;
    vec3 Emission;
    vec3 Gloss;
    float Specular;
    float Alpha;
};
struct appdata_full {
    vec4 vertex;
    vec4 tangent;
    vec3 normal;
    vec4 texcoord;
    vec4 texcoord1;
    vec4 color;
};
struct Input {
    vec2 uv_Diffuse;
    vec2 uv_NormalMap;
};
struct v2f_surf {
    vec4 pos;
    vec4 hip_pack0;
    vec3 lightDir;
    vec3 viewDir;
};
uniform sampler2D _Diffuse;
uniform vec4 _LightColor0;
uniform sampler2D _NormalMap;
vec4 UnpackNormal( in vec4 packednormal );
void surf( in Input IN, inout EditorSurfaceOutput o );
float Luminance( in vec3 c );
vec4 LightingBlinnPhongEditor_PrePass( in EditorSurfaceOutput s, in vec4 light );
vec4 LightingBlinnPhongEditor( in EditorSurfaceOutput s, in vec3 lightDir, in vec3 viewDir, in float atten );
vec4 frag_surf( in v2f_surf IN );
vec4 UnpackNormal( in vec4 packednormal ) {
    vec4 normal;
    normal.xy  = ((packednormal.wy  * 2.00000) - 1.00000);
    normal.z  = sqrt( ((1.00000 - (normal.x  * normal.x )) - (normal.y  * normal.y )) );
    return normal;
}
void surf( in Input IN, inout EditorSurfaceOutput o ) {
    vec4 Tex2D1;
    vec4 Tex2D0;
    vec4 UnpackNormal0;
    vec4 Master0_2_NoInput = vec4( 0.000000, 0.000000, 0.000000, 0.000000);
    vec4 Master0_3_NoInput = vec4( 0.000000, 0.000000, 0.000000, 0.000000);
    vec4 Master0_4_NoInput = vec4( 0.000000, 0.000000, 0.000000, 0.000000);
    vec4 Master0_6_NoInput = vec4( 1.00000, 1.00000, 1.00000, 1.00000);
    vec4 _Alpha;
    o.Albedo = vec3( 0.000000);
    o.Normal = vec3( 0.000000, 0.000000, 1.00000);
    o.Emission = vec3( 0.000000);
    o.Gloss = vec3( 0.000000);
    o.Specular = 0.000000;
    o.Alpha = 1.00000;
    Tex2D1 = texture2D( _Diffuse, IN.uv_Diffuse.xyxy .xy );
    Tex2D0 = texture2D( _NormalMap, IN.uv_NormalMap.xyxy .xy );
    UnpackNormal0 = UnpackNormal( Tex2D0);
    o.Albedo = vec3( Tex2D1);
    o.Normal = vec3( UnpackNormal0);
    o.Alpha = vec4( _Alpha);
}
float Luminance( in vec3 c ) {
    return dot( c, vec3( 0.220000, 0.707000, 0.0710000));
}
vec4 LightingBlinnPhongEditor_PrePass( in EditorSurfaceOutput s, in vec4 light ) {
    vec3 spec;
    vec4 c;
    spec = (light.w  * s.Gloss);
    c.xyz  = ((s.Albedo * light.xyz ) + (light.xyz  * spec));
    c.w  = (s.Alpha + Luminance( spec));
    return c;
}
vec4 LightingBlinnPhongEditor( in EditorSurfaceOutput s, in vec3 lightDir, in vec3 viewDir, in float atten ) {
    vec3 h;
    float diff;
    float nh;
    vec3 spec;
    vec4 res;
    viewDir = normalize( viewDir );
    h = normalize( (lightDir + viewDir) );
    diff = max( 0.000000, dot( s.Normal, lightDir));
    nh = max( 0.000000, dot( s.Normal, h));
    spec = (pow( nh, (s.Specular * 128.000)) * s.Gloss);
    res.xyz  = (_LightColor0.xyz  * ((diff * atten) * 2.00000));
    res.w  = float( (spec * Luminance( _LightColor0.xyz )));
    return LightingBlinnPhongEditor_PrePass( s, res);
}
vec4 frag_surf( in v2f_surf IN ) {
    Input surfIN;
    EditorSurfaceOutput o;
    vec3 lightDir;
    vec4 c;
    surfIN.uv_Diffuse = IN.hip_pack0.xy ;
    surfIN.uv_NormalMap = IN.hip_pack0.zw ;
    o.Albedo = vec3( 0.000000);
    o.Emission = vec3( 0.000000);
    o.Specular = 0.000000;
    o.Alpha = 0.000000;
    surf( surfIN, o);
    lightDir = IN.lightDir;
    c = LightingBlinnPhongEditor( o, lightDir, normalize( vec3( IN.viewDir) ), 1.00000);
    c.w  = o.Alpha;
    return c;
}
void main() {
    vec4 xl_retval;
    v2f_surf xlt_IN;
    xlt_IN.pos = vec4(0.0);
    xlt_IN.hip_pack0 = vec4( gl_TexCoord[0]);
    xlt_IN.lightDir = vec3( gl_TexCoord[1]);
    xlt_IN.viewDir = vec3( gl_TexCoord[2]);
    xl_retval = frag_surf( xlt_IN);
    gl_FragData[0] = vec4( xl_retval);
}
/* NOTE: GLSL optimization failed
0:83(13): error: type mismatch
*/

#endif
"
}

SubProgram "opengl " {
Keywords { "SPOT" }
Bind "vertex" Vertex
Bind "tangent" ATTR14
Bind "normal" Normal
Bind "texcoord" TexCoord0
Vector 17 [unity_Scale]
Vector 18 [_WorldSpaceCameraPos]
Vector 19 [_WorldSpaceLightPos0]
Matrix 5 [_Object2World]
Matrix 9 [_World2Object]
Matrix 13 [_LightMatrix0]
Vector 20 [_Diffuse_ST]
Vector 21 [_NormalMap_ST]
"3.0-!!ARBvp1.0
# 35 ALU
PARAM c[22] = { { 1 },
		state.matrix.mvp,
		program.local[5..21] };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
MOV R1.xyz, c[18];
MOV R1.w, c[0].x;
MOV R0.xyz, vertex.attrib[14];
DP4 R2.z, R1, c[11];
DP4 R2.y, R1, c[10];
DP4 R2.x, R1, c[9];
MAD R2.xyz, R2, c[17].w, -vertex.position;
MUL R1.xyz, vertex.normal.zxyw, R0.yzxw;
MAD R1.xyz, vertex.normal.yzxw, R0.zxyw, -R1;
MOV R0, c[19];
MUL R1.xyz, R1, vertex.attrib[14].w;
DP4 R3.z, R0, c[11];
DP4 R3.x, R0, c[9];
DP4 R3.y, R0, c[10];
MAD R0.xyz, R3, c[17].w, -vertex.position;
DP4 R0.w, vertex.position, c[8];
DP3 result.texcoord[1].y, R0, R1;
DP3 result.texcoord[1].z, vertex.normal, R0;
DP3 result.texcoord[1].x, R0, vertex.attrib[14];
DP4 R0.z, vertex.position, c[7];
DP4 R0.x, vertex.position, c[5];
DP4 R0.y, vertex.position, c[6];
DP3 result.texcoord[2].y, R1, R2;
DP3 result.texcoord[2].z, vertex.normal, R2;
DP3 result.texcoord[2].x, vertex.attrib[14], R2;
DP4 result.texcoord[3].w, R0, c[16];
DP4 result.texcoord[3].z, R0, c[15];
DP4 result.texcoord[3].y, R0, c[14];
DP4 result.texcoord[3].x, R0, c[13];
MAD result.texcoord[0].zw, vertex.texcoord[0].xyxy, c[21].xyxy, c[21];
MAD result.texcoord[0].xy, vertex.texcoord[0], c[20], c[20].zwzw;
DP4 result.position.w, vertex.position, c[4];
DP4 result.position.z, vertex.position, c[3];
DP4 result.position.y, vertex.position, c[2];
DP4 result.position.x, vertex.position, c[1];
END
# 35 instructions, 4 R-regs
"
}

SubProgram "d3d9 " {
Keywords { "SPOT" }
Bind "vertex" Vertex
Bind "tangent" TexCoord2
Bind "normal" Normal
Bind "texcoord" TexCoord0
Matrix 0 [glstate_matrix_mvp]
Vector 16 [unity_Scale]
Vector 17 [_WorldSpaceCameraPos]
Vector 18 [_WorldSpaceLightPos0]
Matrix 4 [_Object2World]
Matrix 8 [_World2Object]
Matrix 12 [_LightMatrix0]
Vector 19 [_Diffuse_ST]
Vector 20 [_NormalMap_ST]
"vs_3_0
; 38 ALU
dcl_position o0
dcl_texcoord0 o1
dcl_texcoord1 o2
dcl_texcoord2 o3
dcl_texcoord3 o4
def c21, 1.00000000, 0, 0, 0
dcl_position0 v0
dcl_tangent0 v1
dcl_normal0 v2
dcl_texcoord0 v3
mov r0.w, c21.x
mov r0.xyz, c17
dp4 r1.z, r0, c10
dp4 r1.y, r0, c9
dp4 r1.x, r0, c8
mad r3.xyz, r1, c16.w, -v0
mov r0.xyz, v1
mul r1.xyz, v2.zxyw, r0.yzxw
mov r0.xyz, v1
mad r1.xyz, v2.yzxw, r0.zxyw, -r1
mul r2.xyz, r1, v1.w
mov r0, c10
dp4 r4.z, c18, r0
mov r0, c9
dp4 r4.y, c18, r0
mov r1, c8
dp4 r4.x, c18, r1
mad r0.xyz, r4, c16.w, -v0
dp4 r0.w, v0, c7
dp3 o2.y, r0, r2
dp3 o2.z, v2, r0
dp3 o2.x, r0, v1
dp4 r0.z, v0, c6
dp4 r0.x, v0, c4
dp4 r0.y, v0, c5
dp3 o3.y, r2, r3
dp3 o3.z, v2, r3
dp3 o3.x, v1, r3
dp4 o4.w, r0, c15
dp4 o4.z, r0, c14
dp4 o4.y, r0, c13
dp4 o4.x, r0, c12
mad o1.zw, v3.xyxy, c20.xyxy, c20
mad o1.xy, v3, c19, c19.zwzw
dp4 o0.w, v0, c3
dp4 o0.z, v0, c2
dp4 o0.y, v0, c1
dp4 o0.x, v0, c0
"
}

SubProgram "gles " {
Keywords { "SPOT" }
"!!GLES

#ifdef VERTEX
attribute vec4 TANGENT;
uniform vec4 unity_Scale;
uniform vec4 _WorldSpaceLightPos0;
uniform vec3 _WorldSpaceCameraPos;
uniform mat4 _World2Object;
uniform mat4 _Object2World;
uniform vec4 _NormalMap_ST;
uniform mat4 _LightMatrix0;
uniform vec4 _Diffuse_ST;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1 = gl_Vertex.xyzw;
  vec4 tmpvar_3;
  tmpvar_3 = TANGENT.xyzw;
  vec3 tmpvar_5;
  tmpvar_5 = gl_Normal.xyz;
  vec4 tmpvar_7;
  tmpvar_7 = gl_MultiTexCoord0.xyzw;
  vec4 tmpvar_98;
  tmpvar_98.xy = ((tmpvar_7.xy * _Diffuse_ST.xy) + _Diffuse_ST.zw);
  tmpvar_98.zw = ((tmpvar_7.xy * _NormalMap_ST.xy) + _NormalMap_ST.zw).xy;
  vec3 tmpvar_43;
  tmpvar_43 = (cross (tmpvar_5, tmpvar_3.xyz) * tmpvar_3.w);
  mat3 tmpvar_44;
  tmpvar_44[0].x = tmpvar_3.x;
  tmpvar_44[0].y = tmpvar_43.x;
  tmpvar_44[0].z = tmpvar_5.x;
  tmpvar_44[1].x = tmpvar_3.y;
  tmpvar_44[1].y = tmpvar_43.y;
  tmpvar_44[1].z = tmpvar_5.y;
  tmpvar_44[2].x = tmpvar_3.z;
  tmpvar_44[2].y = tmpvar_43.z;
  tmpvar_44[2].z = tmpvar_5.z;
  vec4 tmpvar_61;
  tmpvar_61.xyz = _WorldSpaceCameraPos.xyz;
  tmpvar_61.w = 1.0;
  gl_Position = (gl_ModelViewProjectionMatrix * tmpvar_1).xyzw;
  gl_TexCoord[0] = tmpvar_98.xyzw;
  vec4 tmpvar_19;
  tmpvar_19.xyz = (tmpvar_44 * (((_World2Object * _WorldSpaceLightPos0).xyz * unity_Scale.w) - tmpvar_1.xyz)).xyz;
  tmpvar_19.w = 0.0;
  gl_TexCoord[1] = tmpvar_19;
  vec4 tmpvar_21;
  tmpvar_21.xyz = (tmpvar_44 * (((_World2Object * tmpvar_61).xyz * unity_Scale.w) - tmpvar_1.xyz)).xyz;
  tmpvar_21.w = 0.0;
  gl_TexCoord[2] = tmpvar_21;
  gl_TexCoord[3] = (_LightMatrix0 * (_Object2World * tmpvar_1)).xyzw;
}


#endif
#ifdef FRAGMENT
struct v2f_vertex_lit {
    vec2 uv;
    vec4 diff;
    vec4 spec;
};
struct v2f_img {
    vec4 pos;
    vec2 uv;
};
struct appdata_img {
    vec4 vertex;
    vec2 texcoord;
};
struct SurfaceOutput {
    vec3 Albedo;
    vec3 Normal;
    vec3 Emission;
    float Specular;
    float Gloss;
    float Alpha;
};
struct EditorSurfaceOutput {
    vec3 Albedo;
    vec3 Normal;
    vec3 Emission;
    vec3 Gloss;
    float Specular;
    float Alpha;
};
struct appdata_full {
    vec4 vertex;
    vec4 tangent;
    vec3 normal;
    vec4 texcoord;
    vec4 texcoord1;
    vec4 color;
};
struct Input {
    vec2 uv_Diffuse;
    vec2 uv_NormalMap;
};
struct v2f_surf {
    vec4 pos;
    vec4 hip_pack0;
    vec3 lightDir;
    vec3 viewDir;
    vec4 _LightCoord;
};
uniform sampler2D _Diffuse;
uniform vec4 _LightColor0;
uniform sampler2D _LightTexture0;
uniform sampler2D _LightTextureB0;
uniform sampler2D _NormalMap;
vec4 UnpackNormal( in vec4 packednormal );
void surf( in Input IN, inout EditorSurfaceOutput o );
float UnitySpotCookie( in vec4 LightCoord );
float UnitySpotAttenuate( in vec3 LightCoord );
float Luminance( in vec3 c );
vec4 LightingBlinnPhongEditor_PrePass( in EditorSurfaceOutput s, in vec4 light );
vec4 LightingBlinnPhongEditor( in EditorSurfaceOutput s, in vec3 lightDir, in vec3 viewDir, in float atten );
vec4 frag_surf( in v2f_surf IN );
vec4 UnpackNormal( in vec4 packednormal ) {
    vec4 normal;
    normal.xy  = ((packednormal.wy  * 2.00000) - 1.00000);
    normal.z  = sqrt( ((1.00000 - (normal.x  * normal.x )) - (normal.y  * normal.y )) );
    return normal;
}
void surf( in Input IN, inout EditorSurfaceOutput o ) {
    vec4 Tex2D1;
    vec4 Tex2D0;
    vec4 UnpackNormal0;
    vec4 Master0_2_NoInput = vec4( 0.000000, 0.000000, 0.000000, 0.000000);
    vec4 Master0_3_NoInput = vec4( 0.000000, 0.000000, 0.000000, 0.000000);
    vec4 Master0_4_NoInput = vec4( 0.000000, 0.000000, 0.000000, 0.000000);
    vec4 Master0_6_NoInput = vec4( 1.00000, 1.00000, 1.00000, 1.00000);
    vec4 _Alpha;
    o.Albedo = vec3( 0.000000);
    o.Normal = vec3( 0.000000, 0.000000, 1.00000);
    o.Emission = vec3( 0.000000);
    o.Gloss = vec3( 0.000000);
    o.Specular = 0.000000;
    o.Alpha = 1.00000;
    Tex2D1 = texture2D( _Diffuse, IN.uv_Diffuse.xyxy .xy );
    Tex2D0 = texture2D( _NormalMap, IN.uv_NormalMap.xyxy .xy );
    UnpackNormal0 = UnpackNormal( Tex2D0);
    o.Albedo = vec3( Tex2D1);
    o.Normal = vec3( UnpackNormal0);
    o.Alpha = vec4( _Alpha);
}
float UnitySpotCookie( in vec4 LightCoord ) {
    return texture2D( _LightTexture0, ((LightCoord.xy  / LightCoord.w ) + 0.500000)).w ;
}
float UnitySpotAttenuate( in vec3 LightCoord ) {
    return texture2D( _LightTextureB0, vec2( vec2( dot( LightCoord, LightCoord)))).w ;
}
float Luminance( in vec3 c ) {
    return dot( c, vec3( 0.220000, 0.707000, 0.0710000));
}
vec4 LightingBlinnPhongEditor_PrePass( in EditorSurfaceOutput s, in vec4 light ) {
    vec3 spec;
    vec4 c;
    spec = (light.w  * s.Gloss);
    c.xyz  = ((s.Albedo * light.xyz ) + (light.xyz  * spec));
    c.w  = (s.Alpha + Luminance( spec));
    return c;
}
vec4 LightingBlinnPhongEditor( in EditorSurfaceOutput s, in vec3 lightDir, in vec3 viewDir, in float atten ) {
    vec3 h;
    float diff;
    float nh;
    vec3 spec;
    vec4 res;
    viewDir = normalize( viewDir );
    h = normalize( (lightDir + viewDir) );
    diff = max( 0.000000, dot( s.Normal, lightDir));
    nh = max( 0.000000, dot( s.Normal, h));
    spec = (pow( nh, (s.Specular * 128.000)) * s.Gloss);
    res.xyz  = (_LightColor0.xyz  * ((diff * atten) * 2.00000));
    res.w  = float( (spec * Luminance( _LightColor0.xyz )));
    return LightingBlinnPhongEditor_PrePass( s, res);
}
vec4 frag_surf( in v2f_surf IN ) {
    Input surfIN;
    EditorSurfaceOutput o;
    vec3 lightDir;
    vec4 c;
    surfIN.uv_Diffuse = IN.hip_pack0.xy ;
    surfIN.uv_NormalMap = IN.hip_pack0.zw ;
    o.Albedo = vec3( 0.000000);
    o.Emission = vec3( 0.000000);
    o.Specular = 0.000000;
    o.Alpha = 0.000000;
    surf( surfIN, o);
    lightDir = IN.lightDir;
    lightDir = normalize( lightDir );
    c = LightingBlinnPhongEditor( o, lightDir, normalize( vec3( IN.viewDir) ), ((float( (IN._LightCoord.z  > 0.000000) ) * UnitySpotCookie( IN._LightCoord)) * UnitySpotAttenuate( IN._LightCoord.xyz )));
    c.w  = o.Alpha;
    return c;
}
void main() {
    vec4 xl_retval;
    v2f_surf xlt_IN;
    xlt_IN.pos = vec4(0.0);
    xlt_IN.hip_pack0 = vec4( gl_TexCoord[0]);
    xlt_IN.lightDir = vec3( gl_TexCoord[1]);
    xlt_IN.viewDir = vec3( gl_TexCoord[2]);
    xlt_IN._LightCoord = vec4( gl_TexCoord[3]);
    xl_retval = frag_surf( xlt_IN);
    gl_FragData[0] = vec4( xl_retval);
}
/* NOTE: GLSL optimization failed
0:88(13): error: type mismatch
*/

#endif
"
}

SubProgram "opengl " {
Keywords { "POINT_COOKIE" }
Bind "vertex" Vertex
Bind "tangent" ATTR14
Bind "normal" Normal
Bind "texcoord" TexCoord0
Vector 17 [unity_Scale]
Vector 18 [_WorldSpaceCameraPos]
Vector 19 [_WorldSpaceLightPos0]
Matrix 5 [_Object2World]
Matrix 9 [_World2Object]
Matrix 13 [_LightMatrix0]
Vector 20 [_Diffuse_ST]
Vector 21 [_NormalMap_ST]
"3.0-!!ARBvp1.0
# 34 ALU
PARAM c[22] = { { 1 },
		state.matrix.mvp,
		program.local[5..21] };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
MOV R1.xyz, c[18];
MOV R1.w, c[0].x;
MOV R0.xyz, vertex.attrib[14];
DP4 R2.z, R1, c[11];
DP4 R2.y, R1, c[10];
DP4 R2.x, R1, c[9];
MAD R2.xyz, R2, c[17].w, -vertex.position;
MUL R1.xyz, vertex.normal.zxyw, R0.yzxw;
MAD R1.xyz, vertex.normal.yzxw, R0.zxyw, -R1;
MOV R0, c[19];
MUL R1.xyz, R1, vertex.attrib[14].w;
DP4 R3.z, R0, c[11];
DP4 R3.x, R0, c[9];
DP4 R3.y, R0, c[10];
MAD R0.xyz, R3, c[17].w, -vertex.position;
DP3 result.texcoord[1].y, R0, R1;
DP3 result.texcoord[1].z, vertex.normal, R0;
DP3 result.texcoord[1].x, R0, vertex.attrib[14];
DP4 R0.w, vertex.position, c[8];
DP4 R0.z, vertex.position, c[7];
DP4 R0.x, vertex.position, c[5];
DP4 R0.y, vertex.position, c[6];
DP3 result.texcoord[2].y, R1, R2;
DP3 result.texcoord[2].z, vertex.normal, R2;
DP3 result.texcoord[2].x, vertex.attrib[14], R2;
DP4 result.texcoord[3].z, R0, c[15];
DP4 result.texcoord[3].y, R0, c[14];
DP4 result.texcoord[3].x, R0, c[13];
MAD result.texcoord[0].zw, vertex.texcoord[0].xyxy, c[21].xyxy, c[21];
MAD result.texcoord[0].xy, vertex.texcoord[0], c[20], c[20].zwzw;
DP4 result.position.w, vertex.position, c[4];
DP4 result.position.z, vertex.position, c[3];
DP4 result.position.y, vertex.position, c[2];
DP4 result.position.x, vertex.position, c[1];
END
# 34 instructions, 4 R-regs
"
}

SubProgram "d3d9 " {
Keywords { "POINT_COOKIE" }
Bind "vertex" Vertex
Bind "tangent" TexCoord2
Bind "normal" Normal
Bind "texcoord" TexCoord0
Matrix 0 [glstate_matrix_mvp]
Vector 16 [unity_Scale]
Vector 17 [_WorldSpaceCameraPos]
Vector 18 [_WorldSpaceLightPos0]
Matrix 4 [_Object2World]
Matrix 8 [_World2Object]
Matrix 12 [_LightMatrix0]
Vector 19 [_Diffuse_ST]
Vector 20 [_NormalMap_ST]
"vs_3_0
; 37 ALU
dcl_position o0
dcl_texcoord0 o1
dcl_texcoord1 o2
dcl_texcoord2 o3
dcl_texcoord3 o4
def c21, 1.00000000, 0, 0, 0
dcl_position0 v0
dcl_tangent0 v1
dcl_normal0 v2
dcl_texcoord0 v3
mov r0.w, c21.x
mov r0.xyz, c17
dp4 r1.z, r0, c10
dp4 r1.y, r0, c9
dp4 r1.x, r0, c8
mad r3.xyz, r1, c16.w, -v0
mov r0.xyz, v1
mul r1.xyz, v2.zxyw, r0.yzxw
mov r0.xyz, v1
mad r1.xyz, v2.yzxw, r0.zxyw, -r1
mul r2.xyz, r1, v1.w
mov r0, c10
dp4 r4.z, c18, r0
mov r0, c9
dp4 r4.y, c18, r0
mov r1, c8
dp4 r4.x, c18, r1
mad r0.xyz, r4, c16.w, -v0
dp3 o2.y, r0, r2
dp3 o2.z, v2, r0
dp3 o2.x, r0, v1
dp4 r0.w, v0, c7
dp4 r0.z, v0, c6
dp4 r0.x, v0, c4
dp4 r0.y, v0, c5
dp3 o3.y, r2, r3
dp3 o3.z, v2, r3
dp3 o3.x, v1, r3
dp4 o4.z, r0, c14
dp4 o4.y, r0, c13
dp4 o4.x, r0, c12
mad o1.zw, v3.xyxy, c20.xyxy, c20
mad o1.xy, v3, c19, c19.zwzw
dp4 o0.w, v0, c3
dp4 o0.z, v0, c2
dp4 o0.y, v0, c1
dp4 o0.x, v0, c0
"
}

SubProgram "gles " {
Keywords { "POINT_COOKIE" }
"!!GLES

#ifdef VERTEX
attribute vec4 TANGENT;
uniform vec4 unity_Scale;
uniform vec4 _WorldSpaceLightPos0;
uniform vec3 _WorldSpaceCameraPos;
uniform mat4 _World2Object;
uniform mat4 _Object2World;
uniform vec4 _NormalMap_ST;
uniform mat4 _LightMatrix0;
uniform vec4 _Diffuse_ST;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1 = gl_Vertex.xyzw;
  vec4 tmpvar_3;
  tmpvar_3 = TANGENT.xyzw;
  vec3 tmpvar_5;
  tmpvar_5 = gl_Normal.xyz;
  vec4 tmpvar_7;
  tmpvar_7 = gl_MultiTexCoord0.xyzw;
  vec4 tmpvar_98;
  tmpvar_98.xy = ((tmpvar_7.xy * _Diffuse_ST.xy) + _Diffuse_ST.zw);
  tmpvar_98.zw = ((tmpvar_7.xy * _NormalMap_ST.xy) + _NormalMap_ST.zw).xy;
  vec3 tmpvar_43;
  tmpvar_43 = (cross (tmpvar_5, tmpvar_3.xyz) * tmpvar_3.w);
  mat3 tmpvar_44;
  tmpvar_44[0].x = tmpvar_3.x;
  tmpvar_44[0].y = tmpvar_43.x;
  tmpvar_44[0].z = tmpvar_5.x;
  tmpvar_44[1].x = tmpvar_3.y;
  tmpvar_44[1].y = tmpvar_43.y;
  tmpvar_44[1].z = tmpvar_5.y;
  tmpvar_44[2].x = tmpvar_3.z;
  tmpvar_44[2].y = tmpvar_43.z;
  tmpvar_44[2].z = tmpvar_5.z;
  vec4 tmpvar_61;
  tmpvar_61.xyz = _WorldSpaceCameraPos.xyz;
  tmpvar_61.w = 1.0;
  gl_Position = (gl_ModelViewProjectionMatrix * tmpvar_1).xyzw;
  gl_TexCoord[0] = tmpvar_98.xyzw;
  vec4 tmpvar_19;
  tmpvar_19.xyz = (tmpvar_44 * (((_World2Object * _WorldSpaceLightPos0).xyz * unity_Scale.w) - tmpvar_1.xyz)).xyz;
  tmpvar_19.w = 0.0;
  gl_TexCoord[1] = tmpvar_19;
  vec4 tmpvar_21;
  tmpvar_21.xyz = (tmpvar_44 * (((_World2Object * tmpvar_61).xyz * unity_Scale.w) - tmpvar_1.xyz)).xyz;
  tmpvar_21.w = 0.0;
  gl_TexCoord[2] = tmpvar_21;
  vec4 tmpvar_23;
  tmpvar_23.xyz = (_LightMatrix0 * (_Object2World * tmpvar_1)).xyz;
  tmpvar_23.w = 0.0;
  gl_TexCoord[3] = tmpvar_23;
}


#endif
#ifdef FRAGMENT
struct v2f_vertex_lit {
    vec2 uv;
    vec4 diff;
    vec4 spec;
};
struct v2f_img {
    vec4 pos;
    vec2 uv;
};
struct appdata_img {
    vec4 vertex;
    vec2 texcoord;
};
struct SurfaceOutput {
    vec3 Albedo;
    vec3 Normal;
    vec3 Emission;
    float Specular;
    float Gloss;
    float Alpha;
};
struct EditorSurfaceOutput {
    vec3 Albedo;
    vec3 Normal;
    vec3 Emission;
    vec3 Gloss;
    float Specular;
    float Alpha;
};
struct appdata_full {
    vec4 vertex;
    vec4 tangent;
    vec3 normal;
    vec4 texcoord;
    vec4 texcoord1;
    vec4 color;
};
struct Input {
    vec2 uv_Diffuse;
    vec2 uv_NormalMap;
};
struct v2f_surf {
    vec4 pos;
    vec4 hip_pack0;
    vec3 lightDir;
    vec3 viewDir;
    vec3 _LightCoord;
};
uniform sampler2D _Diffuse;
uniform vec4 _LightColor0;
uniform samplerCube _LightTexture0;
uniform sampler2D _LightTextureB0;
uniform sampler2D _NormalMap;
vec4 UnpackNormal( in vec4 packednormal );
void surf( in Input IN, inout EditorSurfaceOutput o );
float Luminance( in vec3 c );
vec4 LightingBlinnPhongEditor_PrePass( in EditorSurfaceOutput s, in vec4 light );
vec4 LightingBlinnPhongEditor( in EditorSurfaceOutput s, in vec3 lightDir, in vec3 viewDir, in float atten );
vec4 frag_surf( in v2f_surf IN );
vec4 UnpackNormal( in vec4 packednormal ) {
    vec4 normal;
    normal.xy  = ((packednormal.wy  * 2.00000) - 1.00000);
    normal.z  = sqrt( ((1.00000 - (normal.x  * normal.x )) - (normal.y  * normal.y )) );
    return normal;
}
void surf( in Input IN, inout EditorSurfaceOutput o ) {
    vec4 Tex2D1;
    vec4 Tex2D0;
    vec4 UnpackNormal0;
    vec4 Master0_2_NoInput = vec4( 0.000000, 0.000000, 0.000000, 0.000000);
    vec4 Master0_3_NoInput = vec4( 0.000000, 0.000000, 0.000000, 0.000000);
    vec4 Master0_4_NoInput = vec4( 0.000000, 0.000000, 0.000000, 0.000000);
    vec4 Master0_6_NoInput = vec4( 1.00000, 1.00000, 1.00000, 1.00000);
    vec4 _Alpha;
    o.Albedo = vec3( 0.000000);
    o.Normal = vec3( 0.000000, 0.000000, 1.00000);
    o.Emission = vec3( 0.000000);
    o.Gloss = vec3( 0.000000);
    o.Specular = 0.000000;
    o.Alpha = 1.00000;
    Tex2D1 = texture2D( _Diffuse, IN.uv_Diffuse.xyxy .xy );
    Tex2D0 = texture2D( _NormalMap, IN.uv_NormalMap.xyxy .xy );
    UnpackNormal0 = UnpackNormal( Tex2D0);
    o.Albedo = vec3( Tex2D1);
    o.Normal = vec3( UnpackNormal0);
    o.Alpha = vec4( _Alpha);
}
float Luminance( in vec3 c ) {
    return dot( c, vec3( 0.220000, 0.707000, 0.0710000));
}
vec4 LightingBlinnPhongEditor_PrePass( in EditorSurfaceOutput s, in vec4 light ) {
    vec3 spec;
    vec4 c;
    spec = (light.w  * s.Gloss);
    c.xyz  = ((s.Albedo * light.xyz ) + (light.xyz  * spec));
    c.w  = (s.Alpha + Luminance( spec));
    return c;
}
vec4 LightingBlinnPhongEditor( in EditorSurfaceOutput s, in vec3 lightDir, in vec3 viewDir, in float atten ) {
    vec3 h;
    float diff;
    float nh;
    vec3 spec;
    vec4 res;
    viewDir = normalize( viewDir );
    h = normalize( (lightDir + viewDir) );
    diff = max( 0.000000, dot( s.Normal, lightDir));
    nh = max( 0.000000, dot( s.Normal, h));
    spec = (pow( nh, (s.Specular * 128.000)) * s.Gloss);
    res.xyz  = (_LightColor0.xyz  * ((diff * atten) * 2.00000));
    res.w  = float( (spec * Luminance( _LightColor0.xyz )));
    return LightingBlinnPhongEditor_PrePass( s, res);
}
vec4 frag_surf( in v2f_surf IN ) {
    Input surfIN;
    EditorSurfaceOutput o;
    vec3 lightDir;
    vec4 c;
    surfIN.uv_Diffuse = IN.hip_pack0.xy ;
    surfIN.uv_NormalMap = IN.hip_pack0.zw ;
    o.Albedo = vec3( 0.000000);
    o.Emission = vec3( 0.000000);
    o.Specular = 0.000000;
    o.Alpha = 0.000000;
    surf( surfIN, o);
    lightDir = IN.lightDir;
    lightDir = normalize( lightDir );
    c = LightingBlinnPhongEditor( o, lightDir, normalize( vec3( IN.viewDir) ), (texture2D( _LightTextureB0, vec2( vec2( dot( IN._LightCoord, IN._LightCoord)))).w  * textureCube( _LightTexture0, IN._LightCoord).w ));
    c.w  = o.Alpha;
    return c;
}
void main() {
    vec4 xl_retval;
    v2f_surf xlt_IN;
    xlt_IN.pos = vec4(0.0);
    xlt_IN.hip_pack0 = vec4( gl_TexCoord[0]);
    xlt_IN.lightDir = vec3( gl_TexCoord[1]);
    xlt_IN.viewDir = vec3( gl_TexCoord[2]);
    xlt_IN._LightCoord = vec3( gl_TexCoord[3]);
    xl_retval = frag_surf( xlt_IN);
    gl_FragData[0] = vec4( xl_retval);
}
/* NOTE: GLSL optimization failed
0:86(13): error: type mismatch
*/

#endif
"
}

SubProgram "opengl " {
Keywords { "DIRECTIONAL_COOKIE" }
Bind "vertex" Vertex
Bind "tangent" ATTR14
Bind "normal" Normal
Bind "texcoord" TexCoord0
Vector 17 [unity_Scale]
Vector 18 [_WorldSpaceCameraPos]
Vector 19 [_WorldSpaceLightPos0]
Matrix 5 [_Object2World]
Matrix 9 [_World2Object]
Matrix 13 [_LightMatrix0]
Vector 20 [_Diffuse_ST]
Vector 21 [_NormalMap_ST]
"3.0-!!ARBvp1.0
# 32 ALU
PARAM c[22] = { { 1 },
		state.matrix.mvp,
		program.local[5..21] };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
MOV R1.xyz, c[18];
MOV R1.w, c[0].x;
MOV R0.xyz, vertex.attrib[14];
DP4 R2.z, R1, c[11];
DP4 R2.y, R1, c[10];
DP4 R2.x, R1, c[9];
MAD R2.xyz, R2, c[17].w, -vertex.position;
MUL R1.xyz, vertex.normal.zxyw, R0.yzxw;
MAD R1.xyz, vertex.normal.yzxw, R0.zxyw, -R1;
MOV R0, c[19];
MUL R1.xyz, R1, vertex.attrib[14].w;
DP4 R3.z, R0, c[11];
DP4 R3.y, R0, c[10];
DP4 R3.x, R0, c[9];
DP4 R0.w, vertex.position, c[8];
DP4 R0.z, vertex.position, c[7];
DP4 R0.x, vertex.position, c[5];
DP4 R0.y, vertex.position, c[6];
DP3 result.texcoord[1].y, R3, R1;
DP3 result.texcoord[2].y, R1, R2;
DP3 result.texcoord[1].z, vertex.normal, R3;
DP3 result.texcoord[1].x, R3, vertex.attrib[14];
DP3 result.texcoord[2].z, vertex.normal, R2;
DP3 result.texcoord[2].x, vertex.attrib[14], R2;
DP4 result.texcoord[3].y, R0, c[14];
DP4 result.texcoord[3].x, R0, c[13];
MAD result.texcoord[0].zw, vertex.texcoord[0].xyxy, c[21].xyxy, c[21];
MAD result.texcoord[0].xy, vertex.texcoord[0], c[20], c[20].zwzw;
DP4 result.position.w, vertex.position, c[4];
DP4 result.position.z, vertex.position, c[3];
DP4 result.position.y, vertex.position, c[2];
DP4 result.position.x, vertex.position, c[1];
END
# 32 instructions, 4 R-regs
"
}

SubProgram "d3d9 " {
Keywords { "DIRECTIONAL_COOKIE" }
Bind "vertex" Vertex
Bind "tangent" TexCoord2
Bind "normal" Normal
Bind "texcoord" TexCoord0
Matrix 0 [glstate_matrix_mvp]
Vector 16 [unity_Scale]
Vector 17 [_WorldSpaceCameraPos]
Vector 18 [_WorldSpaceLightPos0]
Matrix 4 [_Object2World]
Matrix 8 [_World2Object]
Matrix 12 [_LightMatrix0]
Vector 19 [_Diffuse_ST]
Vector 20 [_NormalMap_ST]
"vs_3_0
; 35 ALU
dcl_position o0
dcl_texcoord0 o1
dcl_texcoord1 o2
dcl_texcoord2 o3
dcl_texcoord3 o4
def c21, 1.00000000, 0, 0, 0
dcl_position0 v0
dcl_tangent0 v1
dcl_normal0 v2
dcl_texcoord0 v3
mov r0.w, c21.x
mov r0.xyz, c17
dp4 r1.z, r0, c10
dp4 r1.y, r0, c9
dp4 r1.x, r0, c8
mad r3.xyz, r1, c16.w, -v0
mov r0.xyz, v1
mul r1.xyz, v2.zxyw, r0.yzxw
mov r0.xyz, v1
mad r1.xyz, v2.yzxw, r0.zxyw, -r1
mul r2.xyz, r1, v1.w
mov r0, c10
dp4 r4.z, c18, r0
mov r0, c9
dp4 r4.y, c18, r0
mov r1, c8
dp4 r4.x, c18, r1
dp4 r0.w, v0, c7
dp4 r0.z, v0, c6
dp4 r0.x, v0, c4
dp4 r0.y, v0, c5
dp3 o2.y, r4, r2
dp3 o3.y, r2, r3
dp3 o2.z, v2, r4
dp3 o2.x, r4, v1
dp3 o3.z, v2, r3
dp3 o3.x, v1, r3
dp4 o4.y, r0, c13
dp4 o4.x, r0, c12
mad o1.zw, v3.xyxy, c20.xyxy, c20
mad o1.xy, v3, c19, c19.zwzw
dp4 o0.w, v0, c3
dp4 o0.z, v0, c2
dp4 o0.y, v0, c1
dp4 o0.x, v0, c0
"
}

SubProgram "gles " {
Keywords { "DIRECTIONAL_COOKIE" }
"!!GLES

#ifdef VERTEX
attribute vec4 TANGENT;
uniform vec4 unity_Scale;
uniform vec4 _WorldSpaceLightPos0;
uniform vec3 _WorldSpaceCameraPos;
uniform mat4 _World2Object;
uniform mat4 _Object2World;
uniform vec4 _NormalMap_ST;
uniform mat4 _LightMatrix0;
uniform vec4 _Diffuse_ST;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1 = gl_Vertex.xyzw;
  vec4 tmpvar_3;
  tmpvar_3 = TANGENT.xyzw;
  vec3 tmpvar_5;
  tmpvar_5 = gl_Normal.xyz;
  vec4 tmpvar_7;
  tmpvar_7 = gl_MultiTexCoord0.xyzw;
  vec4 tmpvar_98;
  tmpvar_98.xy = ((tmpvar_7.xy * _Diffuse_ST.xy) + _Diffuse_ST.zw);
  tmpvar_98.zw = ((tmpvar_7.xy * _NormalMap_ST.xy) + _NormalMap_ST.zw).xy;
  vec3 tmpvar_43;
  tmpvar_43 = (cross (tmpvar_5, tmpvar_3.xyz) * tmpvar_3.w);
  mat3 tmpvar_44;
  tmpvar_44[0].x = tmpvar_3.x;
  tmpvar_44[0].y = tmpvar_43.x;
  tmpvar_44[0].z = tmpvar_5.x;
  tmpvar_44[1].x = tmpvar_3.y;
  tmpvar_44[1].y = tmpvar_43.y;
  tmpvar_44[1].z = tmpvar_5.y;
  tmpvar_44[2].x = tmpvar_3.z;
  tmpvar_44[2].y = tmpvar_43.z;
  tmpvar_44[2].z = tmpvar_5.z;
  vec4 tmpvar_61;
  tmpvar_61.xyz = _WorldSpaceCameraPos.xyz;
  tmpvar_61.w = 1.0;
  gl_Position = (gl_ModelViewProjectionMatrix * tmpvar_1).xyzw;
  gl_TexCoord[0] = tmpvar_98.xyzw;
  vec4 tmpvar_19;
  tmpvar_19.xyz = (tmpvar_44 * (_World2Object * _WorldSpaceLightPos0).xyz).xyz;
  tmpvar_19.w = 0.0;
  gl_TexCoord[1] = tmpvar_19;
  vec4 tmpvar_21;
  tmpvar_21.xyz = (tmpvar_44 * (((_World2Object * tmpvar_61).xyz * unity_Scale.w) - tmpvar_1.xyz)).xyz;
  tmpvar_21.w = 0.0;
  gl_TexCoord[2] = tmpvar_21;
  vec4 tmpvar_23;
  tmpvar_23.xy = (_LightMatrix0 * (_Object2World * tmpvar_1)).xy;
  tmpvar_23.z = 0.0;
  tmpvar_23.w = 0.0;
  gl_TexCoord[3] = tmpvar_23;
}


#endif
#ifdef FRAGMENT
struct v2f_vertex_lit {
    vec2 uv;
    vec4 diff;
    vec4 spec;
};
struct v2f_img {
    vec4 pos;
    vec2 uv;
};
struct appdata_img {
    vec4 vertex;
    vec2 texcoord;
};
struct SurfaceOutput {
    vec3 Albedo;
    vec3 Normal;
    vec3 Emission;
    float Specular;
    float Gloss;
    float Alpha;
};
struct EditorSurfaceOutput {
    vec3 Albedo;
    vec3 Normal;
    vec3 Emission;
    vec3 Gloss;
    float Specular;
    float Alpha;
};
struct appdata_full {
    vec4 vertex;
    vec4 tangent;
    vec3 normal;
    vec4 texcoord;
    vec4 texcoord1;
    vec4 color;
};
struct Input {
    vec2 uv_Diffuse;
    vec2 uv_NormalMap;
};
struct v2f_surf {
    vec4 pos;
    vec4 hip_pack0;
    vec3 lightDir;
    vec3 viewDir;
    vec2 _LightCoord;
};
uniform sampler2D _Diffuse;
uniform vec4 _LightColor0;
uniform sampler2D _LightTexture0;
uniform sampler2D _NormalMap;
vec4 UnpackNormal( in vec4 packednormal );
void surf( in Input IN, inout EditorSurfaceOutput o );
float Luminance( in vec3 c );
vec4 LightingBlinnPhongEditor_PrePass( in EditorSurfaceOutput s, in vec4 light );
vec4 LightingBlinnPhongEditor( in EditorSurfaceOutput s, in vec3 lightDir, in vec3 viewDir, in float atten );
vec4 frag_surf( in v2f_surf IN );
vec4 UnpackNormal( in vec4 packednormal ) {
    vec4 normal;
    normal.xy  = ((packednormal.wy  * 2.00000) - 1.00000);
    normal.z  = sqrt( ((1.00000 - (normal.x  * normal.x )) - (normal.y  * normal.y )) );
    return normal;
}
void surf( in Input IN, inout EditorSurfaceOutput o ) {
    vec4 Tex2D1;
    vec4 Tex2D0;
    vec4 UnpackNormal0;
    vec4 Master0_2_NoInput = vec4( 0.000000, 0.000000, 0.000000, 0.000000);
    vec4 Master0_3_NoInput = vec4( 0.000000, 0.000000, 0.000000, 0.000000);
    vec4 Master0_4_NoInput = vec4( 0.000000, 0.000000, 0.000000, 0.000000);
    vec4 Master0_6_NoInput = vec4( 1.00000, 1.00000, 1.00000, 1.00000);
    vec4 _Alpha;
    o.Albedo = vec3( 0.000000);
    o.Normal = vec3( 0.000000, 0.000000, 1.00000);
    o.Emission = vec3( 0.000000);
    o.Gloss = vec3( 0.000000);
    o.Specular = 0.000000;
    o.Alpha = 1.00000;
    Tex2D1 = texture2D( _Diffuse, IN.uv_Diffuse.xyxy .xy );
    Tex2D0 = texture2D( _NormalMap, IN.uv_NormalMap.xyxy .xy );
    UnpackNormal0 = UnpackNormal( Tex2D0);
    o.Albedo = vec3( Tex2D1);
    o.Normal = vec3( UnpackNormal0);
    o.Alpha = vec4( _Alpha);
}
float Luminance( in vec3 c ) {
    return dot( c, vec3( 0.220000, 0.707000, 0.0710000));
}
vec4 LightingBlinnPhongEditor_PrePass( in EditorSurfaceOutput s, in vec4 light ) {
    vec3 spec;
    vec4 c;
    spec = (light.w  * s.Gloss);
    c.xyz  = ((s.Albedo * light.xyz ) + (light.xyz  * spec));
    c.w  = (s.Alpha + Luminance( spec));
    return c;
}
vec4 LightingBlinnPhongEditor( in EditorSurfaceOutput s, in vec3 lightDir, in vec3 viewDir, in float atten ) {
    vec3 h;
    float diff;
    float nh;
    vec3 spec;
    vec4 res;
    viewDir = normalize( viewDir );
    h = normalize( (lightDir + viewDir) );
    diff = max( 0.000000, dot( s.Normal, lightDir));
    nh = max( 0.000000, dot( s.Normal, h));
    spec = (pow( nh, (s.Specular * 128.000)) * s.Gloss);
    res.xyz  = (_LightColor0.xyz  * ((diff * atten) * 2.00000));
    res.w  = float( (spec * Luminance( _LightColor0.xyz )));
    return LightingBlinnPhongEditor_PrePass( s, res);
}
vec4 frag_surf( in v2f_surf IN ) {
    Input surfIN;
    EditorSurfaceOutput o;
    vec3 lightDir;
    vec4 c;
    surfIN.uv_Diffuse = IN.hip_pack0.xy ;
    surfIN.uv_NormalMap = IN.hip_pack0.zw ;
    o.Albedo = vec3( 0.000000);
    o.Emission = vec3( 0.000000);
    o.Specular = 0.000000;
    o.Alpha = 0.000000;
    surf( surfIN, o);
    lightDir = IN.lightDir;
    c = LightingBlinnPhongEditor( o, lightDir, normalize( vec3( IN.viewDir) ), (texture2D( _LightTexture0, IN._LightCoord).w  * 1.00000));
    c.w  = o.Alpha;
    return c;
}
void main() {
    vec4 xl_retval;
    v2f_surf xlt_IN;
    xlt_IN.pos = vec4(0.0);
    xlt_IN.hip_pack0 = vec4( gl_TexCoord[0]);
    xlt_IN.lightDir = vec3( gl_TexCoord[1]);
    xlt_IN.viewDir = vec3( gl_TexCoord[2]);
    xlt_IN._LightCoord = vec2( gl_TexCoord[3]);
    xl_retval = frag_surf( xlt_IN);
    gl_FragData[0] = vec4( xl_retval);
}
/* NOTE: GLSL optimization failed
0:85(13): error: type mismatch
*/

#endif
"
}

}
Program "fp" {
// Fragment combos: 5
//   opengl - ALU: 14 to 26, TEX: 2 to 4
//   d3d9 - ALU: 12 to 22, TEX: 2 to 4
SubProgram "opengl " {
Keywords { "POINT" }
Vector 0 [_LightColor0]
Float 1 [_Alpha]
SetTexture 0 [_Diffuse] 2D
SetTexture 1 [_NormalMap] 2D
SetTexture 2 [_LightTexture0] 2D
"3.0-!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 20 ALU, 3 TEX
PARAM c[3] = { program.local[0..1],
		{ 2, 1, 0 } };
TEMP R0;
TEMP R1;
TEX R0.yw, fragment.texcoord[0].zwzw, texture[1], 2D;
MAD R0.xy, R0.wyzw, c[2].x, -c[2].y;
MUL R0.z, R0.y, R0.y;
MAD R0.z, -R0.x, R0.x, -R0;
ADD R0.z, R0, c[2].y;
DP3 R0.w, fragment.texcoord[1], fragment.texcoord[1];
RSQ R0.w, R0.w;
MUL R1.xyz, R0.w, fragment.texcoord[1];
RSQ R0.z, R0.z;
RCP R0.z, R0.z;
DP3 R0.x, R0, R1;
DP3 R0.w, fragment.texcoord[3], fragment.texcoord[3];
TEX R0.w, R0.w, texture[2], 2D;
MAX R0.x, R0, c[2].z;
MUL R0.x, R0, R0.w;
MUL R0.x, R0, c[2];
MUL R1.xyz, R0.x, c[0];
TEX R0.xyz, fragment.texcoord[0], texture[0], 2D;
MUL result.color.xyz, R0, R1;
MOV result.color.w, c[1].x;
END
# 20 instructions, 2 R-regs
"
}

SubProgram "d3d9 " {
Keywords { "POINT" }
Vector 0 [_LightColor0]
Float 1 [_Alpha]
SetTexture 0 [_Diffuse] 2D
SetTexture 1 [_NormalMap] 2D
SetTexture 2 [_LightTexture0] 2D
"ps_3_0
; 17 ALU, 3 TEX
dcl_2d s0
dcl_2d s1
dcl_2d s2
def c2, 2.00000000, -1.00000000, 1.00000000, 0.00000000
dcl_texcoord0 v0
dcl_texcoord1 v1.xyz
dcl_texcoord3 v3.xyz
texld r0.yw, v0.zwzw, s1
mad_pp r1.xy, r0.wyzw, c2.x, c2.y
mul_pp r0.x, r1.y, r1.y
mad_pp r0.x, -r1, r1, -r0
add_pp r0.x, r0, c2.z
rsq_pp r0.x, r0.x
rcp_pp r1.z, r0.x
dp3_pp r0.y, v1, v1
rsq_pp r0.y, r0.y
mul_pp r0.yzw, r0.y, v1.xxyz
dp3_pp r0.y, r1, r0.yzww
dp3 r0.x, v3, v3
max_pp r0.y, r0, c2.w
texld r0.x, r0.x, s2
mul_pp r0.x, r0.y, r0
mul_pp r0.x, r0, c2
mul r1.xyz, r0.x, c0
texld r0.xyz, v0, s0
mul_pp oC0.xyz, r0, r1
mov_pp oC0.w, c1.x
"
}

SubProgram "gles " {
Keywords { "POINT" }
"!!GLES"
}

SubProgram "opengl " {
Keywords { "DIRECTIONAL" }
Vector 0 [_LightColor0]
Float 1 [_Alpha]
SetTexture 0 [_Diffuse] 2D
SetTexture 1 [_NormalMap] 2D
"3.0-!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 14 ALU, 2 TEX
PARAM c[3] = { program.local[0..1],
		{ 2, 1, 0 } };
TEMP R0;
TEMP R1;
TEX R0.yw, fragment.texcoord[0].zwzw, texture[1], 2D;
MAD R0.xy, R0.wyzw, c[2].x, -c[2].y;
MUL R0.z, R0.y, R0.y;
MAD R0.z, -R0.x, R0.x, -R0;
ADD R0.z, R0, c[2].y;
RSQ R0.z, R0.z;
RCP R0.z, R0.z;
DP3 R0.x, R0, fragment.texcoord[1];
MAX R0.x, R0, c[2].z;
MUL R0.x, R0, c[2];
MUL R1.xyz, R0.x, c[0];
TEX R0.xyz, fragment.texcoord[0], texture[0], 2D;
MUL result.color.xyz, R0, R1;
MOV result.color.w, c[1].x;
END
# 14 instructions, 2 R-regs
"
}

SubProgram "d3d9 " {
Keywords { "DIRECTIONAL" }
Vector 0 [_LightColor0]
Float 1 [_Alpha]
SetTexture 0 [_Diffuse] 2D
SetTexture 1 [_NormalMap] 2D
"ps_3_0
; 12 ALU, 2 TEX
dcl_2d s0
dcl_2d s1
def c2, 2.00000000, -1.00000000, 1.00000000, 0.00000000
dcl_texcoord0 v0
dcl_texcoord1 v1.xyz
texld r0.yw, v0.zwzw, s1
mad_pp r0.xy, r0.wyzw, c2.x, c2.y
mul_pp r0.z, r0.y, r0.y
mad_pp r0.z, -r0.x, r0.x, -r0
add_pp r0.z, r0, c2
rsq_pp r0.z, r0.z
rcp_pp r0.z, r0.z
dp3_pp r0.x, r0, v1
max_pp r0.x, r0, c2.w
mul_pp r0.x, r0, c2
mul r1.xyz, r0.x, c0
texld r0.xyz, v0, s0
mul_pp oC0.xyz, r0, r1
mov_pp oC0.w, c1.x
"
}

SubProgram "gles " {
Keywords { "DIRECTIONAL" }
"!!GLES"
}

SubProgram "opengl " {
Keywords { "SPOT" }
Vector 0 [_LightColor0]
Float 1 [_Alpha]
SetTexture 0 [_Diffuse] 2D
SetTexture 1 [_NormalMap] 2D
SetTexture 2 [_LightTexture0] 2D
SetTexture 3 [_LightTextureB0] 2D
"3.0-!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 26 ALU, 4 TEX
PARAM c[3] = { program.local[0..1],
		{ 2, 1, 0, 0.5 } };
TEMP R0;
TEMP R1;
TEX R0.yw, fragment.texcoord[0].zwzw, texture[1], 2D;
MAD R0.xy, R0.wyzw, c[2].x, -c[2].y;
MUL R0.z, R0.y, R0.y;
MAD R0.z, -R0.x, R0.x, -R0;
ADD R0.z, R0, c[2].y;
DP3 R0.w, fragment.texcoord[1], fragment.texcoord[1];
RSQ R0.w, R0.w;
MUL R1.xyz, R0.w, fragment.texcoord[1];
RSQ R0.z, R0.z;
RCP R0.z, R0.z;
DP3 R0.x, R0, R1;
DP3 R0.y, fragment.texcoord[3], fragment.texcoord[3];
TEX R1.w, R0.y, texture[3], 2D;
RCP R0.w, fragment.texcoord[3].w;
MAD R0.zw, fragment.texcoord[3].xyxy, R0.w, c[2].w;
TEX R0.w, R0.zwzw, texture[2], 2D;
SLT R0.y, c[2].z, fragment.texcoord[3].z;
MUL R0.y, R0, R0.w;
MUL R0.y, R0, R1.w;
MAX R0.x, R0, c[2].z;
MUL R0.x, R0, R0.y;
MUL R0.x, R0, c[2];
MUL R1.xyz, R0.x, c[0];
TEX R0.xyz, fragment.texcoord[0], texture[0], 2D;
MUL result.color.xyz, R0, R1;
MOV result.color.w, c[1].x;
END
# 26 instructions, 2 R-regs
"
}

SubProgram "d3d9 " {
Keywords { "SPOT" }
Vector 0 [_LightColor0]
Float 1 [_Alpha]
SetTexture 0 [_Diffuse] 2D
SetTexture 1 [_NormalMap] 2D
SetTexture 2 [_LightTexture0] 2D
SetTexture 3 [_LightTextureB0] 2D
"ps_3_0
; 22 ALU, 4 TEX
dcl_2d s0
dcl_2d s1
dcl_2d s2
dcl_2d s3
def c2, 2.00000000, -1.00000000, 1.00000000, 0.00000000
def c3, 0.50000000, 0, 0, 0
dcl_texcoord0 v0
dcl_texcoord1 v1.xyz
dcl_texcoord3 v3
texld r0.yw, v0.zwzw, s1
mad_pp r0.xy, r0.wyzw, c2.x, c2.y
mul_pp r0.z, r0.y, r0.y
mad_pp r0.z, -r0.x, r0.x, -r0
add_pp r0.z, r0, c2
dp3_pp r0.w, v1, v1
rsq_pp r0.w, r0.w
mul_pp r1.xyz, r0.w, v1
rsq_pp r0.z, r0.z
rcp_pp r0.z, r0.z
dp3_pp r0.y, r0, r1
rcp r0.w, v3.w
mad r1.xy, v3, r0.w, c3.x
dp3 r0.x, v3, v3
texld r0.w, r1, s2
cmp r0.z, -v3, c2.w, c2
texld r0.x, r0.x, s3
mul r0.z, r0, r0.w
mul r0.z, r0, r0.x
max_pp r0.x, r0.y, c2.w
mul_pp r0.x, r0, r0.z
mul_pp r0.x, r0, c2
mul r1.xyz, r0.x, c0
texld r0.xyz, v0, s0
mul_pp oC0.xyz, r0, r1
mov_pp oC0.w, c1.x
"
}

SubProgram "gles " {
Keywords { "SPOT" }
"!!GLES"
}

SubProgram "opengl " {
Keywords { "POINT_COOKIE" }
Vector 0 [_LightColor0]
Float 1 [_Alpha]
SetTexture 0 [_Diffuse] 2D
SetTexture 1 [_NormalMap] 2D
SetTexture 2 [_LightTextureB0] 2D
SetTexture 3 [_LightTexture0] CUBE
"3.0-!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 22 ALU, 4 TEX
PARAM c[3] = { program.local[0..1],
		{ 2, 1, 0 } };
TEMP R0;
TEMP R1;
TEX R0.yw, fragment.texcoord[0].zwzw, texture[1], 2D;
MAD R0.xy, R0.wyzw, c[2].x, -c[2].y;
MUL R0.z, R0.y, R0.y;
MAD R0.z, -R0.x, R0.x, -R0;
DP3 R0.w, fragment.texcoord[1], fragment.texcoord[1];
RSQ R0.w, R0.w;
MUL R1.xyz, R0.w, fragment.texcoord[1];
ADD R0.z, R0, c[2].y;
RSQ R0.z, R0.z;
RCP R0.z, R0.z;
DP3 R0.x, R0, R1;
DP3 R0.y, fragment.texcoord[3], fragment.texcoord[3];
TEX R0.w, fragment.texcoord[3], texture[3], CUBE;
TEX R1.w, R0.y, texture[2], 2D;
MUL R0.y, R1.w, R0.w;
MAX R0.x, R0, c[2].z;
MUL R0.x, R0, R0.y;
MUL R0.x, R0, c[2];
MUL R1.xyz, R0.x, c[0];
TEX R0.xyz, fragment.texcoord[0], texture[0], 2D;
MUL result.color.xyz, R0, R1;
MOV result.color.w, c[1].x;
END
# 22 instructions, 2 R-regs
"
}

SubProgram "d3d9 " {
Keywords { "POINT_COOKIE" }
Vector 0 [_LightColor0]
Float 1 [_Alpha]
SetTexture 0 [_Diffuse] 2D
SetTexture 1 [_NormalMap] 2D
SetTexture 2 [_LightTextureB0] 2D
SetTexture 3 [_LightTexture0] CUBE
"ps_3_0
; 18 ALU, 4 TEX
dcl_2d s0
dcl_2d s1
dcl_2d s2
dcl_cube s3
def c2, 2.00000000, -1.00000000, 1.00000000, 0.00000000
dcl_texcoord0 v0
dcl_texcoord1 v1.xyz
dcl_texcoord3 v3.xyz
texld r0.yw, v0.zwzw, s1
mad_pp r0.xy, r0.wyzw, c2.x, c2.y
mul_pp r0.z, r0.y, r0.y
mad_pp r0.z, -r0.x, r0.x, -r0
dp3_pp r0.w, v1, v1
rsq_pp r0.w, r0.w
mul_pp r1.xyz, r0.w, v1
add_pp r0.z, r0, c2
rsq_pp r0.z, r0.z
rcp_pp r0.z, r0.z
dp3_pp r0.y, r0, r1
dp3 r0.x, v3, v3
texld r0.x, r0.x, s2
texld r0.w, v3, s3
mul r0.z, r0.x, r0.w
max_pp r0.x, r0.y, c2.w
mul_pp r0.x, r0, r0.z
mul_pp r0.x, r0, c2
mul r1.xyz, r0.x, c0
texld r0.xyz, v0, s0
mul_pp oC0.xyz, r0, r1
mov_pp oC0.w, c1.x
"
}

SubProgram "gles " {
Keywords { "POINT_COOKIE" }
"!!GLES"
}

SubProgram "opengl " {
Keywords { "DIRECTIONAL_COOKIE" }
Vector 0 [_LightColor0]
Float 1 [_Alpha]
SetTexture 0 [_Diffuse] 2D
SetTexture 1 [_NormalMap] 2D
SetTexture 2 [_LightTexture0] 2D
"3.0-!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 16 ALU, 3 TEX
PARAM c[3] = { program.local[0..1],
		{ 2, 1, 0 } };
TEMP R0;
TEMP R1;
TEX R0.yw, fragment.texcoord[0].zwzw, texture[1], 2D;
MAD R0.xy, R0.wyzw, c[2].x, -c[2].y;
MUL R0.z, R0.y, R0.y;
MAD R0.z, -R0.x, R0.x, -R0;
ADD R0.z, R0, c[2].y;
RSQ R0.z, R0.z;
RCP R0.z, R0.z;
DP3 R0.x, R0, fragment.texcoord[1];
TEX R0.w, fragment.texcoord[3], texture[2], 2D;
MAX R0.x, R0, c[2].z;
MUL R0.x, R0, R0.w;
MUL R0.x, R0, c[2];
MUL R1.xyz, R0.x, c[0];
TEX R0.xyz, fragment.texcoord[0], texture[0], 2D;
MUL result.color.xyz, R0, R1;
MOV result.color.w, c[1].x;
END
# 16 instructions, 2 R-regs
"
}

SubProgram "d3d9 " {
Keywords { "DIRECTIONAL_COOKIE" }
Vector 0 [_LightColor0]
Float 1 [_Alpha]
SetTexture 0 [_Diffuse] 2D
SetTexture 1 [_NormalMap] 2D
SetTexture 2 [_LightTexture0] 2D
"ps_3_0
; 13 ALU, 3 TEX
dcl_2d s0
dcl_2d s1
dcl_2d s2
def c2, 2.00000000, -1.00000000, 1.00000000, 0.00000000
dcl_texcoord0 v0
dcl_texcoord1 v1.xyz
dcl_texcoord3 v3.xy
texld r0.yw, v0.zwzw, s1
mad_pp r0.xy, r0.wyzw, c2.x, c2.y
mul_pp r0.z, r0.y, r0.y
mad_pp r0.z, -r0.x, r0.x, -r0
add_pp r0.z, r0, c2
rsq_pp r0.z, r0.z
rcp_pp r0.z, r0.z
dp3_pp r0.x, r0, v1
texld r0.w, v3, s2
max_pp r0.x, r0, c2.w
mul_pp r0.x, r0, r0.w
mul_pp r0.x, r0, c2
mul r1.xyz, r0.x, c0
texld r0.xyz, v0, s0
mul_pp oC0.xyz, r0, r1
mov_pp oC0.w, c1.x
"
}

SubProgram "gles " {
Keywords { "DIRECTIONAL_COOKIE" }
"!!GLES"
}

}
	}

#LINE 111

	}
	Fallback "Diffuse"
}