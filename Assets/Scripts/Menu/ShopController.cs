using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{

    [Header("Currency")]
    [SerializeField] private TMPro.TMP_Text coinTxt;
    [SerializeField] private TMPro.TMP_Text gemTxt;

    [Header("Shop Select View")]
    [SerializeField] private GameObject shopSelectView;
    [SerializeField] private Button ssBustersBtn;
    [SerializeField] private Button ssSkinsBtn;
    [SerializeField] private Button ssCurencyBtn;
    [SerializeField] private Button ssBackBtn;

    [Header("Shop View")]
    [SerializeField] private GameObject shopView;
    [SerializeField] private Button shopBackBtn;
    [SerializeField] private GameObject boostersPanel;
    [SerializeField] private GameObject skinsPanel;
    [SerializeField] private GameObject currencyPanel;

    [Header("Animations Settings")]
    [SerializeField] private Animation shopSelectAnim;
    [SerializeField] private AnimationClip shopSelectOn;
    [SerializeField] private AnimationClip shopSelectOff;
    [SerializeField] private AnimationClip shopSelectNextOff;
    [SerializeField] private AnimationClip shopSelectNextOn;
    [SerializeField] private Animation shopAnim;
    [SerializeField] private AnimationClip shopOn;
    [SerializeField] private AnimationClip shopOff;

    public static ShopController instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        StartViewSettings();
        UpdateCurrency();
        ButtonSettings();
    }

    //начальные настройки экранов
    private void StartViewSettings()
    {
        shopSelectView.SetActive(false);
        shopView.SetActive(false);
        boostersPanel.SetActive(false);
        skinsPanel.SetActive(false);
        currencyPanel.SetActive(false);
    }

    //настройка нкопок
    private void ButtonSettings()
    {
        ssBackBtn.onClick.AddListener(ShopSelectOff);
        ssBustersBtn.onClick.AddListener(BoostersSelect);
        ssSkinsBtn.onClick.AddListener(SkinsSelect);
        ssCurencyBtn.onClick.AddListener(CurrencySelect);

        shopBackBtn.onClick.AddListener(ShopOff);
    }

    //включаем экран выбора магазина
    public void ShopSelectOn()
    {
        UpdateCurrency();
        shopSelectView.SetActive(true);
        StartCoroutine(ShopSelectOnAnim());
    }

    private IEnumerator ShopSelectOnAnim()
    {
        ItteractAllBtns(false);
        shopSelectAnim.Play(shopSelectOn.name);
        yield return new WaitForSeconds(shopSelectOn.length);
        ItteractAllBtns(true);
    }

    //вылючаем меню выбора экрана
    private void ShopSelectOff()
    {
        MenuController.instance.MenuOn();
        ItteractAllBtns(false);
        StartCoroutine(ShopSelectOffAnim());
        ItteractAllBtns(true);
    }

    //анимация выключения меню выбора
    private IEnumerator ShopSelectOffAnim()
    {
        shopSelectAnim.Play(shopSelectOff.name);
        ItteractAllBtns(true);
        yield return new WaitForSeconds(shopSelectOff.length);
        ItteractAllBtns(true);
        shopSelectView.SetActive(false);
    }


    //двигаем меню выбора экарана
    private IEnumerator ShopSelectOffNext()
    {
        shopSelectAnim.Play(shopSelectNextOff.name);
        ItteractAllBtns(false);
        yield return new WaitForSeconds(shopSelectNextOff.length);
        ItteractAllBtns(true);
        shopSelectView.SetActive(false);
    }

    //двигаем меню выбора экрана обратно
    private void ShopSelectOnNext()
    {
        shopSelectView.SetActive(true);
        StartCoroutine(ShopSelectOnNextAnim());
    }

    private IEnumerator ShopSelectOnNextAnim()
    {
        shopSelectAnim.Play(shopSelectNextOn.name);
        ItteractAllBtns(false);
        yield return new WaitForSeconds(shopSelectNextOn.length);
        ItteractAllBtns(true);
    }

    //выбрали бустеры
    private void BoostersSelect()
    {
        boostersPanel.SetActive(true);
        skinsPanel.SetActive(false);
        currencyPanel.SetActive(false);

        ShopOn();
    }

    //выбрали скины
    private void SkinsSelect()
    {
        skinsPanel.SetActive(true);
        boostersPanel.SetActive(false);
        currencyPanel.SetActive(false);

        ShopOn();
    }

    //выбрали валюту
    private void CurrencySelect()
    {
        currencyPanel.SetActive(true);
        boostersPanel.SetActive(false);
        skinsPanel.SetActive(false);

        ShopOn();
    }

    //включаем экран магазина
    private void ShopOn()
    {
        StartCoroutine(ShopSelectOffNext());
        shopView.SetActive(true);
        StartCoroutine(ShopOnAnim());
    }

    private IEnumerator ShopOnAnim()
    {
        shopAnim.Play(shopOn.name);
        ItteractAllBtns(false);
        yield return new WaitForSeconds(shopOn.length);
        ItteractAllBtns(true);
    }

    //выключаем экран магазина 
    private void ShopOff()
    {
        StartCoroutine(ShopOffAnim());
    }

    private void ItteractAllBtns(bool value)
    {
        shopBackBtn.interactable = value;
        ssBackBtn.interactable = value;
        ssBustersBtn.interactable = value;
        //ssCurencyBtn.interactable = value;
        ssSkinsBtn.interactable = value;
    }

    //выключаем экран магазина анимация
    private IEnumerator ShopOffAnim()
    {
        shopAnim.Play(shopOff.name);
        ShopSelectOnNext();
        ItteractAllBtns(false);
        yield return new WaitForSeconds(shopOff.length);
        ItteractAllBtns(true);
        shopView.SetActive(false);
    }

    //обновляем значения валюты
    public void UpdateCurrency()
    {
        coinTxt.text = $"{GameSettings.instance.Coins}";
        gemTxt.text = $"{GameSettings.instance.Gems}";
    }


}
