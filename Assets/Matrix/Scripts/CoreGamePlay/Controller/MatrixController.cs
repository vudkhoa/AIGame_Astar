namespace CoreGamePlay.Controller
{
    using CoreGamePlay.Model;
    using CoreGamePlay.View;
    using Data;
    using Unity.VisualScripting;
    using UnityEngine;

    public class MatrixController : MonoBehaviour
    {
        [Header("Matrix")]
        public MatrixElementModel[,] MatrixElementModelList;
        public MatrixElementView MatrixElementPrefab;
        public RectTransform MatrixElementParent;
        public Vector2Int Size;

        [Header("Data")]
        public MatrixData MatrixData;

        private void Start()
        {
            InitMatrix(1);
        }

        public void InitMatrix(int level)
        {
            int[,] map = MatrixData.GetMap(level - 1);

            Size.x = map.GetLength(0);
            Size.y = map.GetLength(1);

            this.MatrixElementModelList = new MatrixElementModel[Size.x, Size.y];

            for (int i = 0; i < Size.x; ++i)
            {
                for (int j = 0; j < Size.y; ++j)
                {
                    // Set View
                    MatrixElementView elementView =  Instantiate(MatrixElementPrefab, MatrixElementParent);
                    if (map[i, j] == 1)
                    {
                        elementView.SetColor(1);
                    }

                    // Set Model
                    MatrixElementModel elementModel = new MatrixElementModel();
                    elementModel.Init(new Vector2Int(i, j), elementView);

                    // Add to List
                    this.MatrixElementModelList[i, j] = elementModel;
                }
            }
        }
    }
}