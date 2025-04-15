using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace PilaGeneric
{
    internal class Pila<T> : IEnumerable<T>, ICollection<T>
    {
        #region Atributos
        private T[] items;
        private const int DEFAULT_SIZE = 5;
        private int top;
        #endregion

        #region Constructores
        public Pila():this(DEFAULT_SIZE)
        {
        }
        public Pila(int size)
        {
            items = new T[size];
            top = -1;
        }
        public Pila(IEnumerable<T> collection)
        {
            items = collection.ToArray();
            top = items.Length;
        }
        #endregion

        #region Propiedades
        //Propiedades
        public bool IsFull { get { return top == items.Length-1; } }
        public bool IsEmpty { get { return top == -1; } }
        private T this[int index] { get { return items[index]; } }
        public int Count { get { return top+1; } }
        public int Capacity { get  { return items.Length; } }
        //Propiedad de ICollection
        public bool IsReadOnly => false;
        #endregion

        #region Métodos
        //Métodos
        /// <summary>
        /// Muestra y elimina el último elemento de la pila
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public T Pop()
        {
            if (IsEmpty) throw new InvalidOperationException("No hay elementos en la pila");
            else
            {
                top--;
                return items[top + 1];
            }
        }
        /// <summary>
        /// Muestra SIN eliminar el último elemento de la pila
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public T Peek()
        {
            if (IsEmpty) throw new InvalidOperationException("No hay elementos en la pila");
            else
                return items[top];
        }
        /// <summary>
        /// Agrega un elemento al final de la pila
        /// </summary>
        /// <param name="item"></param>
        /// <exception cref="StackOverflowException"></exception>
        public void Push(T item)
        {
            if (IsFull) throw new StackOverflowException("La pila esta llena");
            else
            {
                top++;
                items[top] = item;
            }
        }
        /// <summary>
        /// Devuelve un array con todos los elementos válidos de la pila usando el enumerator.
        /// </summary>
        /// <returns></returns>
        public T[] ToArray()
        {
            T[] nouArray = new T[Count];
            int i = 0;
            IEnumerator<T> nou;
            nou = GetEnumerator();
            while (nou.MoveNext())
            {
                nouArray[i] = nou.Current;
                i++;
            }
            return nouArray;
        }
        /// <summary>
        /// Si la capacidad proporcionada por el usuario es mayor a la capacidad actual, se amplia la capacidad
        /// de la pila.
        /// </summary>
        /// <param name="newCapacity"></param>
        /// <returns>Devuelve un int con la capacidad de la pila actualizada.</returns>
        /// <exception cref="Exception"></exception>
        public int EnsureCapacity(int newCapacity)
        {
            if (Capacity >= newCapacity) throw new Exception("La nueva Capacidad es inferior a la capacidad actual.");
            else
            {
                T[] itemsAux = new T[newCapacity];
                int i = 0;
                foreach (T item in items)
                {
                    itemsAux[i] = item;
                    i++;
                }
                items = itemsAux;
                return Capacity;
            }
        }
        public override string ToString()
        {
            string elements = "[ ";
            foreach (T item in this)
            {
                elements += $"{item.ToString()}, ";
            }
            elements += "]";
            string salida = elements.Replace(", ]", " ]");
            return salida;
        }
        public override bool Equals(object? obj)
        {
            return true;
        }
        #endregion

        #region Interfícies
        //Métodos IEnumerable
        public IEnumerator<T> GetEnumerator()
        {
            return new PilaEnumerator(items, top);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        //Métodos ICollection
        public void Add(T item)
        {
            if (top == items.Length - 1) throw new Exception("Pila llena");
            else
            {
                top++;
                items[top] = item;
            }
        }
        public void Clear()
        {
            top = -1;
        }
        public bool Contains(T item)
        {
            if(IsEmpty) throw new Exception("No hay elementos en la Pila.");
            else
            {
                bool trobat = false;
                int pos = top;
                while (!trobat && pos > -1)
                {
                    if (items[pos].Equals(item))
                        trobat = true;
                    else pos--;
                }
                return trobat;
            }
        }
        /// <summary>
        /// Copia todos los elementos válidos de la pila a un array a partir del índice indicado. 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void CopyTo(T[] array, int arrayIndex)
        {
            if (arrayIndex < 0) throw new ArgumentOutOfRangeException("El índice del array debe ser mas grande que -1");
            else
            {
                foreach (T item in this)
                {
                    array[arrayIndex] = item;
                    arrayIndex++;
                }
            }
        }
        /// <summary>
        /// Elimina todos los elementos hasta llegar al elemento indicado que también se elimina.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool Remove(T item)
        {
            if (!Contains(item)) throw new Exception("No existe el elemento a eliminar");
            else
            {
                bool trobat = false;
                int pos = top;
                while (!trobat && pos > -1)
                {
                    if (items[pos].Equals(item))
                    {
                        trobat = true;
                        top = pos - 1;
                    }
                    else pos--;
                }
                return trobat;
            }
        }
        #endregion

        #region Internal Class PilaEnumerator
        //Internal class Pila Enumerator
        private class PilaEnumerator : IEnumerator<T>
        {
            //Atributos
            private T[] dades;
            private int pos;
            private int posInicial;

            //Constructores
            public PilaEnumerator(T[] elements, int top)
            {
                dades = elements;
                pos = top+1;
                posInicial = top;
            }

            //Propiedades
            public T Current => dades[pos];

            object IEnumerator.Current => Current;

            //Métodos
            public bool MoveNext()
            {
                pos--;
                return pos > -1;
            }

            public void Reset()
            {
                pos = posInicial;
            }
            /// <summary>
            /// Limpiar memoria, una vez acabado el método, limpia el array "datos" para que no ocupe memoria.
            /// </summary>
            public void Dispose()
            {
                dades = null;
            }
        }

        #endregion
    }
}
