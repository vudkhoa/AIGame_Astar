using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Vector2Int startGrid = Vector2Int.zero;

    private PlayerModel model;
    private PlayerView view;
    private void Start()
    {
        model = new PlayerModel(startGrid);
        view = GetComponent<PlayerView>();
        transform.position = view.GridToWorld(model.gridPosition);
    }

    private void Update()
    {
        if (!view.IsMoving)
        {
            Vector2Int dir = GetInputDirection();
            if(dir != Vector2Int.zero)
            {
                model.Move(dir);
                StartCoroutine(view.MoveToPosition(model.gridPosition));
            }
        }
    }

    Vector2Int GetInputDirection()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) return Vector2Int.up;
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) return Vector2Int.down;
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) return Vector2Int.left;
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) return Vector2Int.right;
        return Vector2Int.zero;
    }
}
