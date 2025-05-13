namespace Data 
{
    using System.Collections.Generic;
    using UnityEngine;

    public class MatrixData : MonoBehaviour
    {
        public int[,] map = new int[8, 16]
        {
            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
            {1,0,0,0,1,0,0,0,0,1,0,0,0,0,0,1},
            {1,0,1,0,1,0,1,1,0,1,0,1,1,1,0,1},
            {1,0,1,0,0,0,0,1,0,1,0,0,0,1,0,1},
            {1,0,1,1,1,1,0,1,0,1,1,1,0,1,0,1},
            {1,0,0,0,0,1,0,0,0,0,0,1,0,1,0,1},
            {1,1,1,1,0,1,1,1,1,1,0,1,0,0,0,1},
            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
        };

        public List<int[,]> MapList;

        private void Init()
        {
            MapList.Add(map);
        }

        public int[,] GetMap(int levelIndex)
        {
            return this.MapList[levelIndex];
        }
    }
}
