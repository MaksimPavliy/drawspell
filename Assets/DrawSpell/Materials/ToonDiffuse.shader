// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "TOZ/Object/TriProj/Local/ToonDiffuse" {
	Properties {
		_Color("Main Color", Color) = (1, 1, 1, 1)
		_MainTex("Base (RGB)", 2D) = "white" {}
		_Blend("Blending", Range (0.01, 0.4)) = 0.2

	    _MinLightIntesity("Min Light Intesity", Range(0, 1)) = 0
		_MinLightIntesityValue("Min Light Intesity Value", Range(0, 1)) = 0
		_MinAttenuation("Min Attenuation", Range(0, 1)) = 0
		_MinAttenuationValue("Min Attenuation Value", Range(0, 1)) = 0
		_Glossiness("Glossiness", Range(0,1)) = 0.5
		_GlossinessIntensivity("GlossinessIntensivity", Range(0,3)) = 0.5
		_GlossinessEdge("GlossinessEdge", Range(0,1)) = 0.5
	}

	SubShader {
		Tags { "RenderType" = "Opaque" "Queue" = "Geometry" }
		LOD 200

		CGPROGRAM
		#pragma surface surf CelShaded vertex:vert
		float _MinLightIntesity;
		float _MinLightIntesityValue;
		float _MinAttenuation;
		float _MinAttenuationValue;
		half _Glossiness;
		half _GlossinessIntensivity;
		half _GlossinessEdge;

		fixed4 _Color;
		sampler2D _MainTex;
		float4 _MainTex_ST;
		fixed _Blend;

		struct Input {
			float3 weight : TEXCOORD0;
			float3 worldPos;
		};

		struct SurfaceOutputCelShaded
		{
			fixed3 Albedo;
			fixed3 Normal;
			float Smoothness;
			half3 Emission;
			fixed Alpha;
		};


		half4 LightingCelShaded(SurfaceOutputCelShaded s, half3 lightDir, half3 viewDir, half atten) {
			half NdotL = dot(s.Normal, lightDir);

			float m = 1 / _MinLightIntesity;
			float lightIntensity = NdotL > _MinLightIntesity ? 1 : lerp(_MinLightIntesityValue, 1, NdotL * m);
			float l = 1 / _MinAttenuation;
			float attenuation = atten > _MinAttenuation ? 1 : lerp(_MinAttenuationValue, 1, atten * l);

			float3 refl = reflect(normalize(lightDir), normalize(s.Normal));
			float vDotRefl = dot(viewDir, -refl);
			float kk = 1 / _Glossiness;
			float gloss = _Glossiness * 30;
			float3 specular = vDotRefl > _GlossinessEdge ? pow(vDotRefl * lightIntensity, gloss * gloss) : 0;

			//float clr = (_LightColor0.r + _LightColor0.g + _LightColor0.b) / 3;
			//half4 midColor = float4(clr, clr, clr, 1);

			half4 c;

			c.rgb = (s.Albedo + specular * _GlossinessIntensivity) * (lightIntensity * attenuation);
			c.a = s.Alpha;
			return c;
		}

		void vert(inout appdata_full v, out Input o) {
			UNITY_INITIALIZE_OUTPUT(Input, o);
			fixed3 n = max(abs(v.normal) - _Blend, 0);
			o.weight = n / (n.x + n.y + n.z).xxx;
		}

		void surf(Input IN, inout SurfaceOutputCelShaded o) {
			//Unity 5 texture interpolators already fill in limits, and no room for packing
			//So we do the uvs per pixel :(
			float3 oPos = mul(unity_WorldToObject, fixed4(IN.worldPos, 1.0)).xyz;
			fixed2 uvx = (oPos.yz - _MainTex_ST.zw) * _MainTex_ST.xy;
			fixed2 uvy = (oPos.xz - _MainTex_ST.zw) * _MainTex_ST.xy;
			fixed2 uvz = (oPos.xy - _MainTex_ST.zw) * _MainTex_ST.xy;
			fixed4 cz = tex2D(_MainTex, uvx) * IN.weight.xxxx;
			fixed4 cy = tex2D(_MainTex, uvy) * IN.weight.yyyy;
			fixed4 cx = tex2D(_MainTex, uvz) * IN.weight.zzzz;
			fixed4 col = (cz + cy + cx) * _Color;
			o.Albedo = col.rgb;
			o.Alpha = col.a;
		}
		ENDCG
	}

	FallBack "Legacy Shaders/Diffuse"
}