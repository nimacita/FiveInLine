using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LvlBtn : MonoBehaviour
{
    [Header("Lvl Settings")]
    [Tooltip("Номер уровня, от которого зависит выбранный уровень")]
    [SerializeField] private int lvlNumber;
    [Tooltip("Количество очков которое нужно набрать для победы")]
    [SerializeField] private int neededScore;
    [Tooltip("Количество ходов со старта")]
    [SerializeField] private int stepCount;
    [Tooltip("Время на ход")]
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

    //определяем открыт ли уровень
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

    //показываем есть ли в этом уровне квест
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

    //определяем отображение кнопки
    private void DefineBtnView()
    {
        lvlTxt.text = lvlNumber.ToString();
        lvlTxt.gameObject.SetActive(true);
        //открыт ли уровень
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

        //открыта ли кнопка квеста
        tipsBtn.gameObject.SetActive(IsTips());
    }

    //нажатие на кнопку
    private void LvlBtnClick()
    {
        //передаем данные уровня для запуска
        gameSettings.ThisLvlIndex = lvlNumber - 1;
        gameSettings.NeededScore = neededScore;
        gameSettings.TimeStep = timeToStep;
        gameSettings.StepCount = stepCount;
        gameSettings.SetCompaign();
        //запускаем уровень
        levelMenuController.LvlBtnClick("GameScene");
    }
}
