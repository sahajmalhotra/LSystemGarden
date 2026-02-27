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
            axiomLength = 0.75f,
            axiomRadius = 0.06f,
            defaultAngle = 38f,
lengthScale = 0.70f,
radiusScale = 0.80f,
            leafStartAge = 2,
            leafSizeMultiplier = 1.2f,
            includePitch = true,
            pitchChance = 0.55f
        };
    }
}