using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoostersShopItem : MonoBehaviour
{

    [System.Serializable]
    enum BoosterType
    {
        bomb = 0,
        lightning = 1,
        swap = 2,
        doublePoints = 3,
        extraMove = 4,
        freeze = 5
    }

    [Header("Item Settings")]
    [SerializeField] private BoosterType boostType;

    [Header("Price Settings")]
    [SerializeField] private int coinPrice;

    [Header("To Item Image")]
    public Sprite itemIconSprite;

    [Space]
    [Header("Components")]
    [SerializeField] private GameObject itemIcon;
    [SerializeField] private Button shopBtn;
    [SerializeField] private TMPro.TMP_Text coinTxt;
    [SerializeField] private ShopController shopController;
    [SerializeField] private ViewController viewController;

    private GameSettings gameSettings;


    void Start()
    {
        gameSettings = GameSettings.instance;

        itemIcon.GetComponent<Image>().sprite = itemIconSprite;
        coinTxt.text = $"{coinPrice}";
        shopBtn.onClick.AddListener(ShopItemBtnClick);

        UpdateItemView();
    }

    private void FixedUpdate()
    {
        UpdateItemView();
    }

    //���������� ��� ������
    private void UpdateItemView()
    {
        //��������� ������� �� �����
        if (gameSettings.Coins < coinPrice)
        {
            shopBtn.interactable = false;
        }
        else
        {
            shopBtn.interactable = true;
        }

    }

    //�������� ��������� �����
    private void BuySelectedBonus()
    {
        switch (boostType)
        {
            case BoosterType.bomb:
                gameSettings.BombCount++;
                break;
            case BoosterType.lightning:
                gameSettings.LightningCount++;
                break;
            case BoosterType.swap:
                gameSettings.SwapCount++;
                break;
            case BoosterType.doublePoints:
                gameSettings.DoublePointsCount++; 
                break;
            case BoosterType.extraMove:
                gameSettings.ExtraMoveCount++;
                break;
            case BoosterType.freeze:
                gameSettings.FreezeCount++;
                break;
        }
    }

    //������� �� ������ �������
    public void ShopItemBtnClick()
    {
        //���� ������� ����� - ��������
        if (gameSettings.Coins >= coinPrice)
        {
            //����� ������
            gameSettings.Coins -= coinPrice;
            SoundController.instance.PlayCollectedSound();
            //�������� �����
            BuySelectedBonus();
            //��������� ����������� �����
            if(shopController != null) shopController.UpdateCurrency();
            if(viewController != null) viewController.UpdateCoinTxt();
        }
    }
}
