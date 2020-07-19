using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
public class NPC : MonoBehaviour
{
    public string Name;
    public Actor Actor;
    public float CurrentAffection = 0;
    public float CurrentAcquaintance = 0;

    public float AcquaintanceRaiseMultiplier = 1;
    public float AcquaintanceLowerMultiplier = 1;

    public float GreetingMaxAffectionGain = 5;
    public float GreetingMaxAffectionLoss = 0.1f;

    public float CompatibilityMultiplier;

    public float AffectionRaiseMultiplier = 1;
    public float AffectionLowerMultiplier = 1;

    public float MinAffection = 0;
    public float MaxAffection = 1000;
    public float MaxAcquaintance = 1000;

    public float minCompatibility = 0;
    public float maxCompatibility = 3;

    private NPCData Data;

    [SerializeField]
    public List<NPCTraitScriptableObject> Traits = new List<NPCTraitScriptableObject>();

    void Start()
    {
        Data = GetComponent<NPCData>();
    }
    void OnEnable()
    {
        TimeManager.OnDayChanged += NewDay;
    }

    void OnDisable()
    {
        TimeManager.OnDayChanged -= NewDay;
    }

    void NewDay(int pDay)
    {
        if (Data != null)
        {
            Data.GreetedToday = false;

        }
    }

    public void Interact()
    {
        SpeakTo();
    }
    public void SpeakTo()
    {
        if (Data.Met == false)
        {
           GetComponent<NPCConversationPicker>().Meet();
            Data.Met = true;
        }
        else
        {

            if (Data.GreetedToday == false)
            {
                Greet();
            }
            GetComponent<NPCConversationPicker>().SpeakTo();
        }

    }

    public void Greet()
    {
        Data.GreetedToday = true;
        GreetingChangeAffection(1);
        GreetingChangeAcquaintance(1);
        print(Data.CurrentAcquaintance);
        print(Data.CurrentAffection);
    }

    void GreetingChangeAffection(int pAmount)
    {
        float amount = pAmount;
        CheckCompatibility();
        amount *= CompatibilityMultiplier;
        if (amount >= 0)
        {
            amount *= AffectionRaiseMultiplier;
        }
        else
        {
            amount *= AffectionLowerMultiplier;
        }
        print("changing affection: " + amount);

        amount = Mathf.Clamp(amount, GreetingMaxAffectionLoss, GreetingMaxAffectionGain);
        ChangeAffection(amount);
    }

    public void ChangeAffection(float pAmount)
    {
        float amt = pAmount;
        Data.CurrentAffection = Mathf.Clamp(Data.CurrentAffection + amt, MinAffection, MaxAffection);
    }

    public void ChangeAcquaintance(float pAmount)
    {
        float amt = pAmount;
        Data.CurrentAcquaintance = Mathf.Clamp(Data.CurrentAcquaintance + amt, 0f, MaxAcquaintance);
    }

    void GreetingChangeAcquaintance(int pAmount)
    {
        float amount = pAmount;

        if (amount >= 0)
        {
            amount *= AcquaintanceRaiseMultiplier;
        }
        else
        {
            amount *= AcquaintanceLowerMultiplier;
        }
        print("changing acquaintance: " + amount);
        Data.CurrentAcquaintance += amount;
    }

    public float CheckCompatibility()
    {
        PlayerCharacter player = GameManager.Instance.Player;
        float compatibility = 1;
        foreach (NPCTraitScriptableObject trait in Traits)
        {
            float traitCompatibility = 0;
            foreach (PlayerTraitScriptableObject playerTrait in player.Traits)
            {
                foreach (NPCTraitCompatibility traitCompat in trait.Compatibilities)
                {
                    if (traitCompat.Trait.Name == playerTrait.Name)
                    {
                        traitCompatibility += traitCompat.Modifier;
                    }
                }
                print(trait.Name + " " + traitCompatibility);
            }
            compatibility += traitCompatibility;
        }
        compatibility = Mathf.Clamp(compatibility, minCompatibility, maxCompatibility);
        CompatibilityMultiplier = compatibility;

        print("compatibility: " + CompatibilityMultiplier);
        return compatibility;

    }

}
