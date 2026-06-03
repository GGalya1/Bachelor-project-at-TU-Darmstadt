using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class RegisterFileVizualizer : BaseVizualizer
{

    private RegisterFieldPanelUI _uiController;
    public RegisterFieldPanelUI UIRegisterPanel => _uiController;

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

        // Set the initial/default data on the UI panel
        if (_uiController != null)
        {
            _uiController.Display(new int[16]);
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
    public void ForceUpdateWriteEnableVisualization(bool flag)
    {
        if (writeEnableIndicator != null)
        {
            isWriteEnabled = flag;
            writeEnableIndicator.SetActive(!isWriteEnabled);
        }
    }

    protected override void InitializePanelController()
    {
        _uiController = panelInstance.GetComponent<RegisterFieldPanelUI>();
        if (_uiController == null)
        {
            Debug.LogError($"InfoPanelUI component not found on the prefab for {gameObject.name}!");
        }
    }

    public override void ResetVizualization()
    {
        throw new System.NotImplementedException();
    }

    public void TriggerBlink()
    {
        blinker.Trigger();
    }
}
