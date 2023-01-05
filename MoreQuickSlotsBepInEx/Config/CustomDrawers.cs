using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MoreQuickSlotsBepInEx.Config
{
    internal class CustomDrawers
    {
        readonly private Dictionary<CustomKeyboardShortcut.AllowedModifiers, bool> _toggleStates = 
            Enum.GetValues(typeof(CustomKeyboardShortcut.AllowedModifiers))
            .Cast<CustomKeyboardShortcut.AllowedModifiers>()
            .ToDictionary(key => key, _ => false);

        internal void HotkeyModifiersDrawer(ConfigEntryBase entry)
        {
            string setting = (string)entry.BoxedValue;
            HashSet<CustomKeyboardShortcut.AllowedModifiers> modifiers = CustomKeyboardShortcut.DeserializeModifiers(setting).ToHashSet();

            bool isDirty = false;
            foreach (CustomKeyboardShortcut.AllowedModifiers modifier in Enum.GetValues(typeof(CustomKeyboardShortcut.AllowedModifiers)))
            {
                if (GUILayout.Toggle(modifiers.Contains(modifier), modifier.ToString()) != _toggleStates[modifier])
                {
                    _toggleStates[modifier] = !_toggleStates[modifier];
                    if (_toggleStates[modifier])
                    {
                        if (modifiers.Add(modifier))
                            isDirty = true;
                    }
                    else
                    {
                        if (modifiers.Remove(modifier))
                            isDirty = true;
                    }
                }
            }
            if (isDirty)
                entry.BoxedValue = CustomKeyboardShortcut.SerializeModifiers(modifiers);
        }
    }
}
