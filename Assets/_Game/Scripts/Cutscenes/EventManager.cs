using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
public class EventManager : MonoBehaviour
{

    GameEvent[] Events = new GameEvent[0];
    public List<GameEvent> EligibleEvents = new List<GameEvent>();
    List<GameEvent> PlayedEvents = new List<GameEvent>();
    string CurrentScene;
    string PreviousScene;

    void OnEnable()
    {
        Events = Resources.LoadAll<GameEvent>("GameEvents");
        GameManager.OnSceneChanged += SceneChange;
    }

    void OnDisable()
    {
        GameManager.OnSceneChanged -= SceneChange;

    }

    void SceneChange()
    {
        if (GameManager.Instance.LevelInfo != null)
        {
            CurrentScene = GameManager.Instance.LevelInfo.Name;

        }
        else
        {
            CurrentScene = string.Empty;
        }
        if (GameManager.Instance.PreviousLevelInfo != null)
        {
            PreviousScene = GameManager.Instance.PreviousLevelInfo.Name;

        }
        else
        {
            PreviousScene = string.Empty;
        }
        GetEvents();
        StartCoroutine("Play");
    }

    private IEnumerator Play()
    {
        yield return new WaitForSeconds(0.1f);
        PlayEvent();
    }

    void PlayEvent()
    {
        if (EligibleEvents.Count <= 0)
        {
            return;
        }
        GameEvent currentEvent = EligibleEvents[0];
        DialogueManager.StopConversation();
        //        print("Playing event: " + currentEvent.Name);

        DialogueManager.StartConversation(currentEvent.ConversationTitle);
        if (!PlayedEvents.Contains(currentEvent))
        {
            PlayedEvents.Add(currentEvent);
        }
        EligibleEvents.Remove(currentEvent);
        PlayEvent();
    }

    void EventDone()
    {

    }

    bool CheckIfPlayed(GameEvent pEvent)
    {
        if (PlayedEvents.Contains(pEvent))
        {
            return true;
        }
        return false;
    }

    void GetEvents()
    {

        foreach (GameEvent gameEvent in Events)
        {
            //print(gameEvent.Name);
            if (gameEvent.Location != string.Empty && gameEvent.Location != CurrentScene)
            {
                //    print("wrong location");
                continue;
            }
            if (gameEvent.FromLocation != string.Empty && gameEvent.FromLocation != PreviousScene)
            {
                //     print("wrong previous location");
                continue;
            }
            if (CheckIfPlayed(gameEvent) == true && gameEvent.Replayable == false)
            {
                //                print("already played");

                continue;
            }
            //           print("Checking " + gameEvent.Name);
            if (DialogueManager.ConversationHasValidEntry(gameEvent.ConversationTitle))
            {
                //                print("Checked " + gameEvent.Name);
                EligibleEvents.Add(gameEvent);
            }

        }
    }
}
