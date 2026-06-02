using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public enumCor turnoCor;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        GameOM.OnMovePeca += TurnChange;
    }

    private void OnDisable()
    {
        GameOM.OnMovePeca -= TurnChange;
    }

    public void Start()
    {
        turnoCor = enumCor.branco;
    }

    private void TurnChange()
    {
        if (turnoCor == enumCor.branco)
        {
            turnoCor = enumCor.preto;
        }
        else if (turnoCor == enumCor.preto)
        {
            turnoCor = enumCor.branco;
        }
    }
}
