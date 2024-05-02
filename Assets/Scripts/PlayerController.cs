using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public Grid _grid;
    [SerializeField]
    private float _speed = 5.0f;

    Vector3Int _cellPos = Vector3Int.zero;
    MoveDirection _dir = MoveDirection.None;
    bool _isMoving = false;

    void Start()
    {
        Vector3 pos = _grid.CellToWorld(_cellPos) + new Vector3(0.5f, 0.5f, 0);
        transform.position = pos;
    }

    // Update is called once per frame
    void Update()
    {
        GetDirInput();
        UpdatePosition();
        UpdateIsMoving();
 
    }

    private void UpdatePosition()
    {
        if(_isMoving == false) { return; }

        // 도착지, 방향
        Vector3 destPos = _grid.CellToWorld(_cellPos) + new Vector3(0.5f, 0.5f, 0);
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
        if (_isMoving == false)
        {
            switch (_dir)
            {
                case MoveDirection.Up:
                    _cellPos += Vector3Int.up;
                    _isMoving = true;
                    break;

                case MoveDirection.Down:
                    _cellPos += Vector3Int.down;
                    _isMoving = true;
                    break;

                case MoveDirection.Left:
                    _cellPos += Vector3Int.left;
                    _isMoving = true;
                    break;

                case MoveDirection.Right:
                    _cellPos += Vector3Int.right;
                    _isMoving = true;
                    break;
            }

        }
    }

    private void GetDirInput()
    {
        if (Input.GetKey(KeyCode.W))
        {
            //transform.position += Vector3.up * Time.deltaTime * _speed;
            _dir = MoveDirection.Up;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            //transform.position += Vector3.down * Time.deltaTime * _speed;
            _dir = MoveDirection.Down;

        }
        else if (Input.GetKey(KeyCode.A))
        {
            //transform.position += Vector3.left * Time.deltaTime * _speed;
            _dir = MoveDirection.Left;

        }
        else if (Input.GetKey(KeyCode.D))
        {
            //transform.position += Vector3.right * Time.deltaTime * _speed;
            _dir = MoveDirection.Right;

        }
        else
        {
            _dir = MoveDirection.None;
        }
    }
}
