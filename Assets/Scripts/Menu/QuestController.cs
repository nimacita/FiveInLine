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

    //���� �� �����
    private void IsPopUp()
    {
        if (Quests.instance.DoneQuestId != -1 && Quests.instance.QuestSelected && !Quests.instance.AllQuestDone)
        {
            //���� ����� ������
            popUpDailyGetTxt.SetActive(true);
            popUpDailyNewTxt.SetActive(false);
            popUpDailyPanel.SetActive(true);
        }else if (!Quests.instance.QuestSelected && !Quests.instance.AllQuestDone && Quests.instance.NewQuests)
        {
            //���� ����� �������
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

    //��������� ��������� ������
    private void StartSettingsView()
    {
        QuestView.SetActive(false);
    }

    //�������� ���� �������
    public void QuestViewOn()
    {
        QuestView.SetActive(true);
        Quests.instance.NewQuests = false;
        questAnim.Play(questOn.name);
    }

    //��������� �����
    private void QuestOff()
    {
        menuController.MenuOn();
        StartCoroutine(QuestOffAnim());
    }

    //��������� ����� � ���������
    private IEnumerator QuestOffAnim()
    {
        questAnim.Play(questOff.name);
        yield return new WaitForSeconds(questOff.length);
        QuestView.SetActive(false);
    }

    //������� ��� ����������� ����� ���� ���� ���������
    private void UpdateQuests()
    {
        //���� ��� ������ �� ����������, ����������
        if (!Quests.instance.AllQuestDone) 
        {
            doneQuestTxt.SetActive(false);
            //���� ���� ��������� ����� ���������� ������ ���
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
                //���� ������ �� �������������, �� ���������� 5 ��������� �������
                if (!Quests.instance.RandomlyQuests)
                {
                    //����������
                    GenerateRandomQuests();
                    //�������� ����� ������
                    Quests.instance.NewQuests = true;
                }
                //���� ��� ���������� ������, �� ���������� ��� ��������-��������������� �� �������
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
            //���� ��� ������ ��������� �� �������, �� ��������� ��� ������ � ���������� ���������
            for (int i = 0; i < allQuests.Length; i++)
            {
                allQuests[i].SetActive(false);
            }
            doneQuestTxt.SetActive(true);
        }
    }

    //���������� ��������� ����� ������� �� �������
    private void GenerateRandomQuests(int startInd = 0)
    {
        for (int i = startInd; i < 5; i++)
        {
            //���������� ��������� ����� ������� ��������� �� 0 �� ���������� ���������
            int RandomItemInd = UnityEngine.Random.Range(0, allQuests.Length);
            for (int j = 0; j < 5; j++) 
            {
                //���� ����� ����� ��� ����, ���������� ������
                if (RandomItemInd == Quests.instance.GetRandomItemInd(j))
                {
                    GenerateRandomQuests(i);
                    return;
                }
            }
            //���� ������ ����� ���, �� ���������� � ������ 
            Quests.instance.SetRandomItemInd(i, RandomItemInd);
        }
        Quests.instance.RandomlyQuests = true;
    }

    //������� ����� ���� ��������� ������� �� ���������� �����
    public void QuestCompleteLevelInMinuteSelected(int questId, int lvl, float minutes)
    {
        //������������� �������� ���������� ������
        Quests.instance.SelectedLelvInMinute(questId, lvl, minutes);
    }

    //������� ����� ���� ������� ��������� ����� � ��� �����
    public void QuestCollectChipsOnceSelected(int questId, int chipsOnceCount)
    {
        //������������� �������� ���������� ������
        Quests.instance.SelectedChipsOnceCollect(questId, chipsOnceCount);
    }
}
