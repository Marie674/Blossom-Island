using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using PixelCrushers.DialogueSystem;
using System.Linq;
using Game.NPCs.Blossoms;
using Cinemachine;
using PixelCrushers.DialogueSystem;
using TMPro;
using DG.Tweening;
public class RaceTimelineControl : MonoBehaviour
{
    public List<GameObject> Blossoms;
    public List<Transform> Tracks = new List<Transform>();

    int BlossomsDone = 0;

    public float FinishLineX = 10f;
    public List<AnimationCurve> Curves = new List<AnimationCurve>();

    GameObject WinningBlossom;

    List<bool> DoneBlossoms = new List<bool>();

    void Start()
    {
        Curves = ListExtension.Shuffle<AnimationCurve>(Curves);
        for (int i = 0; i < Blossoms.Count; i++)
        {
            DOTweenPath path = Blossoms[i].GetComponent<DOTweenPath>();
            Tween t = path.GetTween();
            t.SetEase(Curves[i]);
            path.DOPause();
        }

        for (int i = 0; i < Blossoms.Count; i++)
        {
            DoneBlossoms.Add(false);
        }
        StartRace();
    }
    private void StartRace()
    {
        int i = 0;
        foreach (KeyValuePair<string, float> competitor in BlossomCompetitionManager.Instance.CurrentResults)
        {
            float speed = MapRangeExtension.MapRange(competitor.Value, 0, BlossomCompetitionManager.Instance.GetCompetitionMaxScore(), 0.75f, 1.25f);
            DOTweenPath path = Blossoms[i].GetComponent<DOTweenPath>();
            path.GetTween().timeScale = speed * 0.25f;
            BlossomAppearance appearance = Blossoms[i].GetComponent<BlossomAppearance>();
            string id = competitor.Key;
            string growth = DialogueLua.GetVariable(id + "Growth").AsString;
            string color = DialogueLua.GetVariable(id + "Color").AsString;
            BlossomData.BlossomGrowth growthData = (BlossomData.BlossomGrowth)System.Enum.Parse(typeof(BlossomData.BlossomGrowth), growth);
            appearance.SetAppearance(growthData, color);
            string name = DialogueLua.GetVariable(id + "Name").AsString;
            Blossoms[i].GetComponentInChildren<TextMeshProUGUI>().text = name;
            i++;
            path.DOPlay();
        }


    }
    void Update()
    {

        for (int i = 0; i < Blossoms.Count; i++)
        {
            if (Blossoms[i].transform.position.x >= FinishLineX)
            {
                if (DoneBlossoms[i] == false)
                {
                    BlossomDone(Blossoms[i]);
                    DoneBlossoms[i] = true;
                }
                continue;
            }
        }
    }

    public void BlossomDone(GameObject pBlossom)
    {
        print(pBlossom.name);
        BlossomsDone += 1;
        if (BlossomsDone == 1)
        {
            WinningBlossom = pBlossom;

            print("Winner: " + pBlossom.name);
            WinningBlossom.GetComponent<Animator>().SetTrigger("Tippytap");

        }
        if (BlossomsDone == Blossoms.Count)
        {
            print("Race Over");
            StopCoroutine("PerformRace");

            GameManager.Instance.EndCompetitionCutscene(60);
        }
    }
}
