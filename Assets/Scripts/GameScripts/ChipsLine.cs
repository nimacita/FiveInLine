using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChipsLine : MonoBehaviour
{

    [Header("Chips in Line")]
    [SerializeField] private GameObject[] chips;
    private item[] itemsInLine;

    [Header("Chips Setting")]
    private item[] itemsType;

    [Header("Moved Chips Line")]
    [SerializeField] private GameObject[] movedLineItems;

    void Start()
    {
        
    }

    //устанавливаем текущий тип элементов
    public void SetCurrentItems(item[] items)
    {
        itemsType = items;
    }

    //генерируем рандомную линию из текущих элементов
    public void GenerateRandomLine()
    {
        itemsInLine = new item[chips.Length];
        if (itemsType.Length > 0)
        {
            for (int i = 0; i < chips.Length; i++) 
            {
                //генерируем случаные фишки из текущего типа фишек
                itemsInLine[i] = itemsType[Random.Range(0, itemsType.Length)];
                //отображаем выбранный случайный спрайт на текущую линию и на линию для перемещения
                chips[i].GetComponent<Image>().sprite = itemsInLine[i].sprite;
                movedLineItems[i].GetComponent<Image>().sprite = itemsInLine[i].sprite;
            }
        }
    }

    //устанавливаем определенные значения в линию
    public void GenerateDefineLine(int[] chipsId)
    {
        itemsInLine = new item[chipsId.Length];
        for (int i = 0; i < chipsId.Length; i++) 
        {
            itemsInLine[i] = itemsType[chipsId[i]];
            //отображаем выбранный спрайт на текущую линию и на линию для перемещения
            chips[i].GetComponent<Image>().sprite = itemsInLine[i].sprite;
            movedLineItems[i].GetComponent<Image>().sprite = itemsInLine[i].sprite;
        }
    }

    //получаем значение item по координате
    public item GetChipItem(int ind)
    {
        return itemsInLine[ind];
    }

}
