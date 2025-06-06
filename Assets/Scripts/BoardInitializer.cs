using System;
using System.Collections.Generic;
using UnityEngine;


// este script se encarga de inicializar el tablero dependiendo lo que tenga el usuario
public class BoardInitializer : MonoBehaviour
{
    [SerializeField] private BoardApplier boardApplier;
    [SerializeField] private List<BoardData> availableBoards; // esta es la lista de SO de los tableros
    [SerializeField] private BoardSelectionUI selectionUI;

    private List<BoardType> unlockedBoards = new List<BoardType>();

    private void OnEnable()
    {
        if (selectionUI != null)
            selectionUI.BoardSelected += ApplyBoard;
    }

    private void OnDisable()
    {
        if (selectionUI != null)
            selectionUI.BoardSelected -= ApplyBoard;
    }


    public void InitializeWithProducts(ProductResponse[] products)
    {
        unlockedBoards.Clear();

        Debug.Log("Inicializando tableros desbloqueados para el usuario...");

        foreach (var prod in products)
        {
            if (prod.game == "RoyaltyChess" && prod.product.Contains("Tablero"))
            {
                var normalized = NormalizeProductName(prod.product);

                if (Enum.TryParse<BoardType>(normalized, out var type))
                {
                    unlockedBoards.Add(type);
                    Debug.Log($"Tablero desbloqueado: {type} (producto: {prod.product})");
                }
                else
                {
                    Debug.LogWarning($"No se pudo convertir producto '{prod.product}' a BoardType");
                }
            }
        }

        // Siempre desbloqueado
        if (!unlockedBoards.Contains(BoardType.Default))
        {
            unlockedBoards.Add(BoardType.Default);
            Debug.Log("Tablero Default añadido a desbloqueados");
        }

        selectionUI.SetUnlockedBoards(unlockedBoards);

        // Aplicar tablero Default por defecto
        ApplyBoard(BoardType.Default);
    }

    public void ApplyBoard(BoardType type)
    {
        var data = availableBoards.Find(b => b.boardType == type);
        if (data != null)
        {
            Debug.Log($"Aplicando tablero: {type}");
            boardApplier.ApplyBoard(data);
        }
        else
        {
            Debug.LogWarning($"No se encontró BoardData para: {type}");
        }
    }

    private string NormalizeProductName(string dbProductName)
    {
        return dbProductName
            .Replace(" ", "")
            .Replace("á", "a")
            .Replace("é", "e")
            .Replace("í", "i")
            .Replace("ó", "o")
            .Replace("ú", "u")
            .Replace("ñ", "n");
    }
}
