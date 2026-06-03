using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

public class AluVizualiser : BaseVizualizer
{
    [FormerlySerializedAs("_operationBanner")]
    [Header("Operations Renderers (Optional 3D Text/Objects)")]
    [SerializeField] protected GameObject operationBanner;
    [FormerlySerializedAs("_symbolForOperation")] [SerializeField] protected TMP_Text symbolForOperation;

    [FormerlySerializedAs("_uiController")] public AluControlPanel uiController;

    protected int Operation;
    public int CurrentAluOperation => Operation;


    protected override void Awake()
    {
        base.Awake();

        if (uiController != null)
        {
            uiController.Setup("ALU");
        }

        uiController.FirstOperationButton.onClick.AddListener(() => ChooseAluOperation(0));
        uiController.SecondOperationButton.onClick.AddListener(() => ChooseAluOperation(1));
        uiController.ThirdOperationButton.onClick.AddListener(() => ChooseAluOperation(2));
        uiController.FourthOperationButton.onClick.AddListener(() => ChooseAluOperation(3));

        ResetVizualization();
    }
    protected override void InitializePanelController()
    {
        // Controller initialization specific to this class
        uiController = panelInstance.GetComponent<AluControlPanel>();
        if (uiController == null)
        {
            Debug.LogError($"ALUControlPanel component not found on the prefab for {gameObject.name}!");
        }
    }

    public override void ResetVizualization(){
        if (operationBanner.activeSelf) {
            operationBanner.SetActive(false);
        }
    }

    public void ChooseAluOperation(int operation)
    {
        HideData();
        string symbol;
        switch (operation)
        {
            case 0:
                symbol = "+"; // ADD
                break;
            case 1:
                symbol = "-"; // SUBTRACT
                break;
            case 2:
                symbol = "&"; // MULTIPLY (logic AND)
                break;
            case 3:
                symbol = "|"; // DIVIDE (logic OR)
                break;
            default:
                Debug.LogWarning($"ALU operation is not valid and is equal {operation}. Displaying '?'");
                symbol = "?";
                break;
        }

        symbolForOperation.text = symbol;
        Operation = operation;

        if (!operationBanner.activeSelf) {
            operationBanner.SetActive(true);
        }
    }
}
