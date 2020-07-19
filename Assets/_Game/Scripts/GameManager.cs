using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CreativeSpore.RpgMapEditor;
using CreativeSpore.SuperTilemapEditor;
using PixelCrushers;
using UnityEngine.SceneManagement;
using Cinemachine;
using UnityEngine.Experimental.Rendering.Universal;
using ItemSystem;

[System.Serializable]
public struct ShippedItem
{
    public ItemBase Item;
    public int Amount;
}

public class GameManager : Singleton<GameManager>
{

    public Light2D Sun;
    public PlayerCharacter Player;
    public CinemachineVirtualCamera VC;
    public LevelInfo LevelInfo;
    public string LevelName;

    public LevelInfo PreviousLevelInfo;

    public WindowToggle CurrentWindow = null;
    public bool Paused = false;

    public TreeBase NativeTree;
    public List<TreeBase> PossibleNativeTrees;
    public ItemBase NativeFruit;

    public int NativeTreeID;

    public bool GameStarted = false;

    // Use this for initialization
    void Awake()
    {
        Init();
    }

    public override void Init()
    {
        PauseGame();
        SceneChanged();
        StartCoroutine("LoadTitle");
    }
    private IEnumerator LoadTitle()
    {

        yield return new WaitForSeconds(0.1f);
        SaveSystem.LoadScene("Title");

    }

    void OnEnable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += delegate
        {
            SceneChanged();
        };

        UnityEngine.SceneManagement.SceneManager.sceneUnloaded += delegate
        {
            SceneWillChange();
        };
    }


    void OnDisable()
    {

    }

    public delegate void SceneUnload();

    public static event SceneChange OnSceneUnloaded;

    void SceneWillChange()
    {
        if (OnSceneUnloaded != null)
        {
            OnSceneUnloaded();
        }
    }
    public delegate void SceneChange();

    public static event SceneChange OnSceneChanged;

    void SceneChanged()
    {
        LevelName = SceneManager.GetActiveScene().name;
        PreviousLevelInfo = LevelInfo;
        PreviousLevelInfo.Name = LevelInfo.Name;

        LevelInfo = GameObject.FindObjectOfType<LevelInfo>();
        Collider2D coll = LevelInfo.GetComponent<PolygonCollider2D>();
        if (coll != null)
        {
            VC.GetComponent<CinemachineConfiner>().m_BoundingShape2D = coll;
            VC.GetComponent<CinemachineConfiner>().InvalidatePathCache();

        }
        Camera.main.transform.position = Player.transform.position;
        if (OnSceneChanged != null)
        {
            OnSceneChanged();
        }
        if (AstarPath.active != null)
        {
            AstarPath.active.Scan();
        }

    }

    public void StartGame(string pStartingScene)
    {
        print("start game");

        SaveSystem.LoadScene(pStartingScene + "@Player Start");
        int NativeTreeID = Random.Range(0, PossibleNativeTrees.Count);
        print("Native tree id: " + NativeTreeID);
        NativeTree = PossibleNativeTrees[NativeTreeID];
        print(NativeTree.gameObject.name);
        NativeFruit = NativeTree.ProduceOutputs.Items[0].Item.item;
        PixelCrushers.DialogueSystem.DialogueLua.SetVariable(name + "NativeTreeID", NativeTreeID);
        UnPauseGame();
        GameStarted = true;
        PixelCrushers.DialogueSystem.DialogueLua.SetVariable(name + "GameStarted", GameStarted);


    }

    public void PauseGame()
    {
        TimeManager.Instance.ToggleTime(false);
        Player.GetComponent<PlayerCharacter>().enabled = false;
        foreach (Animator anim in Player.Animators)
        {
            anim.SetFloat("Speed", 0);
        }
        Player.GetComponentInChildren<PlayerInteractionDetector>().enabled = false;
        Paused = true;
    }
    public void UnPauseGame()
    {
        if (PixelCrushers.DialogueSystem.DialogueManager.IsConversationActive) return;
        if (CurrentWindow != null && CurrentWindow.PauseWhileOpen) return;

        TimeManager.Instance.ToggleTime(true);
        Player.GetComponent<PlayerCharacter>().enabled = true;
        Player.GetComponentInChildren<PlayerInteractionDetector>().enabled = true;

        Paused = false;
    }


    [SerializeField]
    public List<ShippedItem> ShippedItems = new List<ShippedItem>();
    public void AddShippedItem(ItemBase pItem, int pAmount)
    {
        ShippedItem existingItem = new ShippedItem();
        bool match = false;

        foreach (ShippedItem item in ShippedItems)
        {
            if (item.Item.itemID == pItem.itemID)
            {
                existingItem = item;
                match = true;
            }
        }

        if (match == true)
        {
            existingItem.Amount += pAmount;

        }
        else
        {
            ShippedItem newItem = new ShippedItem();
            newItem.Item = pItem;
            newItem.Amount = pAmount;
            ShippedItems.Add(newItem);

        }

    }

    public int GetShippedItemAmount(ItemBase pItem)
    {

        ShippedItem existingItem = new ShippedItem();
        bool match = false;
        foreach (ShippedItem item in ShippedItems)
        {
            if (item.Item.itemID == pItem.itemID)
            {
                existingItem = item;
                match = true;
            }
        }
        if (match == true)
        {
            return existingItem.Amount;
        }
        else
        {
            return 0;
        }
    }
}

