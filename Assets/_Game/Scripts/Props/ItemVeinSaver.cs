using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
public class ItemVeinSaver : MonoBehaviour
{

    ItemVein TargetVein;
    string VariableName;
    string LevelName;
    void Start()    // Start is called before the first frame updatepublic override void Start()
    {
        LevelName = GameManager.Instance.LevelName;
        TargetVein = GetComponent<ItemVein>();
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
        TargetVein = GetComponent<ItemVein>();

        VariableName = name + LevelName + transform.position.x.ToString("F2") + transform.position.ToString("F2");
        DialogueLua.SetVariable(VariableName + "TriesToday", TargetVein.TriesToday);
    }
    void OnApplyPersistentData()
    {
        VariableName = name + LevelName + transform.position.x.ToString("F2") + transform.position.ToString("F2");

        TargetVein.TriesToday = DialogueLua.GetVariable(VariableName + "TriesToday").asInt;
    }
}
