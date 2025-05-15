namespace CoreGamePlay.Controller
{
    using CoreGamePlay.View;
    using CoreGmePlay.Model;
    using CustomUtils;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public enum Direction
    {
        Up = 1,
        Down = 2, 
        Left = 3, 
        Right = 4, 
        None = 0
    }

    public class OpenListElement
    {
        public float FCost;
        public float GCost;
        public Vector2Int Current;
        public Vector2Int Parent;
    }

    public class CameFromElement
    {
        public Vector2Int Parent;
        public Vector2Int Child;
    }

    public class EnemyController : SingletonMono<EnemyController>
    {
        [Header("Enemy Data")]
        public EnemyView enemyViewPrefab;
        public RectTransform parent;
        public List<EnemyModel> EnemyModelList;
        private bool _isMoving = false;

        public void SpawnEnemy()
        {
            if (EnemyModelList == null)
            {
                EnemyModelList = new List<EnemyModel>();
            }

            // View
            EnemyView enemyView = Instantiate(enemyViewPrefab, parent);
            Vector2Int newPos = new Vector2Int(0, 0);
            while (MatrixController.Instance.MatrixElementModelList[newPos.x, newPos.y].Type == 1)
            {
                newPos.x = Random.Range(0, MatrixController.Instance.Size.x);
                newPos.y = Random.Range(0, MatrixController.Instance.Size.y);
            }
            newPos.x += 1;
            newPos.y += 1;

            enemyView.SetPosition(newPos);

            // Model
            EnemyModel enemyModel = new EnemyModel();
            enemyModel.Init(enemyView, newPos);
            EnemyModelList.Add(enemyModel);

            //List<Vector2Int> path = this.AStar(posDirection, new Vector2Int(10, 30));
            //StartCoroutine(MovePath(path));
        }

        public void FocusPlayer(Vector2Int posPlayer)
        {
            Vector2Int posDirection = this.EnemyModelList[0].Position;
            List<Vector2Int> path = this.AStar(posDirection, posPlayer);

            Debug.Log(posDirection + " " + posPlayer);

            if (this._isMoving)
            {
                StopAllCoroutines();
            }

            if (path != null)
            {
                StartCoroutine(MovePath(path));
            }
        }

        public List<Vector2Int> AStar(Vector2Int start, Vector2Int goal)
        {
            float[,] gScore = new float[16, 32];
            Vector2Int[,] cameFrom = new Vector2Int[16, 32];
            List<OpenListElement> openList = new List<OpenListElement>();
            HashSet<Vector2Int> closedList = new HashSet<Vector2Int>();

            for (int i = 0; i < 16; i++)
                for (int j = 0; j < 32; j++)
                {
                    gScore[i, j] = float.MaxValue;
                    cameFrom[i, j] = new Vector2Int(-1, -1);
                }

            gScore[start.x, start.y] = 0;
            openList.Add(new OpenListElement
            {
                GCost = 0,
                FCost = 0,
                Current = start,
                Parent = new Vector2Int(-1, -1)
            });

            while (openList.Count > 0)
            {
                OpenListElement current = openList.OrderBy(e => e.FCost).First();
                openList.Remove(current);
                closedList.Add(current.Current);

                if (current.Current == goal)
                {
                    List<Vector2Int> path = new List<Vector2Int>();
                    Vector2Int child = current.Current;
                    Vector2Int parent = cameFrom[child.x, child.y];
                    path.Add(goal);
                    while (parent.x != -1)
                    {
                        path.Add(parent);
                        child = parent;
                        parent = cameFrom[child.x, child.y];
                    }
                    return path;
                }

                for (int i = 1; i <= 4; ++i)
                {
                    Direction dir = (Direction)i;
                    Vector2Int neighbor = this.Actions(dir, current.Current);
                    if (neighbor.x == -1 || closedList.Contains(neighbor)) continue;

                    float tentativeG = gScore[current.Current.x, current.Current.y] + 1;

                    bool inOpenList = openList.Any(e => e.Current == neighbor);

                    if (!inOpenList || tentativeG < gScore[neighbor.x, neighbor.y])
                    {
                        gScore[neighbor.x, neighbor.y] = tentativeG;
                        float fScore = tentativeG;
                        cameFrom[neighbor.x, neighbor.y] = current.Current;

                        if (!inOpenList)
                        {
                            openList.Add(new OpenListElement
                            {
                                GCost = tentativeG,
                                FCost = fScore,
                                Current = neighbor,
                                Parent = current.Current
                            });
                        }
                    }
                }
            }
            return null;
        }


        public Vector2Int Actions(Direction dir, Vector2Int pos)
        {
            Vector2Int posDirection = new Vector2Int(0, 0);

            switch (dir)
            {
                case Direction.Up:
                    posDirection = new Vector2Int(-1, 0);
                    break;
                case Direction.Down:
                    posDirection = new Vector2Int(1, 0);
                    break;
                case Direction.Left:
                    posDirection = new Vector2Int(0, -1);
                    break;
                case Direction.Right:
                    posDirection = new Vector2Int(0, 1);
                    break;
            }

            Vector2Int newPos = pos + posDirection;
            if (newPos.x < 0 && newPos.y < 0) return new Vector2Int(-1, -1);
            if (MatrixController.Instance.MatrixElementModelList[newPos.x - 1, newPos.y - 1].Type == 1) 
                return new Vector2Int(-1, -1);

            return newPos;
        }


        IEnumerator MovePath(List<Vector2Int> path)
        {
            this._isMoving = true;
            for (int i = path.Count - 1; i >= 0; i--) 
            {
                Vector2Int newPos = path[i]; 
                if (MatrixController.Instance.MatrixElementModelList[newPos.x - 1, newPos.y - 1].Type != 1)
                {
                    this.EnemyModelList[0].SetPosition(newPos);
                    yield return new WaitForSeconds(0.2f);
                }
            }
            this._isMoving = false;
        }

        public void Move(Direction dir, int index)
        {
            Vector2Int posDirection = new Vector2Int(0, 0);

            switch (dir)
            {
                case Direction.Up:
                    posDirection = new Vector2Int(-1, 0);
                    break;
                case Direction.Down:
                    posDirection = new Vector2Int(1, 0);
                    break;
                case Direction.Left:
                    posDirection = new Vector2Int(0, -1);
                    break;
                case Direction.Right:
                    posDirection = new Vector2Int(0, 1);
                    break;
            }

            Vector2Int newPos = this.EnemyModelList[index].Position + posDirection;

            if (MatrixController.Instance.MatrixElementModelList[newPos.x - 1, newPos.y - 1].Type == 1) return;

            this.EnemyModelList[0].SetPosition(newPos);
        }
    }
}