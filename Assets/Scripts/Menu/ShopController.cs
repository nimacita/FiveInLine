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

    //��������� ��������� �������
    private void StartViewSettings()
    {
        shopSelectView.SetActive(false);
        shopView.SetActive(false);
        boostersPanel.SetActive(false);
        skinsPanel.SetActive(false);
        currencyPanel.SetActive(false);
    }

    //��������� ������
    private void ButtonSettings()
    {
        ssBackBtn.onClick.AddListener(ShopSelectOff);
        ssBustersBtn.onClick.AddListener(BoostersSelect);
        ssSkinsBtn.onClick.AddListener(SkinsSelect);
        ssCurencyBtn.onClick.AddListener(CurrencySelect);

        shopBackBtn.onClick.AddListener(ShopOff);
    }

    //�������� ����� ������ ��������
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

    //�������� ���� ������ ������
    private void ShopSelectOff()
    {
        MenuController.instance.MenuOn();
        ItteractAllBtns(false);
        StartCoroutine(ShopSelectOffAnim());
        ItteractAllBtns(true);
    }

    //�������� ���������� ���� ������
    private IEnumerator ShopSelectOffAnim()
    {
        shopSelectAnim.Play(shopSelectOff.name);
        ItteractAllBtns(true);
        yield return new WaitForSeconds(shopSelectOff.length);
        ItteractAllBtns(true);
        shopSelectView.SetActive(false);
    }


    //������� ���� ������ �������
    private IEnumerator ShopSelectOffNext()
    {
        shopSelectAnim.Play(shopSelectNextOff.name);
        ItteractAllBtns(false);
        yield return new WaitForSeconds(shopSelectNextOff.length);
        ItteractAllBtns(true);
        shopSelectView.SetActive(false);
    }

    //������� ���� ������ ������ �������
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

    //������� �������
    private void BoostersSelect()
    {
        boostersPanel.SetActive(true);
        skinsPanel.SetActive(false);
        currencyPanel.SetActive(false);

        ShopOn();
    }

    //������� �����
    private void SkinsSelect()
    {
        skinsPanel.SetActive(true);
        boostersPanel.SetActive(false);
        currencyPanel.SetActive(false);

        ShopOn();
    }

    //������� ������
    private void CurrencySelect()
    {
        currencyPanel.SetActive(true);
        boostersPanel.SetActive(false);
        skinsPanel.SetActive(false);

        ShopOn();
    }

    //�������� ����� ��������
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

    //��������� ����� �������� 
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

    //��������� ����� �������� ��������
    private IEnumerator ShopOffAnim()
    {
        shopAnim.Play(shopOff.name);
        ShopSelectOnNext();
        ItteractAllBtns(false);
        yield return new WaitForSeconds(shopOff.length);
        ItteractAllBtns(true);
        shopView.SetActive(false);
    }

    //��������� �������� ������
    public void UpdateCurrency()
    {
        coinTxt.text = $"{GameSettings.instance.Coins}";
        gemTxt.text = $"{GameSettings.instance.Gems}";
    }


}
