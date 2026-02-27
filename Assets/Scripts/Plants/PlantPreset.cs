using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlantPreset
{
    public string presetName = "Plant";

    // Axiom base parameters
    public float axiomLength = 1.2f;
    public float axiomRadius = 0.08f;

    // Rule parameters (controlled by UI too)
    public float defaultAngle = 25f;
    public float lengthScale = 0.75f;
    public float radiusScale = 0.7f;

    // Leaf behavior
    public int leafStartAge = 3;
    public float leafSizeMultiplier = 1.0f;

    // Extra “plausibility”
    public bool includePitch = true;
    public float pitchChance = 0.35f; // deterministic pattern used later

    public List<Symbol> MakeAxiom()
    {
        return new List<Symbol> { new Symbol('F', axiomLength, axiomRadius, 0) };
    }
}