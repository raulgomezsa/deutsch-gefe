/// <summary>
/// Gast 5 - Mujer rubia sentada en la barra
/// </summary>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace ClienteCafeteria
{
    class Program
    {
        public static Bot bot;
        public static bool conectado;
        public static ControlDialogo dialogo;
        public static Configurador configurador = new Configurador();
        public static Diccionario diccionario = new Diccionario();
        public static Vector3 posCamarero;
        public static Consola consola;
        public static Temporizador tiempo = new Temporizador();
        private static int CANAL_BOT2 = 5553;

        static void Main(string[] args)
        {
            conectado = false;
            bot = new Bot();
            // Cargamos sus datos de conexión y el diccionario.
            configurador.cargarDatosConexion();
            configurador.cargarDiccionario();
            consola = new Consola();
            // Una vez cargados los datos, conectamos al servidor.
            bot.conectar();
            consola.escribeEvento("Conectando ... ");
            // Esperamos a que se conecte y lo sentamos.
            consola.escribeEvento("Bot conectado como: " + bot.nombre + " " + bot.apellido);
            conectado = true;
            //Activamos control de eventos de chat publico y mensajes instantaneos
            bot.cliente.Self.ChatFromSimulator += new EventHandler<ChatEventArgs>(Self_ChatFromSimulator);
            bot.cliente.Self.IM += new EventHandler<InstantMessageEventArgs>(Self_IM);
            // Envio mensaje para solicitar UUID de la silla.
            /*System.Threading.Thread.Sleep(6000);
            Program.bot.cliente.Self.Chat("dame_id_silla", 15, OpenMetaverse.ChatType.Normal);
            System.Threading.Thread.Sleep(2000);
            bot.sentarse();*/

            // Rotamos la posicion del bot y lo sentamos.
            Program.bot.rotar();
            System.Threading.Thread.Sleep(2000);
            bot.sentarseEnTierra();

            consola.loop();
            bot.desconectar();
            consola.escribeEvento("Bot desconectado. Pulse INTRO para salir.");
            Console.ReadLine();
        }

        static public void Self_ChatFromSimulator(object sender, OpenMetaverse.ChatEventArgs e)
        {
            posCamarero = e.Position;
            dialogo = new ControlDialogo(e, calcularDistancia());
            
            if (bot.cliente.Self.AgentID != e.OwnerID)
            {
                dialogo.responder();
            }
        }

        static void Self_IM(object sender, InstantMessageEventArgs e)
        {
            if (e.IM.Message == "acierto")
            {
                System.Threading.Thread.Sleep(3000);
                Program.bot.decir("Vielen Dank " + Program.bot.nombre_camarero);
                System.Threading.Thread.Sleep(3000);
                Program.bot.cliente.Self.Chat("servido", CANAL_BOT2, OpenMetaverse.ChatType.Shout);

                System.Threading.Thread.Sleep(1000);
                Program.bot.ejecutarGesto(Program.bot.CELEBRACION);
                Program.consola.escribeMensaje("El camarero ha realizado la entrega bien.");
                //Program.bot.estado = 0;
                Program.bot.dejar_atender(); // Deja libre al bot.
            }
            else if (e.IM.Message == "fallo")
            {
                System.Threading.Thread.Sleep(3000);
                Program.bot.decir("Vielen Dank " + Program.bot.nombre_camarero + ", aber die Bestellung ist nicht korrekt.");
                System.Threading.Thread.Sleep(3000);
                Program.bot.cliente.Self.Chat("servido", CANAL_BOT2, OpenMetaverse.ChatType.Shout);

                System.Threading.Thread.Sleep(1000);
                Program.bot.ejecutarGesto(Program.bot.NO);
                Program.consola.escribeMensaje("El camarero ha realizado la entrega mal.");
                Program.bot.dejar_atender(); // Deja libre al bot.
            }
            else
            {
                string csv = e.IM.Message;
                string[] parts = csv.Split(',');

                int tam = parts.GetLength(0);

                if (tam > 1)
                {
                    if (parts[0] == "start")
                    {
                        Program.bot.activo = true;
                        Program.bot.setCamarero((UUID)parts[1], parts[2]);
                        Program.bot.gritar(Program.bot.nombre_camarero + " Entschuldige bitte!");
                        while (Program.bot.estado != 1 && Program.bot.activo != false)
                        {
                            Program.bot.cliente.Self.AnimationStart(Animations.HELLO, true);
                            System.Threading.Thread.Sleep(1000);
                            Program.bot.cliente.Self.AnimationStop(Animations.HELLO, true);
                            System.Threading.Thread.Sleep(1000);
                        }
                    }
                    else if (parts[0] == "resetear")
                    {
                        Program.bot.activo = false;
                        Program.bot.decir(Program.bot.nombre_camarero + " Jetzt habe ich keinen Appetit mehr!!!");
                        Program.bot.dejar_atender();
                    }
                }
                else
                {
                    Program.bot.silla = new UUID(e.IM.Message);
                }
            }
        }

        static int calcularDistancia()
        {
            Vector2 posicionBot = new Vector2(Program.bot.cliente.Self.SimPosition.X, Program.bot.cliente.Self.SimPosition.Y);
            Vector2 posicionCamarero = new Vector2(posCamarero.X, posCamarero.Y);

            return (int)(Math.Abs(Math.Sqrt(Math.Pow(posicionBot.X - posicionCamarero.X, 2) + Math.Pow(posicionBot.Y - posicionCamarero.Y, 2))));
        }
    }
}
