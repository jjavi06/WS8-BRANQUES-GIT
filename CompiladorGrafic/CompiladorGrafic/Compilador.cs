using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using PilaGeneric;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CompiladorGrafic
{
    class Compilador
    {
        #region Atributos
        private Pila<char> elementos;
        #endregion

        #region Constructores
        public Compilador()
        {
            elementos = new Pila<char>();
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Recorre cada uno de los carácteres de una expresión y si cualquiera de ellos es un paréntesis o claudator abierto, lo apila y
        /// si es uno cerrado comprueba que coincidan con un Peek, si estos dos coinciden se hace un Pop para elimianar el caracter abierto. 
        /// </summary>
        /// <param name="expresion"></param>
        /// <returns>Devuelve una expresión boleana en base a si todos los carácteres de apertura y cierre de la expresión 
        /// coinciden.</returns>
        public bool Validar(string expresion)
        {
            bool valido = true;
            int i=0;
            char actual, abierto;
            while(valido && i < expresion.Length)
            {
                actual = expresion[i++];
                if (actual == '(' || actual == '[' || actual == '{')
                    elementos.Push(actual);
                else if(actual == ')' || actual == ']' || actual == '}')
                {
                    if (!elementos.IsEmpty)
                    {
                        abierto = elementos.Peek();
                        if (!Coinciden(actual, abierto))
                            valido = false;
                        else elementos.Pop();
                    }
                    else valido = true;
                }
            }
            if (!elementos.IsEmpty)
            {
                valido = false;
                elementos.Clear();
            }
            return valido;
        }
        /// <summary>
        /// Comprueba que los claudators o paréntesis sean uno de apertura y el otro de cierre, en caso de que sea así, deveulve true.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public bool Coinciden(char a, char b)
        {
            bool coinciden = false;
            if (a == '(' && b == ')') coinciden = true;
            else if (a == '[' && b == ']') coinciden = true;
            else if (a == '{' && b == '}') coinciden = true;
            return coinciden;
        }
        #endregion
    }
}
