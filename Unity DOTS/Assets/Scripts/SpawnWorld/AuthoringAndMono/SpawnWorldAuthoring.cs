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

    public class SpawnWorldBaker : Baker<SpawnWorldAuthoring>
    {
        public override void Bake(SpawnWorldAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity ,new SpawnWorldProperty
            {
                FieldDimensions = authoring.FieldDimensions,
                SpawnPortalCount = authoring.SpawnPortalCount,
                PortalPrefab =  GetEntity(authoring.PortalPrefab , TransformUsageFlags.Dynamic)
            });

            AddComponent(entity, new SpawnWorldRandom
            {
                value = Unity.Mathematics.Random.CreateFromIndex(authoring.RandomSeed)
            }); 
        }
    }
}
