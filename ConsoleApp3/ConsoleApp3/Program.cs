using System;

public class Quaternion
{
    public double W { get; set; } // Скалярна компонента
    public double X { get; set; } // I компонента
    public double Y { get; set; } // J компонента
    public double Z { get; set; } // K компонента

    public Quaternion(double w, double x, double y, double z)
    {
        W = w;
        X = x;
        Y = y;
        Z = z;
    }

    // Перевантажені операції додавання
    public static Quaternion operator +(Quaternion q1, Quaternion q2)
    {
        return new Quaternion(q1.W + q2.W, q1.X + q2.X, q1.Y + q2.Y, q1.Z + q2.Z);
    }

    // Перевантажені операції віднімання
    public static Quaternion operator -(Quaternion q1, Quaternion q2)
    {
        return new Quaternion(q1.W - q2.W, q1.X - q2.X, q1.Y - q2.Y, q1.Z - q2.Z);
    }

    // Перевантажені операції множення
    public static Quaternion operator *(Quaternion q1, Quaternion q2)
    {
        double w = q1.W * q2.W - q1.X * q2.X - q1.Y * q2.Y - q1.Z * q2.Z;
        double x = q1.W * q2.X + q1.X * q2.W + q1.Y * q2.Z - q1.Z * q2.Y;
        double y = q1.W * q2.Y - q1.X * q2.Z + q1.Y * q2.W + q1.Z * q2.X;
        double z = q1.W * q2.Z + q1.X * q2.Y - q1.Y * q2.X + q1.Z * q2.W;

        return new Quaternion(w, x, y, z);
    }

    // Метод для обчислення норми кватерніона
    public double Norm()
    {
        return Math.Sqrt(W * W + X * X + Y * Y + Z * Z);
    }

    // Метод для обчислення спряженого кватерніона
    public Quaternion Conjugate()
    {
        return new Quaternion(W, -X, -Y, -Z);
    }

    // Метод для обчислення інверсного кватерніона
    public Quaternion Inverse()
    {
        double normSquared = W * W + X * X + Y * Y + Z * Z;
        if (normSquared != 0)
        {
            double inverseNorm = 1.0 / normSquared;
            return new Quaternion(W * inverseNorm, -X * inverseNorm, -Y * inverseNorm, -Z * inverseNorm);
        }
        else
        {
            throw new InvalidOperationException("Cannot invert a quaternion with zero norm.");
        }
    }

    // Перевантажені операції порівняння
    public static bool operator ==(Quaternion q1, Quaternion q2)
    {
        return q1.W == q2.W && q1.X == q2.X && q1.Y == q2.Y && q1.Z == q2.Z;
    }

    public static bool operator !=(Quaternion q1, Quaternion q2)
    {
        return !(q1 == q2);
    }

    // Метод для конвертації кватерніона в матрицю обертання
    public double[,] ToRotationMatrix()
    {
        double[,] matrix = new double[3, 3];

        matrix[0, 0] = 1 - 2 * (Y * Y + Z * Z);
        matrix[0, 1] = 2 * (X * Y - W * Z);
        matrix[0, 2] = 2 * (X * Z + W * Y);

        matrix[1, 0] = 2 * (X * Y + W * Z);
        matrix[1, 1] = 1 - 2 * (X * X + Z * Z);
        matrix[1, 2] = 2 * (Y * Z - W * X);

        matrix[2, 0] = 2 * (X * Z - W * Y);
        matrix[2, 1] = 2 * (Y * Z + W * X);
        matrix[2, 2] = 1 - 2 * (X * X + Y * Y);

        return matrix;
    }

    // Метод для конвертації матриці обертання в кватерніон
    public static Quaternion FromRotationMatrix(double[,] matrix)
    {
        double trace = matrix[0, 0] + matrix[1, 1] + matrix[2, 2];

        if (trace > 0)
        {
            double s = 0.5 / Math.Sqrt(trace + 1);
            double w = 0.25 / s;
            double x = (matrix[2, 1] - matrix[1, 2]) * s;
            double y = (matrix[0, 2] - matrix[2, 0]) * s;
            double z = (matrix[1, 0] - matrix[0, 1]) * s;

            return new Quaternion(w, x, y, z);
        }
        else if (matrix[0, 0] > matrix[1, 1] && matrix[0, 0] > matrix[2, 2])
        {
            double s = 2.0 * Math.Sqrt(1 + matrix[0, 0] - matrix[1, 1] - matrix[2, 2]);
            double w = (matrix[2, 1] - matrix[1, 2]) / s;
            double x = 0.25 * s;
            double y = (matrix[0, 1] + matrix[1, 0]) / s;
            double z = (matrix[0, 2] + matrix[2, 0]) / s;

            return new Quaternion(w, x, y, z);
        }
        else if (matrix[1, 1] > matrix[2, 2])
        {
            double s = 2.0 * Math.Sqrt(1 + matrix[1, 1] - matrix[0, 0] - matrix[2, 2]);
            double w = (matrix[0, 2] - matrix[2, 0]) / s;
            double x = (matrix[0, 1] + matrix[1, 0]) / s;
            double y = 0.25 * s;
            double z = (matrix[1, 2] + matrix[2, 1]) / s;

            return new Quaternion(w, x, y, z);
        }
        else
        {
            double s = 2.0 * Math.Sqrt(1 + matrix[2, 2] - matrix[0, 0] - matrix[1, 1]);
            double w = (matrix[1, 0] - matrix[0, 1]) / s;
            double x = (matrix[0, 2] + matrix[2, 0]) / s;
            double y = (matrix[1, 2] + matrix[2, 1]) / s;
            double z = 0.25 * s;

            return new Quaternion(w, x, y, z);
        }
    }

    // Перевизначений метод для порівняння об'єктів
    public override bool Equals(object obj)
    {
        if (obj is Quaternion otherQuaternion)
        {
            return this == otherQuaternion;
        }
        return false;
    }

    // Перевизначений метод для отримання хеш-коду
    public override int GetHashCode()
    {
        return W.GetHashCode() ^ X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
    }
}

class Program
{
    static void Main()
    {
        // Приклад використання класу Quaternion
        Quaternion q1 = new Quaternion(1, 2, 3, 4);
        Quaternion q2 = new Quaternion(5, 6, 7, 8);

        Quaternion sum = q1 + q2;
        Quaternion difference = q1 - q2;
        Quaternion product = q1 * q2;

        Console.WriteLine($"q1 + q2: {sum.W}, {sum.X}, {sum.Y}, {sum.Z}");
        Console.WriteLine($"q1 - q2: {difference.W}, {difference.X}, {difference.Y}, {difference.Z}");
        Console.WriteLine($"q1 * q2: {product.W}, {product.X}, {product.Y}, {product.Z}");

        Console.WriteLine($"Norm of q1: {q1.Norm()}");
        Console.WriteLine($"Conjugate of q1: {q1.Conjugate().W}, {q1.Conjugate().X}, {q1.Conjugate().Y}, {q1.Conjugate().Z}");
        Console.WriteLine($"Inverse of q1: {q1.Inverse().W}, {q1.Inverse().X}, {q1.Inverse().Y}, {q1.Inverse().Z}");

        // Конвертація кватерніона в матрицю обертання
        double[,] rotationMatrix = q1.ToRotationMatrix();
        Console.WriteLine("Rotation Matrix:");
        PrintMatrix(rotationMatrix);

        // Конвертація матриці обертання в кватерніон
        Quaternion fromMatrix = Quaternion.FromRotationMatrix(rotationMatrix);
        Console.WriteLine($"Quaternion from Rotation Matrix: {fromMatrix.W}, {fromMatrix.X}, {fromMatrix.Y}, {fromMatrix.Z}");
    }

    static void PrintMatrix(double[,] matrix)
    {
        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Console.Write($"{matrix[i, j]} ");
            }
            Console.WriteLine();
        }
    }
}
