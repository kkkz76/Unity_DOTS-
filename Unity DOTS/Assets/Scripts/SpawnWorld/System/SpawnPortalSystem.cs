using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
[UpdateInGroup(typeof(InitializationSystemGroup))]
public partial struct SpawnPortalSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
       
        state.RequireForUpdate<SpawnWorldProperty>();
    }

    public void OnUpdate(ref SystemState state)
    {
        // Disable the system after it runs once
        state.Enabled = false;

        // Get the singleton entity and its aspect
        var spawnWorldEntity = SystemAPI.GetSingletonEntity<SpawnWorldProperty>();
        var spawnWorld = SystemAPI.GetAspect<SpawnWorldAspect>(spawnWorldEntity);

        // Log the number of portals to be spawned
        Debug.Log("Number of portals to spawn: " + spawnWorld.SpawnPortalCount);

        // Create an EntityCommandBuffer for deferred entity creation
        var ecb = new EntityCommandBuffer(Allocator.Temp);

        // Instantiate the portal entities
        for (var i = 0; i < spawnWorld.SpawnPortalCount; i++)
        {
            Entity spawnEntity = ecb.Instantiate(spawnWorld.PortalPrefab);
            ecb.SetComponent(spawnEntity, new LocalTransform
            {
                Position = spawnWorld.GetRandomPosition(),
                Rotation = quaternion.identity,
                Scale = 1f
            });

         
            Debug.Log("Spawned portal entity: " + spawnEntity);
        }

        // Playback the commands to the EntityManager and dispose of the buffer
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}
