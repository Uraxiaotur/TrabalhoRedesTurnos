using Unity.VisualScripting;
using UnityEngine;

public class Dama : Peca
{
    [SerializeField] private enumCor cor;
    [SerializeField] private bool virouDama;
    [SerializeField] private GameObject detector;

    public Dama(int x, int y, enumCor cor) : base(x, y, cor) {}

    public void Update()
    {
        
    }
    
    public void SelecionarCasa(enumCor cor)
    {
        if (Input.GetMouseButtonDown(0))
        {
            
        }
    }

    public void OnTurn(enumCor cor)
    {
        
    }
    
}
