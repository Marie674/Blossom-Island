using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ToolControllerPickaxe : ToolControllerBase
{

    protected override IEnumerator UseCountdown()
    {
        Vector2 pos = ToolCursorManager.Instance.CurrentCursor.transform.position;
        GameManager.Instance.Player.DoAction(CurrentTool.trigger, CurrentTool.useInterval, pos, 0, CurrentTool.toolTrigger, true);
        List<GameObject> objects = ToolCursorManager.Instance.GetObjects();
        objects = objects.Where(i => i != null).ToList();

        yield return new WaitForSeconds(CurrentTool.useInterval);
        ProceedUse(objects);
    }
    protected override void ProceedUse(List<GameObject> pObjects)
    {

        NeedBase energyNeed = PlayerNeedManager.Instance.GetNeed("Energy");
        energyNeed.Change(-CurrentTool.energyCost * (ToolCursorManager.Instance.CursorIndex + 1));

        List<GameObject> objects = pObjects;
        objects = objects.Where(i => i != null).ToList();

        foreach (GameObject obj in objects)
        {
            obj.SendMessage("Hit", "Pickaxe", SendMessageOptions.DontRequireReceiver);

        }

    }
}
