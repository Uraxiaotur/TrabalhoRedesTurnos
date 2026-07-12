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
    [SerializeField] private bool isDama;
    
    [SerializeField] private Sprite[] sprites;

    // Update is called once per frame
    void Start()
    {
        pecaLugar = GetComponent<TilePosition>();
        tabuleiro = GameObject.Find("Tabuleiro");
        _table = tabuleiro.GetComponent<Tabuleiro>();
    }

    private void OnEnable()
    {
        GameOM.OnMovePeca += Promotion;
    }
    
    private void OnDisable()
    {
        GameOM.OnMovePeca -= Promotion;
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
        if (time == enumCor.branco && !isDama)
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
        else if (time == enumCor.branco && isDama)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    GameObject tile = _table.espacos[i, j];
                    TilePosition tilePosition = tile.GetComponent<TilePosition>();
                    Button tileButton = tile.gameObject.GetComponent<Button>();
                    if (Math.Abs(tilePosition.x - pecaLugar.x) == 1 && Math.Abs(tilePosition.y - pecaLugar.y) == 1)
                    {
                        tileButton.interactable = true;
                    }
                }
            }
        }
        
        if (time == enumCor.preto && !isDama)
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
        else if (time == enumCor.preto && isDama)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    GameObject tile = _table.espacos[i, j];
                    TilePosition tilePosition = tile.GetComponent<TilePosition>();
                    Button tileButton = tile.gameObject.GetComponent<Button>();
                    if (Math.Abs(tilePosition.x - pecaLugar.x) == 1 && Math.Abs(tilePosition.y - pecaLugar.y) == 1)
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

    public void GetNewPosition(int X, int Y, GameObject newTile, enumCor timeTile)
    {
        // guarda posição anterior para envio
        int fromX = pecaLugar.x;
        int fromY = pecaLugar.y;

        pecaLugar.x = X;
        pecaLugar.y = Y;
        gameObject.transform.position = newTile.transform.position;
        isSelected = false;

        // enviar mensagem serializada de movimento: MOVE;fromX;fromY;toX;toY;cor
        string moveMsg = $"MOVE;{fromX};{fromY};{X};{Y};{time}";
        GameOM.SendCheckerPositionMessage(moveMsg);

        GameOM.MovePeca();
        GameManager.Instance.selectedChecker = null;
    }

    private void ComerPeca()
    {
        if (GameManager.Instance.selectedChecker == null)
        {
            return;
        }
        
        GameObject selectedChecker = GameManager.Instance.selectedChecker; //Peça que foi escolhida no gameManager
        Dama currentChecker = GameManager.Instance.selectedChecker.GetComponent<Dama>();
        TilePosition checkerPosition = GameManager.Instance.selectedChecker.GetComponent<TilePosition>();
        // lógica de captura: a peça selecionada (selectedChecker) deve pular sobre esta peça (this)
        // e pousar na casa imediatamente além dela na mesma direção.
        if (currentChecker != null)
        {
            // só pode capturar peça de cor/oponente
            if (currentChecker.time == time)
                return;

            int dx = pecaLugar.x - checkerPosition.x;
            int dy = pecaLugar.y - checkerPosition.y;

            // captura válida apenas se a peça a capturar está a uma casa em diagonal (1,1)
            if (Math.Abs(dx) == 1 && Math.Abs(dy) == 1)
            {
                int landingX = pecaLugar.x + dx; // casa além da peça capturada
                int landingY = pecaLugar.y + dy;

                // verificar limites do tabuleiro
                if (landingX < 0 || landingX > 7 || landingY < 0 || landingY > 7)
                {
                    Debug.Log("Movimento inválido: casa de pouso fora do tabuleiro.");
                    return;
                }

                // verificar se existe alguma peça ocupando a casa de pouso
                bool occupied = false;
                Dama[] todasPecas = FindObjectsOfType<Dama>();
                foreach (Dama p in todasPecas)
                {
                    TilePosition tp = p.GetComponent<TilePosition>();
                    if (tp != null && tp.x == landingX && tp.y == landingY)
                    {
                        occupied = true;
                        break;
                    }
                }

                if (occupied)
                {
                    // se houver alguma peça na casa de pouso, movimento inválido
                    Debug.Log("Movimento inválido: casa de pouso ocupada.");
                    return;
                }

                // movimento válido: mover selectedChecker para a casa de pouso e destruir a peça capturada
                // guardar origem da peça que captura
                int fromX = checkerPosition.x;
                int fromY = checkerPosition.y;

                checkerPosition.x = landingX;
                checkerPosition.y = landingY;
                // Note: espacos is stored as [row(y), column(x)] in Tabuleiro, so index as [landingY, landingX]
                GameObject landingTile = _table.espacos[landingY, landingX];
                Vector3 landingPos = landingTile.transform.position;
                // manter a peça à frente dos tiles (z negativo como ao instanciar)
                landingPos.z = -1f;
                selectedChecker.transform.position = landingPos;
                
                UnsellectAllTiles();
                // enviar mensagem de captura: CAPTURE;fromX;fromY;capturedX;capturedY;toX;toY;corMover
                int capturedX = pecaLugar.x;
                int capturedY = pecaLugar.y;
                string captureMsg = $"CAPTURE;{fromX};{fromY};{capturedX};{capturedY};{landingX};{landingY};{currentChecker.time}";
                GameOM.SendCheckerPositionMessage(captureMsg);

                GameOM.MovePeca();
                Destroy(gameObject);
                GameManager.Instance.selectedChecker = null;
                GameOM.CheckerCollected(time);
            }
        }
    }

    private void Promotion()
    {
        if(time == enumCor.branco && pecaLugar.y == 7)
        {
            isDama = true;
            Image sprite = gameObject.GetComponent<Image>();
            sprite.sprite = sprites[1];
        }

        if (time == enumCor.preto && pecaLugar.y == 0)
        {
            isDama = true;
            Image sprite = gameObject.GetComponent<Image>();
            sprite.sprite = sprites[1];
        }
    }
}
