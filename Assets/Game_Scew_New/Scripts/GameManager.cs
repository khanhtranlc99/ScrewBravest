using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{

    public GameObject WinPanel, LosePanel, SettingsPanel;
    public Image SoundBtn, MusicBtn;
    public Sprite soundOn, soundOff, musicOn, musicOff;

    public TextMeshProUGUI TimerText;

    public float targetTime;
    public GameObject WinVfx;
    public AudioClip WinSound, loseSound, KeyCollectedSound, selectingBoltSound, PlacingBoltSound;
    public Image BackGroundImage;
    public Sprite[] BackGrounds;
    public List<GameObject> Levels = new List<GameObject>();

    Camera _camera;
    public AudioSource SoundAudioSource, MusicAudioSource;
    Bolt SelectedBolt;
    Bar[] bars;
    bool isGameFinished;
    public bool isUseDrillBooster;
    public bool isUseDestroyScewBooster;
    public GameObject prefabHold;

    public static GameManager instance;
    public GameScene gameScene;
    private void Awake()
    {
        instance = this;
        LoadCurrentLevel();
    }
    void Start()
    {
        _camera = Camera.main;
        SoundAudioSource = GetComponent<AudioSource>();
        bars = FindObjectsOfType<Bar>().ToArray();
        MusicAudioSource.volume = 0;
        //if (PlayerPrefs.GetInt("CanPlayMusic", 1) == 0)
        //{
        //    MusicBtn.sprite = musicOff;
        //    MusicAudioSource.volume = 0;
        //}
        //if (PlayerPrefs.GetInt("CanPlaySounds", 1) == 0)
        //{
        //    SoundBtn.sprite = soundOff;

        //}

        gameScene.Init();
        GameController.Instance.AnalyticsController.LoadingComplete();
        GameController.Instance.AnalyticsController.StartLevel(UseProfile.CurrentLevel);
    }


    void Update()
    {
        if (SettingsPanel.activeSelf)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }

        if (!isGameFinished)
        {
            CheckIfLevelCompleted();
            Timer();
        }



        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D[] hits = Physics2D.GetRayIntersectionAll(ray);

            if (hits.Length > 0)
            {
                BoardHole hole = null;
                Bolt bolt = null;

                for (var i = 0; i < hits.Length; i++)
                {
                    if (hits[i].transform.gameObject.layer == 6)
                    {
                        bolt = hits[i].transform.GetComponent<Bolt>();
                    
                    }
                    else if (hits[i].transform.gameObject.layer == 8)
                    {
                        hole = hits[i].transform.GetComponent<BoardHole>();
      
                    }
                    else if (hits[i].transform.gameObject.layer == 17)
                    {
                        if(isUseDrillBooster)
                        {
                            var temp = Instantiate(prefabHold);
                            var tempPost = _camera.ScreenToWorldPoint(Input.mousePosition);
                            temp.transform.position = new Vector3(tempPost.x, tempPost.y , 0);
                            isUseDrillBooster = false;
                            gameScene.BlockBooster(true);
                            if (UseProfile.CurrentLevel == 5)
                            {
                                TutBoosterDrill.Instance.Step_2();
                            }
                        }
                   
                    }

                }

                if (bolt != null && !bolt.Locked)
                {
                    if (SelectedBolt == null)
                    {
                        PlayClip(selectingBoltSound);
                        bolt.Select();
                        SelectedBolt = bolt;
                    }
                    else if (SelectedBolt != null)
                    {
                        if (SelectedBolt == bolt)
                        {
                            SelectedBolt.Deselect();
                        }
                        else
                        {
                            SelectedBolt.Deselect();
                            bolt.Select();
                            SelectedBolt = bolt;
                        }
                    }
                }
                else if (hole != null)
                {
                    if (!hole.checkIfCollidingWithBars() && !hole.Locked && !hole.Reward)
                    {
                        if (SelectedBolt != null)
                        {
                            PlayClip(PlacingBoltSound);
                            SelectedBolt.transform.position = hole.transform.position;
                            SelectedBolt.Deselect();
                            SelectedBolt = null;
                        }
                    }
                    else if (hole.Reward)
                    {
                        hole.RewardHole();
                    }
                }
            }
        }
    }






    bool CheckIfLevelCompleted()
    {
        for (int i = 0; i < bars.Length; i++)
        {
            if (bars[i].screwed) return false;
        }
        StartCoroutine(ShowWinPanel());
        isGameFinished = true;
        return true;
    }




    public void Timer()
    {
        targetTime -= Time.deltaTime;


        if (targetTime <= 0.0f)
        {
            StartCoroutine(ShowLosePanel());
        }
        else
        {
            TimeSpan time = TimeSpan.FromSeconds(targetTime);
            TimerText.text = "" + time.Minutes.ToString("00") + ":" + time.Seconds.ToString("00");
        }
    }



    void LoadCurrentLevel()
    {
        BackGroundImage.sprite = BackGrounds[PlayerPrefs.GetInt("CurrentBG", 0)];

        int currentLevel = PlayerPrefs.GetInt("CurrentLevel", 0);


        if (currentLevel >= Levels.Count)
        {
            //PlayerPrefs.SetInt("CurrentLevel", 0);
            //currentLevel = 0;
          
            GameController.Instance.moneyEffectController.SpawnEffectText_FlyUp(Vector3.zero, "New level is comming soon!", Color.white);
            Instantiate(Levels[Levels.Count -1]);
            return;
        }
        print(currentLevel);
        Instantiate(Levels[currentLevel]);
    
    }

    public bool OutLevel
    {
        get
        {
            int currentLevel = PlayerPrefs.GetInt("CurrentLevel", 0);
            if (currentLevel >= Levels.Count)
            {
                return true;
            }
            return false;
        }
    }


    public void PlayClip(AudioClip clip)
    {
        if (PlayerPrefs.GetInt("CanPlaySounds", 1) == 1) SoundAudioSource.PlayOneShot(clip);
    }

    /// ui functions ---------------------------------------------------
    IEnumerator ShowLosePanel()
    {
        PlayClip(loseSound);
        isGameFinished = true;
        yield return new WaitForSeconds(0.8f);
        LosePanel.SetActive(true);
    }
    IEnumerator ShowWinPanel()
    {
        Instantiate(WinVfx);
        PlayClip(WinSound);
        yield return new WaitForSeconds(1.5f);
        //WinPanel.SetActive(true);
        if (UseProfile.CurrentLevel == RemoteConfigController.GetIntConfig(FirebaseConfig.RATING_POPUP, 5))
        {

            DialogueRate.Setup().Show();
        }
        else
        {
            WinBox.Setup(100, false).Show();
        }
      
    }

    public void NextLevelBtn()
    {
    

        PlayerPrefs.SetInt("CurrentBG", UnityEngine.Random.Range(0, BackGrounds.Length));
        PlayerPrefs.SetInt("CurrentLevel", PlayerPrefs.GetInt("CurrentLevel", 0) + 1);
        SceneManager.LoadScene(1);
    }
    public void ReplayLevelBtn()
    {
        GameController.Instance.admobAds.ShowInterstitial(false, actionIniterClose: () =>
        {
            GameController.Instance.admobAds.ShowInterstitial();
            SceneManager.LoadScene(1);
        }, actionWatchLog: "ReplayLevelBtn");

        GameController.Instance.musicManager.PlayClickSound();
    }
    public void ReviveBtn()
    {
        GameController.Instance.admobAds.ShowVideoReward(delegate { ClaimAds(); }, delegate {
            GameController.Instance.moneyEffectController.SpawnEffectText_FlyUp(
            this.transform.position,
            "No video at the moment!",
            Color.white,
            isSpawnItemPlayer: true
            );
        }, delegate { }, ActionWatchVideo.HoldAds, UseProfile.CurrentLevel.ToString());

        void ClaimAds()
        {
            GameController.Instance.musicManager.PlayClickSound();
            isGameFinished = false;
            targetTime = 60;
            LosePanel.SetActive(false);
        }
     
    }

    private void ReviveCompleteMethod(bool completed, string advertiser)
    {
        Debug.Log("Closed rewarded from: " + advertiser + " -> Completed " + completed);
        if (completed == true)
        {
            isGameFinished = false;
            targetTime = 60;
            LosePanel.SetActive(false);
        }
        else
        {
            //no reward
        }
    }
    public void ShowSettingsPanel()
    {
        //SettingsPanel.SetActive(true);
        SettingBox.Setup().Show();
    }
    public void HideSettingsPanel()
    {
        SettingsPanel.SetActive(false);
    }

    public void SoundButton()
    {
        if (SoundBtn.sprite == soundOn)
        {
            SoundBtn.sprite = soundOff;
            PlayerPrefs.SetInt("CanPlaySounds", 0);
        }
        else
        {
            SoundBtn.sprite = soundOn;
            PlayerPrefs.SetInt("CanPlaySounds", 1);
        }
    }
    public void MusicButton()
    {
        if (MusicBtn.sprite == musicOn)
        {
            MusicBtn.sprite = musicOff;
            PlayerPrefs.SetInt("CanPlayMusic", 0);
            MusicAudioSource.volume = 0;
        }
        else
        {
            MusicBtn.sprite = musicOn;
            PlayerPrefs.SetInt("CanPlayMusic", 1);
            MusicAudioSource.volume = 0.4f;
        }
    }
}
