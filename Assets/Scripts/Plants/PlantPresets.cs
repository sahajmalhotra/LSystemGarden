using UnityEngine;

public static class PlantPresets
{
    public static PlantPreset Tree()
    {
        return new PlantPreset
        {
            presetName = "Tree",
            axiomLength = 1.4f,
            axiomRadius = 0.09f,
            defaultAngle = 25f,
            lengthScale = 0.78f,
            radiusScale = 0.70f,
            leafStartAge = 3,
            leafSizeMultiplier = 1.0f,
            includePitch = true,
            pitchChance = 0.30f
        };
    }

    public static PlantPreset Bush()
    {
        return new PlantPreset
        {
            presetName = "Bush/Vine",
            axiomLength = 1.0f,
            axiomRadius = 0.06f,
            defaultAngle = 32f,
            lengthScale = 0.72f,
            radiusScale = 0.78f,
            leafStartAge = 2,
            leafSizeMultiplier = 1.2f,
            includePitch = true,
            pitchChance = 0.55f
        };
    }
}