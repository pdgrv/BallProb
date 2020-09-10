using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    private int _score;

    public UnityAction<int> ScoreChanged;

    private void Start()
    {
        ScoreChanged?.Invoke(_score);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Coin coin))
        {
            _score++;
            ScoreChanged?.Invoke(_score);
            Destroy(coin);
        }
    }
}
