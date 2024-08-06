using Unity.Mathematics;

public static class MathsHelper
{
    public static float GetHeading(float3 objectPosition, float3 targetPosition)
    {
        var x = targetPosition.x - objectPosition.x;
        var z = targetPosition.z - objectPosition.z;
        return math.atan2(z, x) + math.PI;
    }
}
