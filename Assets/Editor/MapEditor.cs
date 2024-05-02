using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MapEditor
{
#if UNITY_EDITOR
    // ctrl : %
    // shift : #
    // alt : &
    [MenuItem("Tools/GenerateMap %#m")]
    private static void GenerateMap()
    {
        // test
        if(EditorUtility.DisplayDialog("Genertate Map", "Do you wnat to generate map?", "Generate", "Close"))
        {
            new GameObject("Generated Map");
        }
    }
#endif
}
