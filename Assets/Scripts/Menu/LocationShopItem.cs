using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocationShopItem : MonoBehaviour
{
    [Header("Item Settings")]
    [Tooltip("јйди скина локации дл€ сохранени€")]
    [Range(1, 2)]
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
    private bool IsLocationItemPurchased
    {
        get
        {
            if (PlayerPrefs.HasKey($"IsLocationItemPurchased{currentSkinId}"))
            {
                if (PlayerPrefs.GetInt($"IsLocationItemPurchased{currentSkinId}") == 1)
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
                PlayerPrefs.SetInt($"IsLocationItemPurchased{currentSkinId}", 0);
                return false;
            }
        }

        set
        {
            if (value)
            {
                PlayerPrefs.SetInt($"IsLocationItemPurchased{currentSkinId}", 1);
            }
            else
            {
                PlayerPrefs.SetInt($"IsLocationItemPurchased{currentSkinId}", 0);
            }
        }
    }

    //определ€ем вид кнопки
    private void UpdateItemView()
    {
        itemIcon.SetActive(true);

        //проверить куплено ли
        if (!IsLocationItemPurchased)
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
        if (GameSettings.instance.CurrentLocSkinId == currentSkinId)
        {
            isEquiped = true;
        }
        return isEquiped;
    }

    //можем ли купить, выводим нужные экраны
    private void CanClaim()
    {
        if (!IsLocationItemPurchased)
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
                IsLocationItemPurchased = true;
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
        if (GameSettings.instance.CurrentLocSkinId == currentSkinId)
        {
            //снимаем
            GameSettings.instance.CurrentLocSkinId = 0;
        }
        else
        {
            //экипируем
            GameSettings.instance.CurrentLocSkinId = currentSkinId;
        }
    }

    //нажатие на кнопку покупки
    public void ShopItemBtnClick()
    {
        CanClaim();
    }
}
