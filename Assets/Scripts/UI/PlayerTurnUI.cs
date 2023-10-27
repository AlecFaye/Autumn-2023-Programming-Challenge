using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTurnUI : MonoBehaviour
{
    public static PlayerTurnUI Instance { get; private set; }

    [SerializeField] private Color blackColour;
    [SerializeField] private Color whiteColour;

    [SerializeField] private TextMeshProUGUI turnText;
    [SerializeField] private Image turnImage;

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateTurnUI(PlayerColour playerColour)
    {
        Color backgroundColour = playerColour == PlayerColour.White 
            ? whiteColour
            : blackColour;

        Color textColour = playerColour == PlayerColour.White
            ? blackColour
            : whiteColour;
       
        turnImage.color = backgroundColour;

        turnText.text = playerColour.ToString();
        turnText.color = textColour;
    }
}
