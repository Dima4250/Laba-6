using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba_6
{
    public class MatrixSizeException : Exception
    {
        public MatrixSizeException() : base("Размеры матриц не совпадают.") { }

        public MatrixSizeException(string message) : base(message) { }

        public MatrixSizeException(string message, Exception innerException)
        : base(message, innerException) { }

        public class MatrixNotInvertibleException : Exception
        {
            public MatrixNotInvertibleException() : base("Матрица не обратима (определитель равен 0)") { }
        }
    }
    
}