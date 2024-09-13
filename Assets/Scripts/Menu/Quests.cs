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

    //������������ �������� 

    //������ ����� ���� �����
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

    //����� ������ ������� ��� �� ������
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

    //����������� �������� ���� ���������� ������
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

    //����������� �������� ���������� ������, ���� -1 �� ����� �� �������
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

    //������� ����� ������, ����������� ������ ���� � 00:00:01
    public DateTime QuestAcceptedTime
    {
        get
        {
            DateTime dateTime = new DateTime();
            if (!PlayerPrefs.HasKey("QuestAcceptedTime"))
            {
                //������ ����� �� ������� ���� �� 00:00:01
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

    //������ ����� ������� � ��� �����
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

    //����������� �������� ������� ����� ���� �������
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

    //����������� �������� ������� ����� ���� �������
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

    //������ ����� ������ ������� �� ������
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

    //����������� �������� ������ 
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

    //����������� �������� �����
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

    //����������� ��������, ��� ������ ������� ��������
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

    //������������ �� ������� ��������� ����� �������
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

    //�������� ��������-��������������� ������� ������ ������, �������� ������ ��������� � ��������� ������� �� 0-5
    public int GetRandomItemInd(int index)
    {
        if (!PlayerPrefs.HasKey($"RandomItemInd{index}"))
        {
            PlayerPrefs.SetInt($"RandomItemInd{index}", -1);
        }
        //�������� �� 0-5
        return PlayerPrefs.GetInt($"RandomItemInd{index}");
    }

    //index - �������� �� 0 �� 5  ItmeInd - ����� ������ ����������
    public void SetRandomItemInd(int index, int ItemInd)
    {
        PlayerPrefs.SetInt($"RandomItemInd{index}", ItemInd);
    }

    //�������� ��������� ������� ��������� �������
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

    //��������� ������ ��������
    public string UpdateTimeToNextrewardTxt()
    {
        TimeSpan sub = new TimeSpan(24, 0, 0).Subtract(currentTime.Subtract(QuestAcceptedTime));

        string txt = $"{sub.Hours:D2}:{sub.Minutes:D2}:{sub.Seconds:D2}";
        return txt;
    }

    //��������� �� ��� ����� ��� ���� ��� ��� �����
    private bool IsQuestWait() 
    {
        if (currentTime.Subtract(QuestAcceptedTime).Days >= 1)
        {
            //����� ������
            return true;
        }
        else
        {
            //����� �� ������
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

    //��������� ������������ �� ��� �����
    private void IsQuestActive()
    {
        //����� ������
        if (IsQuestWait())
        {
            if (QuestSelected)
            {
                //���� ����� ������ � ����� ��� �������, �� ��������

            }
            //��������� ������ � �����
            ResetQuests();
        }
    }

    //������� �������� �������
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

    //�������� �����
    private void ResetTime()
    {
        //��������� ����� �� ������� ���� �� 00:00:01 � �������� ����� ������
        QuestAcceptedTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 1);
    }

    //�������� ��� ������ �� ������� �����������
    public void AllQuestsTodayDone()
    {
        ResetQuests();
        AllQuestDone = true;
    }

    //������� ����� ������ ������� �� ������
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

    //�������� ���������� ����� ���� ������ ����� � ����� �������
    public void StartQuestLelvInMinute()
    {
        if (DoneQuestId == -1)
        {
            if (QuestSelected && QuestSelectedLelvInMinute)
            {
                if (GameSettings.instance.ThisLvlIndex == QLIMlvl - 1)
                {
                    //���� ����� ������� � ������ ������ �������, �� ������� ������� ������
                    isTimer = true;
                    currentTimerInMinutes = 0f;
                }
            }
        }
    }

    //������� ������ ��� ������
    private void TimerQuestLelvInMinute()
    {
        if (isTimer)
        {
            currentTimerInMinutes += Time.fixedDeltaTime;
        }
    }

    //��������� ����� ����� ���������� ������
    public void CheckQuestLelvInMinute()
    {
        isTimer = false;
        if (DoneQuestId == -1)
        {
            if (QuestSelected && QuestSelectedLelvInMinute)
            {
                if (GameSettings.instance.ThisLvlIndex == QLIMlvl - 1)
                {
                    //���� ����� ������� � ������ ������ �������, �� ��������� �������� ��
                    if (currentTimerInMinutes <= (QLIMMinuts * 60f))
                    {
                        //������
                        DoneQuestId = SelectedQuestId;

                    }
                    else
                    {
                        //�� ������
                    }
                    currentTimerInMinutes = 0;
                }
            }
        }
    }

    //� ������ ��������� ��� ������ �������� ������� ������
    public void ResetQuestLelvInMinuteTimer()
    {
        if (isTimer)
        {
            currentTimerInMinutes = 0;
            isTimer = false;
        }
    }

    //���������� ������ �������
    public float GetCurrentTimer()
    {
        return currentTimerInMinutes;
    }

    //������� ����� ������� ��������� ����� �����
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

    //��������� ������� �� ������ ����������
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
                    //������
                    DoneQuestId = SelectedQuestId;
                }
            }
        }
    }
    
}
