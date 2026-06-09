using UnityEngine;

public class Dama : MonoBehaviour
{
    public enumCor time;

    // Update is called once per frame
    void Start()
    {
    }

    public void Selected()
    {
        VerifyTurn();

        if (!VerifyTurn())
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
            Debug.Log("Vez do outro time");
            return false;
        }
    }

    private void ChooseTile()
    {
        
    }
}
