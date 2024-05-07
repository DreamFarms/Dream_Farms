using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class ArrowController : CreatureController
{
    protected override void Init()
    {
        switch (_lastDir)
        {
            case MoveDir.Up:
                transform.rotation = Quaternion.Euler(0, 0, 0);
                break;

            case MoveDir.Down:
                transform.rotation = Quaternion.Euler(0, 0, -180);
                break;

            case MoveDir.Left:
                transform.rotation = Quaternion.Euler(0, 0, 90);

                break;

            case MoveDir.Right:
                transform.rotation = Quaternion.Euler(0, 0, -90);
                break;
        }
        base.Init();
    }

    // ¾È ½á¿ä
    protected override void UpdateAnimation()
    {

    }

    protected override void UpdateIdle()
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
                GameObject go = Managers.Object.Find(destiPos);
                if (go == null)
                {
                    CellPos = destiPos;
                }
                else
                {
                    Debug.Log(go.name);
                    Managers.Resource.Destroy(gameObject);
                }
            }
            else
            {
                Managers.Resource.Destroy(gameObject);
            }

        }
    }
}
