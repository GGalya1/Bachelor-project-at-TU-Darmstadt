using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

public class IntructionDataMemoryVizualizer: BaseVizualizer
{
    private InstrMemoryControlPanel _uiController;
    public InstrMemoryControlPanel UIRegisterPanel => _uiController;

    [FormerlySerializedAs("_writeEnableIndicator")]
    [Header("Write Enable Visualization")]
    [Tooltip("Object that controls WE-signal and Stop-image")]
    [SerializeField] private GameObject writeEnableIndicator;
    public bool isWriteEnabled;

    [FormerlySerializedAs("_blinker")]
    [Header("Blinker of sequential component")]
    [SerializeField] private Blinker blinker;

    protected override void Awake()
    {
        base.Awake();

        // Set the initial state for STOP indicator
        if (writeEnableIndicator != null)
        {
            writeEnableIndicator.SetActive(false);
            isWriteEnabled = true;
            _uiController.WeButton.onClick.AddListener(SwitchWriteEnableVisualization);
        }
    }

    /// <summary>
    /// Concrete implementation of the base class initialization. 
    /// Retrieves the InfoPanelUI component from the instantiated UI prefab.
    /// </summary>
    protected override void InitializePanelController()
    {
        _uiController = panelInstance.GetComponent<InstrMemoryControlPanel>();
        if (_uiController == null)
        {
            Debug.LogError($"InstrMemoryControlPanel component not found on the prefab for {gameObject.name}!");
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
    public void ForceUpdateWriteEnableVisualization(bool flag)
    {
        if (writeEnableIndicator != null)
        {
            isWriteEnabled = flag;
            writeEnableIndicator.SetActive(!isWriteEnabled);
        }
    }

    public void TriggerBlink() {
        blinker.Trigger();
    }

    public override void ResetVizualization() { }
}
