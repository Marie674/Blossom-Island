using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Linq;

[CanEditMultipleObjects]
public class SpriteLightingGUI : ShaderGUI
{
    Material targetMat;

    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        targetMat = materialEditor.target as Material;
        string[] oldKeyWords = targetMat.shaderKeywords;
        GUIStyle style = EditorStyles.helpBox;
        style.margin = new RectOffset(0, 0, 0, 0);

        materialEditor.ShaderProperty(properties[0], properties[0].displayName);
        materialEditor.ShaderProperty(properties[1], properties[1].displayName);
        materialEditor.ShaderProperty(properties[2], properties[2].displayName);

        EditorGUILayout.Separator();
        GUILayout.Label("___Effects___", EditorStyles.boldLabel);
        GenericEffect(materialEditor, properties, style, oldKeyWords.Contains("TOON_ON"), "1.Toon Light", "TOON_ON", 9, 12);
        NormalMapping(materialEditor, properties, style, oldKeyWords.Contains("NORMALMAP_ON"));
        GenericEffect(materialEditor, properties, style, oldKeyWords.Contains("SPECULAR_ON"), "3.Specular", "SPECULAR_ON", 3, 4);
        GenericEffect(materialEditor, properties, style, oldKeyWords.Contains("GLOW_ON"), "4.Glow", "GLOW_ON", 12, 15);
        GenericEffect(materialEditor, properties, style, oldKeyWords.Contains("OUTLINE_ON"), "5.Outline", "OUTLINE_ON", 16, 20);
        GenericEffect(materialEditor, properties, style, oldKeyWords.Contains("FADE_ON"), "6.Fade", "FADE_ON", 21, 27);
        GenericEffect(materialEditor, properties, style, oldKeyWords.Contains("HSV_ON"), "7.Hue Shift", "HSV_ON", 28, 29);
        GenericEffect(materialEditor, properties, style, oldKeyWords.Contains("HITEFFECT_ON"), "8.Hit Effect", "HITEFFECT_ON", 31, 33);
    }

    private void GenericEffect(MaterialEditor materialEditor, MaterialProperty[] properties, GUIStyle style, bool toggle, string inspector, string flag, int first, int last)
    {
        bool ini = toggle;
        toggle = EditorGUILayout.BeginToggleGroup(inspector, toggle);
        if (ini != toggle && !Application.isPlaying) EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        if (toggle)
        {
            targetMat.EnableKeyword(flag);
            if (first > 0)
            {
                EditorGUILayout.BeginVertical(style);
                {
                    for (int i = first; i <= last; i++) materialEditor.ShaderProperty(properties[i], properties[i].displayName);
                }
                EditorGUILayout.EndVertical();
            }
        }
        else targetMat.DisableKeyword(flag);
        EditorGUILayout.EndToggleGroup();
    }

    private void NormalMapping(MaterialEditor materialEditor, MaterialProperty[] properties, GUIStyle style, bool toggle)
    {
        bool ini = toggle;
        toggle = EditorGUILayout.BeginToggleGroup("2.Normal Mapping", toggle);
        if (ini != toggle && !Application.isPlaying) EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        if (toggle)
        {
            targetMat.EnableKeyword("NORMALMAP_ON");
            EditorGUILayout.BeginVertical(style);
            {
                materialEditor.ShaderProperty(properties[5], properties[5].displayName);
                materialEditor.ShaderProperty(properties[6], properties[6].displayName);

                materialEditor.ShaderProperty(properties[7], properties[7].displayName);
                MaterialProperty flipX = ShaderGUI.FindProperty("_NormalFlipX", properties);
                if (flipX.floatValue == 1) targetMat.EnableKeyword("NORMALMAPFLIPX_ON");
                else targetMat.DisableKeyword("NORMALMAPFLIPX_ON");

                materialEditor.ShaderProperty(properties[8], properties[8].displayName);
                MaterialProperty flipY = ShaderGUI.FindProperty("_NormalFlipY", properties);
                if (flipY.floatValue == 1) targetMat.EnableKeyword("NORMALMAPFLIPY_ON");
                else targetMat.DisableKeyword("NORMALMAPFLIPY_ON");
            }
            EditorGUILayout.EndVertical();
        }
        else targetMat.DisableKeyword("NORMALMAP_ON");
        EditorGUILayout.EndToggleGroup();
    }
}