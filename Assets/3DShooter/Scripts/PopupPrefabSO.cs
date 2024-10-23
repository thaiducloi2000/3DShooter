using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PopupPrefab", menuName = "GameData/Popup", order = 1)]
public class PopupPrefabSO : ScriptableObject
{
    [SerializeField] private GamePopup[] popupPrefabs;
    private Dictionary<string, GamePopup> popups = new();

    private void OnValidate()
    {
        foreach(GamePopup popup in popupPrefabs)
        {
            if (!popups.ContainsKey(popup.name))
            {
                popups.Add(popup.name, popup);
            }
        }
        Debug.Log(popups.Count);
    }

    public GameObject GetPrefab(string prefabName)
    {
        if (popups.ContainsKey(prefabName))
        {
            return popups[prefabName].gameObject;
        }
        return null;
    }
}
