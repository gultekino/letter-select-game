using UnityEditor;

namespace Array2DEditor
{
    [CustomPropertyDrawer(typeof(Array2DChar))]
    public class Array2DCharDrawer : Array2DDrawer
    {
        protected override object GetDefaultCellValue() => 'a';

        protected override object GetCellValue(SerializedProperty cell) => cell.intValue;

        protected override void SetValue(SerializedProperty cell, object obj)
        {
            cell.intValue = (char) obj;
        }
    }
}