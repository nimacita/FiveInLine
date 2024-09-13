using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    [Header("Bg Music")]
    [SerializeField] private AudioSource BgMusic;

    [Header("Boosters Sounds")]
    [SerializeField] private AudioSource bombSound;
    [SerializeField] private AudioSource lightningSound;
    [SerializeField] private AudioSource swapSound;
    [SerializeField] private AudioSource doubleSound;
    [SerializeField] private AudioSource extraMoveSound;
    [SerializeField] private AudioSource freezeSound;

    [Header("Game Sounds")]
    [SerializeField] private AudioSource victorySound;
    [SerializeField] private AudioSource defeatSound;
    [SerializeField] private AudioSource putLineSound;
    [SerializeField] private AudioSource[] popSounds;

    [Header("Coins")]
    [SerializeField] private AudioSource coinSound;
    [SerializeField] private AudioSource collectedSound;
    [SerializeField] private AudioSource equipedSound;

    public static SoundController instance;

    void Awake()
    {
        if (!instance)
            instance = this;
        else
            Destroy(this.gameObject);


        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        PlayBgMusic();
    }

    //играме музыку
    private void PlayBgMusic()
    {
        BgMusic.volume = GameSettings.instance.MusicVolume;
        BgMusic.Play();
    }

    //настройка звука мущыки
    public void ChangeMusicSound(float volume)
    {
        BgMusic.volume = volume;
    }

    //играем выбранный звук
    private void PlayCurrSound(AudioSource sound)
    {
        sound.volume = GameSettings.instance.SoundVolume;
        sound.Play();
    }

    public void PlayVictorySound() { PlayCurrSound(victorySound); }
    public void PlayDefeatSound() { PlayCurrSound(defeatSound); }
    public void PlayBombSound() { PlayCurrSound(bombSound); }
    public void PlayLightningSound() { PlayCurrSound(lightningSound); }
    public void PlaySwapSound() { PlayCurrSound(swapSound); }
    public void PlayDoubleSound() { PlayCurrSound(doubleSound); }
    public void PlayExtraMoveSound() { PlayCurrSound(extraMoveSound); }
    public void PlayFreezeSound() { PlayCurrSound(freezeSound); }
    public void PlayPutLineSound() { PlayCurrSound(putLineSound); }
    public void PlayCoinSound() { PlayCurrSound(coinSound); }
    public void PlayCollectedSound() { PlayCurrSound(collectedSound); }
    public void PlayEquipedSound() { PlayCurrSound(equipedSound); }

    public void PlayPopSound()
    {
        PlayCurrSound(popSounds[Random.Range(0, popSounds.Length)]);
    }

}
