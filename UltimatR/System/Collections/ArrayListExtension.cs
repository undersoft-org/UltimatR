
namespace System.Collections
{
    public static class ArrayListExtensions
    {
        #region Methods

        public static ArrayList Resize(this ArrayList array, int size)
        {
            int resize = size - array.Count;
            ArrayList fill = ArrayList.Repeat(null, resize);
            (array).Capacity = size;
            (array).AddRange(fill);
            return array;
        }

        #endregion
    }
}
