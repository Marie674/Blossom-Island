using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSeasonChange : MonoBehaviour
{

    public List<Sprite> SeasonSprites;
    protected SpriteRenderer Sprite;

    // Use this for initialization
    void Start()
    {
        Sprite = GetComponent<SpriteRenderer>();
        UpdateSprite(TimeManager.Instance.CurrentMonth);
    }

    protected virtual void OnEnable()
    {
        TimeManager.OnMonthChanged += UpdateSprite;
    }

    protected virtual void OnDisable()
    {
        TimeManager.OnMonthChanged -= UpdateSprite;
    }

    protected virtual void UpdateSprite(Month pCurrentMonth)
    {
        if (Sprite != null)
        {
            Sprite.sprite = SeasonSprites[TimeManager.Instance.CurrentMonthIndex];
        }
    }
}
