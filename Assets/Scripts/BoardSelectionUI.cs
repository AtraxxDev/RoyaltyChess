using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Este script se encarga de cambiar los botones de la UI depende si lo tiene en la base de datos

public class BoardSelectionUI : MonoBehaviour
{
    [System.Serializable]
    public class BoardButtonEntry
    {
        public BoardType boardType;
        public Button button;
    }

    [SerializeField] private List<BoardButtonEntry> boardButtons;

    private HashSet<BoardType> unlockedBoards = new HashSet<BoardType>();

    public delegate void OnBoardSelected(BoardType boardType);
    public event OnBoardSelected BoardSelected;

    private void Start()
    {
        foreach (var entry in boardButtons)
        {
            entry.button.onClick.AddListener(() =>
            {
                Debug.Log($"Botón de tablero {entry.boardType} presionado");
                if (BoardSelected != null)
                    BoardSelected.Invoke(entry.boardType);
            });
        }
    }


    public void SetUnlockedBoards(List<BoardType> boards)
    {
        unlockedBoards = new HashSet<BoardType>(boards);
        UpdateButtonStates();
    }

    private void UpdateButtonStates()
    {
        Debug.Log("Actualizando estado de botones de tableros...");

        foreach (var entry in boardButtons)
        {
            bool interactable = entry.boardType == BoardType.Default || unlockedBoards.Contains(entry.boardType);
            entry.button.interactable = interactable;
            Debug.Log($"Botón '{entry.boardType}' interactuable: {interactable}");
        }
    }

}
