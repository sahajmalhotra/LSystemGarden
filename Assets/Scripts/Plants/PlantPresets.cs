using UnityEngine;

// Static class that contains predefined plant configuration presets
public static class PlantPresets
{
    // Returns a preset configured to behave like a tree
    public static PlantPreset Tree()
    {
        return new PlantPreset
        {
            // Name shown/used for this preset
            presetName = "Tree",

            // Initial trunk length
            axiomLength = 1.4f,

            // Initial trunk thickness
            axiomRadius = 0.09f,

            // Default branch angle in degrees
            defaultAngle = 25f,

            // Multiplier applied to branch length each generation
            lengthScale = 0.78f,

            // Multiplier applied to branch radius each generation
            radiusScale = 0.70f,

            // Generation (age) when leaves start appearing
            leafStartAge = 3,

            // Multiplier controlling overall leaf size
            leafSizeMultiplier = 1.0f,

            // Enables vertical pitch variation for more natural growth
            includePitch = true,

            // Probability of pitch being applied to a branch
            pitchChance = 0.30f
        };
    }

    // Returns a preset configured to behave like a bush or vine
    public static PlantPreset Bush()
    {
        return new PlantPreset
        {
            // Name shown/used for this preset
            presetName = "Bush/Vine",

            // Initial stem length
            axiomLength = 0.75f,

            // Initial stem thickness
            axiomRadius = 0.06f,

            // Wider branch angle for bushier appearance
            defaultAngle = 38f,

            // Faster reduction in length for compact growth
            lengthScale = 0.70f,

            // Slower reduction in thickness to keep bush dense
            radiusScale = 0.80f,

            // Leaves appear earlier than tree preset
            leafStartAge = 2,

            // Slightly larger leaves for fuller look
            leafSizeMultiplier = 1.2f,

            // Enables pitch variation for more organic spread
            includePitch = true,

            // Higher chance of pitch for more chaotic branching
            pitchChance = 0.55f
        };
    }
}