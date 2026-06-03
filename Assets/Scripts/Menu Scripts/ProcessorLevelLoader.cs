using UnityEngine;

public class ProcessorLevelLoader : MonoBehaviour
{
    [SerializeField] private ProcessorInitialState levelData;
    [SerializeField] private MainMenu menuController;

    public void OpenProcessorLevel(int idx)
    {
        FullProcessorRegiseur.Initial = levelData;

        menuController.OpenLevel(idx);
    }
}
