using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
namespace RunAndJump.LevelCreator
{
    public static class EditorUtils
    {

        // Creates a new scene
        public static void NewScene()
        {
            EditorApplication.SaveCurrentSceneIfUserWantsTo();
            EditorApplication.NewScene();
        }
        // Remove all the elements of the scene
        public static void CleanScene()
        {
            GameObject[] allObjects = Object.FindObjectsOfType<GameObject>
           ();
            foreach (GameObject go in allObjects)
            {
                GameObject.DestroyImmediate(go);
            }
        }
        // Creates a new scene capable to be used as a level
        public static void NewLevel()
        {
            NewScene();
            CleanScene();
            GameObject levelGO = new GameObject("Level");
            levelGO.transform.position = Vector3.zero;
            levelGO.AddComponent<Level>();
        }
    }
}