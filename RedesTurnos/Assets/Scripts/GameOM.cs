using UnityEngine;
using System;

public class GameOM : MonoBehaviour
{
    public static event Action OnMovePeca;
    public static void MovePeca()
    {
        OnMovePeca?.Invoke();
    }
    
    public static event Action<enumCor> OnTeamVictory;
    public static void TeamVictory(enumCor cor)
    {
        OnTeamVictory?.Invoke(cor);
    }
    
    public static event Action<enumCor> OnCheckerCollected;
    public static void CheckerCollected(enumCor cor)
    {
        OnCheckerCollected?.Invoke(cor);
    }

    public static event Action OnUIChange;
    public static void UIChange()
    {
        OnUIChange?.Invoke();
    }

    // Legacy event that used TilePosition pairs (kept for compatibility)
    public static event Action<TilePosition, TilePosition> OnSendCheckerPosition;
    public static void SendCheckerPositionMessage(TilePosition peca1, TilePosition peca2)
    {
        OnSendCheckerPosition?.Invoke(peca1, peca2);
    }

    // New: evento para enviar mensagens serializadas via rede
    public static event Action<string> OnSendCheckerPositionMessageString;
    public static void SendCheckerPositionMessage(string msg)
    {
        OnSendCheckerPositionMessageString?.Invoke(msg);
    }
}
