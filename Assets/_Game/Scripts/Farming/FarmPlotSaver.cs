using UnityEngine;
using PixelCrushers.DialogueSystem;

// This subclass of SpawnedObjectWithExtraInfo does a few things:
// 1. Records extra info (whether it has a child or not).
// 1. If clicked, it instantiates a child GameObject.
public class FarmPlotSaver : PixelCrushers.SpawnedObject
{
    FarmPlot Target;

    private string VariableName = "";
    public override void Start()
    {
        base.Start();
        Target = GetComponent<FarmPlot>();
    }

    public override void OnEnable()
    {
        base.OnEnable();
        PersistentDataManager.RegisterPersistentData(this.gameObject);
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PersistentDataManager.RegisterPersistentData(this.gameObject);
    }
    public void OnRecordPersistentData()
    {
        Target = GetComponent<FarmPlot>();

        VariableName = "FarmPlot " + transform.position.x.ToString() + "," + transform.position.y.ToString();
        DialogueLua.SetVariable(VariableName + "CurrentlyWatered", Target.CurrentlyWatered);
        DialogueLua.SetVariable(VariableName + "TimeSinceWatered", Target.TimeSinceWatered);

    }
    public void OnApplyPersistentData()
    {
        Target = GetComponent<FarmPlot>();

        VariableName = "FarmPlot " + transform.position.x.ToString() + "," + transform.position.y.ToString();
        Target.CurrentlyWatered = DialogueLua.GetVariable(VariableName + "CurrentlyWatered").asBool;
        Target.TimeSinceWatered = DialogueLua.GetVariable(VariableName + "TimeSinceWatered").asInt;
        Target.Load();

    }

    FarmPlot GetPlot(Vector2 pTileWorldPos)
    {
        RaycastHit2D hit = Physics2D.Raycast(pTileWorldPos, Vector2.zero, 0.5f, LayerMask.GetMask("Farm"));
        if (hit.collider == null)
        {
            return null;
        }
        if (hit.collider.GetComponent<FarmPlot>() == null)
        {
            return null;
        }

        FarmPlot farmPlot = hit.collider.gameObject.GetComponent<FarmPlot>();
        return farmPlot;
    }


}
