using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("References")]
    public SystemManager systemManager;

    [Header("UI")]
    public TMP_Dropdown plantDropdown;
    public Slider iterationsSlider;
    public Slider angleSlider;
    public Slider growthSpeedSlider;
    public Toggle growToggle;

    public TMP_Text iterationsValue;
    public TMP_Text angleValue;
    public TMP_Text speedValue;

    void Start()
    {
        if (systemManager == null)
        {
            Debug.LogError("UIManager: SystemManager not assigned.");
            return;
        }

        // Initialize UI from SystemManager defaults
        SyncUIFromSystem();
        UpdateValueLabels();

        // Hook events
        plantDropdown.onValueChanged.AddListener(_ => ApplyUIToSystem());
        iterationsSlider.onValueChanged.AddListener(_ => { ApplyUIToSystem(); UpdateValueLabels(); });
        angleSlider.onValueChanged.AddListener(_ => { ApplyUIToSystem(); UpdateValueLabels(); });
        growthSpeedSlider.onValueChanged.AddListener(_ => { ApplyUIToSystem(); UpdateValueLabels(); });
        growToggle.onValueChanged.AddListener(_ => ApplyUIToSystem());
    }

    public void OnGenerateClicked()
{
    Debug.Log("UIManager: Generate button clicked");
    ApplyUIToSystem();
    Debug.Log($"After apply: useBush={systemManager.useBush}, growOverTime={systemManager.growOverTime}");
    systemManager.Generate();
}

    private void SyncUIFromSystem()
    {
        plantDropdown.value = systemManager.useBush ? 1 : 0;
        iterationsSlider.value = systemManager.iterations;
        angleSlider.value = systemManager.angle;
        growthSpeedSlider.value = systemManager.growthSpeed;
        growToggle.isOn = systemManager.growOverTime;
    }

    private void ApplyUIToSystem()
    {
        systemManager.useBush = (plantDropdown.value == 1);
        systemManager.iterations = Mathf.RoundToInt(iterationsSlider.value);
        systemManager.angle = angleSlider.value;
        systemManager.growthSpeed = growthSpeedSlider.value;
        systemManager.growOverTime = growToggle.isOn;
        Debug.Log($"After apply, useBush = {systemManager.useBush}");
    }

    private void UpdateValueLabels()
    {
        if (iterationsValue) iterationsValue.text = $"{Mathf.RoundToInt(iterationsSlider.value)}";
        if (angleValue) angleValue.text = $"{angleSlider.value:F0}°";
        if (speedValue) speedValue.text = $"{growthSpeedSlider.value:F1}";
    }
}