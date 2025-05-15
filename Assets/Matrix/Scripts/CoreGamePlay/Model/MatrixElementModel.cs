namespace CoreGamePlay.Model
{
    using CoreGamePlay.View;
    using UnityEngine;

    public class MatrixElementModel
    {
        public Vector2Int Position;
        public MatrixElementView View;
        public int Type;

        public void Init(Vector2Int position, MatrixElementView view)
        {
            this.Position = position;
            this.View = view;
        }

        public void SetType(int type)
        {
            this.Type = type;
            this.View.SetColor(type);
        }
    }
}