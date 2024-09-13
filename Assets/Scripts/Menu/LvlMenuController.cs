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

    //выход в меню
    public void LevlMenuOff()
    {
        MenuController.instance.MenuOn();
        StartCoroutine(LevelMenuOffAnim());
    }

    private IEnumerator LevelMenuOffAnim()
    {
        //играем анимацию
        lvlMenuAnim.Play(lvlMenuOff.name);
        yield return new WaitForSeconds(lvlMenuOff.length);
        lvlMenuView.SetActive(false);
    }

    //включаем меню уровней
    public void LevelMenuOn()
    {
        //играем анимацию
        lvlMenuAnim.Play(lvlMenuOn.name);
    }

    //нажатие на кнопку уровня
    public void LvlBtnClick(string levelName)
    {
        StartCoroutine(openScene(levelName));
    }

    //открываем сцену после задержки для перехода
    IEnumerator openScene(string sceneName)
    {
        float fadeTime = mainCamera.GetComponent<SceneFading>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(sceneName);
    }
}
