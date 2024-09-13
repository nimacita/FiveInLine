using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{

    [Header("Tutorial View")]
    [SerializeField] private GameObject[] tips;
    private int currentTip = 0;
    [SerializeField] private Button nextTipBtn;
    [SerializeField] private GameObject mainTutPanel;
    [SerializeField] private GameObject tutorialView;
    [SerializeField] private GameObject handPointer;

    [Header("Components")]
    [SerializeField] private ChipsLine lineController;
    [SerializeField] private GameController gameController;
    [SerializeField] private ViewController viewController;

    public static TutorialController instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        mainTutPanel.SetActive(false);
        tutorialView.SetActive(true);
        handPointer.SetActive(false);
        nextTipBtn.gameObject.SetActive(true);
        nextTipBtn.onClick.AddListener(NextTip);
        ActiveCurrentTip();

        lineController.SetCurrentItems(GameSettings.instance.GetCurrentItem());
        GenerateFirstLine();
    }
    
    //��� ���������
    private void NextTip()
    {
        currentTip++;
        if (currentTip < tips.Length)
        {
            ActiveCurrentTip();
        }
        else
        {
            StartTutorialGame();
        }
    }

    //�������� ��������� ���������
    private void ActiveCurrentTip()
    {
        for (int i = 0; i < tips.Length; i++)
        {
            if (i == currentTip)
            {
                tips[i].SetActive(true);
            }
            else
            {
                tips[i].SetActive(false);
            }
        }
    }

    //�������� �������� ����
    private void StartTutorialGame()
    {
        nextTipBtn.gameObject.SetActive(false);
        for (int i = 0; i < tips.Length; i++)
        {
            tips[i].SetActive(false);
        }
        mainTutPanel.SetActive(true);
        handPointer.SetActive(true);
        GenerateFirstChipsOnBoard();
    }

    public void OffPointer()
    {
        handPointer.SetActive(false);
    }

    //���������� �����
    private void GenerateFirstLine()
    {
        //������ ��������� �����
        int[] line = { 2, 2, 1, 0, 3 };
        lineController.GenerateDefineLine(line);
    }

    //��������� ����� �� �����
    public void GenerateFirstChipsOnBoard()
    {
        //������� ��� �����
        for (int i = 0; i < 2; i++) 
        {
            for (int j = 0;j<gameController.GetColumnCount();j++)
            {
                gameController.boardItemsControllers[i, j].SetChipItem(GameSettings.instance.GetCurrentItem()[3]);
                gameController.boardItemsControllers[i, j].EnableActive();
            }
        }
        //������� ������
        for (int j = 0; j < gameController.GetColumnCount(); j++)
        {
            gameController.boardItemsControllers[2, j].SetChipItem(GameSettings.instance.GetCurrentItem()[4]);
            gameController.boardItemsControllers[2, j].EnableActive();
        }
        //������� �� ��������� �������
        gameController.boardItemsControllers[2, 5].DisableActive();
        gameController.boardItemsControllers[2, 5].SetChipItem(GameSettings.instance.GetCurrentItem()[2]);
        gameController.boardItemsControllers[2, 5].EnableActive();
        //������� ���������
        for (int j = 0; j < gameController.GetColumnCount(); j++)
        {
            gameController.boardItemsControllers[2, j].SetChipItem(GameSettings.instance.GetCurrentItem()[1]);
            gameController.boardItemsControllers[2, j].EnableActive();
        }
    }
}
