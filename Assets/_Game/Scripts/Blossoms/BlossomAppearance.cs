using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.NPCs.Blossoms
{
    public class BlossomAppearance : MonoBehaviour
    {

        public Animator Anim;
        public void SetAppearance(BlossomData.BlossomGrowth pGrowth, string pColor)
        {
            Anim = GetComponent<Animator>();
            AnimatorOverrideController aoc = new AnimatorOverrideController(Anim.runtimeAnimatorController);
            aoc.name = "Blossom_Override";
            AnimationClip[] clips = Anim.runtimeAnimatorController.animationClips;

            //List<KeyValuePair<AnimationClip, AnimationClip>> overrides = new List<KeyValuePair<AnimationClip, AnimationClip>>();

            foreach (AnimationClip clip in clips)
            {
                string path = "BlossomColors/Blossom_" + pColor + "/" + pGrowth.ToString() + "/" + clip.name;
                AnimationClip newClip = Resources.Load(path) as AnimationClip;
                aoc[clip.name] = newClip;
                //                print(clip.name + newClip.name);
            }
            //   aoc.ApplyOverrides(overrides);
            Anim.runtimeAnimatorController = aoc;
        }
    }
}

