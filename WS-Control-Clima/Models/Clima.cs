using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WS_Control_Clima.Models
{
    public class Clima
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string ciudad { get; set; }
        public string grados { get; set; }
        public string fechaHora { get; set; }
    }
}