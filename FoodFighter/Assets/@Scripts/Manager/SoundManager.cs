using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    private AudioSource _sfxAudioSource;
    private AudioSource _bgmAudioSource;

    public AudioClip ClickSound;
    public AudioClip EnterSound;
    public AudioClip CancelSound;
    public AudioClip PurchaseSound;
    public AudioClip CoinSound;
    public AudioClip EnemyHitSound;
    public AudioClip GameStartSound;

    public AudioClip NormalBGM;
    public AudioClip BossDungeonBGM;

    protected override void Initialize()
    {
        base.Initialize();
        _sfxAudioSource = gameObject.AddComponent<AudioSource>();
        _bgmAudioSource = gameObject.AddComponent<AudioSource>();

        ClickSound = Resources.Load<AudioClip>(Define.ClickSoundPath);
        EnterSound = Resources.Load<AudioClip>(Define.EnterSoundPath);
        CancelSound = Resources.Load<AudioClip>(Define.CancelSoundPath);
        PurchaseSound = Resources.Load<AudioClip>(Define.PurchaseSoundPath);
        CoinSound = Resources.Load<AudioClip>(Define.CoinSoundPath);
        EnemyHitSound = Resources.Load<AudioClip>(Define.EnemyHitSoundPath);
        GameStartSound = Resources.Load<AudioClip>(Define.GameStartSoundPath);

        NormalBGM = Resources.Load<AudioClip>(Define.NormalBGMPath);
        BossDungeonBGM = Resources.Load<AudioClip>(Define.CurrencyDungeonBGMPath);
    }

    public void PlayClickSound()
    {
        _sfxAudioSource.PlayOneShot(ClickSound, 0.5f);
    }

    public void PlayEnterSound()
    {
        _sfxAudioSource.PlayOneShot(EnterSound, 0.5f);
    }

    public void PlayCancelSound() => _sfxAudioSource.PlayOneShot(CancelSound, 0.5f);

    public void PlayPurchaseSound() => _sfxAudioSource.PlayOneShot(PurchaseSound, 0.3f);

    public void PlayCoinSound() => _sfxAudioSource.PlayOneShot(CoinSound, 0.2f);

    public void PlayEnemyHitSound() => _sfxAudioSource.PlayOneShot(EnemyHitSound, 0.2f);

    public void PlayBGM(AudioClip clip, float volume)
    {
        if (clip == null)
        {
            Debug.LogWarning("BGM 이 존재하지 않습니다.");
            return;
        }

        _bgmAudioSource.clip = clip;
        _bgmAudioSource.volume = volume;
        _bgmAudioSource.playOnAwake = true;
        _bgmAudioSource.loop = true;
        _bgmAudioSource.Play();
    }

    public void PlayGameStartSound() => _sfxAudioSource.PlayOneShot(GameStartSound);
}
