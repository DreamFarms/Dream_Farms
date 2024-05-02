using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Scripts : MonoBehaviour
{
    public Tilemap _tilemap;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        List<Vector3Int> blocked = new List<Vector3Int>();
        foreach(Vector3Int pos in _tilemap.cellBounds.allPositionsWithin)
        {
            TileBase tileBase = _tilemap.GetTile(pos);
            if(tileBase != null)
            {
                blocked.Add(pos);
            }
        }
    }
}
