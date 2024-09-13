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

    //������������� ������� ��� ���������
    public void SetCurrentItems(item[] items)
    {
        itemsType = items;
    }

    //���������� ��������� ����� �� ������� ���������
    public void GenerateRandomLine()
    {
        itemsInLine = new item[chips.Length];
        if (itemsType.Length > 0)
        {
            for (int i = 0; i < chips.Length; i++) 
            {
                //���������� �������� ����� �� �������� ���� �����
                itemsInLine[i] = itemsType[Random.Range(0, itemsType.Length)];
                //���������� ��������� ��������� ������ �� ������� ����� � �� ����� ��� �����������
                chips[i].GetComponent<Image>().sprite = itemsInLine[i].sprite;
                movedLineItems[i].GetComponent<Image>().sprite = itemsInLine[i].sprite;
            }
        }
    }

    //������������� ������������ �������� � �����
    public void GenerateDefineLine(int[] chipsId)
    {
        itemsInLine = new item[chipsId.Length];
        for (int i = 0; i < chipsId.Length; i++) 
        {
            itemsInLine[i] = itemsType[chipsId[i]];
            //���������� ��������� ������ �� ������� ����� � �� ����� ��� �����������
            chips[i].GetComponent<Image>().sprite = itemsInLine[i].sprite;
            movedLineItems[i].GetComponent<Image>().sprite = itemsInLine[i].sprite;
        }
    }

    //�������� �������� item �� ����������
    public item GetChipItem(int ind)
    {
        return itemsInLine[ind];
    }

}
