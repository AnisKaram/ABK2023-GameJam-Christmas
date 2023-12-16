float2 rayBoxDst(float3 boundsMin, float3 boundsMax, float3 rayOrigin, float3 invRaydir)
{
	float3 t0 = (boundsMin - rayOrigin) * invRaydir;
	float3 t1 = (boundsMax - rayOrigin) * invRaydir;
	float3 tmin = min(t0, t1);
	float3 tmax = max(t0, t1);

	float dstA = max(max(tmin.x, tmin.y), tmin.z);
	float dstB = min(tmax.x, min(tmax.y, tmax.z));

	float dstToBox = max(0, dstA);
	float dstInsideBox = max(0, dstB - dstToBox);
	return float2(dstToBox, dstInsideBox);
}

float Voronoi(float2 UV, float AngleOffset, float CellDensity)
{
	float2 g = floor(UV * CellDensity);
	float2 f = frac(UV * CellDensity);
	float3 res = float3(2.0, 0.0, 0.0);
	float Out;

	[unroll]
	for (int y = -1; y <= 1; y++)
	{
		[unroll]
		for (int x = -1; x <= 1; x++)
		{
			float2 lattice = float2(x, y);
			float2 m = float2(15.27, 47.63);
			float2 RND = frac(sin((lattice + g) * m) * 46839.32);
			float2 offset = float2(sin(RND.y * AngleOffset) * 0.5 + 0.5, cos(RND.x * AngleOffset) * 0.5 + 0.5);
			float d = distance(lattice + offset, f);
			if (d < res.x)
			{
				res = float3(d, offset.x, offset.y);
				Out = res.x;
			}
		}
	}

	return Out;
}

float3 GetRay_float(float2 screenPos)
{
	float3 viewVector = mul(unity_CameraInvProjection, float4(screenPos * 2 - 1, 0, -1));
	float3 viewDir = mul(unity_CameraToWorld, float4(viewVector, 0));
	float viewLength = length(viewDir);
	float3 ray = viewDir / viewLength;
	return ray;
}

float3 GetShadows(float3 rayPos)
{
	float ShadowAttenuation;
	float DistanceAttenuation;
#ifdef SHADERGRAPH_PREVIEW
	ShadowAttenuation = 0.4;
	DistanceAttenuation = 0.4;
#else

#if SHADOWS_SCREEN
	half4 clipPos = TransformWorldToHClip(rayPos);
	half4 shadowCoord = ComputeScreenPos(clipPos);
#else
	half4 shadowCoord = TransformWorldToShadowCoord(rayPos);
#endif
	Light mainLight = GetMainLight(shadowCoord);
	DistanceAttenuation = mainLight.distanceAttenuation;
	ShadowAttenuation = mainLight.shadowAttenuation;
#endif

	return ShadowAttenuation * DistanceAttenuation;
}

float3 GetLightDir_float()
{
	float3 Direction;
#ifdef SHADERGRAPH_PREVIEW
	Direction = float3(-0.5, 0.5, -0.5);
#else
	Light mainLight = GetMainLight(float4(0, 0, 0, 0));
	Direction = mainLight.direction;
#endif

	return Direction;
}

float SampleDensity_float(float density, float3 rayPos, SamplerState _sampler, texture2D noiseTexture, float tiling, float offset, float thicknessMin, float thicknessMax, float3 pos, float3 bounds)
{
	if (thicknessMin > 0)
	{
		offset /= 10;
		float2 noiseUV = float2(rayPos.x, rayPos.z);
		float noise = SAMPLE_TEXTURE2D_LOD(noiseTexture, _sampler, (noiseUV + (offset * 200)) / (tiling * 1000), 0).x;
		noise += min(Voronoi((noiseUV + offset) / (tiling * 7), offset + 10, 1), 1) / 10;
		noise /= 1.1;

		float distToEdge = abs(rayPos.y - pos.y) / bounds.y;
		float minEdge = thicknessMin * (1 - distToEdge);

		return clamp(density * smoothstep(noise, minEdge + thicknessMax, minEdge), 0, 1);
	}
	else
	{
		return density;
	}
}

float map01_float(float value)
{
	value += 1;
	value = 1 / value;
	value = 1 - value;
	value = pow(value, 5);
	return value;
}

float DensityLighting_float(float3 rayPos, float lightingSteps, bool directionalLighting, float density, SamplerState _sampler, texture2D noiseTexture, float tiling, float offset, float thicknessMin, float thicknessMax, float3 pos, float3 bounds, bool useSphereMask, float sphereMaskRadius, float3 sphereMaskPos, float farPlane)
{
	float3 posBL = pos - bounds / 2;
	float3 posTR = pos + bounds / 2;
	float lighting;
	float3 cumulativeRayPos = rayPos;
	float cumulativeDensity = 0;

	lighting = 1;
	
	float3 dirToLight = GetLightDir_float();
	float lightDist = rayBoxDst(posBL, posTR, rayPos, 1 / dirToLight).y;
	for (int i = 0; i < lightingSteps; i++)
	{
		float _stepSize = lightDist / lightingSteps;
		cumulativeRayPos += dirToLight * _stepSize;

		if ((useSphereMask && distance(cumulativeRayPos, sphereMaskPos) < sphereMaskRadius) || !useSphereMask)
		{
			float pointDensity = SampleDensity_float(density, cumulativeRayPos, _sampler, noiseTexture, tiling, offset, thicknessMin, thicknessMax, pos, bounds);

			cumulativeDensity += pointDensity * _stepSize;

			lighting *= 1 - exp(-cumulativeDensity);
		}
	}

	return lighting;
}

float SunHighlight(float3 rayDir)
{
	float fade = dot(GetLightDir_float(), float3(0, -1, 0));
	fade = max(fade, 0);
	fade = pow(1 - fade, 50);

	float phase = dot(GetLightDir_float(), rayDir);
	phase += 1;
	phase /= 2;

	phase = pow(phase, 25) * 25;

	return (phase * fade) + 1;
}

void RenderVolume_float(float3 pos, float3 bounds, float3 sphereMaskPos, float sphereMaskRadius, bool useSphereMask, float3 worldPos, float2 screenPos, float depth, float density, float densityStrength, float steps, float stepSize, float lightingSteps, bool directionalLighting, SamplerState _sampler, texture2D noiseTexture, float tiling, float offset, float thicknessMin, float thicknessMax, float farPlane, out float volumeDensity, out float3 lighting, out float distInBox, out float distToBox, out float distToLight, out float linearDepth, out float3 rayPos)
{
	float3 posBL = pos - bounds / 2;
	float3 posTR = pos + bounds / 2;

	float3 viewVector = mul(unity_CameraInvProjection, float4(screenPos * 2 - 1, 0, -1));
	float3 viewDir = mul(unity_CameraToWorld, float4(viewVector, 0));
	float viewLength = length(viewDir);

	float3 ray = GetRay_float(screenPos);

	float2 boxDist = rayBoxDst(posBL, posTR, _WorldSpaceCameraPos, 1 / ray);

	distToBox = boxDist.x;
	distInBox = boxDist.y;

	float3 entryPoint = _WorldSpaceCameraPos + ray * distToBox;

	rayPos = entryPoint + ray * viewLength;

	linearDepth = depth * length(viewDir);

	//Density
	float shadowMap = 0;
	float distTravelled = 0;
	float cumulativeDensity = 0;
	float3 cumulativeRayPos = rayPos;
	float transmittance = 1;
	for (int j = 0; j < steps && distTravelled < distInBox && distTravelled + distToBox < linearDepth; j++)
	{
		cumulativeRayPos += ray * stepSize;
		distTravelled += stepSize;
		if ((useSphereMask && distance(cumulativeRayPos, sphereMaskPos) < sphereMaskRadius) || !useSphereMask)
		{
			float pointDensity = SampleDensity_float(density, cumulativeRayPos, _sampler, noiseTexture, tiling, offset, thicknessMin, thicknessMax, pos, bounds);
			float cumDens = pointDensity * stepSize;
			cumulativeDensity += cumDens * densityStrength;
			
			if (lightingSteps > 0)
			{
				float lightingAtPoint = DensityLighting_float(cumulativeRayPos, lightingSteps, directionalLighting, density, _sampler, noiseTexture, tiling, offset, thicknessMin, thicknessMax, pos, bounds, useSphereMask, sphereMaskRadius, sphereMaskPos, farPlane);

#ifdef MAIN_LIGHT_CALCULATE_SHADOWS
				float shadows = (1 - GetShadows(cumulativeRayPos));
#else
				float shadows = 0;
#endif
				shadowMap += shadows;

				float ambient = exp(-cumDens);
				transmittance *= exp(-cumDens * pow(exp(-ambient), 3));

				//if (transmittance < 0.01 || pow(exp(-ambient), 2) < 0.1)
				if (transmittance < 0.01 || exp(exp(-ambient)) < 0.1)
				{
					break;
				}

				lighting += lightingAtPoint * transmittance * pow(exp(-ambient), 2);
			}
			else
			{
				lighting += 1;
			}
		}
	}

	cumulativeDensity = map01_float(cumulativeDensity);
	//cumulativeDensity = 1 - exp(-cumulativeDensity);

    lighting = exp(-lighting) * SunHighlight(ray);
	lighting = clamp(lighting, 0, 5);

#ifdef SHADERGRAPH_PREVIEW
	distToLight = 1000;
#else
	distToLight = length(_MainLightPosition + _WorldSpaceCameraPos - rayPos);
#endif

	//float3 rayToSun = G
	//distToLight = dot(ray, GetLightDir_float());


#ifdef MAIN_LIGHT_CALCULATE_SHADOWS
	lighting += ((1 - clamp(shadowMap, 0, 1)) - 0.3);
#endif

	volumeDensity = cumulativeDensity;

	if (distToBox > linearDepth)
	{
		distInBox = 0;
		volumeDensity = 0;
		lighting = 0;
	}

	if (distInBox + distToBox > linearDepth)
	{
		distInBox = linearDepth - distToBox;
	}
}