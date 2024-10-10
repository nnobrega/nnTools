using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nnTools.classes
{
    public static class Calendario
    {
        private const string ConfrUniversal = "01/01";
        private const string Tiradentes = "21/04";
        private const string DiaTrabalho = "01/05";
        private const string Independencia = "07/09";
        private const string Padroeira = "12/10";
        private const string Finados = "02/11";
        private const string Republica = "15/11";
        private const string Natal = "25/12";

        // Feriados para estado de São Paulo
        private const string Revolucao = "09/07";
        private const string ConscienciaNegra = "20/11";
        private const string AniversarioSP = "25/01";
        private const string AniversarioSBC = "20/08";
        public static DateTime Pascoa(int pAno)
        {
            DateTime Pascoa = DateTime.MinValue;
            long r1, r2, r3, r4, r5, dia, Mes, Ano;
            Pascoa = DateTime.MinValue;

            r1 = pAno % 19;
            r2 = pAno % 4;
            r3 = pAno % 7;
            r4 = (19 * r1 + 24) % 30;
            r5 = (6 * r4 + 4 * r3 + 2 * r2 + 5) % 7;

            DateTime dtPascoa = DateTime.Parse($"{pAno}/3/22");
            dtPascoa = dtPascoa.AddDays(r4 + r5);
            dia = dtPascoa.Day;
            Mes = dtPascoa.Month;
            Ano = dtPascoa.Year;

            if (dia == 26)
            {
                dtPascoa = DateTime.Parse($"{pAno}/4/19");
            }
            else if (dia == 25)
            {
                if (r1 > 10)
                {
                    dtPascoa = DateTime.Parse($"{pAno}/4/18");
                }
            }

            Pascoa = dtPascoa;
            return Pascoa;
        }
        public static DateTime SextaSanta(int Ano)
        {
            DateTime dtAux = Pascoa(Ano);
            dtAux = dtAux.AddDays(-2);
            return dtAux;
        }
        public static DateTime QuartaCinzas(int Ano)
        {
            DateTime dtAux = Pascoa(Ano);
            dtAux = dtAux.AddDays(-46);
            return dtAux;
        }
        public static DateTime CorpusChristi(int Ano)
        {
            DateTime dtAux = Pascoa(Ano);
            dtAux = dtAux.AddDays(60);
            return dtAux;
        }
        public static DateTime SomaDiasUteis(DateTime Data, int Dias)
        {
            long lngDias = 1;
            long Indice = 1;
            DateTime dtAux = Data;

            while (Indice <= Dias)
            {
                dtAux = Data.AddDays(lngDias);
                if (DiaUtil(dtAux))
                {
                    Indice++;
                }

                lngDias++;
            }

            return dtAux;
        }
        public static DateTime ProximoDiaUtil(DateTime Data)
        {
            int idias = 1;
            DateTime dtAux = Data;

            while (!DiaUtil(dtAux))
            {
                dtAux = dtAux.AddDays(idias);
            }

            return dtAux;
        }
        public static DateTime AnteriorDiaUtil(DateTime Data)
        {
            int idias = -1;
            DateTime dtAux = Data;

            while (DiaUtil(dtAux) == false)
            {
                dtAux = dtAux.AddDays(idias);
            }

            return dtAux;
        }
        public static DateTime SomaDiasUteisHoras(DateTime Data, long Horas, int Fator = 6)
        {
            long lngDias = 1;
            long Indice = 1;
            DateTime dtAux = Data;
            long lngDiasSoma = (Horas / Fator) + 1;

            while (Indice <= lngDiasSoma)
            {
                dtAux = Data.AddDays(lngDias);

                if (DiaUtil(dtAux))
                {
                    Indice++;
                }

                lngDias++;
            }

            return dtAux;
        }
        public static DateTime Carnaval(int Ano)
        {
            DateTime dtAux = Pascoa(Ano);
            dtAux = dtAux.AddDays(-47);
            return dtAux;
        }
        public static DateTime DiaDosPais(int Ano)
        {
            DateTime dtaAux = new DateTime(Ano, 8, 1);
            int qtDom = 0;

            qtDom = 0;

            while (qtDom <= 1)
            {
                dtaAux = dtaAux.AddDays(1);

                if (dtaAux.DayOfWeek == DayOfWeek.Sunday)
                {
                    qtDom++;
                }
            }

            return dtaAux;
        }
        public static DateTime DiaDasMaes(int Ano)
        {
            DateTime dtaAux = new DateTime(Ano, 5, 1);
            int qtDom = 0;

            qtDom = 0;

            while (qtDom <= 1)
            {
                dtaAux = dtaAux.AddDays(1);

                if (dtaAux.DayOfWeek == DayOfWeek.Sunday)
                {
                    qtDom++;
                }
            }

            return dtaAux;
        }
        public static bool DiaUtil(DateTime Data)
        {
            DayOfWeek _DiaSemana = Data.DayOfWeek;

            if (_DiaSemana == DayOfWeek.Sunday || _DiaSemana == DayOfWeek.Saturday)
            {
                return false;
            }

            if (Data == DateTime.Parse($"{ConfrUniversal}/{Data.Year}") ||
                Data == DateTime.Parse($"{Tiradentes}/{Data.Year}") ||
                Data == DateTime.Parse($"{DiaTrabalho}/{Data.Year}") ||
                Data == DateTime.Parse($"{Independencia}/{Data.Year}") ||
                Data == DateTime.Parse($"{Padroeira}/{Data.Year}") ||
                Data == DateTime.Parse($"{Finados}/{Data.Year}") ||
                Data == DateTime.Parse($"{Republica}/{Data.Year}") ||
                Data == DateTime.Parse($"{Natal}/{Data.Year}") ||
                Data == DateTime.Parse($"{Revolucao}/{Data.Year}") ||
                Data == DateTime.Parse($"{ConscienciaNegra}/{Data.Year}") ||
                Data == DateTime.Parse($"{AniversarioSBC}/{Data.Year}") ||
                Data == DateTime.Parse($"{AniversarioSP}/{Data.Year}") ||
                Data == Pascoa(Data.Year) ||
                Data == DiaDasMaes(Data.Year) ||
                Data == DiaDosPais(Data.Year) ||
                Data == QuartaCinzas(Data.Year) ||
                Data == SextaSanta(Data.Year) ||
                Data == Carnaval(Data.Year) ||
                Data == CorpusChristi(Data.Year))
            {
                return false;
            }

            return true;
        }
        public static string DescricaoFeriado(DateTime Data)
        {
            DayOfWeek _DiaSemana = Data.DayOfWeek;
            string DescricaoFeriadoAux = "";

            if (Data == DateTime.Parse($"{ConfrUniversal}/{Data.Year}")) DescricaoFeriadoAux = "Confraterização Universal";
            if (Data == DateTime.Parse($"{Tiradentes}/{Data.Year}")) DescricaoFeriadoAux = "Tiradentes";
            if (Data == DateTime.Parse($"{DiaTrabalho}/{Data.Year}")) DescricaoFeriadoAux = "Dia do Trabalho";
            if (Data == DateTime.Parse($"{Independencia}/{Data.Year}")) DescricaoFeriadoAux = "Dia da Independência";
            if (Data == DateTime.Parse($"{Padroeira}/{Data.Year}")) DescricaoFeriadoAux = "Dia de NSra Aparecida" + Environment.NewLine + "Dia das Crianças";
            if (Data == DateTime.Parse($"{Finados}/{Data.Year}")) DescricaoFeriadoAux = "Dia de Finados";
            if (Data == DateTime.Parse($"{Republica}/{Data.Year}")) DescricaoFeriadoAux = "Proclamação da República";
            if (Data == DateTime.Parse($"{Natal}/{Data.Year}")) DescricaoFeriadoAux = "Natal";
            if (Data == DateTime.Parse($"{Revolucao}/{Data.Year}")) DescricaoFeriadoAux = "Revolução 32";
            if (Data == DateTime.Parse($"{ConscienciaNegra}/{Data.Year}")) DescricaoFeriadoAux = "Consciência Negra";
            if (Data == DateTime.Parse($"{AniversarioSBC}/{Data.Year}")) DescricaoFeriadoAux = "Aniversário SBC";
            if (Data == DateTime.Parse($"{AniversarioSP}/{Data.Year}")) DescricaoFeriadoAux = "Aniversário São Paulo";

            if (Data == Pascoa(Data.Year)) DescricaoFeriadoAux = "Páscoa";
            if (Data == DiaDasMaes(Data.Year)) DescricaoFeriadoAux = "Dia das Mães";
            if (Data == DiaDosPais(Data.Year)) DescricaoFeriadoAux = "Dia dos Pais";
            if (Data == QuartaCinzas(Data.Year)) DescricaoFeriadoAux = "Quarta-Feira de Cinzas";
            if (Data == SextaSanta(Data.Year)) DescricaoFeriadoAux = "Sexta-Feira Santa";
            if (Data == Carnaval(Data.Year)) DescricaoFeriadoAux = "Carnaval";
            if (Data == CorpusChristi(Data.Year)) DescricaoFeriadoAux = "Corpus Christi";

            if (DescricaoFeriadoAux == "")
            {
                if (_DiaSemana == DayOfWeek.Sunday  || _DiaSemana == DayOfWeek.Saturday)
                {
                    DescricaoFeriadoAux = "Fim de Semana";
                }
                else
                {
                    DescricaoFeriadoAux = "Dia Útil";
                }
            }

            return DescricaoFeriadoAux;
        }
        public static Mes FormatarMes(int Ano, int Mes)
        {
            try
            {
                Mes _mes = new Mes();
                int _diaSemana = 0;

                _mes.Id = Mes;
                _mes.Ano = Ano;

                _mes.Semanas = new List<Semana>();

                for (DateTime _data = new DateTime(Ano, Mes, 1); _data.Month == Mes;)
                {
                    Semana _semana = new Semana();

                    _semana.Id = GetNumeroSemanaAno(_data);
                    _semana.Dias = new List<Dia>();

                    for (int i = 0; i < 7; i++)
                    {
                        _semana.Dias.Add(new Dia
                        {
                            DiaSemana = (DayOfWeek)i
                        });
                    }

                    for (_diaSemana = 0; _diaSemana < 7 && _data.Month == Mes;)
                    {
                        if ((int)_data.DayOfWeek == _diaSemana)
                        {
                            _semana.Dias[(int)_data.DayOfWeek].Id = _data.Day;
                            _semana.Dias[(int)_data.DayOfWeek].DataCompleta = _data;
                            _semana.Dias[(int)_data.DayOfWeek].DiaUtil = DiaUtil(_data);
                            _semana.Dias[(int)_data.DayOfWeek].DescricaoDia = DescricaoFeriado(_data);
                            _semana.Dias[(int)_data.DayOfWeek].CorFonte = Color.Black;
                            _semana.Dias[(int)_data.DayOfWeek].CorFundo = Color.White;

                            _data = _data.AddDays(1);
                        }
                        _diaSemana++;
                    }

                    _mes.Semanas.Add(_semana);
                }

                return _mes;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static int GetNumeroSemanaAno(DateTime Data)
        {
            // Defina o primeiro dia da semana para segunda-feira
            DayOfWeek firstDay = DayOfWeek.Monday;

            // Obtém o dia da semana da data especificada
            DayOfWeek dayOfWeek = Data.DayOfWeek;

            // Define a cultura para garantir que o cálculo seja consistente
            System.Globalization.CultureInfo culture = System.Globalization.CultureInfo.CurrentCulture;

            // Obtém o número da semana do ano
            int weekNumber = culture.Calendar.GetWeekOfYear(Data, culture.DateTimeFormat.CalendarWeekRule, firstDay);

            return weekNumber;
        }
    }
}
