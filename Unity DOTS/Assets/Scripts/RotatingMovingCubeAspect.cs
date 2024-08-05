using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public readonly partial struct RotatingMovingCubeAspect : IAspect
{
    public readonly RefRW<LocalTransform> localTransform;
    public readonly RefRO<RotateSpeed> rotateSpeed;
    public readonly RefRO<Movement> movement;

    public void MoveAndRotate(float DeltaTime)
    {
        localTransform.ValueRW = localTransform.ValueRO.RotateY(rotateSpeed.ValueRO.value * DeltaTime);
        localTransform.ValueRW = localTransform.ValueRO.Translate(movement.ValueRO.movementVector * DeltaTime);
    }
}
   

