using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static Define;

public class CreatureController : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5.0f;

    protected Vector3Int _cellPos = Vector3Int.zero;
    protected Animator _animator;
    protected SpriteRenderer _spriteRenderer;


    CreatureState _state = CreatureState.Idle;
    public CreatureState State
    {
        get { return _state; }
        set
        {
            if(_state == value)
            {
                return;
            }

            _state = value;
            UpdateAnimation();
        }
    }


    MoveDir _lastDir = MoveDir.Down;
    MoveDir _dir = MoveDir.Down;
    public MoveDir Dir
    {
        get { return _dir; }
        set
        {
            if (_dir == value)
            {
                return;
            }
            _dir = value;

            if(value != MoveDir.None)
            {
                _lastDir = value;
            }

            // 애니메이션 변명 메서드 실행
            UpdateAnimation();
        }
    }

    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateController();
    }

    protected virtual void Init()
    {
        _animator = GetComponent<Animator>();
        Vector3 pos = Managers.Map.CurrentGrid.CellToWorld(_cellPos) + new Vector3(0.5f, 0.5f, 0f);
        transform.position = pos;

        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected virtual void UpdateController()
    {
        UpdatePosition();
        UpdateIsMoving();
    }


    private void UpdatePosition()
    {
        if (State != CreatureState.Moving) { return; }

        // 도착지, 방향
        Vector3 destPos = Managers.Map.CurrentGrid.CellToWorld(_cellPos) + new Vector3(0.5f, 0.5f, 0);
        Vector3 destDir = destPos - transform.position;

        // 도착 여부 체크
        float dist = destDir.magnitude;
        if (dist < _speed * Time.deltaTime)
        {
            transform.position = destPos;
            // 예외적으로 애니메이션을 직접 컨트롤하기
            _state = CreatureState.Idle;
            if(_dir == MoveDir.None)
            {
                // 플레이어가 움직이지 않으면
                // 애니메이션 업데이트
                UpdateAnimation();
            }
        }
        else
        {
            transform.position += destDir.normalized * _speed * Time.deltaTime;
            State = CreatureState.Moving;
        }
    }

    private void UpdateIsMoving()
    {
        if (State == CreatureState.Idle && _dir != MoveDir.None)
        {
            Vector3Int destiPos = _cellPos;
            switch (_dir)
            {
                case MoveDir.Up:
                    destiPos += Vector3Int.up;
                    break;

                case MoveDir.Down:
                    destiPos += Vector3Int.down;
                    break;

                case MoveDir.Left:
                    destiPos += Vector3Int.left;
                    break;

                case MoveDir.Right:
                    destiPos += Vector3Int.right;
                    break;
            }

            if (Managers.Map.CanGo(destiPos))
            {
                _cellPos = destiPos;
                State = CreatureState.Moving;
            }

        }
    }

    protected virtual void UpdateAnimation()
    {
        // 기본 상태라면?
        if(State == CreatureState.Idle)
        {
            // 마지막 방향에 따라 그에 맞는 애니메이션 실행
            switch(_lastDir)
            {
                case MoveDir.Up:
                    _animator.Play("IDLE_BACK");
                    _spriteRenderer.flipX = false;
                    break;
                case MoveDir.Down:
                    _animator.Play("IDLE_FRONT");
                    _spriteRenderer.flipX = false;
                    break;
                case MoveDir.Left:
                    _animator.Play("IDLE_RIGHT");
                    _spriteRenderer.flipX = true;
                    break;
                case MoveDir.Right:
                    _animator.Play("IDLE_RIGHT");
                    _spriteRenderer.flipX = false;

                    break;
            }
        }
        else if(State == CreatureState.Moving)
        {
            switch (_dir)
            {
                case MoveDir.Up:
                    _animator.Play("WALK_BACK");
                    _spriteRenderer.flipX = false;
                    break;

                case MoveDir.Right:
                    _animator.Play("WALK_RIGHT");
                    _spriteRenderer.flipX = false;


                    break;

                case MoveDir.Left:
                    _animator.Play("WALK_RIGHT");
                    _spriteRenderer.flipX = true;
                    //transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                    break;

                case MoveDir.Down:
                    _animator.Play("WALK_FRONT");
                    _spriteRenderer.flipX = false;
                    break;
            }
        }
        else if(State == CreatureState.Skill)
        {
            // 스킬
        }
        else
        {
            // 기타 애니메이션 생기면 추가하기
        }
    }
}
