using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class SpawnCubesConfigAuthoring : MonoBehaviour
{
    public GameObject cubePrefab;
    public int amountToSpawn;


    public class Baker : Baker<SpawnCubesConfigAuthoring>
    {
        public override void Bake(SpawnCubesConfigAuthoring authoring)
        {
           Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new SpawnCubeConfig
            {
                cubePrefabEntity = GetEntity(authoring.cubePrefab, TransformUsageFlags.Dynamic),
                amountToSpawn = authoring.amountToSpawn,
            });
        }
    }
}


public struct SpawnCubeConfig : IComponentData
{
    public Entity cubePrefabEntity;
    public int amountToSpawn;
}
