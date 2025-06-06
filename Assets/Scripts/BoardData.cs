using UnityEngine;

[CreateAssetMenu(fileName = "BoardData", menuName = "RoyaltyChess/BoardData", order = 0)]
public class BoardData : ScriptableObject
{
    public BoardType boardType;
    public Sprite boardSprite;
    public string displayName; // ejemplo: "Tablero Glacial"

}
