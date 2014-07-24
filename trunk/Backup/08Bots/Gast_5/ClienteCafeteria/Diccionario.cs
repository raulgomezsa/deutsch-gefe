using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClienteCafeteria
{
    class Diccionario
    {
        // Lista de productos
        public List<string> productos_grasos = new List<string>();
        public List<string> productos_grasos_formal = new List<string>();
        public List<string> productos_sanos = new List<string>();
        public List<string> productos_sanos_formal = new List<string>();
        public List<string> productos = new List<string>();
        public List<string> productos_formal = new List<string>();
        public List<string> bebidas = new List<string>();
        public List<string> bebidas_formal = new List<string>();
        // Lista de palabras que reconoce el bot
        public List<string> sinonimos_saludos = new List<string>();
        public List<string> sinonimos_estado = new List<string>();
        public List<string> sinonimos_desear = new List<string>();
        public List<string> sinonimos_repetir = new List<string>();
        public List<string> sinonimos_gracias = new List<string>();
        public List<string> sinonimos_adios = new List<string>();
        public List<string> sinonimos_insultos_graciosos = new List<string>();
        public List<string> sinonimos_piropos = new List<string>();
        public List<string> sinonimos_insultos_graves = new List<string>();
        public List<string> sinonimos_disculpas = new List<string>();
        public List<string> sinonimos_ligar_descarado = new List<string>();
        public List<string> sinonimos_ligar_indiscreto = new List<string>();
        public List<string> sinonimos_esperar = new List<string>();
        public List<string> sinonimos_vale = new List<string>();
        public List<string> sinonimos_denada = new List<string>();

        public bool busqueda(List<string> lista, string elemento)
        {
            bool encontrado = false;
            for (int i = 0; i < lista.Count; i++)
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(elemento, lista.ElementAt(i), System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                    encontrado = true;
            }
            return encontrado;
        }


    }
}
