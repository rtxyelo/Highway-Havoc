using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using SimpleInputNamespace;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rb;

    private MoveDirection _moveDirection = MoveDirection.None;

    [SerializeField]
    private ScoreCounter _scoreCounter;

    private AudioController _audioController;

    private bool _isCanMove = true;

    [HideInInspector]
    public UnityEvent<GameObject> IsBallHit;

    [SerializeField]
    private SteeringWheel _steeringWheel;

    private float _speed = 0f;

    private void Awake()
    {
        IsBallHit = new UnityEvent<GameObject>();

        if (TryGetComponent(out Rigidbody2D rb))
        {
            _rb = rb;
        }
        else
        {
            new NullReferenceException("Check Player RigidBody!");
        }

        _audioController = FindObjectOfType<AudioController>();
    }

    private void Update()
    {
        _speed = _steeringWheel.Value;

        if (Mathf.Sign(_speed) == Mathf.Sign(-1) && _speed != 0f)
            ChangeMoveSide(0);
        else if (Mathf.Sign(_speed) == Mathf.Sign(1) && _speed != 0f)
            ChangeMoveSide(2);
        else
            ChangeMoveSide(4);
    }

    private void FixedUpdate()
    {
        if (_moveDirection != MoveDirection.None && _isCanMove)
        {
            switch (_moveDirection)
            {
                case MoveDirection.Left:
                    MovePlayer(-1);
                    break;

                case MoveDirection.Right:
                    MovePlayer(1);
                    break;

                default:
                    break;
            }
        }
    }

    private void MovePlayer(int moveDir)
    {
        _rb.velocity = new Vector2(moveDir * Time.deltaTime * Mathf.Abs(_speed), 0f);
    }

    public void ChangeMoveSide(int moveDirection)
    {
        _moveDirection = (MoveDirection)moveDirection;
        _rb.velocity = Vector2.zero;

        if (_audioController != null)
            _audioController.PlayBoatSound();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.CompareTag("Obtacle"))
            {
                if (_audioController != null)
                    _audioController.PlayLogSound();

                _scoreCounter.DecreaseScore();
                collision.gameObject.tag = "ObtacleCrashed";
                Destroy(collision.gameObject, 0.2f);
            }
        }
    }
}
