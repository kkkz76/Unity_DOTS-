using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine;

public partial struct RotatingSystem : ISystem
{

    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<RotateSpeed>();
    }

    //[BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        state.Enabled = false;

        return;
        foreach ((RefRW<LocalTransform> localTransfrom, RefRO<RotateSpeed> rotateSpeed) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<RotateSpeed>>())
        {
            float power = 1f;
            for (int i = 0; i < 100000; i++)
            {
                power *= 2f;
                power /= 2f;
            }

            localTransfrom.ValueRW = localTransfrom.ValueRO.RotateY(rotateSpeed.ValueRO.value * SystemAPI.Time.DeltaTime * power);
        }

        //RotatingCubeJob rotatingCubeJob = new RotatingCubeJob
        //{
        //    deltaTime = Time.deltaTime,
        //};

        //rotatingCubeJob.ScheduleParallel();
    }

    //[BurstCompile]
    [WithAll(typeof(RotatingCube))]
    public partial struct RotatingCubeJob : IJobEntity
    {
        public float deltaTime;
        public void Execute(ref LocalTransform localTransform, in RotateSpeed rotateSpeed)
        {
            float power = 1f;
            for (int i = 0; i < 100000; i++)
            {
                power *= 2f;
                power /= 2f;
            }

            localTransform = localTransform.RotateY(rotateSpeed.value * deltaTime * power);
        }
    }
}
