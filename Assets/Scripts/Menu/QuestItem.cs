using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestItem : MonoBehaviour
{

    [Header("ID")]
    [Tooltip("���� ������ �� �������� ��������� ������, � ������� ������ ������ ���� ���� ����")]
    [SerializeField] private int itemId;

    enum QuestType
    { 
        completeLevelInMinute = 0, 
        collectChipsAtOnce = 1,
    }
    [Header("Quest Type Settings")]
    [SerializeField] private QuestType type;

    [Header("Quest Type Complete in Minute")]
    [Range(1, 15)]
    [Tooltip("������� ��� ������")]
    [SerializeField] private int lvlToQuest;
    [Range(0.5f, 5f)]
    [Tooltip("������ �� ��������� �������")]
    [SerializeField] private float minuteToLevel;

    [Header("Collect Chips At Once")]
    [SerializeField] private int chipsAtOnceCount;

    enum treasure { coin = 0, gem = 1 }
    [Header("Rewards")]
    [SerializeField] private treasure rewardType;
    [SerializeField] private int rewardCount;

    [Header("Components")]
    [SerializeField] private TMPro.TMP_Text itemTxt;
    [SerializeField] private GameObject itemBtn;
    [SerializeField] private TMPro.TMP_Text btnTxt;
    [SerializeField] private Sprite blueBtnSprite;
    [SerializeField] private Sprite greenBtnSprite;
    [SerializeField] private TMPro.TMP_Text rewardTxt;
    [SerializeField] private GameObject coinImg;
    [SerializeField] private GameObject gemImg;

    private bool isDone = false;
    private bool isSelected = false;



    void Start()
    {
        itemBtn.GetComponent<Button>().onClick.AddListener(QuestBtnClick);
        UpdateItemVisual();
    }

    private void FixedUpdate()
    {
        UpdateItemVisual();
    }

    //��������� ����������� ������ � ����������� �� ��� ����
    private void UpdateItemVisual()
    {
        switch (type)
        {
            case QuestType.completeLevelInMinute:
                UpdateItemVisualType1();
                break;
            case QuestType.collectChipsAtOnce:
                UpdateItemVisualType2();
                break;
        }

        if (rewardType == treasure.gem)
        {
            gemImg.SetActive(true);
            coinImg.SetActive(false);
        }
        else
        {
            gemImg.SetActive(false);
            coinImg.SetActive(true);
        }
        rewardTxt.text = $"{rewardCount}";

        //BtnSettings
        if (!Quests.instance.QuestSelected)
        {
            btnTxt.text = "Select";
        }
        else
        {
            if (Quests.instance.DoneQuestId == -1)
            {
                //���� ����� �� ��������, ������� ������ �� ��� �����
                if (Quests.instance.SelectedQuestId == itemId)
                {
                    btnTxt.text = "Selected";
                    isSelected = true;
                }
                else
                {
                    isSelected = false;
                }
            }
            else
            {
                //���� ����� ��������, � ��� ��� ����� �� ������ ������ �� ���
                if (Quests.instance.DoneQuestId == itemId)
                {
                    btnTxt.text = "Get";
                    isDone = true;
                }
                else
                {
                    isDone = false;
                }
            }
        }

        itemBtn.GetComponent<Button>().interactable = true;
        if (!isDone)
        {
            if (isSelected)
            {
                itemBtn.GetComponent<Button>().interactable = false;
            }
            itemBtn.GetComponent<Image>().sprite = blueBtnSprite;
        }
        else
        {
            itemBtn.GetComponent<Image>().sprite = greenBtnSprite;
        }
    }

    //��������� ����������� ������ ������ ����������� ������� �� ������
    private void UpdateItemVisualType1()
    {
        itemTxt.text = $"Complete level {lvlToQuest} in {minuteToLevel} minutes";      
    }

    //��������� ����������� ������ ������� ������ ��������� �����
    private void UpdateItemVisualType2()
    {
        itemTxt.text = $"Collect {chipsAtOnceCount} chips at once";
    }

    //������ �� ������ ������
    private void QuestBtnClick()
    {
        //�������� �������
        if (!isSelected && !isDone) 
        {
            switch (type)
            {
                case QuestType.completeLevelInMinute:
                    QuestController.Instance.QuestCompleteLevelInMinuteSelected(itemId, lvlToQuest, minuteToLevel);
                    break;
                case QuestType.collectChipsAtOnce:
                    QuestController.Instance.QuestCollectChipsOnceSelected(itemId, chipsAtOnceCount);
                    break;
            }
        }
        //�������� �������
        if (isDone)
        {
            if (rewardType == treasure.gem)
            {
                GameSettings.instance.Gems += rewardCount;
            }
            else
            {
                GameSettings.instance.Coins += rewardCount;
            }

            //Play Sound
            SoundController.instance.PlayCoinSound();

            isSelected = false;
            isDone = false;
            //�������� ��� ������ �� ������� ����������
            Quests.instance.AllQuestsTodayDone();
        }
    }

    //���������� ���� ���������� ������
    public int GetId()
    {
        return itemId;
    }
}
