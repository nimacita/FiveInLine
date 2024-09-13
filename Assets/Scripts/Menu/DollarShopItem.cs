using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DollarShopItem : MonoBehaviour
{
    [Header("Item Settings")]
    [SerializeField] private Sprite itemImage;

    enum CurrencyType { Gems = 0, Coins = 1}
    [Header("Dollar Item")]
    [SerializeField] private float dollarPrice;
    [SerializeField] private CurrencyType currencyType;
    [SerializeField] private int itemReward;

    [Space]
    [Header("Components")]
    [SerializeField] private Image itemIcon;
    [SerializeField] private TMPro.TMP_Text dollarTxt;
    [SerializeField] private TMPro.TMP_Text rewardTxt;
    [SerializeField] private ShopController shopController;

    void Start()
    {

        UpdateItemView();
    }

    //���������� ��� ������
    private void UpdateItemView()
    {

        itemIcon.sprite = itemImage;
        //������������� ����������� ��� ������� �� �������
        dollarTxt.text = $"${dollarPrice.ToString("0.00")}$";
        rewardTxt.text = $"{itemReward}";
    }


    //����� ����������� �������
    public void PurchaseComplete()
    {
        //���������� ������� ��� ����
        if (currencyType == CurrencyType.Coins)
        {
            GameSettings.instance.Coins += itemReward;
        }
        else
        {
            GameSettings.instance.Gems += itemReward;
        }
        //Play Sound
        SoundController.instance.PlayCoinSound();
        if (shopController != null) shopController.UpdateCurrency();
    }
}
