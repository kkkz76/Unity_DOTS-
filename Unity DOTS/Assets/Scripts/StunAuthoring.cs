using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class StunAuthoring : MonoBehaviour
{

    public class Baker : Baker<StunAuthoring>
    {
        public override void Bake(StunAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Stunned());
            SetComponentEnabled<Stunned>(entity, false);
        }
    }
}
public struct Stunned : IComponentData , IEnableableComponent
{

}