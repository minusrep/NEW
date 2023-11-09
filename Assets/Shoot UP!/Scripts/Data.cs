[System.Serializable]
public class Data
{
    public int selectedWeaponID;
    public bool[] weaponStates;
    public int moneyAmount;
    public float maxHeight;
    public bool muteState;
    public bool isRated;

    public Data()
    {
        this.weaponStates = new bool[10];
        this.weaponStates[0] = true;
        this.selectedWeaponID = 0;
        this.moneyAmount = 100;
        this.maxHeight = 0f;
        this.muteState = false;
        this.isRated = false;
    }
}