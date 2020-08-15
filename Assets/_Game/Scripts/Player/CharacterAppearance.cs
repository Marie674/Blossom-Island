using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CharacterAppearance : MonoBehaviour
{
    public Animator BodyAnim;
    public Animator EyeAnim;
    public Animator HairAnim;

    public Animator TopAnim;
    public Animator BottomAnim;
    public Animator ShoesAnim;
    // Start is called before the first frame update
    public void ChangeAll(int pBodyIndex, int pEyesIndex, int pHairIndex, int pTopIndex, int pBottomIndex, int pShoesIndex)
    {
        ChangeBody(pBodyIndex);
        ChangeEyes(pEyesIndex);
        ChangeHair(pHairIndex);
        ChangeTop(pTopIndex);
        ChangeBottom(pBottomIndex);
        ChangeShoes(pShoesIndex);
    }
    public void ChangeBody(int pIndex)
    {

        pIndex += 1;

        AnimatorOverrideController aoc = new AnimatorOverrideController(BodyAnim.runtimeAnimatorController);
        aoc.name = "Player Body Override";
        AnimationClip[] clips = BodyAnim.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            string path = "Character Art/Bodies/Body" + pIndex + "/" + clip.name;
            AnimationClip newClip = Resources.Load(path) as AnimationClip;
            aoc[clip.name] = newClip;
        }

        BodyAnim.runtimeAnimatorController = aoc;
        print("set");
    }

    public void ChangeEyes(int pIndex)
    {
        pIndex += 1;

        AnimatorOverrideController aoc = new AnimatorOverrideController(EyeAnim.runtimeAnimatorController);
        AnimationClip[] clips = EyeAnim.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            string path = "Character Art/Eyes/Eyes" + pIndex + "/" + clip.name;
            AnimationClip newClip = Resources.Load(path) as AnimationClip;
            aoc[clip.name] = newClip;
        }
        EyeAnim.runtimeAnimatorController = aoc;
    }

    public void ChangeHair(int pIndex)
    {
        pIndex += 1;

        AnimatorOverrideController aoc = new AnimatorOverrideController(HairAnim.runtimeAnimatorController);
        AnimationClip[] clips = HairAnim.runtimeAnimatorController.animationClips;

        foreach (AnimationClip clip in clips)
        {
            string path = "Character Art/Hairstyles/Hair" + pIndex + "/" + clip.name;
            AnimationClip newClip = Resources.Load(path) as AnimationClip;
            aoc[clip.name] = newClip;
        }
        HairAnim.runtimeAnimatorController = aoc;
    }

    public void ChangeTop(int pIndex)
    {
        pIndex += 1;

        AnimatorOverrideController aoc = new AnimatorOverrideController(TopAnim.runtimeAnimatorController);
        AnimationClip[] clips = TopAnim.runtimeAnimatorController.animationClips;

        foreach (AnimationClip clip in clips)
        {
            string path = "Character Art/Tops/Top" + pIndex + "/" + clip.name;
            AnimationClip newClip = Resources.Load(path) as AnimationClip;
            aoc[clip.name] = newClip;
        }
        TopAnim.runtimeAnimatorController = aoc;
    }

    public void ChangeBottom(int pIndex)
    {
        pIndex += 1;

        AnimatorOverrideController aoc = new AnimatorOverrideController(BottomAnim.runtimeAnimatorController);
        AnimationClip[] clips = BottomAnim.runtimeAnimatorController.animationClips;

        foreach (AnimationClip clip in clips)
        {
            string path = "Character Art/Bottoms/Bottom" + pIndex + "/" + clip.name;
            AnimationClip newClip = Resources.Load(path) as AnimationClip;
            aoc[clip.name] = newClip;
        }
        BottomAnim.runtimeAnimatorController = aoc;
    }


    public void ChangeShoes(int pIndex)
    {
        pIndex += 1;
        AnimatorOverrideController aoc = new AnimatorOverrideController(ShoesAnim.runtimeAnimatorController);
        AnimationClip[] clips = ShoesAnim.runtimeAnimatorController.animationClips;

        foreach (AnimationClip clip in clips)
        {
            string path = "Character Art/Shoes/Shoes" + pIndex + "/" + clip.name;
            AnimationClip newClip = Resources.Load(path) as AnimationClip;
            aoc[clip.name] = newClip;
        }
        ShoesAnim.runtimeAnimatorController = aoc;
    }
}
