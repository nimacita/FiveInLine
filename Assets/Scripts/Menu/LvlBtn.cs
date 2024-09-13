using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LvlBtn : MonoBehaviour
{
    [Header("Lvl Settings")]
    [Tooltip("����� ������, �� �������� ������� ��������� �������")]
    [SerializeField] private int lvlNumber;
    [Tooltip("���������� ����� ������� ����� ������� ��� ������")]
    [SerializeField] private int neededScore;
    [Tooltip("���������� ����� �� ������")]
    [SerializeField] private int stepCount;
    [Tooltip("����� �� ���")]
    [SerializeField] private int timeToStep;

    [Header("Editor")]
    [SerializeField] private TMPro.TMP_Text lvlTxt;
    [SerializeField] private GameObject lockedImg;
    [SerializeField] private GameObject tipsBtn;
    public LvlMenuController levelMenuController;

    [Header("Debug")]
    [SerializeField]
    private bool locked;

    private GameSettings gameSettings;

    void Start()
    {
        gameSettings = GameSettings.instance;

        GetComponent<Button>().onClick.AddListener(LvlBtnClick);

        DefineBtnView();
    }

    //���������� ������ �� �������
    private bool DefineIsOpenLvl()
    {
        if (lvlNumber - 1 <= gameSettings.CurrentOpenLvlIndex + 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //���������� ���� �� � ���� ������ �����
    private bool IsTips()
    {
        if (Quests.instance.QuestSelected && 
            !Quests.instance.AllQuestDone && 
            Quests.instance.DoneQuestId == -1 && 
            lvlNumber == Quests.instance.QLIMlvl &&
            Quests.instance.QuestSelectedLelvInMinute)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //���������� ����������� ������
    private void DefineBtnView()
    {
        lvlTxt.text = lvlNumber.ToString();
        lvlTxt.gameObject.SetActive(true);
        //������ �� �������
        if (DefineIsOpenLvl())
        {
            lockedImg.SetActive(false);
            gameObject.GetComponent<Button>().interactable = true;
        }
        else
        {
            lockedImg.SetActive(true);
            gameObject.GetComponent<Button>().interactable = false;
        }

        //������� �� ������ ������
        tipsBtn.gameObject.SetActive(IsTips());
    }

    //������� �� ������
    private void LvlBtnClick()
    {
        //�������� ������ ������ ��� �������
        gameSettings.ThisLvlIndex = lvlNumber - 1;
        gameSettings.NeededScore = neededScore;
        gameSettings.TimeStep = timeToStep;
        gameSettings.StepCount = stepCount;
        gameSettings.SetCompaign();
        //��������� �������
        levelMenuController.LvlBtnClick("GameScene");
    }
}
