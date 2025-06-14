using UnityEngine;

public class BoardApplier : MonoBehaviour, IBoardApplier
{
    [SerializeField] private SpriteRenderer boardRenderer;

    public void ApplyBoard(BoardData boardData)
    {
        if (boardData == null)
        {
            Debug.LogWarning("No se proporcionˇ BoardData.");
            return;
        }

        boardRenderer.sprite = boardData.boardSprite;
        Debug.Log($"Tablero aplicado: {boardData.boardType}");
    }
}
