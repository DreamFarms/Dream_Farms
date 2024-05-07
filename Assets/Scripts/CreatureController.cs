using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using static Define;

public class CreatureController : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5.0f;

    public Vector3Int CellPos { get; set; } = Vector3Int.zero;

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


    protected MoveDir _lastDir = MoveDir.Down;
    protected MoveDir _dir = MoveDir.Down;
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

            // �ִϸ��̼� ���� �޼��� ����
            UpdateAnimation();
        }
    }

    // ���� �ٶ󺸴� ������ ��ĭ �� ĭ ��ǥ�� ����
    // nĭ �� ĭ�� �������� �Ű��� ���ڸ� �ְ� Vector3Int.up * n;
    public Vector3Int GetFrontCellPos()
    {
        Vector3Int cellPos = CellPos;

        switch(_lastDir)
        {
            case MoveDir.Up:
                cellPos += Vector3Int.up;
                break;
            case MoveDir.Down:
                cellPos += Vector3Int.down;
                break;
            case MoveDir.Left:
                cellPos += Vector3Int.left;
                break;
            case MoveDir.Right:
                cellPos += Vector3Int.right;
                break;
        }

        return cellPos;
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
        Vector3 pos = Managers.Map.CurrentGrid.CellToWorld(CellPos) + new Vector3(0.5f, 0.5f, 0f);
        transform.position = pos;

        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected virtual void UpdateController()
    {
        switch(State)
        {
            case CreatureState.Idle:
                UpdateIdle();
                break;
            case CreatureState.Moving:
                 UpdateMoving();
                break;
            case CreatureState.Skill:
                UpdateSkill();
                break;
            case CreatureState.Dead:
                UpdateDead();
                break;
        }
    }


    // �̵� ������ ������ ��, �̵�
    protected virtual void UpdateMoving()
    {
        if (State != CreatureState.Moving) { return; }

        // ������, ����
        Vector3 destPos = Managers.Map.CurrentGrid.CellToWorld(CellPos) + new Vector3(0.5f, 0.5f, 0);
        Vector3 destDir = destPos - transform.position;

        // ���� ���� üũ
        float dist = destDir.magnitude;
        if (dist < _speed * Time.deltaTime)
        {
            transform.position = destPos;
            // ���������� �ִϸ��̼��� ���� ��Ʈ���ϱ�
            _state = CreatureState.Idle;
            if(_dir == MoveDir.None)
            {
                // �÷��̾ �������� ������
                // �ִϸ��̼� ������Ʈ
                UpdateAnimation();
            }
        }
        else
        {
            transform.position += destDir.normalized * _speed * Time.deltaTime;
            State = CreatureState.Moving;
        }
    }

    // �̵� ������Ʈ
    protected virtual void UpdateIdle()
    {
        if (_dir != MoveDir.None)
        {
            Vector3Int destiPos = CellPos;
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

            State = CreatureState.Moving;

            if (Managers.Map.CanGo(destiPos))
            {
                if(Managers.Object.Find(destiPos) == null)
                {
                    CellPos = destiPos;
                }
            }

        }
    }


    // ��ų ������Ʈ
    protected virtual void UpdateSkill()
    {

    }


    // ���� ������Ʈ
    protected virtual void UpdateDead()
    {

    }


    protected virtual void UpdateAnimation()
    {
        // �⺻ ���¶��?
        if(State == CreatureState.Idle)
        {
            // ������ ���⿡ ���� �׿� �´� �ִϸ��̼� ����
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
            switch (_lastDir)
            {
                case MoveDir.Up:
                    _animator.Play("ATTACK_BACK");
                    _spriteRenderer.flipX = false;
                    break;

                case MoveDir.Right:
                    _animator.Play("ATTACK_RIGHT");
                    _spriteRenderer.flipX = false;
                    break;

                case MoveDir.Left:
                    _animator.Play("ATTACK_RIGHT");
                    _spriteRenderer.flipX = true;
                    //transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                    break;

                case MoveDir.Down:
                    _animator.Play("ATTACK_FRONT");
                    _spriteRenderer.flipX = false;
                    break;
            }
        }
        else
        {
            // ��Ÿ �ִϸ��̼� ����� �߰��ϱ�
        }
    }
}
