using Sirenix.OdinInspector;
using System;
using UnityEngine;

[Serializable]
public class GatedAreaEnum
{
    [SerializeField, Required]
    private DynamicEnumData _dynamicEnumData; // Reference to the ScriptableObject

    [SerializeField, ValueDropdown("GetAreaOptions")]
    private int _areaId;

    public int AreaId => _areaId;

    // Default values for Tutorial and None
    public const int Tutorial = -1;
    public const int None = 0;

    // Method to provide dropdown options
    private ValueDropdownList<int> GetAreaOptions()
    {
        var options = new ValueDropdownList<int>
        {
            { "Tutorial", Tutorial },
            { "None", None }
        };

        // Add dynamic values from the ScriptableObject
        if (_dynamicEnumData != null)
        {
            for (int i = 0; i < _dynamicEnumData.EnumValues.Length; i++)
            {
                options.Add(_dynamicEnumData.EnumValues[i], i + 1); // Start dynamic IDs from 1
            }
        }

        return options;
    }
}
