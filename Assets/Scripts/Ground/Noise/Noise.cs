using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

using static Unity.Mathematics.math;

public static partial class Noise {

	[Serializable]
	public struct Settings {

		public int seed;

		[Range(0.1f, 4)]
		public float frequency;

		[Range(1, 6)]
		public int octaves;

		[Range(2, 4)]
		public float lacunarity;

		[Range(0f, 1f)]
		public float persistence;

		public static Settings Default => new Settings {
			frequency = 4,
			octaves = 1,
			lacunarity = 2,
			persistence = 0.5f
		};
	}

	public interface INoise {
		Sample4 GetNoise4 (float4x3 positions, SmallXXHash4 hash, float frequency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Sample4 GetFractalNoise<N> (
		float4x3 position, Settings settings
	) where N : struct, INoise {
		var hash = SmallXXHash4.Seed(settings.seed);
		float frequency = settings.frequency;
		float amplitude = 1f, amplitudeSum = 0f;
		Sample4 sum = default;

		for (int o = 0; o < settings.octaves; o++) {
			sum += amplitude * default(N).GetNoise4(position, hash + o, frequency);
			amplitudeSum += amplitude;
			frequency *= settings.lacunarity;
			amplitude *= settings.persistence;
		}
		return sum / amplitudeSum;
	}

	[BurstCompile(FloatPrecision.Standard, FloatMode.Fast, CompileSynchronously = true)]
	public struct Job<N> : IJobFor where N : struct, INoise {

		[ReadOnly]
		public NativeArray<float3x4> positions;

		[WriteOnly]
		public NativeArray<float4> noise;

		public Settings settings;

		public float3x4 domainTRS;

		public void Execute (int i) => noise[i] = GetFractalNoise<N>(
			domainTRS.TransformVectors(transpose(positions[i])), settings
		).v;

		public static JobHandle ScheduleParallel (
			NativeArray<float3x4> positions, NativeArray<float4> noise,
			Settings settings, SpaceTRS domainTRS, int resolution, int size, JobHandle dependency
		) => new Job<N> {
			positions = positions,
			noise = noise,
			settings = settings,
			domainTRS = domainTRS.Matrix,
		}.ScheduleParallel(positions.Length, resolution * size, dependency);
	}

	public delegate JobHandle ScheduleDelegate (
		NativeArray<float3x4> positions, NativeArray<float4> noise,
		Settings settings, SpaceTRS domainTRS, int resolution, int size, JobHandle dependency
	);
}