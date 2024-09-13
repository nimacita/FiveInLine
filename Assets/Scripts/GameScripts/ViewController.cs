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
    //�������� �� ������� ���������� ���������t �����
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

    //��������� ���������
    private void StartSettings()
    {
        LoadSettings();
        currentStepsCount = stepCounts;
        currentTime = timeToStep;
        currentScore = 0;

        //���� �� ���� ������
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

    //��������� ��������� ��� ������
    private void LoadSettings()
    {
        stepCounts = GameSettings.instance.StepCount;
        timeToStep = GameSettings.instance.TimeStep;
        neededScore = GameSettings.instance.NeededScore;
        coinsRewardKoef = GameSettings.instance.CurrentCoinsKoef;
    }

    //��������� ��������� ���
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

    //�������� ������ � ����� �����
    private void StartQuestsSettings()
    {
        //������� ������ � ������ ���� ��� ������������ ������� ���� ������ �������
        Quests.instance.StartQuestLelvInMinute();
    }

    //�������� ������ � ����� �����
    private void EndQuestSettings()
    {
        //�������� ������ � ����� ����, ��� ��������� ������������
        Quests.instance.CheckQuestLelvInMinute();
    }

    //��������� ������
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

    //��������� �������� �����
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

    //��������� ������ ������
    private void UpdateQuestPanel()
    {
        if (questPanel != null && !GameSettings.instance.IsTutorial) 
        {
            questPanel.SetActive(false);
            if (Quests.instance.QuestSelected && !Quests.instance.AllQuestDone && Quests.instance.DoneQuestId == -1) 
            {
                //���� ��� ������� ������� � �� ������� ����� ������ ������� �� �����
                if (GameSettings.instance.ThisLvlIndex == Quests.instance.QLIMlvl - 1 && Quests.instance.QuestSelectedLelvInMinute)
                {
                    questPanel.SetActive(true);
                    questDiscribeTxt.text = $"Complete level in {Quests.instance.QLIMMinuts} minutes";
                    questSecondsTxt.text = $"{Quests.instance.GetCurrentTimer().ToString("00")}/" +
                        $"{Quests.instance.QLIMMinuts * 60}";
                }
                //���� �� ����� ����� ������� ����� ������
                if (Quests.instance.QuestSelectedCollectChipsOnce)
                {
                    questPanel.SetActive(true);
                    questDiscribeTxt.text = $"Collect {Quests.instance.OnceChipsCount} chips at once";
                    questSecondsTxt.text = $"{Quests.instance.MaxOnceChipsCount}/{Quests.instance.OnceChipsCount}";
                }
            }
        }
    }

    //������� ������
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
                //���� ��������
                GameOver();
            }
        }
        UpdateTimerTxt();
    }

    //������ �������� ������� � ����������� �� ��������
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

    //��������� ����� �����
    private void UpdateStepTxt()
    {
        //���� ���� ������������ ���������� �����
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

    //��������� ����� �������
    private void UpdateTimerTxt()
    {
        string str = Convert.ToString($"{currentTime:00.00}");
        string[] parts = str.Split(',');
        timerTxt.text = $"{parts[0]}:{parts[1]}";
    }

    //��������� ��� ������ ��������
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

    //������ �� ����� ��������
    private void DoubleBonusClicked()
    {
        if (!isDouble)
        {
            //���� ����� ����, ����������, ���� ���, ��������
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

    //���� �� ��� �����
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

    //��������� ��� ������ ��������
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

    //������ �� ����� ��������
    private void ExtraMoveBonusClicked()
    {
        //���� ����� ����, ����������, ���� ���, ��������
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

    //��������� ��� ������ ���������
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
        //���� ��������� ���������
        if (isFreeze)
        {

        }
    }

    //������ �� ����� ���������
    private void FreezeBonusClick()
    {
        //���� ����� ����, ����������, ���� ���, ��������
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

    //������� ����� �������� ���������
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

    //��������� ����� �������� ������
    private void UpdateScoreTxt()
    {
        scoreTxt.text = $"{currentScore}";
        if (neededScore > 0) 
        {
            UpdateGoalTxt();
        }
    }

    //��������� ����� � �������� ��� ����
    private void UpdateGoalTxt()
    {
        if (currentScore >= neededScore)
        {
            //���� ���������
            goalImg.color = doneGoalColor;
            goalImg.fillAmount = 1;
            //�������� ������ � ����� ���� ���� ��������
            EndQuestSettings();
            Victory();
        }
        else
        {
            //���� �� ���������
            goalImg.color = notDoneGoalColor;
            goalImg.fillAmount = (float)currentScore/(float)neededScore;
        }
    }

    //��������� ������
    public void AddScore(int score)
    {
        int addedScore = score * scoreKoef;
        currentScore += addedScore;
        //���� ���� ��������, ���������� ��� ���
        if (isDouble)
        {
            currentScore += addedScore;
            currentDoubleSteps++;
            UpdateIsDouble();
        }
        UpdateScoreTxt();
    }

    //���� ��������, ���� ��������� ���� ��������
    private void GameOver()
    {
        GameController.instance.GameOver();
        isGameOver = true;
        //���� �� ���� ������
        if (neededScore > 0)
        {
            //���� ������� ����� ���������� �����, �� ��������, ���� ��� �� ���������
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
            //���� ���� ��� �� ���������� ���� ������� ������ 0 �����
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

    //��������� ������
    public void StartTimer()
    {
        isTimer = true;
    }

    //��������� ������ � ��������� �����
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

        //���� ��������� ���� � ���� �� ��� �����������
        if (currentStepsCount == 0 && stepCounts > 0)
        {
            GameOver();
        }
    }

    //�������� �����
    private void PauseOn()
    {
        StartCoroutine("OnPauseAnim");
    }

    //�������� ��������� �����
    private IEnumerator OnPauseAnim()
    {
        ItteractPauseBtn(false);
        isPause = true;
        pauseView.SetActive(true);
        pauseAnim.Play(pauseOnAnim.name);
        yield return new WaitForSeconds(pauseOnAnim.length);
        ItteractPauseBtn(true);
        //�������� �������
        onPauseActived?.Invoke(true);
    }

    //��������� �����
    private void PauseOff()
    {
        StartCoroutine("OffPauseAnim");
    }

    //�������� ���������� �����
    private IEnumerator OffPauseAnim()
    {
        ItteractPauseBtn(false);
        isPause = false;
        pauseAnim.Play(pauseOffAnim.name);
        yield return new WaitForSeconds(pauseOffAnim.length);
        //�������� �������
        onPauseActived?.Invoke(false);
        pauseView.SetActive(false);
    }

    private void ItteractPauseBtn(bool value)
    {
        pauseContinueBtn.interactable = value;
        pauseMenuBtn.interactable = value;
        pauseRestartBtn.interactable = value;
    }

    //������� ����� ������
    private void Victory()
    {
        if (!isVictory) 
        {
            vicScoreTxt.text = $"{currentScore}";
            SetUsedBonusesTxt();

            //������������ ������
            int currentCoins;
            if (GameSettings.instance.IsTutorial) 
            {
                //���� ��� ������ �������� �� ���� ������, ����� �� ����
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
                //���� �� �������� - ������� ������
                currentCoins = (int)((float)currentScore * coinsRewardKoef);
            }
            vicCoinTxt.text = $"{currentCoins}";
            GameSettings.instance.Coins += currentCoins;

            //�������� ������� ����������
            GameSettings.instance.CompleteThisLvl();

            //play sound
            SoundController.instance.PlayVictorySound();

            //������� ��������
            StartCoroutine(VictoryOnAnim());
            isVictory = true;
        }
    }

    //������������� ����� �������������� �������
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

    //�������� ������ ������
    private IEnumerator VictoryOnAnim()
    {
        yield return new WaitForSeconds(0.2f);
        vicView.SetActive(true);
        vicAnim.Play(vicOn.name);
    }

    //������� ����� ���������
    private void Defeat()
    {
        defeatScoreTxt.text = $"{currentScore}";
        SetUsedBonusesTxt();
        StartCoroutine(DefeatOnAnim());

        //play sound
        SoundController.instance.PlayDefeatSound();

        //�������� ������ ���� �������
        Quests.instance.ResetQuestLelvInMinuteTimer();
    }

    //�������� ������ ������
    private IEnumerator DefeatOnAnim()
    {
        yield return new WaitForSeconds(0.2f);
        defeatView.SetActive(true);
        defeatAnim.Play(defeatOn.name);
    }

    //��������� �������
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

    //��������� ���� ������� �������
    private void BuyBoostersOn()
    {
        //������������� ����� � ������ �������
        isPause = true;
        //�������� �������
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

    //��������� ���� ������� �������
    private void BuyBoosterOff()
    {
        StartCoroutine(BoosterOffAnim());
    }

    //�������� ����������
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
        //�������� �������
        onPauseActived?.Invoke(false);
        isPause = false;
    }

    //������������� �������
    private void Restart()
    {
        StartCoroutine(openScene(SceneManager.GetActiveScene().name));
    }

    //������� � ����
    private void ExitToMenu()
    {
        //��������� ����� ����
        StartCoroutine(openScene("MainMenu"));
    }

    //��������� ����� ����� �������� ��� ��������
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
