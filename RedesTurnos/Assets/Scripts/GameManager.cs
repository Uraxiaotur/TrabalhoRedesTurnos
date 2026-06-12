using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum enumCor
{
    branco,
    preto,
    vazio
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public enumCor turnoCor;
    public GameObject selectedChecker;
    public int whiteCheckersCollected = 0;
    public int blackCheckersCollected = 0;
    
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
        GameOM.OnCheckerCollected += CheckersCollected;
        GameOM.OnMovePeca += TurnChange;
    }

    private void OnDisable()
    {
        GameOM.OnCheckerCollected -= CheckersCollected;
        GameOM.OnMovePeca -= TurnChange;
    }

    private void Start()
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

    public void SelectedChecker(GameObject checker)
    {
        selectedChecker = checker;
    }

    private void CheckersCollected(enumCor cor)
    {
        if (cor == enumCor.branco)
        {
            whiteCheckersCollected++;
            GameOM.UIChange();
        }
        else if (cor == enumCor.preto)
        {
            blackCheckersCollected++;
            GameOM.UIChange();
        }

        if (whiteCheckersCollected == 12)
        {
            GameOM.TeamVictory(enumCor.preto);
            Debug.Log("Time Preto Venceu");
        }

        if (blackCheckersCollected == 12)
        {
            GameOM.TeamVictory(enumCor.branco);
            Debug.Log("Time Branco Venceu");
        }
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        whiteCheckersCollected = 0;
        blackCheckersCollected = 0;
        turnoCor = enumCor.branco;
    }
}
