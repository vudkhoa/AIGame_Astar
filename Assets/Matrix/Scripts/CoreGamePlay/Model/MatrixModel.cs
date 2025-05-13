namespace CoreGamePlay.Model
{
    using CoreGamePlay.View;
    using UnityEngine;

    public class MatrixModel
    {
        public Vector2Int Position;
        public MatrixView View;
        public void Init(Vector2Int position, MatrixView view)
        {
            this.Position = position;
            this.View = view;
        }
    }
}
