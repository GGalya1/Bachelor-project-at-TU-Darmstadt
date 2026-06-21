using UnityEngine;

/// <summary>
/// Holds the initial configuration for a single Eintaktprozessor (single-cycle) sub-level.
/// Unlike ProcessorInitialState (Mehrtaktprozessor), this explicitly separates
/// Instruction Memory and Data Memory.
/// </summary>
[CreateAssetMenu(fileName = "OneTickProcessorInitialState",
                 menuName = "Scriptable Objects/OneTickProcessorInitialState")]
public class OneTickProcessorInitialState : ScriptableObject
{
    [Header("Level Target Description")]
    public string levelTarget;

    [Header("Program Counter")]
    public int pcRegisterInitialValue;

    // Instruction Memory
    [Header("Instruction Memory  [addresses 0 / 4 / 8 / 12]")]
    public int firstInstructionWord;
    public int secondInstructionWord;
    public int thirdInstructionWord;
    public int fourthInstructionWord;

    // Data Memory
    [Header("Data Memory  [addresses 0 / 4 / 8 / 12]")]
    public int firstDataWord;
    public int secondDataWord;
    public int thirdDataWord;
    public int fourthDataWord;

    // Win condition
    [Header("Answer Fields")]
    public ExerciseTyp aufgabeTyp = ExerciseTyp.REGISTER_FIELD;

    [Tooltip("Register index to check (used for REGISTER_FIELD and JAL)")]
    public int registerFieldAddressAnswer;
    [Tooltip("Expected register value")]
    public int registerFieldValueAnswer;

    [Tooltip("Data-memory address to check (used for MEMORY)")]
    public int memoryAddressAnswer;
    [Tooltip("Expected data-memory value")]
    public int memoryValueAnswer;

    [Tooltip("Expected PC value (used for BEQ and JAL)")]
    public int pcValueAnswer;

    // Dialogue
    [Header("Level Dialogues")]
    public DialogueGraph customDialogueGraph;


    [Header("Next Sub-Level  (leave empty to return to Main Menu)")]
    public OneTickProcessorInitialState nextSceneInitial;
}