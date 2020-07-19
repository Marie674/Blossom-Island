using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "GameEvent")]

public class GameEvent : ScriptableObject
{
    public string Name;
    public string ConversationTitle;

    public string Location;

    public string FromLocation;

    public bool FestivalAllowed = false;

    public bool CanQueue = false;
    public bool Replayable;


}
