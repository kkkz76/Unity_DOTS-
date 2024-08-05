using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;



public partial class SpawnCubeSystem : SystemBase
{
    protected override void OnCreate()
    {
       RequireForUpdate<SpawnCubeConfig>();
    }

    
    protected override void OnUpdate()
    {
        this.Enabled = false;
        

        SpawnCubeConfig spawnCubeConfig = SystemAPI.GetSingleton<SpawnCubeConfig>();

        for(int i = 0; i < spawnCubeConfig.amountToSpawn; i++)
        {
           Entity spawnEntity =  EntityManager.Instantiate(spawnCubeConfig.cubePrefabEntity);
            EntityManager.SetComponentData(spawnEntity, new LocalTransform
            {
                Position = new float3(UnityEngine.Random.Range(-10f, +5f), 0.6f, UnityEngine.Random.Range(-4f, +7f)),
                Rotation = quaternion.identity,
                Scale = 1f
            }) ;
        }
    }

    
}
