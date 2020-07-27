﻿using JetBrains.Util;

namespace RiderPlugin.UnrealLink.Ini
{
    /// <summary>
    /// Class for storing ActionMappings and AxisMappings (i.e. key bindings)
    /// TODO: create interface to access data
    /// </summary>
    public class KeyBindingsCache : IIniCacheBuilder
    {
        public KeyBindingsCache()
        {
            actionMappings = new IniCachedProperty("ActionMappings");
            axisMappings = new IniCachedProperty("AxisMappings");
        }

        private IniCachedProperty actionMappings;
        private IniCachedProperty axisMappings;
        private string curPlatform = IniCachedProperty.DefaultPlatform;

        public void ProcessProperty(FileSystemPath file, string section, string key, IniPropertyOperators op, IniCachedItem value)
        {
            if (!file.NameWithoutExtension.EndsWith("Input") || section != "/Script/Engine.InputSettings")
            {
                return;
            }
            
            switch (key)
            {
                case "ActionMappings":
                    actionMappings.ModifyValue(value, op, curPlatform);
                    break;
                case "AxisMappings":
                    axisMappings.ModifyValue(value, op, curPlatform);
                    break;
            }
        }

        public void SetupPlatform(string platform)
        {
            curPlatform = platform;
        }

        public bool IsEmpty => actionMappings.IsEmpty && axisMappings.IsEmpty;
    }
}