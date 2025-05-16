namespace CoreGamePlay.Model
{
    using CoreGamePlay.View;
    using UnityEngine;
    using System.Collections.Generic;

    public class EnemyModel
    {
        public EnemyView EnemyView;
        public Vector2Int Position;
        
        public void Init(EnemyView enemyView, Vector2Int position)
        {
            this.EnemyView = enemyView;
            this.SetPosition(position);   
        }

        public void SetPosition(Vector2Int position)
        {
            this.Position = position;
            this.EnemyView.SetPosition(position);
        }
    }
}