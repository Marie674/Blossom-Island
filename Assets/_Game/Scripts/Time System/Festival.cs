using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct FestivalProp
{
    public GameObject Prop;
    public Vector2 Position;
    public string Level;
}

[CreateAssetMenu(menuName = "Festival")]
public class Festival : ScriptableObject
{
    public string Name;
    public int StartTime;
    public int EndTime;

    [SerializeField]
    public List<FestivalProp> Props;
    public Game.NPCs.Blossoms.BlossomCompetition Competition = null;
    public Game.NPCs.Blossoms.CompetitionStarter Presenter = null;

}
