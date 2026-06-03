using System.Collections.Generic;

public class DataInstMemory
{
    #region INPUTS
    // WE
    public bool MemoryWrite { get; set; }

    // WD
    public int WriteData { get; set; }

    // A
    public int Adress { get; set; }
    #endregion

    #region OUTPUTS
    // RD
    public int ReadData { get; private set; }
    #endregion

    // all information is stored as a pair (adress - object). For Objects stays instructions and data
    public Dictionary<int, int> Memory = new Dictionary<int, int>();

    public void LoadWord(int adress, int data) { 
        Memory[adress] = data;
    }

    #region Sequential logic
    public void PreClockUpdate() {
        if (Memory.ContainsKey(Adress)) {
            ReadData = Memory[Adress];
        }
        else {
            ReadData = 0;
        }
    }

    public void Clock() {
        if (MemoryWrite) {
            Memory[Adress] = WriteData;
        }
    }
    #endregion
}
