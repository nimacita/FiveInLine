using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;

public class ViewController : MonoBehaviour
{

    [Header("Main Settings")]
    private int stepCounts;
    private int currentStepsCount;
    private float timeToStep;
    private float currentTime;
    private int neededScore;
    private int currentScore;
    //згачегие на которое умножается полученныt очков
    private int scoreKoef = 1;
    private float coinsRewardKoef;

    [Header("Game View")]
    [SerializeField] private GameObject gameView;
    [SerializeField] private TMPro.TMP_Text timerTxt;
    [SerializeField] private Animation timerAnim;
    [SerializeField] private AnimationClip timerOn;
    [SerializeField] private AnimationClip timerOff;
    [SerializeField] private GameObject stepCountObj;
    [SerializeField] private TMPro.TMP_Text stepsTxt;
    [SerializeField] private TMPro.TMP_Text scoreTxt;
    [SerializeField] private Button pauseBtn;
    [SerializeField] private GameObject gameGoal;
    [SerializeField] private TMPro.TMP_Text goalTxt;
    [SerializeField] private Image goalImg;
    [SerializeField] private Color doneGoalColor;
    [SerializeField] private Color notDoneGoalColor;

    [Header("Quest Panel")]
    [SerializeField] private GameObject questPanel;
    [SerializeField] private TMPro.TMP_Text questDiscribeTxt;
    [SerializeField] private TMPro.TMP_Text questSecondsTxt;

    [Header("Pause View")]
    [SerializeField] private GameObject pauseView;
    [SerializeField] private Animation pauseAnim;
    [SerializeField] private AnimationClip pauseOffAnim, pauseOnAnim;
    [SerializeField] private Button pauseContinueBtn;
    [SerializeField] private Button pauseRestartBtn;
    [SerializeField] private Button pauseMenuBtn;

    [Header("Buy Boosters View")]
    [SerializeField] private GameObject buyBoostersView;
    [SerializeField] private Animation buyBoostersAnim;
    [SerializeField] private AnimationClip buyBoostersOn;
    [SerializeField] private AnimationClip buyBoostersOff;
    [SerializeField] private Button buyBoostersBackBtn;
    [SerializeField] private GameObject bombBooster;
    [SerializeField] private GameObject lightningBooster;
    [SerializeField] private GameObject swapBooster;
    [SerializeField] private GameObject doubleBooster;
    [SerializeField] private GameObject extraMoveBooster;
    [SerializeField] private GameObject freezeBooster;
    [SerializeField] private TMPro.TMP_Text cointTxt;

    [Header("Victory View")]
    [SerializeField] private GameObject vicView;
    [SerializeField] private Animation vicAnim;
    [SerializeField] private AnimationClip vicOn;
    [SerializeField] private TMPro.TMP_Text vicScoreTxt;
    [SerializeField] private TMPro.TMP_Text vicCoinTxt;
    [SerializeField] private Button vicRestartBtn;
    [SerializeField] private Button vicMenuBtn;
    [SerializeField] private TMPro.TMP_Text vicBombCountTxt;
    [SerializeField] private TMPro.TMP_Text vicLightningCountTxt;
    [SerializeField] private TMPro.TMP_Text vicSwapCountTxt;
    [SerializeField] private TMPro.TMP_Text vicDoubleCountTxt;
    [SerializeField] private TMPro.TMP_Text vicExtraMoveCountTxt;
    [SerializeField] private TMPro.TMP_Text vicFreezeCountTxt;

    [Header("Defeat View")]
    [SerializeField] private GameObject defeatView;
    [SerializeField] private Animation defeatAnim;
    [SerializeField] private AnimationClip defeatOn;
    [SerializeField] private TMPro.TMP_Text defeatScoreTxt;
    [SerializeField] private Button defeatRestartBtn;
    [SerializeField] private Button defeatMenuBtn;
    [SerializeField] private TMPro.TMP_Text defeatBombCountTxt;
    [SerializeField] private TMPro.TMP_Text defeatLightningCountTxt;
    [SerializeField] private TMPro.TMP_Text defeatSwapCountTxt;
    [SerializeField] private TMPro.TMP_Text defeatDoubleCountTxt;
    [SerializeField] private TMPro.TMP_Text defeatExtraMoveCountTxt;
    [SerializeField] private TMPro.TMP_Text defeatFreezeCountTxt;

    [Header("Double Bonus")]
    [SerializeField] private Button doubleBonus;
    [SerializeField] private GameObject doubleCountBg;
    [SerializeField] private TMPro.TMP_Text doubleCountTxt;
    [SerializeField] private GameObject doublePlusBg;
    [SerializeField] private GameObject doublePopUp;
    private int currentDoubleCount = 0;
    private int currentDoubleSteps = 0;
    private int maxDoubleSteps = 5;

    [Header("Extra Move Bonus")]
    [SerializeField] private Button extraMoveBonus;
    [SerializeField] private GameObject extraMoveCountBg;
    [SerializeField] private TMPro.TMP_Text extraMoveCountTxt;
    [SerializeField] private GameObject extraMovePlusBg;
    private int currentExtraMoveCount = 0;

    [Header("Freeze Bonus")]
    [SerializeField] private Button freezeBonus;
    [SerializeField] private GameObject freezeCountBg;
    [SerializeField] private TMPro.TMP_Text freezeCountTxt;
    [SerializeField] private GameObject freezePlusBg;
    private int currentFreezeCount = 0;
    private float currentfreezeStopTime = 0;
    private float freezeStopTime = 10;

    [Header("Skin Settings")]
    [SerializeField] private Image boardImg;
    [SerializeField] private Image bgImg;
    [SerializeField] private Image chipLineBgImg;
    [SerializeField] private Image chipLineImg;
    [SerializeField] private Image rSidePanelImg;
    [SerializeField] private Image lSidePanelImg;
    [SerializeField] private Image swapRotPanelImg;
    [SerializeField] private Image lineRotPanelImg;


    [Header("Editor")]
    [SerializeField] private GameObject mainCamera;

    //bonuses
    private int bombUsed = 0;
    private int lightningUsed = 0;
    private int swapUsed = 0;
    private int doubleUsed = 0;
    private int extraMoveUsed = 0;
    private int freezeUsed = 0;

    //bools
    private bool isTimer = false;
    private bool isGameOver = false;
    private bool isPause = false;
    private bool isDouble = false;
    private bool isFreeze = false;
    private bool isVictory = false;

    public static ViewController instance;
    public static Action<bool> onPauseActived;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        StartSettings();
        StartViewSettings();
        ButtonSettings();
        StartSkinSettings();
        StartQuestsSettings();
    }

    //начальные настройки
    private void StartSettings()
    {
        LoadSettings();
        currentStepsCount = stepCounts;
        currentTime = timeToStep;
        currentScore = 0;

        //есть ли цель уровня
        if (neededScore > 0)
        {
            gameGoal.SetActive(true);
        }
        else
        {
            gameGoal.SetActive(false);
        }
        goalTxt.text = $"{neededScore}";

        UpdateScoreTxt();
        UpdateStepTxt();
        UpdateDoubleVisual();
        UpdateExtraMoveVisual();
        UpdateFreezeVisual();
    }

    //загружаем настройки для уровня
    private void LoadSettings()
    {
        stepCounts = GameSettings.instance.StepCount;
        timeToStep = GameSettings.instance.TimeStep;
        neededScore = GameSettings.instance.NeededScore;
        coinsRewardKoef = GameSettings.instance.CurrentCoinsKoef;
    }

    //начальные настройки вью
    private void StartViewSettings()
    {
        gameView.SetActive(true);
        pauseView.SetActive(false);
        defeatView.SetActive(false);
        vicView.SetActive(false);

        buyBoostersView.SetActive(false);
        bombBooster.SetActive(false);
        lightningBooster.SetActive(false);
        swapBooster.SetActive(false);
        doubleBooster.SetActive(false);
        extraMoveBooster.SetActive(false);
        freezeBooster.SetActive(false);
    }

    //передаем данные в квест чекер
    private void StartQuestsSettings()
    {
        //предаем данные о начале игры для отслеживание времени если нужный уровень
        Quests.instance.StartQuestLelvInMinute();
    }

    //передаем данные в квест чекер
    private void EndQuestSettings()
    {
        //передаем данные о конце игры, для оокнчания отслеживания
        Quests.instance.CheckQuestLelvInMinute();
    }

    //настройки кнопок
    private void ButtonSettings()
    {
        pauseBtn.onClick.AddListener(PauseOn);

        pauseContinueBtn.onClick.AddListener(PauseOff);
        pauseRestartBtn.onClick.AddListener(Restart);
        pauseMenuBtn.onClick.AddListener(ExitToMenu);

        vicRestartBtn.onClick.AddListener(Restart);
        vicMenuBtn.onClick.AddListener(ExitToMenu);

        defeatRestartBtn.onClick.AddListener(Restart);
        defeatMenuBtn.onClick.AddListener(ExitToMenu);

        doubleBonus.onClick.AddListener(DoubleBonusClicked);
        extraMoveBonus.onClick.AddListener(ExtraMoveBonusClicked);
        freezeBonus.onClick.AddListener(FreezeBonusClick);

        buyBoostersBackBtn.onClick.AddListener(BuyBoosterOff);
    }

    //настройка текущего скина
    private void StartSkinSettings()
    {
        LocationSkin currSkin = GameSettings.instance.GetCurrentLocSkin();
        boardImg.sprite = currSkin.gameBoardSkin;
        bgImg.sprite = currSkin.gameBgSkin;
        chipLineBgImg.sprite = currSkin.chipLineBgSkin;
        chipLineImg.sprite = currSkin.chipLineSkin;
        rSidePanelImg.sprite = currSkin.sidePanelsSkin;
        lSidePanelImg.sprite = currSkin.sidePanelsSkin;
        swapRotPanelImg.sprite = currSkin.horizontalPanelSkin;
        lineRotPanelImg.sprite = currSkin.horizontalPanelSkin;
    }

    private void Update()
    {
        TimerCount();
        FreezeTimeCount();
        UpdateQuestPanel();
    }

    //обновляем панель квеста
    private void UpdateQuestPanel()
    {
        if (questPanel != null && !GameSettings.instance.IsTutorial) 
        {
            questPanel.SetActive(false);
            if (Quests.instance.QuestSelected && !Quests.instance.AllQuestDone && Quests.instance.DoneQuestId == -1) 
            {
                //если это текущий уровень и мы считаем квест пройти уровень за время
                if (GameSettings.instance.ThisLvlIndex == Quests.instance.QLIMlvl - 1 && Quests.instance.QuestSelectedLelvInMinute)
                {
                    questPanel.SetActive(true);
                    questDiscribeTxt.text = $"Complete level in {Quests.instance.QLIMMinuts} minutes";
                    questSecondsTxt.text = $"{Quests.instance.GetCurrentTimer().ToString("00")}/" +
                        $"{Quests.instance.QLIMMinuts * 60}";
                }
                //если мы взяли квест собрать фишки подряд
                if (Quests.instance.QuestSelectedCollectChipsOnce)
                {
                    questPanel.SetActive(true);
                    questDiscribeTxt.text = $"Collect {Quests.instance.OnceChipsCount} chips at once";
                    questSecondsTxt.text = $"{Quests.instance.MaxOnceChipsCount}/{Quests.instance.OnceChipsCount}";
                }
            }
        }
    }

    //считаем таймер
    private void TimerCount()
    {
        if (isTimer && !isPause && !isGameOver && !isFreeze)
        {
            if (currentTime > 0)
            {
                currentTime -= Time.deltaTime;
            }
            else
            {
                currentTime = 0;
                //игра окончена
                GameOver();
            }
        }
        UpdateTimerTxt();
    }

    //играем анимацию таймера в зависимости от значения
    private void PlayTimerAnim(bool value)
    {
        if (value)
        {
            timerAnim.Play(timerOn.name);
        }
        else
        {
            timerAnim.Play(timerOff.name);
        }
    }

    //обновляем текст шагов
    private void UpdateStepTxt()
    {
        //если есть ограниченное количество ходов
        if (stepCounts > 0)
        {
            stepCountObj.SetActive(true);
            stepsTxt.text = $"{currentStepsCount}";
        }
        else
        {
            stepCountObj.SetActive(false);
        }
    }

    //обновляем текст таймера
    private void UpdateTimerTxt()
    {
        string str = Convert.ToString($"{currentTime:00.00}");
        string[] parts = str.Split(',');
        timerTxt.text = $"{parts[0]}:{parts[1]}";
    }

    //обновляем вид бонуса увдоение
    private void UpdateDoubleVisual()
    {
        currentDoubleCount = GameSettings.instance.DoublePointsCount;
        if (currentDoubleCount > 0)
        {
            doubleCountTxt.text = $"{currentDoubleCount}";
            doubleCountBg.SetActive(true);
            doublePlusBg.SetActive(false);
        }
        else
        {
            doubleCountBg.SetActive(false);
            doublePlusBg.SetActive(true);
        }
        doublePopUp.SetActive(isDouble);
    }

    //нажали на бонус удвоения
    private void DoubleBonusClicked()
    {
        if (!isDouble)
        {
            //если бонус есть, активируем, если нет, покупаем
            if (currentDoubleCount > 0)
            {
                isDouble = true;
                currentDoubleSteps = 0;
                GameSettings.instance.DoublePointsCount--;

                //Play Sound
                SoundController.instance.PlayDoubleSound();

                UpdateIsDouble();
                PlusDoubleUsed();
                UpdateDoubleVisual();
            }
            else
            {
                DoubleBusterClick();
            }
        }
    }

    //есть ли еще бонус
    private void UpdateIsDouble()
    {
        if (isDouble)
        {
            if (currentDoubleSteps >= maxDoubleSteps)
            {
                currentDoubleSteps = 0;
                isDouble = false;
            }
            UpdateDoubleVisual();
        }
    }

    //обновляем вид бонуса увдоение
    private void UpdateExtraMoveVisual()
    {
        currentExtraMoveCount = GameSettings.instance.ExtraMoveCount;
        if (currentExtraMoveCount > 0)
        {
            extraMoveCountTxt.text = $"{currentExtraMoveCount}";
            extraMoveCountBg.SetActive(true);
            extraMovePlusBg.SetActive(false);
        }
        else
        {
            extraMoveCountBg.SetActive(false);
            extraMovePlusBg.SetActive(true);
        }
    }

    //нажали на бонус удвоения
    private void ExtraMoveBonusClicked()
    {
        //если бонус есть, активируем, если нет, покупаем
        if (currentExtraMoveCount > 0)
        {
            if (stepCounts > 0)
            {
                GameSettings.instance.ExtraMoveCount--;
                currentStepsCount++;

                //Play Sound
                SoundController.instance.PlayExtraMoveSound();

                UpdateStepTxt();
                PlusExtraMoveUsed();
                UpdateExtraMoveVisual();
            }
        }
        else
        {
            ExtraMoveBusterClick();
        }
    }

    //обновляем вид бонуса заморозки
    private void UpdateFreezeVisual()
    {
        currentFreezeCount = GameSettings.instance.FreezeCount;
        if (currentFreezeCount > 0)
        {
            freezeCountTxt.text = $"{currentFreezeCount}";
            freezeCountBg.SetActive(true);
            freezePlusBg.SetActive(false);
        }
        else
        {
            freezeCountBg.SetActive(false);
            freezePlusBg.SetActive(true);
        }
        //если заморозка действует
        if (isFreeze)
        {

        }
    }

    //нажали на бонус заморозки
    private void FreezeBonusClick()
    {
        //если бонус есть, активируем, если нет, покупаем
        if (!isFreeze && currentTime != timeToStep) 
        {
            if (currentFreezeCount > 0)
            {
                GameSettings.instance.FreezeCount--;
                isFreeze = true;
                currentfreezeStopTime = freezeStopTime;

                //Play Sound
                SoundController.instance.PlayFreezeSound();

                PlayTimerAnim(true);
                PlusFreezeUsed();
                UpdateFreezeVisual();
            }
            else
            {
                FreezeBusterClick();
            }
        }
    }

    //считаем время действия заморозки
    private void FreezeTimeCount()
    {
        if (isFreeze && !isPause && !isGameOver)
        {
            if (currentfreezeStopTime > 0)
            {
                currentfreezeStopTime -= Time.deltaTime;
            }
            else
            {
                currentfreezeStopTime = 0;
                UpdateFreezeVisual();
                PlayTimerAnim(false);
                isFreeze = false;
            }
        }
    }

    //обновляем текст текущего рекода
    private void UpdateScoreTxt()
    {
        scoreTxt.text = $"{currentScore}";
        if (neededScore > 0) 
        {
            UpdateGoalTxt();
        }
    }

    //обновляем текст и картинку для целм
    private void UpdateGoalTxt()
    {
        if (currentScore >= neededScore)
        {
            //если выполнено
            goalImg.color = doneGoalColor;
            goalImg.fillAmount = 1;
            //передаем данные о конце игры если выйграли
            EndQuestSettings();
            Victory();
        }
        else
        {
            //если не выполнено
            goalImg.color = notDoneGoalColor;
            goalImg.fillAmount = (float)currentScore/(float)neededScore;
        }
    }

    //добавляем рекорд
    public void AddScore(int score)
    {
        int addedScore = score * scoreKoef;
        currentScore += addedScore;
        //если есть удвоение, прибавляем еще раз
        if (isDouble)
        {
            currentScore += addedScore;
            currentDoubleSteps++;
            UpdateIsDouble();
        }
        UpdateScoreTxt();
    }

    //игра окончена, либо проиграли либо выйграли
    private void GameOver()
    {
        GameController.instance.GameOver();
        isGameOver = true;
        //есть ли цель раунда
        if (neededScore > 0)
        {
            //если набрано нужно количество очков, то выйграли, если нет то проиграли
            if (currentScore >= neededScore)
            {
                Victory();
            }
            else
            {
                Defeat();
            }
        }
        else
        {
            //если цели нет то выигрываем если набрали больше 0 очков
            if (currentScore > 0)
            {
                Victory();
            }
            else
            {
                Defeat();
            }
        }

    }

    //запускаем таймер
    public void StartTimer()
    {
        isTimer = true;
    }

    //обновляем таймер и колечство ходов
    public void StepDone()
    {
        currentTime = timeToStep;
        if(currentStepsCount > 0) currentStepsCount--;

        UpdateStepTxt();

        if (isFreeze)
        {
            currentfreezeStopTime = 0;
            isFreeze = false;
            PlayTimerAnim(false);
        }

        //если закончили ходы и есть на них ограничение
        if (currentStepsCount == 0 && stepCounts > 0)
        {
            GameOver();
        }
    }

    //включаем паузу
    private void PauseOn()
    {
        StartCoroutine("OnPauseAnim");
    }

    //анимация включения паузы
    private IEnumerator OnPauseAnim()
    {
        ItteractPauseBtn(false);
        isPause = true;
        pauseView.SetActive(true);
        pauseAnim.Play(pauseOnAnim.name);
        yield return new WaitForSeconds(pauseOnAnim.length);
        ItteractPauseBtn(true);
        //вызываем событие
        onPauseActived?.Invoke(true);
    }

    //выключаем паузу
    private void PauseOff()
    {
        StartCoroutine("OffPauseAnim");
    }

    //анимация выключения паузы
    private IEnumerator OffPauseAnim()
    {
        ItteractPauseBtn(false);
        isPause = false;
        pauseAnim.Play(pauseOffAnim.name);
        yield return new WaitForSeconds(pauseOffAnim.length);
        //вызываем событие
        onPauseActived?.Invoke(false);
        pauseView.SetActive(false);
    }

    private void ItteractPauseBtn(bool value)
    {
        pauseContinueBtn.interactable = value;
        pauseMenuBtn.interactable = value;
        pauseRestartBtn.interactable = value;
    }

    //влючаем экран победы
    private void Victory()
    {
        if (!isVictory) 
        {
            vicScoreTxt.text = $"{currentScore}";
            SetUsedBonusesTxt();

            //заработанные деньги
            int currentCoins;
            if (GameSettings.instance.IsTutorial) 
            {
                //если был первый туториал то даем деньги, иначе не даем
                if (GameSettings.instance.IsFirstTutorial)
                {
                    currentCoins = (int)((float)currentScore * coinsRewardKoef);
                    GameSettings.instance.IsFirstTutorial = false;
                }
                else
                {
                    currentCoins = 0;
                }
            }
            else
            {
                //если не туториал - считаем деньги
                currentCoins = (int)((float)currentScore * coinsRewardKoef);
            }
            vicCoinTxt.text = $"{currentCoins}";
            GameSettings.instance.Coins += currentCoins;

            //отмечаем уровень пройденным
            GameSettings.instance.CompleteThisLvl();

            //play sound
            SoundController.instance.PlayVictorySound();

            //влючаем анимацию
            StartCoroutine(VictoryOnAnim());
            isVictory = true;
        }
    }

    //устанавливаем текст использованных бонусов
    private void SetUsedBonusesTxt()
    {
        defeatBombCountTxt.text = $"{bombUsed}";
        vicBombCountTxt.text = $"{bombUsed}";

        defeatLightningCountTxt.text = $"{lightningUsed}";
        vicLightningCountTxt.text = $"{lightningUsed}";

        defeatSwapCountTxt.text = $"{swapUsed}";
        vicSwapCountTxt.text = $"{swapUsed}";

        defeatDoubleCountTxt.text = $"{doubleUsed}";
        vicDoubleCountTxt.text = $"{doubleUsed}";

        defeatExtraMoveCountTxt.text = $"{extraMoveUsed}";
        vicExtraMoveCountTxt.text = $"{extraMoveUsed}";

        defeatFreezeCountTxt.text = $"{freezeUsed}";
        vicFreezeCountTxt.text = $"{freezeUsed}";
    }

    //анимация экрана победы
    private IEnumerator VictoryOnAnim()
    {
        yield return new WaitForSeconds(0.2f);
        vicView.SetActive(true);
        vicAnim.Play(vicOn.name);
    }

    //влючаем экран поражения
    private void Defeat()
    {
        defeatScoreTxt.text = $"{currentScore}";
        SetUsedBonusesTxt();
        StartCoroutine(DefeatOnAnim());

        //play sound
        SoundController.instance.PlayDefeatSound();

        //обнуляем таймер если запущен
        Quests.instance.ResetQuestLelvInMinuteTimer();
    }

    //анимация экрана победы
    private IEnumerator DefeatOnAnim()
    {
        yield return new WaitForSeconds(0.2f);
        defeatView.SetActive(true);
        defeatAnim.Play(defeatOn.name);
    }

    //обновляем монетки
    public void UpdateCoinTxt()
    {
        cointTxt.text = $"{GameSettings.instance.Coins}";
        UpdateDoubleVisual();
        UpdateExtraMoveVisual();
        UpdateFreezeVisual();
        GameController.instance.UpdateBombVisual();
        GameController.instance.UpdateLightningVisual();
        GameController.instance.UpdateSwapVisual();
    }

    //открываем меню покупки бонусов
    private void BuyBoostersOn()
    {
        //останавливаем время в момент покупки
        isPause = true;
        //вызываем событие
        onPauseActived?.Invoke(true);

        buyBoostersView.SetActive(true);
        buyBoostersAnim.Play(buyBoostersOn.name);
        UpdateCoinTxt();
    }

    public void BombBusterClick()
    {
        BuyBoostersOn();
        bombBooster.SetActive(true);
    }
    public void LightningBusterClick()
    {
        BuyBoostersOn();
        lightningBooster.SetActive(true);
    }
    public void SwapBusterClick()
    {
        BuyBoostersOn();
        swapBooster.SetActive(true);
    }
    public void DoubleBusterClick()
    {
        BuyBoostersOn();
        doubleBooster.SetActive(true);
    }
    public void ExtraMoveBusterClick()
    {
        BuyBoostersOn();
        extraMoveBooster.SetActive(true);
    }
    public void FreezeBusterClick()
    {
        BuyBoostersOn();
        freezeBooster.SetActive(true);
    }

    //выключаем меню покупки бонусов
    private void BuyBoosterOff()
    {
        StartCoroutine(BoosterOffAnim());
    }

    //анимация выключения
    private IEnumerator BoosterOffAnim()
    {
        buyBoostersAnim.Play(buyBoostersOff.name);
        yield return new WaitForSeconds(buyBoostersOff.length);
        bombBooster.SetActive(false);
        lightningBooster.SetActive(false);
        swapBooster.SetActive(false);
        doubleBooster.SetActive(false);
        extraMoveBooster.SetActive(false);
        freezeBooster.SetActive(false);
        buyBoostersView.SetActive(false);
        //вызываем событие
        onPauseActived?.Invoke(false);
        isPause = false;
    }

    //перезапускаем уровень
    private void Restart()
    {
        StartCoroutine(openScene(SceneManager.GetActiveScene().name));
    }

    //выходим в меню
    private void ExitToMenu()
    {
        //запускаем сцену меню
        StartCoroutine(openScene("MainMenu"));
    }

    //открываем сцену после задержки для перехода
    IEnumerator openScene(string sceneName)
    {
        float fadeTime = mainCamera.GetComponent<SceneFading>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(sceneName);
    }

    public void PlusBombUsed()
    {
        bombUsed++;
    }

    public void PlusLightningUsed()
    {
        lightningUsed++;
    }

    public void PlusSwapUsed()
    {
        swapUsed++;
    }

    public void PlusDoubleUsed()
    {
        doubleUsed++;
    }

    public void PlusExtraMoveUsed()
    {
        extraMoveUsed++;
    }

    public void PlusFreezeUsed()
    {
        freezeUsed++;
    }

}
