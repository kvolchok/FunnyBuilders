using System;
using System.Linq;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public event Action<Vector3Int, Vector3Int> UnitDropped;
    
    [SerializeField]
    private Grid _grid;
    [field:SerializeField]
    public Vector2Int MapSize { get; private set; }

    [SerializeField]
    private Tile _tilePrefab;
    
    private Camera _camera;
    private Tile[,] _tiles;
    
    private Tile _currentTile;
    private Tile _targetTile;
    private Vector3Int _currentTileIndex;
    private Vector3Int _targetTileIndex;

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
            _currentTileIndex = GetTile();

            if (_currentTileIndex == Vector3Int.up)
            {
                return;
            }
            
            _currentTile = _tiles[_currentTileIndex.x, _currentTileIndex.z];
        }
        
        if (!Input.GetMouseButtonUp(0) || _currentTile == null)
        {
            return;
        }
        
        _targetTileIndex = GetTile();
        _targetTile = _tiles[_targetTileIndex.x, _targetTileIndex.z];

        if (_targetTile == null || _currentTile == _targetTile)
        {
            return;
        }
        
        UnitDropped?.Invoke(_currentTileIndex, _targetTileIndex);
    }

    public Tile GetFirstAvailable()
    {
        return _tiles.Cast<Tile>().FirstOrDefault(tile => tile.IsAvailable);
    }

    public Vector3Int GetIndex(Tile tile)
    {
        return _grid.WorldToCell(tile.transform.position);
    }
    
    public void ChangeCurrentTileState(bool isAvailable)
    {
        _currentTile.ChangeState(isAvailable);
    }

    private void SpawnTiles()
    {
        _tiles = new Tile[MapSize.x, MapSize.y];
        
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
    private Vector3Int GetTile()
    {
        var mousePosition = Input.mousePosition;
        var ray = _camera.ScreenPointToRay(mousePosition);

        if (!Physics.Raycast(ray, out var hitInfo))
        {
            return Vector3Int.up;
        }

        var worldPosition = hitInfo.point;
        var cell = _grid.WorldToCell(worldPosition);

        return IsOutOfRange(cell) ? Vector3Int.up : cell;
    }

    private bool IsOutOfRange(Vector3Int cell)
    {
        return cell.x < 0 || cell.x >= _tiles.GetLength(0) ||
               cell.z < 0 || cell.z >= _tiles.GetLength(1);
    }
}