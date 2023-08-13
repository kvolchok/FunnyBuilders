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
    
    private Camera _camera;
    private Tile[,] _tiles;
    
    private Tile _currentTile;
    private Tile _targetTile;

    private void Awake()
    {
        _camera = Camera.main;

        SpawnTiles();
    }

    // Метод для теста, потом нужно дропнуть
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _currentTile = GetTile();
        }
        
        if (!Input.GetMouseButtonUp(0) || _currentTile == null)
        {
            return;
        }
        
        _targetTile = GetTile();

        if (_targetTile == null || _currentTile == _targetTile)
        {
            return;
        }
        
        UnitDropped?.Invoke(_currentTile, _targetTile);
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
    
    // Метод для теста, потом нужно дропнуть
    private Tile GetTile()
    {
        var mousePosition = Input.mousePosition;
        var ray = _camera.ScreenPointToRay(mousePosition);

        if (!Physics.Raycast(ray, out var hitInfo))
        {
            return null;
        }

        var worldPosition = hitInfo.point;
        var cell = _grid.WorldToCell(worldPosition);

        return IsOutOfRange(cell) ? null : _tiles[cell.x, cell.z];
    }

    // Метод для теста, потом нужно дропнуть
    private bool IsOutOfRange(Vector3Int cell)
    {
        return cell.x < 0 || cell.x >= _tiles.GetLength(0) ||
               cell.z < 0 || cell.z >= _tiles.GetLength(1);
    }
}