using System;
using System.Linq;

namespace Xema.Core
{
    public class Matrix<T>
    {
        private readonly T[][] _array;

        public T[][] Entries { get => _array; }
        public int Rows { get => _array.Length; }
        public int Columns { get; private set; }

        #region Constructors
        public Matrix()
        {
            _array = Array.Empty<T[]>();
            Columns = 0;
        }

        public Matrix(int n, int m)
        {

            _array = new T[n][];
            Columns = m;

            for (int i = 0; i < n; i++)
            {
                _array[i] = new T[m];
            }
        }

        public Matrix(Matrix<T> matrix)
        {
            _array = new T[matrix.Rows][];
            Columns = matrix.Columns;

            for (int i = 0; i < matrix.Rows; i++)
            {
                _array[i] = matrix[i];
            }
        }
        #endregion

        #region Indexers
        public T this[int rowIndex, int columnIndex]
        {
            get
            {
                return _array[rowIndex][columnIndex];
            }

            set
            {
                _array[rowIndex][columnIndex] = value;
            }
        }

        public T[] this[int rowIndex]
        {

            get
            {
                return _array[rowIndex];
            }

            set
            {
                _array[rowIndex] = value;
            }
        }
        #endregion

        public T[] GetColumn(int columnIndex) => _array.Select(row => row[columnIndex]).ToArray();

        public Matrix<T> ReorderRows(int startRowIndex, int endRowIndex)
        {
            for (int j = 0; j < Columns; j++)
            {
                var temp = _array[startRowIndex][j];
                _array[startRowIndex][j] = _array[endRowIndex][j];
                _array[endRowIndex][j] = temp;
            }

            return this;
        }

        public Matrix<T> ReorderColumns(int startColumnIndex, int endColumnIndex)
        {
            for (int i = 0; i < Rows; i++)
            {
                var temp = _array[i][startColumnIndex];
                _array[i][startColumnIndex] = _array[i][endColumnIndex];
                _array[i][endColumnIndex] = temp;
            }

            return this;
        }
    }
}
