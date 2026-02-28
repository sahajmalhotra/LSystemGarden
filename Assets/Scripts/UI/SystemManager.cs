using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// SystemManager
/// --------------
/// Controls the full L-system generation pipeline.
/// 
/// Responsibilities:
/// 1. Select preset (Tree or Bush).
/// 2. Apply UI parameter overrides.
/// 3. Generate L-system symbol list.
/// 4. Send symbols to Turtle3D for interpretation.
/// 5. Optionally animate plant growth over time.
/// </summary>
public class SystemManager : MonoBehaviour
{
    /// <summary>
    /// Reference to Turtle3D interpreter responsible for rendering geometry.
    /// Must be assigned in Inspector.
    /// </summary>
    public Turtle3D turtle;

    // ============================================================
    // Preset Selection
    // ============================================================

    [Header("Preset Selection")]
    public bool useBush = false;
    // If true → use Bush preset
    // If false → use Tree preset

    // ============================================================
    // UI-Controlled Parameters
    // ============================================================

    [Header("UI-controlled Params")]

    [Range(0, 10)]
    public int iterations = 5;
    // Number of L-system rewriting iterations.
    // Higher = more complex plant.

    [Range(5f, 45f)]
    public float angle = 25f;
    // Default rotation angle for turtle turns.

    [Range(0.5f, 0.95f)]
    public float lengthScale = 0.78f;
    // Multiplier applied to branch length each generation.

    [Range(0.5f, 0.95f)]
    public float radiusScale = 0.70f;
    // Multiplier applied to branch thickness each generation.

    [Range(0.1f, 10f)]
    public float growthSpeed = 3f;
    // Controls how fast the plant grows when animated.

    // ============================================================
    // Growth Settings
    // ============================================================

    [Header("Growth")]
    public bool growOverTime = true;
    // If true → plant is revealed gradually.
    // If false → entire plant appears instantly.

    // ============================================================
    // Internal State
    // ============================================================

    private List<Symbol> fullSymbols;
    // Full list of generated L-system symbols.

    private int drawCount;
    // How many symbols are currently being interpreted (for growth animation).

    // ============================================================
    // Initialization
    // ============================================================

    void Start()
    {
        // Start with an empty scene.
        // Generation happens only when explicitly triggered.
        if (turtle != null) 
            turtle.ClearPlant();
    }

    // Adds a button in the Unity Inspector for manual generation
    [ContextMenu("Generate Now")]

    /// <summary>
    /// Generates a new plant based on current UI parameters.
    /// </summary>
    public void Generate()
    {
        Debug.Log($"SystemManager.Generate called. useBush={useBush}, iterations={iterations}");

        // Safety check
        if (turtle == null)
        {
            Debug.LogError("SystemManager: Turtle not assigned.");
            return;
        }

        // ============================================================
        // Step 1: Choose preset (Tree or Bush)
        // ============================================================

        PlantPreset preset = useBush ? PlantPresets.Bush() : PlantPresets.Tree();

        // ============================================================
        // Step 2: Apply UI overrides to preset
        // ============================================================

        preset.defaultAngle = angle;
        preset.lengthScale = lengthScale;
        preset.radiusScale = radiusScale;

        // ============================================================
        // Step 3: Create L-system
        // ============================================================

        LSystem sys = new LSystem(
            preset.MakeAxiom(),
            iterations,
            preset.defaultAngle,
            preset.lengthScale,
            preset.radiusScale
        );

        // Configure additional system behavior
        sys.mode = useBush ? PlantMode.Bush : PlantMode.Tree;
        sys.leafStartAge = preset.leafStartAge;
        sys.leafSizeMultiplier = preset.leafSizeMultiplier;
        sys.includePitch = preset.includePitch;
        sys.pitchChance = preset.pitchChance;

        // Generate full symbol sequence
        fullSymbols = sys.Generate();

        // ============================================================
        // Step 4: Setup growth behavior
        // ============================================================

        drawCount = growOverTime ? 0 : fullSymbols.Count;

        // Clear previous geometry
        turtle.ClearPlant();

        // If not animating growth, draw entire structure immediately
        if (!growOverTime)
        {
            turtle.Interpret(fullSymbols, angle, clearFirst: true);
        }
    }

    // ============================================================
    // Growth Animation
    // ============================================================

    void Update()
    {
        // If growth animation disabled or not generated yet → exit
        if (!growOverTime || fullSymbols == null) 
            return;

        // Gradually increase number of symbols drawn
        drawCount = Mathf.Min(
            fullSymbols.Count,
            drawCount + Mathf.CeilToInt(growthSpeed * Time.deltaTime * 60f)
        );

        // Interpret only the visible portion
        turtle.Interpret(
            fullSymbols.GetRange(0, drawCount),
            angle,
            clearFirst: true
        );
    }
}