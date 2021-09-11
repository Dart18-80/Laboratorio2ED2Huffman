﻿using System;
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

        public void ConstruirArbol(NodoCP<T> Head,Delegate Comparacion, Delegate Suma) 
        {
            if (isEmpty(Head) == 0) 
            {
                if (isEmpty(Head.Siguiente) == 0)
                {
                    NodoCP<T> HijoDerecho = pop(Head);
                    Heap(NodoCPPadre, Comparacion);

                    NodoCP<T> HijoIzquierdo = pop(Head);
                    Heap(NodoCPPadre, Comparacion);

                    NodoCP<T> NodoNuevo = new NodoCP<T>();
                    //Se debe de hacer una suma
                    NodoNuevo.Data = (T)Convert.ChangeType(Suma.DynamicInvoke(HijoDerecho.Data,HijoIzquierdo.Data),typeof(T));

                    NodoNuevo.Derecha = HijoDerecho;
                    NodoNuevo.Izquierda = HijoIzquierdo;

                    NodoNuevo.Letra = false;

                    push(NodoCPPadre, NodoNuevo);
                    Heap(NodoCPPadre, Comparacion);

                    ConstruirArbol(NodoCPPadre, Comparacion, Suma);
                }
                else 
                {
                    // Arbol de Huffman Terminado
                }
            }

        }

        string Codificacion = "";
        public  void printCode(NodoCP<T> root, string s, Delegate Compa)
        {
            if (root.Izquierda == null && root.Derecha == null && root.Letra == true)
            {
                root.Data = (T)Convert.ChangeType(Compa.DynamicInvoke(root.Data,s), typeof(T));
                return;
            }
            printCode(root.Izquierda, s + "0",Compa);
            printCode(root.Derecha, s + "1", Compa);
        }

        public NodoCP<T> newNode(T Nuevo)
        {
            NodoCP<T> temp = new NodoCP<T>();
            temp.Data = Nuevo;
            temp.Letra = true;
            temp.Siguiente = null;
            return temp;
        }


        public  NodoCP<T> pop(NodoCP<T> head) //Enviar El Primero
        {
            NodoCP<T> Aux = head;
            head.Data = Ultima.Data;
            head.Derecha = Ultima.Derecha;
            head.Izquierda = Ultima.Izquierda;
            Aux.Siguiente = null;
            return Aux;
        }

        public void push(NodoCP<T> Padre, NodoCP<T> Nuevo) //Insertar uno nuevo 
        {
            NodoCP<T> Temporal = Padre;
            if (isEmpty(Padre.Siguiente) == 1)
            {
                Padre.Siguiente = Nuevo;
                Ultima = Nuevo;
            }
            else 
            {
                push(Padre.Siguiente, Nuevo);
            }
        }

        public void Heap(NodoCP<T> Padre, Delegate Menor)
        {
            if (isEmpty(Padre) == 0)
            {
                if (isEmpty(Padre.Siguiente) == 0)
                {
                    int Comparacion = Convert.ToInt32(Menor.DynamicInvoke(Padre.Data, Padre.Siguiente));
                    if (Comparacion > 0)
                    {
                        T Aux = Padre.Data;
                        NodoCP<T> DerechaPequeño = Padre.Derecha;
                        NodoCP<T> IzquierdoPequeño = Padre.Izquierda;
                        Padre.Data = Padre.Siguiente.Data;
                        Padre.Derecha = Padre.Siguiente.Derecha;
                        Padre.Izquierda = Padre.Siguiente.Siguiente;
                        Padre.Siguiente.Data = Aux;
                        Padre.Siguiente.Derecha = DerechaPequeño;
                        Padre.Siguiente.Izquierda = IzquierdoPequeño;
                        Heap(NodoCPPadre, Menor);
                    }
                    else
                    {
                        Heap(NodoCPPadre.Siguiente, Menor);
                    }
                }
            }
        }

        public  int isEmpty(NodoCP<T> head)
        {
            return ((head) == null) ? 1 : 0;
        }
    }
}
