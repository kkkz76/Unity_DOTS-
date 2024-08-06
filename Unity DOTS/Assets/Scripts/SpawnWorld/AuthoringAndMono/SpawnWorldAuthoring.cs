using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;


public class SpawnWorldAuthoring : MonoBehaviour
{
    public float2 FieldDimensions;
    public int SpawnPortalCount;
    public GameObject PortalPrefab;
    public uint RandomSeed;
    public GameObject EnemyPrefab;
    public float EnemySpawnRate;

    public class SpawnWorldBaker : Baker<SpawnWorldAuthoring>
    {
        public override void Bake(SpawnWorldAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity ,new SpawnWorldProperty
            {
                FieldDimensions = authoring.FieldDimensions,
                SpawnPortalCount = authoring.SpawnPortalCount,
                PortalPrefab =  GetEntity(authoring.PortalPrefab , TransformUsageFlags.Dynamic),
                EnemyPrefab = GetEntity(authoring.EnemyPrefab , TransformUsageFlags.Dynamic),
                EnemySpawnRate = authoring.EnemySpawnRate
            });

            AddComponent(entity, new SpawnWorldRandom
            {
                value = Unity.Mathematics.Random.CreateFromIndex(authoring.RandomSeed)
            });

            AddComponent<EnemySpawnPointProperty>(entity);
            AddComponent<EnemySpawnTimer>(entity);
        }
    }
}
