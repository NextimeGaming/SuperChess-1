using System.Collections.Generic;
using UnityEngine;

public class PieceManager : MonoBehaviour
{
    // Dicion�rio para armazenar todas as pe�as
    private Dictionary<string, GameObject> pieces = new Dictionary<string, GameObject>();

    // Gerenciadores de pe�as 
    public WhitePawnManager whitePawnManager;
    public whiteArcherManager whiteArcherManager;
    public whiteMageManager whiteMageManager;

    // Gerenciador de turno
    public TurnManager turnManager;

    private void Start()
    {
        // Inicializa o dicion�rio de pe�as
        InitializePieces();
    }

    // Inicializa o dicion�rio de pe�as
    private void InitializePieces()
    {
        // Adiciona as pe�as brancas
        pieces.Add("WhitePawn", whitePawnManager.gameObject);
        pieces.Add("WhiteArcher", whiteArcherManager.gameObject);
        pieces.Add("WhiteMage", whiteMageManager.gameObject);

        // Adiciona as pe�as pretas (faltando)
        // pieces.Add("BlackPawn", blackPawnManager.gameObject);
        // pieces.Add("BlackArcher", blackArcherManager.gameObject);
    }

    // Notifica as pe�as sobre o turno atual
    public void NotifyPieceManager(TurnManager.Turn turn)
    {
        // Notifique as pe�as sobre o turno atual aqui
        Debug.Log($"Turno atual: {turn}");

        // Atualize as pe�as conforme necess�rio (faltando)
    }

    // Retorna uma pe�a espec�fica
    public GameObject GetPiece(string pieceName)
    {
        return pieces[pieceName];
    }

    // Verifica se uma pe�a pode se mover para uma posi��o
    public bool CanMovePiece(string pieceName, Vector3 targetPosition)
    {
        // Verifica se a pe�a existe
        if (!pieces.ContainsKey(pieceName))
        {
            return false;
        }

        //  verifica��o para o gerenciador 
        switch (pieceName)
        {
            case "WhiteMage":
                return whiteMageManager.CanMove(targetPosition);
            case "WhitePawn":
                return whitePawnManager.CanMove(targetPosition);
            case "WhiteArcher":
                return whiteArcherManager.CanMove(targetPosition);
            default:
                return false;
        }
    }

    // Atualiza o estado de uma pe�a
    public void UpdatePieceState(string pieceName, Vector3 newPosition)
    {
        // Verifica se a pe�a existe
        if (!pieces.ContainsKey(pieceName))
        {
            return;
        }

        // atualiza��o para o gerenciador 
        switch (pieceName)
        {
            case "WhiteMage":
                whiteMageManager.UpdatePosition(newPosition);
                break;

            case "WhitePawn":
                whitePawnManager.UpdatePosition(newPosition);
                break;
            case "WhiteArcher":
                whiteArcherManager.UpdatePosition(newPosition);
                break;
        }
    }
}
