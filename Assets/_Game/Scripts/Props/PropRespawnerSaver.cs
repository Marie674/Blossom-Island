using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
public class PropRespawnerSaver : MonoBehaviour
{
    PropRespawner TargetSpawner;
    string VariableName;
    string LevelName;
    void Start()    // Start is called before the first frame updatepublic override void Start()
    {
        LevelName = GameManager.Instance.LevelName;
        TargetSpawner = GetComponent<PropRespawner>();
    }

    void OnEnable()
    {
        VariableName = name + LevelName + transform.position.x.ToString("F2") + transform.position.ToString("F2");
        PersistentDataManager.RegisterPersistentData(this.gameObject);
    }

    void OnDisable()
    {
        PersistentDataManager.RegisterPersistentData(this.gameObject);
    }

    void OnRecordPersistentData()
    {
        TargetSpawner = GetComponent<PropRespawner>();

        VariableName = name + LevelName + transform.position.x.ToString("F2") + transform.position.ToString("F2");
        DialogueLua.SetVariable(VariableName + "DaysPassed", TargetSpawner.DaysPassed);
    }
    void OnApplyPersistentData()
    {
        VariableName = name + LevelName + transform.position.x.ToString("F2") + transform.position.ToString("F2");

        TargetSpawner.DaysPassed = DialogueLua.GetVariable(VariableName + "DaysPassed").asInt;
        TargetSpawner.GetTarget();
    }
}
