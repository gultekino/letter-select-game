using JetBrains.Annotations;
using System.Linq;
using UnityEngine;

namespace Array2DEditor
{
    [System.Serializable]
    public class Array2DChar : Array2D<char>
    {
        [SerializeField]
        CellRowChar[] cells = new CellRowChar[Consts.defaultGridSize];

        protected override CellRow<char> GetCellRow(int idx)
        {
            return cells[idx];
        }

        /// <inheritdoc cref="Array2D{T}.Clone"/>
        protected override Array2D<char> Clone(CellRow<char>[] cells)
        {
            return new Array2DChar() { cells = cells.Select(r => new CellRowChar(r)).ToArray() };
        }
    }

    [System.Serializable]
    public class CellRowChar : CellRow<char>
    {
        /// <inheritdoc/>
        [UsedImplicitly]
        public CellRowChar() { }

        /// <inheritdoc/>
        public CellRowChar(CellRow<char> row)
            : base(row) { }
    }
}