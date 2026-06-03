using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Serialization;

public class RegisterFieldPanelUI : MonoBehaviour
{
    [FormerlySerializedAs("_weButton")] [SerializeField] private Button weButton;
    public Button WeButton => weButton;

    public TextMeshProUGUI titleText;

    public TextMeshProUGUI[] registerText;

    public void Awake()
    {
        titleText.text = "Register Field";
    }

    public void Display(int[] values) {
        for (var i = 0; i < registerText.Length; i++) { 
            if (i == 15) {
                registerText[i].text = $"r{i} (pc):\n{values[i]}";
            }
            else
                registerText[i].text = $"r{i}: {values[i]}";
        }
    }
}
