using UnityEngine;
using CustomUtils;
using CoreGamePlay.View;
using CoreGamePlay.Controller;

public class PlayerController : SingletonMono<PlayerController>
{
    public Vector2Int startGrid = Vector2Int.zero;

    [Header("Player Data")]
    public PlayerView playerPrefab;
    public RectTransform parent;

    public  PlayerModel model;
    private PlayerView view;

    public void SpawnPlayer()
    {
        model = new PlayerModel(startGrid);
        view = Instantiate(playerPrefab, parent);
        this.view.GetComponent<RectTransform>().anchoredPosition = view.GridToWorld(model.gridPosition);
    }

    private void Update()
    {
        if (!view.IsMoving)
        {
            Vector2Int dir = GetInputDirection();
            if(dir != Vector2Int.zero)
            {
                bool check = model.Move(dir);
                if (check)
                {
                    StartCoroutine(view.MoveToPosition(model.gridPosition));
                    EnemyController.Instance.FocusPlayer(new Vector2Int(model.gridPosition.x + 1, model.gridPosition.y + 1));
                }
            }
        }
    }

    Vector2Int GetInputDirection()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) return new Vector2Int(-1, 0);
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) return new Vector2Int(1, 0);
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) return new Vector2Int(0, -1);
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) return new Vector2Int(0, 1);
        return Vector2Int.zero;
    }
}
