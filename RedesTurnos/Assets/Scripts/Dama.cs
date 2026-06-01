using UnityEngine;

public class Dama : MonoBehaviour
{
    [SerializeField] private enumCor cor;
    [SerializeField] private bool virouDama;
    [SerializeField] private GameObject detector;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelecionarCasa()
    {
        if (Input.GetMouseButtonDown(0))
        {
            BoxCollider2D detectorCollider = GetComponentInChildren<BoxCollider2D>();
            
        }
    }
}
