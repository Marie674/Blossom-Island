using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Items;

public class HarvestObject : MonoBehaviour
{

    [Tooltip("Can be harvested using hands")]
    public bool HandHarvest = false;

    [Tooltip("Is destroyed upon harvesting (does not use health system")]
    public bool DestroyOnHarvest = true;

    public float MaxHealth = 2f;
    protected float CurrentHealth = 2f;

    [Tooltip("Tool level required to hit")]
    public uint RequiredToolLevel = 1;


    [SerializeField]
    public List<LootTable> Outputs;


    public ParticleSpawner.ParticleTypes Particle;

    protected virtual void OnEnable()
    {
        CurrentHealth = MaxHealth;
        if (GetComponent<PixelCrushers.DialogueSystem.PersistentDestructible>() != null)
        {
            GetComponent<PixelCrushers.DialogueSystem.PersistentDestructible>().variableName = name + transform.position.x.ToString("F2") + transform.position.y.ToString("F2");

        }
        if (GetComponent<PixelCrushers.SpawnedObject>() != null)
        {
            GetComponent<PixelCrushers.SpawnedObject>().key = name + transform.position.x.ToString("F2") + transform.position.y.ToString("F2");

        }
    }

    public virtual void Interact()
    {
        if (HandHarvest == true)
        {
            if (DestroyOnHarvest == true)
            {
                Destroyed();
            }
            else
            {
                Output();
            }

        }
    }

    public virtual bool Hit()
    {
        ItemTool tool = ToolManager.Instance.CurrentTool;

        if (RequiredToolLevel > tool.Level)
        {
            return false;
        }
        Vector3 position = transform.position;
        position.z -= 0.0001f;
        ParticleSpawner.Instance.SpawnOneShot(Particle, position);

        CurrentHealth = Mathf.Clamp(CurrentHealth - tool.Power, 0, MaxHealth);

        if (CurrentHealth == 0)
        {
            Destroyed();
            return true;
        }
        return false;
    }
    public delegate void DestroyedObject();

    public event DestroyedObject OnObjectDestroyed;
    protected void Destroyed()
    {
        Output();
        if (OnObjectDestroyed != null)
        {
            OnObjectDestroyed();
        }
        Destroy(this.gameObject);

    }

    protected void Output()
    {

        foreach (LootTable list in Outputs)
        {
            List<ItemBase> items = list.Output();
            foreach (ItemBase item in items)
            {
                ItemSpawner.Instance.SpawnItems(item, transform.position);

            }
        }


    }
}
