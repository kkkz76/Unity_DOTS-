using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public readonly partial struct SpawnWorldAspect : IAspect
{
    public readonly Entity Entity;

    private readonly RefRO<SpawnWorldProperty> _spawnWorldProperty;
    private readonly RefRW<SpawnWorldRandom> _spawnWorldRandom;
    public readonly RefRW<LocalTransform> localTransform;

    public int SpawnPortalCount => _spawnWorldProperty.ValueRO.SpawnPortalCount;

    public Entity PortalPrefab => _spawnWorldProperty.ValueRO.PortalPrefab;

    public float3 GetRandomPosition()
    {
        float3 randomPosition;
        randomPosition = _spawnWorldRandom.ValueRW.value.NextFloat3(MinCorner,MaxCorner);
        return randomPosition;
    }


    private float3 MinCorner => localTransform.ValueRO.Position - HalfDimension;
    private float3 MaxCorner => localTransform.ValueRO.Position + HalfDimension;

    private float3 HalfDimension => new()
    {
        x = _spawnWorldProperty.ValueRO.FieldDimensions.x * 0.5f,
        y = 0f,
        z = _spawnWorldProperty.ValueRO.FieldDimensions.y * 0.5f
    };


}
