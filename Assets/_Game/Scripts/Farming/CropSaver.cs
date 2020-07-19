using UnityEngine;
using PixelCrushers.DialogueSystem;
using System.Collections;

public class CropSaver : PixelCrushers.SpawnedObject
{
    CropModel TargetModel;
    CropController TargetController;
    CropTemplate TargetTemplate;

    private string VariableName = "";

    string LevelName;
    public override void Start()
    {
        base.Start();
        TargetModel = GetComponent<CropModel>();
        TargetController = GetComponent<CropController>();
        LevelName = GameManager.Instance.LevelName;
    }

    public override void OnEnable()
    {
        base.OnEnable();
        // PersistentDataManager.RegisterPersistentData(this.gameObject);
    }

    public override void OnDisable()
    {
        base.OnDisable();
        //  PersistentDataManager.RegisterPersistentData(this.gameObject);
    }
    public void OnRecordPersistentData()
    {
        //  print("RECORD");
        TargetModel = GetComponent<CropModel>();
        TargetController = GetComponent<CropController>();

        VariableName = name + LevelName + transform.position.x.ToString("F2") + transform.position.ToString("F2");
        DialogueLua.SetVariable(VariableName + "CurrentStage", TargetModel.CurrentStage);
        DialogueLua.SetVariable(VariableName + "CurrentStageGrowth", TargetModel.CurrentStageGrowth);
        DialogueLua.SetVariable(VariableName + "WateredToday", TargetModel.WateredToday);
        DialogueLua.SetVariable(VariableName + "CurrentlyWatered", TargetModel.CurrentlyWatered);
        DialogueLua.SetVariable(VariableName + "StageWaterLevel", TargetModel.StageWaterLevel);
        DialogueLua.SetVariable(VariableName + "StageSunLevel", TargetModel.StageSunLevel);
        DialogueLua.SetVariable(VariableName + "CurrentWilt", TargetModel.CurrentWilt);
        DialogueLua.SetVariable(VariableName + "Quality", TargetModel.Quality);
        DialogueLua.SetVariable(VariableName + "TimeSinceWatered", TargetModel.TimeSinceWatered);
    }
    public void OnApplyPersistentData()
    {
        //print("APPLYYY");
        StartCoroutine("ApplyTimer");

    }

    private IEnumerator ApplyTimer()
    {
        //  print("apply");
        yield return new WaitForSeconds(0);
        Apply();

    }

    void Apply()
    {
        TargetModel = GetComponent<CropModel>();
        TargetController = GetComponent<CropController>();


        VariableName = name + LevelName + transform.position.x.ToString("F2") + transform.position.ToString("F2");

        TargetController.Plot = GetPlot(transform.position);

        TargetModel.CurrentStage = DialogueLua.GetVariable(VariableName + "CurrentStage").asInt;
        TargetModel.CurrentStageGrowth = DialogueLua.GetVariable(VariableName + "CurrentStageGrowth").asInt;
        TargetModel.WateredToday = DialogueLua.GetVariable(VariableName + "WateredToday").asBool;
        TargetModel.CurrentlyWatered = DialogueLua.GetVariable(VariableName + "CurrentlyWatered").asBool;
        TargetModel.StageWaterLevel = DialogueLua.GetVariable(VariableName + "StageWaterLevel").asInt;
        TargetModel.StageSunLevel = DialogueLua.GetVariable(VariableName + "StageSunLevel").asInt;
        TargetModel.CurrentWilt = DialogueLua.GetVariable(VariableName + "CurrentWilt").asInt;
        TargetModel.Quality = DialogueLua.GetVariable(VariableName + "Quality").asFloat;
        TargetModel.TimeSinceWatered = DialogueLua.GetVariable(VariableName + "TimeSinceWatered").asInt;


        print("Plot: " + TargetController.Plot);

        TargetController.Plot.Crop = TargetController;
        TargetController.Load();
    }

    FarmPlot GetPlot(Vector2 pTileWorldPos)
    {

        // Vector2 pos = pTileWorldPos;
        // pos.x = 0.32f * Mathf.Round(pos.x / 0.32f);
        // pos.y = 0.32f * Mathf.Round(pos.y / 0.32f);

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
        print("Plot " + farmPlot.name);
        return farmPlot;
    }


}
