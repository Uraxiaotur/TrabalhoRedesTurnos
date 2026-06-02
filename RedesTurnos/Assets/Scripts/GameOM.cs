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
}
