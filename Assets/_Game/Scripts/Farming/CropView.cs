using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropView : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> StageObjects;
    public GameObject WateredSprite;
    public void UpdateWaterGraphic(bool pWatered)
    {
        WateredSprite.SetActive(pWatered);
    }

    public void UpdateCropGraphic(int pStage)
    {
        foreach (GameObject obj in StageObjects)
        {
            obj.SetActive(false);
        }
        StageObjects[pStage].SetActive(true);
    }
}
