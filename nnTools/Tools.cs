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

namespace nnTools
{
    public class Tools
    {
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

            System.Windows.Forms.Application.DoEvents();
        }
        public static void ExibirMensagemAlerta(string mensagem, string titulo)
        {
            MessageBox.Show(mensagem, titulo, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        case eTipoCelula.Texto:
                            dgvc = new DataGridViewTextBoxColumn();
                            break;
                        case eTipoCelula.Combo:
                            dgvc = new DataGridViewComboBoxColumn();
                            break;
                        case eTipoCelula.Botao:
                            dgvc = new DataGridViewButtonColumn();
                            break;
                        case eTipoCelula.Imagem:
                            dgvc = new DataGridViewImageColumn();
                            break;
                        case eTipoCelula.CheckBox:
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

        private eTipoCelula _TipoCelula;
        public eTipoCelula TipoCelula { get { return _TipoCelula; } set { _TipoCelula = value; } }

        public ColunasGrid(string pNome, string pCabecalho, bool pLeitura, bool pVisivel, eTipoCelula pTipoCelula = eTipoCelula.Texto,
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
    public enum eTipoCelula
    {
        Imagem,
        Texto,
        Combo,
        Botao,
        CheckBox
    }

}