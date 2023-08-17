using System;
using System.Linq;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public event Action<Tile, Tile> UnitDropped;
    
    [SerializeField]
    private Grid _grid;
    [SerializeField]
    private Vector2Int _mapSize;
    [SerializeField]
    private Tile _tilePrefab;

    private Tile[,] _tiles;
    
    private Tile _currentTile;
    private Tile _targetTile;

    private void Awake()
    {
        SpawnTiles();
    }

    public Tile GetFirstAvailable()
    {
        return _tiles.Cast<Tile>().FirstOrDefault(tile => tile.IsAvailable);
    }

    private void SpawnTiles()
    {
        _tiles = new Tile[_mapSize.x, _mapSize.y];
        
        for (var x = 0; x < _tiles.GetLength(0); x++)
        {
            for (var y = 0; y < _tiles.GetLength(1); y++)
            {
                var tileIndex = new Vector3Int(x, 0, y);
                var tilePosition = _grid.CellToWorld(tileIndex);
                var tile = Instantiate(_tilePrefab, transform);
                tile.transform.position = tilePosition;
                _tiles[x, y] = tile;
            }   
        }
    }
}