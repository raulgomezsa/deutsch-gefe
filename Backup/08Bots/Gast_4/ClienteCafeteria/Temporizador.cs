using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClienteCafeteria
{
    class Temporizador
    {
        // Tiempo que puede tardar en atenderle: 2 minutos y medio.
        // El 3000 de incremento es por el retardo de los scripts.
        const int TIEMPO = 903000;

        // Declaramos el temporizador
        public System.Timers.Timer temporizador = new System.Timers.Timer(TIEMPO);

        // Constructor de la clase
        public Temporizador()
        {
            temporizador.Elapsed += new System.Timers.ElapsedEventHandler(temporizador_Elapsed);
        }

        // Evento del temporizador. Se lanza cuando alguien se asigna como camarero.
        void temporizador_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Program.bot.dejar_atender();
            Program.consola.escribeEvento("Tiempo limite de atencion al bot terminado, bot liberado.");
        }
    }
}
