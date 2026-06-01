using UnityEngine;

public class Tabuleiro : MonoBehaviour
{
    private static int n = 8;
    [SerializeField] private GameObject quadrado;
    [SerializeField] private GameObject peca;
    [SerializeField] private GameObject pecaBranca;
    [SerializeField] private GameObject pecaPreta;
    public GameObject[,] espaco = new GameObject[n,n];
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ConstruirTabuleiro();
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
                            if (i < 3)
                            {
                                ColocarPeca(espacoLugar.x, espacoLugar.y, "branca");
                            }
                            else if (i > 4)
                            {
                                ColocarPeca(espacoLugar.x, espacoLugar.y, "preta");
                            }
                        }
                        espaco[i,j] = novoQuadrado;
                        espacoLugar.x += 1.0f;
                    }
                    espacoLugar.y -= 1.0f;
                    espacoLugar.x = quadrado.GetComponent<Transform>().position.x;
                }
    }

    private void ColocarPeca(float posX, float posY, string time)
    {
        Vector3 pecaLugar = peca.GetComponent<Transform>().position;
        pecaLugar.z = -1f;
        pecaLugar.x = posX;
        pecaLugar.y = posY;
        if (time == "branca")
        {
            GameObject novaPeca = Instantiate(pecaBranca, pecaLugar, Quaternion.identity);
        }
        else if (time == "preta")
        {
            GameObject novaPeca = Instantiate(pecaPreta, pecaLugar, Quaternion.identity);
        }
    }

}
