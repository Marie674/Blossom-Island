using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusEffectLayout : MonoBehaviour
{

    //	HorizontalLayoutGroup layout;
    public int Spacing = 16;

    void Start()
    {
        //	layout = GetComponent<HorizontalLayoutGroup> ();
    }

    public void Sort()
    {
        StatusEffectUI[] children = transform.GetComponentsInChildren<StatusEffectUI>();
        List<StatusEffectUI> effects = new List<StatusEffectUI>();
        foreach (StatusEffectUI child in children)
        {
            if (child.Widget.activeSelf)
            {
                effects.Add(child);
            }
        }
        for (int i = 0; i < effects.Count; i++)
        {

            Vector3 pos = effects[i].GetComponent<RectTransform>().anchoredPosition;
            //pos.x = (GetComponent<RectTransform>().rect.width + Spacing*2) + (i * (effects [i].Widget.GetComponent<RectTransform> ().rect.width + Spacing));
            pos.x += effects[i].GetComponent<RectTransform>().rect.width * i;
            //			print(i + " "+effects[i].GetComponent<RectTransform>().rect.width);
            //print ( effects[i].name+" "+pos.x);
            effects[i].GetComponent<RectTransform>().anchoredPosition = pos;

        }

    }
}
