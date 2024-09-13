using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Quests : MonoBehaviour
{

    [Header("Debug")]
    [SerializeField] private bool questSelected;
    [SerializeField] private int donequestId;
    [SerializeField] private bool allQuestDone;
    [SerializeField] private bool newQuests;
    private int addDays;

    [Header("QLiM debug")]
    [SerializeField] private bool questSelectedIM;
    [SerializeField] private int qlimLvl;
    [SerializeField] private float qlimTime;
    [SerializeField]
    private bool isTimer = false;
    [SerializeField]
    private float currentTimerInMinutes = 0f;

    [Header("CCO debug")]
    [SerializeField] private bool questSelectdCCO;
    [SerializeField] private int onceChipsCount;
    [SerializeField] private int maxChipsOnceCount;

    [Header("RandomlyItem")]
    [SerializeField] private bool randomlyQuests;
    private int randomly0;
    private int randomly1;
    private int randomly2;
    private int randomly3;
    private int randomly4;

    private DateTime currentTime;

    public static Quests instance;

    void Awake()
    {
        if (!instance)
            instance = this;
        else
            Destroy(this.gameObject);


        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        
    }

    //сохранненные значения 

    //выбран какой либо квест
    public bool QuestSelected
    {
        get
        {
            if (!PlayerPrefs.HasKey("QuestSelected"))
            {
                PlayerPrefs.SetInt("QuestSelected", 0);
            }
            if (PlayerPrefs.GetInt("QuestSelected") == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        set
        {
            if (value)
            {
                PlayerPrefs.SetInt("QuestSelected", 1);
            }
            else
            {
                PlayerPrefs.SetInt("QuestSelected", 0);
            }
        }
    }

    //новые квесты которые еще не видели
    public bool NewQuests
    {
        get
        {
            if (!PlayerPrefs.HasKey("NewQuests"))
            {
                PlayerPrefs.SetInt("NewQuests", 0);
            }
            if (PlayerPrefs.GetInt("NewQuests") == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        set
        {
            if (value)
            {
                PlayerPrefs.SetInt("NewQuests", 1);
            }
            else
            {
                PlayerPrefs.SetInt("NewQuests", 0);
            }
        }
    }

    //сохраненное значения айди ввыбраного квеста
    public int SelectedQuestId
    {
        get
        {
            if (!PlayerPrefs.HasKey("SelectedQuestId"))
            {
                PlayerPrefs.SetInt("SelectedQuestId", -1);
            }
            return PlayerPrefs.GetInt("SelectedQuestId");
        }
        set
        {
            PlayerPrefs.SetInt("SelectedQuestId", value);
        }
    }

    //сохраненное значения пройденого квеста, если -1 то квест не пройден
    public int DoneQuestId
    {
        get
        {
            if (!PlayerPrefs.HasKey("DoneQuestId"))
            {
                PlayerPrefs.SetInt("DoneQuestId", -1);
            }
            return PlayerPrefs.GetInt("DoneQuestId");
        }
        set
        {
            PlayerPrefs.SetInt("DoneQuestId", value);
        }
    }

    //текущее время отсчет, обновляется каждый день в 00:00:01
    public DateTime QuestAcceptedTime
    {
        get
        {
            DateTime dateTime = new DateTime();
            if (!PlayerPrefs.HasKey("QuestAcceptedTime"))
            {
                //ставим верям на текущий день но 00:00:01
                dateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 1);
                PlayerPrefs.SetString("QuestAcceptedTime", dateTime.ToString());
            }
            return DateTime.Parse(PlayerPrefs.GetString("QuestAcceptedTime"));
        }
        set
        {
            PlayerPrefs.SetString("QuestAcceptedTime", value.ToString());
        }
    }

    //выбран квест собрать в ряд фишки
    public bool QuestSelectedCollectChipsOnce
    {
        get
        {
            if (!PlayerPrefs.HasKey("QuestSelectedCollectChopsOnce"))
            {
                PlayerPrefs.SetInt("QuestSelectedCollectChopsOnce", 0);
            }
            if (PlayerPrefs.GetInt("QuestSelectedCollectChopsOnce") == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        set
        {
            if (value)
            {
                PlayerPrefs.SetInt("QuestSelectedCollectChopsOnce", 1);
            }
            else
            {
                PlayerPrefs.SetInt("QuestSelectedCollectChopsOnce", 0);
            }
        }
    }

    //сохраненное значения сколько фишек надо собрать
    public int OnceChipsCount
    {
        get
        {
            if (!PlayerPrefs.HasKey("OnceChipsCount"))
            {
                PlayerPrefs.SetInt("OnceChipsCount", -1);
            }
            return PlayerPrefs.GetInt("OnceChipsCount");
        }
        set
        {
            PlayerPrefs.SetInt("OnceChipsCount", value);
        }
    }

    //сохраненное значения сколько фишек надо собрать
    public int MaxOnceChipsCount
    {
        get
        {
            if (!PlayerPrefs.HasKey("MaxOnceChipsCount"))
            {
                PlayerPrefs.SetInt("MaxOnceChipsCount", -1);
            }
            return PlayerPrefs.GetInt("MaxOnceChipsCount");
        }
        set
        {
            PlayerPrefs.SetInt("MaxOnceChipsCount", value);
        }
    }

    //выбран квест пройти уровень за минуты
    public bool QuestSelectedLelvInMinute
    {
        get
        {
            if (!PlayerPrefs.HasKey("QuestSelectedLelvInMinute"))
            {
                PlayerPrefs.SetInt("QuestSelectedLelvInMinute", 0);
            }
            if (PlayerPrefs.GetInt("QuestSelectedLelvInMinute") == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        set
        {
            if (value)
            {
                PlayerPrefs.SetInt("QuestSelectedLelvInMinute", 1);
            }
            else
            {
                PlayerPrefs.SetInt("QuestSelectedLelvInMinute", 0);
            }
        }
    }

    //сохраненное значения уровня 
    public int QLIMlvl
    {
        get
        {
            if (!PlayerPrefs.HasKey("QLIMlvl"))
            {
                PlayerPrefs.SetInt("QLIMlvl", -1);
            }
            return PlayerPrefs.GetInt("QLIMlvl");
        }
        set
        {
            PlayerPrefs.SetInt("QLIMlvl", value);
        }
    }

    //сохраненное значение минут
    public float QLIMMinuts
    {
        get
        {
            if (!PlayerPrefs.HasKey("QLIMMinuts"))
            {
                PlayerPrefs.SetFloat("QLIMMinuts", -1f);
            }
            return PlayerPrefs.GetFloat("QLIMMinuts");
        }
        set
        {
            PlayerPrefs.SetFloat("QLIMMinuts", value);
        }
    }

    //сохраненное значение, все квесты сегодня пройдены
    public bool AllQuestDone
    {
        get
        {
            if (!PlayerPrefs.HasKey("AllQuestDone"))
            {
                PlayerPrefs.SetInt("AllQuestDone", 0);
            }
            if (PlayerPrefs.GetInt("AllQuestDone") == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        set
        {
            if (value)
            {
                PlayerPrefs.SetInt("AllQuestDone", 1);
            }
            else
            {
                PlayerPrefs.SetInt("AllQuestDone", 0);
            }
        }
    }

    //сгенерирован ли сегодня случайный набор квестов
    public bool RandomlyQuests
    {
        get
        {
            if (!PlayerPrefs.HasKey("RandomlyQuests"))
            {
                PlayerPrefs.SetInt("RandomlyQuests", 0);
            }
            if (PlayerPrefs.GetInt("RandomlyQuests") == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        set
        {
            if (value)
            {
                PlayerPrefs.SetInt("RandomlyQuests", 1);
            }
            else
            {
                PlayerPrefs.SetInt("RandomlyQuests", 0);
            }
        }
    }

    //получаем случайно-сгененированный сегодня индекс квеста, передаем индекс эллемнета в случайном массиве от 0-5
    public int GetRandomItemInd(int index)
    {
        if (!PlayerPrefs.HasKey($"RandomItemInd{index}"))
        {
            PlayerPrefs.SetInt($"RandomItemInd{index}", -1);
        }
        //элементы от 0-5
        return PlayerPrefs.GetInt($"RandomItemInd{index}");
    }

    //index - значение от 0 до 5  ItmeInd - номер квеста выбранного
    public void SetRandomItemInd(int index, int ItemInd)
    {
        PlayerPrefs.SetInt($"RandomItemInd{index}", ItemInd);
    }

    //обнуляем эллементы массива случайных квестов
    public void ResetRandomItems()
    {
        for (int i = 0; i < 5; i++)
        {
            PlayerPrefs.SetInt($"RandomItemInd{i}", -1);
        }
        RandomlyQuests = false;
    }

    void FixedUpdate()
    {
        CurrentTimeUpdate();
        IsQuestActive();
        UpdateDebugData();
        TimerQuestLelvInMinute();
    }

    private void CurrentTimeUpdate()
    {
        currentTime = DateTime.Now.AddDays(addDays);
    }

    //обнволяем таймер ожидания
    public string UpdateTimeToNextrewardTxt()
    {
        TimeSpan sub = new TimeSpan(24, 0, 0).Subtract(currentTime.Subtract(QuestAcceptedTime));

        string txt = $"{sub.Hours:D2}:{sub.Minutes:D2}:{sub.Seconds:D2}";
        return txt;
    }

    //действует ли еще квест или идет или еще откат
    private bool IsQuestWait() 
    {
        if (currentTime.Subtract(QuestAcceptedTime).Days >= 1)
        {
            //время прошло
            return true;
        }
        else
        {
            //время не прошло
            return false;
        }
    }

    private void UpdateDebugData()
    {
        questSelected = QuestSelected;
        donequestId = DoneQuestId;
        allQuestDone = AllQuestDone;
        randomlyQuests = RandomlyQuests;

        randomly0 = GetRandomItemInd(0);
        randomly1 = GetRandomItemInd(1);
        randomly2 = GetRandomItemInd(2);
        randomly3 = GetRandomItemInd(3);
        randomly4 = GetRandomItemInd(4);

        questSelectdCCO = QuestSelectedCollectChipsOnce;
        onceChipsCount = OnceChipsCount;
        maxChipsOnceCount = MaxOnceChipsCount;

        questSelectedIM = QuestSelectedLelvInMinute;
        qlimLvl = QLIMlvl;
        qlimTime = QLIMMinuts;
    }

    //обновляем используется ли еще квест
    private void IsQuestActive()
    {
        //время прошло
        if (IsQuestWait())
        {
            if (QuestSelected)
            {
                //если время прошло а квест еще активен, то обнуляем

            }
            //обновляем квесты и время
            ResetQuests();
        }
    }

    //обнуяем значения квестов
    private void ResetQuests()
    {
        QuestSelected = false;
        AllQuestDone = false;
        RandomlyQuests = false;
        ResetRandomItems();
        SelectedQuestId = -1;
        DoneQuestId = -1;

        QuestSelectedLelvInMinute = false;
        QLIMlvl = -1;
        QLIMMinuts = -1f;

        QuestSelectedCollectChipsOnce = false;
        OnceChipsCount = -1;
        MaxOnceChipsCount = 0;

        isTimer = false;
        currentTimerInMinutes = 0f;

    ResetTime();
    }

    //обнлуяем время
    private void ResetTime()
    {
        //обновляем время на текущий день на 00:00:01 и начинаем новый отсчет
        QuestAcceptedTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 1);
    }

    //отмечаем все квесты на сегодня пройденными
    public void AllQuestsTodayDone()
    {
        ResetQuests();
        AllQuestDone = true;
    }

    //выбрали квест пройти урвоень за минуты
    public void SelectedLelvInMinute(int questId, int lvl, float minutes)
    {
        if (!QuestSelected)
        {
            QuestSelectedLelvInMinute = true;
            QLIMMinuts = minutes;
            QLIMlvl = lvl;

            SelectedQuestId = questId;
            QuestSelected = true;
        }
    }

    //начинаем записывать время если нужный раунд и квест активен
    public void StartQuestLelvInMinute()
    {
        if (DoneQuestId == -1)
        {
            if (QuestSelected && QuestSelectedLelvInMinute)
            {
                if (GameSettings.instance.ThisLvlIndex == QLIMlvl - 1)
                {
                    //если квест активен и выбран нужный уровень, то нчинаем считать минуты
                    isTimer = true;
                    currentTimerInMinutes = 0f;
                }
            }
        }
    }

    //считаем таймер для квеста
    private void TimerQuestLelvInMinute()
    {
        if (isTimer)
        {
            currentTimerInMinutes += Time.fixedDeltaTime;
        }
    }

    //проверяем квест после завершения уровня
    public void CheckQuestLelvInMinute()
    {
        isTimer = false;
        if (DoneQuestId == -1)
        {
            if (QuestSelected && QuestSelectedLelvInMinute)
            {
                if (GameSettings.instance.ThisLvlIndex == QLIMlvl - 1)
                {
                    //если квест активен и выбран нужный уровень, то проверяем выполнен ли
                    if (currentTimerInMinutes <= (QLIMMinuts * 60f))
                    {
                        //прошли
                        DoneQuestId = SelectedQuestId;

                    }
                    else
                    {
                        //не прошли
                    }
                    currentTimerInMinutes = 0;
                }
            }
        }
    }

    //в случае поражения или выхода обнуляем счетчик квеста
    public void ResetQuestLelvInMinuteTimer()
    {
        if (isTimer)
        {
            currentTimerInMinutes = 0;
            isTimer = false;
        }
    }

    //возвращаем таймер текущий
    public float GetCurrentTimer()
    {
        return currentTimerInMinutes;
    }

    //выбрали квест собрать несколько фишек сразу
    public void SelectedChipsOnceCollect(int questId, int onceChipsCount)
    {
        if (!QuestSelected)
        {
            QuestSelectedCollectChipsOnce = true;
            OnceChipsCount = onceChipsCount;
            MaxOnceChipsCount = 0;
            
            SelectedQuestId = questId;
            QuestSelected = true;
        }
    }

    //проверяем собрали ли нужное количество
    public void IsCollectedOnceChips(int currChipsOnce)
    {
        if (DoneQuestId == -1)
        {
            if (QuestSelected && QuestSelectedCollectChipsOnce)
            {
                if (currChipsOnce > MaxOnceChipsCount)
                {
                    MaxOnceChipsCount = currChipsOnce;
                }
                if (MaxOnceChipsCount >= OnceChipsCount)
                {
                    //прошли
                    DoneQuestId = SelectedQuestId;
                }
            }
        }
    }
    
}
