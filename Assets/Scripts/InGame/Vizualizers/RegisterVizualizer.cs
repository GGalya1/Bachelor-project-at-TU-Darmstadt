using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Component for visualizing a digital Register unit within a 3D scene.
/// It is responsible for: displaying the data UI panel, changing the model's color 
/// upon activation, and flashing (blinking) when a Clock signal is triggered.
/// It inherits common visualization logic from <see cref="BaseVizualizer"/>.
/// </summary>
public class RegisterVizualizer: BaseVizualizer
{

    /// <summary>
    /// The cached UI controller for the information panel (InfoPanelUI), 
    /// instantiated during Awake. Provides access for the Level_X_Regisseur 
    /// to update the displayed register value.
    /// </summary>
    private RegisterControlPanel _uiController;
    public RegisterControlPanel UIRegisterPanel => _uiController;

    [FormerlySerializedAs("_writeEnableIndicator")]
    [Header("Write Enable Visualization")]
    [Tooltip("Object that controls WE-signal and Stop-image")]
    [SerializeField] private GameObject writeEnableIndicator;
    public bool isWriteEnabled;

    [FormerlySerializedAs("_blinker")]
    [Header("Blinker of sequential component")]
    [SerializeField] private Blinker blinker;

    protected override void Awake() {
        base.Awake();

        // Set the initial/default data on the UI panel
        if (_uiController != null)
        {
            _uiController.Display("N/A", "0");
        }

        // Set the initial state for STOP indicator
        if (writeEnableIndicator != null)
        {
            writeEnableIndicator.SetActive(false);
            isWriteEnabled = true;
            _uiController.WeButton.onClick.AddListener(SwitchWriteEnableVisualization);
        }
    }
    public void SwitchWriteEnableVisualization()
    {
        if (writeEnableIndicator != null)
        {
            // if WE is true -> indicator must be inactive
            // if WE is false -> indicator must be active
            isWriteEnabled = !isWriteEnabled;
            writeEnableIndicator.SetActive(!isWriteEnabled);
            HideData();

        }
    }
    public void ForceUpdateWriteEnableVisualization(bool flag) {
        if (writeEnableIndicator != null)
        {
            isWriteEnabled = flag;
            writeEnableIndicator.SetActive(!isWriteEnabled);
        }
    }

    /// <summary>
    /// Concrete implementation of the base class initialization. 
    /// Retrieves the InfoPanelUI component from the instantiated UI prefab.
    /// </summary>
    protected override void InitializePanelController()
    {
        _uiController = panelInstance.GetComponent<RegisterControlPanel>();
        if (_uiController == null)
        {
            Debug.LogError($"InfoPanelUI component not found on the prefab for {gameObject.name}!");
        }
    }

    public void TriggerBlink()
    {
        blinker.Trigger();
    }

    public override void ResetVizualization() { }
}
