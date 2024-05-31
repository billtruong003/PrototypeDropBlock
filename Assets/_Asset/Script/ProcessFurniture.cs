using System.Collections.Generic;
using BlockBuilder.BlockManagement;
using UnityEngine;

public class ProcessFurniture : MonoBehaviour
{
    public void ProcessDoor(GameObject block, BlockController currentBlockController)
    {
        if (!block.CompareTag("Block"))
            return;

        BlockController collideBlockController = block.GetComponent<BlockController>();
        Vector3 currentBlockCenter = currentBlockController.GetCenter();
        Vector3 collideBlockCenter = collideBlockController.GetCenter();
        Direction dir = ProcessDirection(currentBlockCenter, collideBlockCenter);

    }

    public List<Vector3> GetAllCubePosition(BlockController blockController)
    {
        List<Vector3> position = new();
        foreach (var item in blockController.GetTotalCube())
        {
            position.Add(item.transform.position);
        }
        return position;
    }

    public Direction ProcessDirection(Vector3 currentBlockCenter, Vector3 collideBlockCenter)
    {
        Vector3 directionVector = currentBlockCenter - collideBlockCenter;

        if (Mathf.Abs(directionVector.x) > Mathf.Abs(directionVector.z))
        {
            if (directionVector.x > 0)
                return Direction.RIGHT;
            else
                return Direction.LEFT;
        }
        else
        {
            if (directionVector.z > 0)
                return Direction.FORWARD;
            else
                return Direction.BACKWARD;
        }
    }
    public void AppearDoor(Direction dir)
    {
        switch (dir)
        {
            case Direction.RIGHT:

                break;
            case Direction.LEFT:
                break;
            case Direction.FORWARD:
                break;
            case Direction.BACKWARD:
                break;
        }
    }
}
