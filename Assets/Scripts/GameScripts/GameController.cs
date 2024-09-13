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
    [Tooltip("Минимальное количество фишек подряд для того чтобы лопнуть")]
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

    //начальные настройки
    private void StartSettings()
    {
        //устанавливаем текущий тип фишек в линию
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

    //обновляем зажата ли кнопка мыши
    private void UpdateClickUp()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (isLineTake)
            {
                isLineTake = false;
                //обновляем отображение верхней панели
                UpdateUpPanel();
                //обновляем отображение линии фишек
                UpdateChipsLine();
                //обновляем отображение  двигающейся линии
                UpdateMovedLine();
                //ставим фишки если можем
                PutSelectedLine();
            }
            if (isBombTake)
            {
                isBombTake = false;
                UpdateBombVisual();
                //ставм бомбу
                PutBomb();
            }
            if (isLightningTake)
            {
                isLightningTake = false;
                UpdateLightningVisual();
                //ставим молнию
                PutLightning();
            }
            if (isSwapTake)
            {
                isSwapTake = false;
                isSwapClicked = false;
                UpdateSwapVisual();
                //ставми бонус
                PutSwap();
            }
        }
    }

    //передвигаем линию
    private void MoveChipLine()
    {
        if (isLineTake && !isGameOver)
        {
            Vector3 moveVector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            moveVector.z = 0;
            movedChipsLine.transform.position = moveVector;
        }
    }

    //передвигаем бомбу
    private void MoveBomb()
    {
        if (isBombTake && !isGameOver)
        {
            Vector3 moveVector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            moveVector.z = 0;
            movedBomb.transform.position = moveVector;
        }
    }

    //передвигаем молнию
    private void MoveLightning()
    {
        if (isLightningTake && !isGameOver)
        {
            Vector3 moveVector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            moveVector.z = 0;
            movedLight.transform.position = moveVector;
        }
    }

    //передвигаем свап
    private void MoveSwap()
    {
        if (isSwapTake && !isGameOver)
        {
            Vector3 moveVector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            moveVector.z = 0;
            movedSwap.transform.position = moveVector;
        }
    }

    //обновляем отображение верхней панели выбора поворота
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

    //обновляем вид бонуса бомаы
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

    //обновляем вид бонуса молнии
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

    //обновляем вид бонуса замены
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

    //обновляем отображение линии 
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

    //обновляем отображение двигающейся линии
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

    //генерируем случайную линию
    private void RandomLine()
    {
        if (!isTutorial) 
        {
            //генерируем случайную линию если не туториал
            lineController.GenerateRandomLine();
        }
    }

    //выбрали положение двигающейся за курсором лнии
    public void SelectLineRotation(int rotAngle)
    {
        //если можем брать
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

    //выбираем бомбу
    public void SelectedBomb()
    {
        if (currentBombCount > 0 && !isGameOver)
        {
            //выбираем
            isBombTake = true;
        }
        else
        {
            //предлагаем купить
            ViewController.instance.BombBusterClick();
        }
        UpdateBombVisual();
    }

    //выбираем молнию
    public void SelectedLightning()
    {
        if (currentLightningCount > 0 && !isGameOver)
        {
            //выбираем
            isLightningTake = true;
        }
        else
        {
            //предлагаем купить
            ViewController.instance.LightningBusterClick();
        }
        UpdateLightningVisual();
    }

    //нажимаем на свап
    private void SwapClicked()
    {
        if (currentSwapCount > 0 && !isGameOver)
        {
            isSwapClicked = !isSwapClicked;
            UpdateSwapVisual();
        }
        else
        {
            //предлагаем купить
            ViewController.instance.SwapBusterClick();
        }

    }

    //выбираем свап
    public void SelectedSwap(int rotAngle)
    {
        //запоминаем угол
        currSwapRot = rotAngle;
        //выбираем
        isSwapTake = true;
        UpdateSwapVisual();
    }

    //инициализируем все элементы на доске
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

    //рисуем прозрачные эллементы на поле, если можем поставить линию, для наглядности
    //получаем коориднаты центральной точки в линии
    public void GetSellectedChip(int cellI, int cellJ)
    {
        if (selectedSells.Count > 0)
        {
            //если эти клетки уже выделенны, то выходим
            if (selectedSells[2].GetComponent<BoardItem>().GetCoord().x == cellI &&
                selectedSells[2].GetComponent<BoardItem>().GetCoord().y == cellJ)
            {
                return;
            }
        }
        //если элемент уже активен, то выходим
        if (boardItemsControllers[cellI,cellJ].GetActive())
        {
            return;
        }
        DisableTransparentLine();
        //если линия в руках, то отрисовываем задействованные клетки в зависимости от положения линии
        if (isLineTake)
        {
            switch (currRotAngle)
            {
                case 0:
                    //горизонтальная слева направо
                    TransparentHorizLine(cellI, cellJ);
                    break;
                case -90:
                    //вертикальная сверху вниз
                    TransparentVerticalLine(cellI, cellJ);
                    break;
                case 45:
                    //диагональная снизу вверх
                    TransparentDiagBotToUpLine(cellI, cellJ);
                    break;
                case -45:
                    //диагональная сверху вниз
                    TransparentDiagUpToBotLine(cellI, cellJ);
                    break;
            }
        }
    }

    //получаем координаты выделенной клектки, и если бомба в руках то отрисовываем область для наглядности
    public void GetSellectedBombChip(int cellI, int cellJ)
    {
        if (isBombTake)
        {
            EnableBombSelected(false);
            //есди бомюа в руках то отрисовываем область ее взрыва
            SelectedBombSquare(cellI, cellJ);
        }

    }

    //получаем координаты выделенной клектки, и если молния в руках то отрисовываем область для наглядности
    public void GetSellectedLightningChip(int cellI, int cellJ)
    {
        EnableLightningSelected(false);
        //если молний в руках то отрисовываем
        if (isLightningTake)
        {
            SelectedLightChips(cellI, cellJ);
        }
    }

    //получаем координаты выделенной клектки, и если свап в руках то отрисовываем область для наглядности
    public void GetSellectedSwapChip(int cellI, int cellJ)
    {
        EnableSwapSelected(false);
        //если молний в руках то отрисовываем
        if (isSwapTake)
        {
            SelectedSwapChips(cellI, cellJ);
        }
    }

    //выделяем прозрачную горизонтальную линию
    private void TransparentHorizLine(int cellI, int cellJ)
    {
        //если линию нельзя нарисовать, так как выходит за пределы поля, то выходим
        if (cellJ > columnCount - 3 || cellJ < 2) 
        {
            //выходим
            selectedSells.Clear();
            return;
        }
        //если линию нарисовать нельзя так как есть несвободные клетки на пути, то выходим
        for (int j = cellJ - 2; j <= cellJ + 2; j++) 
        {
            if (boardItemsControllers[cellI,j].GetActive())
            {
                return;
            }
        }
        //если можно поставить то начинаем заполнять

        //идем слева
        AddItem(selectedSells, boardItems[cellI, cellJ - 2]);
        AddItem(selectedSells, boardItems[cellI, cellJ - 1]);
        //добавляем серединную
        AddItem(selectedSells, boardItems[cellI, cellJ]);
        //идем вправо
        AddItem(selectedSells, boardItems[cellI, cellJ + 1]);
        AddItem(selectedSells, boardItems[cellI, cellJ + 2]);

        //отрисовываем
        EnableTransparentLine();
    }

    //выделяем прозрачную вертикальную линию
    private void TransparentVerticalLine(int cellI, int cellJ)
    {
        //если линию нельзя нарисовать, так как выходит за пределы поля, то выходим
        if (cellI > rowCount - 3 || cellI < 2)
        {
            //выходим
            selectedSells.Clear();
            return;
        }
        //если линию нарисовать нельзя так как есть несвободные клетки на пути, то выходим
        for (int i = cellI - 2; i <= cellI + 2; i++)
        {
            if (boardItemsControllers[i, cellJ].GetActive())
            {
                return;
            }
        }
        //если можно поставить то начинаем заполнять

        //идем сверху
        AddItem(selectedSells, boardItems[cellI - 2, cellJ]);
        AddItem(selectedSells, boardItems[cellI - 1, cellJ]);
        //добавляем серединную
        AddItem(selectedSells, boardItems[cellI, cellJ]);
        //идем вниз
        AddItem(selectedSells, boardItems[cellI + 1, cellJ]);
        AddItem(selectedSells, boardItems[cellI + 2, cellJ]);

        //отрисовываем
        EnableTransparentLine();
    }

    //выделяем прозрачную диагональную линию снизу вверх - слева направо (баг с линией рисует активными элементами проводя по активным)
    private void TransparentDiagBotToUpLine(int cellI, int cellJ)
    {
        //если линию нельзя нарисовать, так как выходит за пределы поля, то выходим
        if (cellI > rowCount - 3 || cellI < 2 || cellJ > columnCount - 3 || cellJ < 2)
        {
            //выходим
            selectedSells.Clear();
            return;
        }
        //если линию нарисовать нельзя так как есть несвободные клетки на пути, то выходим
        for (int j = cellJ -2, i = cellI + 2; i >= cellI - 2; i--, j++)
        {

            if (boardItemsControllers[i, j].GetActive())
            {
                return;
            }
        }
        //если можно поставить то начинаем заполнять

        //идем снизу - слева
        AddItem(selectedSells, boardItems[cellI + 2, cellJ - 2]);
        AddItem(selectedSells, boardItems[cellI + 1, cellJ - 1]);
        //добавляем серединную
        AddItem(selectedSells, boardItems[cellI, cellJ]);
        //идем вверх - вправо
        AddItem(selectedSells, boardItems[cellI - 1, cellJ + 1]);
        AddItem(selectedSells, boardItems[cellI - 2, cellJ + 2]);

        //отрисовываем
        EnableTransparentLine();
    }

    //выделяем прозрачную диагональную линию верху вниз - слева направо
    private void TransparentDiagUpToBotLine(int cellI, int cellJ)
    {
        //если линию нельзя нарисовать, так как выходит за пределы поля, то выходим
        if (cellI > rowCount - 3 || cellI < 2 || cellJ > columnCount - 3 || cellJ < 2)
        {
            //выходим
            selectedSells.Clear();
            return;
        }
        //если линию нарисовать нельзя так как есть несвободные клетки на пути, то выходим
        for (int j = cellJ - 2, i = cellI - 2; i <= cellI + 2; i++, j++)
        {
            if (boardItemsControllers[i, j].GetActive())
            {
                return;
            }
        }
        //если можно поставить то начинаем заполнять

        //идем снизу - слева
        AddItem(selectedSells, boardItems[cellI - 2, cellJ - 2]);
        AddItem(selectedSells, boardItems[cellI - 1, cellJ - 1]);
        //добавляем серединную
        AddItem(selectedSells, boardItems[cellI, cellJ]);
        //идем вверх - вправо
        AddItem(selectedSells, boardItems[cellI + 1, cellJ + 1]);
        AddItem(selectedSells, boardItems[cellI + 2, cellJ + 2]);

        //отрисовываем
        EnableTransparentLine();
    }

    //выделяем область бомбы
    private void SelectedBombSquare(int cellI, int cellJ)
    {
        //идем по всем линиям сверху вниз
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

        //отрисовываем
        EnableBombSelected(true);
    }

    //выделаем все элементы для молнии
    public void SelectedLightChips(int cellI, int cellJ)
    {
        //сохраняем тип выбранного элемента
        int currChipId = boardItemsControllers[cellI, cellJ].GetCurrItem().id;

        //проходимся по всем элементам и выбираем активные элементы с таким же айди
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

        //отрисовываем
        EnableLightningSelected(true);
    }

    //выделяем все элементы для свапа
    private void SelectedSwapChips(int cellI, int cellJ)
    {
        //если вышли за пределы выходим из метода
        if ((currSwapRot == 0 && cellJ == columnCount - 1) ||
            (currSwapRot == 180 && cellI == 0)) 
        {
            swapSelected.Clear();
            return;
        }

        //добавляем первый элемнт
        AddItem(swapSelected, boardItems[cellI, cellJ]);
        //добавляем второй элемент в зависимости от выбранного вращения
        if (currSwapRot == 0)
        {
            //правый элемент
            AddItem(swapSelected, boardItems[cellI, cellJ + 1]);
        }
        else
        {
            //верхний элемент
            AddItem(swapSelected, boardItems[cellI - 1, cellJ]);
        }
        //если оба выбранных лемента не активны то выходим
        if (!swapSelected[0].GetComponent<BoardItem>().GetActive() &&
            !swapSelected[1].GetComponent<BoardItem>().GetActive())
        {
            EnableSwapSelected(false);
            return;
        }

        //отрисовываем
        EnableSwapSelected(true);
    }

    //добавляем элемент в лист если нет такого
    private void AddItem(List<GameObject> currList, GameObject itm)
    {
        for (int i = 0;i<currList.Count;i++)
        {
            //если такой есть то выходим
            if (itm == currList[i])
            {
                return;
            }
        }
        currList.Add(itm);
    }

    //отрисовываем прозручную выделенную линию
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

    //убираем отрисованную прозрачную линию
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

    //область бомбы
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

    //область молнии
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

    //область молнии
    private void EnableSwapSelected(bool value)
    {
        if (swapSelected.Count > 0)
        {
            //первый красный
            swapSelected[0].GetComponent<BoardItem>().EnableRedSelected(value);
            //второй зеленый
            swapSelected[1].GetComponent<BoardItem>().EnableGreenSelected(value);
            if (!value)
            {
                swapSelected.Clear();
            }
        }
    }

    //ставим клетки на выбранную позицию
    private void PutSelectedLine()
    {
        if (selectedSells.Count > 0 && !isGameOver)
        {
            for (int i = 0; i < selectedSells.Count; i++)
            {
                selectedSells[i].GetComponent<BoardItem>().EnableActive();
            }
            RandomLine();
            //провеярем поставленную линию на одинаковые
            for (int i = 0; i < selectedSells.Count; i++)
            {
                FindMatch((int)selectedSells[i].GetComponent<BoardItem>().GetCoord().x,
                    (int)selectedSells[i].GetComponent<BoardItem>().GetCoord().y);
            }
            DisableTransparentLine();

            //Play Sound
            SoundController.instance.PlayPutLineSound();

            //если поставили в первый раз, то запускаем таймер
            if (firstLinePut)
            {
                ViewController.instance.StartTimer();
                firstLinePut = false;
            }
            //сделали ход
            ViewController.instance.StepDone();
            if (isTutorial)
            {
                TutorialController.instance.OffPointer();
            }
        }
    }

    //ставим бомбу
    private void PutBomb()
    {
        if (bombSelected.Count > 0 && !isGameOver)
        {
            for (int i = 0;i<bombSelected.Count;i++)
            {
                //если активен то лопаем
                if (bombSelected[i].GetComponent<BoardItem>().GetActive())
                {
                    bombSelected[i].GetComponent<BoardItem>().PopItem();
                }
            }
            //вычитаем из количества бомб
            GameSettings.instance.BombCount--;

            //Play Sound
            SoundController.instance.PlayBombSound();

            ViewController.instance.PlusBombUsed();
            EnableBombSelected(false);
            UpdateBombVisual();
        }
    }

    //ставим молнию
    private void PutLightning()
    {
        if (lightningSelected.Count > 0 && !isGameOver)
        {
            for (int i = 0; i < lightningSelected.Count; i++)
            {
                //если активен то лопаем
                if (lightningSelected[i].GetComponent<BoardItem>().GetActive())
                {
                    lightningSelected[i].GetComponent<BoardItem>().PopItem();
                }
            }
            //вычитаем из количества
            GameSettings.instance.LightningCount--;

            //Play Sound
            SoundController.instance.PlayLightningSound();

            ViewController.instance.PlusLightningUsed();
            EnableLightningSelected(false);
            UpdateLightningVisual();
        }
    }

    //ставим свап
    private void PutSwap()
    {
        if (swapSelected.Count > 0 && !isGameOver)
        {
            //меняем местами
            SwapChips();

            //вычитаем из количества
            GameSettings.instance.SwapCount--;

            //Play Sound
            SoundController.instance.PlaySwapSound();

            ViewController.instance.PlusSwapUsed();
            EnableSwapSelected(false);
            UpdateSwapVisual();
        }
    }

    //меняем элементы местами
    private void SwapChips()
    {
        BoardItem swapChip1 = swapSelected[0].GetComponent<BoardItem>();
        BoardItem swapChip2 = swapSelected[1].GetComponent<BoardItem>();

        item tmp = swapChip1.GetCurrItem();
        bool tmpActive = swapChip1.GetActive();

        //первый на второй
        if (swapChip2.GetActive())
        {
            //если меняем на активный
            swapChip1.SwapChipItem(swapChip2.GetCurrItem());
            swapChip1.EnableActive();
        }
        else
        {
            //если меняем на неактивный
            swapChip1.SwapChipItem(swapChip2.GetCurrItem());
            swapChip1.DisableActive();
            swapChip1.SetNullItem();
        }
        //меняем второй на первый
        if (tmpActive)
        {
            //если меняем на активный
            swapChip2.SwapChipItem(tmp);
            swapChip2.EnableActive();
        }
        else
        {
            //если меняем на неактивный
            swapChip2.SwapChipItem(tmp);
            swapChip2.DisableActive();
            swapChip2.SetNullItem();
        }

        //находим если ли предметы с которыми можно лопнуть
        FindMatch((int)swapChip1.GetCoord().x, (int)swapChip1.GetCoord().y);
        FindMatch((int)swapChip2.GetCoord().x, (int)swapChip2.GetCoord().y);

        EnableSwapSelected(false);
    }

    //если не одна клетка не выделенна
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


    //Проверка на одинаковые элементы
    //нажатие на определенный айтем
    public void FindMatch(int currI, int currJ)
    {
        if (!isGameOver)
        {
            List<GameObject> matchItems = FindAllMatchItems(currI, currJ);
            //если выделенных объектов достаточно для удаления
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

    //выключаем все выделенные объекты
    private IEnumerator DisableMatchItems(List<GameObject> matchItems)
    {
        canTake = false;
        //играем анимацию
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

    //находим все прилегающие объекты одного цвета
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

    //удаляем одинаковые элементы
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

    //метод для постепенного прибавления всех прилегающих объектов
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

    //находим все объекты одного цвета вокруг выбранного
    private List<GameObject> GetMatchAroundItems(int currI, int currJ)
    {
        List<GameObject> matchItems = new List<GameObject>();
        if (!boardItemsControllers[currI, currJ].SelectedToPop)
        {
            matchItems.Add(boardItems[currI, currJ]);
            boardItemsControllers[currI, currJ].SelectedToPop = true;
        }
        //верхний
        if (currI > 0)
        {
            //кординаты сравнимаемого айтема
            int compI = currI - 1;
            int compJ = currJ;
            if (IsMatchColors(compI, compJ, currI, currJ))
            {
                //если цвета одинаковы то добавляем в список
                matchItems.Add(boardItems[compI, compJ]);
                boardItemsControllers[currI, currJ].SelectedToPop = true;
            }
        }
        //нижний
        if (currI < rowCount - 1)
        {
            //кординаты сравнимаемого айтема
            int compI = currI + 1;
            int compJ = currJ;
            if (IsMatchColors(compI, compJ, currI, currJ))
            {
                //если цвета одинаковы то добавляем в список
                matchItems.Add(boardItems[compI, compJ]);
                boardItemsControllers[currI, currJ].SelectedToPop = true;
            }
        }
        //правый
        if (currJ < columnCount - 1)
        {
            //кординаты сравнимаемого айтема
            int compI = currI;
            int compJ = currJ + 1;
            if (IsMatchColors(compI, compJ, currI, currJ))
            {
                //если цвета одинаковы то добавляем в список
                matchItems.Add(boardItems[compI, compJ]);
                boardItemsControllers[currI, currJ].SelectedToPop = true;
            }
        }
        //левый
        if (currJ > 0)
        {
            //кординаты сравнимаемого айтема
            int compI = currI;
            int compJ = currJ - 1;
            if (IsMatchColors(compI, compJ, currI, currJ))
            {
                //если цвета одинаковы то добавляем в список
                matchItems.Add(boardItems[compI, compJ]);
                boardItemsControllers[currI, currJ].SelectedToPop = true;
            }
        }
        //возвращаем список айтомв с одним цветом
        return matchItems;
    }

    //проверка на одинаковый цвет
    private bool IsMatchColors(int compI, int compJ, int mainI, int mainJ)
    {
        //если одинаковые цвета и оба эллемента активны и сравниваеммый эллемент не выделен уже
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

    //задержка на то чтобы взять линию после лопанья
    IEnumerator TakeWait()
    {
        canTake = false;
        yield return new WaitForSeconds(0.5f);
        canTake = true;
    }

    //убираем выделение с отсавшихся
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
        //обновляем отображение верхней панели
        UpdateUpPanel();
        //обновляем отображение линии фишек
        UpdateChipsLine();
        //обновляем отображение  двигающейся линии
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
