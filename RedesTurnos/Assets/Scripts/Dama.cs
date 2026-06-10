using System;
using UnityEngine;
using UnityEngine.UI;

public class Dama : MonoBehaviour
{
    public enumCor time;
    public TilePosition pecaLugar;
    public GameObject tabuleiro;
    private Tabuleiro _table;
    [SerializeField] private bool isSelected;

    // Update is called once per frame
    void Start()
    {
        pecaLugar = GetComponent<TilePosition>();
        tabuleiro = GameObject.Find("Tabuleiro");
        _table = tabuleiro.GetComponent<Tabuleiro>();
    }
    
    public void Selected()
    {
        if (GameManager.Instance.selectedChecker != null)
        {
            Dama previousDama = GameManager.Instance.selectedChecker.GetComponent<Dama>();
            previousDama.isSelected = false;
        }
        
        if (!isSelected)
        {
            VerifyTurn();
            if (!VerifyTurn())
            {
                ComerPeca();
                return;
            }
            
            UnsellectAllTiles();
            GameManager.Instance.SelectedChecker(gameObject);
            VerifyNearTiles();
            isSelected = true;
        }
        else
        {
        }
    }

    public bool VerifyTurn()
    {
        if (GameManager.Instance.turnoCor == time)
        {
            Debug.Log("Peça selecionada");
            return true;
        }
        else
        {
            return false;
        }
    }

    private void VerifyNearTiles()
    {
        if (time == enumCor.branco)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    GameObject tile = _table.espacos[i, j];
                    TilePosition tilePosition = tile.GetComponent<TilePosition>();
                    Button tileButton = tile.gameObject.GetComponent<Button>();
                    if (Math.Abs(tilePosition.x - pecaLugar.x) == 1 && tilePosition.y == pecaLugar.y + 1)
                    {
                        tileButton.interactable = true;
                    }
                }
            }   
        }
        if (time == enumCor.preto)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    GameObject tile = _table.espacos[i, j];
                    TilePosition tilePosition = tile.GetComponent<TilePosition>();
                    Button tileButton = tile.gameObject.GetComponent<Button>();
                    if (Math.Abs(tilePosition.x - pecaLugar.x) == 1 && tilePosition.y == pecaLugar.y - 1)
                    {
                        tileButton.interactable = true;
                    }
                }
            }   
        }
    }

    public void UnsellectAllTiles()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                GameObject tile = _table.espacos[i, j];
                Button tileButton = tile.gameObject.GetComponent<Button>();
                tileButton.interactable = false;
            }
        }
    }

    public void GetNewPosition(int X, int Y, GameObject newTile, enumCor time)
    {
        pecaLugar.x = X;
        pecaLugar.y = Y;
        gameObject.transform.position = newTile.transform.position;
        isSelected = false;
        GameOM.MovePeca();
        GameManager.Instance.selectedChecker = null;
    }

    private void ComerPeca()
    {
        if (GameManager.Instance.selectedChecker == null)
        {
            return;
        }
        
        GameObject selectedChecker = GameManager.Instance.selectedChecker;
        Dama currentChecker = GameManager.Instance.selectedChecker.GetComponent<Dama>();
        TilePosition checkerPosition = GameManager.Instance.selectedChecker.GetComponent<TilePosition>();
        if (currentChecker != null && (Math.Abs(checkerPosition.x - pecaLugar.x) == 1 && Math.Abs(checkerPosition.y - pecaLugar.y) == 1))
        {
            if (currentChecker.time != time)
            {
                checkerPosition.x = pecaLugar.x;
                checkerPosition.y = pecaLugar.y;
                selectedChecker.transform.position = gameObject.transform.position;
                Destroy(gameObject);
                UnsellectAllTiles();
                GameOM.MovePeca();
            }
            else
            {
                Debug.Log("Não é possível comer uma peça do mesmo time");
            }
        }
    }
}
