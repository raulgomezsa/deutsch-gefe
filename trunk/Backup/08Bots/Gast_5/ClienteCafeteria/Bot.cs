using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace ClienteCafeteria
{
    class Bot
    {
        // Constantes

        const int NRO_COMIDAS = 2; // Nº de platos de comida
        const int NRO_BEBIDAS = 1; // Nº de bebidas
        const int CANALCAMARERO = 5;

        // Gestos predefinidos en opensim
        public UUID SALUDO = new UUID("cce0e317-2c49-411e-8716-f9ce3007c715");
        public UUID SI = new UUID("392c292f-3d27-45ff-9437-c79c06699237");
        public UUID NO = new UUID("6c123970-0f5a-448e-920a-07bae9aadf4c");
        public UUID APLAUSO = new UUID("712e81fd-a215-498c-ab1a-caf1f5bf950d");
        public UUID RISA = new UUID("d545ac78-09cc-4811-8700-8df1a37d7f56");
        public UUID NOENTIENDO = new UUID("9bc46cd2-95cb-456d-9070-a4439e42af9e");
        public UUID CELEBRACION = new UUID("7f7384c0-848c-4bf0-8e83-50879981e1a4");
        public UUID ABURRIDO = new UUID("5954170e-e6bc-44f6-96a9-2c02a382165a");
        public UUID WINK = new UUID("67d47cd0-9634-4c99-97db-ddce9bda467c");
        public UUID YO = new UUID("652475bc-ffb7-4a18-b6bb-7731ddeb6a51");
        public UUID SORPRENDIDO = new UUID("dbaf104b-cba8-4df7-b5d3-0cb57d0d63b2");
        public UUID ASUSTADO = new UUID("2d0819cf-452b-4db8-b7ac-cda443bc89e7");

        // Variables
        public UUID silla;
        public string nombre;
        public string apellido;
        public string password;
        public string servidor;
        public GridClient cliente;
        public bool atendido; // Variable que indica si el bot esta atendido por un avatar (camarero) o no
        public bool activo;
        public UUID id_camarero;
        public string nombre_camarero;
        public int estado; // Estado de la conversacion del bot.
        public int estado_anterior; // Almacenamos el estado anterior del bot.
        public string pedido; // El pedido que desea el bot.
        List<string> alimentos = new List<string>();
        List<string> alimentos_formal = new List<string>();
        List<string> bebidas = new List<string>();
        List<string> bebidas_formal = new List<string>();

        public Bot(string _nombre, string _apellido, string _password, string _servidor)
        {
            nombre = _nombre;
            apellido = _apellido;
            password = _password;
            servidor = _servidor;
            atendido = false;
            activo = false;
            estado = 0;
            nombre_camarero = "";
        }
        public Bot()
        {
        }

        public void conectar()
        {
            cliente = new GridClient();
            OpenMetaverse.Settings.LOG_LEVEL = OpenMetaverse.Helpers.LogLevel.None;
            cliente.Settings.LOGIN_SERVER = servidor;
            cliente.Network.Login(nombre, apellido, password, "Bot cliente", "1.0");
        }
        public void desconectar()
        {
            cliente.Network.Logout();
        }
        public void sentarse()
        {
            Vector3 posicion = new Vector3((float)0.1, 0, (float)0.3);
            cliente.Self.RequestSit(silla, posicion);
        }
        public void levantarse()
        {
            cliente.Self.Stand();
        }
        public void ejecutarGesto(UUID gesto)
        {
            cliente.Self.PlayGesture(gesto);

        }
        public void ejecutarAnimacion(string animacion)
        {
            UUID anim = new UUID(animacion);
            Program.bot.cliente.Self.AnimationStart(anim, true);
            System.Threading.Thread.Sleep(1000);
            Program.bot.cliente.Self.AnimationStop(anim, true);
        }
        public void setCamarero(UUID _id_camarero, string _nombre_camarero)
        {
            id_camarero = _id_camarero;
            nombre_camarero = _nombre_camarero;
            //atendido = true;
        }

        public void getCamarero()
        {
            if (atendido)
            {
                Program.consola.escribeMensaje("El camarero actual es: " + nombre_camarero);
            }
            else
            {
                Program.consola.escribeMensaje("Ahora mismo no le esta atendiendo ningun camarero.");
            }

        }
        public void decir(string mensaje)
        {
            cliente.Self.Chat(mensaje, 0, OpenMetaverse.ChatType.Normal);
        }

        public void gritar(string mensaje)
        {
            cliente.Self.Chat(mensaje, 0, OpenMetaverse.ChatType.Shout);
        }

        public void generarPedido()
        {
            string cadena = "Ich möchte ";
            int aleat;
            // Generamos 'NRO_COMIDAS'
            for (int i = 0; i < NRO_COMIDAS; i++)
            {
                if (alimentos.Count() == 0)
                {
                    // AQUI SELECCIONAMOS EL TIPO DE PEDIDO (productos, productos_sanos o productos_grasos)
                    alimentos.AddRange(Program.diccionario.productos);
                    alimentos_formal.AddRange(Program.diccionario.productos_formal);
                }

                Random random = new Random();
                aleat = random.Next(alimentos.Count());
                cadena += alimentos.ElementAt(aleat);
                //Enviamos el nombre formal de la comida por el canal 5 y lo eliminamos de la lista.
                cliente.Self.Chat(alimentos_formal.ElementAt(aleat), CANALCAMARERO, OpenMetaverse.ChatType.Normal);
                if (i < NRO_COMIDAS - 1)
                {
                    cadena += ", ";
                }

                alimentos.RemoveAt(aleat);
                alimentos_formal.RemoveAt(aleat);      
            }
            // Generamos 'NRO_BEBIDAS'
            for (int i = 0; i < NRO_BEBIDAS; i++)
            {
                if (bebidas.Count == 0)
                {
                    bebidas.AddRange(Program.diccionario.bebidas);
                    bebidas_formal.AddRange(Program.diccionario.bebidas_formal);
                }

                Random random = new Random();
                cadena += " und ";
                aleat = random.Next(bebidas.Count());
                cadena += bebidas.ElementAt(aleat);
                //Enviamos el nombre formal de la comida por el canal 5 y lo eliminamos de la lista.
                cliente.Self.Chat(bebidas_formal.ElementAt(aleat), CANALCAMARERO, OpenMetaverse.ChatType.Normal);

                bebidas.RemoveAt(aleat);
                bebidas_formal.RemoveAt(aleat);
                
            }
            cadena += ".";
            pedido = cadena;

        }
        public void mirar()
        {
            cliente.Self.LookAtEffect(cliente.Self.AgentID, id_camarero, Vector3d.Zero, LookAtType.Mouselook, UUID.Zero);
        }
        public void dejar_mirar()
        {
            cliente.Self.LookAtEffect(cliente.Self.AgentID, UUID.Zero, Vector3d.Zero, LookAtType.None, UUID.Zero);
        }

        public void dejar_atender()
        {
            // Detenemos el temporizador 
            Program.tiempo.temporizador.Stop();

            estado = 0; // Estado 0
            estado_anterior = 0;
            atendido = false;
            Program.consola.escribeMensaje(nombre_camarero + " deja de ser camarero.");
            nombre_camarero = "";
            dejar_mirar();
        }
    }
}
