using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

//[BurstCompile]
public partial struct SpawnEnemySystem : ISystem
{
    //[BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<EnemySpawnTimer>();
    }

   //[BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
    }

   [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var deltaTime = SystemAPI.Time.DeltaTime;
        var ecbSingleton = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>();

        // Create a command buffer with parallel support
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();

        // Create and schedule the job
        var spawnZombieJob = new SpawnZombieJob
        {
            DeltaTime = deltaTime,
            ECB = ecb
        };

        // Schedule the job with parallel support
        state.Dependency = spawnZombieJob.ScheduleParallel(state.Dependency);
    }

   [BurstCompile]
    public partial struct SpawnZombieJob : IJobEntity
    {
        public float DeltaTime;
        public EntityCommandBuffer.ParallelWriter ECB;

       
        private void Execute(SpawnWorldAspect spawnWorldAspect, [EntityIndexInQuery] int entityIndex)
        {
            spawnWorldAspect.EnemySpawnTimer -= DeltaTime;
            if (!spawnWorldAspect.TimeToSpawnEnemy) return;
            if (!spawnWorldAspect.EnemySpawnPointInitialized()) return;

            spawnWorldAspect.EnemySpawnTimer = spawnWorldAspect.EnemySpawnRate;
            var newEnemy = ECB.Instantiate(entityIndex, spawnWorldAspect.EnemyPrefab);

            var newEnemyTransform = spawnWorldAspect.GetEnemySpawnPoint();
            ECB.SetComponent(entityIndex, newEnemy, new LocalTransform
            {
                Position = newEnemyTransform.Position,
                Rotation = quaternion.identity,
                Scale = 1f
            });


            //var enemyHeading = MathsHelper.GetHeading(newEnemyTransform.Position, spawnWorldAspect.getPosition());
            //ECB.SetComponent(entityIndex, newEnemy,new enemyHeading { Value = enemyHeading });
        }
    }
}
