using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClienteCafeteria
{
    class Configurador
    {
        // Fichero de configuración de los datos de login "config.ini"
        private System.IO.FileInfo fichConfig = new System.IO.FileInfo("./config.ini");

        // Fichero de palabras que reconoce el bot "diccionario.ini"
        private System.IO.FileInfo fichDiccionario = new System.IO.FileInfo("./diccionario.ini");

        // El flujo de datos de los ficheros.
        private System.IO.FileStream flujo;

        // Un lector de texto para el flujo.
        private System.IO.StreamReader fichero;

        // La línea que se ha leído.
        private string linea = "";

        // Lee de un fichero de configuracion los datos de conexión. El fichero debe estar codificado en UTF-8
        public bool cargarDatosConexion()
        {
            if (fichConfig.Exists)
            {
                int numDatos = 0;
                flujo = fichConfig.OpenRead();
                fichero = new System.IO.StreamReader(flujo, Encoding.UTF8);
                while (!fichero.EndOfStream)
                {
                    linea = fichero.ReadLine();
                    if (linea.Length != 0 && linea[0] != ';')
                    {
                        // Busca la posición del igual y guarda en un string una mitad y en otro la otra.
                        string parte1 = "", parte2 = "";
                        int posIgual = localizarIgual(linea);
                        for (int j = 0; j < posIgual; j++)
                        {
                            parte1 += linea[j];
                        }
                        for (int j = posIgual + 1; j < linea.Length; j++)
                        {
                            parte2 += linea[j];
                        }
                        // Según lo que valga la parte 1 le asigna la parte 2 a una variable u otra.
                        switch (parte1)
                        {
                            case "nombre":
                                Program.bot.nombre = parte2;
                                numDatos++;
                                break;
                            case "apellido":
                                Program.bot.apellido = parte2;
                                numDatos++;
                                break;
                            case "contraseña":
                                Program.bot.password = parte2;
                                numDatos++;
                                break;
                            case "servidor":
                                Program.bot.servidor = parte2;
                                numDatos++;
                                break;
                        }
                    }
                }
                fichero.Close();
                if (numDatos == 4)
                {
                    return (true); // Ha leido todos los datos.
                }
                else
                {
                    return (false); // Significa que faltan datos.
                }
            }
            else
            {
                return (false); // El fichero no se ha podido abrir.
            }
        }

        // Devuelve el índice del primer "=" que encuentre en una cadena.
        private int localizarIgual(string cadena)
        {
            for (int i = 0; i < cadena.Length; i++)
            {
                if (cadena[i] == '=')
                    return (i);
            }
            return (-1);
        }


        // Carga el diccionario de palabras que va a reconocer el bot.

        public bool cargarDiccionario()
        {
            if (fichDiccionario.Exists)
            {
                int numDatos = 0;
                flujo = fichDiccionario.OpenRead();
                fichero = new System.IO.StreamReader(flujo, Encoding.UTF8);
                while (!fichero.EndOfStream)
                {
                    linea = fichero.ReadLine();
                    if (linea.Length != 0 && linea[0] != ';')
                    {
                        // Busca la posición del igual y guarda en un string una mitad y en otro la otra.
                        string parte1 = "", parte2 = "";
                        int posIgual = localizarIgual(linea);
                        for (int j = 0; j < posIgual; j++)
                        {
                            parte1 += linea[j];
                        }
                        for (int j = posIgual + 1; j < linea.Length; j++)
                        {
                            parte2 += linea[j];
                        }
                        // Según lo que valga la parte 1 le asigna la parte 2 a una variable u otra.
                        switch (parte1)
                        {
                            case "productos_grasos": // 1
                                Program.diccionario.productos_grasos = procesaLista(parte2);
                                numDatos++;
                                break;
                            case "productos_grasos_formal": // 2
                                Program.diccionario.productos_grasos_formal = procesaLista(parte2);
                                numDatos++;
                                break;
                            case "productos_sanos": // 3
                                Program.diccionario.productos_sanos = procesaLista(parte2);
                                numDatos++;
                                break;
                            case "productos_sanos_formal": // 4
                                Program.diccionario.productos_sanos_formal = procesaLista(parte2);
                                numDatos++;
                                break;
                            case "bebidas": // 5
                                Program.diccionario.bebidas = procesaLista(parte2);
                                numDatos++;
                                break;
                            case "bebidas_formal": // 6
                                Program.diccionario.bebidas_formal = procesaLista(parte2);
                                numDatos++;
                                break;
                            case "sinonimos_saludos": // 7
                                Program.diccionario.sinonimos_saludos = procesaLista(parte2);
                                numDatos++;
                                break;
                            case "sinonimos_estado": // 8
                                Program.diccionario.sinonimos_estado = procesaLista(parte2);
                                numDatos++;
                                break;
                            case "sinonimos_desear": // 9
                                Program.diccionario.sinonimos_desear = procesaLista(parte2);
                                numDatos++;
                                break;
                            case "sinonimos_repetir": // 10
                                Program.diccionario.sinonimos_repetir = procesaLista(parte2);
                                numDatos++;
                                break;
                            case "sinonimos_gracias": // 11
                                Program.diccionario.sinonimos_gracias = procesaLista(parte2);
                                numDatos++;
                                break;
                            case "sinonimos_adios": // 12
                                Program.diccionario.sinonimos_adios = procesaLista(parte2);
                                numDatos++;
                                break;
                            case "sinonimos_insultos_graciosos": // 13
                                Program.diccionario.sinonimos_insultos_graciosos = procesaLista(parte2);
                                numDatos++;
                                break;
                            case "sinonimos_insultos_graves": // 14
                                Program.diccionario.sinonimos_insultos_graves = procesaLista(parte2);
                                numDatos++;
                                break;
                            case "sinonimos_piropos": // 15
                                Program.diccionario.sinonimos_piropos = procesaLista(parte2);
                                numDatos++;
                                break;
                            case "sinonimos_ligar_descarados": // 16
                                Program.diccionario.sinonimos_ligar_descarado = procesaLista(parte2);
                                numDatos++;
                                break;
                            case "sinonimos_ligar_indiscretos": // 17
                                Program.diccionario.sinonimos_ligar_indiscreto = procesaLista(parte2);
                                numDatos++;
                                break;
                            case "sinonimos_esperar": // 18
                                Program.diccionario.sinonimos_esperar = procesaLista(parte2);
                                numDatos++;
                                break;
                            case "sinonimos_vale": // 19
                                Program.diccionario.sinonimos_vale = procesaLista(parte2);
                                numDatos++;
                                break;
                            case "sinonimos_denada": // 20
                                Program.diccionario.sinonimos_denada = procesaLista(parte2);
                                numDatos++;
                                break;
                            case "sinonimos_disculpa": // 20
                                Program.diccionario.sinonimos_disculpas = procesaLista(parte2);
                                numDatos++;
                                break;
                        }
                    }
                }
                // Los productos totales seran la concatenacion de las dos listas de productos grasos y sanos.
                Program.diccionario.productos.AddRange(Program.diccionario.productos_grasos);
                Program.diccionario.productos.AddRange(Program.diccionario.productos_sanos);
                // Lo mismo para los productos formales.
                Program.diccionario.productos_formal.AddRange(Program.diccionario.productos_grasos_formal);
                Program.diccionario.productos_formal.AddRange(Program.diccionario.productos_sanos_formal);
                fichero.Close();
                if (numDatos == 20)
                    return (true); // Significa que no ha habido problemas.
                else
                    return (false);
            }
            else
            {
                return (false); // El fichero no se ha podido abrir.
            }
        }

        // Se pasa una cadena con elementos separados por ',' y devuelve una lista con los elementos insertados.
        private List<string> procesaLista(string cadena)
        {
            List<string> lista = new List<string>();
            string[] cadena_separada = cadena.Split(',');
            for (int i = 0; i < cadena_separada.Length; i++)
            {
                lista.Add(cadena_separada[i]);
            }
            return lista;
        }
    }
}