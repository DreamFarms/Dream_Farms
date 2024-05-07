using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;



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
        //if(EditorUtility.DisplayDialog("Genertate Map", "Do you wnat to generate map?", "Generate", "Close"))
        //{
        //    new GameObject("Generated Map");
        //}

        GameObject[] gameObjects = Resources.LoadAll<GameObject>("Prefabs/Map");
        foreach (GameObject gameObject in gameObjects)
        {
            Tilemap tilemapBase = Util.FindChild<Tilemap>(gameObject, "Tilemap_Base", true);
            Tilemap tilemap = Util.FindChild<Tilemap>(gameObject, "Collision", true);
            if(tilemap == null)
            {
                return;
            }


            // 추출된 blocked 정보를 파일로 만들기
            using (var writer = File.CreateText($"Assets/Resources/Map/{gameObject.name}.txt"))
            {
                writer.WriteLine(tilemapBase.cellBounds.xMin);
                writer.WriteLine(tilemapBase.cellBounds.xMax);
                writer.WriteLine(tilemapBase.cellBounds.yMin);
                writer.WriteLine(tilemapBase.cellBounds.yMax);

                for (int y = tilemapBase.cellBounds.yMax; y >= tilemapBase.cellBounds.yMin; y--)
                {
                    for (int x = tilemapBase.cellBounds.xMin; x <= tilemapBase.cellBounds.xMax; x++)
                    {
                        TileBase tileBase = tilemap.GetTile(new Vector3Int(x, y, 0));
                        if (tileBase != null)
                        {
                            writer.Write("1");
                        }
                        else
                        {
                            writer.Write("0");
                        }
                    }
                    writer.WriteLine();
                }


            }
        }



    }
#endif
}
