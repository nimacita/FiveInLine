using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

[System.Serializable]
public struct item
{
    public int id;
    public Sprite sprite;
    public Color particlesColor;
}

public class GameController : MonoBehaviour
{
    [Header("Chips Settings")]
    [Tooltip("����������� ���������� ����� ������ ��� ���� ����� �������")]
    [SerializeField] private int minChipToPop = 3;

    [Header("Chips Line")]
    [SerializeField] private GameObject chipsLine;
    private ChipsLine lineController;
    [SerializeField] private GameObject movedChipsLine;
    private Vector3 startMovedPosition;
    private int currRotAngle;

    [Header("Bomb Bonus")]
    [SerializeField] private GameObject bombVisual;
    [SerializeField] private GameObject bombCountBg;
    [SerializeField] private TMPro.TMP_Text bombCountTxt;
    private int currentBombCount = 0;
    [SerializeField] private GameObject bombPlusBg;
    [SerializeField] private GameObject movedBomb;

    [Header("Lightning Bonus")]
    [SerializeField] private GameObject lightningVisual;
    [SerializeField] private GameObject lightningCountBg;
    [SerializeField] private TMPro.TMP_Text lightningCountTxt;
    private int currentLightningCount = 0;
    [SerializeField] private GameObject lightningPlusBg;
    [SerializeField] private GameObject movedLight;

    [Header("Swap Bonus")]
    [SerializeField] private Button swapBonusBtn;
    [SerializeField] private GameObject swapVisual;
    [SerializeField] private GameObject swapCountBg;
    [SerializeField] private TMPro.TMP_Text swapCountTxt;
    private int currentSwapCount = 0;
    private int currSwapRot = 0;
    [SerializeField] private GameObject swapPlusBg;
    [SerializeField] private GameObject movedSwap;
    [SerializeField] private GameObject swapUpDirPanel;

    [Header("Editor")]
    [SerializeField] private GameObject upRotPanel;

    [Header("Board Settings")]
    [SerializeField] private GameObject boardPanel;
    private int rowCount = 10;
    private int columnCount = 10;
    private GameObject[,] boardItems;
    public BoardItem[,] boardItemsControllers;
    private List<GameObject> selectedSells;
    private List<GameObject> bombSelected;
    private List<GameObject> lightningSelected;
    private List<GameObject> swapSelected;

    [Header("Bools")]
    private bool isLineTake = false;
    private bool isBombTake = false;
    private bool isLightningTake = false;
    private bool isSwapTake = false;
    private bool isSwapClicked = false;
    private bool isGameOver = false;
    private bool isPause = false;
    private bool canTake = true;
    private bool firstLinePut = true;

    [Header("Is tutorial")]
    [SerializeField]
    private bool isTutorial = false;


    public static GameController instance;
    public static Action onGameOvered;

    private void OnEnable()
    {
        ViewController.onPauseActived += Paused;
    }

    private void OnDisable()
    {
        ViewController.onPauseActived -= Paused;
    }

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        lineController = chipsLine.GetComponent<ChipsLine>();
        startMovedPosition = movedChipsLine.transform.position;

        bombSelected = new List<GameObject>();
        selectedSells = new List<GameObject>();
        lightningSelected = new List<GameObject>();
        swapSelected = new List<GameObject>();

        swapBonusBtn.onClick.AddListener(SwapClicked);

        StartSettings();
        UpdateUpPanel();
        UpdateChipsLine();
        UpdateMovedLine();
        UpdateBombVisual();
        UpdateLightningVisual();
        UpdateSwapVisual();

        InitializedBoardItems();
    }

    //��������� ���������
    private void StartSettings()
    {
        //������������� ������� ��� ����� � �����
        if (!isTutorial) 
        {
            lineController.SetCurrentItems(GameSettings.instance.GetCurrentItem());
            RandomLine();
        }
    }
    
    void Update()
    {
        UpdateClickUp();

        MoveChipLine();
        MoveBomb();
        MoveLightning();
        MoveSwap();

        DisableAllItems();
    }

    //��������� ������ �� ������ ����
    private void UpdateClickUp()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (isLineTake)
            {
                isLineTake = false;
                //��������� ����������� ������� ������
                UpdateUpPanel();
                //��������� ����������� ����� �����
                UpdateChipsLine();
                //��������� �����������  ����������� �����
                UpdateMovedLine();
                //������ ����� ���� �����
                PutSelectedLine();
            }
            if (isBombTake)
            {
                isBombTake = false;
                UpdateBombVisual();
                //����� �����
                PutBomb();
            }
            if (isLightningTake)
            {
                isLightningTake = false;
                UpdateLightningVisual();
                //������ ������
                PutLightning();
            }
            if (isSwapTake)
            {
                isSwapTake = false;
                isSwapClicked = false;
                UpdateSwapVisual();
                //������ �����
                PutSwap();
            }
        }
    }

    //����������� �����
    private void MoveChipLine()
    {
        if (isLineTake && !isGameOver)
        {
            Vector3 moveVector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            moveVector.z = 0;
            movedChipsLine.transform.position = moveVector;
        }
    }

    //����������� �����
    private void MoveBomb()
    {
        if (isBombTake && !isGameOver)
        {
            Vector3 moveVector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            moveVector.z = 0;
            movedBomb.transform.position = moveVector;
        }
    }

    //����������� ������
    private void MoveLightning()
    {
        if (isLightningTake && !isGameOver)
        {
            Vector3 moveVector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            moveVector.z = 0;
            movedLight.transform.position = moveVector;
        }
    }

    //����������� ����
    private void MoveSwap()
    {
        if (isSwapTake && !isGameOver)
        {
            Vector3 moveVector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            moveVector.z = 0;
            movedSwap.transform.position = moveVector;
        }
    }

    //��������� ����������� ������� ������ ������ ��������
    private void UpdateUpPanel()
    {
        if (!isLineTake)
        {
            upRotPanel.SetActive(true);
        }
        else
        {
            upRotPanel.SetActive(false);
        }
    }

    //��������� ��� ������ �����
    public void UpdateBombVisual()
    {
        currentBombCount = GameSettings.instance.BombCount;
        if (currentBombCount > 0)
        {
            bombCountTxt.text = $"{currentBombCount}";
            bombCountBg.SetActive(true);
            bombPlusBg.SetActive(false);
        }
        else
        {
            bombCountBg.SetActive(false);
            bombPlusBg.SetActive(true);
        }
        bombVisual.SetActive(!isBombTake);
        movedBomb.SetActive(isBombTake);
    }

    //��������� ��� ������ ������
    public void UpdateLightningVisual()
    {
        currentLightningCount = GameSettings.instance.LightningCount;
        if (currentLightningCount > 0)
        {
            lightningCountTxt.text = $"{currentLightningCount}";
            lightningCountBg.SetActive(true);
            lightningPlusBg.SetActive(false);
        }
        else
        {
            lightningCountBg.SetActive(false);
            lightningPlusBg.SetActive(true);
        }
        lightningVisual.SetActive(!isLightningTake);
        movedLight.SetActive(isLightningTake);
    }

    //��������� ��� ������ ������
    public void UpdateSwapVisual()
    {
        currentSwapCount = GameSettings.instance.SwapCount;
        if (currentSwapCount > 0)
        {
            swapCountTxt.text = $"{currentSwapCount}";
            swapCountBg.SetActive(true);
            swapPlusBg.SetActive(false);
        }
        else
        {
            swapCountBg.SetActive(false);
            swapPlusBg.SetActive(true);
        }
        swapVisual.SetActive(!isSwapTake);
        movedSwap.SetActive(isSwapTake);

        swapUpDirPanel.SetActive(isSwapClicked);
    }

    //��������� ����������� ����� 
    private void UpdateChipsLine()
    {
        if (!isLineTake)
        {
            chipsLine.SetActive(true);
        }
        else
        {
            chipsLine.SetActive(false);
        }
    }

    //��������� ����������� ����������� �����
    private void UpdateMovedLine()
    {
        if (isLineTake)
        {
            movedChipsLine.SetActive(true);
        }
        else
        {
            movedChipsLine.SetActive(false);
            movedChipsLine.transform.position = startMovedPosition;
        }
    }

    //���������� ��������� �����
    private void RandomLine()
    {
        if (!isTutorial) 
        {
            //���������� ��������� ����� ���� �� ��������
            lineController.GenerateRandomLine();
        }
    }

    //������� ��������� ����������� �� �������� ����
    public void SelectLineRotation(int rotAngle)
    {
        //���� ����� �����
        if (canTake && !isGameOver) {
            currRotAngle = rotAngle;
            Vector3 rotVector = new Vector3(0, 0, currRotAngle);
            movedChipsLine.GetComponent<RectTransform>().rotation = Quaternion.Euler(rotVector);
            isLineTake = true;
            UpdateUpPanel();
            UpdateChipsLine();
            UpdateMovedLine();
        }
    }

    //�������� �����
    public void SelectedBomb()
    {
        if (currentBombCount > 0 && !isGameOver)
        {
            //��������
            isBombTake = true;
        }
        else
        {
            //���������� ������
            ViewController.instance.BombBusterClick();
        }
        UpdateBombVisual();
    }

    //�������� ������
    public void SelectedLightning()
    {
        if (currentLightningCount > 0 && !isGameOver)
        {
            //��������
            isLightningTake = true;
        }
        else
        {
            //���������� ������
            ViewController.instance.LightningBusterClick();
        }
        UpdateLightningVisual();
    }

    //�������� �� ����
    private void SwapClicked()
    {
        if (currentSwapCount > 0 && !isGameOver)
        {
            isSwapClicked = !isSwapClicked;
            UpdateSwapVisual();
        }
        else
        {
            //���������� ������
            ViewController.instance.SwapBusterClick();
        }

    }

    //�������� ����
    public void SelectedSwap(int rotAngle)
    {
        //���������� ����
        currSwapRot = rotAngle;
        //��������
        isSwapTake = true;
        UpdateSwapVisual();
    }

    //�������������� ��� �������� �� �����
    private void InitializedBoardItems()
    {
        boardItems = new GameObject[rowCount, rowCount];
        boardItemsControllers = new BoardItem[rowCount, columnCount];

        int itemInd = 0;
        for (int i = 0; i <rowCount; i++)
        {
            for (int j = 0; j < columnCount; j++)
            {
                boardItems[i, j] = boardPanel.transform.GetChild(itemInd).gameObject;
                boardItemsControllers[i, j] = boardItems[i, j].GetComponent<BoardItem>();
                boardItemsControllers[i, j].SetCellCoord(i, j);

                itemInd++;
            }
        }
    }

    //������ ���������� ��������� �� ����, ���� ����� ��������� �����, ��� �����������
    //�������� ���������� ����������� ����� � �����
    public void GetSellectedChip(int cellI, int cellJ)
    {
        if (selectedSells.Count > 0)
        {
            //���� ��� ������ ��� ���������, �� �������
            if (selectedSells[2].GetComponent<BoardItem>().GetCoord().x == cellI &&
                selectedSells[2].GetComponent<BoardItem>().GetCoord().y == cellJ)
            {
                return;
            }
        }
        //���� ������� ��� �������, �� �������
        if (boardItemsControllers[cellI,cellJ].GetActive())
        {
            return;
        }
        DisableTransparentLine();
        //���� ����� � �����, �� ������������ ��������������� ������ � ����������� �� ��������� �����
        if (isLineTake)
        {
            switch (currRotAngle)
            {
                case 0:
                    //�������������� ����� �������
                    TransparentHorizLine(cellI, cellJ);
                    break;
                case -90:
                    //������������ ������ ����
                    TransparentVerticalLine(cellI, cellJ);
                    break;
                case 45:
                    //������������ ����� �����
                    TransparentDiagBotToUpLine(cellI, cellJ);
                    break;
                case -45:
                    //������������ ������ ����
                    TransparentDiagUpToBotLine(cellI, cellJ);
                    break;
            }
        }
    }

    //�������� ���������� ���������� �������, � ���� ����� � ����� �� ������������ ������� ��� �����������
    public void GetSellectedBombChip(int cellI, int cellJ)
    {
        if (isBombTake)
        {
            EnableBombSelected(false);
            //���� ����� � ����� �� ������������ ������� �� ������
            SelectedBombSquare(cellI, cellJ);
        }

    }

    //�������� ���������� ���������� �������, � ���� ������ � ����� �� ������������ ������� ��� �����������
    public void GetSellectedLightningChip(int cellI, int cellJ)
    {
        EnableLightningSelected(false);
        //���� ������ � ����� �� ������������
        if (isLightningTake)
        {
            SelectedLightChips(cellI, cellJ);
        }
    }

    //�������� ���������� ���������� �������, � ���� ���� � ����� �� ������������ ������� ��� �����������
    public void GetSellectedSwapChip(int cellI, int cellJ)
    {
        EnableSwapSelected(false);
        //���� ������ � ����� �� ������������
        if (isSwapTake)
        {
            SelectedSwapChips(cellI, cellJ);
        }
    }

    //�������� ���������� �������������� �����
    private void TransparentHorizLine(int cellI, int cellJ)
    {
        //���� ����� ������ ����������, ��� ��� ������� �� ������� ����, �� �������
        if (cellJ > columnCount - 3 || cellJ < 2) 
        {
            //�������
            selectedSells.Clear();
            return;
        }
        //���� ����� ���������� ������ ��� ��� ���� ����������� ������ �� ����, �� �������
        for (int j = cellJ - 2; j <= cellJ + 2; j++) 
        {
            if (boardItemsControllers[cellI,j].GetActive())
            {
                return;
            }
        }
        //���� ����� ��������� �� �������� ���������

        //���� �����
        AddItem(selectedSells, boardItems[cellI, cellJ - 2]);
        AddItem(selectedSells, boardItems[cellI, cellJ - 1]);
        //��������� ����������
        AddItem(selectedSells, boardItems[cellI, cellJ]);
        //���� ������
        AddItem(selectedSells, boardItems[cellI, cellJ + 1]);
        AddItem(selectedSells, boardItems[cellI, cellJ + 2]);

        //������������
        EnableTransparentLine();
    }

    //�������� ���������� ������������ �����
    private void TransparentVerticalLine(int cellI, int cellJ)
    {
        //���� ����� ������ ����������, ��� ��� ������� �� ������� ����, �� �������
        if (cellI > rowCount - 3 || cellI < 2)
        {
            //�������
            selectedSells.Clear();
            return;
        }
        //���� ����� ���������� ������ ��� ��� ���� ����������� ������ �� ����, �� �������
        for (int i = cellI - 2; i <= cellI + 2; i++)
        {
            if (boardItemsControllers[i, cellJ].GetActive())
            {
                return;
            }
        }
        //���� ����� ��������� �� �������� ���������

        //���� ������
        AddItem(selectedSells, boardItems[cellI - 2, cellJ]);
        AddItem(selectedSells, boardItems[cellI - 1, cellJ]);
        //��������� ����������
        AddItem(selectedSells, boardItems[cellI, cellJ]);
        //���� ����
        AddItem(selectedSells, boardItems[cellI + 1, cellJ]);
        AddItem(selectedSells, boardItems[cellI + 2, cellJ]);

        //������������
        EnableTransparentLine();
    }

    //�������� ���������� ������������ ����� ����� ����� - ����� ������� (��� � ������ ������ ��������� ���������� ������� �� ��������)
    private void TransparentDiagBotToUpLine(int cellI, int cellJ)
    {
        //���� ����� ������ ����������, ��� ��� ������� �� ������� ����, �� �������
        if (cellI > rowCount - 3 || cellI < 2 || cellJ > columnCount - 3 || cellJ < 2)
        {
            //�������
            selectedSells.Clear();
            return;
        }
        //���� ����� ���������� ������ ��� ��� ���� ����������� ������ �� ����, �� �������
        for (int j = cellJ -2, i = cellI + 2; i >= cellI - 2; i--, j++)
        {

            if (boardItemsControllers[i, j].GetActive())
            {
                return;
            }
        }
        //���� ����� ��������� �� �������� ���������

        //���� ����� - �����
        AddItem(selectedSells, boardItems[cellI + 2, cellJ - 2]);
        AddItem(selectedSells, boardItems[cellI + 1, cellJ - 1]);
        //��������� ����������
        AddItem(selectedSells, boardItems[cellI, cellJ]);
        //���� ����� - ������
        AddItem(selectedSells, boardItems[cellI - 1, cellJ + 1]);
        AddItem(selectedSells, boardItems[cellI - 2, cellJ + 2]);

        //������������
        EnableTransparentLine();
    }

    //�������� ���������� ������������ ����� ����� ���� - ����� �������
    private void TransparentDiagUpToBotLine(int cellI, int cellJ)
    {
        //���� ����� ������ ����������, ��� ��� ������� �� ������� ����, �� �������
        if (cellI > rowCount - 3 || cellI < 2 || cellJ > columnCount - 3 || cellJ < 2)
        {
            //�������
            selectedSells.Clear();
            return;
        }
        //���� ����� ���������� ������ ��� ��� ���� ����������� ������ �� ����, �� �������
        for (int j = cellJ - 2, i = cellI - 2; i <= cellI + 2; i++, j++)
        {
            if (boardItemsControllers[i, j].GetActive())
            {
                return;
            }
        }
        //���� ����� ��������� �� �������� ���������

        //���� ����� - �����
        AddItem(selectedSells, boardItems[cellI - 2, cellJ - 2]);
        AddItem(selectedSells, boardItems[cellI - 1, cellJ - 1]);
        //��������� ����������
        AddItem(selectedSells, boardItems[cellI, cellJ]);
        //���� ����� - ������
        AddItem(selectedSells, boardItems[cellI + 1, cellJ + 1]);
        AddItem(selectedSells, boardItems[cellI + 2, cellJ + 2]);

        //������������
        EnableTransparentLine();
    }

    //�������� ������� �����
    private void SelectedBombSquare(int cellI, int cellJ)
    {
        //���� �� ���� ������ ������ ����
        for (int currI = cellI - 1;currI <= cellI + 1; currI ++)
        {
            if (currI >= 0 && currI < rowCount)
            {
                for (int currJ = cellJ - 1; currJ <= cellJ + 1; currJ++)
                {
                    if (currJ >= 0 && currJ < columnCount)
                    {
                        AddItem(bombSelected, boardItems[currI, currJ]);
                    }
                }
            }
        }

        //������������
        EnableBombSelected(true);
    }

    //�������� ��� �������� ��� ������
    public void SelectedLightChips(int cellI, int cellJ)
    {
        //��������� ��� ���������� ��������
        int currChipId = boardItemsControllers[cellI, cellJ].GetCurrItem().id;

        //���������� �� ���� ��������� � �������� �������� �������� � ����� �� ����
        for (int i = 0; i < rowCount; i++) 
        {
            for (int j = 0; j < columnCount; j++) 
            {
                if (boardItemsControllers[i,j].GetActive() && 
                    boardItemsControllers[i, j].GetCurrItem().id == currChipId)
                {
                    AddItem(lightningSelected, boardItems[i, j]);
                }
            }
        }

        //������������
        EnableLightningSelected(true);
    }

    //�������� ��� �������� ��� �����
    private void SelectedSwapChips(int cellI, int cellJ)
    {
        //���� ����� �� ������� ������� �� ������
        if ((currSwapRot == 0 && cellJ == columnCount - 1) ||
            (currSwapRot == 180 && cellI == 0)) 
        {
            swapSelected.Clear();
            return;
        }

        //��������� ������ ������
        AddItem(swapSelected, boardItems[cellI, cellJ]);
        //��������� ������ ������� � ����������� �� ���������� ��������
        if (currSwapRot == 0)
        {
            //������ �������
            AddItem(swapSelected, boardItems[cellI, cellJ + 1]);
        }
        else
        {
            //������� �������
            AddItem(swapSelected, boardItems[cellI - 1, cellJ]);
        }
        //���� ��� ��������� ������� �� ������� �� �������
        if (!swapSelected[0].GetComponent<BoardItem>().GetActive() &&
            !swapSelected[1].GetComponent<BoardItem>().GetActive())
        {
            EnableSwapSelected(false);
            return;
        }

        //������������
        EnableSwapSelected(true);
    }

    //��������� ������� � ���� ���� ��� ������
    private void AddItem(List<GameObject> currList, GameObject itm)
    {
        for (int i = 0;i<currList.Count;i++)
        {
            //���� ����� ���� �� �������
            if (itm == currList[i])
            {
                return;
            }
        }
        currList.Add(itm);
    }

    //������������ ���������� ���������� �����
    private void EnableTransparentLine()
    {
        if (selectedSells.Count > 0)
        {
            for (int i = 0; i < selectedSells.Count; i++) 
            {
                selectedSells[i].GetComponent<BoardItem>().SetChipItem(lineController.GetChipItem(i));
                selectedSells[i].GetComponent<BoardItem>().EnableTransparent(true);
            }
        }
    }

    //������� ������������ ���������� �����
    private void DisableTransparentLine()
    {
        if (selectedSells.Count > 0)
        {
            for (int i = 0; i < selectedSells.Count; i++)
            {
                selectedSells[i].GetComponent<BoardItem>().EnableTransparent(false);
            }
            selectedSells.Clear();
        }
    }

    //������� �����
    private void EnableBombSelected(bool value)
    {
        if (bombSelected.Count > 0)
        {
            for (int i = 0; i < bombSelected.Count; i++)
            {
                bombSelected[i].GetComponent<BoardItem>().EnableRedSelected(value);
            }
            if (!value)
            {
                bombSelected.Clear();
            }
        }
    }

    //������� ������
    private void EnableLightningSelected(bool value)
    {
        if (lightningSelected.Count > 0)
        {
            for (int i = 0; i < lightningSelected.Count; i++)
            {
                lightningSelected[i].GetComponent<BoardItem>().EnableRedSelected(value);
            }
            if (!value)
            {
                lightningSelected.Clear();
            }
        }
    }

    //������� ������
    private void EnableSwapSelected(bool value)
    {
        if (swapSelected.Count > 0)
        {
            //������ �������
            swapSelected[0].GetComponent<BoardItem>().EnableRedSelected(value);
            //������ �������
            swapSelected[1].GetComponent<BoardItem>().EnableGreenSelected(value);
            if (!value)
            {
                swapSelected.Clear();
            }
        }
    }

    //������ ������ �� ��������� �������
    private void PutSelectedLine()
    {
        if (selectedSells.Count > 0 && !isGameOver)
        {
            for (int i = 0; i < selectedSells.Count; i++)
            {
                selectedSells[i].GetComponent<BoardItem>().EnableActive();
            }
            RandomLine();
            //��������� ������������ ����� �� ����������
            for (int i = 0; i < selectedSells.Count; i++)
            {
                FindMatch((int)selectedSells[i].GetComponent<BoardItem>().GetCoord().x,
                    (int)selectedSells[i].GetComponent<BoardItem>().GetCoord().y);
            }
            DisableTransparentLine();

            //Play Sound
            SoundController.instance.PlayPutLineSound();

            //���� ��������� � ������ ���, �� ��������� ������
            if (firstLinePut)
            {
                ViewController.instance.StartTimer();
                firstLinePut = false;
            }
            //������� ���
            ViewController.instance.StepDone();
            if (isTutorial)
            {
                TutorialController.instance.OffPointer();
            }
        }
    }

    //������ �����
    private void PutBomb()
    {
        if (bombSelected.Count > 0 && !isGameOver)
        {
            for (int i = 0;i<bombSelected.Count;i++)
            {
                //���� ������� �� ������
                if (bombSelected[i].GetComponent<BoardItem>().GetActive())
                {
                    bombSelected[i].GetComponent<BoardItem>().PopItem();
                }
            }
            //�������� �� ���������� ����
            GameSettings.instance.BombCount--;

            //Play Sound
            SoundController.instance.PlayBombSound();

            ViewController.instance.PlusBombUsed();
            EnableBombSelected(false);
            UpdateBombVisual();
        }
    }

    //������ ������
    private void PutLightning()
    {
        if (lightningSelected.Count > 0 && !isGameOver)
        {
            for (int i = 0; i < lightningSelected.Count; i++)
            {
                //���� ������� �� ������
                if (lightningSelected[i].GetComponent<BoardItem>().GetActive())
                {
                    lightningSelected[i].GetComponent<BoardItem>().PopItem();
                }
            }
            //�������� �� ����������
            GameSettings.instance.LightningCount--;

            //Play Sound
            SoundController.instance.PlayLightningSound();

            ViewController.instance.PlusLightningUsed();
            EnableLightningSelected(false);
            UpdateLightningVisual();
        }
    }

    //������ ����
    private void PutSwap()
    {
        if (swapSelected.Count > 0 && !isGameOver)
        {
            //������ �������
            SwapChips();

            //�������� �� ����������
            GameSettings.instance.SwapCount--;

            //Play Sound
            SoundController.instance.PlaySwapSound();

            ViewController.instance.PlusSwapUsed();
            EnableSwapSelected(false);
            UpdateSwapVisual();
        }
    }

    //������ �������� �������
    private void SwapChips()
    {
        BoardItem swapChip1 = swapSelected[0].GetComponent<BoardItem>();
        BoardItem swapChip2 = swapSelected[1].GetComponent<BoardItem>();

        item tmp = swapChip1.GetCurrItem();
        bool tmpActive = swapChip1.GetActive();

        //������ �� ������
        if (swapChip2.GetActive())
        {
            //���� ������ �� ��������
            swapChip1.SwapChipItem(swapChip2.GetCurrItem());
            swapChip1.EnableActive();
        }
        else
        {
            //���� ������ �� ����������
            swapChip1.SwapChipItem(swapChip2.GetCurrItem());
            swapChip1.DisableActive();
            swapChip1.SetNullItem();
        }
        //������ ������ �� ������
        if (tmpActive)
        {
            //���� ������ �� ��������
            swapChip2.SwapChipItem(tmp);
            swapChip2.EnableActive();
        }
        else
        {
            //���� ������ �� ����������
            swapChip2.SwapChipItem(tmp);
            swapChip2.DisableActive();
            swapChip2.SetNullItem();
        }

        //������� ���� �� �������� � �������� ����� �������
        FindMatch((int)swapChip1.GetCoord().x, (int)swapChip1.GetCoord().y);
        FindMatch((int)swapChip2.GetCoord().x, (int)swapChip2.GetCoord().y);

        EnableSwapSelected(false);
    }

    //���� �� ���� ������ �� ���������
    private void DisableAllItems()
    {
        for (int i = 0;i<rowCount;i++)
        {
            for (int j = 0;j<columnCount;j++)
            {
                if (boardItemsControllers[i,j].GetSelected())
                {
                    return;
                }
            }
        }
        DisableTransparentLine();
        EnableBombSelected(false);
        EnableLightningSelected(false);
        EnableSwapSelected(false);
    }


    //�������� �� ���������� ��������
    //������� �� ������������ �����
    public void FindMatch(int currI, int currJ)
    {
        if (!isGameOver)
        {
            List<GameObject> matchItems = FindAllMatchItems(currI, currJ);
            //���� ���������� �������� ���������� ��� ��������
            if (matchItems.Count >= minChipToPop)
            {
                StartCoroutine(DisableMatchItems(matchItems));
                //PlaySound(currentPopSound);
                //PlusScore(matchItems.Count);
                ViewController.instance.AddScore(matchItems.Count);
                if (Quests.instance.QuestSelected && Quests.instance.QuestSelectedCollectChipsOnce &&
                    !GameSettings.instance.IsTutorial)
                {
                    Quests.instance.IsCollectedOnceChips(matchItems.Count);
                }
            }
            DeselectedToPop();
        }
    }

    //��������� ��� ���������� �������
    private IEnumerator DisableMatchItems(List<GameObject> matchItems)
    {
        canTake = false;
        //������ ��������
        for (int i = 0; i < matchItems.Count; i++)
        {
            matchItems[i].GetComponent<BoardItem>().PlayPopAnim();
        
        }

        //Play Sound
        SoundController.instance.PlayPopSound();

        yield return new WaitForSeconds(0.12f);
        for (int i = 0; i < matchItems.Count; i++)
        {
            matchItems[i].GetComponent<BoardItem>().PopItem();
        }
        StartCoroutine(TakeWait());
    }

    //������� ��� ����������� ������� ������ �����
    private List<GameObject> FindAllMatchItems(int currI, int currJ)
    {
        List<GameObject> allMatchItems = new List<GameObject>();
        allMatchItems.Add(boardItems[currI, currJ]);
        boardItemsControllers[currI, currJ].SelectedToPop = true;

        allMatchItems.AddRange(CurrentMatchItems(currI, currJ));
        if (allMatchItems.Count < minChipToPop)
        {
            allMatchItems.Clear();
        }
        DeleteSameItems(allMatchItems);
        return allMatchItems;
    }

    //������� ���������� ��������
    private List<GameObject> DeleteSameItems(List<GameObject> list)
    {
        for (int i = 0; i < list.Count; i++) 
        {
            for (int j = i + 1; j < list.Count; j++) 
            {
                if (list[i] == list[j])
                {
                    list.RemoveAt(j);
                    list = DeleteSameItems(list);
                    return list;
                }
            }
        }
        return list;
    }

    //����� ��� ������������ ����������� ���� ����������� ��������
    private List<GameObject> CurrentMatchItems(int currI, int currJ)
    {
        List<GameObject> currentMatchItems = new List<GameObject>();
        currentMatchItems = GetMatchAroundItems(currI, currJ);
        if (currentMatchItems.Count != 0)
        {
            for (int i = 0; i < currentMatchItems.Count; i++)
            {
                currentMatchItems.AddRange(CurrentMatchItems(
                    (int)currentMatchItems[i].GetComponent<BoardItem>().GetCoord().x,
                    (int)currentMatchItems[i].GetComponent<BoardItem>().GetCoord().y));
            }
        }
        return currentMatchItems;
    }

    //������� ��� ������� ������ ����� ������ ����������
    private List<GameObject> GetMatchAroundItems(int currI, int currJ)
    {
        List<GameObject> matchItems = new List<GameObject>();
        if (!boardItemsControllers[currI, currJ].SelectedToPop)
        {
            matchItems.Add(boardItems[currI, currJ]);
            boardItemsControllers[currI, currJ].SelectedToPop = true;
        }
        //�������
        if (currI > 0)
        {
            //��������� ������������� ������
            int compI = currI - 1;
            int compJ = currJ;
            if (IsMatchColors(compI, compJ, currI, currJ))
            {
                //���� ����� ��������� �� ��������� � ������
                matchItems.Add(boardItems[compI, compJ]);
                boardItemsControllers[currI, currJ].SelectedToPop = true;
            }
        }
        //������
        if (currI < rowCount - 1)
        {
            //��������� ������������� ������
            int compI = currI + 1;
            int compJ = currJ;
            if (IsMatchColors(compI, compJ, currI, currJ))
            {
                //���� ����� ��������� �� ��������� � ������
                matchItems.Add(boardItems[compI, compJ]);
                boardItemsControllers[currI, currJ].SelectedToPop = true;
            }
        }
        //������
        if (currJ < columnCount - 1)
        {
            //��������� ������������� ������
            int compI = currI;
            int compJ = currJ + 1;
            if (IsMatchColors(compI, compJ, currI, currJ))
            {
                //���� ����� ��������� �� ��������� � ������
                matchItems.Add(boardItems[compI, compJ]);
                boardItemsControllers[currI, currJ].SelectedToPop = true;
            }
        }
        //�����
        if (currJ > 0)
        {
            //��������� ������������� ������
            int compI = currI;
            int compJ = currJ - 1;
            if (IsMatchColors(compI, compJ, currI, currJ))
            {
                //���� ����� ��������� �� ��������� � ������
                matchItems.Add(boardItems[compI, compJ]);
                boardItemsControllers[currI, currJ].SelectedToPop = true;
            }
        }
        //���������� ������ ������ � ����� ������
        return matchItems;
    }

    //�������� �� ���������� ����
    private bool IsMatchColors(int compI, int compJ, int mainI, int mainJ)
    {
        //���� ���������� ����� � ��� ��������� ������� � ������������� �������� �� ������� ���
        if (boardItemsControllers[compI, compJ].GetCurrItem().id ==
            boardItemsControllers[mainI, mainJ].GetCurrItem().id
            && (boardItemsControllers[compI, compJ].GetActive() && boardItemsControllers[mainI, mainJ].GetActive())
            && !boardItemsControllers[compI, compJ].SelectedToPop
            && boardItemsControllers[compI, compJ].GetCurrItem().id != -1
            && !boardItemsControllers[compI, compJ].ReadyToPop)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //�������� �� �� ����� ����� ����� ����� �������
    IEnumerator TakeWait()
    {
        canTake = false;
        yield return new WaitForSeconds(0.5f);
        canTake = true;
    }

    //������� ��������� � ����������
    private void DeselectedToPop()
    {
        for (int i = 0; i < rowCount; i++) 
        {
            for (int j = 0; j < columnCount; j++) 
            {
                if (boardItemsControllers[i, j].SelectedToPop) boardItemsControllers[i, j].SelectedToPop = false;
            }
        }
    }

    public void GameOver()
    {
        isGameOver = true;
        isLineTake = false;
        onGameOvered?.Invoke();
        //��������� ����������� ������� ������
        UpdateUpPanel();
        //��������� ����������� ����� �����
        UpdateChipsLine();
        //��������� �����������  ����������� �����
        UpdateMovedLine();
    }

    private void Paused(bool value)
    {
        isPause = value;
    }

    public int GetRowCount()
    {
        return rowCount;
    }

    public int GetColumnCount()
    {
        return columnCount;
    }
}
