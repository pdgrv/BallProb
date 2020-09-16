using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private List<GridObject> _templates;
    [SerializeField] private Transform _player;
    [SerializeField] private float _cellSize;

    private HashSet<Vector3Int> _collisionsMatrix = new HashSet<Vector3Int>();
    private List<GridObject> _pool = new List<GridObject>();

    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        FillLine(_player.position, _camera.ViewportToWorldPoint(new Vector3(1f, 1f, -_camera.transform.position.z * 2)).x);
        DestroyObjectAbroadScreen(_pool);
    }

    private void FillLine(Vector3 center, float viewRadius)
    {
        var cellCountOnAxis = (int)(viewRadius / _cellSize);
        var fillAreaCenter = WorldToGridPosition(center);

        for (int x = 0; x <= cellCountOnAxis; x++)
        {
            TryCreateOnLayer(GridLayer.Ground, fillAreaCenter + new Vector3Int(x, 0, 0));
            TryCreateOnLayer(GridLayer.OnGround, fillAreaCenter + new Vector3Int(x, 0, 0));
        }
    }

    private void TryCreateOnLayer(GridLayer layer, Vector3Int gridPosition)
    {
        gridPosition.y = (int)layer;

        if (!_collisionsMatrix.Add(gridPosition))
            return;

        var template = GetRandomTemplate(layer);

        if (template == null)
            return;

        var position = GridToWorldPosition(gridPosition);

        GridObject createdObject = Instantiate(template, position, template.transform.rotation, transform);
        _pool.Add(createdObject);
    }

    private GridObject GetRandomTemplate(GridLayer layer)
    {
        var variants = _templates.Where(template => template.Layer == layer);

        foreach (var template in variants)
        {
            if (template.Chance > Random.Range(0, 100))
                return template;
        }

        return null;
    }

    private void DestroyObjectAbroadScreen(List<GridObject> objectPool)
    {
        Vector3 disablePoint = _camera.ViewportToWorldPoint(new Vector3(0f, 0f, -_camera.transform.position.z));

        foreach (GridObject item in objectPool)
        {
            if (item == null)
            {
                objectPool.Remove(item);
                return;
            }

            if (item.transform.position.x < disablePoint.x)
            {
                _collisionsMatrix.Remove(WorldToGridPosition(item.transform.position));
                Destroy(item.gameObject);
            }
        }
    }

    private Vector3 GridToWorldPosition(Vector3Int gridPosition)
    {
        return new Vector3(
            gridPosition.x * _cellSize,
            gridPosition.y * _cellSize,
            gridPosition.z * _cellSize);
    }

    private Vector3Int WorldToGridPosition(Vector3 worldPosition)
    {
        return new Vector3Int(
            (int)(worldPosition.x / _cellSize),
            (int)(worldPosition.y / _cellSize),
            (int)(worldPosition.z / _cellSize));
    }
}