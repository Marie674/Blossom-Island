using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Blossoms
{
    public class BlossomAppearance : MonoBehaviour
    {

        public Animator Anim;
        public void SetAppearance(BlossomData.BlossomGrowth pGrowth, string pColor)
        {
            pColor = "Blue";
            pGrowth = BlossomData.BlossomGrowth.Adult;


            Anim = GetComponent<Animator>();
            AnimatorOverrideController aoc = new AnimatorOverrideController(Anim.runtimeAnimatorController);

            AnimationClip[] clips = Anim.runtimeAnimatorController.animationClips;
            List<KeyValuePair<AnimationClip, AnimationClip>> newClips = new List<KeyValuePair<AnimationClip, AnimationClip>>();

            foreach (AnimationClip clip in clips)
            {
                string path = "BlossomColors/Blossom_" + pColor + "/" + pGrowth.ToString() + "/" + clip.name;
                AnimationClip newClip = Resources.Load(path) as AnimationClip;
                newClips.Add(new KeyValuePair<AnimationClip, AnimationClip>(clip, newClip));
            }
            aoc.ApplyOverrides(newClips);
            Anim.runtimeAnimatorController = aoc;
        }
    }
}

