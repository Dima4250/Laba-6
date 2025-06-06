using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Laba_6.MatrixSizeException;

namespace Laba_6
{
    public class SquareMatrix
    {
        private int[,] matrix;
        public int size { get; set; }


        //Генерация матрицы случайным образом
        public SquareMatrix(int size, bool initZero = false)
        {
            this.size = size;
            matrix = new int[size, size];

            Random random = new Random();
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    matrix[i, j] = random.Next(0, 9);
                }
            }
        }

        //Индекцсикация
        public int this[int row, int col]
        {
            get
            {
                if (row < 0 || row >= size || col < 0 || col >= size)
                    throw new IndexOutOfRangeException("Индекс выходит за границы матрицы");
                return matrix[row, col];
            }
            set
            {
                if (row < 0 || row >= size || col < 0 || col >= size)
                    throw new IndexOutOfRangeException("Индекс выходит за границы матрицы");
                matrix[row, col] = value;
            }
        }

        //Вывод матрицы
        public void PrintSquareMatrix()
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Console.Write(matrix[i, j] + "\t");
                }
                Console.WriteLine();
            }

        }

        //Перегрузка сложения матриц
        public static SquareMatrix operator + (SquareMatrix a, SquareMatrix b)
        {
            if (a.size != b.size)
            {
                throw new MatrixSizeException("Матрицы должны быть одного размера!");
            }
            SquareMatrix result = new SquareMatrix(a.size);
            for (int i = 0; i < a.size; i++)
            {
                for (int j = 0; j < a.size; j++)
                {
                    result.matrix[i, j] = a.matrix[i, j] + b.matrix[i, j];
                }
            }
            return result;
        }

        //Перегрузка умножения матриц
        public static SquareMatrix operator * (SquareMatrix a, SquareMatrix b)
        {
            if (a.size != b.size)
            {
                throw new MatrixSizeException("Матрицы должны быть одного размера!");
            }
            SquareMatrix result = new SquareMatrix(a.size,true);
            for (int i = 0; i < a.size; i++)
            {
                for (int j = 0; j < a.size; j++)
                {
                    int sum = 0;
                    for (int k = 0; k < a.size; k++)
                    {
                        sum += a.matrix[i, k] * b.matrix[k, j];
                    }
                    result.matrix[i,j] = sum;
                }
            }
            return result;
        }

        //Перегрузка булевских операций
        public static bool operator > (SquareMatrix a, SquareMatrix b)
        {
            return a.SumOfElements() > b.SumOfElements();
        }

        public static bool operator < (SquareMatrix a, SquareMatrix b)
        {
            return a.SumOfElements() < b.SumOfElements();
        }

        public static bool operator >= (SquareMatrix a, SquareMatrix b)
        {
            return a.SumOfElements() >= b.SumOfElements();
        }

        public static bool operator <= (SquareMatrix a, SquareMatrix b)
        {
            return a.SumOfElements() <= b.SumOfElements();
        }

        public static bool operator == (SquareMatrix a, SquareMatrix b)
        {
            return a.Equals(b);
        }

        public static bool operator != (SquareMatrix a, SquareMatrix b)
        {
            return !a.Equals(b);
        }

        //Нахождение определителя матрицы по т-ме Лапласа, Cofactor - дополнение,
        // кт. состоит из произведения знака (-1)^1+i и минора(опр-ля подматрицы).
        public int Determinant()
        {
            if (size == 1) return matrix[0, 0];
            if (size == 2) return matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0
                ];

            int det = 0;
            for (int j = 0; j < size; j++)
            {
                det += matrix[0, j] * Cofactor(0, j);
            }
            return det;
        }

        //Вычисление дополнения
        private int Cofactor(int row, int col)
        {
            int sign = (row + col) % 2 == 0 ? 1 : -1;
            return sign * GetMinor(row, col).Determinant();
        }

        //Нахождение минора
        private SquareMatrix GetMinor(int row, int col)
        {
            SquareMatrix minor = new SquareMatrix(size - 1);
            int minorRow = 0;
            for (int i = 0; i < size; i++)
            {
                if (i == row) continue;
                int minorCol = 0;
                for (int j = 0; j < size; j++)
                {
                    if (j == col) continue;
                    minor[minorRow, minorCol] = matrix[i, j];
                    minorCol++;
                }
                minorRow++;
            }
            return minor;
        }

        public static explicit operator int(SquareMatrix a)
        {
            return a.Determinant();
        }

        //Обратная матрица
        public SquareMatrix Inverse()
        {
            int det = Determinant();
            if (Math.Abs(det) < double.Epsilon)
                throw new MatrixNotInvertibleException();

            SquareMatrix inverse = new SquareMatrix(size);
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    inverse[j, i] = (int)(Cofactor(i, j) / det);
                }
            }
            return inverse;

        }

        //Проверка на нулевую матрицу
        public bool ZeroMatrix()
        {
            for(int i = 0; i < size; i++)
                for(int j = 0; j < size; j++)
                {
                    if (matrix[i, j] != 0)
                    {
                        return false;
                    }
                }
            return true;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    sb.Append(matrix[i, j].ToString("F2")).Append("\t");
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }

        public int CompareTo(SquareMatrix other)
        {
            if (other == null) return 1;

            int thisSum = this.SumOfElements();
            int otherSum = other.SumOfElements();

            return thisSum.CompareTo(otherSum);
        }

        private int SumOfElements()
        {
            int sum = 0;
            for (int i = 0; i < size; ++i)
            {
                for (int j = 0; j < size; ++j)
                {
                    sum += matrix[i, j];
                }
            }
            return sum;
        }

        public override bool Equals(object obj)
        {
            if (obj is not SquareMatrix other || size != other.size)
                return false;

            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    if (matrix[i, j] != other.matrix[i, j])
                        return false;

            return true;
        }

        public override int GetHashCode()
        {
            return matrix.GetHashCode();
        }

        public SquareMatrix Clone()
        {
            SquareMatrix clone = new SquareMatrix(this.size);
            for (int i = 0; i < size; ++i)
            {
                for (int j = 0; j < size; ++j)
                {
                    clone.matrix[i, j] = this.matrix[i, j];
                }
            }
            return clone;
        }
    }
    
}

       


  

