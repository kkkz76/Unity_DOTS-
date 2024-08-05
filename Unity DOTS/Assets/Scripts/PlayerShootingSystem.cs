using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.EventSystems;

public partial class PlayerShootingSystem : SystemBase
{


    public event EventHandler OnShoot;
    protected override void OnCreate()
    {
        RequireForUpdate<Player>();
    }
    protected override void OnUpdate()
    {
        
      

        if (Input.GetKeyDown(KeyCode.T))
        {
            Entity playerEntity = SystemAPI.GetSingletonEntity<Player>();
            EntityManager.SetComponentEnabled<Stunned>(playerEntity, true);
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            Entity playerEntity = SystemAPI.GetSingletonEntity<Player>();
            EntityManager.SetComponentEnabled<Stunned>(playerEntity, false);
        }

        if (!Input.GetKey(KeyCode.Space)) {
            
            return;
        }

        SpawnCubeConfig spawnCubeConfig = SystemAPI.GetSingleton<SpawnCubeConfig>();

        //using Entity Manager

        //foreach(RefRO<LocalTransform> localTransform in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<Player>()) 
        //{
        //    Entity spawnEntity = EntityManager.Instantiate(spawnCubeConfig.cubePrefabEntity);
        //    EntityManager.SetComponentData(spawnEntity, new LocalTransform
        //    {
        //        Position = localTransform.ValueRO.Position,
        //        Rotation = quaternion.identity,
        //        Scale = 1f
        //    });
        //}

        //Spawn without structural change

        EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(WorldUpdateAllocator);

        foreach ((RefRO<LocalTransform> localTransform, Entity entity )in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<Player>().WithDisabled<Stunned>().WithEntityAccess())
        {
            Entity spawnEntity = entityCommandBuffer.Instantiate(spawnCubeConfig.cubePrefabEntity);
            entityCommandBuffer.SetComponent(spawnEntity, new LocalTransform
            {
                Position = localTransform.ValueRO.Position,
                Rotation = quaternion.identity,
                Scale = 1f
            });
            OnShoot?.Invoke(entity, EventArgs.Empty);

        }
        entityCommandBuffer.Playback(EntityManager);
    }
}
