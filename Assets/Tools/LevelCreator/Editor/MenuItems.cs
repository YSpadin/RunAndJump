using UnityEngine;
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
    }
}
