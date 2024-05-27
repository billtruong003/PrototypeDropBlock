using UnityEngine;

public interface IBlock
{
    void DropToCenter();
    void SetAllCubeToParent();

    public Transform GetPivot();
}
