using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace ProvaTaula
{
    public class TaulaLlista<T> : ICollection<T>, IList<T>, ICloneable
    {
        #region Atributos
        //Atributs
        private T[] dades;
        private int nElem;
        private const int DEFAULT_SIZE = 10;
        #endregion

        #region Constructores
        //Constructors
        public TaulaLlista() : this(DEFAULT_SIZE)
        {
        }
        public TaulaLlista(int size)
        {
            dades = new T[size];
            nElem = 0;
        }
        public TaulaLlista(TaulaLlista<T> llista)
        {
            nElem = llista.nElem;
            dades = new T[llista.dades.Length];
            for (int i = 0; i < nElem; i++)
            {
                this.dades[i] = llista.dades[i];
            }
        }
        #endregion

        #region Propiedades
        //Propiedades
        public int Count
        {
            get { return nElem; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public T this[int index] { get { return dades[index]; } set { throw new Exception("No se puede realizar esta acción."); } }

        public int Capacity
        {
            get { return dades.Length; }
        }
        public bool IsFull
        {
            get { return nElem == dades.Length; }
        }
        #endregion

        #region Métodos
        //Métodos
        public void Add(T item)
        {
            if (item is null) throw new NotSupportedException("Item introduït és null");
            else if (IsReadOnly) throw new NotSupportedException("Llista es només de lectura");
            else
            {
                if (IsFull) DuplicarCapacitat();
                dades[nElem] = item;
                nElem++;
            }
        }

        public void Clear()
        {
            if (nElem == 0)
                throw new NotSupportedException("Llista no te elements");
            else if (nElem > 0)
                nElem = 0;
        }

        public bool Contains(T item)
        {
            bool contain=false;
            if (nElem > 0)
            {
                int i = 0;
                while (!contain && i < nElem)
                {
                    if (dades[i].Equals(item))
                        contain = true;
                    i++;
                }
            }
            else throw new NotSupportedException("Llista biuda");
            return contain;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array is null) throw new ArgumentNullException("No hi han elements a l'array. ");
            else if (arrayIndex < 0) throw new ArgumentOutOfRangeException("L'índex no pot ser mer petit que 0. ");
            else
            {
                foreach (T item in this)
                {
                    array[arrayIndex] = item;
                    arrayIndex++;
                }
            }
        }

        public bool Remove(T item)
        {
            if (nElem == 0) throw new Exception("No hi han valor a la Llista.");
            else if (!Contains(item)) throw new Exception("L'element no existeix en la Llista");
            else
            {
                RemoveAt(IndexOf(item));
                return true;
            }
        }
        public void RemoveAt(int index)
        {
            for (int i = index; i < nElem; i++)
            {
                dades[i] = dades[i + 1];
            }
            nElem--;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new LlistaEnumerator(dades, nElem);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        private void DuplicarCapacitat()
        {
            T[] dadesAux = new T[dades.Length*2];
            int i = 0;
            foreach (T item in this)
            {
                dadesAux[i] = item;
                i++;
            }
            dades = dadesAux;
        }

        //IList
        public int IndexOf(T item)
        {
            if (nElem == 0) throw new NotSupportedException("No hi han elements a l'array.");
            int index = -1, i = 0;
            while (index == -1 && i < nElem)
            {
                if (dades[i].Equals(item))
                    index = i;
                i++;
            }
            return index;
        }

        public void Insert(int index, T item)
        {
            if (dades.IsReadOnly) throw new NotSupportedException("Llista es només de lectura.");
            if (item == null) throw new NotSupportedException("Item introduït és null.");
            if (index < 0 || index > nElem) throw new NotSupportedException("Índex fora de rang.");
            if (IsFull)
                DuplicarCapacitat();
            if (nElem > -1 && nElem < dades.Length)
            {
                for (int i = nElem; i > index; i--)
                {
                    dades[i] = dades[i - 1];
                }
                dades[index] = item;
                nElem++;
            }
        }

        //Métodos extras
        public T[] ToArray()
        {
            if (nElem == 0) throw new NotSupportedException("No hi han elements a la llista");
            T[] array = new T[nElem];
            int i = 0;
            foreach(T item in this)
            {
                array[i] = item;
                i++;
            }
            return array;
        }
        public int LastIndexOf(T item)
        {
            if (nElem == 0) throw new NotSupportedException("No hi han elements a la llista");
            int i = nElem-1;
            int index = -1;
            while (index == -1 && i >= 0)
            {
                if (dades[i].Equals(item))
                    index = i;
                i--;
            }
            if (index == -1) throw new NotSupportedException("L'element no existeix a la llista. ");
            return index;
        }
        public T[] Reverse()
        {
            if (nElem == 0) throw new NotSupportedException("No hi han elements a la llista");
            T[] array = new T[nElem];
            for (int i = 0; i < nElem; i++)
            {
                array[i] = dades[nElem-1-i];
            }
            return array;
        }
        //IClonable
        public object Clone()
        {
            TaulaLlista<T> copia = new TaulaLlista<T>(dades.Length);
            int n = nElem;
            for(int i = 0;i < n; i++)
            {
                if (dades[i] is ICloneable cloneable)
                {
                    copia.Add((T)cloneable.Clone());
                }
                else
                {
                    copia.Add(dades[i]);
                }
            }
            return copia;
        }
        #endregion

        #region Internal Class TaulaLlistaEnumerator
        private class LlistaEnumerator : IEnumerator<T>
        {
            //Atributos
            private T[] dades;
            private int posActual;
            private int nElem;

            //Constructores
            public LlistaEnumerator(T[] elements, int nElem)
            {
                dades = elements;
                this.nElem = nElem;
                posActual = -1;
            }

            //Propiedades
            public T Current
            {
                get
                {
                    if (posActual < 0 || posActual >= nElem)
                        throw new InvalidOperationException();
                    return dades[posActual];
                }
            }
            object IEnumerator.Current => Current;

            //Métodos
            public bool MoveNext()
            {
                if (posActual < nElem - 1)
                {
                    posActual++;
                    return true;
                }
                return false;
            }

            public void Reset()
            {
                posActual = -1;
            }

            public void Dispose() { dades = null; }
        }
        #endregion
    }
}
