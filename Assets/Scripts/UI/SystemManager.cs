using System.Collections.Generic;
using UnityEngine;

public class SystemManager : MonoBehaviour
{
    public Turtle3D turtle;

    [Header("Preset Selection")]
    public bool useBush = false;

    [Header("UI-controlled Params")]
    [Range(0, 10)] public int iterations = 5;
    [Range(5f, 45f)] public float angle = 25f;
    [Range(0.5f, 0.95f)] public float lengthScale = 0.78f;
    [Range(0.5f, 0.95f)] public float radiusScale = 0.70f;
    [Range(0.1f, 10f)] public float growthSpeed = 3f;

    [Header("Growth")]
    public bool growOverTime = true;

    private List<Symbol> fullSymbols;
    private int drawCount;

    void Start()
{
    // Start empty so UI controls first generation
    if (turtle != null) turtle.ClearPlant();
}
[ContextMenu("Generate Now")]

    public void Generate()
    {
        Debug.Log($"SystemManager.Generate called. useBush={useBush}, iterations={iterations}");
        if (turtle == null)
        {
            Debug.LogError("SystemManager: Turtle not assigned.");
            return;
        }

        PlantPreset preset = useBush ? PlantPresets.Bush() : PlantPresets.Tree();

        // Apply UI overrides
        preset.defaultAngle = angle;
        preset.lengthScale = lengthScale;
        preset.radiusScale = radiusScale;

        LSystem sys = new LSystem(preset.MakeAxiom(), iterations, preset.defaultAngle, preset.lengthScale, preset.radiusScale);
        sys.mode = useBush ? PlantMode.Bush : PlantMode.Tree;
        sys.leafStartAge = preset.leafStartAge;
        sys.leafSizeMultiplier = preset.leafSizeMultiplier;
        sys.includePitch = preset.includePitch;
        sys.pitchChance = preset.pitchChance;

        fullSymbols = sys.Generate();

        drawCount = growOverTime ? 0 : fullSymbols.Count;

        turtle.ClearPlant();
        if (!growOverTime)
        {
            turtle.Interpret(fullSymbols, angle, clearFirst: true);
        }
    }

    void Update()
    {
        if (!growOverTime || fullSymbols == null) return;

        // Increase how many symbols we draw
        drawCount = Mathf.Min(fullSymbols.Count, drawCount + Mathf.CeilToInt(growthSpeed * Time.deltaTime * 60f));

        // Interpret only first drawCount symbols
        turtle.Interpret(fullSymbols.GetRange(0, drawCount), angle, clearFirst: true);
    }
}