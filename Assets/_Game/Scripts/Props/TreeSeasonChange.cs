using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSeasonChange : ObjectSeasonChange
{

    public bool NativeTree = false;
    protected override void UpdateSprite(Month pCurrentMonth)
    {
        TimeManager.MonthNames currentSeason = TimeManager.Instance.CurrentMonth.Name;
        TreeBase target = GetComponent<TreeBase>();
        NativeTree = target.NativeTree;

        if (Sprite != null)
        {

            if (NativeTree == true)
            {
                if (target.ProduceSeasons.Contains(currentSeason))
                {
                    if (target.FloweringSprite != null)
                    {
                        GetComponent<SpriteRenderer>().sprite = target.FloweringSprite;

                    }
                    else
                    {
                        Sprite.sprite = SeasonSprites[TimeManager.Instance.CurrentMonthIndex];

                    }
                }
            }
            else
            {
                Sprite.sprite = SeasonSprites[TimeManager.Instance.CurrentMonthIndex];

            }
        }
    }

}
