using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CreativeSpore.RpgMapEditor;
using CreativeSpore.SuperTilemapEditor;
using PixelCrushers;
using UnityEngine.SceneManagement;
using Cinemachine;
using UnityEngine.Experimental.Rendering.Universal;
using Game.Items;
using DG.Tweening;
using UnityEngine.UI;
using Game.NPCs;
using Game.NPCs.Blossoms;
using PixelCrushers.DialogueSystem;
using UnityEngine.Events;

[System.Serializable]
public struct ShippedItem
{
    public ItemBase ContainedItem;
    public int Amount;
}

public class GameManager : Singleton<GameManager>
{

    public Light2D Sun;
    public PlayerCharacter Player;
    public CinemachineVirtualCamera VC;
    public LevelInfo LevelInfo;
    public string LevelName;
    public string PreviousLevelName;
    public WindowToggle CurrentWindow = null;
    public bool Paused = false;

    public TreeBase NativeTree;
    public List<TreeBase> PossibleNativeTrees;
    public ItemBase NativeFruit;

    public int NativeTreeID;

    public bool GameStarted = false;
    public GameObject HUD;
    public GameObject QuestHUD;

    public Image BlackScreen;


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
        PreviousLevelName = SceneManager.GetActiveScene().name;
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

        LevelInfo = GameObject.FindObjectOfType<LevelInfo>();
        Collider2D coll = LevelInfo.GetComponent<PolygonCollider2D>();
        if (coll != null)
        {
            VC.GetComponent<CinemachineConfiner>().m_BoundingShape2D = coll;
            VC.GetComponent<CinemachineConfiner>().InvalidatePathCache();

        }
        Camera.main.transform.position = Player.transform.position;

        if (AstarPath.active != null)
        {
            AstarPath.active.Scan();
        }
        if (OnSceneChanged != null)
        {
            OnSceneChanged();
        }

    }

    public void StartFestival()
    {

    }
    string PreviousScene;
    Vector3 PreviousPosition = new Vector2(44, 93);
    PlayerCharacter.CharacterDirection PreviousFacing = PlayerCharacter.CharacterDirection.Down;

    public void StartCompetitionCutscene(string pSceneName)
    {
        Player.gameObject.SetActive(false);
        VC.gameObject.SetActive(false);
        PreviousScene = SceneManager.GetActiveScene().name;
        PreviousPosition = Player.transform.position;
        HUD.gameObject.SetActive(false);
        QuestHUD.GetComponent<PixelCrushers.QuestMachine.UnityUIQuestHUD>().Hide();
        SceneManager.LoadScene(pSceneName);
        PauseGame();
    }

    public void EndCompetitionCutscene(int pMinutesPassed)
    {
        SceneManager.LoadScene(PreviousScene);
        Player.transform.position = PreviousPosition;
        Player.gameObject.SetActive(true);
        VC.gameObject.SetActive(true);
        HUD.gameObject.SetActive(true);
        TimeManager.Instance.PassTime(pMinutesPassed);
        UnPauseGame();
        BlossomCompetitionManager.Instance.ShowResultsScreen();
        QuestHUD.GetComponent<PixelCrushers.QuestMachine.UnityUIQuestHUD>().Show();
    }
    public void StartCutscene(Vector3 pPlayerPos, PlayerCharacter.CharacterDirection pPlayerFacing, string pConversationName, List<EventNPCLocation> pLocations, UnityEvent pEvent)
    {
        PreviousPosition = Player.transform.position;
        PreviousFacing = Player.Direction;
        PauseGame();
        StartCoroutine(DoCutsceneStart(pPlayerPos, pPlayerFacing, pConversationName, pLocations, pEvent));
    }

    IEnumerator DoCutsceneStart(Vector3 pPlayerPos, PlayerCharacter.CharacterDirection pPlayerFacing, string pConversationName, List<EventNPCLocation> pNPCs, UnityEvent pEvent)
    {
        // FadeOut(0.5f);
        yield return new WaitForSeconds(0.5f);
        Vector3 pos = pPlayerPos;
        pos.z = ((Player.transform.position.y / 100) - (Player.transform.position.x / 1000)) - 5f;
        Player.transform.position = pos;
        Player.ChangeFacing(pPlayerFacing);
        NPCManager.Instance.SpawnEventNPCs(pNPCs);
        pEvent.Invoke();
        HUD.gameObject.SetActive(false);
        QuestHUD.GetComponent<PixelCrushers.QuestMachine.UnityUIQuestHUD>().Hide();
        yield return new WaitForSeconds(0.1f);
        FadeIn(0.5f);
        yield return new WaitForSeconds(0.5f);
        DialogueManager.StartConversation(pConversationName);
    }
    public void EndCutscene()
    {
        StartCoroutine(DoCutsceneEnd());
    }

    IEnumerator DoCutsceneEnd()
    {
        FadeOut(0.5f);
        yield return new WaitForSeconds(0.5f);
        Player.transform.position = PreviousPosition;
        PreviousPosition.z = ((PreviousPosition.y / 100) - (PreviousPosition.x / 1000)) - 5f;
        Player.ChangeFacing(PreviousFacing);
        NPCManager.Instance.DeSpawnEventNPCSs();
        HUD.gameObject.SetActive(true);
        QuestHUD.GetComponent<PixelCrushers.QuestMachine.UnityUIQuestHUD>().Show();
        yield return new WaitForSeconds(0.1f);
        FadeIn(0.5f);
        yield return new WaitForSeconds(0.5f);
        EventManager.Instance.Playing = false;
        UnPauseGame();

    }

    public void FadeOut(float pTime)
    {
        BlackScreen.CrossFadeAlpha(1, pTime, true);
    }

    public void FadeIn(float pTime)
    {
        BlackScreen.CrossFadeAlpha(0, pTime, true);

    }
    public void StartGame(string pStartingScene)
    {

        // SaveSystem.LoadScene(pStartingScene + "@Player Start");

        int NativeTreeID = Random.Range(0, PossibleNativeTrees.Count);
        //        print("Native tree id: " + NativeTreeID);
        NativeTree = PossibleNativeTrees[NativeTreeID];
        //        print(NativeTree.gameObject.name);
        NativeFruit = NativeTree.ProduceOutputs.Items[0].Item;
        PixelCrushers.DialogueSystem.DialogueLua.SetVariable(name + "NativeTreeID", NativeTreeID);
        //UnPauseGame();
        GameStarted = true;
        PixelCrushers.DialogueSystem.DialogueLua.SetVariable(name + "GameStarted", GameStarted);
        //Player.transform.position = PreviousPosition;
        LoadScene(pStartingScene, "Player Start", PlayerCharacter.CharacterDirection.Down);
        BlossomManager.Instance.GiveStarterHut();

    }

    public void LoadScene(string pSceneName, string SpawnPoint, PlayerCharacter.CharacterDirection pPlayerFacing)
    {

        PauseGame();
        StartCoroutine(DoLoadScene(pSceneName, SpawnPoint, pPlayerFacing));
    }

    IEnumerator DoLoadScene(string pSceneName, string SpawnPoint, PlayerCharacter.CharacterDirection pPlayerFacing)
    {
        string sceneNameAndSpawnPoint = pSceneName + "@" + SpawnPoint;
        FadeOut(0.5f);
        yield return new WaitForSeconds(0.5f);
        //SaveSystem.LoadScene(sceneNameAndSpawnPoint);
        SaveSystem.LoadScene(sceneNameAndSpawnPoint);
        yield return null;
        //yield return SceneManager.LoadSceneAsync(pSceneName);
        if (GameObject.Find(SpawnPoint) != null)
        {
            Vector3 pos = GameObject.Find(SpawnPoint).transform.position;
            pos.z = ((pos.y / 100) - (pos.x / 1000)) - 5f;
            Player.transform.position = pos;
        }
        Player.ChangeFacing(pPlayerFacing);
        HUD.gameObject.SetActive(false);
        QuestHUD.GetComponent<PixelCrushers.QuestMachine.UnityUIQuestHUD>().Hide();
        yield return new WaitForSeconds(0.1f);
        HUD.gameObject.SetActive(true);
        QuestHUD.GetComponent<PixelCrushers.QuestMachine.UnityUIQuestHUD>().Show();
        // SaveSystem.ApplySavedGameData();
        SceneChanged();
        if (EventManager.Instance.Playing == false)
        {
            FadeIn(0.5f);
            yield return new WaitForSeconds(0.5f);
            UnPauseGame();
        }

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
        if (Paused == false)
        {
            return;
        }
        if (EventManager.Instance.Playing)
        {
            return;
        }
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
            if (item.ContainedItem.ID == pItem.ID)
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
            newItem.ContainedItem = pItem;
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
            if (item.ContainedItem.ID == pItem.ID)
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

