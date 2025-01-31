using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "DynamicEnumData", menuName = "ScriptableObjects/DynamicEnumData", order = 1)]
public class DynamicEnumData : ScriptableObject
{
    [SerializeField, ListDrawerSettings(ShowFoldout = true)]
    private string[] _enumValues;

    public string[] EnumValues => _enumValues;
}
