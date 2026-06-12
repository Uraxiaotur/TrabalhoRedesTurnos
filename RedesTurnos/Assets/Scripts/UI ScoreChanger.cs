using UnityEngine;
using UnityEngine.UI;

public class UIScoreChanger : MonoBehaviour
{
    [SerializeField] private Text textWhite;
    [SerializeField] private Text textBlack;
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private Text victoryText;

    private void Start()
    {
        victoryPanel.SetActive(false);
    }
    
    private void OnEnable()
    {
        GameOM.OnUIChange += ChangeText;
        GameOM.OnTeamVictory += Victory;
    }
    
    private void OnDisable()
    {
        GameOM.OnUIChange -= ChangeText;
        GameOM.OnTeamVictory -= Victory;
    }

    private void ChangeText()
    {
        int whiteCollected = GameManager.Instance.whiteCheckersCollected;
        int blackCollected = GameManager.Instance.blackCheckersCollected;
        textWhite.text = whiteCollected.ToString();
        textBlack.text = blackCollected.ToString();
    }

    private void Victory(enumCor cor)
    {
        if (cor == enumCor.preto)
        {
            victoryPanel.SetActive(true);
            victoryText.text = ("Time Preto Venceu!");
        }
        
        if (cor == enumCor.branco)
        {
            victoryPanel.SetActive(true);
            victoryText.text = ("Time Branco Venceu!");
        }
    }

    public void Reset()
    {
        GameManager.Instance.ResetGame();
    }
}
