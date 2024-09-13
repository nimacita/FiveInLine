using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LvlMenuController : MonoBehaviour
{
    [Header("Main Settings")]
    [SerializeField] private GameObject lvlMenuView;
    [SerializeField] private Button backBtn;

    [Header("Animation Settings")]
    [SerializeField] private Animation lvlMenuAnim;
    [SerializeField] private AnimationClip lvlMenuOn;
    [SerializeField] private AnimationClip lvlMenuOff;
    [SerializeField] private AnimationClip tipsAnim;

    [Header("Editor")]
    [SerializeField]
    private GameObject mainCamera;


    void Start()
    {
        backBtn.onClick.AddListener(LevlMenuOff);
    }

    //����� � ����
    public void LevlMenuOff()
    {
        MenuController.instance.MenuOn();
        StartCoroutine(LevelMenuOffAnim());
    }

    private IEnumerator LevelMenuOffAnim()
    {
        //������ ��������
        lvlMenuAnim.Play(lvlMenuOff.name);
        yield return new WaitForSeconds(lvlMenuOff.length);
        lvlMenuView.SetActive(false);
    }

    //�������� ���� �������
    public void LevelMenuOn()
    {
        //������ ��������
        lvlMenuAnim.Play(lvlMenuOn.name);
    }

    //������� �� ������ ������
    public void LvlBtnClick(string levelName)
    {
        StartCoroutine(openScene(levelName));
    }

    //��������� ����� ����� �������� ��� ��������
    IEnumerator openScene(string sceneName)
    {
        float fadeTime = mainCamera.GetComponent<SceneFading>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(sceneName);
    }
}
