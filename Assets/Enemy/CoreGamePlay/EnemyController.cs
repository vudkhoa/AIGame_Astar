namespace CoreGamePlay.Controller
{
    using CoreGamePlay.View;
    using CoreGamePlay.Model;
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
        UpLeft = 5,
        UpRight = 6,
        DownLeft = 7,
        DownRight = 8,
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
        public int SizeEnemyModelList;
        private bool _isMoving = false;

        public void SpawnListEnemy()
        {
            for (int i = 0; i < SizeEnemyModelList; ++i)
            {
                this.SpawnEnemy();
            }
        }

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
        }

        public void AllFocusPlayer(Vector2Int posPlayer)
        {
            if (this._isMoving)
            {
                StopAllCoroutines();
            }

            for (int i = 0; i < SizeEnemyModelList; ++i)
            {
                this.FocusPlayer(i, this.EnemyModelList[i].Position, posPlayer);
            }
        }

        public void FocusPlayer(int index, Vector2Int posEnemy, Vector2Int posPlayer)
        {
            List<Vector2Int> path = this.AStar(posEnemy, posPlayer);

            if (path != null)
            {
                StartCoroutine(MovePath(index, path));
            }
        }

        public float EuclideanDistance(Vector2 pos1, Vector2 pos2)
        {
            return Vector2.Distance(pos1, pos2);
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

                for (int i = 1; i <= 8; ++i)
                {
                    Direction dir = (Direction)i;
                    Vector2Int neighbor = this.Actions(dir, current.Current);
                    if (neighbor.x == -1 || closedList.Contains(neighbor)) continue;

                    float tentativeG = gScore[current.Current.x, current.Current.y] + 1;
                    bool inOpenList = openList.Any(e => e.Current == neighbor);

                    if (!inOpenList || tentativeG < gScore[neighbor.x, neighbor.y])
                    {
                        gScore[neighbor.x, neighbor.y] = tentativeG;
                        Vector2 pos1 = new Vector2(current.Current.x, current.Current.y);
                        Vector2 pos2 = new Vector2(neighbor.x, neighbor.y);
                        float fScore = tentativeG + this.EuclideanDistance(pos1, pos2);
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
                case Direction.UpLeft:
                    posDirection = new Vector2Int(-1, -1);
                    break;
                case Direction.UpRight:
                    posDirection = new Vector2Int(-1, 1);
                    break;
                case Direction.DownLeft:
                    posDirection = new Vector2Int(1, -1);
                    break;
                case Direction.DownRight:
                    posDirection = new Vector2Int(1, 1);
                    break;
            }

            Vector2Int newPos = pos + posDirection;
            if (newPos.x < 0 && newPos.y < 0) return new Vector2Int(-1, -1);
            if (MatrixController.Instance.MatrixElementModelList[newPos.x - 1, newPos.y - 1].Type == 1) 
                return new Vector2Int(-1, -1);

            if (dir == Direction.UpLeft &&
                    MatrixController.Instance.MatrixElementModelList[newPos.x - 1, newPos.y].Type == 1 &&
                    MatrixController.Instance.MatrixElementModelList[newPos.x, newPos.y - 1].Type == 1
                )
            {
                return new Vector2Int(-1, -1);
            }

            if (dir == Direction.UpRight &&
                    MatrixController.Instance.MatrixElementModelList[newPos.x - 1, newPos.y - 2].Type == 1 &&
                    MatrixController.Instance.MatrixElementModelList[newPos.x, newPos.y - 1].Type == 1
                )
            {
                return new Vector2Int(-1, -1);
            }

            if (dir == Direction.DownLeft &&
                    MatrixController.Instance.MatrixElementModelList[newPos.x - 2, newPos.y - 1].Type == 1 &&
                    MatrixController.Instance.MatrixElementModelList[newPos.x - 1, newPos.y].Type == 1
                )
            {
                return new Vector2Int(-1, -1);
            }

            if (dir == Direction.DownRight &&
                    MatrixController.Instance.MatrixElementModelList[newPos.x - 2, newPos.y - 1].Type == 1 &&
                    MatrixController.Instance.MatrixElementModelList[newPos.x - 1, newPos.y - 2].Type == 1
                )
            {
                return new Vector2Int(-1, -1);
            }

            return newPos;
        }


        IEnumerator MovePath(int index, List<Vector2Int> path)
        {
            this._isMoving = true;
            for (int i = path.Count - 1; i >= 0; i--) 
            {
                Vector2Int newPos = path[i]; 
                if (MatrixController.Instance.MatrixElementModelList[newPos.x - 1, newPos.y - 1].Type != 1)
                {
                    float time = 0.2f;
                    bool check = EnemyModelList.Any(m => m.Position == newPos && EnemyModelList.IndexOf(m) != index);

                    if (check)
                    {
                        time = 0.3f;
                        yield return new WaitForSeconds(time);
                        this.EnemyModelList[index].SetPosition(newPos);
                    }
                    else
                    {
                        time = 0.2f;
                        this.EnemyModelList[index].SetPosition(newPos);
                        yield return new WaitForSeconds(time);
                    }
                }
            }
            this._isMoving = false;
        }
    }
}