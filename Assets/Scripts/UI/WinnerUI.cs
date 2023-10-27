using TMPro;
using UnityEngine;

public class WinnerUI : MonoBehaviour
{
    public static WinnerUI Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI winnerText;

    private void Awake()
    {
        Instance = this;

        gameObject.SetActive(false);
    }

    public void UpdateWinnerText(string playerName)
    {
        gameObject.SetActive(true);
        winnerText.text = $"{playerName} Wins!";
    }
}
