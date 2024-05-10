using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class UIManager : BaseScene
{
    public static UIManager INSTANCE;

    public GameObject winpanel;
    public GameObject nextbutton;
    public GameObject retrybutton;
    public TextMeshProUGUI level;
    public List<int> helplevel;

    [Header("LevelNumber Bar")] public Image lvlBgFill;
    public GameObject[] lvlNumImgs;
    public Sprite unFilled, filling;
    public List<Sprite> specialImages;
    public static int LvlNum = 0;

    public static int levelAttempts;
    public bool win;

    [Header("Help Text")]
    [Header("unpin help")]
    public GameObject PinObject;

    public GameObject FillObject;
    [Header("Key Help")] public GameObject KeyText;

    public bool help;

    public bool pin;
    public bool fill;

    public static int currentLvl = 0;
    public Button btnBoosterDrill;
    public Button btnDestroyScew;
    public Text tvCoin;
    public Text tvLevel;


    public Button btnNumDrill;
    public Button btnNumDestroyScew;
    public Text tvNumDrill;
    public Text tvNumDestroyScew;
    public Sprite spritePlus;
    public Sprite spriteNum;
    private bool lockTut;
    //public Button btnTest;
    private void Awake()
    {
        INSTANCE = this;
        LvlNum = PlayerPrefs.GetInt("levelnumber", 1);
        //LvlNum = SceneManager.GetActiveScene().buildIndex;
        LevelNumberHandler();
    }

    void Start()
    {
   
        if (helplevel.Contains(LvlNum))
        {
            if (PinObject != null && FillObject != null)
            {
                help = true;
                GameController.Instance.AnalyticsController.StartTut_1();
                lockTut = true;
            }

            if (PinObject != null && FillObject != null)
            {
                pin = true;
                fill = false;
                if (pin)
                {
                    PinObject.SetActive(true);
                }
            }
        }

        if (KeyText != null)
        {
            KeyText.SetActive(true);
        }
     
        tvCoin.text = "" + UseProfile.Coin;
        tvLevel.text = "LEVEL " + UseProfile.CurrentLevel;
        HandleShowStateBooster();
        HandleUnlock();
        // GAScript.LevelStart(PlayerPrefs.GetInt("levelnumber", 1), levelAttempts);
        // level.text = "Level " + LvlNum.ToString();
        //levelnum = currentLevel;
        //AudioManager.instance.Play("BACKGROUND");
        EventDispatcher.EventDispatcher.Instance.RegisterListener(EventID.CHANGE_COIN, HandleShowCoin);
        //btnTest.onClick.AddListener(delegate { Debug.LogError("123"); appreview1.instance.Show();  });
    }
    public void HandleUnlock()
    {
        if(UseProfile.CurrentLevel < 5)
        {
            btnBoosterDrill.gameObject.SetActive(false);
        }
        else
        {
            btnBoosterDrill.gameObject.SetActive(true);
        }
        if (UseProfile.CurrentLevel < 7)
        {
            btnDestroyScew.gameObject.SetActive(false);
        }
        else
        {
            btnDestroyScew.gameObject.SetActive(true);
        }

    }

    public void HandleShowStateBooster()
    {
        
        btnBoosterDrill.onClick.RemoveAllListeners();
        btnDestroyScew.onClick.RemoveAllListeners();
        btnNumDrill.onClick.RemoveAllListeners();
        btnNumDestroyScew.onClick.RemoveAllListeners();
        if (UseProfile.DrillBooster <= 0)
        {
            btnNumDrill.image.sprite = spritePlus;
            btnNumDrill.onClick.AddListener(delegate { SuggetBox.Setup(GiftType.DrillBooster).Show(); });
            tvNumDrill.gameObject.SetActive(false);
            btnBoosterDrill.onClick.AddListener(delegate { SuggetBox.Setup(GiftType.DrillBooster).Show(); });
        }
        else
        {
            btnNumDrill.image.sprite = spriteNum;
            tvNumDrill.gameObject.SetActive(true);
            tvNumDrill.text = "" + UseProfile.DrillBooster;
            btnBoosterDrill.onClick.AddListener(HandleBoosterDrill);

        }


        if (UseProfile.DestroyScewBooster <= 0)
        {
            btnNumDestroyScew.image.sprite = spritePlus;
            btnNumDestroyScew.onClick.AddListener(delegate { SuggetBox.Setup(GiftType.DestroyScewBooster).Show(); });
            tvNumDestroyScew.gameObject.SetActive(false);
            btnDestroyScew.onClick.AddListener(delegate { SuggetBox.Setup(GiftType.DestroyScewBooster).Show(); });
        }
        else
        {
            btnNumDestroyScew.image.sprite = spriteNum;
            tvNumDestroyScew.gameObject.SetActive(true);
            tvNumDestroyScew.text = "" + UseProfile.DestroyScewBooster;
            btnDestroyScew.onClick.AddListener(HandleBoosterDestroyScew);
        }



    }

    void Update()
    {
        /*if (levelnum == helplevel)
        {
            if (fill && !pin)
            {
                PinObject.SetActive(false);
                FillObject.SetActive(true);
            }
        }*/
        if (help)
        {
            if (fill && !pin)
            {
                PinObject.SetActive(false);
                FillObject.SetActive(true);
               
            }
        }

        if (help)
        {
            if (!fill && !pin)
            {
                PinObject.SetActive(false);
                FillObject.SetActive(false);
                if(lockTut)
                {
                    GameController.Instance.AnalyticsController.EndTut_1();
                    lockTut = false;
                }
            
            }
        }
    }

    private void LevelNumberHandler()
    {
        var lvlNumber = HandlingUnlockBasedOnLevel();
        print(lvlNumber + ":;" + LvlNum);
        lvlBgFill.DOFillAmount((lvlNumber - 1) / 5f, 0.25f).SetEase(Ease.Flash);
        for (var i = 0; i < lvlNumImgs.Length - 1; i++)
        {
            if (i < lvlNumber - 1)
            {
                lvlNumImgs[i].GetComponent<Image>().color = Color.green;
            }
            else if (i == lvlNumber - 1) lvlNumImgs[i].GetComponent<Image>().sprite = filling;
        }

        switch (lvlNumber)
        {
            case 1:
                lvlNumImgs[0].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = LvlNum.ToString();
                lvlNumImgs[1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (LvlNum + 1).ToString();
                lvlNumImgs[2].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (LvlNum + 2).ToString();
                lvlNumImgs[3].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (LvlNum + 3).ToString();
                BossLevelImg(true);
                break;
            case 2:
                lvlNumImgs[0].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (LvlNum - 1).ToString();
                lvlNumImgs[1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = LvlNum.ToString();
                lvlNumImgs[2].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (LvlNum + 1).ToString();
                lvlNumImgs[3].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (LvlNum + 2).ToString();
                BossLevelImg(false);
                break;
            case 3:
                lvlNumImgs[0].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (LvlNum - 2).ToString();
                lvlNumImgs[1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (LvlNum - 1).ToString();
                lvlNumImgs[2].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = LvlNum.ToString();
                lvlNumImgs[3].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (LvlNum + 1).ToString();
                BossLevelImg(false);
                break;
            case 4:
                lvlNumImgs[0].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (LvlNum - 3).ToString();
                lvlNumImgs[1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (LvlNum - 2).ToString();
                lvlNumImgs[2].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (LvlNum - 1).ToString();
                lvlNumImgs[3].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = LvlNum.ToString();
                BossLevelImg(false);
                break;
            case 5:
                lvlNumImgs[0].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (LvlNum - 4).ToString();
                lvlNumImgs[1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (LvlNum - 3).ToString();
                lvlNumImgs[2].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (LvlNum - 2).ToString();
                lvlNumImgs[3].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (LvlNum - 1).ToString();
                BossLevelImg(false);
                break;
        }
    }

    private void BossLevelImg(bool call)
    {
        var lvlNo = PlayerPrefs.GetInt("SpecialImg", 0);
        if (LvlNum > 5 && call && currentLvl != LvlNum)
        {
            lvlNo = lvlNo >= specialImages.Count - 1 ? specialImages.Count - 1 : ++lvlNo;
            currentLvl = LvlNum;
        }

        //  lvlNumImgs[^1].GetComponent<Image>().sprite = specialImages[lvlNo];
        PlayerPrefs.SetInt("SpecialImg", lvlNo);
        print(lvlNo + "::");
    }


    private int HandlingUnlockBasedOnLevel()
    {
        var currentlvlnum = 0;

        var temp = LvlNum;

        if (temp.ToString().Length > 1)
        {
            if (temp.ToString().EndsWith("1") || temp.ToString().EndsWith("6"))
            {
                currentlvlnum = 1;
            }
            else if (temp.ToString().EndsWith("2") || temp.ToString().EndsWith("7"))
            {
                currentlvlnum = 2;
            }
            else if (temp.ToString().EndsWith("3") || temp.ToString().EndsWith("8"))
            {
                currentlvlnum = 3;
            }
            else if (temp.ToString().EndsWith("4") || temp.ToString().EndsWith("9"))
            {
                currentlvlnum = 4;
            }
            else if (temp.ToString().EndsWith("5") || temp.ToString().EndsWith("0"))
            {
                currentlvlnum = 5;
            }
        }
        else
        {
            if (temp.ToString().StartsWith("1") || temp.ToString().StartsWith("6"))
            {
                currentlvlnum = 1;
            }
            else if (temp.ToString().StartsWith("2") || temp.ToString().StartsWith("7"))
            {
                currentlvlnum = 2;
            }
            else if (temp.ToString().StartsWith("3") || temp.ToString().StartsWith("8"))
            {
                currentlvlnum = 3;
            }
            else if (temp.ToString().StartsWith("4") || temp.ToString().StartsWith("9"))
            {
                currentlvlnum = 4;
            }
            else if (temp.ToString().StartsWith("5") || temp.ToString().StartsWith("0"))
            {
                currentlvlnum = 5;
            }
        }

        return currentlvlnum;
    }

    public void NextlevelButton()
    {


        if (AudioManager.instance)
        {
            AudioManager.instance.Play("Fill");
            GameManager_Scew_Old.instance.vibration();
        }

        Debug.Log($"Level Attempts::{levelAttempts}");
        levelAttempts = 0;
        //NEXT BUTTON CALL
        if (PlayerPrefs.GetInt("Level", 1) >= SceneManager.sceneCountInBuildSettings - 1)
        {
            SceneManager.LoadScene(Random.Range(0, SceneManager.sceneCountInBuildSettings - 1));
            PlayerPrefs.SetInt("Level", (PlayerPrefs.GetInt("Level", 1) + 1));
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            PlayerPrefs.SetInt("Level", (PlayerPrefs.GetInt("Level", 1) + 1));
        }

        PlayerPrefs.SetInt("levelnumber", PlayerPrefs.GetInt("levelnumber", 1) + 1);
    }

    public void retryButton()
    {
        GameController.Instance.admobAds.ShowInterstitial(false, actionIniterClose: () =>
        {    
            Reset();
        }, actionWatchLog: "ResetLevel");

        void Reset()
        {
            if (AudioManager.instance)
            {
                AudioManager.instance.Play("Fill");
                GameManager_Scew_Old.instance.vibration();
            }

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            levelAttempts++;
            Debug.Log($"Level Attempts::{levelAttempts}");
        }

   
    }

    public void Winpanel()
    {
        StartCoroutine(winactive());
    }

    IEnumerator winactive()
    {
        yield return new WaitForSeconds(0.5f);
        //winpanel.SetActive(true);
        //StartCoroutine(nextWait());
        if (UseProfile.CurrentLevel == RemoteConfigController.GetIntConfig(FirebaseConfig.RATING_POPUP, 5))
        {
            
            DialogueRate.Setup().Show();
        }
        else
        {
            WinBox.Setup(100, false).Show();
        }
     
    }
    public void HandleBoosterDrill()
    {
        TouchDrop.instance.useBoosterDrill = true;
        UseProfile.DrillBooster -= 1;
        HandleShowStateBooster();
        BlockBooster(false);
        TutDrillBooster.Instance.MoveTutDrill();
    }
    public void HandleBoosterDestroyScew()
    {
        TouchDrop.instance.useBoosterDestroyScew = true;
        UseProfile.DestroyScewBooster -= 1;
        HandleShowStateBooster();
        BlockBooster(false);
        TutDestroyBooster.Instance.MoveTutDestroy();
    }
    public void ShowSetting()
    {
        SettingBox.Setup().Show();
    }

    public void BlockBooster(bool param)
    {
        btnNumDrill.interactable = param;
        btnBoosterDrill.interactable = param;
        btnNumDestroyScew.interactable = param;
        btnDestroyScew.interactable = param;
    }

    public void CheatWin()
    {
        WinBox.Setup(100, false).Show();
    }
    public void Test()
    {
        GameController.Instance.appReview.RateAndReview();
    }

    public override void OnEscapeWhenStackBoxEmpty()
    {
  
    }
    private void HandleShowCoin(object param)
    {
        tvCoin.text = "" + UseProfile.Coin;

    }
    private void OnDestroy()
    {
        EventDispatcher.EventDispatcher.Instance.RemoveListener(EventID.CHANGE_COIN, HandleShowCoin);
    }
}