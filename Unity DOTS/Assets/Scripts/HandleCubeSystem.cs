using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;



public partial struct HandleCubeSystem : ISystem 
{

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {




        //foreach (RotatingMovingCubeAspect rotatingMovingCubeAspect in SystemAPI.Query<RotatingMovingCubeAspect>().WithAll<RotatingCube>())
        //{
        //    rotatingMovingCubeAspect.MoveAndRotate(SystemAPI.Time.DeltaTime);
        //}

        var job = new RotatingCubeJob
        {
            deltaTime = SystemAPI.Time.DeltaTime
        };

        job.ScheduleParallel();

    }


   
    [WithAll(typeof(RotatingCube))]

 
    public partial struct RotatingCubeJob : IJobEntity
    {
        public float deltaTime;

        public void Execute(ref LocalTransform localTransform, in RotateSpeed rotateSpeed, in Movement movement)
        {
            localTransform = localTransform.RotateY(rotateSpeed.value * deltaTime);

            localTransform = localTransform.Translate(movement.movementVector * deltaTime);
        }
    }

}
