/*
 *
 * API geométrica plana.
 *
 * Contém diversas classes que representam lugares geométricos como pontos (vetores), retângulos e polígonos.
 *
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geometry2D
{
    /// <summary>
    /// Um lugar geométrico bidimensional
    /// </summary>
    public interface IGeometry2D
    {
    }

    /// <summary>
    /// União de elementos geométricos disjuntos
    /// </summary>
    public readonly struct Union : IGeometry2D
    {
        public static readonly Union EMPTY_SET = new Union();

        private readonly IGeometry2D[] parts;

        /// <summary>
        /// Cria uma união a partir das partes
        /// </summary>
        /// <param name="parts">Partes</param>
        public Union(params IGeometry2D[] parts) => this.parts = parts;

        /// <summary>
        /// Quantidade de partes disjuntas contidas na união
        /// </summary>
        public int Count => parts.Length;

        /// <summary>
        /// Retorna true se a união for vazia, false caso contrário
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty() => parts.Length == 0;

        /// <summary>
        /// Igualdade entre uniões
        /// </summary>
        /// <param name="set1">Primeira união</param>
        /// <param name="set2">Segunda união</param>
        /// <returns>true se as uniões forem iguais, false caso contrário</returns>
        public static bool operator ==(Union set1, Union set2)
        {
            var list = set2.parts.ToList<IGeometry2D>();

            for (int i = 0; i < set1.parts.Length; i++)
            {
                IGeometry2D g1 = set1.parts[i];
                bool found = false;

                for (int j = 0; j < list.Count; j++)
                {
                    IGeometry2D g2 = list[j];

                    if (g1 == g2)
                    {
                        found = true;
                        list.RemoveAt(j);
                        break;
                    }
                }

                if (!found)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Inequalidade entre uniões
        /// </summary>
        /// <param name="set1">Primeira união</param>
        /// <param name="set2">Segunda união</param>
        /// <returns>true se as uniões forem diferentes, false caso contrário</returns>
        public static bool operator !=(Union set1, Union set2) => !(set1 == set2);
    }

    /// <summary>
    /// Vetor bidimensional
    /// </summary>
    public readonly struct Vector2D : IGeometry2D
    {
        /// <summary>
        /// Vetor nulo
        /// </summary>
        public static readonly Vector2D NULL_VECTOR = new Vector2D(); // Vetor nulo
        /// <summary>
        /// Vetor leste
        /// </summary>
        public static readonly Vector2D LEFT_VECTOR = new Vector2D(-1, 0);
        /// <summary>
        /// Vetor norte
        /// </summary>
        public static readonly Vector2D UP_VECTOR = new Vector2D(0, -1);
        /// <summary>
        /// Vetor oeste
        /// </summary>
        public static readonly Vector2D RIGHT_VECTOR = new Vector2D(1, 0);
        /// <summary>
        /// Vetor sul
        /// </summary>
        public static readonly Vector2D DOWN_VECTOR = new Vector2D(0, 1);

        /// <summary>
        /// Cria um vetor a partir de duas coordenadas
        /// </summary>
        /// <param name="x">Coordenada x</param>
        /// <param name="y">Coordenada y</param>
        public Vector2D(float x, float y)
        {
            X = x;
            this.Y = y;
        }

        /// <summary>
        /// Cria um vetor a partir de um ponto do tipo Point
        /// </summary>
        /// <param name="p">Ponto</param>
        public Vector2D(Point p)
        {
            X = p.X;
            Y = p.Y;
        }

        /// <summary>
        /// Cria um vetor a partir de um ponto do tipo PointF
        /// </summary>
        /// <param name="p">Ponto</param>
        public Vector2D(PointF p)
        {
            X = p.X;
            Y = p.Y;
        }

        /// <summary>
        /// Cria um vetor a partir de uma estrutura do tipo Size
        /// </summary>
        /// <param name="size">Estrutura do tipo Size</param>
        public Vector2D(Size size)
        {
            X = size.Width;
            Y = size.Height;
        }

        /// <summary>
        /// Cria um vetor a partir de uma estrutura do tipo SizeF
        /// </summary>
        /// <param name="size">Estrutura do tipo SizeF</param>
        public Vector2D(SizeF size)
        {
            X = size.Width;
            Y = size.Height;
        }

        public override int GetHashCode() => 31 * (int) X + (int) Y;

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (!(obj is Vector2D))
                return false;

            var other = (Vector2D) obj;
            return other.X == X && other.Y == Y;
        }

        public override string ToString() => X + "," + Y;

        /// <summary>
        /// Converte o vetor para um ponto do tipo Point
        /// </summary>
        /// <returns>Ponto do tipo Point</returns>
        public Point ToPoint() => new Point((int) X, (int) Y);

        /// <summary>
        /// Converte o vetor para um ponto do tipo PointF
        /// </summary>
        /// <returns>Ponto do tipo PointF</returns>
        public PointF ToPointF() => new PointF(X, Y);

        /// <summary>
        /// Converte o vetor para uma estrutura do tipo Size
        /// </summary>
        /// <returns>Vetor convertido em uma estrutura do tipo Size</returns>
        public Size ToSize() => new Size((int) X, (int) Y);

        /// <summary>
        /// Converte o vetor para uma estrutura do tipo SizeF
        /// </summary>
        /// <returns>Vetor convertido em uma estrutura do tipo SizeF</returns>
        public SizeF ToSizeF() => new SizeF(X, Y);

        /// <summary>
        /// Normaliza o vetor
        /// </summary>
        /// <returns>O vetor normalizado</returns>
        public Vector2D Versor()
        {
            if (IsNull)
                return NULL_VECTOR;

            float abs = Length;
            return new Vector2D(X / abs, Y / abs);
        }

        /// <summary>
        /// Rotaciona o vetor ao redor da origem
        /// </summary>
        /// <param name="angle">Angulo de rotação em radianos</param>
        /// <returns>O vetor rotacionado</returns>
        public Vector2D Rotate(float angle)
        {
            float cos = (float) Math.Cos(angle);
            float sin = (float) Math.Sin(angle);

            return new Vector2D(X * cos - Y * sin, X * sin + Y * cos);
        }

        /// <summary>
        /// Rotaciona o vetor ao redor de um outro vetor
        /// </summary>
        /// <param name="center">Centro de rotação</param>
        /// <param name="angle">Angulo de rotação em radianos</param>
        /// <returns>O vetor rotacionado</returns>
        public Vector2D Rotate(Vector2D center, float angle) => (this - center).Rotate(angle) + center;

        /// <summary>
        /// Rotaciona um vetor em 90 graus ao redor da origem
        /// </summary>
        /// <returns>O vetor rotacionado</returns>
        public Vector2D Rotate90() => new Vector2D(-Y, X);

        /// <summary>
        /// Rotaciona um vetor em 90 graus ao redor de outro vetor
        /// </summary>
        /// <param name="center">Centro de rotação</param>
        /// <returns>O vetor rotacionado</returns>
        public Vector2D Rotate90(Vector2D center) => (this - center).Rotate90() + center;

        /// <summary>
        /// Rotaciona um vetor em 180 graus ao redor da origem
        /// </summary>
        /// <returns>O vetor rotacionado</returns>
        public Vector2D Rotate180() => new Vector2D(-X, -Y);

        /// <summary>
        /// Rotaciona um vetor em 180 graus ao redor de outro vetor
        /// </summary>
        /// <param name="center">Centro de rotação</param>
        /// <returns>O vetor rotacionado</returns>
        public Vector2D Rotate180(Vector2D center) => (this - center).Rotate180() + center;

        /// <summary>
        /// Rotaciona um vetor em 270 graus ao redor da origem
        /// </summary>
        /// <returns>O vetor rotacionado</returns>
        public Vector2D Rotate270() => new Vector2D(Y, -X);

        /// <summary>
        /// Rotaciona um vetor em 270 graus ao redor de outro vetor
        /// </summary>
        /// <param name="center">Centro de rotação</param>
        /// <returns>O vetor rotacionado</returns>
        public Vector2D Rotate270(Vector2D center) => (this - center).Rotate270() + center;

        /// <summary>
        /// Coordenada x do vetor
        /// </summary>
        public float X
        {
            get;
        }

        /// <summary>
        /// Coordenada y do vetor
        /// </summary>
        public float Y
        {
            get;
        }

        /// <summary>
        /// Módulo/Norma/Comprimento do vetor
        /// </summary>
        public float Length => (float) Math.Sqrt(X * X + Y * Y);

        /// <summary>
        /// Distâcia entre vetores
        /// </summary>
        /// <param name="vec">Vetor no qual será medido a sua distância até este vetor</param>
        /// <returns>A distância entre este vetor e o vetor dado</returns>
        public float DistanceTo(Vector2D vec)
        {
            float dx = X - vec.X;
            float dy = Y - vec.Y;
            return (float) Math.Sqrt(dx * dx + dy * dy);
        }

        /// <summary>
        /// Retorna true se este vetor for nulo, false caso contrário
        /// </summary>
        public bool IsNull => X == 0 && Y == 0;

        /// <summary>
        /// Adição de vetores
        /// </summary>
        /// <param name="vec1">Primeiro vetor</param>
        /// <param name="vec2">Segundo vetor</param>
        /// <returns>Soma entre os dois vetores</returns>
        public static Vector2D operator +(Vector2D vec1, Vector2D vec2) => new Vector2D(vec1.X + vec2.X, vec1.Y + vec2.Y);

        /// <summary>
        /// Subtração de vetores
        /// </summary>
        /// <param name="vec1">Primeiro vetor</param>
        /// <param name="vec2">Segundo vetor</param>
        /// <returns>Diferença entre os dois vetores</returns>
        public static Vector2D operator -(Vector2D vec1, Vector2D vec2) => new Vector2D(vec1.X - vec2.X, vec1.Y - vec2.Y);

        /// <summary>
        /// Inverte o sentido do vetor
        /// </summary>
        /// <param name="vec">Vetor</param>
        /// <returns>O oposto do vetor</returns>
        public static Vector2D operator -(Vector2D vec) => new Vector2D(-vec.X, -vec.Y);

        /// <summary>
        /// Produto de um vetor por um escalar
        /// </summary>
        /// <param name="alpha">Escalar</param>
        /// <param name="vec">Vetor</param>
        /// <returns>O vetor escalado por alpha</returns>
        public static Vector2D operator *(float alpha, Vector2D vec) => new Vector2D(alpha * vec.X, alpha * vec.Y);

        /// <summary>
        /// Produto de um vetor por um escalar
        /// </summary>
        /// <param name="vec">Vetor</param>
        /// <param name="alpha">Escalar</param>
        /// <returns>O vetor escalado por alpha</returns>
        public static Vector2D operator *(Vector2D vec, float alpha) => new Vector2D(alpha * vec.X, alpha * vec.Y);

        /// <summary>
        /// Divisão de vetor por um escalar, o mesmo que multiplicar o vetor pelo inverso do escalar
        /// </summary>
        /// <param name="vec">Vetor</param>
        /// <param name="alpha">Escalar</param>
        /// <returns>O vetor dividido pelo escalar alpha</returns>
        public static Vector2D operator /(Vector2D vec, float alpha) => new Vector2D(vec.X / alpha, vec.Y / alpha);

        /// <summary>
        /// Produto escalar/interno/ponto entre dois vetores
        /// </summary>
        /// <param name="vec1">Primeiro vetor</param>
        /// <param name="vec2">Segundo vetor</param>
        /// <returns>Produto escalar entre os dois vetores</returns>
        public static float operator *(Vector2D vec1, Vector2D vec2) => vec1.X * vec2.X + vec1.Y * vec2.Y;

        /// <summary>
        /// Igualdade entre vetores
        /// </summary>
        /// <param name="vec1">Primeiro vetor</param>
        /// <param name="vec2">Segundo vetor</param>
        /// <returns>true se os vetores forem iguais, false caso contrário</returns>
        public static bool operator ==(Vector2D vec1, Vector2D vec2) => vec1.X == vec2.X && vec1.Y == vec2.Y;

        /// <summary>
        /// Inequalidade entre vetores
        /// </summary>
        /// <param name="vec1">Primeiro vetor</param>
        /// <param name="vec2">Segundo vetor</param>
        /// <returns>true se os vetores forem diferentes, false caso contrário</returns>
        public static bool operator !=(Vector2D vec1, Vector2D vec2) => vec1.X != vec2.X || vec1.Y != vec2.Y;
    }

    /// <summary>
    /// Segmento de reta
    /// </summary>
    public readonly struct LineSegment : IGeometry2D
    {
        /// <summary>
        /// Cria um segmento de reta a partir de dois pontos
        /// </summary>
        /// <param name="start">Ponto inicial do segmento</param>
        /// <param name="end">Ponto final do segmento</param>
        public LineSegment(Vector2D start, Vector2D end)
        {
            this.Start = start;
            this.End = end;
        }

        /// <summary>
        /// Ponto inicial do segmento
        /// </summary>
        public Vector2D Start
        {
            get;
        }

        /// <summary>
        /// Ponto final do segmento
        /// </summary>
        public Vector2D End
        {
            get;
        }

        /// <summary>
        /// Comprimento do segmento
        /// </summary>
        public float Length => (End - Start).Length;

        /// <summary>
        /// Inverte o sentido do segmento trocando seu ponto inicial com seu ponto final
        /// </summary>
        /// <returns>O segmento de reta invertido</returns>
        public LineSegment Negate() => new LineSegment(End, Start);

        /// <summary>
        /// Rotaciona um segmento de reta ao redor de um ponto
        /// </summary>
        /// <param name="origin">Centro de rotação</param>
        /// <param name="angle">Algumo de rotação em radianos</param>
        /// <returns>Segmento de reta rotacionado</returns>
        public LineSegment Rotate(Vector2D origin, float angle)
        {
            Vector2D u = Start.Rotate(origin, angle);
            Vector2D v = End.Rotate(origin, angle);
            return new LineSegment(u, v);
        }

        /// <summary>
        /// Rotaciona um segmento de reta em 90 graus ao redor de um ponto
        /// </summary>
        /// <param name="origin">Centro de rotação</param>
        /// <returns>Segmento de reta rotacionado</returns>
        public LineSegment Rotate90(Vector2D origin)
        {
            Vector2D u = Start.Rotate90(origin);
            Vector2D v = End.Rotate90(origin);
            return new LineSegment(u, v);
        }

        /// <summary>
        /// Rotaciona um segmento de reta em 180 graus ao redor de um ponto
        /// </summary>
        /// <param name="origin">Centro de rotação</param>
        /// <returns>Segmento de reta rotacionado</returns>
        public LineSegment Rotate180(Vector2D origin)
        {
            Vector2D u = Start.Rotate180(origin);
            Vector2D v = End.Rotate180(origin);
            return new LineSegment(u, v);
        }

        /// <summary>
        /// Rotaciona um segmento de reta em 270 graus ao redor de um ponto
        /// </summary>
        /// <param name="origin">Centro de rotação</param>
        /// <returns>Segmento de reta rotacionado</returns>
        public LineSegment Rotate270(Vector2D origin)
        {
            Vector2D u = Start.Rotate270(origin);
            Vector2D v = End.Rotate270(origin);
            return new LineSegment(u, v);
        }

        /// <summary>
        /// Compara a posição de um vetor em relação ao segmento de reta
        /// </summary>
        /// <param name="v">Vetor a ser testado</param>
        /// <returns>1 se o vetor está a esquerda do segmento, -1 se estiver a direta, 0 se for colinear ao segmento</returns>
        public int Compare(Vector2D v)
        {
            float f = (v.Y - Start.Y) * (End.X - Start.X) - (v.X - Start.X) * (End.Y - Start.Y);

            return f > 0 ? 1 : f < 0 ? -1 : 0;
        }

        /// <summary>
        /// Verifica se o segmento contém o vetor dado
        /// </summary>
        /// <param name="v">Vetor a ser testado</param>
        /// <returns>true se o segmento contém o vetor, false caso contrário</returns>
        public bool Contains(Vector2D v)
        {
            if (Compare(v) != 0)
                return false;

            float mX = Math.Min(Start.X, End.X);
            float MX = Math.Max(Start.X, End.X);
            float mY = Math.Min(Start.Y, End.Y);
            float MY = Math.Max(Start.Y, End.Y);

            return mX <= v.X && v.X <= MX && mY <= v.Y && v.Y <= MY;
        }

        /// <summary>
        /// Verifica se dois segmentos de reta são paralelos
        /// </summary>
        /// <param name="s">Segmento a ser testado</param>
        /// <returns>true se forem paralelos, false caso contrário</returns>
        public bool IsParallel(LineSegment s)
        {
            float A1 = End.Y - Start.Y;
            float B1 = End.X - Start.X;
            float A2 = s.End.Y - s.Start.Y;
            float B2 = s.End.X - s.Start.X;

            return A1 * B2 == A2 * B1;
        }

        /// <summary>
        /// Obtém a intersecção entre dois segmentos de reta
        /// </summary>
        /// <param name="s">Segmento de reta a ser testado</param>
        /// <returns>A intersecção entre os dois segmentos caso ela exista, ou retorna conjunto vazio caso contrário</returns>
        public IGeometry2D Intersection(LineSegment s)
        {
            if (s == this)
                return this;

            float A1 = End.Y - Start.Y;
            float B1 = End.X - Start.X;
            float A2 = s.End.Y - s.Start.Y;
            float B2 = s.End.X - s.Start.X;

            float D = A1 * B2 - A2 * B1;

            if (D == 0)
                return Union.EMPTY_SET;

            float C1 = Start.X * End.Y - End.X * Start.Y;
            float C2 = s.Start.X * s.End.Y - s.End.X * s.Start.Y;

            float x = (A2 * C2 - A1 * C1) / D;
            float y = (B1 * C1 - B2 * C2) / D;
            var v = new Vector2D(x, y);

            return !Contains(v) ? Union.EMPTY_SET : (IGeometry2D) v;
        }

        /// <summary>
        /// Verifica se o vetor v está a direita do seguimento de reta s
        /// </summary>
        /// <param name="v">Vetor</param>
        /// <param name="s">Seguimento de reta</param>
        /// <returns>Resultado da comparação</returns>
        public static bool operator <(Vector2D v, LineSegment s) => s.Compare(v) == -1;

        /// <summary>
        /// Verifica se o vetor v está a direita ou é colinear ao seguimento de reta s
        /// </summary>
        /// <param name="v">Vetor</param>
        /// <param name="s">Seguimento de reta</param>
        /// <returns>Resultado da comparação</returns>
        public static bool operator <=(Vector2D v, LineSegment s) => s.Compare(v) <= 0;

        /// <summary>
        /// Verifica se o vetor v está a esquerda do seguimento de reta s
        /// </summary>
        /// <param name="v">Vetor</param>
        /// <param name="s">Seguimento de reta</param>
        /// <returns>Resultado da comparação</returns>
        public static bool operator >(Vector2D v, LineSegment s) => s.Compare(v) == 1;

        /// <summary>
        /// Verifica se o vetor v está a esquerda ou é colinear ao seguimento de reta s
        /// </summary>
        /// <param name="v">Vetor</param>
        /// <param name="s">Seguimento de reta</param>
        /// <returns>Resultado da comparação</returns>
        public static bool operator >=(Vector2D v, LineSegment s) => s.Compare(v) >= 0;

        /// <summary>
        /// O mesmo que v > s
        /// </summary>
        /// <param name="s">Seguimento de reta</param>
        /// <param name="v">Vetor</param>
        /// <returns>v > s</returns>
        public static bool operator <(LineSegment s, Vector2D v) => v > s;

        /// <summary>
        /// O mesmo que v >= s
        /// </summary>
        /// <param name="s">Seguimento de reta</param>
        /// <param name="v">Vetor</param>
        /// <returns>v >= s</returns>
        public static bool operator <=(LineSegment s, Vector2D v) => v >= s;

        /// <summary>
        /// O mesmo que v < s
        /// </summary>
        /// <param name="s">Seguimento de reta</param>
        /// <param name="v">Vetor</param>
        /// <returns>v < s</returns>
        public static bool operator >(LineSegment s, Vector2D v) => v < s;

        /// <summary>
        /// O mesmo que v <= s
        /// </summary>
        /// <param name="s">Seguimento de reta</param>
        /// <param name="v">Vetor</param>
        /// <returns>v <= s</returns>
        public static bool operator >=(LineSegment s, Vector2D v) => v <= s;

        /// <summary>
        /// Compara se dois seguimentos de reta são iguais
        /// </summary>
        /// <param name="s1">Primeiro seguimento de reta</param>
        /// <param name="s2">Seguindo seguimento de reta</param>
        /// <returns>true se forem iguais, false caso contrário</returns>
        public static bool operator ==(LineSegment s1, LineSegment s2) => s1.Start == s2.Start && s1.Length == s2.Length;

        /// <summary>
        /// Compara se dois seguimentos de reta são diferentes
        /// </summary>
        /// <param name="s1">Primeiro seguimento de reta</param>
        /// <param name="s2">Seguindo seguimento de reta</param>
        /// <returns>true se forem diferentes, false caso contrário</returns>
        public static bool operator !=(LineSegment s1, LineSegment s2) => s1.Start != s2.Start || s1.Length != s2.Length;
    }

    /// <summary>
    /// Uma matriz quadrada de ordem 2
    /// </summary>
    public readonly struct Matrix2x2
    {
        /// <summary>
        /// Matriz nula
        /// </summary>
        public static readonly Matrix2x2 NULL_MATRIX = new Matrix2x2();
        /// <summary>
        /// Matriz identidade
        /// </summary>
        public static readonly Matrix2x2 IDENTITY = new Matrix2x2(1, 0, 0, 1);

        /// <summary>
        /// Cria uma matriz a partir de um array de valores numéricos
        /// </summary>
        /// <param name="values"></param>
        public Matrix2x2(params float[] values)
        {
            Element00 = values[0];
            Element01 = values[1];
            Element10 = values[2];
            Element11 = values[3];
        }

        /// <summary>
        /// Cria uma matriz a partir de dois vetores
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        public Matrix2x2(Vector2D v1, Vector2D v2)
        {
            Element00 = v1.X;
            Element01 = v1.Y;
            Element10 = v2.X;
            Element11 = v2.Y;
        }

        public float Element00
        {
            get;
        }

        public float Element01
        {
            get;
        }

        public float Element10
        {
            get;
        }

        public float Element11
        {
            get;
        }

        /// <summary>
        /// Calcula o determinante da uma matriz
        /// </summary>
        /// <returns>Determinante</returns>
        public float Determinant() => Element00 * Element11 - Element10 * Element01;

        /// <summary>
        /// Transpõe a matriz
        /// </summary>
        /// <returns>Transposta da matriz</returns>
        public Matrix2x2 Transpose() => new Matrix2x2(Element00, Element10, Element01, Element11);

        /// <summary>
        /// Inverte a matriz
        /// </summary>
        /// <returns>Inversa da matriz</returns>
        public Matrix2x2 Inverse() => new Matrix2x2(Element11, -Element01, -Element10, Element00) / Determinant();

        /// <summary>
        /// Soma entre duas matrizes
        /// </summary>
        /// <param name="m1">Primeira matriz</param>
        /// <param name="m2">Segunda matriz</param>
        /// <returns>Soma</returns>
        public static Matrix2x2 operator +(Matrix2x2 m1, Matrix2x2 m2) => new Matrix2x2(m1.Element00 + m2.Element00, m1.Element01 + m2.Element01, m1.Element10 + m2.Element10, m1.Element11 + m2.Element11);

        /// <summary>
        /// Diferença/Subtração entre duas matrizes
        /// </summary>
        /// <param name="m1">Primeira matriz</param>
        /// <param name="m2">Segunda matriz</param>
        /// <returns>Diferença</returns>
        public static Matrix2x2 operator -(Matrix2x2 m1, Matrix2x2 m2) => new Matrix2x2(m1.Element00 - m2.Element00, m1.Element01 - m2.Element01, m1.Element10 - m2.Element10, m1.Element11 - m2.Element11);

        /// <summary>
        /// Oposto aditivo de uma matriz
        /// </summary>
        /// <param name="m">Matriz</param>
        /// <returns>Oposto</returns>
        public static Matrix2x2 operator -(Matrix2x2 m) => new Matrix2x2(-m.Element00, -m.Element01, -m.Element10, -m.Element11);

        /// <summary>
        /// Produto de uma matriz por um escalar
        /// </summary>
        /// <param name="factor">Escalar</param>
        /// <param name="m">Matriz</param>
        /// <returns>Produto</returns>
        public static Matrix2x2 operator *(float factor, Matrix2x2 m) => new Matrix2x2(factor * m.Element00, factor * m.Element01, factor * m.Element10, factor * m.Element11);

        /// <summary>
        /// Produto de uma matriz por um escalar
        /// </summary>
        /// <param name="m">Matriz</param>
        /// <param name="factor">Escalar</param>
        /// <returns>Produto</returns>
        public static Matrix2x2 operator *(Matrix2x2 m, float factor) => new Matrix2x2(m.Element00 * factor, m.Element01 * factor, m.Element10 * factor, m.Element11 * factor);

        /// <summary>
        /// Divisão de uma matriz por um escalar
        /// </summary>
        /// <param name="m">Matriz</param>
        /// <param name="factor">Escalar</param>
        /// <returns>Divisão</returns>
        public static Matrix2x2 operator /(Matrix2x2 m, float factor) => new Matrix2x2(m.Element00 / factor, m.Element01 / factor, m.Element10 / factor, m.Element11 / factor);

        /// <summary>
        /// Produto entre duas matrizes
        /// </summary>
        /// <param name="m1">Primeira matriz</param>
        /// <param name="m2">Segunda matriz</param>
        /// <returns>Produto matricial</returns>
        public static Matrix2x2 operator *(Matrix2x2 m1, Matrix2x2 m2) => new Matrix2x2(m1.Element00 * m2.Element00 + m1.Element01 * m2.Element10, m1.Element00 * m2.Element01 + m1.Element01 * m2.Element11,
                                 m1.Element10 * m2.Element00 + m1.Element11 * m2.Element10, m1.Element10 * m2.Element01 + m1.Element11 * m2.Element11);

        /// <summary>
        /// Calcula a matriz de rotação a partir de um angulo dado
        /// </summary>
        /// <param name="angle">Angulo em radianos</param>
        /// <returns>Matriz de rotação</returns>
        public static Matrix2x2 RotationMatrix(float angle)
        {
            float cos = (float) Math.Cos(angle);
            float sin = (float) Math.Sin(angle);
            return new Matrix2x2(cos, -sin, sin, cos);
        }
    }

    /// <summary>
    /// Retângulo bidimensional com lados paralelos aos eixos coordenados
    /// </summary>
    public readonly struct Box2D : IGeometry2D
    {
        /// <summary>
        /// Retângulo vazio
        /// </summary>
        public static readonly Box2D EMPTY_BOX = new Box2D();
        /// <summary>
        /// Retângulo universo
        /// </summary>
        public static readonly Box2D UNIVERSE_BOX = new Box2D(Vector2D.NULL_VECTOR, Vector2D.NULL_VECTOR, new Vector2D(float.MaxValue, float.MaxValue));

        /// <summary>
        /// Cria um retângulo vazio com uma determinada origem
        /// </summary>
        /// <param name="origin">origem do retângulo</param>
        public Box2D(Vector2D origin)
        {
            this.Origin = origin;
            Mins = Vector2D.NULL_VECTOR;
            Maxs = Vector2D.NULL_VECTOR;
        }

        /// <summary>
        /// Cria um retângulo a partir da origem, mínimos e máximos
        /// </summary>
        /// <param name="origin">Origem</param>
        /// <param name="mins">Mínimos</param>
        /// <param name="maxs">Máximos</param>
        public Box2D(Vector2D origin, Vector2D mins, Vector2D maxs)
        {
            this.Origin = origin;
            this.Mins = mins;
            this.Maxs = maxs;
        }

        /// <summary>
        /// Cria um retângulo a partir de um retângulo do tipo Rectangle
        /// </summary>
        /// <param name="rect">Retângulo do tipo Rectangle</param>
        public Box2D(Rectangle rect)
        {
            Origin = new Vector2D(rect.Left, rect.Top);
            Mins = Vector2D.NULL_VECTOR;
            Maxs = new Vector2D(rect.Width, rect.Height);
        }

        /// <summary>
        /// Cria um retângulo a partir de um retângulo do tipo RectangleF
        /// </summary>
        /// <param name="rect">Retângulo do tipo RectangleF</param>
        public Box2D(RectangleF rect)
        {
            Origin = new Vector2D(rect.Left, rect.Top);
            Mins = Vector2D.NULL_VECTOR;
            Maxs = new Vector2D(rect.Width, rect.Height);
        }

        /// <summary>
        /// Converte para um retângulo do tipo RectangleF
        /// </summary>
        /// <returns>Retângulo do tipo Rectangle</returns>
        public Rectangle ToRectangle()
        {
            Box2D truncated = Truncate();
            return new Rectangle(truncated.LeftTop.ToPoint(), truncated.Size);
        }

        /// <summary>
        /// Converte para um retângulo do tipo Rectangle com seus extremos inferiores arredondados para baixo e seus extremos superiores arredondados para cima
        /// </summary>
        /// <returns>Retângulo arredondado do tipo Rectangle</returns>
        public Rectangle ToRoundedRectangle()
        {
            Vector2D mins = Origin + this.Mins;
            mins = new Vector2D((float) Math.Floor(mins.X), (float) Math.Floor(mins.Y));
            Vector2D maxs = Origin + this.Maxs;
            maxs = new Vector2D((float) Math.Ceiling(maxs.X), (float) Math.Ceiling(maxs.Y));
            maxs -= mins;
            return new Rectangle((int) mins.X, (int) mins.Y, (int) maxs.X, (int) maxs.Y);
        }

        /// <summary>
        /// Converte para um retângulo do tipo RectangleF
        /// </summary>
        /// <returns>Retângulo do tipo RectangleF</returns>
        public RectangleF ToRectangleF() => new RectangleF((Origin + Mins).ToPointF(), SizeF);

        /// <summary>
        /// Trunca as coordenadas do retângulo
        /// </summary>
        /// <returns>Retângulo truncado</returns>
        public Box2D Truncate()
        {
            Vector2D mins = Origin + this.Mins;
            mins = new Vector2D((float) Math.Floor(mins.X), (float) Math.Floor(mins.Y));
            Vector2D maxs = Origin + this.Maxs;
            maxs = new Vector2D((float) Math.Floor(maxs.X), (float) Math.Floor(maxs.Y));
            return new Box2D(mins, Vector2D.NULL_VECTOR, maxs - mins);
        }

        public override int GetHashCode()
        {
            Vector2D m = Origin + Mins;
            Vector2D M = Origin + Maxs;

            return 31 * m.GetHashCode() + M.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (!(obj is Box2D))
                return false;

            var other = (Box2D) obj;

            Vector2D m1 = Origin + Mins;
            Vector2D M1 = Origin + Maxs;

            Vector2D m2 = other.Origin + other.Mins;
            Vector2D M2 = other.Origin + other.Maxs;

            return m1 == m2 && M1 == M2;
        }

        public override string ToString() => "[" + Origin + ":" + Mins + ":" + Maxs + "]";

        /// <summary>
        /// Origem do retângulo
        /// </summary>
        public Vector2D Origin
        {
            get;
        }

        /// <summary>
        /// Extremo superior esquerdo do retângulo (ou mínimos absolutos)
        /// </summary>
        public Vector2D LeftTop => Origin + Mins;

        /// <summary>
        /// Extremo inferior direito do retângulo (ou máximos absolutos)
        /// </summary>
        public Vector2D RightBottom => Origin + Maxs;

        /// <summary>
        /// Mínimos relativos
        /// </summary>
        public Vector2D Mins
        {
            get;
        }

        /// <summary>
        /// Máximos relativos
        /// </summary>
        public Vector2D Maxs
        {
            get;
        }

        /// <summary>
        /// Vetor correspondente ao tamanho do retângulo contendo sua largura (width) na coordenada x e sua altura (height) na coordenada y
        /// </summary>
        public Vector2D SizeVector => new Vector2D(Width, Height);

        /// <summary>
        /// Estrutura do tipo Size correspondente ao retângulo. Faz a mesma coisa que SizeVector mas ao invés de retornar um valor do tipo Vector2D retorna do tipo Size.
        /// </summary>
        public Size Size => new Size((int) Width, (int) Height);

        /// <summary>
        /// Estrutura do tipo SizeF correspondente ao retângulo. Faz a mesma coisa que SizeVector mas ao invés de retornar um valor do tipo Vector2D retorna do tipo SizeF.
        /// </summary>
        public SizeF SizeF => new SizeF(Width, Height);

        /// <summary>
        /// Largura (base) do retângulo
        /// </summary>
        public float Width => Math.Abs(Maxs.X - Mins.X);

        /// <summary>
        /// Altura do retângulo
        /// </summary>
        public float Height => Math.Abs(Maxs.Y - Mins.Y);

        /// <summary>
        /// Área do retângulo
        /// </summary>
        /// <returns></returns>
        public float Area() => Width * Height;

        /// <summary>
        /// Escala o retângulo para a esquerda
        /// </summary>
        /// <param name="alpha"></param>
        /// <returns></returns>
        public Box2D ScaleLeft(float alpha)
        {
            float width = Width;
            return new Box2D(LeftTop + alpha * (width - 1) * Vector2D.LEFT_VECTOR, Vector2D.NULL_VECTOR, new Vector2D(alpha * width, Height));
        }

        /// <summary>
        /// Escala o retângulo para a direita
        /// </summary>
        /// <param name="alpha"></param>
        /// <returns></returns>
        public Box2D ScaleRight(float alpha) => new Box2D(LeftTop, Vector2D.NULL_VECTOR, new Vector2D(alpha * Width, Height));

        /// <summary>
        /// Escala o retângulo para cima
        /// </summary>
        /// <param name="alpha"></param>
        /// <returns></returns>
        public Box2D ScaleTop(float alpha)
        {
            float height = Height;
            return new Box2D(LeftTop + alpha * (height - 1) * Vector2D.UP_VECTOR, Vector2D.NULL_VECTOR, new Vector2D(Width, alpha * height));
        }

        /// <summary>
        /// Escala o retângulo para baixo
        /// </summary>
        /// <param name="alpha"></param>
        /// <returns></returns>
        public Box2D ScaleBottom(float alpha) => new Box2D(LeftTop, Vector2D.NULL_VECTOR, new Vector2D(Width, alpha * Height));

        /// <summary>
        /// Translata um retângulo na direção de um vetor
        /// </summary>
        /// <param name="box">Retângulo</param>
        /// <param name="vec">Vetor</param>
        /// <returns>Retângulo box translatado na direção de vec</returns>
        public static Box2D operator +(Box2D box, Vector2D vec) => new Box2D(box.Origin + vec, box.Mins, box.Maxs);

        /// <summary>
        /// Translata um retângulo na direção de um vetor
        /// </summary>
        /// <param name="vec">Vetor</param>
        /// /// <param name="box">Retângulo</param>
        /// <returns>Retângulo box translatado na direção de vec</returns>

        public static Box2D operator +(Vector2D vec, Box2D box) => new Box2D(box.Origin + vec, box.Mins, box.Maxs);

        /// <summary>
        /// Translata um retângulo na direção oposta de um vetor
        /// </summary>
        /// <param name="box">Retângulo</param>
        /// <param name="vec">Vetor</param>
        /// <returns>Retângulo box translatado na direção oposta de vec</returns>
        public static Box2D operator -(Box2D box, Vector2D vec) => new Box2D(box.Origin - vec, box.Mins, box.Maxs);

        /// <summary>
        /// Incrementa os lados de um retângulo
        /// </summary>
        /// <param name="increment">Incremento</param>
        /// <param name="box">Retângulo</param>
        /// <returns>Retângulo com seus lados incrementados</returns>
        public static Box2D operator +(float increment, Box2D box)
        {
            Vector2D m = box.Origin + box.Mins;
            var incVector = new Vector2D(increment, increment);
            return new Box2D(m - incVector, Vector2D.NULL_VECTOR, new Vector2D(box.Size) + 2 * incVector);
        }

        /// <summary>
        /// Incrementa os lados de um retângulo
        /// </summary>
        /// <param name="box">Retângulo</param>
        /// <param name="increment">Incremento</param>
        /// <returns>Retângulo com seus lados incrementados</returns>
        public static Box2D operator +(Box2D box, float increment)
        {
            Vector2D m = box.Origin + box.Mins;
            var incVector = new Vector2D(increment, increment);
            return new Box2D(m - incVector, Vector2D.NULL_VECTOR, new Vector2D(box.Size) + 2 * incVector);
        }

        /// <summary>
        /// Decrementa os lados de um retângulo
        /// </summary>
        /// <param name="decrement">Demento</param>
        /// <param name="box">Retângulo</param>
        /// <returns>Retângulo com seus lados decrementados</returns>
        public static Box2D operator -(float decrement, Box2D box)
        {
            Vector2D m = box.Origin + box.Mins;
            var decVector = new Vector2D(decrement, decrement);
            return new Box2D(m + decVector, Vector2D.NULL_VECTOR, new Vector2D(box.Size) - 2 * decVector);
        }

        /// <summary>
        /// Decrementa os lados de um retângulo
        /// </summary>
        /// <param name="box">Retângulo</param>
        /// /// <param name="decrement">Demento</param>
        /// <returns>Retângulo com seus lados decrementados</returns>
        public static Box2D operator -(Box2D box, float increment)
        {
            Vector2D m = box.Origin + box.Mins;
            var incVector = new Vector2D(increment, increment);
            return new Box2D(m + incVector, Vector2D.NULL_VECTOR, new Vector2D(box.Size) - 2 * incVector);
        }

        /// <summary>
        /// Escala um retângulo
        /// </summary>
        /// <param name="factor">Fator de escala</param>
        /// <param name="box">Retângulo</param>
        /// <returns>Retângulo com suas coordenadas e dimensões escaladas por factor</returns>
        public static Box2D operator *(float factor, Box2D box)
        {
            Vector2D m = box.Origin + box.Mins;
            return new Box2D(m * factor, Vector2D.NULL_VECTOR, new Vector2D(box.Size) * factor);
        }

        /// <summary>
        /// Escala um retângulo
        /// </summary>
        /// <param name="box">Retângulo</param>
        /// /// <param name="factor">Fator de escala</param>
        /// <returns>Retângulo com suas coordenadas e dimensões escaladas por factor</returns>
        public static Box2D operator *(Box2D box, float factor)
        {
            Vector2D m = box.Origin + box.Mins;
            return new Box2D(m * factor, Vector2D.NULL_VECTOR, new Vector2D(box.Size) * factor);
        }

        /// <summary>
        /// Escala um retângulo inversamente (escala pelo inverso do divisor)
        /// </summary>
        /// <param name="box">Retângulo</param>
        /// <param name="divisor">Divisor</param>
        /// <returns>Retângulo com suas coordenadas e dimensões divididas por divisor</returns>
        public static Box2D operator /(Box2D box, float divisor)
        {
            Vector2D m = box.Origin + box.Mins;
            return new Box2D(m / divisor, Vector2D.NULL_VECTOR, new Vector2D(box.Size) / divisor);
        }

        /// <summary>
        /// Faz a união entre dois retângulos que resultará no menor retângulo que contém os retângulos dados
        /// </summary>
        /// <param name="box1">Primeiro retângulo</param>
        /// <param name="box2">Segundo retângulo</param>
        /// <returns>Menor retângulo que contém os dois retângulos dados</returns>
        public static Box2D operator |(Box2D box1, Box2D box2)
        {
            Vector2D m1 = box1.Origin + box1.Mins;
            Vector2D M1 = box1.Origin + box1.Maxs;

            Vector2D m2 = box2.Origin + box2.Mins;
            Vector2D M2 = box2.Origin + box2.Maxs;

            float minsX = Math.Min(m1.X, m2.X);
            float maxsX = Math.Max(M1.X, M2.X);

            float minsY = Math.Min(m1.Y, m2.Y);
            float maxsY = Math.Max(M1.Y, M2.Y);

            return new Box2D(new Vector2D(minsX, minsY), Vector2D.NULL_VECTOR, new Vector2D(maxsX - minsX, maxsY - minsY));
        }

        /// <summary>
        /// Faz a intersecção de dois retângulos que resultará em um novo retângulo que esteja contido nos dois retângulos dados. Se os dois retângulos dados forem disjuntos então o resultado será um vetor nulo.
        /// </summary>
        /// <param name="box1">Primeiro retângulo</param>
        /// <param name="box2">Segundo retângulo</param>
        /// <returns>Interesecção entre os dois retângulos dados ou um vetor nulo caso a intersecção seja um conjunto vazio</returns>
        public static Box2D operator &(Box2D box1, Box2D box2)
        {
            Vector2D m1 = box1.Origin + box1.Mins;
            Vector2D M1 = box1.Origin + box1.Maxs;

            Vector2D m2 = box2.Origin + box2.Mins;
            Vector2D M2 = box2.Origin + box2.Maxs;

            float minsX = Math.Max(m1.X, m2.X);
            float maxsX = Math.Min(M1.X, M2.X);

            if (maxsX < minsX)
                return EMPTY_BOX;

            float minsY = Math.Max(m1.Y, m2.Y);
            float maxsY = Math.Min(M1.Y, M2.Y);

            return maxsY < minsY
            ? EMPTY_BOX
            : new Box2D(new Vector2D(minsX, minsY), Vector2D.NULL_VECTOR, new Vector2D(maxsX - minsX, maxsY - minsY));
        }

        /// <summary>
        /// Verifica se um vetor está contido no interior de um retângulo
        /// </summary>
        /// <param name="vec">Vetor</param>
        /// <param name="box">Retângulo</param>
        /// <returns>true se vec estiver contido no interior box, false caso contrário</returns>
        public static bool operator <(Vector2D vec, Box2D box)
        {
            Vector2D m = box.Origin + box.Mins;
            Vector2D M = box.Origin + box.Maxs;
            return m.X < vec.X && vec.X < M.X && m.Y < vec.Y && vec.Y < M.Y;
        }

        /// <summary>
        /// Verifica se um vetor está contido no de um retângulo
        /// </summary>
        /// <param name="vec">Vetor</param>
        /// <param name="box">Retângulo</param>
        /// <returns>true se vec estiver contido no exterior de box, false caso contrário</returns>
        public static bool operator >(Vector2D vec, Box2D box) => !(vec <= box);

        /// <summary>
        /// Verifica se um retâgulo contém um vetor em seu exterior
        /// </summary>
        /// <param name="box">Retângulo</param>
        /// <param name="vec">Vetor</param>
        /// <returns>true se box contém vec em seu exterior, false caso contrário</returns>
        public static bool operator <(Box2D box, Vector2D vec) => !(box >= vec);

        /// <summary>
        /// Verifica um retângulo contém um vetor em seu interior
        /// </summary>
        /// <param name="box">Retângulo</param>
        /// /// <param name="vec">Vetor</param>
        /// <returns>true box contém vec em seu interior, false caso contrário</returns>
        public static bool operator >(Box2D box, Vector2D vec) => vec < box;

        /// <summary>
        /// Verifica se um vetor está contido no interior ou na borda de um retângulo
        /// </summary>
        /// <param name="vec">Vetor</param>
        /// <param name="box">Retângulo</param>
        /// <returns>true se vec estiver contido no interior ou na borda de box, false caso contrário</returns>
        public static bool operator <=(Vector2D vec, Box2D box)
        {
            Vector2D m = box.Origin + box.Mins;
            Vector2D M = box.Origin + box.Maxs;
            return m.X <= vec.X && vec.X <= M.X && m.Y <= vec.Y && vec.Y <= M.Y;
        }

        /// <summary>
        /// Verifica se um vetor está contido no exterior ou na borda de um retângulo
        /// </summary>
        /// <param name="vec">Vetor</param>
        /// <param name="box">Retângulo</param>
        /// <returns>true se vec estiver contido no exterior ou na borda de box, false caso contrário</returns>
        public static bool operator >=(Vector2D vec, Box2D box) => !(vec < box);

        /// <summary>
        /// Verifica se um retângulo contém um vetor em seu exterior ou em sua borda
        /// </summary>
        /// <param name="box">Retângulo</param>
        /// <param name="vec">Vetor</param>
        /// <returns>true se box contém vec em seu exterior ou em sua borda, false caso contrário</returns>
        public static bool operator <=(Box2D box, Vector2D vec) => !(box > vec);

        /// <summary>
        /// Verifica um retângulo contém um vetor em seu interior ou emsua borda
        /// </summary>
        /// <param name="box">Retângulo</param>
        /// /// <param name="vec">Vetor</param>
        /// <returns>true box contém vec em seu interior ou em sua borda, false caso contrário</returns>
        public static bool operator >=(Box2D box, Vector2D vec) => vec <= box;

        /// <summary>
        /// Veririca se um retângulo está contido em outro retângulo
        /// </summary>
        /// <param name="box1">Primeiro retângulo</param>
        /// <param name="box2">Segundo retângulo</param>
        /// <returns>true se box1 está contido em box2, falso caso contrário</returns>
        public static bool operator <=(Box2D box1, Box2D box2) => (box1 & box2) == box1;

        /// <summary>
        /// Verifica se um retângulo contém outro retângulo
        /// </summary>
        /// <param name="box1">Primeiro retângulo</param>
        /// <param name="box2">Segundo retângulo</param>
        /// <returns>true se box1 contém box2, false caso contrário</returns>
        public static bool operator >=(Box2D box1, Box2D box2) => (box2 & box1) == box2;

        /// <summary>
        /// Veririca se um retângulo está inteiramente contido em outro retângulo
        /// </summary>
        /// <param name="box1">Primeiro retângulo</param>
        /// <param name="box2">Segundo retângulo</param>
        /// <returns>true se box1 está inteiramente contido em box2 (ou seja box1 está em box2 mas box1 não é igual a box2), falso caso contrário</returns>
        public static bool operator <(Box2D box1, Box2D box2) => box1 <= box2 && box1 != box2;

        /// <summary>
        /// Verifica se um retângulo contém inteiramente outro retângulo
        /// </summary>
        /// <param name="box1">Primeiro retângulo</param>
        /// <param name="box2">Segundo retângulo</param>
        /// <returns>true se box1 contém inteiramente box2 (ou seja, box1 contém box2 mas box1 não é igual a box2), false caso contrário</returns>
        public static bool operator >(Box2D box1, Box2D box2) => box2 <= box1 && box1 != box2;

        /// <summary>
        /// Verifica se dois retângulos são iguais
        /// </summary>
        /// <param name="box1">Primeiro retângulo</param>
        /// <param name="box2">Segundo retângulo</param>
        /// <returns>true se forem iguais, false caso contrário</returns>
        public static bool operator ==(Box2D box1, Box2D box2)
        {
            Vector2D m1 = box1.Origin + box1.Mins;
            Vector2D M1 = box1.Origin + box1.Maxs;

            Vector2D m2 = box2.Origin + box2.Mins;
            Vector2D M2 = box2.Origin + box2.Maxs;

            return m1 == m2 && M1 == M2;
        }

        /// <summary>
        /// Verifica se dois retângulos são diferentes
        /// </summary>
        /// <param name="box1">Primeiro retângulo</param>
        /// <param name="box2">Segundo retângulo</param>
        /// <returns>true se forem diferentes, false caso contrário</returns>
        public static bool operator !=(Box2D box1, Box2D box2)
        {
            Vector2D m1 = box1.Origin + box1.Mins;
            Vector2D M1 = box1.Origin + box1.Maxs;

            Vector2D m2 = box2.Origin + box2.Mins;
            Vector2D M2 = box2.Origin + box2.Maxs;

            return m1 != m2 || M1 != M2;
        }
    }

    /// <summary>
    /// Polígono bidimensional.
    /// Para a representação do polígono, é assumido que seus vértices estejam em sequência no sentido anti-horário.
    /// Caso estejam no sentido horário, então a região delimitada pelo polígono será o complementar do mesmo pológono se seus vértices estivessem orientados no sentido anti-horário.
    /// </summary>
    public readonly struct Polygon2D : IGeometry2D
    {
        private readonly Vector2D[] vertexes; // vértices do polígono

        /// <summary>
        /// Cria um polígono a partir de uma lista de vértices
        /// </summary>
        /// <param name="vertexes">Vértices do pológino</param>
        public Polygon2D(params Vector2D[] vertexes) => this.vertexes = vertexes;

        /// <summary>
        /// Vértice correspondente a posição index do polígono
        /// </summary>
        /// <param name="index">Posição do vértice</param>
        /// <returns>Vértice</returns>
        public Vector2D this[int index] => vertexes[index];

        /// <summary>
        /// Número de vértices do polígono
        /// </summary>
        /// <returns>Quantidade de vértices</returns>
        public int Count() => vertexes.Length;

        /// <summary>
        /// Perímetro do polígono
        /// </summary>
        /// <returns>Perímetro</returns>
        public float Perimeter()
        {
            float result = 0;
            int n = vertexes.Length;

            for (int i = 0; i < n; i++)
                result += (vertexes[(i + 1) % n] - vertexes[i]).Length;

            return result;
        }

        /// <summary>
        /// Área do polígono
        /// </summary>
        /// <returns>Área</returns>
        public float Area()
        {
            float result = 0;
            int n = vertexes.Length;

            for (int i = 0; i < n; i++)
            {
                Vector2D v1 = vertexes[i];
                Vector2D v2 = vertexes[(i + 1) % n];
                result += v1.X * v2.Y - v2.X * v1.Y;
            }

            return result / 2F;
        }

        /// <summary>
        /// Verifica se o polígono está vazio
        /// </summary>
        /// <returns>true se estiver vazio, false caso contrário</returns>
        public bool IsEmpty() => vertexes.Length == 0;

        /// <summary>
        /// Compara a posição de um vetor com o polígono
        /// </summary>
        /// <param name="v">Vetor</param>
        /// <returns>1 se o vetor estiver no interior do polígono, 0 se estiver na fronteira, -1 se estiver no exterior</returns>
        public int Compare(Vector2D v)
        {
            int n = vertexes.Length;
            int s = 0;

            for (int i = 0; i < n; i++)
            {
                Vector2D v1 = vertexes[i];
                Vector2D v2 = vertexes[(i + 1) % n];

                float f = (v.X - v1.X) * (v2.Y - v1.Y) - (v.Y - v1.Y) * (v2.X - v1.X);

                if (f == 0 && new LineSegment(v1, v2).Contains(v))
                    return 0;

                if (f > 0)
                    s++;
            }

            return (s & 1) != 0 ? -1 : 1;
        }

        /// <summary>
        /// Inverte a orientação dos vértices do polígono
        /// </summary>
        /// <returns>Polígono orientado no sentido inverso</returns>
        public Polygon2D Negate()
        {
            int n = this.vertexes.Length;
            var vertexes = new Vector2D[n];

            for (int i = 0; i < n; i++)
                vertexes[n - i - 1] = vertexes[i];

            return new Polygon2D(vertexes);
        }

        /// <summary>
        /// Inverte a orientação dos vértices de um polígono
        /// </summary>
        /// <returns>Polígono p orientado no sentido inverso</returns>
        public static Polygon2D operator -(Polygon2D p) => p.Negate();

        /// <summary>
        /// Verifica se um vetor está no interior de um polígono
        /// </summary>
        /// <param name="v">Vetor</param>
        /// <param name="p">Polígono</param>
        /// <returns>true se v estiver no interior de p, false caso contrário</returns>
        public static bool operator <(Vector2D v, Polygon2D p) => p.Compare(v) == 1;

        /// <summary>
        /// Verifica se um vetor está no interior ou na fronteira de um polígono
        /// </summary>
        /// <param name="v">Vetor</param>
        /// <param name="p">Polígono</param>
        /// <returns>true se v estiver no interior ou na fronteira de p, false caso contrário</returns>
        public static bool operator <=(Vector2D v, Polygon2D p) => p.Compare(v) >= 0;

        /// <summary>
        /// Verifica se um vetor está no exterior de um polígono
        /// </summary>
        /// <param name="v">Vetor</param>
        /// <param name="p">Polígono</param>
        /// <returns>true se v está no exterior de p, false caso contrário</returns>
        public static bool operator >(Vector2D v, Polygon2D p) => p.Compare(v) == -1;

        /// <summary>
        /// Verifica se um vetor está no exterior ou na fronteira de um polígono
        /// </summary>
        /// <param name="v">Vetor</param>
        /// <param name="p">Polígono</param>
        /// <returns>true se v está no exterior ou na fronteira de p, false caso contrário</returns>
        public static bool operator >=(Vector2D v, Polygon2D p) => p.Compare(v) <= 0;

        /// <summary>
        /// Verifica se dois polígonos são iguais
        /// </summary>
        /// <param name="p1">Primeiro polígono</param>
        /// <param name="p2">Segundo polígono</param>
        /// <returns>true se forem iguais, false caso contrário</returns>
        public static bool operator ==(Polygon2D p1, Polygon2D p2)
        {
            int n = p1.vertexes.Length;

            if (n != p2.vertexes.Length)
                return false;

            for (int i = 0; i < n; i++)
            {
                if (p1[i] != p2[i])
                    break;
            }

            return false;
        }

        /// <summary>
        /// Veririca se dois polígonos são diferentes
        /// </summary>
        /// <param name="p1">Primeiro polígono</param>
        /// <param name="p2">Segundo polígono</param>
        /// <returns>true se forem diferentes, false caso contrário</returns>
        public static bool operator !=(Polygon2D p1, Polygon2D p2) => !(p1 == p2);
    }
}
