using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;

public class Tabuleiro : MonoBehaviour
{
    private static int n = 8;
    [SerializeField] private GameObject tile;
    [SerializeField] private Button peca;
    [SerializeField] private Button pecaBranca;
    [SerializeField] private Button pecaPreta;
    [SerializeField] private GameObject parentTiles;
    [SerializeField] private GameObject parentPecas;
    public GameObject[] pecasBrancas;
    public GameObject[] pecasPretas;
    public GameObject[,] espacos;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ConstruirTabuleiro();
    }

    private void ConstruirTabuleiro()
    {
        espacos = new GameObject[n, n];
        Vector2 espacoLugar = tile.GetComponent<RectTransform>().position;
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        GameObject novoQuadrado = Instantiate(tile, espacoLugar, Quaternion.identity);
                        novoQuadrado.GetComponent<TilePosition>().x = j;
                        novoQuadrado.GetComponent<TilePosition>().y = i;
                        novoQuadrado.transform.SetParent(parentTiles.transform);
                        Image sprite = novoQuadrado.GetComponent<Image>();
                        if (j % 2 == 0 ^ i % 2 == 0)
                        {
                            sprite.color = Color.navajoWhite;
                        }
                        else
                        {
                            sprite.color = Color.seaGreen;
                            if (i < 3)
                            {
                                ColocarPeca(espacoLugar.x, espacoLugar.y, "branca", j, i);
                            }
                            else if (i > 4)
                            {
                                ColocarPeca(espacoLugar.x, espacoLugar.y, "preta", j, i);
                            }
                        }
                        espacoLugar.x += 40f;
                        espacos[i, j] = novoQuadrado;
                    }
                    
                    espacoLugar.y -= 40f;
                    espacoLugar.x = tile.GetComponent<Transform>().position.x;
                }
    }

    private void ColocarPeca(float posX, float posY, string time, int X, int Y)
    {
        Vector3 pecaLugar = peca.GetComponent<Transform>().position;
        pecaLugar.z = -1f;
        pecaLugar.x = posX;
        pecaLugar.y = posY;
        if (time == "branca")
        {
            Button novaPeca = Instantiate(pecaBranca, pecaLugar, Quaternion.identity);
            novaPeca.GetComponent<TilePosition>().x = X;
            novaPeca.GetComponent<TilePosition>().y = Y;
            Image image = novaPeca.GetComponent<Image>();
            image.color = Color.white;
            novaPeca.transform.SetParent(parentPecas.transform);
            pecasBrancas[X] = novaPeca.gameObject;
        }
        else if (time == "preta")
        {
            Button novaPeca = Instantiate(pecaPreta, pecaLugar, Quaternion.identity);
            novaPeca.GetComponent<TilePosition>().x = X;
            novaPeca.GetComponent<TilePosition>().y = Y;
            Image image = novaPeca.GetComponent<Image>();
            image.color = Color.black;
            novaPeca.transform.SetParent(parentPecas.transform);
            pecasPretas[X] = novaPeca.gameObject;
        }
    }

    private void ShowTilesArray()
    {
        foreach (GameObject espacos in espacos)
        {
            espacos.GetComponent<TilePosition>().x.ToString();
            espacos.GetComponent<TilePosition>().y.ToString();
            
            Debug.Log("X: " + espacos.GetComponent<TilePosition>().x + " Y: " + espacos.GetComponent<TilePosition>().y);
        }
    }
}
