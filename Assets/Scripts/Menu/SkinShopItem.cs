using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinShopItem : MonoBehaviour
{
    [Header("Item Settings")]
    [Tooltip("јйди скина дл€ сохранени€")]
    [Range(1,5)]
    [SerializeField] private int currentSkinId;

    [Header("Gem Item")]
    [SerializeField] private int gemPrice;

    [Header("To Item Image")]
    public Sprite itemIconSprite;

    [Space]
    [Header("Components")]
    [SerializeField] private GameObject itemIcon;
    [SerializeField] private GameObject shopBtn;
    [SerializeField] private Sprite buySprite;
    [SerializeField] private Sprite selectSprite;
    [SerializeField] private TMPro.TMP_Text gemTxt;
    [SerializeField] private GameObject gemBg;
    [SerializeField] private GameObject equiped;
    [SerializeField] private ShopController shopController;

    void Start()
    {
        itemIcon.GetComponent<Image>().sprite = itemIconSprite;
        gemTxt.text = $"{gemPrice}";
        shopBtn.GetComponent<Button>().interactable = true;
        shopBtn.GetComponent<Button>().onClick.AddListener(ShopItemBtnClick);

        UpdateItemView();
    }

    private void FixedUpdate()
    {
        UpdateItemView();
    }

    //сохраненное значение куплен ли товар
    private bool IsSkinItemPurchased
    {
        get
        {
            if (PlayerPrefs.HasKey($"IsSkinItemPurchased{currentSkinId}"))
            {
                if (PlayerPrefs.GetInt($"IsSkinItemPurchased{currentSkinId}") == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                PlayerPrefs.SetInt($"IsSkinItemPurchased{currentSkinId}", 0);
                return false;
            }
        }

        set
        {
            if (value)
            {
                PlayerPrefs.SetInt($"IsSkinItemPurchased{currentSkinId}", 1);
            }
            else
            {
                PlayerPrefs.SetInt($"IsSkinItemPurchased{currentSkinId}", 0);
            }
        }
    }

    //определ€ем вид кнопки
    private void UpdateItemView()
    {
        itemIcon.SetActive(true);

        //проверить куплено ли
        if (!IsSkinItemPurchased)
        {
            //если не куплен
            equiped.SetActive(false);
            gemBg.SetActive(true);
            shopBtn.GetComponent<Image>().sprite = buySprite;
            //проверить хватает ли денег
            if (GameSettings.instance.Gems < gemPrice)
            {
                shopBtn.GetComponent<Button>().interactable = false;
            }
            else
            {
                shopBtn.GetComponent<Button>().interactable = true;
            }
        }
        else
        {
            //если куплен
            shopBtn.GetComponent<Image>().sprite = selectSprite;
            equiped.SetActive(IsEquiped());
            gemBg.SetActive(false);
        }



    }

    //провер€ем если скин экипирован
    private bool IsEquiped()
    {
        bool isEquiped = false;
        if (GameSettings.instance.CurrentSkinId == currentSkinId)
        {
            isEquiped = true;
        }
        return isEquiped;
    }

    //можем ли купить, выводим нужные экраны
    private void CanClaim()
    {
        if (!IsSkinItemPurchased)
        {
            //если не куплено - покупаем
            if (GameSettings.instance.Gems < gemPrice)
            {
                //не можем купить
                
            }
            else
            {
                //можем купить
                GameSettings.instance.Gems -= gemPrice;
                SoundController.instance.PlayCollectedSound();
                IsSkinItemPurchased = true;
                if (shopController != null) shopController.UpdateCurrency();
            }
        }
        else
        {
            //иначе по нажатию - экипируем
            EquipedSelectProduct();
        }
    }

    //экипируем или снимаем
    private void EquipedSelectProduct()
    {
        if (GameSettings.instance.CurrentSkinId == currentSkinId)
        {
            //снимаем
            GameSettings.instance.CurrentSkinId = 0;
        }
        else
        {
            //экипируем
            GameSettings.instance.CurrentSkinId = currentSkinId;
            SoundController.instance.PlayEquipedSound();
        }
    }

    //нажатие на кнопку покупки
    public void ShopItemBtnClick()
    {
        CanClaim();
    }
}
