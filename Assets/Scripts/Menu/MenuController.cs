using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{

    [Header("Menu Settings")]
    [SerializeField] private Button playBtn;
    [SerializeField] private Button compaignBtn;
    [SerializeField] private Button settingsBtn;
    [SerializeField] private Button shopBtn;
    [SerializeField] private Button dailyBtn;

    [Header("Currency")]
    [SerializeField] private TMPro.TMP_Text cointTxt;
    [SerializeField] private TMPro.TMP_Text gemTxt;

    [Header("Play Selected Settings")]
    [SerializeField] private Button psBackBtn;
    [SerializeField] private Button psSingleBtn;
    [SerializeField] private Button psTutorBtn;

    [Header("Animation Settings")]
    [SerializeField] private Animation menuAnim;
    [SerializeField] private AnimationClip menuOn;
    [SerializeField] private AnimationClip menuOff;
    [SerializeField] private Animation playSelectedAnim;
    [SerializeField] private AnimationClip playSelectedOn;
    [SerializeField] private AnimationClip playSelectedOff;

    [Header("Views Settings")]
    [SerializeField] private GameObject menuView;
    [SerializeField] private ShopController shopController;
    [SerializeField] private GameObject settingsView;
    [SerializeField] private SettingsController settingsController;
    [SerializeField] private GameObject levelView;
    [SerializeField] private LvlMenuController lvlMenuController;
    [SerializeField] private GameObject playSelectView;
    [SerializeField] private GameObject dailyView;
    [SerializeField] private QuestController questController;

    [Header("Editor")]
    [SerializeField] private GameObject mainCamera;

    public static MenuController instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        StartViewSettings();
        ButtonSettings();
        UpdateCurrency();
        StartQuestSettings();
    }

    //начальные настройки экранов
    private void StartViewSettings()
    {
        menuView.SetActive(true);
        settingsView.SetActive(false);
        playSelectView.SetActive(false);
        levelView.SetActive(false);
    }

    //настройка кнопок
    private void ButtonSettings()
    {
        playBtn.onClick.AddListener(PlayClick);
        compaignBtn.onClick.AddListener(CompaignClick);
        settingsBtn.onClick.AddListener(SettingsClick);
        shopBtn.onClick.AddListener(ShopClick);
        dailyBtn.onClick.AddListener(DailyClick);

        psBackBtn.onClick.AddListener(PlaySelectedOff);
        psSingleBtn.onClick.AddListener(SinglePlayClick);
        psTutorBtn.onClick.AddListener(TutClikc);
    }

    //обнуляем таймер если запущен
    private void StartQuestSettings()
    {
        Quests.instance.ResetQuestLelvInMinuteTimer();
    }

    //обновляем значения валюты
    private void UpdateCurrency()
    {
        cointTxt.text = $"{GameSettings.instance.Coins}";
        gemTxt.text = $"{GameSettings.instance.Gems}";
    }

    //возращаемся в меню с анимацией
    public void MenuOn()
    {
        menuView.SetActive(true);
        menuAnim.Play(menuOn.name);
        UpdateCurrency();
    }

    //выключаем меню
    private IEnumerator MenuOff()
    {
        //играем анимацию
        menuAnim.Play(menuOff.name);
        yield return new WaitForSeconds(menuOff.length);
        menuView.SetActive(false);
    }

    //включаем экран выбора
    private void PlaySelectedOn()
    {
        playSelectView.SetActive(true);
        //анимация включения меню
        playSelectedAnim.Play(playSelectedOn.name);
    }

    //выключаем меню выбора игры
    private void PlaySelectedOff()
    {
        StartCoroutine(PlaySelectedOffAnim());
    }

    //анимация выключения меню выбора
    private IEnumerator PlaySelectedOffAnim()
    {
        MenuOn();
        //анимация выключения
        playSelectedAnim.Play(playSelectedOff.name);
        yield return new WaitForSeconds(playSelectedOff.length);;
        playSelectView.SetActive(false);
    }

    //нажатие на кнопку играть
    private void PlayClick()
    {
        StartCoroutine(MenuOff());
        PlaySelectedOn();
    }

    //нажатие на сингл плеер
    private void SinglePlayClick()
    {
        //устанавливаем настройки для одиночной игры
        GameSettings.instance.SetSinglePlayer();
        StartCoroutine(openScene("GameScene"));
    }

    //нажатие на туториал
    private void TutClikc()
    {
        //устанавливаем настройки для туториала
        GameSettings.instance.SetTutorial();
        StartCoroutine(openScene("TutorialScene"));
    }

    //нажатие на кнопку компании
    private void CompaignClick()
    {
        StartCoroutine(MenuOff());
        levelView.SetActive(true);
        lvlMenuController.LevelMenuOn();
    }

    //нажатие на кнопку настроек
    private void SettingsClick()
    {
        StartCoroutine(MenuOff());
        settingsView.SetActive(true);
        settingsController.SettingsOn();
    }

    //нажатие на кнопку ежедневных заданий
    private void DailyClick()
    {
        StartCoroutine(MenuOff());
        questController.QuestViewOn();
    }

    //нажатие на кнопку магазина
    private void ShopClick()
    {
        StartCoroutine(MenuOff());
        shopController.ShopSelectOn();
    }

    //открываем сцену после задержки для перехода
    IEnumerator openScene(string sceneName)
    {
        float fadeTime = mainCamera.GetComponent<SceneFading>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(sceneName);
    }
}
