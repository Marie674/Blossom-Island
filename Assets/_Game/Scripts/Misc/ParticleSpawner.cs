using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Particle
{
    public ParticleSpawner.ParticleTypes Type;
    public GameObject ParticleSystem;
}

public class ParticleSpawner : Singleton<ParticleSpawner>
{

    public bool SpawnOneShotParticles = true;
    public bool SpawnLoopParticles = true;

    public List<Particle> Particles = new List<Particle>();

    public GameObject BubblePrefab;
    public enum ParticleTypes
    {
        Wood,
        Stone,
        Leaf,
        Water,
        Rain,
        Clouds,
        Snow
    }

    public enum BubbleTypes
    {
        Heart,
    }

    //	public Dictionary<string,int> Particles = new Dictionary<string, int>(){
    //		{"Wood",0},
    //		{"Stone",1},
    //		{"Leaf",2}
    //	};

    public void SpawnBubble(BubbleTypes pBubble, Vector2 pPosition, Transform pParent = null)
    {
        GameObject bubble = Instantiate(BubblePrefab, pPosition, transform.rotation, pParent);
        bubble.GetComponent<Animator>().SetTrigger(pBubble.ToString());
        Destroy(bubble.gameObject, 2.6f);
    }

    public void SpawnOneShot(ParticleTypes pParticle, Vector3 pPosition, Transform pParent = null)
    {
        if (SpawnOneShotParticles)
        {
            int particleIndex = -1;
            GameObject tempParticle = GetParticle(pParticle);

            if (tempParticle != null)
            {
                GameObject newParticle = Instantiate(tempParticle, pPosition, transform.rotation);
                ParticleSystem[] particles = newParticle.GetComponentsInChildren<ParticleSystem>();
                foreach (ParticleSystem particle in particles)
                {
                    if (particle.main.playOnAwake == true)
                    {
                        particle.Play(false);
                        Destroy(newParticle.gameObject, particle.main.duration);
                    }

                }

            }

        }
    }

    public void Spawn(ParticleTypes pParticle, Vector3 pPosition, Transform pParent = null)
    {
        if (SpawnLoopParticles)
        {
            int particleIndex = -1;
            GameObject tempParticle = GetParticle(pParticle);

            if (tempParticle != null)
            {
                Vector3 pos = tempParticle.transform.localPosition;
                GameObject newParticle = Instantiate(tempParticle, Vector3.zero, tempParticle.transform.rotation, pParent);
                newParticle.transform.localPosition = pos;
                ParticleSystem[] particles = newParticle.GetComponentsInChildren<ParticleSystem>();
                foreach (ParticleSystem particle in particles)
                {
                    if (particle.main.playOnAwake == true)
                    {
                        particle.Play(false);
                    }

                }

            }

        }
    }

    private GameObject GetParticle(ParticleTypes pType)
    {
        foreach (Particle particle in Particles)
        {
            if (particle.Type == pType)
            {
                return particle.ParticleSystem;
            }
        }
        return null;
    }

}
