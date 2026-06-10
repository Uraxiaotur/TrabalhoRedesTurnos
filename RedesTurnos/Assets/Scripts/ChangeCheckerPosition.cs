using UnityEngine;
using UnityEngine.UI;

public class ChangeCheckerPosition : MonoBehaviour
{
    [SerializeField] private TilePosition tilePosition;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tilePosition = GetComponent<TilePosition>();
    }

    public void ChangeSelectedChecker()
    {
        if (GameManager.Instance.selectedChecker != null)
        {
            Dama dama = GameManager.Instance.selectedChecker.GetComponent<Dama>();
            dama.GetNewPosition(tilePosition.x,tilePosition.y, gameObject, enumCor.vazio);
            dama.UnsellectAllTiles();
        }
    }
}
