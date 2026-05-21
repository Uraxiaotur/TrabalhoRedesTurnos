using UnityEngine;

public class EspaçoVazio : MonoBehaviour
{
    public int linha;
    public int coluna;
    public bool temDama;
    private SpriteRenderer spriteRenderer;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
