using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ES3Internal;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEditor.AddressableAssets;
using System.Linq;
using UnityEngine.U2D;
[CreateAssetMenu(fileName = "ItemDataProcessor", menuName = "Data/ItemDataProcessor")]
public class ItemDataProcessor : ScriptableObject
{
    ES3Spreadsheet Spreadsheet;

    [FilePath]
    public string SpreadSheetPath;
    [FolderPath]
    public string ObjectPath;
    [FilePath]
    public string AtlasPath;

#if (UNITY_EDITOR)


    [Button("Load Spreadsheet", ButtonSizes.Gigantic)]
    public void LoadData()
    {
        Spreadsheet = new ES3Spreadsheet();
        ES3Settings settings = new ES3Settings();
        settings.location = ES3.Location.Resources;
        Spreadsheet.Load("Data/ItemData.csv", settings);
        PopulateObjects();

    }
    void PopulateObjects()
    {

        SpriteAtlas atlas = EditorGUIUtility.Load(AtlasPath) as SpriteAtlas;

        for (int row = 1; row < Spreadsheet.RowCount - 1; row++)
        {

            int ID = Spreadsheet.GetCell<int>(0, row);
            string itemName = Spreadsheet.GetCell<string>(1, row);
            string description = Spreadsheet.GetCell<string>(2, row);
            float value = Spreadsheet.GetCell<float>(3, row);
            int markup = Spreadsheet.GetCell<int>(4, row);
            float weight = Spreadsheet.GetCell<float>(5, row);
            string iconName = Spreadsheet.GetCell<string>(6, row);
            iconName = iconName.Substring(0, iconName.Length - 1);
            Sprite itemSprite = atlas.GetSprite(iconName);
            bool sellable = Spreadsheet.GetCell<bool>(7, row);


        }
    }

#endif
}
