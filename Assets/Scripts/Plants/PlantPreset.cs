using System.Collections.Generic;
using UnityEngine;

// Allows this class to be shown and edited in the Unity Inspector
[System.Serializable]
public class PlantPreset
{
    // Display name of the preset (used for UI or identification)
    public string presetName = "Plant";

    // Axiom base parameters (starting trunk/stem values)

    // Initial segment length
    public float axiomLength = 1.2f;

    // Initial segment thickness (radius)
    public float axiomRadius = 0.08f;

    // Rule parameters (also controlled through UI sliders)

    // Default branching angle in degrees
    public float defaultAngle = 25f;

    // Multiplier applied to segment length each generation
    public float lengthScale = 0.75f;

    // Multiplier applied to segment radius each generation
    public float radiusScale = 0.7f;

    // Leaf behavior settings

    // The generation (age) at which leaves begin to appear
    public int leafStartAge = 3;

    // Scales the overall size of generated leaves
    public float leafSizeMultiplier = 1.0f;

    // Extra “plausibility” settings for more natural growth

    // Enables vertical pitch variation in branches
    public bool includePitch = true;

    // Probability of applying pitch variation (used deterministically later)
    public float pitchChance = 0.35f; // deterministic pattern used later

    // Creates the initial symbol (axiom) for the L-system
    public List<Symbol> MakeAxiom()
    {
        // Starts with a single forward-growth symbol 'F'
        // Parameters: (symbol character, length, radius, age)
        return new List<Symbol> { new Symbol('F', axiomLength, axiomRadius, 0) };
    }
}