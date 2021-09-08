using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbolesDeHuffman
{
    public class ColaDePrioridad <T> where T : IComparable
    {
        public static NodoCP<T> NodoCPPadre = new NodoCP<T>();

        public static NodoCP<T> newNode(T Nuevo)
        {
            NodoCP<T> temp = new NodoCP<T>();
            temp.Data = Nuevo;
            temp.Siguiente = null;
            return temp;
        }

        public static T top(NodoCP<T> head)
        {
            return head.Data;
        }

        public static NodoCP<T> pop(NodoCP<T> head)
        {
            NodoCP<T> temp = head;
            (head) = (head).Siguiente;
            return head;
        }

        public NodoCP<T> push(NodoCP<T> Padre, T Nuevo, Delegate Comparacion)
        {
            NodoCP<T> Empieza = (Padre);
            NodoCP<T> Temporal = new NodoCP<T>();
            Temporal.Data = Nuevo;
            int Cambio = Convert.ToInt32(Comparacion.DynamicInvoke(Padre.Data, Temporal.Data));
            if (Cambio > 0)
            {
                Temporal.Siguiente = Padre;
                (Padre) = Temporal;
            }
            else
            {
                Cambio = Convert.ToInt32(Comparacion.DynamicInvoke(Empieza.Siguiente.Data, Temporal.Data));
                while (Empieza.Siguiente != null && Cambio < 0)
                {
                    Empieza = Empieza.Siguiente;
                }
                Temporal.Siguiente = Empieza.Siguiente;
                Empieza.Siguiente = Temporal;
            }
            return Temporal;
        }
        public static int isEmpty(NodoCP<T> head)
        {
            return ((head) == null) ? 1 : 0;
        }
    }
}
