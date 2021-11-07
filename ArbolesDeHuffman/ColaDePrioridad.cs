using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbolesDeHuffman
{
    public class ColaDePrioridad <T> where T : IComparable
    {
        public  NodoCP<T> NodoCPPadre = new NodoCP<T>();
        public  NodoCP<T> Ultima = new NodoCP<T>();
        public T Enviar;

        public void ConstruirArbol(NodoCP<T> Head,Delegate Comparacion, Delegate Suma, Delegate CompFinalizacion) 
        {
            if (isEmpty(Head) == 0) 
            {
                int i = Convert.ToInt32(CompFinalizacion.DynamicInvoke(Head.Data));
                if (i != 0) 
                {
                    NodoCP<T> HijoIzquierdo = new NodoCP<T>();
                    NodoCP<T> HijoDerecho = pop(Head, Comparacion);
                    Heap(NodoCPPadre, Comparacion);

                    if (isEmpty(Head) == 0)
                    {

                        HijoIzquierdo = pop(Head, Comparacion);
                        Heap(NodoCPPadre, Comparacion);
                    }
                    NodoCP<T> NodoNuevo = new NodoCP<T>();
                    //Se debe de hacer una suma
                    NodoNuevo.Data = (T)Convert.ChangeType(Suma.DynamicInvoke(HijoDerecho.Data, HijoIzquierdo.Data), typeof(T));

                    NodoNuevo.Derecha = HijoDerecho;
                    NodoNuevo.Izquierda = HijoIzquierdo;

                    NodoNuevo.Letra = false;

                    push(NodoCPPadre, NodoNuevo);
                    Heap(NodoCPPadre, Comparacion);

                    ConstruirArbol(NodoCPPadre, Comparacion, Suma, CompFinalizacion);
                }
            }

        }

        string Codificacion = "";
        public  void printCode(NodoCP<T> root, string s, Delegate Compa)
        {
            if (isEmpty(root.Izquierda) == 1  && isEmpty(root.Derecha) == 1)
            {
                root.Letra = true;
                root.Data = (T)Convert.ChangeType(Compa.DynamicInvoke(root.Data,s), typeof(T));
                return;
            }
            printCode(root.Izquierda, s + "0",Compa);
            printCode(root.Derecha, s + "1", Compa);
        }

        public void printCodeLate(NodoCP<T> root, char s, Delegate Compa)
        {
            if (isEmpty(root.Izquierda) == 1 && isEmpty(root.Derecha) == 1)
            {
                if (isEmpty(root) == 0) 
                {
                    int Comparacion = Convert.ToInt32(Compa.DynamicInvoke(root.Data, s));
                    if (Comparacion == 0) 
                    {
                        Enviar = root.Data;
                    }
                    return;
                }
            }
            printCodeLate(root.Izquierda, s, Compa);
            printCodeLate(root.Derecha, s, Compa);
        }

        public NodoCP<T> newNode(T Nuevo)
        {
            NodoCP<T> temp = new NodoCP<T>();
            temp.Data = Nuevo;
            temp.Letra = true;
            temp.Siguiente = null;
            return temp;
        }


        public  NodoCP<T> pop(NodoCP<T> head, Delegate Iguales) //Enviar El Primero
        {
            NodoCP<T> Aux = new NodoCP<T>();
            Aux.Data = head.Data;
            Aux.Derecha = head.Derecha;
            Aux.Izquierda = head.Izquierda;

            head.Data = Ultima.Data;
            head.Derecha = Ultima.Derecha;
            head.Izquierda = Ultima.Izquierda;

            Eliminar(head, Ultima, Iguales);
            return Aux;
        }
        public void Eliminar(NodoCP<T> NodoPrincipal, NodoCP<T> Search, Delegate Iguales)
        {
            if (isEmpty(NodoPrincipal.Siguiente) == 0) 
            {
                int ig = Convert.ToInt32(Iguales.DynamicInvoke(NodoPrincipal.Siguiente.Data, Search.Data));
                if (ig == 0)
                {
                    NodoPrincipal.Siguiente = null;
                    Ultima = NodoPrincipal;
                }
                else
                {
                    Eliminar(NodoPrincipal.Siguiente, Search, Iguales);
                }
            }
            else 
            {
                NodoCPPadre = null;
            }
        }
        public void push(NodoCP<T> Padre, NodoCP<T> Nuevo) //Insertar uno nuevo 
        {
            if(isEmpty(Padre) == 1) 
            {
                NodoCPPadre = Nuevo;
            }
            else if (isEmpty(Padre.Siguiente) == 1)
            {
                Padre.Siguiente = Nuevo;
                Ultima = Padre.Siguiente;
            }
            else 
            {
                push(Padre.Siguiente, Nuevo);
            }
        }

        public void Heap(NodoCP<T> Padre, Delegate Menor) 
        {
            NodoCP<T> Nuevo = Padre;
            while (isEmpty(Nuevo)== 0) 
            {
                if (isEmpty(Nuevo) == 0)
                {
                    if (isEmpty(Nuevo.Siguiente) == 0) 
                    {
                        int Comparacion = Convert.ToInt32(Menor.DynamicInvoke(Nuevo.Data, Nuevo.Siguiente.Data));
                        if (Comparacion > 0)
                        {
                            T Aux = Nuevo.Data;
                            NodoCP<T> DerechaPequeño = Nuevo.Derecha;
                            NodoCP<T> IzquierdoPequeño = Nuevo.Izquierda;
                            Nuevo.Data = Nuevo.Siguiente.Data;
                            Nuevo.Derecha = Nuevo.Siguiente.Derecha;
                            Nuevo.Izquierda = Nuevo.Siguiente.Izquierda;
                            Nuevo.Siguiente.Data = Aux;
                            Nuevo.Siguiente.Derecha = DerechaPequeño;
                            Nuevo.Siguiente.Izquierda = IzquierdoPequeño;
                        }
                        else
                        {
                            Nuevo = Padre.Siguiente;
                        }
                    }
                }
            }
        }

        public  int isEmpty(NodoCP<T> head)
        {
            if (head!=null)
            {
                return ((head.Data) == null) ? 1 : 0;
            }
            return 1;
        }
    }
}
