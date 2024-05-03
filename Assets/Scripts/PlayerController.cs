using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5.0f;

    Vector3Int _cellPos = Vector3Int.zero;
    MoveDirection _dir = MoveDirection.Down;
    bool _isMoving = false;
    Animator _animator;

    public MoveDirection Dir
    {
        get { return _dir; }
        set 
        { 
            if(_dir == value)
            {
                return;
            }

            switch(value)
            {
                case MoveDirection.Up:
                    _animator.Play("WALK_BACK");
                    transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

                    break;

                case MoveDirection.Right:
                    _animator.Play("WALK_RIGHT");
                    transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

                    break;

                case MoveDirection.Left:
                    _animator.Play("WALK_RIGHT");
                    transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);  
                    break;

                case MoveDirection.Down:
                    _animator.Play("WALK_FRONT");
                    transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    break;
                case MoveDirection.None:
                    if(_dir == MoveDirection.Up)
                    {
                        _animator.Play("IDLE_BACK");
                        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

                    }
                    else if(_dir == MoveDirection.Right)
                    {
                        _animator.Play("IDLE_RIGHT");
                        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

                    }
                    else if (_dir == MoveDirection.Left)
                    {
                        _animator.Play("IDLE_RIGHT");
                        transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                    }
                    else
                    {
                        _animator.Play("IDLE_FRONT");
                        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

                    }
                    break;
            }
            _dir = value;
        }
    }

    void Start()
    {
        _animator = GetComponent<Animator>();
        Vector3 pos = Managers.Map.CurrentGrid.CellToWorld(_cellPos) + new Vector3(0.5f, 0.5f, 0f);
        transform.position = pos;
    }

    // Update is called once per frame
    void Update()
    {
        GetDirInput();
        UpdatePosition();
        UpdateIsMoving();
 
    }

    private void LateUpdate()
    {
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -10f);
    }

    private void UpdatePosition()
    {
        if(_isMoving == false) { return; }

        // 도착지, 방향
        Vector3 destPos = Managers.Map.CurrentGrid.CellToWorld(_cellPos) + new Vector3(0.5f, 0.5f, 0);
        Vector3 destDir = destPos - transform.position;

        // 도착 여부 체크
        float dist = destDir.magnitude;
        if(dist < _speed * Time.deltaTime)
        {
            transform.position = destPos;
            _isMoving = false;
        }
        else
        {
            transform.position += destDir.normalized * _speed * Time.deltaTime;
            _isMoving = true;
        }
    }

    private void UpdateIsMoving()
    {
        if (_isMoving == false && _dir != MoveDirection.None)
        {
            Vector3Int destiPos = _cellPos;
            switch (_dir)
            {
                case MoveDirection.Up:
                    destiPos += Vector3Int.up;
                    break;

                case MoveDirection.Down:
                    destiPos += Vector3Int.down;
                    break;

                case MoveDirection.Left:
                    destiPos += Vector3Int.left;
                    break;

                case MoveDirection.Right:
                    destiPos += Vector3Int.right;
                    break;
            }

            if(Managers.Map.CanGo(destiPos))
            {
                _cellPos = destiPos;
                _isMoving = true;
            }

        }
    }

    private void GetDirInput()
    {
        if (Input.GetKey(KeyCode.W))
        {
            //transform.position += Vector3.up * Time.deltaTime * _speed;
            Dir = MoveDirection.Up;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            //transform.position += Vector3.down * Time.deltaTime * _speed;
            Dir = MoveDirection.Down;

        }
        else if (Input.GetKey(KeyCode.A))
        {
            //transform.position += Vector3.left * Time.deltaTime * _speed;
            Dir = MoveDirection.Left;

        }
        else if (Input.GetKey(KeyCode.D))
        {
            //transform.position += Vector3.right * Time.deltaTime * _speed;
            Dir = MoveDirection.Right;

        }
        else
        {
            Dir = MoveDirection.None;
        }
    }
}
