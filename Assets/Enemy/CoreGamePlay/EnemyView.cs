namespace CoreGamePlay.View
{
    using UnityEngine;

    public class EnemyView : MonoBehaviour
    {
        private RectTransform _rectTransform;

        private void Awake()
        {
            this._rectTransform = GetComponent<RectTransform>();
        }

        public void SetPosition(Vector2Int position)
        {
            int yPos =  (1080 / 2) - (50 + position.x * 50 + (position.x - 1) * 4 - 25);
            int xPos = -(1920 / 2) + (98 + position.y * 50 + (position.y - 1) * 4 - 25);

            this._rectTransform.anchoredPosition = new Vector2(xPos, yPos);
        }
    }
}
