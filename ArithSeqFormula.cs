using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    internal class Program
    {
        //Funcion para cambiar el color del texto

        static void ConColor(ConsoleColor color)
        {
            Console.ForegroundColor = color;
        }

        static void Main(string[] args)
        {
            //Propiedades de la consola

            Console.Title = "Sucesion numerica en C# por Starlyn1232";
            Console.WindowHeight = 20;
            Console.WindowWidth = 60;
            ConColor(ConsoleColor.White);

            //Verificador de numeros

            char[] valid = {'0','1','2', '3', '4', '5', '6', '7', '8', '9' };

            //Una variable temporal

            string data = "";

            //Tenemos una collecion de numeros

            List<int> numbers = new List<int>();

            ConColor(ConsoleColor.Blue);
            Console.WriteLine("\nBienvenidos al analizador de sucesiones numericas: ");
            ConColor(ConsoleColor.DarkGray);
            Console.WriteLine("\n(Escribe \"listo\" para salir)");
            ConColor(ConsoleColor.Yellow);
            Console.Write("\nIntroduce al menos 4 numeros para deducir el patron: ");

            //Leer la informacion (Si escriben "listo" dejaremos de guardar numeros)

            ConColor(ConsoleColor.White);
            while ((data = Console.ReadLine()) != "listo")
            {
                //Verificamos el detalle

                foreach(char c in data)
                {
                    foreach (char c2 in valid)
                    {
                        //Es un numero, salimos

                        if (c2 == c)
                            break;

                        //Si llegamos al final, entonces no fue un numero, y cerramos la app

                        if (c2 == valid[valid.Length - 1])
                        {
                            ConColor(ConsoleColor.Red);
                            Console.WriteLine(String.Format("\n\aValor invalido detectado! [{0}]", data));
                            Console.ReadKey();
                            return;
                        }
                    }
                }

                //Guardamos el numero

                if(data != String.Empty)
                    numbers.Add(Convert.ToInt32(data));

                //Texto para tomar el proximo numero

                ConColor(ConsoleColor.Yellow);
                Console.Write(String.Format("Escriba el siguiente numero (Cantidad : {0}) : ", numbers.Count));
                ConColor(ConsoleColor.White);
            }

            //Determinar el patron (Necesitamos al menos 4 valores para sacar la conclusion)

            if(numbers.Count < 4)
            {
                ConColor(ConsoleColor.Red);
                Console.WriteLine("\nHay que escribir al menos 4 valores para analizarlos!");
                ConColor(ConsoleColor.Yellow);
                Console.Write(String.Format("\n\aValores introducidos: {0}", numbers.Count));
                Console.ReadKey();
                return;
            }

            //Preparamos una coleccion de enteros para guardar el analisis y luego compararlo

            List<int> temp = new List<int>();

            //Generamos los datos

            for (int i = numbers.Count - 1; i >= 0; i--)
            {
                //Verificaremos todos los datos, menos el ultimo (Porque "no podemos" restar la posicion 0 a la posicion, -1? Nop)

                //Guardamos resultados

                if (i != 0)
                    temp.Add(numbers[i] - numbers[i - 1]);

            }

            bool pattern_found = true;

            //Empezaremos desde la posicion 1 (La segunda) porque usaremos el valor 9 (primero) como el valor a comparar
            //De todas formas, de encontrar el patron, todas deberias ser iguales

            for(int i = 1;i < temp.Count; i++)
                if (temp[i] != temp[0]) { pattern_found = false; break; }

            //Si no hay errores, el patron es encontrado, damos detalles

            if (pattern_found)
            {
                //Mostramos la relacion entre ellos

                ConColor(ConsoleColor.Green);
                Console.WriteLine("\nPatron encontrado!");

                ConColor(ConsoleColor.Yellow);
                Console.Write("\nFormula: "); //An = termino n, A1 = primer termine, n = posicion del termino, d = diferencia comun
                ConColor(ConsoleColor.White);
                Console.Write("An = A1 + (n-1)d");

                ConColor(ConsoleColor.Blue);
                Console.WriteLine(String.Format("\n\nN = {0}", temp[0]));
                ConColor(ConsoleColor.Yellow);
                Console.WriteLine(String.Format("\nVariables introducidas: \n"));

                ConColor(ConsoleColor.White);
                for (int i = 0;i < numbers.Count; i++)
                {
                    Console.Write(numbers[i]);

                    if (i < numbers.Count - 1)
                        Console.Write(",");
                }

                //Simulamos una continuacion de 3 valores mas

                ConColor(ConsoleColor.Yellow);
                Console.WriteLine("\n\nLa secuencia continuaria: \n");

                int[] final = { numbers[numbers.Count - 1], (numbers[numbers.Count - 1] + (temp[0] * 3)) };

                ConColor(ConsoleColor.White);
                while (final[0] <= final[1])
                {
                    Console.Write(final[0]);

                    if (final[0] != final[1])
                        Console.Write(",");

                    final[0] += temp[0];
                }

                Console.WriteLine(String.Format(""));
            }

            else
            {
                ConColor(ConsoleColor.Red);
                Console.WriteLine("\nEl patron no pudo ser encontrado... (No aparece Pablo)");
            }

            Console.ReadKey();
            return;
        }
    }
}
