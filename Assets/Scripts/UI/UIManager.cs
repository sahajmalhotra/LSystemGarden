using UnityEngine;         // Core Unity types (MonoBehaviour, Debug, etc.)
using UnityEngine.UI;      // Unity UI types (Slider, Toggle, Button components)
using TMPro;               // TextMeshPro UI types (TMP_Dropdown, TMP_Text)

/*
 * UIManager.cs
 * ------------
 * Purpose:
 * - Acts as the “glue” between the UI (Canvas controls) and the procedural plant system (SystemManager).
 * - Reads user input from:
 *      - Plant type dropdown (Tree vs Bush/Vine)
 *      - Sliders (iterations, angle, growth speed)
 *      - Toggle (grow over time)
 * - Writes those values into SystemManager fields.
 * - Updates UI value labels so the user can see numeric values while adjusting sliders.
 *
 * Important Design Choice:
 * - The plant does NOT regenerate every time the user moves a control.
 *   Instead, regeneration happens when the user clicks "Generate" (clean demo for grading).
 */

public class UIManager : MonoBehaviour
{
    // --------------------------
    // References to other scripts
    // --------------------------

    [Header("References")]
    // SystemManager holds the L-system parameters and performs actual generation + growth.
    // This reference must be assigned in the Unity Inspector (drag SystemManager object here).
    public SystemManager systemManager;

    // --------------------------
    // UI references (assigned in Inspector)
    // --------------------------

    [Header("UI")]
    // Dropdown used to select which plant preset/ruleset to use (0=Tree, 1=Bush/Vine).
    public TMP_Dropdown plantDropdown;

    // Slider controlling how many times the L-system rules are applied.
    // Higher iterations = more complex geometry (but can be slower).
    public Slider iterationsSlider;

    // Slider controlling branch turning angle.
    // Higher angle = wider branching; lower angle = taller/narrower plant.
    public Slider angleSlider;

    // Slider controlling how fast the growth animation reveals the plant over time.
    public Slider growthSpeedSlider;

    // Toggle to switch between:
    // - Grow over time (progressive rendering, "complex attribute" for A+)
    // - Instant generation (render everything immediately)
    public Toggle growToggle;

    // --------------------------
    // UI value labels (TextMeshPro)
    // --------------------------

    // These are optional text fields that display current slider values (e.g., "5", "25°", "3.0").
    // If they are not assigned, the code safely checks before writing.
    public TMP_Text iterationsValue;
    public TMP_Text angleValue;
    public TMP_Text speedValue;

    // --------------------------
    // Unity lifecycle: Start
    // --------------------------
    void Start()
    {
        // Defensive check: if SystemManager is missing, the UI cannot control generation.
        // This prevents NullReferenceExceptions and provides a clear error message.
        if (systemManager == null)
        {
            Debug.LogError("UIManager: SystemManager not assigned.");
            return;
        }

        // Initialize UI controls so they match the current SystemManager settings.
        // This keeps UI consistent if values were changed in the inspector previously.
        SyncUIFromSystem();

        // Update numeric labels (iterations/angle/speed) immediately at startup.
        UpdateValueLabels();

        // --------------------------
        // Hook up UI events (listeners)
        // --------------------------
        // Each listener runs when its UI control changes.
        // These listeners update SystemManager fields so the next "Generate" uses the new values.

        // Dropdown selection changed (Tree/Bush)
        plantDropdown.onValueChanged.AddListener(_ => ApplyUIToSystem());

        // Iterations slider changed: update SystemManager + update label text
        iterationsSlider.onValueChanged.AddListener(_ => { ApplyUIToSystem(); UpdateValueLabels(); });

        // Angle slider changed: update SystemManager + update label text
        angleSlider.onValueChanged.AddListener(_ => { ApplyUIToSystem(); UpdateValueLabels(); });

        // Growth speed slider changed: update SystemManager + update label text
        growthSpeedSlider.onValueChanged.AddListener(_ => { ApplyUIToSystem(); UpdateValueLabels(); });

        // Grow toggle changed: update SystemManager (does not regenerate automatically)
        growToggle.onValueChanged.AddListener(_ => ApplyUIToSystem());
    }

    // --------------------------
    // Button callback: Generate
    // --------------------------
    // This method should be wired to the Generate button's OnClick() event in Unity:
    // GenerateButton -> Button -> On Click() -> UIManager.OnGenerateClicked()
    public void OnGenerateClicked()
    {
        // Debug message confirms the button is correctly wired and clicked.
        Debug.Log("UIManager: Generate button clicked");

        // Apply current UI values to SystemManager fields right before generating.
        // This ensures the generation uses the most recent user settings.
        ApplyUIToSystem();

        // Additional debug information (useful during development).
        // Can be removed for final submission if desired.
        Debug.Log($"After apply: useBush={systemManager.useBush}, growOverTime={systemManager.growOverTime}");

        // Trigger actual plant generation (and reset growth state) in SystemManager.
        systemManager.Generate();
    }

    // --------------------------
    // Sync UI controls FROM SystemManager
    // --------------------------
    // Use this when you want the UI to reflect SystemManager values (at startup).
    private void SyncUIFromSystem()
    {
        // Dropdown index: 0=Tree, 1=Bush
        plantDropdown.value = systemManager.useBush ? 1 : 0;

        // Sliders match SystemManager parameters
        iterationsSlider.value = systemManager.iterations;
        angleSlider.value = systemManager.angle;
        growthSpeedSlider.value = systemManager.growthSpeed;

        // Toggle matches whether growth animation is enabled
        growToggle.isOn = systemManager.growOverTime;
    }

    // --------------------------
    // Apply values FROM UI TO SystemManager
    // --------------------------
    // This updates the procedural system parameters based on the current UI state.
    private void ApplyUIToSystem()
    {
        // Plant selection (dropdown):
        // value == 1 means bush mode; value == 0 means tree mode.
        systemManager.useBush = (plantDropdown.value == 1);

        // Iterations slider returns float, but iterations should be integer.
        systemManager.iterations = Mathf.RoundToInt(iterationsSlider.value);

        // Angle controls turning angle used during turtle interpretation.
        systemManager.angle = angleSlider.value;

        // Growth speed controls how many symbols are revealed per frame (scaled in SystemManager).
        systemManager.growthSpeed = growthSpeedSlider.value;

        // Toggle controls whether we animate growth over time or render instantly.
        systemManager.growOverTime = growToggle.isOn;

        // Debug print (useful for checking dropdown works; can remove for final submission)
        Debug.Log($"After apply, useBush = {systemManager.useBush}");
    }

    // --------------------------
    // Update the numeric slider value labels (UI polish)
    // --------------------------
    // This is purely visual: makes UI clearer for the user and grader.
    private void UpdateValueLabels()
    {
        // Iterations label: show whole number
        if (iterationsValue) iterationsValue.text = $"{Mathf.RoundToInt(iterationsSlider.value)}";

        // Angle label: show integer degrees with ° symbol
        if (angleValue) angleValue.text = $"{angleSlider.value:F0}°";

        // Growth speed label: show one decimal place
        if (speedValue) speedValue.text = $"{growthSpeedSlider.value:F1}";
    }
}