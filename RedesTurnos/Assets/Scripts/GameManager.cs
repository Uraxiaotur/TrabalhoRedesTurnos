using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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
}
