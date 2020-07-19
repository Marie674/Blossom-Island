using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemSystem;
public class TreeBase : HarvestObject
{

    public bool YieldsProduce;
    public bool CanShake = true;

    public GameObject TreeStump;

    [SerializeField]
    public LootTable ShakeOutputs;
    public int DroppedItems = 0;
    public int MaxDroppedItems = 5;

    private bool IsQuitting;

    [SerializeField]
    public LootTable ProduceOutputs;

    [SerializeField]
    public LootTable FellOutputs;

    public int ProduceGrowthTime = 5;
    public int CurrentProduceGrowth = 0;
    public bool ProduceReady = false;
    public List<TimeManager.MonthNames> ProduceSeasons;
    public GameObject ProduceSprite;

    public int CurrentGrowth;
    public int TargetGrowth;
    public bool Mature;

    public float ShakeTime = 0.75f;
    public float ShakeAmplitude = 0.02f;
    public float ShakeSpeed = 12;

    bool IsShaking = false;

    public GameObject NextPhaseProp;

    public Sprite FloweringSprite;

    public bool NativeTree = false;
    protected override void OnEnable()
    {
        base.OnEnable();
        TimeManager.OnDayChanged += DayChange;


    }

    protected virtual void OnDisable()
    {
        TimeManager.OnDayChanged -= DayChange;
    }

    public void HoldUse()
    {
        Interact();
    }

    protected virtual void DayChange(int pDayIndex)
    {

        DroppedItems = 0;

        if (YieldsProduce)
        {
            TimeManager.MonthNames currentSeason = TimeManager.Instance.PastSeasons[pDayIndex];
            CurrentProduceGrowth++;
            if (!ProduceSeasons.Contains(currentSeason))
            {
                CurrentProduceGrowth = 0;
                ProduceReady = false;
            }
            else
            {
                if (FloweringSprite != null)
                {
                    GetComponent<SpriteRenderer>().sprite = FloweringSprite;

                }
            }
            if (CurrentProduceGrowth >= ProduceGrowthTime)
            {
                ProduceReady = true;
            }
            else
            {
                ProduceReady = false;
            }
            SetProduceSprite();
        }

        if (Mature == false)
        {
            CurrentGrowth++;
            if (CurrentGrowth >= TargetGrowth && NextPhaseProp != null)
            {
                GameObject next = Instantiate(NextPhaseProp, transform.position, transform.rotation, transform.parent);

                Destroy(gameObject);
            }
        }
    }

    public void SetProduceSprite()
    {
        if (ProduceReady)
        {
            ProduceSprite.SetActive(true);
        }
        else
        {
            ProduceSprite.SetActive(false);
        }
    }

    public override bool Hit()
    {
        ItemTool tool = ToolManager.Instance.CurrentTool;
        if (RequiredToolLevel > tool.level)
        {
            return false;
        }
        ParticleSpawner.Instance.SpawnOneShot(ParticleSpawner.ParticleTypes.Wood, transform.position);

        if (RequiredToolLevel > tool.level)
        {
            return false;
        }

        CurrentHealth = Mathf.Clamp(CurrentHealth - tool.power, 0, MaxHealth);
        if (CurrentHealth == 0)
        {
            FellTree();
            return true;
        }
        return false;
    }

    public override void Interact()
    {
        if (GameManager.Instance.Player.DoAction("Grab", ShakeTime, transform.position, 0.2f))
        {
            Shake();

        }

    }
    private void Shake()
    {
        StartCoroutine("DoSway");
        StartCoroutine("DoOutput");
    }

    IEnumerator DoOutput()
    {
        yield return new WaitForSeconds(ShakeTime / 2);
        if (ProduceReady)
        {
            OutputProduce();
        }
        if (DroppedItems >= MaxDroppedItems)
        {
            yield return null;
        }
        else
        {

            if (ShakeOutputs != null)
            {
                List<ItemBase> items = ShakeOutputs.Output();
                foreach (ItemBase item in items)
                {
                    if (item.itemName != "NULL")
                    {
                        ItemSpawner.Instance.SpawnItems(item, new Vector3(this.transform.position.x, this.transform.position.y - 1f, this.transform.position.z));
                        DroppedItems += 1;
                    }
                }
            }
        }
        StopCoroutine("DoOutput");
    }

    IEnumerator DoSway()
    {
        SwayShader shader = GetComponent<SwayShader>();
        float initialAmp = shader.Amplitude;
        float initialSpeed = shader.Speed;

        shader.Amplitude = ShakeAmplitude;
        shader.Speed = ShakeSpeed;
        yield return new WaitForSeconds(ShakeTime);
        shader.Amplitude = initialAmp;
        shader.Speed = initialSpeed;
        StopCoroutine("DoSway");
    }


    private void FellTree()
    {
        if (TreeStump != null)
        {
            GameObject stump = Instantiate(TreeStump, transform.position, transform.rotation, transform.parent);

        }

        if (FellOutputs != null)
        {
            List<ItemBase> items = FellOutputs.Output();
            foreach (ItemBase item in items)
            {
                if (item != null)
                {
                    ItemSpawner.Instance.SpawnItems(item, new Vector3(this.transform.position.x, this.transform.position.y - 1f, this.transform.position.z));
                    DroppedItems += 1;
                }
            }
        }


        if (ProduceReady)
        {
            OutputProduce();
        }
        Destroy(gameObject);
    }

    void OnApplicationQuit()
    {
        IsQuitting = true;
    }

    void OnDestroy()
    {

    }

    protected void OutputProduce()
    {

        if (ProduceOutputs == null)
        {
            return;
        }
        CurrentProduceGrowth = 0;

        ProduceReady = false;
        SetProduceSprite();

        List<ItemBase> items = ProduceOutputs.Output();
        foreach (ItemBase item in items)
        {
            if (item != null)
            {
                ItemSpawner.Instance.SpawnItems(item, new Vector3(this.transform.position.x, this.transform.position.y - 1f, this.transform.position.z));
                DroppedItems += 1;
            }
        }

    }
}
