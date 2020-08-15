using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using System.Linq;
public class EventManager : Singleton<EventManager>
{

    [SerializeField]
    GameEvent[] Events = new GameEvent[0];
    public List<GameEvent> EligibleEvents = new List<GameEvent>();
    public List<GameEvent> PlayedEvents = new List<GameEvent>();
    Vector2 InitialPos;

    public bool Playing = false;
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
        GetEvents();
        PlayEvent();
    }

    void PlayEvent()
    {
        if (EligibleEvents.Count < 1)
        {
            return;
        }
        Playing = true;
        GameEvent currentEvent = EligibleEvents[0];
        GameManager.Instance.StartCutscene(currentEvent.PlayerPos, currentEvent.PlayerFacing, currentEvent.ConversationTitle, currentEvent.NPCLocations, currentEvent.EventTriggers);
        if (!PlayedEvents.Contains(currentEvent) && currentEvent.Replayable == false)
        {
            PlayedEvents.Add(currentEvent);
        }
        EligibleEvents.Remove(currentEvent);
    }

    public void EventDone()
    {
        GameManager.Instance.EndCutscene();
        EligibleEvents = EligibleEvents.Where(x => x != null).ToList();
        PlayEvent();
    }
    void GetEvents()
    {
        EligibleEvents.Clear();
        foreach (GameEvent gameEvent in Events)
        {
            if (gameEvent.CheckValidity() == true)
            {
                EligibleEvents.Add(gameEvent);
            }
        }
    }
}
