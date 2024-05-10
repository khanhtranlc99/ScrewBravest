using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;

public class TouchDrop : MonoBehaviour
{
    public static TouchDrop instance;

    [SerializeField] public bool start;

    private RaycastHit _hit;
    [Header("Scripts")]
    public GameManager_Scew_Old gameManager;

    public float boltremoveheight;

    private Vector3 offset;

    //public int touchcount;

    public bool blocking;
    public GameObject Block1;
    public GameObject Block2;

    public AudioManager audioManager;
    public GameObject hold;
    public bool useBoosterDrill;
    public bool useBoosterDestroyScew;
    public float zBooster;
    public GameObject holdInGame;
    private void Awake()
    {
        instance = this;
        boltremoveheight = 2f;
    }

    void Start()
    {
        gameManager = GameManager_Scew_Old.instance;

        if (AudioManager.instance)
        {
            audioManager = AudioManager.instance;
        }

        if (gameManager.gamemodes == GameManager_Scew_Old.Modes.Null)
        {
            start = true;
        }
        holdInGame = GameObject.FindGameObjectWithTag("Fill");
      
    }


    void Update()
    {
        /*if (blocking && Block1.GetComponent<Rigidbody2D>())
        {
            Block1.GetComponent<Rigidbody2D>().simulated = true;
        }

        /*if (!blocking)
        {
            Block1.GetComponent<Rigidbody2D>().simulated = false;
        }#1#*/
        if (Input.GetMouseButtonDown(0))
        {

            if (!UIManager.INSTANCE.win && start)
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out _hit))
                {
                    if (_hit.collider.gameObject.CompareTag("BOLT"))
                    {
                        if (gameManager.gamestate == GameManager_Scew_Old.State.Done)
                        {
                            var selectObject = _hit.collider.gameObject;
                            Boltshifting(selectObject);
                        }

                    }

                    if (_hit.collider.gameObject.CompareTag("Fill"))
                    {
                        if (gameManager.gamestate == GameManager_Scew_Old.State.Done)
                        {
                            var fillingplace = _hit.collider.gameObject;
                            Filling(fillingplace);
                            print(" " + _hit.collider.gameObject.name);
                        }
                    }

                    if (_hit.collider.gameObject.CompareTag("FillAds"))
                    {
                        if (gameManager.gamestate == GameManager_Scew_Old.State.Done)
                        {
                            var fillingplace = _hit.collider.gameObject;
                            fillingplace.GetComponent<FillAds>().OnClick();
                        }
                    }
                }
            }
            if (useBoosterDrill)
            {

                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out _hit))
                {        
                    if (_hit.collider.gameObject.CompareTag("Broad"))
                    {
                        Vector3 spawnPosition = _hit.point;
                        GameObject spawnedObject =  Instantiate(hold, spawnPosition, Quaternion.identity);
                        Vector3 surfaceNormal = _hit.normal;
                        spawnedObject.transform.up = surfaceNormal;
                        var temp = spawnedObject.transform.position;
                        spawnedObject.transform.position = new Vector3(temp.x, temp.y, temp.z );
                        spawnedObject.transform.parent = holdInGame.transform.parent;
                        spawnedObject.transform.position = new Vector3(temp.x, temp.y, holdInGame.transform.position.z);
                        useBoosterDrill = false;
                        UIManager.INSTANCE.BlockBooster(true);
                        TutDrillBooster.Instance.OffTutDrill();
                    }
                }

            }
            if (useBoosterDestroyScew)
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out _hit))
                {
                    if (_hit.collider.gameObject.CompareTag("BOLT"))
                    {

                        HandleBoosterDestroy(new Vector3(100, 100, 100));
                    }

                }
            }
        }
        //if(Input.GetKeyDown(KeyCode.Space))
        //{
        //    UseProfile.CurrentLevel += 1;
        //    if (UseProfile.CurrentLevel < 60)
        //    {
        //        SceneManager.LoadScene("Level " + UseProfile.CurrentLevel);
        //    }
        //    else
        //    {
        //        SceneManager.LoadScene("Level " + UnityEngine.Random.Range(1, 60));
        //    }
        //}
    }
    public void HandleFillAds()
    {


    }
    // ReSharper disable Unity.PerformanceAnalysis
    public void Filling(GameObject fillingreferance)
    {
        // Debug.LogError("Fill");
        if (gameManager.dupPlug != null)
        {
            if (UIManager.INSTANCE.fill)
            {
                UIManager.INSTANCE.fill = false;
            }
            gameManager.vibration();
            var parent = gameManager.dupPlug.transform.parent;
            var position1 = fillingreferance.transform.position;

            gameManager.dupcolider.transform.localPosition = parent.transform.localPosition;
            gameManager.dupcolider.GetComponent<CircleCollider2D>().enabled = true;
            parent.GetComponent<CircleCollider2D>().enabled = false;

            //gameManager.dupPlug.GetComponentInParent<NewBolt>().enabled = false;
            //gameManager.dupPlug.GetComponent<NewBolt>().connectedBodylist.Clear();
            parent.DOMoveX(position1.x, 0.25f);
            parent.DOMoveY(position1.y, 0.25f).SetEase(Ease.Linear).OnComplete(() =>
            {

                audioManager.Play("Fill");
                Instantiate(gameManager.fillPartical, new Vector3(position1.x, position1.y, position1.z - 1.5f),
                    new Quaternion(0f, 0f, 0f, 0f));

                parent.transform.DOLocalMoveZ(
                        parent.transform.localPosition.z + boltremoveheight, 0.3f)
                    .SetEase(Ease.Linear).OnComplete(() =>
                    {
                        gameManager.dupcolider.GetComponent<CircleCollider2D>().enabled = false;
                        parent.GetComponent<CircleCollider2D>().enabled = true;
                        gameManager.gamestate = GameManager_Scew_Old.State.Done;
                    });
            });
            gameManager.dupPlug = null;
        }
    }
    // ReSharper disable Unity.PerformanceAnalysis
    public void Boltshifting(GameObject boltreferance)
    {
        // Debug.LogError("Push");
        if (gameManager.dupPlug != null)
        {
            gameManager.vibration();
            audioManager.Play("Bolt");
            if (UIManager.INSTANCE.pin)
            {
                UIManager.INSTANCE.pin = false;
                UIManager.INSTANCE.fill = true;
            }
            if (gameManager.dupPlug == (boltreferance))
            {

                gameManager.gamestate = GameManager_Scew_Old.State.Select;
                gameManager.dupPlug = null;
                var parent = boltreferance.transform.parent;
                parent.DOLocalMoveZ(parent.localPosition.z + boltremoveheight, 0.3f).SetEase(Ease.Linear).OnComplete(
                    () =>
                    {
                        gameManager.gamestate = GameManager_Scew_Old.State.Done;
                    });
                //gameManager.gamestate = GameManager.State.Select;

            }
            else if (gameManager.dupPlug != (boltreferance))
            {
                gameManager.gamestate = GameManager_Scew_Old.State.Select;
                var parent = gameManager.dupPlug.transform.parent;
                parent.DOLocalMoveZ(parent.localPosition.z + boltremoveheight, 0.3f).SetEase(Ease.Linear);
                gameManager.dupPlug = null;
                gameManager.dupPlug = boltreferance;
                var parent1 = boltreferance.transform.parent;
                //boltreferance.transform.GetComponent<Collider>().enabled = false;
                parent1.DOLocalMoveZ(parent1.localPosition.z - boltremoveheight, 0.3f).SetEase(Ease.Linear).OnComplete(
                    () =>
                    {
                        gameManager.gamestate = GameManager_Scew_Old.State.Done;
                    });
            }
        }

        else if (gameManager.dupPlug == null)
        {
            gameManager.gamestate = GameManager_Scew_Old.State.Select;
            if (UIManager.INSTANCE.pin)
            {
                UIManager.INSTANCE.pin = false;
                UIManager.INSTANCE.fill = true;
            }
            gameManager.vibration();
            audioManager.Play("Bolt");
            var parent = boltreferance.transform.parent;
            parent.DOLocalMoveZ(parent.localPosition.z - boltremoveheight, 0.3f).SetEase(Ease.Linear).OnComplete(() =>
            {
                gameManager.gamestate = GameManager_Scew_Old.State.Done;
            });
            gameManager.dupPlug = boltreferance;
        }
    }

    public void HandleBoosterDestroy(Vector3 fillingreferance)
    {
        // Debug.LogError("Fill");
        if (gameManager.dupPlug != null)
        {
            if (UIManager.INSTANCE.fill)
            {
                UIManager.INSTANCE.fill = false;
            }
            gameManager.vibration();
            var parent = gameManager.dupPlug.transform.parent;
            var position1 = fillingreferance;

            gameManager.dupcolider.transform.localPosition = parent.transform.localPosition;
            gameManager.dupcolider.GetComponent<CircleCollider2D>().enabled = true;
            parent.GetComponent<CircleCollider2D>().enabled = false;

            //gameManager.dupPlug.GetComponentInParent<NewBolt>().enabled = false;
            //gameManager.dupPlug.GetComponent<NewBolt>().connectedBodylist.Clear();
            parent.DOMoveX(position1.x, 0.25f);
            parent.DOMoveY(position1.y, 0.25f).SetEase(Ease.Linear).OnComplete(() =>
            {

                audioManager.Play("Fill");
                Instantiate(gameManager.fillPartical, new Vector3(position1.x, position1.y, position1.z - 1.5f),
                    new Quaternion(0f, 0f, 0f, 0f));

                parent.transform.DOLocalMoveZ(
                        parent.transform.localPosition.z + boltremoveheight, 0.3f)
                    .SetEase(Ease.Linear).OnComplete(() =>
                    {
                        gameManager.dupcolider.GetComponent<CircleCollider2D>().enabled = false;
                        parent.GetComponent<CircleCollider2D>().enabled = true;
                        gameManager.gamestate = GameManager_Scew_Old.State.Done;
                    });
            });
            gameManager.dupPlug = null;

            useBoosterDestroyScew = false;
            UIManager.INSTANCE.BlockBooster(true);
            TutDestroyBooster.Instance.OffTutDestroy();
        }
    }
    public void HandleLogicDrill(Vector3 param)
    {
        //Instantiate(hold, param, Quaternion.identity);.
        var temp = Instantiate(hold);
        temp.transform.position = new Vector3(param.x, param.y, -0.5f);
        useBoosterDrill = false;
    }
    public GameObject param;
    public List<GameObject> gameObjects;
    public Material tempMt;
    [Button]
    private void Test()
    {
        //param = GameObject.Find("Board");

        //param.AddComponent<BoxCollider>().size = new Vector3(0.85f,0.85f,0.85f);
        //param.gameObject.tag = "Broad";
        var temp = GameObject.FindObjectsOfType<GameObject>();
        gameObjects = new List<GameObject>();
        foreach (var item in temp)
        {
            if(item.name == "polySurface1")
            {
                gameObjects.Add(item);
            }
            //item.GetComponent<MeshRenderer>().material = tempMt;
        }
        foreach (var item in gameObjects)
        {
         
            item.GetComponent<MeshRenderer>().material = tempMt;
        }

    }

    private string sceneName;
    public int idLevel;
    [Button]
    private void ChangeScene()
    {

        sceneName = "";
        if (UseProfile.CurrentLevel < 60)
        {
            sceneName = "Level " + idLevel;
        }
        else
        {
            sceneName = "Level " + idLevel;
        }
        SceneManager.LoadScene(sceneName);
    }


}