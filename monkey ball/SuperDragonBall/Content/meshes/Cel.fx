//Cel.fx
//A simple, fast cel shader with outline by Mike Cronin
//cronholio@gmail.com
//Copious comments for people new to shaders


shared float4x4 world; //World Matrix
shared float4x4 view; //View Matrix
shared float4x4 projection; //Projection Matrix
float3 EyePosition; //Camera Position
shared float4 lightColor; //Diffuse light color, float4(0.6f, 0.6f, 0.6f, 1.0) is a good starting point
float3 LightPosition; //Obvious
shared float3 LightDirection;
//the shader has 3 layers, but it works in an aood way that mimics
//the way 2D animators paint Cels.
//The base color is constant, and it serves as the shadowed area of the surface.
//Layer one is what would normally be the diffuse are of the surface.
//Layer One is caluculated with a specular function, so the specular hit has to be really wide
float LayerOneSharp = 0.6; // This sharpens the edges of the layer, valid values are between 0 and 1. 0.6 is a good start
float LayerOneRough = 0.05; // This sets the bleed of the layer. youd want this pretty wide, so 0.05 is a good start
float LayerOneContrib = 0.05; // This is a multiplier for the final layer contribution, 0-1 is valid closer to 1 blows out the layer so, 0.05 is good
//Layer two also uses a specular function, it can provide a specular highlight or an additional paint layer
float LayerTwoSharp = 0.85; //This should be very sharp 0.85 is a good default
float LayerTwoRough = 5.0f; //Highlight shoul1d be small 5.0f-10.0f is good start.
float LayerTwoContrib = 0.3f; //0.3 f wil get you pretty close to white without blowing the highlight out. Set it to 0 if you don't want a spec highlight

//Sampler for the color texture applid to the model
sampler TextureSampler = sampler_state
{
    AddressU  = WRAP;
    AddressV  = WRAP;
    MIPFILTER = LINEAR;
    MINFILTER = LINEAR;
    MAGFILTER = LINEAR;
};

//Input for the vertex Shader
//There are two vertex shaders both take the same input
struct VS_INPUT
{
	float4 Position		: POSITION0;
	float3 Normal		: NORMAL0;
	float2 Texcoord		: TEXCOORD0;

};

//Output for the Cel vertex shader
struct VS_OUTPUT1
{
	float4 Position			: POSITION0;
	float3 Normal			: TEXCOORD1;
	float3 ViewDirection	: TEXCOORD2;
	float3 LightDirection	: TEXCOORD3;
	float2 Texcoord			: TEXCOORD0;
};

//Output for the outline vertex shader
struct VS_OUTPUT2
{
	float4 Position			: POSITION0;
	float4 Normal			: TEXCOORD1;
};

//The Cel vertex shader
//It just transforms the model and
//finds the light and view directions
VS_OUTPUT1 Transform(VS_INPUT Input)
{
	float4x4 WorldViewProjection = mul(mul(world, view), projection);
	float3 ObjectPosition = mul(Input.Position, world);
	
	VS_OUTPUT1 Output;
	Output.Normal			= mul(Input.Normal, world);
	Output.Position			= mul(Input.Position, WorldViewProjection);
	
	Output.ViewDirection	= EyePosition - ObjectPosition;
	//Output.LightDirection	= LightPosition - ObjectPosition;
	Output.LightDirection	= LightDirection;
	Output.Texcoord			= Input.Texcoord;
	
	return Output;
}

//The outline vertex shader
//This tranforms the model and
//"peaks" the surface (scales it out on it's normal) 
VS_OUTPUT2 Outline(VS_INPUT Input)
{
	//Here's the important value. It determins the thickness of the outline.
	//The value is completely dependent on the size of the model.
	//My model is vaery tiny so my outine is very tiny.
	//You may need to increase this or better yet, caluclat it based on the distance
	//between your camera and your model.
	//float offset = 0.003;
	float offset = 0.05;
	
	float4x4 WorldViewProjection = mul(mul(world, view), projection);
	
	VS_OUTPUT2 Output;
	Output.Normal			= mul(Input.Normal, world);
	Output.Position			= mul(Input.Position, WorldViewProjection)+(mul(offset, mul(Input.Normal, WorldViewProjection)));
	
	return Output;
}

//Input for the Cel Pixel shader
struct PS_INPUT
{
	float3 Normal			: TEXCOORD1;
	float3 ViewDirection	: TEXCOORD2;
	float3 LightDirection	: TEXCOORD3;
	float2 Texcoord			: TEXCOORD0;
};

//The Cel Pixel shader
//This calculates the surface color
float4 Cel(PS_INPUT Input) : COLOR0
{
	float3 Normal			= normalize(Input.Normal);
	float3 ViewDirection	= normalize(Input.ViewDirection);
	float3 NLightDirection	= normalize(Input.LightDirection);
	
	float oneW = 0.18f * ( 1.0f - LayerOneSharp );
	float twoW = 0.18f * ( 1.0f - LayerTwoSharp );
	
	float NDotL				= dot(Normal, NLightDirection);
	float3 Reflection		= normalize(2.0f * NDotL * Normal - NLightDirection);
	
	float4 layerOneColor	= smoothstep(0.72f-oneW, 0.72f+oneW, pow(saturate(dot(Reflection, ViewDirection)), LayerOneRough));
	float4 layerTwoColor	= smoothstep(0.72f-twoW, 0.72f+twoW, pow(saturate(dot(Reflection, ViewDirection)), LayerTwoRough));
	
	float4 baseColor		= tex2D(TextureSampler, Input.Texcoord) * (lightColor*0.6f);
	//float4 baseColor		= lightColor*0.6f;
	
	float4 color			= (baseColor + LayerOneContrib*layerOneColor) + LayerTwoContrib*layerTwoColor;
	color.a					= 1.0;
	
	return color;
}

//This is the ouline pixel shader. It just outputs unlit black.
float4 Black() : COLOR
{
   //return float4(0.0f, 0.0f, 0.0f, 1.0f);
   return float4(0.0f, 0.0f, 0.0f, 1.0f);
}

technique Toon
{
	//Render the Cel shader surface color
	pass P0
	{
		VertexShader = compile vs_3_0 Transform();
		PixelShader  = compile ps_3_0 Cel();
		CullMode = CCW;
	}
	
	//Render the outline surface inverted to create fake edge detection
	pass P1
	{
		VertexShader = compile vs_3_0 Outline();
		PixelShader  = compile ps_3_0 Black();
		CullMode = CW;
	}
	
}
