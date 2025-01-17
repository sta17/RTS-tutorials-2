using UnityEngine;

public enum PlayerTypes
{
    Human,
    AI,
    NPC
}

[System.Serializable]
public class FactionManager : MonoBehaviour
{
    [Header("Player Stats")]
    public string      playerName;
    public Color       playerColor;
    public int         playerID;
    public PlayerTypes playerType;

    public void RaiseDeathChangeNotification(UnitController unit)
    {
        // For later
    }
}
