using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct SpawnWorldProperty : IComponentData
{
    public float2 FieldDimensions;
    public int SpawnPortalCount;
    public Entity PortalPrefab;
    public Entity EnemyPrefab;
    public float EnemySpawnRate;
}


public struct EnemySpawnTimer : IComponentData
{
    public float Value;
}
