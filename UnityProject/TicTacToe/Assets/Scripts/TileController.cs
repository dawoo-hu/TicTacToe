using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    public delegate void TileClickController(TileClickedData tile);
    public static event TileClickController OnTileClicked;
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                // 检测是否点击到立方体
                if (hitInfo.collider.gameObject == gameObject)
                {
                    Debug.Log("Tile Clicked!");
                    if (OnTileClicked != null)
                    {
                        TileClickedData tile = new TileClickedData(gameObject);
                        OnTileClicked(tile);
                    }
                }
            }
        }
    }
}

public class TileClickedData
{
    public GameObject tile { get; private set; }

    public TileClickedData(GameObject tile)
    {
        this.tile = tile;
    }
}