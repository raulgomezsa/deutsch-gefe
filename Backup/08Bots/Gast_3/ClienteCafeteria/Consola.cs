using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace ClienteCafeteria
{
    class Consola
    {
        private string comando = "";
        private bool quit_flag = false;
        private bool forcequit = false;

        public void loop()
        {
            escribeMensaje("Linea de comando activa.");
            Console.Write((char)8);  //Retroceso
            while (!quit_flag)
            {
                if (forcequit)
                {
                    quit_flag = true;
                }
                Console.Write('>');
                comando = Console.ReadLine();
                switch (comando)
                {
                    case "exit":
                        quit_flag = true;
                        break;
                    case "sentar":
                        escribeMensaje("Sentando al bot...");
                        Console.Write((char)8);
                        Program.bot.sentarse();
                        break;
                    case "levantar":
                        escribeMensaje("Levantando al bot...");
                        Console.Write((char)8);
                        Program.bot.levantarse();
                        break;
                    case "saludar":
                        escribeMensaje("El bot esta saludando...");
                        Console.Write((char)8);
                        Program.bot.ejecutarGesto(Program.bot.SALUDO);
                        break;
                    case "camarero":
                        Program.bot.getCamarero();
                        Console.Write((char)8);
                        break;
                    case "liberar":
                        Program.bot.dejar_atender();
                        break;
                    case "pedido":
                        Program.bot.generarPedido();
                        Program.bot.decir(Program.bot.pedido);
                        Console.Write((char)8);
                        break;
                    case "help":
                        escribeMensaje("Lista de comandos disponibles:");
                        Console.Write((char)8);
                        escribeMensaje("pedido - Genera un pedido y lo imprime por chat.");
                        Console.Write((char)8);
                        escribeMensaje("sentar - Sienta al bot.");
                        Console.Write((char)8);
                        escribeMensaje("saludar- El bot ejecuta el gesto de saludo.");
                        Console.Write((char)8);
                        escribeMensaje("levantar - Levanta del asiento al bot.");
                        Console.Write((char)8);
                        escribeMensaje("camarero - Muestra el nombre del camarero que le esta atendiendo.");
                        Console.Write((char)8);
                        escribeMensaje("exit - Cierra el programa.");
                        Console.Write((char)8);
                        escribeMensaje("help - Muestra la lista de comandos.");
                        Console.Write((char)8);
                        break;
                    default:
                        if (comando != "")
                        {
                            escribeError("Comando \"" + comando + "\" no reconocido.");
                            Console.Write((char)8);
                        }
                        break;
                }
            }
        }


        // Escribe un mensaje en la consola.
        public void escribeMensaje(string mensaje)
        {
            imprimirNombreBot();
            Console.Write((char)8);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("[" + DateTime.Now.ToShortTimeString() + ":" + DateTime.Now.Second + "] " + "[");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Mensaje");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("] - " + mensaje);
            Console.Write('>');
        }

        // Escribe un mensaje de error en la consola.
        public void escribeError(string mensaje)
        {
            imprimirNombreBot();
            Console.Write((char)8);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("[" + DateTime.Now.ToShortTimeString() + ":" + DateTime.Now.Second + "] " + "[");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Error");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("] - " + mensaje);
            Console.Write('>');
        }

        // Escribe un mensaje de evento en la consola
        public void escribeEvento(string mensaje)
        {
            imprimirNombreBot();
            Console.Write((char)8);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("[" + DateTime.Now.ToShortTimeString() + ":" + DateTime.Now.Second + "] " + "[");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Evento");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("] - " + mensaje);
        }

        // Fuerza la finalización del programa
        public void forceQuit()
        {
            forcequit = true;
        }
        public void imprimirNombreBot()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("[" + Program.bot.cliente.Self.Name + "]  ");
        }
    }
}
