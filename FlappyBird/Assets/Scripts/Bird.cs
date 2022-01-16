using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    
    [Header("broadcasting")]
    [SerializeField] private VoidEventChannelSO _birdDiedEvent = default;
    [SerializeField] private VoidEventChannelSO _startGameEvent = default;

    [SerializeField]
    private float birdJumpAmount = 80;

    private State _state;
    private enum State
    {
        WaitingToState,
        Playing,
        End,
    }
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.bodyType = RigidbodyType2D.Static;
    }
    

    // Update is called once per frame
    void Update()
    {
        switch (_state)
        {
            case State.WaitingToState:
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
                {
                    _state = State.Playing;
                    _rigidbody.bodyType = RigidbodyType2D.Dynamic;
                    Jump();
                    _startGameEvent.RaiseEvent();
                }
                break;
            case State.Playing:
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
                {
                    Jump();
                }

                transform.eulerAngles = new Vector3(0, 0, _rigidbody.velocity.y * .3f + 10);
                break;
            default:
                break;;
        }

    }
    
    private void Jump()
    {
        _rigidbody.velocity = Vector3.up * birdJumpAmount;
        MusicMgr.GetInstance().PlaySound("BirdJump");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        BirdDied();
    }

    private void BirdDied()
    {
        Score.TrySetNewHighScore(Level.GetInstance().GetPipesPassed());
        
        _state = State.End;
        _rigidbody.bodyType = RigidbodyType2D.Static;
        MusicMgr.GetInstance().PlaySound("Lose");
        _birdDiedEvent.RaiseEvent();
    }
}
