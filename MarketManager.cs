using System;
using System.Collections;
using System.Threading;

namespace ConsoleApp7
{
    internal class Program
    {
        internal class CProducto
        {
            private string description;
            private string id;
            private double precio;

            public CProducto()
            {

            }

            public CProducto(string Descripcion, string _ID, double _precio)
            {
                description = Descripcion;
                id = _ID;
                precio = _precio;
            }

            public string Description
            {
                get => description;
            }

            public string ID
            {
                get => id;
            }

            public double Precio
            {
                get => precio;
            }
        }

        internal class CMercado
        {
            private string nombreEmpresa;
            private string owner;
            private ArrayList produtos = new ArrayList();

            public CMercado()
            {
                Console.WriteLine("\n== Bienvenidos a su mercado digital ==");

                Console.Write("\nIntroduzca el nombre de la compañia: ");
                nombreEmpresa = Console.ReadLine();

                Console.Write("Introduzca el nombre del dueño: ");
                owner = Console.ReadLine();
            }

            private void Teclear()
            {
                Console.WriteLine("\n\nPresione Enter para volver.");
                Console.ReadKey();
            }

            private void Limpiar()
            {
                Console.Clear();
                Console.WriteLine("\n== Bienvenidos a su mercado digital ==");
            }

            public void Saludo()
            {
                Console.WriteLine("\nBuenos dias {0}, \"{1}\" esta mejor que nunca.\n\nProductos registrados: {2}", owner, nombreEmpresa, produtos.Count);
            }

            public void IngresarProducto()
            {
                Limpiar();

                bool checkCount()
                {
                    if (produtos.Count > 0)
                        return true;

                    else
                        return false;
                }

                string name = "";
                string id = "";
                double precio = -1;

                ArrayList[] completeData = new ArrayList[3];

                for (int i = 0; i < completeData.Length; i++)
                    completeData[i] = new ArrayList();

                //Unico analisis para comparar datos

                if (checkCount())
                {
                    foreach (CProducto produto in produtos)
                    {
                        completeData[0].Add(produto.Description);
                        completeData[1].Add(produto.ID);
                        completeData[2].Add(produto.Precio);
                    }
                }

                Console.Write("\nAgregando nuevo producto al inventario.");

                Console.Write("\n\nNombre del producto: ");
                name = Console.ReadLine();

                if (name == "")
                {
                    Console.WriteLine("\nDebes introducir un valor valido!");
                    Teclear();
                    return;
                }

                else
                {
                    if (checkCount())
                    {
                        if (completeData[0].Contains(name))
                        {
                            Console.WriteLine("\nYa hay un articulo con el mismo nombre.");
                            Console.Write("\nDeseas continuar de todos modos? (si/no): ");

                            string respuesta = Console.ReadLine();

                            if (respuesta.ToLower() != "si")
                                return;

                            else
                                Console.Write("\n");
                        }
                    }
                }

                Console.Write("ID: ");

                id = Console.ReadLine();

                if (id == "")
                {
                    Console.WriteLine("\nDebes introducir un valor valido!");
                    Teclear();
                    return;
                }

                else
                {
                    if (checkCount())
                    {
                        if (completeData[1].Contains(id))
                        {
                            CProducto tempProduct = null;

                            foreach (CProducto producto in produtos)
                            {
                                if (id == producto.ID)
                                {
                                    tempProduct = producto;
                                    break;
                                }
                            }

                            Limpiar();
                            Console.WriteLine("\nEl ID \"{0}\" ya esta en uso!\n", id);
                            Console.Write("Descripcion: {0}\nID: {1}\nPrecio: ${2} DOP", tempProduct.Description, tempProduct.ID, tempProduct.Precio);
                            Teclear();
                            return;
                        }
                    }
                }

                Console.Write("Precio: $");
                precio = Convert.ToDouble(Console.ReadLine());

                if (precio < 0)
                {
                    Console.WriteLine("\nDebes introducir un valor valido!");
                    Teclear();
                    return;
                }

                Limpiar();

                produtos.Add(new CProducto(name, id, precio));

                Console.Write("\nProducto Agregado.\n\nProducto: {0} \nID: {1} \nPrecio: ${2} DOP ", name, id, precio);

                Teclear();
            }

            public void BuscarProducto()
            {
                Limpiar();

                if (produtos.Count == 0)
                {
                    Console.Write("\nErro: No se han registrado articulos.");
                    Teclear();
                    return;
                }

                Console.Write("\nIntroduce el ID del producto: ");

                string busqueda = Console.ReadLine();

                foreach (CProducto producto in produtos)
                {
                    if (producto.ID == busqueda)
                    {
                        Limpiar();
                        Console.WriteLine("\nProducto encontrado!\n\nDetalles:\n");

                        Console.Write("Descripcion: {0}\nID: {1}\nPrecio: ${2} DOP", producto.Description, producto.ID, producto.Precio);
                        Teclear();
                        return;
                    }
                }

                Console.Write("\nProducto no encontrado!");

                Teclear();
            }
        }

        static void Main(string[] args)
        {
            //Propiedades de ventana

            Console.ForegroundColor = ConsoleColor.White;
            Console.Title = "Market Manager v1.0 by Starlyn1232";

            //Variables

            string[] data = new string[2];
            string option = "";

            //Entry Input

            CMercado mercado = new CMercado();

            //Menu

            while(option != "3")
            {
                Console.Clear();
                Console.WriteLine("\n== Bienvenidos a su mercado digital ==");
                mercado.Saludo();

                Console.WriteLine("\n1-Ingresar producto.");
                Console.WriteLine("2-Buscar producto.");
                Console.WriteLine("3-Cerrar negocio.");
                Console.Write("\nEliga una opcion: ");
                option = Console.ReadLine();

                //Operations

                if(option == "1")
                {
                    mercado.IngresarProducto();
                }

                if (option == "2")
                {
                    mercado.BuscarProducto();
                }
            }

            Console.WriteLine("\nHasta pronto!");
            Thread.Sleep(2000);
        }
    }
}
