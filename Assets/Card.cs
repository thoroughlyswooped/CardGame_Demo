using UnityEngine;
using TMPro;
using Unity.Netcode;
using System;
using UnityEngine.UI;

[System.Serializable]
public class Card : MonoBehaviour
{
    [SerializeField]protected float _effectAmnt;
    [SerializeField]protected float _health;

    [SerializeField] protected TMP_Text _healthText;
    [SerializeField] protected TMP_Text _effectAmntText;
    [SerializeField] protected TMP_Text _cardNameText;
    [SerializeField] protected Image _image;

    private GameController _gameController;


    [SerializeField]
    private CardData _data = null;
    private CardType _cardType;
    private string _cardName;

    [SerializeField]
    private GamePlayer _owningPlayer;

    private void Awake()
    {
        // if no data, hide card
        if (_data == null)
        {
            gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        if(GameController.instance != null)
        {
            _gameController = GameController.instance;
        }

        
    }


    public virtual void Action()
    {
        // Don't do anything if we can't play
        if (!_owningPlayer.canPlay || _owningPlayer.HasPlayed)
        {
            return;
        }

        switch (_cardType)
        {
            case CardType.Dmg:
                _owningPlayer.DealDmg( _effectAmnt);
                break;
            case CardType.Heal:
                break;
            default:
                Debug.LogError($"Unrecognized card type: {_cardType}");
                break;
        }

        _owningPlayer.HasPerformedAction(this);
    }

    public virtual void Discard()
    {
        _owningPlayer.Discard(this);

        //DiscardLogic();

        
        //DiscardServerRpc();
        
    }

    // TODO: create and store actual default values
    internal void Reset()
    {
        _data = null;
        _effectAmnt = 0;
        _cardType = 0;
        _cardName = "";
        _health = 0;
    }

    private void PopulateUI(CardData data)
    {
        _cardNameText.text = data.cardName;
        _healthText.text = data.health.ToString();
        _effectAmntText.text = data.effectAmnt.ToString();

        gameObject.SetActive(true);
    }

    public void PopulateData(CardData data)
    {
        if(data == null)
        {
            Debug.Log("Trying to update card with empty data");
            gameObject.SetActive(false); 
            return;
        }

        PopulateUI(data);

        _data = data;
        _effectAmnt = data.effectAmnt;
        _cardType = data.cardType;
        _cardName = data.cardName;
        _health = data.health;
        _image.sprite = data.cardSprite;
    }

    public CardData GetCardData()
    {
        return _data;
    }

    public void SetOwningPlayer(GamePlayer player)
    {
        _owningPlayer = player;
    }
}
