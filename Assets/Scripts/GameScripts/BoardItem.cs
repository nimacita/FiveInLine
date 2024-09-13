using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BoardItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [Header("Main Settings")]
    [SerializeField] private Image chipImg;
    [SerializeField] private Image inactiveChipImg;
    [SerializeField] private ParticleSystem itemParticels;
    [SerializeField] private GameObject destroySelected;
    [SerializeField] private GameObject greenSelected;

    [Header("Curr Item")]
    [SerializeField] private item currentItem;

    [Header("Animation")]
    [SerializeField] AnimationClip popAnim;
    private Animation anim;

    [Header("Bools")]
    [SerializeField]
    private bool selected;
    [SerializeField]
    private bool active;
    private bool selectedToPop = false;
    public bool SelectedToPop { get { return selectedToPop; } set { selectedToPop = value; } }
    private bool readyToPop = false;
    public bool ReadyToPop { get { return readyToPop; } set { readyToPop = value; } }

    private int cellI, cellJ;

    void Start()
    {
        selected = false;
        chipImg.gameObject.SetActive(false);
        inactiveChipImg.gameObject.SetActive(false);
        destroySelected.SetActive(false);
        anim = GetComponent<Animation>();

        if (currentItem.id == -1)
        {
            active = false;
        }
        else
        {
            EnableActive();
        }
    }

    //определяем значение элемента и картинку и частицы
    public void SetChipItem(item itm)
    {
        if (!active) 
        {
            currentItem = itm;
            chipImg.sprite = currentItem.sprite;
            inactiveChipImg.sprite = currentItem.sprite;
            itemParticels.startColor = currentItem.particlesColor;
        }
    }

    public void SwapChipItem(item itm)
    {
        currentItem = itm;
        chipImg.sprite = currentItem.sprite;
        inactiveChipImg.sprite = currentItem.sprite;
        itemParticels.startColor = currentItem.particlesColor;
    }

    //включаем прозрачные картинки
    public void EnableTransparent(bool value)
    {
        if (!active) 
        {
            inactiveChipImg.gameObject.SetActive(value);
            if (!value)
            {
                SetNullItem();
            }
        }
        else
        {
            inactiveChipImg.gameObject.SetActive(false);
        }
    }

    //включаем выделение для разрушения
    public void EnableRedSelected(bool value)
    {
        destroySelected.SetActive(value);
    }

    //включаем выделение для замены
    public void EnableGreenSelected(bool value)
    {
        greenSelected.SetActive(value);
    }

    //обнуляем айтем
    public void SetNullItem()
    {
        currentItem.id = -1;
        currentItem.sprite = null;
    }

    //выключаем элемент
    public void DisableActive()
    {
        active = false;
        chipImg.gameObject.SetActive(false);
    }

    //включаем активную картинку и помечаем активной
    public void EnableActive()
    {
        active = true;
        chipImg.gameObject.SetActive(true);
    }

    //лопаем текущий элемент
    public void PopItem()
    {
        active = false;
        selected = false;
        selectedToPop = false;
        readyToPop = false;
        SetNullItem();
        itemParticels.Play();
        chipImg.gameObject.SetActive(false);
        inactiveChipImg.gameObject.SetActive(false);
        destroySelected.SetActive(false);
    }

    //анимация лопанья
    public void PlayPopAnim()
    {
        active = false;
        selected = false;
        selectedToPop = false;
        readyToPop = false;
        anim.Play(popAnim.name);
    }

    //устанавливаем координаты на данную клетку с фишкой
    public void SetCellCoord(int i, int j)
    {
        cellI = i;
        cellJ = j;
    }

    //возвращаем координаты точки
    public Vector2 GetCoord()
    {
        Vector2 coord = new Vector2(cellI, cellJ);
        return coord;
    }

    //возвращаем значение селектед
    public bool GetSelected()
    {
        return selected;
    }

    //получаем текущее значение элемента
    public item GetCurrItem()
    {
        return currentItem;
    }

    //возвращаем активна ли клетка
    public bool GetActive()
    {
        return active;
    }

    public void SelectChipItem()
    {
        selected = true;
        GameController.instance.GetSellectedChip(cellI, cellJ);
        GameController.instance.GetSellectedBombChip(cellI, cellJ);
        GameController.instance.GetSellectedLightningChip(cellI, cellJ);
        GameController.instance.GetSellectedSwapChip(cellI, cellJ);
    }

    public void DeSelectChipItem()
    {
        selected = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SelectChipItem();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (Input.touchSupported)
        {
            if (Input.touches[0].phase == TouchPhase.Ended)
            {
                SelectChipItem();
            }
            else
            {
                DeSelectChipItem();
            }
        }
        else
        {
            DeSelectChipItem();
        }
    }
}
