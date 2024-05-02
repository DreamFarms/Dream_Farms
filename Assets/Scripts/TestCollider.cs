using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TestCollider : MonoBehaviour
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
            TileBase tile = _tilemap.GetTile(pos);
            if(tile != null)
            {
                blocked.Add(pos);
            }
        
        }
    }
}
