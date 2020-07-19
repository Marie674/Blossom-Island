// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using ItemSystem;

// // [System.Serializable]
// // public struct StoreItem
// // {
// //     public ItemContainer ItemContainer;
// //     public int Probability;
// //     public int CurrentAmount;
// //     public int MaxAmount;
// //     public bool SoldOut;
// // }

// public class Store : MonoBehaviour
// {

//     public List<StoreShelf> Shelves = new List<StoreShelf>();

//     void Start()
//     {
//         PopulateShelves(TimeManager.Instance.PassedDays);
//     }

//     void OnEnable()
//     {
//         TimeManager.OnDayChanged += PopulateShelves;
//     }

//     void OnDisable()
//     {
//         TimeManager.OnDayChanged -= PopulateShelves;
//     }

//     void PopulateShelves(int pDayIndex)
//     {
//         foreach (StoreShelf shelf in Shelves)
//         {
//             shelf.Populate();
//         }
//     }

// }
