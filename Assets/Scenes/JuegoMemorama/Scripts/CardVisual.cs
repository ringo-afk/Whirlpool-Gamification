using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardVisual : MonoBehaviour
{
    [Header("Referencias UI")]
    public TextMeshProUGUI cardText;
    public Image cardImage;
    public Sprite backSprite;
    public Sprite frontSprite;

    [HideInInspector] public CardData cardData;
    public bool isFlipped = false;

    private GameManager gameManager;
    private Button buttonComponent;

    public void SetupCard(CardData data, GameManager gm)
    {
        cardData = data;
        gameManager = gm;

        buttonComponent = GetComponent<Button>();

        cardText.text = cardData.cardText;
        cardText.gameObject.SetActive(false);
        cardImage.sprite = backSprite;
        isFlipped = false;

        buttonComponent.onClick.AddListener(OnCardClicked);
    }

    public void OnCardClicked()
    {
        if (isFlipped || !gameManager.canPlayerPlay) return;

        gameManager.CardRevealed(this);
    }

    public void FlipCard()
    {
        isFlipped = true;
        cardImage.sprite = frontSprite;
        cardText.gameObject.SetActive(true);
    }

    public void UnflipCard()
    {
        isFlipped = false;
        cardImage.sprite = backSprite;
        cardText.gameObject.SetActive(false);
    }
}