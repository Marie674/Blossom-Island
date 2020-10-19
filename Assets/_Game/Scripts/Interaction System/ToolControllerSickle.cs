using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class ToolControllerSickle : ToolControllerBase
{

    protected override IEnumerator UseCountdown()
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        GameManager.Instance.Player.DoAction(CurrentTool.PlayerTrigger, CurrentTool.UseInterval, pos, 0, CurrentTool.ToolTrigger, true);
        List<GameObject> objects = ToolCursorManager.Instance.GetObjects(AllowedTags);
        objects = objects.Where(i => i != null).ToList();

        yield return new WaitForSeconds(CurrentTool.UseInterval);
        ProceedUse(objects);
    }
    protected override void ProceedUse(List<GameObject> pObjects)
    {

        NeedBase energyNeed = PlayerNeedManager.Instance.GetNeed("Energy");
        energyNeed.Change(-CurrentTool.EnergyCost * (ToolCursorManager.Instance.CursorIndex + 1));

        List<GameObject> objects = pObjects;
        objects = objects.Where(i => i != null).ToList();

        foreach (GameObject obj in objects)
        {
            obj.SendMessage("Hit", "Sickle", SendMessageOptions.DontRequireReceiver);
        }

    }
}
