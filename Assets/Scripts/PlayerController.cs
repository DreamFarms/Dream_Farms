using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using static Define;

public class PlayerController : CreatureController
{
    Coroutine _coSkill;
    protected override void Init()
    {
        base.Init();

    }

    protected override void UpdateController()
    {
        switch(State)
        {
            case CreatureState.Idle:
                GetDirInput();
                GetIdleInput();
                break;
        }    
        GetDirInput();
        base.UpdateController();
    }


    // camera
    private void LateUpdate()
    {
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -10f);
    }


    private void GetDirInput()
    {
        if (Input.GetKey(KeyCode.W))
        {
            //transform.position += Vector3.up * Time.deltaTime * _speed;
            Dir = MoveDir.Up;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            //transform.position += Vector3.down * Time.deltaTime * _speed;
            Dir = MoveDir.Down;

        }
        else if (Input.GetKey(KeyCode.A))
        {
            //transform.position += Vector3.left * Time.deltaTime * _speed;
            Dir = MoveDir.Left;

        }
        else if (Input.GetKey(KeyCode.D))
        {
            //transform.position += Vector3.right * Time.deltaTime * _speed;
            Dir = MoveDir.Right;

        }
        else
        {
            Dir = MoveDir.None;

            
        }
    }
    private void GetIdleInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            State = CreatureState.Skill;
            _coSkill = StartCoroutine("CoStartPunch");
        }
    }

    IEnumerator CoStartPunch()
    {
        // 피격 판정
        GameObject go = Managers.Object.Find(GetFrontCellPos());
        if(go != null)
        {
            Debug.Log(go.name);
        }

        // 대기
       yield return new WaitForSeconds(0.5f);
        State = CreatureState.Idle;
        _coSkill = null;
    }
}
