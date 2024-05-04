using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObtacleBorder : MonoBehaviour
{
    [SerializeField]
    private ScoreCounter _scoreCounter;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null)
        {
            _scoreCounter.IncreaseScore();
            Destroy(collision.gameObject, 0.1f);
        }
    }
}
