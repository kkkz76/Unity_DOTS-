using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public readonly partial struct SpawnWorldAspect : IAspect
{
    public readonly Entity Entity;

    private readonly RefRO<SpawnWorldProperty> _spawnWorldProperty;
    private readonly RefRW<SpawnWorldRandom> _spawnWorldRandom;
    private readonly RefRW<LocalTransform> localTransform;
    private readonly RefRW<EnemySpawnPointProperty> _enemySpawnPointProperty;
    private readonly RefRW<EnemySpawnTimer> _enemySpawnTimer;

    public int SpawnPortalCount => _spawnWorldProperty.ValueRO.SpawnPortalCount;
    public Entity PortalPrefab => _spawnWorldProperty.ValueRO.PortalPrefab;
   

    public float3 getPosition()
    {
          return localTransform.ValueRO.Position; 
    }

    public LocalTransform GetRandomPortalSpawnTransform()
    {
        return new LocalTransform
        {
            Position = GetRandomPosition(),
            Rotation = GetRandomRotation(),
            Scale = GetRandomScale(0.5f)
        };
    }
    public float3 GetRandomPosition()
    {
        float3 randomPosition;
        do
        {
            randomPosition = _spawnWorldRandom.ValueRW.value.NextFloat3(MinCorner, MaxCorner);

        } while (math.distancesq(localTransform.ValueRO.Position, randomPosition) <= BASE_SAFETY_RADIUS);
        return randomPosition;
    }
    private const float BASE_SAFETY_RADIUS = 100;

    private float3 MinCorner => localTransform.ValueRO.Position - HalfDimension;
    private float3 MaxCorner => localTransform.ValueRO.Position + HalfDimension;

    private float3 HalfDimension => new()
    {
        x = _spawnWorldProperty.ValueRO.FieldDimensions.x * 0.5f,
        y = 0f,
        z = _spawnWorldProperty.ValueRO.FieldDimensions.y * 0.5f
    };
    private quaternion GetRandomRotation() => quaternion.RotateY(_spawnWorldRandom.ValueRW.value.NextFloat(-0.25f, 0.25f));
    private float GetRandomScale(float min) => _spawnWorldRandom.ValueRW.value.NextFloat(min, 1f);


    //---------------------------------- Enemy --------------------------------------------
    private int enemySpawnPointCount => _enemySpawnPointProperty.ValueRO.Value.Value.Value.Length;

    public float EnemySpawnTimer
    {
        get => _enemySpawnTimer.ValueRO.Value;
        set => _enemySpawnTimer.ValueRW.Value = value;
    }

    public bool TimeToSpawnEnemy => EnemySpawnTimer <= 0f;
    public bool EnemySpawnPointInitialized()
    {
        return _enemySpawnPointProperty.ValueRO.Value.IsCreated && enemySpawnPointCount > 0;
    }

    public float EnemySpawnRate => _spawnWorldProperty.ValueRO.EnemySpawnRate;

    public Entity EnemyPrefab => _spawnWorldProperty.ValueRO.EnemyPrefab;


    public LocalTransform GetEnemySpawnPoint()
    {
        var position = GetRandomEnemySpawnPoint();
        return new LocalTransform
        {
            Position = position,
            Rotation = quaternion.RotateY(MathsHelper.GetHeading(position, localTransform.ValueRO.Position)),
            Scale = 1f
        };
    }

    private float3 GetRandomEnemySpawnPoint()
    {
        return GetRandomEnemySpawnPoint(_spawnWorldRandom.ValueRW.value.NextInt(enemySpawnPointCount));
    }

    private float3 GetRandomEnemySpawnPoint(int i) => _enemySpawnPointProperty.ValueRO.Value.Value.Value[i];

    

}
