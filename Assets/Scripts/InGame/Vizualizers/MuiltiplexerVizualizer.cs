using UnityEngine;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.InputSystem.XR;
using UnityEngine.Serialization;

public class MuiltiplexerVizualizer: BaseVizualizer
{
    [FormerlySerializedAs("_inputBuses")]
    [Header("Bus Line Renderers")]
    [SerializeField] private LineRenderer[] inputBuses;

    [FormerlySerializedAs("_outputBus")] [SerializeField] private LineRenderer outputBus;
    [FormerlySerializedAs("_controlBus")] [SerializeField] private LineRenderer controlBus;

    [FormerlySerializedAs("_bitRenderers")]
    [Header("Visual Bits Renderers")]
    [SerializeField] private Renderer[] bitRenderers;

    [FormerlySerializedAs("_disabledColor")]
    [Header("Colors")]
    [SerializeField] private Color disabledColor = Color.gray;
    [FormerlySerializedAs("_activeColor")] [SerializeField] private Color activeColor = Color.red;

    private MultiplexerControlPanel _uiController;
    public MultiplexerControlPanel UIController => _uiController;

    private int _currentChoosenPath = -1;
    public int CurrentChoosenMuxPath => _currentChoosenPath;

    private MaterialPropertyBlock _propBlock;
    private static readonly int ColorPropertyID = Shader.PropertyToID("_BaseColor");


    protected override void Awake()
    {
        _propBlock = new MaterialPropertyBlock();
        base.Awake();

 
        if (_uiController != null)
        {
            if (bitRenderers.Length > 2) 
            {
                _uiController.Setup(true, true, true, "Multiplexer 3");
            }
            else
            {
                _uiController.Setup(true, true, false, "Multiplexer 2");
            }
        }

        _uiController.FirstWayButton.onClick.AddListener(() => SelectPath(0));
        _uiController.SecondWayButton.onClick.AddListener(() => SelectPath(1));
        _uiController.ThirdWayButton.onClick.AddListener(() => SelectPath(2));

        ResetVizualization();
    }
    protected override void InitializePanelController()
    {
        // Controller initialization specific to this class
        _uiController = panelInstance.GetComponent<MultiplexerControlPanel>();
        if (_uiController == null)
        {
            Debug.LogError($"MultiplexerControlPanel component not found on the prefab for {gameObject.name}!");
        }
    }

    public override void ResetVizualization() {
        _currentChoosenPath = -1;
        UpdateVisuals(-1);
    }
    public void SelectPath(int index)
    {
        if (index < 0 || index >= inputBuses.Length) return;

        // Debug.Log($"Path {index + 1} chosen");
        _currentChoosenPath = index;
        UpdateVisuals(index);
        HideData();
    }

    #region helpers
    private void UpdateVisuals(int activeIndex)
    {
        for (var i = 0; i < inputBuses.Length; i++)
        {
            var isActive = (i == activeIndex);
            SetColor(inputBuses[i], isActive ? activeColor : disabledColor);

            if (i < bitRenderers.Length)
                SetColor(bitRenderers[i], isActive ? activeColor : disabledColor);
        }

        SetColor(outputBus, activeIndex != -1 ? activeColor : disabledColor);
    }

    private void SetColor(Renderer renderer, Color color)
    {
        if (renderer == null) return;

        renderer.GetPropertyBlock(_propBlock);
        _propBlock.SetColor(ColorPropertyID, color);
        renderer.SetPropertyBlock(_propBlock);
    }
    #endregion
}
