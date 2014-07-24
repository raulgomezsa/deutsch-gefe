using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
using System.Net;

namespace ClienteCafeteria
{
    class ControlDialogo
    {
        // Distancia a la que vamos a escuchar a los camareros
        const int MAX_DISTANCIA = 4;
        const int CANALCAMARERO = 5;
        const int CANALIDCAMARERO = 800; // Canal por el que enviamos su UUID a la mesa.
        // Variables
        ChatEventArgs e;
        public int distancia;



        public ControlDialogo(ChatEventArgs _e, int _distancia)
        {
            e = _e;
            distancia = _distancia;
        }

        /* Estados del bot:
         * 0 -> No le han saludado todavia.
         * 1 -> Le han saludado y tiene establecido un camarero, espera a que le pregunten por su pedido.
         * 2 -> Ya ha realizado el pedido, puede pedirle el camarero que lo repita, darle las gracias o 
         * decirle que espere mientras se lo trae.
         * 3 -> Espera a que el camarero realize la actividad y termina con la entrega.
         * 4 -> Espera a que el camarero le pida perdon para poder continuar, si se lo pide pasa a un estado anterior.
        */
        public void responder()
        {
            if (e.Type != ChatType.StartTyping && e.Type != ChatType.StopTyping && distancia < MAX_DISTANCIA)
            {
                // Comprobamos si el mensaje está en alemán (solo permitimos en español ligar e insultos).
                if (e.Message != "" || diceVale() || esInsultoGracioso() || esInsultoGrave() || esDisculpa() || esPiropo() || intentaLigarDescarado() || intentaLigarIndiscreto() || esDespedida() || diceVale())
                {
                    System.Threading.Thread.Sleep(1500);
                    if (e.OwnerID == Program.bot.id_camarero && Program.bot.activo == true) // Si el mensaje es de su camarero.
                    {
                        if (Program.bot.atendido) // Si está atendido
                        {
                            switch (Program.bot.estado)
                            {
                                case 1: //Estado 1
                                    if (esPedido())
                                        darPedido(); // Paso a estado 2.
                                    else if (esEstado())
                                        responder_Estado();
                                    else if (esGracias())
                                        darGracias();
                                    else if (esDespedida())
                                        responder_DespedidaTemprana();
                                    else if (esInsultoGracioso())
                                        responder_insultoGracioso();
                                    else if (esInsultoGrave())
                                        responder_insultoGrave(); //Paso a estado 4
                                    else if (esPiropo())
                                        responder_Piropo();
                                    else if (intentaLigarDescarado())
                                        responder_intentoLigarDescarado();
                                    else if (intentaLigarIndiscreto())
                                        responder_intentoLigarIndiscreto();
                                    else if (diceVale())
                                        Program.bot.decir("Danke.");
                                    else
                                        noEntiendo();
                                    break;
                                case 2: //Estado 2
                                    if (esPedido())
                                        Program.bot.decir(Program.bot.pedido);
                                    else if (diceVale())
                                        Program.bot.decir("Danke.");
                                    else if (esSaludo() || esDespedida())
                                        estoyEsperando();
                                    else if (esRepetir())
                                        repetirPedido();
                                    else if (esGracias())
                                        darGracias();
                                    else if (esEspera())
                                        esperar(); // Paso a estado 3
                                    else if (esInsultoGracioso())
                                        responder_insultoGracioso();
                                    else if (esInsultoGrave())
                                        responder_insultoGrave(); //Paso a estado 4
                                    else if (esPiropo())
                                        responder_Piropo();
                                    else if (intentaLigarDescarado())
                                        responder_intentoLigarDescarado();
                                    else if (intentaLigarIndiscreto())
                                        responder_intentoLigarIndiscreto();
                                    else
                                        noEntiendo();
                                    break;
                                case 3: //Estado 3
                                    if (esRepetir())
                                        repetirPedido();
                                    else if (esPedido())
                                        Program.bot.decir(Program.bot.pedido);
                                    else if (esGracias())
                                        darGracias();
                                    else if (esInsultoGracioso())
                                        responder_insultoGracioso();
                                    else if (esInsultoGrave())
                                        responder_insultoGrave(); //Paso a estado 4
                                    else if (esPiropo())
                                        responder_Piropo();
                                    else if (intentaLigarDescarado())
                                        responder_intentoLigarDescarado();
                                    else if (intentaLigarIndiscreto())
                                        responder_intentoLigarIndiscreto();
                                    else if (diceVale())
                                        Program.bot.decir("Danke.");
                                    else if (diceDeNada())
                                        Program.bot.decir("Tschüss, Auf Wiedersehen.");
                                    else
                                        noEntiendo();
                                    break;
                                case 4:
                                    if (esDisculpa())
                                        responder_Disculpa(); //Paso a estado anterior
                                    else
                                        responder_Enfado();
                                    break;
                            }
                        }
                        else  // Estado 0. Si no esta atendido.
                        {
                            if (esSaludo() && Program.bot.activo)
                                asignacion(); //Paso a estado 1
                            else if (esDespedida())
                                despedirse();
                            else if (diceDeNada())
                                Program.bot.decir("Tschüss, Auf Wiedersehen.");
                            else if (esInsultoGracioso())
                                responder_insultoGracioso();
                            else if (esInsultoGrave())
                                responder_insultoGraveNoCamarero();
                            else if (esPiropo())
                                responder_Piropo();
                            else if (intentaLigarDescarado())
                                responder_intentoLigarDescarado();
                            else if (intentaLigarIndiscreto())
                                responder_intentoLigarIndiscreto();
                            else
                                noEntiendo();
                        }

                    }
                    else // El mensaje es de otro camarero, le dice que esta atendido por otro camarero.
                    {
                        if (esInsultoGracioso())
                            responder_insultoGracioso();
                        else if (esEstado())
                            responder_Estado();
                        else if (esInsultoGrave())
                            responder_insultoGraveNoCamarero();
                        else if (esPiropo())
                            responder_Piropo();
                        else if (intentaLigarDescarado())
                            responder_intentoLigarDescarado();
                        else if (intentaLigarIndiscreto())
                            responder_intentoLigarIndiscreto();
                        else if (Program.bot.atendido == true)
                            estoyAtendido();
                        else
                            responder_NoQuieroNada();
                    }
                } // fin del if aleman
                else
                {
                    Program.bot.decir("Sprechen Sie bitte Deutsch!"); // Indica que le hable en alemán.
                }
            }
        }

        // Respuestas

        //Estado 0
        public void asignacion()
        {
            // Iniciamos el tiempo que tiene para atenderle y mandamos un mensaje al reloj.
            Program.tiempo.temporizador.Start();
            // Asignamos como camarero al avatar que le ha saludado.
            Program.bot.atendido=true;
            // El bot sigue con la mirada al camarero.
            Program.bot.mirar();
            Program.consola.escribeMensaje("El bot esta siendo atendido por " + Program.bot.nombre_camarero + ".");
            // Saluda a su camarero.
            // Program.bot.ejecutarGesto(Program.bot.SALUDO);
            //Program.bot.ejecutarAnimacion(Animations.HELLO.ToString());
            Program.bot.cliente.Self.AnimationStop(Animations.HELLO, true);
            Program.bot.decir("Guten Tag " + Program.bot.nombre_camarero);
            Program.bot.estado = 1; // Paso a estado 1
            //Enviamos UUID del camarero para que se active la bandeja
            Program.bot.cliente.Self.Chat(Program.bot.id_camarero.ToString(), CANALCAMARERO, OpenMetaverse.ChatType.Normal);
            // Enviamos UUID del camarero para los objetos que se mostraran en la mesa
            Program.bot.cliente.Self.Chat(Program.bot.id_camarero.ToString(), CANALIDCAMARERO, OpenMetaverse.ChatType.Whisper);

        }
        //Estado1
        public void darPedido()
        {
            Program.bot.generarPedido();
            Program.consola.escribeMensaje("Se ha generado un pedido para el camarero.");
            System.Threading.Thread.Sleep(1000);
            Program.bot.ejecutarAnimacion(Animations.SMOKE_IDLE.ToString());
            Program.bot.decir(Program.bot.pedido);
            Program.bot.estado = 2; // Paso a estado 2
        }
        //Estado2
        public void esperar()
        {
            Program.bot.decir("Vielen Dank!");
            System.Threading.Thread.Sleep(2000);
            Program.bot.ejecutarGesto(Program.bot.ABURRIDO);
            Program.bot.estado = 3; // Paso a estado 3
            Program.consola.escribeMensaje("El camarero esta realizando la actividad.");
        }

        public void despedirse()
        {

            Random r = new Random(DateTime.Now.Second);

            int ind = r.Next(1, 4);

            Program.bot.ejecutarAnimacion(Animations.SALUTE.ToString());
            switch (ind)
            {
                case 1:
                    Program.bot.decir("Mach' s gut!");
                    break;
                case 2:
                    Program.bot.decir("Vielen Dank! Auf Wiedersehen " + e.FromName + ".");
                    break;
                case 3:
                    Program.bot.decir("Herzlichen Dank!");
                    break;
            }
            
        }

        public void darGracias()
        {
            Program.bot.ejecutarGesto(Program.bot.WINK);
            Program.bot.decir("Bitte, keine Ursache!");
        }
        public void repetirPedido()
        {
            Random r = new Random(DateTime.Now.Second);

            int ind = r.Next(1, 5);

            Program.bot.ejecutarGesto(Program.bot.SI);
            switch (ind)
            {
                case 1:
                    Program.bot.decir("Kein Problem.");
                    break;
                case 2:
                    Program.bot.decir("Geht in Ordnung!");
                    break;
                case 3:
                    Program.bot.decir("Okay.");
                    break;
                case 4:
                    Program.bot.decir("Alles klar!");
                    break;
            }

            System.Threading.Thread.Sleep(1500);
            Program.bot.decir(Program.bot.pedido);
        }

        public void noEntiendo()
        {
            Random r = new Random(DateTime.Now.Second);

            int ind = r.Next(1, 3);

            Program.bot.ejecutarGesto(Program.bot.NOENTIENDO);
            switch (ind)
            {
                case 1:
                    Program.bot.decir("Wie bitte?");
                    break;
                case 2:
                    Program.bot.decir("Ich verstehe dich nicht.");
                    break;
            }
        }

        public void estoyEsperando()
        {
            Program.bot.ejecutarGesto(Program.bot.ABURRIDO);
            Program.bot.decir("Ich warte auf meine Bestellung.");
        }

        public void estoyAtendido()
        {

            Program.bot.decir("Mich bedient gerade " + Program.bot.nombre_camarero + ", Dennoch, vielen Dank!.");
        }

        public void responder_insultoGracioso()
        {
            Program.bot.decir(e.Message + " du !");
            Program.bot.ejecutarAnimacion(Animations.POINT_YOU.ToString());
        }
        public void responder_Estado()
        {
            Random r = new Random(DateTime.Now.Second);

            int ind = r.Next(1, 3);

            Program.bot.ejecutarAnimacion(Animations.PEACE.ToString());
            switch (ind)
            {
                case 1:
                    Program.bot.decir("Danke, mir geht's gut!");
                    break;
                case 2:
                    Program.bot.decir("Ich kann nicht bleiben.");
                    break;
            }            
        }
        public void responder_DespedidaTemprana()
        {
            Program.bot.decir("Entschuldige bitte! Wann kommst du zu mir?");
            Program.bot.ejecutarAnimacion(Animations.ANGRY.ToString());
        }
        public void responder_insultoGrave()
        {
            Program.bot.ejecutarAnimacion(Animations.CRY.ToString());
            Program.bot.decir("Aber was sagst du da?");
            Program.bot.estado_anterior = Program.bot.estado;
            Program.bot.estado = 4;
        }

        public void responder_insultoGraveNoCamarero()
        {
            Program.bot.ejecutarAnimacion(Animations.CRY.ToString());
            Program.bot.decir("Du bist dick.");
        }

        public void responder_Enfado()
        {
            Random r = new Random(DateTime.Now.Second);

            int ind = r.Next(1, 3);

            Program.bot.ejecutarAnimacion(Animations.SAD.ToString());
            switch (ind)
            {
                case 1:
                    Program.bot.decir("Entschuldige dich sofort oder ich rufe deinen Chef.");
                    break;
                case 2:
                    Program.bot.decir("Entschuldige dich sofort.");
                    break;
            } 
        }

        public void responder_Disculpa()
        {
            Random r = new Random(DateTime.Now.Second);

            int ind = r.Next(1, 3);

            Program.bot.ejecutarAnimacion(Animations.YES.ToString());
            switch(ind){
                case 1:
                    Program.bot.decir("Ja, so gefällst du mir besser.");
                    break;
                case 2:
                    Program.bot.decir("Schon besser.");
                    break;
            }

            Program.bot.estado = Program.bot.estado_anterior;
        }

        public void responder_NoQuieroNada()
        {
            
            Random r = new Random(DateTime.Now.Second);

            int ind = r.Next(1, 3);

            Program.bot.ejecutarAnimacion(Animations.NO.ToString());
            switch (ind)
            {
                case 1:
                    Program.bot.decir("Eventuell später!");
                    break;
                case 2:
                    Program.bot.decir("Momentan bin ich zufrieden. Danke!");
                    break;
            }
        }

        public void responder_Piropo()
        {
            Random r = new Random(DateTime.Now.Second);

            int ind = r.Next(1, 3);

            Program.bot.ejecutarAnimacion(Animations.KISS_MY_BUTT.ToString());
            switch (ind)
            {
                case 1:
                    Program.bot.decir("Danke für das Kompliment!");
                    break;
                case 2:
                    Program.bot.decir("Oh! Wie schön!");
                    break;
            }
        }
        public void responder_intentoLigarDescarado()
        {
            Random r = new Random(DateTime.Now.Second);

            int ind = r.Next(1, 3);

            Program.bot.ejecutarAnimacion(Animations.NO.ToString());
            switch (ind)
            {
                case 1:
                    Program.bot.decir("Ich habe kein Interesse!");
                    break;
                case 2:
                    Program.bot.decir("Danke, aber ich bin nicht interessiert.");
                    break;
            }
        }
        public void responder_intentoLigarIndiscreto()
        {
            Random r = new Random(DateTime.Now.Second);

            int ind = r.Next(1, 3);

            Program.bot.ejecutarAnimacion(Animations.FINGER_WAG.ToString());
            switch (ind)
            {
                case 1:
                    Program.bot.decir("Lass mich bitte in Ruhe!");
                    break;
                case 2:
                    Program.bot.decir("Natürlich...... nicht.");
                    break;
            }
        }
        // Búsquedas en el diccionario para determinar el tipo de palabra recibida
        public bool esSaludo()
        {
            return (Program.diccionario.busqueda(Program.diccionario.sinonimos_saludos, e.Message));
        }
        public bool esEstado()
        {
            return (Program.diccionario.busqueda(Program.diccionario.sinonimos_estado, e.Message));
        }
        public bool esPedido()
        {
            return (Program.diccionario.busqueda(Program.diccionario.sinonimos_desear, e.Message));
        }

        public bool esRepetir()
        {
            return (Program.diccionario.busqueda(Program.diccionario.sinonimos_repetir, e.Message));
        }

        public bool esGracias()
        {
            return (Program.diccionario.busqueda(Program.diccionario.sinonimos_gracias, e.Message));
        }

        public bool esDespedida()
        {
            return (Program.diccionario.busqueda(Program.diccionario.sinonimos_adios, e.Message));
        }

        public bool esEspera()
        {
            return (Program.diccionario.busqueda(Program.diccionario.sinonimos_esperar, e.Message));
        }
        public bool esInsultoGracioso()
        {
            return (Program.diccionario.busqueda(Program.diccionario.sinonimos_insultos_graciosos, e.Message));
        }
        public bool esPiropo()
        {
            return (Program.diccionario.busqueda(Program.diccionario.sinonimos_piropos, e.Message));
        }
        public bool esInsultoGrave()
        {
            return (Program.diccionario.busqueda(Program.diccionario.sinonimos_insultos_graves, e.Message));
        }

        public bool esDisculpa()
        {
            return (Program.diccionario.busqueda(Program.diccionario.sinonimos_disculpas, e.Message));
        }

        public bool intentaLigarDescarado()
        {
            return (Program.diccionario.busqueda(Program.diccionario.sinonimos_ligar_descarado, e.Message));
        }
        public bool intentaLigarIndiscreto()
        {
            return (Program.diccionario.busqueda(Program.diccionario.sinonimos_ligar_indiscreto, e.Message));
        }
        public bool diceVale()
        {
            return (Program.diccionario.busqueda(Program.diccionario.sinonimos_vale, e.Message));
        }
        public bool diceDeNada()
        {
            return (Program.diccionario.busqueda(Program.diccionario.sinonimos_denada, e.Message));
        }
        // Devuelve verdadero si habla en aleman y falso si habla en español.
        public static bool comprobarIdioma(string mensaje)
        {
            bool aleman = false;
            HttpWebRequest solicitud = (HttpWebRequest)WebRequest.Create("https://www.googleapis.com/language/translate/v2/detect?key=AIzaSyD7_9VwXllHA1sPb5KBAngwaxxcU9i6VxM" + "&q=" + mensaje);
            HttpWebResponse respuesta = (HttpWebResponse)solicitud.GetResponse();
            Encoding codificacion = System.Text.Encoding.GetEncoding("utf-8");
            string linea = "";

            if (respuesta.StatusCode == HttpStatusCode.OK)
            {
                System.IO.Stream stream_recibido = respuesta.GetResponseStream();
                System.IO.StreamReader readStream = new System.IO.StreamReader(stream_recibido, codificacion);
                while (!readStream.EndOfStream)
                {
                    linea = readStream.ReadLine();
                    if (linea.Length != 0)
                    {
                        string parte1 = "", parte2 = "";
                        int posDosPuntos = localizarDosPuntos(linea);
                        for (int j = 0; j < posDosPuntos; j++)
                        {
                            parte1 += linea[j];
                        }
                        for (int j = posDosPuntos + 2; j < linea.Length; j++)
                        {
                            parte2 += linea[j];
                        }
                        switch (parte2)
                        {
                            case "\"es\",":
                                aleman = false;
                                break;
                            case "\"de\",":
                                aleman = true;
                                break;
                        }
                    }
                }
                readStream.Close();
            }
            return aleman;
        }
        static int localizarDosPuntos(string cadena)
        {
            for (int i = 0; i < cadena.Length; i++)
            {
                if (cadena[i] == ':')
                    return (i);
            }
            return (-1);
        }
    }
}
