//using NReco.VideoInfo;
//using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;
using System.Drawing;
using System.Runtime.InteropServices;
using access = Microsoft.Office.Interop.Access.Dao;
using System.Security.Principal;
using System.Text.RegularExpressions;

namespace nnTools.classes
{
    public class Tools
    {
        public WindowsIdentity UsuarioAtivo = WindowsIdentity.GetCurrent();
        public static ToolStripStatusLabel stsBar;
        public static bool CancelarProcessamento = false;
        public static readonly DateTime DataMinima = new DateTime(2001, 01, 01);

        #region Tratamento Strings
        public static string strReverse(string Valor)
        {
            if (Valor.Trim() == "") return "";

            char[] chrAux = Valor.ToCharArray();
            string strAux = "";

            for (int i = chrAux.Length - 1; i > -1; i--)
            {
                strAux += chrAux[i];
            }

            return strAux;
        }
        public static string TipoMovimento(EtipoMovimento Tipo)
        {
            return TipoMovimento(Tipo, false);
        }
        public static string TipoMovimento(EtipoMovimento Tipo, bool Descricao)
        {
            if (Descricao)
                return (Tipo == EtipoMovimento.Previsto ? "Previsto" : "Efetivo");
            else
                return (Tipo == EtipoMovimento.Previsto ? "P" : "E");
        }
        public static string LimitarString(string pString, int pTamanho, string pPreencher = " ")
        {
            if (pPreencher == "")
            {
                pPreencher = " ";
            }

            if (pString.Length == pTamanho)
            {
                return pString;
            }
            if (pString.Length > pTamanho)
            {
                return pString.Substring(0, pTamanho);
            }

            pString = pString + pPreencher.PadLeft(pTamanho, Convert.ToChar(pPreencher));

            return pString.Substring(0, pTamanho);

        }
        public static string Mid(string Texto, int Start, int End)
        {
            if (Texto.Trim().Equals(string.Empty))
                return string.Empty;
            else
            {
                if (Texto.Length >= End)
                    return Texto.Substring(Start - 1, End);
                else
                    return Texto;

            }
        }
        public static string Mid(string Texto, int Start)
        {
            if (Texto.Trim().Equals(string.Empty))
                return string.Empty;
            else
                if (Texto.Length < Start)
                return Texto.PadRight(Start).Substring(Start - 1);
            else
                return Texto.Substring(Start - 1);
        }
        public static string Right(string Texto, int Tamanho)
        {
            if (Texto.Trim().Equals(string.Empty))
                return string.Empty;
            else
                if (Texto.Length < Tamanho)
                return Texto;
            else
                return Texto.Substring(Texto.Length - Tamanho);
        }
        public static string Left(string Texto, int Fim)
        {
            if (Texto.Trim().Equals(string.Empty))
                return string.Empty;
            else
                if (Texto.Length < Fim)
                return Texto;
            else
                return Texto.Substring(0, Fim);
        }
        public static string Trim(string Texto)
        {
            if (Texto != null) return Texto.Trim();

            return "";

        }
        public static string UCase(string Texto)
        {
            return Texto.Trim().ToUpper();
        }
        public static void UFCampoTexto(ref object sender, ref KeyPressEventArgs e, int Tamanho)
        {
            Encoding ascii = Encoding.ASCII;
            Encoding unicode = Encoding.Unicode;
            int iTamanhoTexto = 0;
            int iTamanhoSelTexto = 0;
            byte[] unicodeBytes;
            byte[] asciiBytes;

            if ((Keys)e.KeyChar == Keys.Return) return;
            if ((Keys)e.KeyChar == Keys.Back) return;
            if ((Keys)e.KeyChar == Keys.Delete) return;

            if (sender.GetType() == typeof(TextBox))
            {
                TextBox txt = sender as TextBox;
                iTamanhoTexto = txt.Text.Length;
                iTamanhoSelTexto = txt.SelectedText.Length;
            }
            if (sender.GetType() == typeof(ComboBox))
            {
                ComboBox cbo = sender as ComboBox;
                iTamanhoTexto = cbo.Text.Length;
                iTamanhoSelTexto = cbo.SelectedText.Length;
            }

            if (iTamanhoTexto < Tamanho || iTamanhoSelTexto > 0)
            {
                unicodeBytes = unicode.GetBytes(e.KeyChar.ToString().ToUpper());
                asciiBytes = Encoding.Convert(unicode, ascii, unicodeBytes);
                e.KeyChar = (char)asciiBytes[0];
                return;
            }

            e.Handled = true;

        }
        public static string ExtrairConteudo(string texto, string delimitador1, string delimitador2, StringComparison metodo, bool noIntervalo = false)
        {
            int pos1 = texto.IndexOf(delimitador1, 0, metodo);

            // Se não encontrar o primeiro delimitador, retorne uma string vazia
            if (pos1 == -1)
                return string.Empty;

            int pos2;
            if (delimitador1 == delimitador2 || noIntervalo)
            {
                pos2 = texto.LastIndexOf(delimitador2, metodo);
            }
            else
            {
                pos2 = texto.IndexOf(delimitador2, pos1 + delimitador1.Length, metodo);
            }

            // Se não encontrar o segundo delimitador ou a posição do segundo delimitador for inválida
            if (pos2 == -1 || pos2 <= pos1)
            {
                pos2 = texto.IndexOf(';', pos1 + delimitador1.Length);
                if (pos2 == -1 || pos2 <= pos1)
                    return string.Empty;
            }

            // Extrai o conteúdo entre os delimitadores, excluindo os delimitadores
            return texto.Substring(pos1 + delimitador1.Length, pos2 - (pos1 + delimitador1.Length));
        }

        #endregion

        #region Trata Campos Numericos
        public static string FormataNumeroString(string Numero, int QtInteiros)
        {
            return FormataNumeroString(Numero, QtInteiros, 0);
        }

        public static string FormataNumeroString(string Numero, int QtInteiros, int QtDecimais)
        {
            string strFormatado = "0";
            string strFornmato = "";
            string strFator = "1";
            double dblAux = 0;

            strFator = strFator.PadRight(QtDecimais + 1, '0');

            if (UFEhNumerico(Numero.Replace(",", "")))
            {
                if (QtDecimais > 0)
                {
                    if (Numero.Contains(','))
                    {
                        dblAux = Convert.ToInt64(Numero.Replace(",", ""));
                    }
                    else
                    {
                        dblAux = Convert.ToInt64(Numero) * Convert.ToInt64(strFator);
                    }
                }
                else
                {
                    dblAux = Convert.ToInt64(Numero);
                }
            }
            else
            {
                dblAux = 0;
            }

            dblAux = dblAux / Convert.ToInt64(strFator);
            strFornmato = "";

            if (QtDecimais > 0)
            {
                strFornmato += ("0").PadRight((QtInteiros - QtDecimais), '0') + ".";
                strFornmato += ("0").PadRight(QtDecimais, '0');
            }
            else
            {
                strFornmato = strFornmato.PadRight(QtInteiros, '0');
            }

            strFormatado = dblAux.ToString(strFornmato);
            strFormatado = strFormatado.Replace(",", "");

            return strFormatado;
        }

        public static string FormataNumeroDecimal(string Numero, int CasaDecimal)
        {
            string FormataNumeroDecimal = "";
            string strAux = "";

            strAux = Right(Numero, CasaDecimal);
            Numero = Mid(Numero, 1, Numero.Length - CasaDecimal);

            FormataNumeroDecimal = Numero + "," + strAux;

            return FormataNumeroDecimal;
        }
        public static bool UFEhNumerico(string Valor)
        {
            Int32 iNumero;
            return Int32.TryParse(Valor, out iNumero);

        }
        public static void UFLimitarNumeros(ref object sender, ref KeyPressEventArgs e, int Tam)
        {
            UFCampoTexto(ref sender, ref e, Tam);
            DigitarSomenteNumeros(ref e);
        }
        public static void DigitarSomenteNumeros(ref KeyPressEventArgs e, bool AceitaVirgula)
        {
            switch (e.KeyChar)
            {
                case (char)3:  //Ctrl+C
                case (char)22: //Ctrl+V
                case (char)24: //Ctrl+X
                case (char)26: //Ctrl+Z
                case (char)1:  //Ctrl+A
                case (char)8:  //Backspace
                case (char)45:  //Menos
                case (char)43:  //Mais
                    return;
                default:
                    if (e.KeyChar == 44 && AceitaVirgula) return;
                    if (!char.IsDigit(e.KeyChar)) e.Handled = true;
                    break;
            }
        }

        public static void DigitarSomenteNumeros(ref KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case (char)3:  //Ctrl+C
                case (char)22: //Ctrl+V
                case (char)24: //Ctrl+X
                case (char)26: //Ctrl+Z
                case (char)1:  //Ctrl+A
                case (char)8:  //Backspace
                case (char)45:  //Menos
                case (char)43:  //Mais
                    return;
                default:
                    if (!char.IsDigit(e.KeyChar)) e.Handled = true;
                    break;
            }
        }
        #endregion

        #region Mensagem
        public static void MensagemStatus(ref ToolStripStatusLabel status)
        {
            MensagemStatus(ref status, "");
        }
        public static void MensagemStatus(ref ToolStripStatusLabel status, string mensagem)
        {

            if (mensagem == "")
            {
                status.GetCurrentParent().Parent.Cursor = Cursors.Default;
                status.Text = "Pronto!";
            }
            else
            {
                status.GetCurrentParent().Parent.Cursor = Cursors.WaitCursor;
                status.Text = mensagem + "...";
            }

            status.Invalidate();
            status.ForeColor = Color.Black;

            System.Windows.Forms.Application.DoEvents();
        }
        public static void MensagemStatus()
        {
            MensagemStatus(ref stsBar, "");
        }
        public static void MensagemStatus(string mensagem)
        {
            MensagemStatus(ref stsBar, mensagem);
        }

        public static void ExibirMensagemAlerta(string mensagem, string titulo)
        {
            MessageBox.Show(mensagem, titulo, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        public static void ExibirMensagemExclamacao(string mensagem, string titulo)
        {
            MessageBox.Show(mensagem, titulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        public static void ExibirMensagemErro(string mensagem, string titulo)
        {
            MessageBox.Show(mensagem, titulo, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        public static DialogResult ExibirMensagemPergunta(string mensagem, string titulo)
        {
            return MessageBox.Show(mensagem, titulo, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }
        #endregion

        #region Trata tipo arquivo 
        //public static bool VerImagem(FileInfo arquivo)
        //{
        //    try
        //    {
        //        if (arquivo.Extension == ".exe") return false;

        //        using (Image bitmap =    .FromFile(arquivo.FullName))
        //        {
        //            bitmap.Dispose();
        //        }

        //        arquivo = null;

        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        arquivo = null;
        //        return false;
        //    }
        //}
        //public static bool VerVideo(FileInfo arquivo)
        //{
        //    string filePath = arquivo.FullName;

        //    try
        //    {
        //        FFProbe? ffProbe = new FFProbe();
        //        MediaInfo? videoInfo = ffProbe.GetMediaInfo(filePath);

        //        if (videoInfo.FormatName.IndexOf("image") > -1) return false;
        //        if (videoInfo.FormatName.IndexOf("png_pipe") > -1) return false;

        //        ffProbe = null;
        //        videoInfo = null;

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}
        #endregion

        #region Trata list e grid
        public static void ConfigurarColunasListView(ref ListView lsvGen, params string[] Colunas)
        {
            ColumnHeader ch = null;

            lsvGen.Items.Clear();
            lsvGen.Columns.Clear();
            lsvGen.View = View.Details;
            lsvGen.FullRowSelect = true;

            foreach (string coluna in Colunas)
            {
                ch = lsvGen.Columns.Add(coluna);
                ch.Name = coluna;
            }

            AjustarColunasListView(ref lsvGen);
        }
        public static void ConfigurarColunasListView(ref ListView lsvGen, params ColunasListView[] Colunas)
        {
            ColumnHeader ch = null;

            lsvGen.Items.Clear();
            lsvGen.Columns.Clear();
            lsvGen.View = View.Details;
            lsvGen.FullRowSelect = true;

            foreach (ColunasListView coluna in Colunas)
            {
                ch = new ColumnHeader();
                ch.Text = coluna.Texto;
                ch.Name = coluna.Nome;
                ch.TextAlign = coluna.Alinhamento;
                lsvGen.Columns.Add(ch);
            }
        }
        public static void ConfigurarDataGridView(ref DataGridView dgvGen)
        {
            dgvGen.EnableHeadersVisualStyles = false;
            dgvGen.ColumnHeadersDefaultCellStyle.ForeColor = SystemColors.ActiveCaptionText;
            dgvGen.ColumnHeadersDefaultCellStyle.BackColor = SystemColors.ActiveCaption;
            dgvGen.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvGen.ScrollBars = ScrollBars.Both;
            dgvGen.ShowCellErrors = false;
            dgvGen.ShowRowErrors = false;
            dgvGen.CausesValidation = false;
            dgvGen.MultiSelect = false;
            dgvGen.SelectionMode = DataGridViewSelectionMode.CellSelect;
        }
        public static void ConfigurarColunasDataGridView(ref DataGridView dgvGen, params ColunasGrid[] Colunas)
        {
            DataGridViewColumn dgvc;
            DataGridViewCellStyle dgvcs;
            try
            {
                // configura ggrid
                ConfigurarDataGridView(ref dgvGen);
                //geral
                dgvGen.Columns.Clear();
                dgvGen.Rows.Clear();

                foreach (ColunasGrid Coluna in Colunas)
                {
                    switch (Coluna.TipoCelula)
                    {
                        case EtipoCelula.Texto:
                            dgvc = new DataGridViewTextBoxColumn();
                            break;
                        case EtipoCelula.Combo:
                            dgvc = new DataGridViewComboBoxColumn();
                            break;
                        case EtipoCelula.Botao:
                            dgvc = new DataGridViewButtonColumn();
                            break;
                        case EtipoCelula.Imagem:
                            dgvc = new DataGridViewImageColumn();
                            break;
                        case EtipoCelula.CheckBox:
                            dgvc = new DataGridViewCheckBoxColumn();
                            break;
                        default:
                            dgvc = new DataGridViewTextBoxColumn();
                            break;
                    }

                    dgvcs = new DataGridViewCellStyle();

                    dgvcs.Alignment = Coluna.Alinhamento;
                    dgvcs.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

                    dgvc.Name = Coluna.Nome.Trim();
                    dgvc.AutoSizeMode = Coluna.AutoSize;
                    dgvc.HeaderText = Coluna.Cabecalho.Trim();
                    dgvc.SortMode = Coluna.Classificacao;
                    dgvc.ReadOnly = Coluna.Leitura;
                    dgvc.Visible = Coluna.Visivel;
                    dgvc.Width = 50;

                    dgvc.DefaultCellStyle = dgvcs;

                    dgvGen.Columns.Add(dgvc);
                }
            }
            catch { }
        }
        public static void FormatarDadosDataGridView(ref DataGridView dgv, DataSet ds)
        {
            int _QtLinhas = ds.Tables[0].Rows.Count;
            int _QtColunas = dgv.ColumnCount;
            int _Linha = 0;

            try
            {
                dgv.Rows.Clear();
                dgv.Rows.Add(_QtLinhas);

                foreach (DataRow _dr in ds.Tables[0].Rows)
                {
                    foreach (DataGridViewColumn _coluna in dgv.Columns)
                    {
                        dgv.Rows[_Linha].Cells[_coluna.Name].Value = _dr[_coluna.Name].ToString();
                    }
                    _Linha++;
                }
            }
            catch (Exception ex)
            {
                new Exception(ex.Message);
            }
        }
        public static void AjustarColunasListView(ref ListView lvwListView)
        {
            for (int c = 0; c < lvwListView.Columns.Count; c++)
            {
                if (lvwListView.Columns[c].Width > 0)
                {
                    lvwListView.AutoResizeColumn(c, ColumnHeaderAutoResizeStyle.ColumnContent);
                    lvwListView.AutoResizeColumn(c, ColumnHeaderAutoResizeStyle.HeaderSize);
                }
            }
        }
        public static void AjustarColunasListViewCompleta(ref ListView lvwListView)
        {
            for (int c = 0; c < lvwListView.Columns.Count; c++)
            {

                lvwListView.AutoResizeColumn(c, ColumnHeaderAutoResizeStyle.ColumnContent);
                lvwListView.AutoResizeColumn(c, ColumnHeaderAutoResizeStyle.HeaderSize);

            }
        }
        public static ListViewItem ConfigurarSubitems(ref ListView lsvGen, string TextoItem)
        {
            ListViewItem _item = lsvGen.Items.Add(TextoItem);
            bool _primeiraVez = true;

            foreach (ColumnHeader _coluna in lsvGen.Columns)
            {
                if (_primeiraVez)
                {
                    _primeiraVez = false;
                }
                else
                {
                    _item.SubItems.Add(new ListViewItem.ListViewSubItem { Text = "", Name = _coluna.Name });
                }
            }

            return _item;
        }
        #endregion

        #region Combos
        public static void SelecionarComboInfo(ref ComboBox cboCombo, string Valor)
        {
            if (Valor == null)
            {
                cboCombo.SelectedIndex = 0;
                return;
            }

            ComboInfo gr = null;
            for (int i = 0; i < cboCombo.Items.Count; i++)
            {
                gr = (ComboInfo)cboCombo.Items[i];

                if (gr.Codigo.Trim().Equals(Valor.Trim()))
                {
                    cboCombo.SelectedIndex = i;
                    break;
                }
            }
            gr = null;
        }
        public static void SelecionarDescricaoComboInfo(ref ComboBox cboCombo, string Descricao)
        {
            ComboInfo gr = null;

            for (int i = 0; i < cboCombo.Items.Count; i++)
            {
                gr = (ComboInfo)cboCombo.Items[i];

                if (gr.Descricao.Trim().Equals(Descricao.Trim()))
                {
                    cboCombo.SelectedIndex = i;
                    break;
                }
            }
            gr = null;
        }
        public static void SelecionarComboInfo2(ref ComboBox cboCombo, string Valor)
        {
            ComboInfo2 gr = null;
            for (int i = 0; i < cboCombo.Items.Count; i++)
            {
                gr = (ComboInfo2)cboCombo.Items[i];

                if (gr.Codigo2.Trim().Equals(Valor.Trim()))
                {
                    cboCombo.SelectedIndex = i;
                    break;
                }
            }
            gr = null;
        }
        public static void SelecionarComboInfoTSC(ref ToolStripComboBox cboCombo, string Valor)
        {
            ComboInfo gr = null;
            for (int i = 0; i < cboCombo.Items.Count; i++)
            {
                gr = (ComboInfo)cboCombo.Items[i];

                if (gr.Codigo.Trim().Equals(Valor.Trim()))
                {
                    cboCombo.SelectedIndex = i;
                    break;
                }
            }
            gr = null;
        }
        public static void SelecionarComboInfo2TSC(ref ToolStripComboBox cboCombo, string Valor)
        {
            ComboInfo2 gr = null;
            for (int i = 0; i < cboCombo.Items.Count; i++)
            {
                gr = (ComboInfo2)cboCombo.Items[i];

                if (gr.Codigo1.Trim().Equals(Valor.Trim()))
                {
                    cboCombo.SelectedIndex = i;
                    break;
                }
            }
            gr = null;
        }
        #endregion

        #region ... Teclas ...
        public static bool TeclaEnter(KeyPressEventArgs e)
        {
            return ((Keys)e.KeyChar == Keys.Enter);
        }

        public static bool TeclaDel(KeyPressEventArgs e)
        {
            return ((Keys)e.KeyChar == Keys.Delete);
        }
        public static bool TeclaDel(KeyEventArgs e)
        {
            return ((Keys)e.KeyData == Keys.Delete);
        }
        public static bool TeclaEscape(KeyPressEventArgs e)
        {
            return ((Keys)e.KeyChar == Keys.Escape);
        }

        public static bool TeclaBKSpace(KeyPressEventArgs e)
        {
            return ((Keys)e.KeyChar == Keys.Back);
        }

        public static bool TeclaSetas(KeyEventArgs e)
        {
            return (e.KeyCode == Keys.Up ||
                    e.KeyCode == Keys.Down ||
                    e.KeyCode == Keys.Left ||
                    e.KeyCode == Keys.Right);
        }
        #endregion

        #region Trata BancoDadosAccess
        public static void CompactAndRepairDatabase(string dataBasePath, string dataBaseName, string dataBasePathBKP, string dataBaseNameBKP)
        {
            var dao = new access.DBEngine();

            try
            {
                dao.CompactDatabase(dataBasePath + dataBaseName, dataBasePathBKP + dataBaseNameBKP);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Tratamento Forms
        public static void AbrirFormFilho(Form formPai, Form formFilho)
        {
            formFilho.MdiParent = formPai;  // Define o formPai como o MdiParent do formFilho
            formFilho.Show();               // Exibe o formulário filho
            formFilho.WindowState = FormWindowState.Maximized;  // Opcional: abre o formulário maximizado
        }
        #endregion

        #region Tratamento para Data e Horas
        public static double ConvertToHours(string ValorString)
        {
            double months = 0;
            double weeks = 0;
            double days = 0;
            double hours = 0;

            // Regex para meses
            var regex = new Regex(@"(\d+)\s*mo");
            var match = regex.Match(ValorString);
            if (match.Success)
            {
                months = Convert.ToDouble(match.Groups[1].Value);
            }

            // Regex para semanas
            regex = new Regex(@"(\d+)\s*w");
            match = regex.Match(ValorString);
            if (match.Success)
            {
                weeks = Convert.ToDouble(match.Groups[1].Value);
            }

            // Regex para dias
            regex = new Regex(@"(\d+)\s*d");
            match = regex.Match(ValorString);
            if (match.Success)
            {
                days = Convert.ToDouble(match.Groups[1].Value);
            }

            // Regex para horas
            regex = new Regex(@"(\d+)\s*h");
            match = regex.Match(ValorString);
            if (match.Success)
            {
                hours = Convert.ToDouble(match.Groups[1].Value);
            }

            // Converte tudo para horas, considerando 1 mês = 160 horas, 1 semana = 5 dias, 1 dia = 8 horas
            return (months * 160) + (weeks * 5 * 8) + (days * 8) + hours;

        }
        #endregion
    }
    public class ColunasGridInfo
    {
        private string _Nome;

        public string Nome
        {
            get { return _Nome; }
            set { _Nome = value; }
        }

        private int _Posicao;

        public int Posicao
        {
            get { return _Posicao; }
            set { _Posicao = value; }
        }

        private string _Status;

        public string Status
        {
            get { return _Status; }
            set { _Status = value; }
        }

        private int _Tamanho;

        public int Tamanho
        {
            get { return _Tamanho; }
            set { _Tamanho = value; }
        }

        private string _Titulo;

        public string Titulo
        {
            get { return _Titulo; }
            set { _Titulo = value; }
        }

        public ColunasGridInfo(string Nome, int Posicao, string Status, int Tamanho, string Titulo)
        {
            _Nome = Nome;
            _Posicao = Posicao;
            _Status = Status;
            _Tamanho = Tamanho;
            _Titulo = Titulo;
        }
    }
    public class ColunasListView
    {
        string _Texto; public string Texto { get { return _Texto; } set { _Texto = value; } }
        string _Nome; public string Nome { get { return _Nome; } set { _Nome = value; } }
        System.Windows.Forms.HorizontalAlignment _Alinhamento = System.Windows.Forms.HorizontalAlignment.Left;
        public System.Windows.Forms.HorizontalAlignment Alinhamento { get { return _Alinhamento; } set { _Alinhamento = value; } }
        bool _AutoAjuste; public bool AutoAjuste { get { return _AutoAjuste; } set { _AutoAjuste = value; } }

        public ColunasListView(string Texto, string Nome, System.Windows.Forms.HorizontalAlignment Alinhamento, bool AutoAjuste)
        {
            _Texto = Texto;
            _Nome = Nome;
            _Alinhamento = Alinhamento;
            _AutoAjuste = AutoAjuste;
        }
    }
    public class ColunasGrid
    {
        private string _Nome; public string Nome { get { return _Nome; } set { _Nome = value; } }
        private DataGridViewContentAlignment _Alinhamento; public DataGridViewContentAlignment Alinhamento { get { return _Alinhamento; } set { _Alinhamento = value; } }
        private DataGridViewAutoSizeColumnMode _AutoSize; public DataGridViewAutoSizeColumnMode AutoSize { get { return _AutoSize; } set { _AutoSize = value; } }
        private string _Cabecalho; public string Cabecalho { get { return _Cabecalho; } set { _Cabecalho = value; } }
        private DataGridViewColumnSortMode _Classificacao; public DataGridViewColumnSortMode Classificacao { get { return _Classificacao; } set { _Classificacao = value; } }
        private bool _Leitura; public bool Leitura { get { return _Leitura; } set { _Leitura = value; } }
        private bool _Visivel; public bool Visivel { get { return _Visivel; } set { _Visivel = value; } }

        private EtipoCelula _TipoCelula;
        public EtipoCelula TipoCelula { get { return _TipoCelula; } set { _TipoCelula = value; } }

        public ColunasGrid(string pNome, string pCabecalho, bool pLeitura, bool pVisivel, EtipoCelula pTipoCelula = EtipoCelula.Texto,
                           DataGridViewContentAlignment pAlinhamento = DataGridViewContentAlignment.MiddleLeft,
                           DataGridViewAutoSizeColumnMode pAutoSize = DataGridViewAutoSizeColumnMode.AllCells,
                           DataGridViewColumnSortMode pClassificacao = DataGridViewColumnSortMode.NotSortable)
        {
            _Nome = pNome;
            _Alinhamento = pAlinhamento;
            _AutoSize = pAutoSize;
            _Cabecalho = pCabecalho;
            _Classificacao = pClassificacao;
            _Leitura = pLeitura;
            _Visivel = pVisivel;
            TipoCelula = pTipoCelula;
        }
    }
    public enum EtipoCelula
    {
        Imagem,
        Texto,
        Combo,
        Botao,
        CheckBox
    }
    public enum EtipoMovimento
    {
        Previsto = -1,
        Efetivo = 0
    }
    public class ComboInfo2
    {
        public string Codigo1;
        public string Codigo2;
        public string Codigo3;
        public string Descricao;

        public ComboInfo2(string _Codigo1, string _Codigo2, string _Descricao)
        {
            Codigo1 = _Codigo1;
            Codigo2 = _Codigo2;
            Descricao = _Descricao;
        }

        public ComboInfo2(string _Codigo1, string _Codigo2, string _Codigo3, string _Descricao)
        {
            Codigo1 = _Codigo1;
            Codigo2 = _Codigo2;
            Codigo3 = _Codigo3;
            Descricao = _Descricao;
        }

        public override string ToString()
        {
            return Descricao;
        }
    }
    public class ComboInfo
    {
        public string Codigo;
        public string Descricao;
        public ComboInfo(string _Codigo, string _Descricao)
        {
            Descricao = _Descricao;
            Codigo = _Codigo;
        }
        public override string ToString()
        {
            return Descricao;
        }
    }
}