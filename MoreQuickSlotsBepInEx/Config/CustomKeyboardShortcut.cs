﻿using BepInEx;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using BepInEx.Logging;
using BepInEx.Configuration;

namespace MoreQuickSlotsBepInEx.Config
{
    public class CustomKeyboardShortcut
    {
        private class Modifier
        {
            public bool Enabled { get; set; } = false;
            readonly private List<KeyCode> keyCodes = new List<KeyCode>();
            public IEnumerable<KeyCode> Keys => keyCodes;

            public Modifier(bool enabled = false, params KeyCode[] hotkeys)
            {
                Enabled = enabled;
                foreach (var hotkey in hotkeys)
                    keyCodes.Add(hotkey);
            }
        }

        internal enum AllowedKeys
        {
            None = KeyCode.None,
            Keypad0 = KeyCode.Keypad0,
            Keypad1 = KeyCode.Keypad1,
            Keypad2 = KeyCode.Keypad2,
            Keypad3 = KeyCode.Keypad3,
            Keypad4 = KeyCode.Keypad4,
            Keypad5 = KeyCode.Keypad5,
            Keypad6 = KeyCode.Keypad6,
            Keypad7 = KeyCode.Keypad7,
            Keypad8 = KeyCode.Keypad8,
            Keypad9 = KeyCode.Keypad9,
            Alpha0 = KeyCode.Alpha0,
            Alpha1 = KeyCode.Alpha1,
            Alpha2 = KeyCode.Alpha2,
            Alpha3 = KeyCode.Alpha3,
            Alpha4 = KeyCode.Alpha4,
            Alpha5 = KeyCode.Alpha5,
            Alpha6 = KeyCode.Alpha6,
            Alpha7 = KeyCode.Alpha7,
            Alpha8 = KeyCode.Alpha8,
            Alpha9 = KeyCode.Alpha9,
            Minus = KeyCode.Minus,
            Equals = KeyCode.Equals,
            LBracket = KeyCode.LeftCurlyBracket,
            RBracket = KeyCode.RightCurlyBracket,
            Colon = KeyCode.Colon,
            Quote = KeyCode.Quote,
            Comma = KeyCode.Comma,
            Period = KeyCode.Period,
            A = KeyCode.A,
            B = KeyCode.B,
            C = KeyCode.C,
            D = KeyCode.D,
            E = KeyCode.E,
            F = KeyCode.F,
            G = KeyCode.G,
            H = KeyCode.H,
            I = KeyCode.I,
            J = KeyCode.J,
            K = KeyCode.K,
            L = KeyCode.L,
            M = KeyCode.M,
            N = KeyCode.N,
            O = KeyCode.O,
            P = KeyCode.P,
            Q = KeyCode.Q,
            R = KeyCode.R,
            S = KeyCode.S,
            T = KeyCode.T,
            U = KeyCode.U,
            V = KeyCode.V,
            W = KeyCode.W,
            X = KeyCode.X,
            Y = KeyCode.Y,
            Z = KeyCode.Z,
        }

        internal enum AllowedModifiers
        {
            Shift,
            Ctrl,
            Alt
        }

        internal static string SerializeModifiers(IEnumerable<AllowedModifiers> modifiers)
        {
            return string.Join(" + ", modifiers.Select((x) => x.ToString()));
        }

        internal static IEnumerable<AllowedModifiers> DeserializeModifiers(string str)
        {
            try
            {
                string[] splitStr = str.Split(new char[5] { ' ', '+', ',', ';', '|' }, StringSplitOptions.RemoveEmptyEntries);
                return splitStr.Select(x => (AllowedModifiers)Enum.Parse(typeof(AllowedModifiers), x));
            }
            catch (SystemException ex)
            {
                BepInEx.Logging.Logger.CreateLogSource("Modified Keyboard Shortcut").Log(LogLevel.Error, "Failed to read keybind from settings: " + ex.Message);
                return new AllowedModifiers[] { };
            }

        }

        readonly internal ConfigEntry<AllowedKeys> _MainKey;
        internal KeyCode MainKey => (KeyCode)_MainKey.Value;

        readonly private ConfigEntry<string> _Modifiers;
        readonly private Dictionary<AllowedModifiers, Modifier> Modifiers =
            new Dictionary<AllowedModifiers, Modifier> {
                { AllowedModifiers.Shift, new Modifier(false, KeyCode.LeftShift, KeyCode.RightShift) },
                { AllowedModifiers.Ctrl, new Modifier(false, KeyCode.LeftControl, KeyCode.RightControl) },
                { AllowedModifiers.Alt, new Modifier(false, KeyCode.LeftAlt, KeyCode.RightAlt) }
            };

        internal CustomKeyboardShortcut(ConfigEntry<AllowedKeys> mainKey, ConfigEntry<string> modifiers)
        {
            _MainKey = mainKey;
            _Modifiers = modifiers;
            DeserializeModifiers();
        }

        private void DeserializeModifiers()
        {
            IEnumerable<AllowedModifiers> mods = DeserializeModifiers(_Modifiers.Value);
            foreach (KeyValuePair<AllowedModifiers, Modifier> mod in Modifiers)
                mod.Value.Enabled = mods.Contains(mod.Key);
        }

        internal bool IsDown()
        {
            if (MainKey == KeyCode.None)
            {
                return false;
            }

            if (UnityInput.Current.GetKeyDown(MainKey))
            {
                return ModifierKeyTest();
            }

            return false;
        }

        //
        // Summary:
        //     Check if the main key is currently held down (Input.GetKey), and specified modifier
        //     keys are all pressed
        internal bool IsPressed()
        {
            if (MainKey == KeyCode.None)
            {
                return false;
            }

            if (UnityInput.Current.GetKey(MainKey))
            {
                return ModifierKeyTest();
            }

            return false;
        }

        //
        // Summary:
        //     Check if the main key was just lifted (Input.GetKeyUp), and specified modifier
        //     keys are all pressed.
        internal bool IsUp()
        {
            if (MainKey == KeyCode.None)
            {
                return false;
            }

            if (UnityInput.Current.GetKeyUp(MainKey))
            {
                return ModifierKeyTest();
            }

            return false;
        }

        private bool ModifierKeyTest()
        {
            foreach (Modifier mod in Modifiers.Values)
            {
                if (mod.Enabled && mod.Keys.All((c) => !UnityInput.Current.GetKey(c)))
                {
                    return false;
                }
                else if (!mod.Enabled && mod.Keys.Any((c) => UnityInput.Current.GetKey(c)))
                {
                    return false;
                }
            }
            return true;
        }
    }
}