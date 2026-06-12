using UnityEngine;
using System;

public class GameOM : MonoBehaviour
{
    public static event Action<int> OnCollectPeca;

    public static void CollectPeca(int indexPeca)
    {
        OnCollectPeca?.Invoke(indexPeca);
    }

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
}
