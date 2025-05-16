namespace CoreGamePlay.View
{
    using UnityEngine;
    using UnityEngine.UI;

    public class MatrixElementView : MonoBehaviour
    {
        public Image Image;
        public void SetColor(int type)
        {
            if (type == 1)
            {
                this.Image.color = Color.gray;
            }
        }
    }
} 
