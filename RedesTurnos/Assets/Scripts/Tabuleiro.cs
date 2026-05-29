using UnityEngine;

public class Tabuleiro : MonoBehaviour
{
    private static int n = 8;
    [SerializeField] private GameObject quadrado;
    [SerializeField] private GameObject peca;
    public GameObject[,] espaco = new GameObject[n,n];
    public GameObject[,] pecas = new GameObject[n,n];
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ConstruirTabuleiro();
        ColocarPeca();
    }

    private void ConstruirTabuleiro()
    {
        Vector2 espacoLugar = quadrado.GetComponent<Transform>().position;
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        GameObject novoQuadrado = Instantiate(quadrado, espacoLugar, Quaternion.identity);
                        SpriteRenderer sprite = novoQuadrado.GetComponent<SpriteRenderer>();
                        if (j % 2 == 0 ^ i % 2 == 0)
                        {
                            sprite.color = Color.white;
                        }
                        else
                        {
                            sprite.color = Color.black;
                        }
                        espaco[i,j] = novoQuadrado;
                        espacoLugar.x += 1.0f;
                    }
                    espacoLugar.y -= 1.0f;
                    espacoLugar.x = quadrado.GetComponent<Transform>().position.x;
                }
    }

    private void ColocarPeca()
    {
        Vector2 pecaLugar = peca.GetComponent<Transform>().position;
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                if (j % 2 == 0 ^ i % 2 == 0)
                {
                    GameObject novaPeca = Instantiate(peca, pecaLugar, Quaternion.identity);
                    SpriteRenderer sprite = novaPeca.GetComponent<SpriteRenderer>();
                    espaco[i,j] = novaPeca;
                    pecaLugar.x += 1.0f;
                }
                pecaLugar.y -= 1.0f;
                pecaLugar.x = quadrado.GetComponent<Transform>().position.x;
            }
        }
    }

}
