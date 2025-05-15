using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    public float cellSize = 1f;
    public float moveDuration = 0.2f;
    private bool isMoving = false;
    public bool IsMoving => isMoving;

    public Vector3 GridToWorld(Vector2Int girdPos)
    {
        return new Vector3(girdPos.x * cellSize + cellSize * 0.5f, girdPos.y * cellSize + cellSize * 0.5f, transform.position.z);
    }

    public IEnumerator MoveToPosition (Vector2Int targetGrid)
    {
        isMoving = true;

        Vector3 start = transform.position;
        Vector3 end = GridToWorld(targetGrid);
        float time = 0f;

        while(time < moveDuration)
        {
            transform.position = Vector3.Lerp(start, end, time/moveDuration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.position = end;
        isMoving = false;
    }

}
