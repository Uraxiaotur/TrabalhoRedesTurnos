using Unity.VisualScripting;
using UnityEngine;

public class Dama : MonoBehaviour
{
    [SerializeField] private enumCor cor;
    [SerializeField] private bool virouDama;
    [SerializeField] private GameObject detector;
    [SerializeField] private bool onTurn;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        OnTurn();
    }

    private void OnTurn()
    {
        if (GameManager.Instance.turnoCor == cor)
        {
            onTurn = true;
        }
        else
        {
            onTurn = false;
        }
    }
}
