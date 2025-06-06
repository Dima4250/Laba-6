using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba_6
{
    public static class MatrixExtensions
    {
        // Транспонирование матрицы
        public static SquareMatrix Transpose(this SquareMatrix matrix)
        {
            SquareMatrix result = new SquareMatrix(matrix.size, true);

            for (int i = 0; i < matrix.size; i++)
            {
                for (int j = 0; j < matrix.size; j++)
                {
                    result[j, i] = matrix[i, j];
                }
            }

            return result;
        }

        // След матрицы (сумма диагональных элементов)
        public static int Trace(this SquareMatrix matrix)
        {
            int trace = 0;
            for (int i = 0; i < matrix.size; i++)
            {
                trace += matrix[i, i];
            }
            return trace;
        }
    }

    // Делегат для операций с матрицей
    public delegate void MatrixOperationDelegate(SquareMatrix matrix);

    public static class MatrixOperations
    {
        public static readonly MatrixOperationDelegate Diagonalize = delegate (SquareMatrix matrix)
        {
            for (int i = 0; i < matrix.size; i++)
            {
                for (int j = 0; j < matrix.size; j++)
                {
                    if (i != j) matrix[i, j] = 0;
                }
            }
        };
    }

    public interface IMatrixHandler
    {
        IMatrixHandler SetNext(IMatrixHandler handler);
        void Handle(SquareMatrix matrix, int choice);
    }

    public abstract class AbstractMatrixHandler : IMatrixHandler
    {
        private IMatrixHandler _nextHandler;

        public IMatrixHandler SetNext(IMatrixHandler handler)
        {
            _nextHandler = handler;
            return handler;
        }

        public virtual void Handle(SquareMatrix matrix, int choice)
        {
            _nextHandler?.Handle(matrix, choice);
        }
    }

    public class TransposeHandler : AbstractMatrixHandler
    {
        public override void Handle(SquareMatrix matrix, int choice)
        {
            if (choice == 1)
            {
                Console.WriteLine("\nТранспонированная матрица:");
                matrix.Transpose().PrintSquareMatrix();
            }
            else
            {
                base.Handle(matrix, choice);
            }
        }
    }

    public class TraceHandler : AbstractMatrixHandler
    {
        public override void Handle(SquareMatrix matrix, int choice)
        {
            if (choice == 2)
            {
                Console.WriteLine($"\nСлед матрицы: {matrix.Trace()}");
            }
            else
            {
                base.Handle(matrix, choice);
            }
        }
    }

    public class DiagonalizeHandler : AbstractMatrixHandler
    {
        public override void Handle(SquareMatrix matrix, int choice)
        {
            if (choice == 3)
            {
                MatrixOperations.Diagonalize(matrix);
                Console.WriteLine("\nМатрица приведена к диагональному виду:");
                matrix.PrintSquareMatrix();
            }
            else
            {
                base.Handle(matrix, choice);
            }
        }
    }

}
