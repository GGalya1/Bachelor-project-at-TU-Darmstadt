using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Serialization;

public class InstrMemoryControlPanel: InfoPanelUI
{
    [FormerlySerializedAs("_weButton")] [SerializeField] private Button weButton;
    public Button WeButton => weButton;

    public TextMeshProUGUI firstAddresValue;
    public TextMeshProUGUI secondAddresValue;
    public TextMeshProUGUI thirdAddresValue;
    public TextMeshProUGUI fourthAddresValue;

    public void Display(string firstVal, string secondVal, string thirdVal, string fourthVal)
    {
        firstAddresValue.text = firstVal;
        secondAddresValue.text = secondVal;
        thirdAddresValue.text = thirdVal;
        fourthAddresValue.text = fourthVal;
    }
}
