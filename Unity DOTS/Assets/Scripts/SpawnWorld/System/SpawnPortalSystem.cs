using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using static EnemySpawnPointProperty;

//[BurstCompile]
[UpdateInGroup(typeof(InitializationSystemGroup))]
public partial struct SpawnPortalSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<SpawnWorldProperty>();
    }

    public void OnUpdate(ref SystemState state)
    {
       state.Enabled = false;
            var spawnWorldEntity = SystemAPI.GetSingletonEntity<SpawnWorldProperty>();
            var spawnWorld = SystemAPI.GetAspect<SpawnWorldAspect>(spawnWorldEntity);

            var ecb = new EntityCommandBuffer(Allocator.Temp);
            var portalOffest = new float3(0f, 0f, 1f);
            
            var builder = new BlobBuilder(Allocator.Temp);
            ref var spawnPoints = ref builder.ConstructRoot<EnemySpawnPointsBlob>();
            var arrayBuilder = builder.Allocate(ref spawnPoints.Value, spawnWorld.SpawnPortalCount);
            
            for (var i = 0; i < spawnWorld.SpawnPortalCount; i++)
            {
                var newTombstone = ecb.Instantiate(spawnWorld.PortalPrefab);
                var newTombstoneTransform = spawnWorld.GetRandomPortalSpawnTransform();
                ecb.SetComponent(newTombstone, newTombstoneTransform);
                
                var newZombieSpawnPoint = newTombstoneTransform.Position + portalOffest;
                arrayBuilder[i] = newZombieSpawnPoint;
            }

            var blobAsset = builder.CreateBlobAssetReference<EnemySpawnPointsBlob>(Allocator.Persistent);
            ecb.SetComponent(spawnWorldEntity, new EnemySpawnPointProperty{Value = blobAsset});
            builder.Dispose();

            ecb.Playback(state.EntityManager);
    }
}
