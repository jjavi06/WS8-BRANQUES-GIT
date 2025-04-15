using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Windows.Documents;
using PilaGeneric;
using ProvaTaula;
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
            elementos = new Pila<char>(15);
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
        /// <summary>
        /// Define la estructura de la expresión en Notación Polaca Inversa usando el método Desapilar para determinar la prioridad 
        /// de los operadores. En caso de encontrar un número en la expresión lo añade directamente a la tablaLista salida y en caso de
        /// ser un operador apila en operadores o desapila en salida según su prioridad.
        /// </summary>
        /// <param name="expresion"></param>
        /// <returns></returns>
        public TaulaLlista<char> EstructuraNotPolaca(string expresion)
        {
            expresion = expresion.Replace(" ", "");
            Pila<char> operadores = new Pila<char>(expresion.Length);
            TaulaLlista<char> salida = new TaulaLlista<char>(expresion.Length);
            int i = 0, desapilar;
            char c;
            while (i < expresion.Length)
            {
                c = expresion[i++];
                if (char.IsDigit(c))
                    salida.Add(c);
                else
                {
                    if (!operadores.IsEmpty)
                    {
                        desapilar = Desapilar(operadores.Peek(), c);
                        if (desapilar == 1)
                        {
                            salida.Add(operadores.Pop());
                            if (!operadores.IsEmpty && Desapilar(operadores.Peek(), c) == 1)
                                salida.Add(operadores.Pop());
                            operadores.Add(c);
                        }
                        else if (desapilar == 2)
                        {
                            char pop = operadores.Pop();
                            while (pop != '(' && pop != '[' && pop != '{')
                            {
                                salida.Add(pop);
                                pop = operadores.Pop();
                            }
                        }
                        else
                            operadores.Add(c);
                    }
                    else
                        operadores.Add(c);
                }
            }
            while (!operadores.IsEmpty)
                salida.Add(operadores.Pop());
            return salida;
        }
        /// <summary>
        /// Este método compara las prioridades del operador actual y el operador en Pila para determinar si hay que desapilar al 
        /// operador en la pila o no.
        /// </summary>
        /// <param name="enPila"></param>
        /// <param name="actual"></param>
        /// <returns>
        /// Devuelve 0 en caso de que no haya que desapilar, 1 si hay que desapilar el operador de la pila y 2 para el
        /// caso especial.
        /// </returns>
        private int Desapilar(char enPila, char actual)
        {
            int desapilar = 0;
            //Caso especial, si actual es un paréntesis de cierre, hay que desapilar hasta su apertura.
            if (actual == ')' || actual == ']' || actual == '}') desapilar = 2;
            else if (!(enPila == '(' || enPila == '[' || enPila == '{'))
            {
                int prioridadEnPila = 0, prioridadActual = 0;
                if (enPila == '+' || enPila == '-') prioridadEnPila = 3;
                else if (enPila == '/' || enPila == '*') prioridadEnPila = 2;

                if (actual == '+' || actual == '-') prioridadActual = 3;
                else if (actual == '/' || actual == '*') prioridadActual = 2;

                if (prioridadEnPila <= prioridadActual)
                    desapilar = 1;
            }
            return desapilar;
        }
        #endregion
    }
}
