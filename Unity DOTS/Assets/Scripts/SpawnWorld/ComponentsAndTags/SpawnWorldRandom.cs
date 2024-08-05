using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;

public struct SpawnWorldRandom : IComponentData
{
    public Unity.Mathematics.Random value;
}
