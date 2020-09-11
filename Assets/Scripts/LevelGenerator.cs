using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private List<GridObject> _templates;
    [SerializeField] private Transform _player;
    [SerializeField] private float _viewRadius;
    [SerializeField] private float _cellSize;

    private HashSet<Vector3Int> _collisionsMatrix = new HashSet<Vector3Int>();

    private void Update()
    {
        FillLine(_player.position, _viewRadius);
    }

    private void FillLine(Vector3 center, float viewRadius)
    {
        var cellCountOnAxis = (int)(viewRadius / _cellSize);
        var fillAreaCenter = WorldToGridPosition(center);

        for (int x = -cellCountOnAxis; x <= cellCountOnAxis; x++)
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

        Instantiate(template, position, template.transform.rotation, transform);
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