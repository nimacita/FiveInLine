using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinShopItem : MonoBehaviour
{
    [Header("Item Settings")]
    [Tooltip("���� ����� ��� ����������")]
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

    //����������� �������� ������ �� �����
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

    //���������� ��� ������
    private void UpdateItemView()
    {
        itemIcon.SetActive(true);

        //��������� ������� ��
        if (!IsSkinItemPurchased)
        {
            //���� �� ������
            equiped.SetActive(false);
            gemBg.SetActive(true);
            shopBtn.GetComponent<Image>().sprite = buySprite;
            //��������� ������� �� �����
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
            //���� ������
            shopBtn.GetComponent<Image>().sprite = selectSprite;
            equiped.SetActive(IsEquiped());
            gemBg.SetActive(false);
        }



    }

    //��������� ���� ���� ����������
    private bool IsEquiped()
    {
        bool isEquiped = false;
        if (GameSettings.instance.CurrentSkinId == currentSkinId)
        {
            isEquiped = true;
        }
        return isEquiped;
    }

    //����� �� ������, ������� ������ ������
    private void CanClaim()
    {
        if (!IsSkinItemPurchased)
        {
            //���� �� ������� - ��������
            if (GameSettings.instance.Gems < gemPrice)
            {
                //�� ����� ������
                
            }
            else
            {
                //����� ������
                GameSettings.instance.Gems -= gemPrice;
                SoundController.instance.PlayCollectedSound();
                IsSkinItemPurchased = true;
                if (shopController != null) shopController.UpdateCurrency();
            }
        }
        else
        {
            //����� �� ������� - ���������
            EquipedSelectProduct();
        }
    }

    //��������� ��� �������
    private void EquipedSelectProduct()
    {
        if (GameSettings.instance.CurrentSkinId == currentSkinId)
        {
            //�������
            GameSettings.instance.CurrentSkinId = 0;
        }
        else
        {
            //���������
            GameSettings.instance.CurrentSkinId = currentSkinId;
            SoundController.instance.PlayEquipedSound();
        }
    }

    //������� �� ������ �������
    public void ShopItemBtnClick()
    {
        CanClaim();
    }
}
