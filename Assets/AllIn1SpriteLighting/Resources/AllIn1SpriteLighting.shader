Shader "AllIn1SpriteLighting/AllIn1SpriteLighting"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {} //0
		_Color ("Color", Color) = (1,1,1,1) //1
		_LightBoost("Light Boost", Range(0, 10)) = 1 //2

		_SpecBoost("Specular Boost", Range(0, 10)) = 1 //3
		_SpecularTex ("Specular Map", 2D) = "white" {} //4

		_NormalsTex ("Normals Map", 2D) = "bump" {} //5
		_NormalStrenght("Strength Multiplier", Range(0.1, 10)) = 1 //6
		[Toggle()] _NormalFlipX("Invert X", float) = 0 //7
		[Toggle()] _NormalFlipY("Invert Y", float) = 0 //8

		_ToonShadowThresh("Shadow Threshold", Range(0, 1)) = 0.1 //9
		_ToonShadowSmooth("Shadow Smoothing", Range(0, 1)) = 0.1 //10
		[Header(Only used when Specular is active)]
		_ToonSpecThresh("Specular Threshold", Range(0, 1)) = 0.1 //11

		_GlowColor("Glow Color", Color) = (1,1,1,1) //12
		_GlowAmount("Glow Amount", Range(0,100)) = 10 //13
		_GlowTex("Glow Texture", 2D) = "white" {} //14
		_GlowLit("Glow Illumination", Range(0,1)) = 1 //15

		_OutlineColor("Outline Color", Color) = (1,1,1,1) //16
		_OutlineWidth("Outline Width", Range(0,0.2)) = 0.004 //17
		_OutlineAlpha("Outline Alpha",  Range(0,1)) = 1 //18
		_OutlineGlow("Outline Glow", Range(1,100)) = 1.5 //19
		_OutlineLit("Outline Illumination", Range(0,1)) = 0 //20

		_FadeTex("Fade Texture", 2D) = "white" {} //21
		_FadeAmount("Fade Amount",  Range(-0.1,1)) = -0.1
		_FadeBurnWidth("Fade Burn Width",  Range(0,1)) = 0.025
		_FadeBurnTransition("Fade Burn Smooth Transition",  Range(0.01,0.5)) = 0.075
		_FadeBurnColor("Fade Burn Color", Color) = (1,1,0,1)
		_FadeBurnTex("Fade Burn Texture", 2D) = "white" {}
		_FadeBurnGlow("Fade Burn Glow",  Range(1,50)) = 2//27

		_HsvShift("Hue Shift", Range(0, 360)) = 180 //28
		_HsvSaturation("Hue Shift Saturation", Range(0, 2)) = 1
		_HsvBright("Hue Shift Bright", Range(0, 2)) = 1 //30

		_HitEffectColor("Hit Effect Color", Color) = (1,1,1,1) //31
		_HitEffectGlow("Hit Effect Glow Intensity", Range(1,100)) = 5
		[Space]
		[Header(_Tip_ Animate the following property)]
		_HitEffectBlend("Hit Effect Blend", Range(0,1)) = 1 //33
	}

	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="False" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		Cull Off
		Lighting On
		ZWrite Off
		Fog { Mode Off }
		Blend SrcAlpha OneMinusSrcAlpha

		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Custom alpha vertex:vert

		#pragma shader_feature TOON_ON
		#pragma shader_feature NORMALMAP_ON
		#pragma shader_feature NORMALMAPFLIPX_ON
		#pragma shader_feature NORMALMAPFLIPY_ON
		#pragma shader_feature SPECULAR_ON
		#pragma shader_feature GLOW_ON
		#pragma shader_feature OUTLINE_ON
		#pragma shader_feature FADE_ON
		#pragma shader_feature HSV_ON
		#pragma shader_feature HITEFFECT_ON

		sampler2D _MainTex;
		fixed4 _Color;
		fixed _LightBoost;

		#if TOON_ON
		fixed _ToonShadowThresh, _ToonSpecThresh, _ToonShadowSmooth, _ToonSpecSmooth;
		#endif

		#if NORMALMAP_ON
		sampler2D _NormalsTex;
		fixed _NormalStrenght;
		#endif

		#if SPECULAR_ON
		sampler2D _SpecularTex;
		fixed _SpecBoost;
		#endif

		#if GLOW_ON
		sampler2D _GlowTex;
		fixed4 _GlowColor;
		fixed _GlowAmount, _GlowLit;
		#endif

		#if OUTLINE_ON
		fixed4 _OutlineColor;
		fixed _OutlineWidth, _OutlineAlpha, _OutlineGlow, _OutlineLit;
		#endif

		#if FADE_ON
		sampler2D _FadeTex, _FadeBurnTex;
		fixed4 _FadeBurnColor;
		fixed _FadeAmount, _FadeBurnWidth, _FadeBurnTransition, _FadeBurnGlow;
		#endif

		#if COLORSWAP_ON
		sampler2D _ColorSwapTex;
		fixed4 _ColorSwapRed, _ColorSwapGreen, _ColorSwapBlue;
		fixed _ColorSwapRedLuminosity, _ColorSwapGreenLuminosity, _ColorSwapBlueLuminosity;
		#endif

		#if HSV_ON
		fixed _HsvShift, _HsvSaturation, _HsvBright;
		#endif

		#if HITEFFECT_ON
		fixed4 _HitEffectColor;
		fixed _HitEffectGlow, _HitEffectBlend;
		#endif

		struct Input
		{
			fixed2 uv_MainTex;
			fixed4 color;
			fixed3 normal;
		};

		struct SurfaceOutputCustom
		{
			fixed3 Albedo;
			fixed3 Normal;
			fixed3 Emission;
			fixed Alpha;
			fixed2 uv;
			fixed IsLit;
		};
		
		void vert(inout appdata_full v, out Input o)
		{
			v.normal = fixed3(0, 0, -1);
			v.tangent = fixed4(-1, 0, 0, 1);

			UNITY_INITIALIZE_OUTPUT(Input, o);
			o.color = _Color * v.color;
		}

		fixed4 LightingCustom(SurfaceOutputCustom s, fixed3 lightDir, fixed3 viewDir, fixed atten) {
			fixed NdotL = dot(s.Normal, lightDir) * s.IsLit;

			#if SPECULAR_ON
			half3 h = normalize(lightDir + viewDir);
			float nh = saturate(dot(s.Normal, h));
			#if TOON_ON
			nh = step(1 - ((1 - _ToonSpecThresh) / 10.0), nh);
			#endif
			float spec = pow(nh, 48.0);
			#endif

			#if TOON_ON
			NdotL = smoothstep(saturate(_ToonShadowThresh - _ToonShadowSmooth), _ToonShadowThresh, NdotL);
			#endif

			fixed4 c;
			#if SPECULAR_ON
			c.rgb = ((s.Albedo * _LightColor0.rgb * NdotL) + (_LightColor0.rgb * (spec * _SpecBoost * tex2D(_SpecularTex, s.uv).r))) * (atten * _LightBoost);
			#else
			c.rgb = s.Albedo * _LightColor0.rgb * NdotL * atten * _LightBoost;
			#endif
			c.a = s.Alpha;
			return c;
		}

		void surf (Input IN, inout SurfaceOutputCustom o)
		{
			o.IsLit = 1;
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;

			#if HSV_ON
			fixed3 resultHsv = fixed3(o.Albedo);
			fixed cosHsv = _HsvBright * _HsvSaturation * cos(_HsvShift * 3.14159265 / 180);
			fixed sinHsv = _HsvBright * _HsvSaturation * sin(_HsvShift * 3.14159265 / 180);
			resultHsv.x = (.299 * _HsvBright + .701 * cosHsv + .168 * sinHsv) * c.x
				+ (.587 * _HsvBright - .587 * cosHsv + .330 * sinHsv) * c.y
				+ (.114 * _HsvBright - .114 * cosHsv - .497 * sinHsv) * c.z;
			resultHsv.y = (.299 * _HsvBright - .299 * cosHsv - .328 * sinHsv) *c.x
				+ (.587 * _HsvBright + .413 * cosHsv + .035 * sinHsv) * c.y
				+ (.114 * _HsvBright - .114 * cosHsv + .292 * sinHsv) * c.z;
			resultHsv.z = (.299 * _HsvBright - .3 * cosHsv + 1.25 * sinHsv) * c.x
				+ (.587 * _HsvBright - .588 * cosHsv - 1.05 * sinHsv) * c.y
				+ (.114 * _HsvBright + .886 * cosHsv - .203 * sinHsv) * c.z;
			o.Albedo = resultHsv;
			#endif

			#if HITEFFECT_ON
			o.Albedo = lerp(o.Albedo, _HitEffectColor.rgb * _HitEffectGlow, _HitEffectBlend);
			o.IsLit = 1 - _HitEffectBlend;
			#endif

			fixed outlineAlpha = 0;
			#if OUTLINE_ON
			fixed2 destUv = fixed2(_OutlineWidth, _OutlineWidth);
			fixed spriteLeft = tex2D(_MainTex, IN.uv_MainTex + fixed2(destUv.x, 0)).a;
			fixed spriteRight = tex2D(_MainTex, IN.uv_MainTex - fixed2(destUv.x, 0)).a;
			fixed spriteBottom = tex2D(_MainTex, IN.uv_MainTex + fixed2(0, destUv.y)).a;
			fixed spriteTop = tex2D(_MainTex, IN.uv_MainTex - fixed2(0, destUv.y)).a;
			outlineAlpha = spriteLeft + spriteRight + spriteBottom + spriteTop;
			fixed spriteTopLeft = tex2D(_MainTex, IN.uv_MainTex + fixed2(destUv.x, destUv.y)).a;
			fixed spriteTopRight = tex2D(_MainTex, IN.uv_MainTex + fixed2(-destUv.x, destUv.y)).a;
			fixed spriteBotLeft = tex2D(_MainTex, IN.uv_MainTex + fixed2(destUv.x, -destUv.y)).a;
			fixed spriteBotRight = tex2D(_MainTex, IN.uv_MainTex + fixed2(-destUv.x, -destUv.y)).a;
			outlineAlpha = outlineAlpha + spriteTopLeft + spriteTopRight + spriteBotLeft + spriteBotRight;
			outlineAlpha = step(0.05, saturate(outlineAlpha));
			outlineAlpha *= (1 - c.a) *_OutlineAlpha;
			o.Albedo = lerp(o.Albedo, _OutlineColor.rgb * _OutlineGlow, outlineAlpha > 0.1);
			o.Alpha = saturate(c.a + outlineAlpha);
			o.IsLit = lerp(o.IsLit, _OutlineLit, outlineAlpha > 0.1);
			#endif

			#if FADE_ON
			fixed originalAlpha = c.a;
			fixed2 tiledUvFade1 = IN.uv_MainTex;
			fixed2 tiledUvFade2 = IN.uv_MainTex;
			fixed fadeTemp = tex2D(_FadeTex, tiledUvFade1).r;
			fixed fade = smoothstep(_FadeAmount + 0.01, _FadeAmount + _FadeBurnTransition, fadeTemp);
			fixed fadeBurn = smoothstep(_FadeAmount - _FadeBurnWidth, _FadeAmount - _FadeBurnWidth + 0.1, fadeTemp);
			o.Alpha *= fade;
			_FadeBurnColor.rgb *= _FadeBurnGlow;
			o.Albedo += fadeBurn * tex2D(_FadeBurnTex, tiledUvFade2) * _FadeBurnColor * originalAlpha * (1 - o.Alpha);
			#endif

			#if GLOW_ON
			fixed4 emission;
			emission = tex2D(_GlowTex, IN.uv_MainTex);
			emission.rgb *= emission.a * _GlowAmount * _GlowColor * o.Albedo.rgb;
			o.Albedo.rgb += emission.rgb;
			o.IsLit = lerp(o.IsLit, _GlowLit, emission.a);
			#endif

			#if !NORMALMAP_ON
			o.Normal = UnpackNormal(fixed4(0.5, 0.5, 1, 1));
			#else
			o.Normal = UnpackNormal(tex2D(_NormalsTex, IN.uv_MainTex));
			#if !NORMALMAPFLIPX_ON
			o.Normal.x *= -1;
			#endif
			#if NORMALMAPFLIPY_ON
			o.Normal.y *= -1;
			#endif
			o.Normal.xy *= _NormalStrenght;
			o.Normal = normalize(o.Normal);
			#if OUTLINE_ON
			o.Normal = lerp(o.Normal, UnpackNormal(fixed4(0.5, 0.5, 1, 1)), outlineAlpha > 0.1);
			#endif
			#endif

			o.Emission = fixed3(0,0,0);
			o.uv = IN.uv_MainTex;
			o.Albedo *= IN.color;
		}
		ENDCG
	}
	CustomEditor "SpriteLightingGUI"
	Fallback "Transparent/VertexLit"
}