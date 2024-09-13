using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct LocationSkin
{
    public Sprite gameBoardSkin;
    public Sprite gameBgSkin;
    public Sprite chipLineSkin;
    public Sprite chipLineBgSkin;
    public Sprite sidePanelsSkin;
    public Sprite horizontalPanelSkin;
}

public class GameSettings : MonoBehaviour
{

    [Header("Current Level Settings")]
    [Tooltip("���������� ����� �� ������")]
    [SerializeField] private int currentStepCount;
    [Tooltip("����� �� ���")]
    [SerializeField] private int currentTimeStep;
    [Tooltip("���������� ����� ������� ����� ������� ��� ������")]
    [SerializeField] private int currentNeededScore;
    [SerializeField] private float currentCoinsKoef = 2f;
    private bool isTutorial;

    [Header("Chip Skins Settings")]
    [SerializeField] private item[] defaultItems;
    [SerializeField] private item[] Skin1Items;
    [SerializeField] private item[] Skin2Items;
    [SerializeField] private item[] Skin3Items;
    [SerializeField] private item[] Skin4Items;
    [SerializeField] private item[] Skin5Items;

    [Header("Bg Skins Settings")]
    [SerializeField] private LocationSkin[] locationSkins;


    [Header("Debug")]
    [SerializeField]
    private int thisLvlInd;

    public static GameSettings instance;

    void Awake()
    {
        if (!instance)
            instance = this;
        else
            Destroy(this.gameObject);


        DontDestroyOnLoad(this.gameObject);
    }

    //������� �������� ������
    public int ThisLvlIndex
    {
        get
        {
            return thisLvlInd;
        }
        set
        {
            this.thisLvlInd = value;
        }
    }

    //�������� ��
    public bool IsTutorial
    {
        get { return isTutorial; }
        set { isTutorial = value; }
    }

    //����������� �������� ����������� ������
    public int CurrentOpenLvlIndex
    {
        get
        {
            if (PlayerPrefs.HasKey($"currentOpenLvl"))
            {
                return PlayerPrefs.GetInt($"currentOpenLvl");
            }
            else
            {
                PlayerPrefs.SetInt($"currentOpenLvl", -1);
                return -1;
            }
        }
        set
        {
            PlayerPrefs.SetInt($"currentOpenLvl", value);
        }
    }

    //����������� �������� ��������� ������
    public float MusicVolume
    {
        get
        {
            if (!PlayerPrefs.HasKey("MusicVolume"))
            {
                PlayerPrefs.SetFloat("MusicVolume", 1f);
            }
            return PlayerPrefs.GetFloat("MusicVolume");
        }

        set
        {
            if (value > 1f) value = 1f;
            if (value < 0f) value = 0f;
            PlayerPrefs.SetFloat("MusicVolume", value);
        }
    }

    //����������� �������� ��������� ������
    public float SoundVolume
    {
        get
        {
            if (!PlayerPrefs.HasKey("SoundVolume"))
            {
                PlayerPrefs.SetFloat("SoundVolume", 1f);
            }
            return PlayerPrefs.GetFloat("SoundVolume");
        }

        set
        {
            if (value > 1f) value = 1f;
            if (value < 0f) value = 0f;
            PlayerPrefs.SetFloat("SoundVolume", value);
        }
    }

    //����������� �������� �����
    public int Coins
    {
        get
        {
            if (!PlayerPrefs.HasKey("Coins"))
            {
                PlayerPrefs.SetInt("Coins", 0);
            }
            return PlayerPrefs.GetInt("Coins");
        }

        set
        {
            PlayerPrefs.SetInt("Coins", value);
        }
    }

    //����������� �������� �����
    public int Gems
    {
        get
        {
            if (!PlayerPrefs.HasKey("Gems"))
            {
                PlayerPrefs.SetInt("Gems", 0);
            }
            return PlayerPrefs.GetInt("Gems");
        }

        set
        {
            PlayerPrefs.SetInt("Gems", value);
        }
    }

    //������������ �������� ���������� ����� �� �������
    public int StepCount
    {
        get { return currentStepCount; }
        set { currentStepCount = value; }
    }

    //������������ �������� ������� �� ���
    public int TimeStep
    {
        get { return currentTimeStep; }
        set { currentTimeStep = value; }
    }

    //������������ �������� ������� �������
    public int NeededScore
    {
        get { return currentNeededScore; }
        set { currentNeededScore = value; }
    }

    //������������ �������� ����������� ����� ������������ �������
    public float CurrentCoinsKoef
    {
        get { return currentCoinsKoef; }
        set { currentCoinsKoef = value; }
    }

    //����������� �������� ���������� ����
    public int BombCount
    {
        get
        {
            if (!PlayerPrefs.HasKey("BombCount"))
            {
                PlayerPrefs.SetInt("BombCount", 0);
            }
            return PlayerPrefs.GetInt("BombCount");
        }
        set
        {
            PlayerPrefs.SetInt("BombCount", value);
        }
    }

    //����������� �������� ���������� ������
    public int LightningCount
    {
        get
        {
            if (!PlayerPrefs.HasKey("LightningCount"))
            {
                PlayerPrefs.SetInt("LightningCount", 0);
            }
            return PlayerPrefs.GetInt("LightningCount");
        }
        set
        {
            PlayerPrefs.SetInt("LightningCount", value);
        }
    }

    //����������� �������� ���������� ������
    public int SwapCount
    {
        get
        {
            if (!PlayerPrefs.HasKey("SwapCount"))
            {
                PlayerPrefs.SetInt("SwapCount", 0);
            }
            return PlayerPrefs.GetInt("SwapCount");
        }
        set
        {
            PlayerPrefs.SetInt("SwapCount", value);
        }
    }

    //����������� �������� ���������� ������� �����
    public int DoublePointsCount
    {
        get
        {
            if (!PlayerPrefs.HasKey("DoublePointsCount"))
            {
                PlayerPrefs.SetInt("DoublePointsCount", 0);
            }
            return PlayerPrefs.GetInt("DoublePointsCount");
        }
        set
        {
            PlayerPrefs.SetInt("DoublePointsCount", value);
        }
    }

    //����������� �������� ���������� ��� ����
    public int ExtraMoveCount
    {
        get
        {
            if (!PlayerPrefs.HasKey("ExtraMoveCount"))
            {
                PlayerPrefs.SetInt("ExtraMoveCount", 0);
            }
            return PlayerPrefs.GetInt("ExtraMoveCount");
        }
        set
        {
            PlayerPrefs.SetInt("ExtraMoveCount", value);
        }
    }

    //����������� �������� ���������� ���������
    public int FreezeCount
    {
        get
        {
            if (!PlayerPrefs.HasKey("FreezeCount"))
            {
                PlayerPrefs.SetInt("FreezeCount", 0);
            }
            return PlayerPrefs.GetInt("FreezeCount");
        }
        set
        {
            PlayerPrefs.SetInt("FreezeCount", value);
        }
    }

    //����������� �������� ���������� �����
    public int CurrentSkinId
    {
        get
        {
            if (!PlayerPrefs.HasKey("CurrentSkin"))
            {
                PlayerPrefs.SetInt("CurrentSkin", 0);
            }
            return PlayerPrefs.GetInt("CurrentSkin");
        }

        set
        {
            PlayerPrefs.SetInt("CurrentSkin", value);
        }
    }

    //���������� ������� ������������� ����
    public item[] GetCurrentItem()
    {
        switch (CurrentSkinId)
        {
            case 0:
                return defaultItems;
            case 1:
                return Skin1Items;
            case 2:
                return Skin2Items;
            case 3:
                return Skin3Items;
            case 4:
                return Skin4Items;
            case 5:
                return Skin5Items;
            default:
                return defaultItems;

        }
    }

    //����������� �������� ����� �������
    public int CurrentLocSkinId
    {
        get
        {
            if (!PlayerPrefs.HasKey("CurrentLocSkinId"))
            {
                PlayerPrefs.SetInt("CurrentLocSkinId", 0);
            }
            return PlayerPrefs.GetInt("CurrentLocSkinId");
        }
        set
        {
            PlayerPrefs.SetInt("CurrentLocSkinId", value);
        }
    }

    //���������� ������� ���� �������
    public LocationSkin GetCurrentLocSkin()
    {
        return locationSkins[CurrentLocSkinId];
    }

    //������ ����� ���� �����
    public bool IsFirstTutorial
    {
        get
        {
            if (!PlayerPrefs.HasKey("IsFirstTutorial"))
            {
                PlayerPrefs.SetInt("IsFirstTutorial", 1);
            }
            if (PlayerPrefs.GetInt("IsFirstTutorial") == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        set
        {
            if (value)
            {
                PlayerPrefs.SetInt("IsFirstTutorial", 1);
            }
            else
            {
                PlayerPrefs.SetInt("IsFirstTutorial", 0);
            }
        }
    }

    //������������� ����������� �����
    public void SetSinglePlayer()
    {
        NeededScore = 0;
        ThisLvlIndex = -1;
        StepCount = 0;
        TimeStep = 10;
        CurrentCoinsKoef = 1.2f;
        IsTutorial = false;
    }

    //������������� ����� ��������
    public void SetCompaign() 
    {
        CurrentCoinsKoef = 2f;
        IsTutorial = false;
    }

    //������������� ��������
    public void SetTutorial()
    {
        NeededScore = 24;
        ThisLvlIndex = -1;
        StepCount = 2;
        TimeStep = 45;
        CurrentCoinsKoef = 1f;
        IsTutorial = true;
    }

    //������ ������� �������
    public void CompleteThisLvl()
    {
        if (ThisLvlIndex > CurrentOpenLvlIndex && ThisLvlIndex != -1)
        {
            CurrentOpenLvlIndex = ThisLvlIndex;
        }
    }

}
