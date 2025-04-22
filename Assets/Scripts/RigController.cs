using UnityEngine;
using UnityEngine.InputSystem;

public class RigController : MonoBehaviour
{
    public enum CardinalDirection { Forward, ForwardLeft, ForwardRight, Left, Right, Backward, BackwardRight, BackwardLeft, None};
    public Transform MoveDirection;
    public CardinalDirection cardinalDirection;

    PlayerController pc;
    InputAction moveAction;

    float OrientationAngle;

    private void Start()
    {
        pc = GetComponent<PlayerController>();
    }
    void Update()
    {
        moveAction = pc.GetMoveAction();
        Vector3 input = moveAction.ReadValue<Vector3>();
        #region ÀÎÇ² º¸Á¤

        #region ÀÎÇ²º¸Á¤

        if (input.z > 0)
        {
            input.z = 1;
        }

        else if (input.z < 0)
        {
            input.z = -1;
        }

        else
        {
            input.z = 0;
        }

        if (input.x > 0)
        {
            input.x = 1;
        }

        else if (input.x < 0)
        {
            input.x = -1;
        }

        else
        {
            input.x = 0;
        }
        #endregion
        #region Set CardinalDirection
        if (input.z != 0)
        {
            if (input.z > 0)
            {
                if (input.x > 0)
                {
                    cardinalDirection = CardinalDirection.ForwardRight;
                }

                else if (input.x < 0)
                {
                    cardinalDirection = CardinalDirection.ForwardLeft;
                }

                else
                {
                    cardinalDirection = CardinalDirection.Forward;
                }
            }

            else if (input.z < 0)
            {
                if (input.x > 0)
                {
                    cardinalDirection = CardinalDirection.BackwardRight;
                }

                else if (input.x < 0)
                {
                    cardinalDirection = CardinalDirection.BackwardLeft;
                }

                else
                {
                    cardinalDirection = CardinalDirection.Backward;
                }
            }
        }

        else
        {
            if (input.x > 0)
            {
                cardinalDirection = CardinalDirection.Right;
            }

            else if (input.x < 0)
            {
                cardinalDirection = CardinalDirection.Left;
            }

            else
            {
                cardinalDirection = CardinalDirection.None;
            }
        }
        #endregion
        switch (cardinalDirection)
        {
            case CardinalDirection.Forward:
                OrientationAngle = 0;
                break;

            case CardinalDirection.Backward:
                OrientationAngle = 0;
                break;

            case CardinalDirection.Right:
                OrientationAngle = 90;
                break;

            case CardinalDirection.Left:
                OrientationAngle = 90;
                break;
            case CardinalDirection.ForwardLeft:
                OrientationAngle = -45;
                break;

            case CardinalDirection.ForwardRight:
                OrientationAngle = 45;
                break;

            case CardinalDirection.BackwardRight:
                OrientationAngle = -45;
                break;

            case CardinalDirection.BackwardLeft:
                OrientationAngle = 45;
                break;
        }

        MoveDirection.localRotation = Quaternion.Lerp(MoveDirection.localRotation, Quaternion.Euler(0, OrientationAngle, 0), Time.deltaTime * 5f);



        #endregion
        Vector3 targetDir = transform.forward * input.z + transform.right * input.x;
    }

    
}
