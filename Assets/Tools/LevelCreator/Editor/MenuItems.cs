﻿using UnityEngine;
using UnityEditor;

namespace RunAndJump.LevelCreator
{
    public static class MenuItems
    {
        [MenuItem("Tools/Level Creator/New Level Scene")]
        private static void NewLevel()
        {
            EditorUtils.NewLevel();
        }

        [MenuItem("Tools/Level Creator/Show Palette _p")]
        private static void ShowPalette()
        {
            PaletteWindow.ShowPalette();
        }
    }
}