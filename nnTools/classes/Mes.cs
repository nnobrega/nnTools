using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace nnTools.classes
{
    public class Mes
    {
        public int Id { get; set; }
        public int Ano { get; set; }
        public List<Semana> Semanas { get; set; }
    }
    public class Semana
    {
        public int Id { get; set; }
        public List<Dia> Dias { get; set; }
    }

    public class Dia
    {
        public int Id { get; set; }
        public DateTime DataCompleta { get; set; }
        public int DiaAno { get; set; }
        public DayOfWeek DiaSemana { get; set; }
        public bool DiaUtil { get; set; }
        public string DescricaoDia { get; set; }
        public Color CorFonte { get; set; }
        public Color CorFundo { get; set; }
    }
}

