using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestController : MonoBehaviour
{

    [Header("Quests")]
    [SerializeField] private GameObject[] allQuests;

    [Header("View Settings")]
    [SerializeField] private GameObject QuestView;
    [SerializeField] private Animation questAnim;
    [SerializeField] private AnimationClip questOn;
    [SerializeField] private AnimationClip questOff;
    [SerializeField] private Button backBtn;
    [SerializeField] private TMPro.TMP_Text timerTxt;
    [SerializeField] private GameObject doneQuestTxt;

    [Header("PopUp")]
    [SerializeField] private GameObject popUpDailyPanel;
    [SerializeField] private GameObject popUpDailyNewTxt;
    [SerializeField] private GameObject popUpDailyGetTxt;

    [Header("Components")]
    [SerializeField] private MenuController menuController;

    public static QuestController Instance;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        backBtn.onClick.AddListener(QuestOff);
        StartSettingsView();
    }

    private void Update()
    {
        UpdateQuests();
        UpdateTimerTxt();
        IsPopUp();
    }

    private void UpdateTimerTxt()
    {
        timerTxt.text = Quests.instance.UpdateTimeToNextrewardTxt();
    }

    //есть ли попап
    private void IsPopUp()
    {
        if (Quests.instance.DoneQuestId != -1 && Quests.instance.QuestSelected && !Quests.instance.AllQuestDone)
        {
            //окно сбора бонуса
            popUpDailyGetTxt.SetActive(true);
            popUpDailyNewTxt.SetActive(false);
            popUpDailyPanel.SetActive(true);
        }else if (!Quests.instance.QuestSelected && !Quests.instance.AllQuestDone && Quests.instance.NewQuests)
        {
            //окно новых квестов
            popUpDailyGetTxt.SetActive(false);
            popUpDailyNewTxt.SetActive(true);
            popUpDailyPanel.SetActive(true);
        }
        else
        {
            popUpDailyGetTxt.SetActive(false);
            popUpDailyNewTxt.SetActive(false);
            popUpDailyPanel.SetActive(false);
        }
    }

    //начальные настройки экрана
    private void StartSettingsView()
    {
        QuestView.SetActive(false);
    }

    //включаем меню квестов
    public void QuestViewOn()
    {
        QuestView.SetActive(true);
        Quests.instance.NewQuests = false;
        questAnim.Play(questOn.name);
    }

    //выключаем экран
    private void QuestOff()
    {
        menuController.MenuOn();
        StartCoroutine(QuestOffAnim());
    }

    //выключаем экран с анимацией
    private IEnumerator QuestOffAnim()
    {
        questAnim.Play(questOff.name);
        yield return new WaitForSeconds(questOff.length);
        QuestView.SetActive(false);
    }

    //убираем все невыбранные квест если есть выбранный
    private void UpdateQuests()
    {
        //если все квесты не выполненны, отображаем
        if (!Quests.instance.AllQuestDone) 
        {
            doneQuestTxt.SetActive(false);
            //если есть выбранный квест отображаем только его
            if (Quests.instance.QuestSelected)
            {
                for (int i = 0; i < allQuests.Length; i++)
                {
                    if (allQuests[i].GetComponent<QuestItem>().GetId() == Quests.instance.SelectedQuestId)
                    {
                        allQuests[i].SetActive(true);
                    }
                    else
                    {
                        allQuests[i].SetActive(false);
                    }
                }
            }
            else
            {
                //если квесты не сгенерированы, то генерируем 5 случайных квестов
                if (!Quests.instance.RandomlyQuests)
                {
                    //генерируем
                    GenerateRandomQuests();
                    //отмечаем новые квесты
                    Quests.instance.NewQuests = true;
                }
                //если нет выбранного квеста, то показываем все случайно-сгенерированные на сегодня
                for (int i = 0; i < allQuests.Length; i++)
                {
                    allQuests[i].SetActive(false);
                    for (int j = 0; j < 5; j++)
                    {
                        if (allQuests[i].GetComponent<QuestItem>().GetId() == Quests.instance.GetRandomItemInd(j))
                        {
                            allQuests[i].SetActive(true);
                        }
                    }
                }
            }
        }
        else
        {
            //если все квесты выполенны на сегодня, то выключаем все квесты и показываем сообщение
            for (int i = 0; i < allQuests.Length; i++)
            {
                allQuests[i].SetActive(false);
            }
            doneQuestTxt.SetActive(true);
        }
    }

    //генерируем случайный набор квестов на сегодня
    private void GenerateRandomQuests(int startInd = 0)
    {
        for (int i = startInd; i < 5; i++)
        {
            //генерируем случайное число индекса эллемента от 0 до количества элементов
            int RandomItemInd = UnityEngine.Random.Range(0, allQuests.Length);
            for (int j = 0; j < 5; j++) 
            {
                //если такое число уже есть, генерируем заново
                if (RandomItemInd == Quests.instance.GetRandomItemInd(j))
                {
                    GenerateRandomQuests(i);
                    return;
                }
            }
            //если такого числа нет, то записываем в массив 
            Quests.instance.SetRandomItemInd(i, RandomItemInd);
        }
        Quests.instance.RandomlyQuests = true;
    }

    //выбрали квест типа завершить уровень за отведенное время
    public void QuestCompleteLevelInMinuteSelected(int questId, int lvl, float minutes)
    {
        //устанавливаем значения выбранного квеста
        Quests.instance.SelectedLelvInMinute(questId, lvl, minutes);
    }

    //выбрали квест типа собрать несколько фишек в ряд сразу
    public void QuestCollectChipsOnceSelected(int questId, int chipsOnceCount)
    {
        //устанавливаем значения выбранного квеста
        Quests.instance.SelectedChipsOnceCollect(questId, chipsOnceCount);
    }
}
