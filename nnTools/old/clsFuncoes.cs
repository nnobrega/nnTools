//using System.Text.RegularExpressions;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Reflection;
//using System.Data;
//using System.Windows.Forms;
//using System.Drawing;
//using System.Collections;
//using System.ComponentModel;
//using System.Runtime.InteropServices;
//using System.Diagnostics;
//using DCX.ITLC.PCInv;
//using PRMLibrary.Client.Forms;
//using System.IO;
//using System.Net;
//using System.Management;
//using System.Drawing.Printing;
//using System.Configuration;
//using Oracle.DataAccess.Client;
//using System.Security.Cryptography;
//using PRMLibrary.Web.Comum;
//using PRMLibrary.Client.Negocio.Perfil;
//using System.Globalization;
//using PRMLibrary.Comum;
//using nnTools;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
//using System.Threading;

//namespace PRMLibrary.Client.Comum
//{
//    public class ColunasGridInfo
//    {
//        private string _Nome;

//        public string Nome
//        {
//            get { return _Nome; }
//            set { _Nome = value; }
//        }

//        private int _Posicao;

//        public int Posicao
//        {
//            get { return _Posicao; }
//            set { _Posicao = value; }
//        }

//        private string _Status;

//        public string Status
//        {
//            get { return _Status; }
//            set { _Status = value; }
//        }

//        private int _Tamanho;

//        public int Tamanho
//        {
//            get { return _Tamanho; }
//            set { _Tamanho = value; }
//        }

//        private string _Titulo;

//        public string Titulo
//        {
//            get { return _Titulo; }
//            set { _Titulo = value; }
//        }

//        public ColunasGridInfo(string Nome, int Posicao, string Status, int Tamanho, string Titulo)
//        {
//            _Nome = Nome;
//            _Posicao = Posicao;
//            _Status = Status;
//            _Tamanho = Tamanho;
//            _Titulo = Titulo;
//        }

//    }

//    public class Funcoes
//    {
//        public static string QueryPadrao = "select /*+ index (PKTGB)*/ count(*) from TGB where tiutil = '   '  and nuprod = ':1' ";
//        public static string QueryPadraoVeic = "select /*+ index (PKVEC)*/ count(*) from VEC where tiutil = 'PRO'  and nuprod = ':1' ";

//        public static string QueryPadrao_old = "select count(*) from TGB where tiutil = '   '  and nuprod = :1 ";
//        public static string QueryPadraoVeic_old = "select count(*) from VEC where tiutil = 'PRO'  and nuprod = :1 ";

//        public static string PastaSistemaRegistry = "PRM";

//        public enum eTipoFalha
//        {
//            Em_Aberto,
//            Finalizadas,
//            Pecas_Faltantes,
//            Todas
//        }

//        public enum eTipoExcecao
//        {
//            Data,
//            Excecao
//        }

//        public enum eTipoLinha
//        {
//            Agregado,
//            Veiculo
//        }

//        public static string CodigoErroTrataRetorno = "";

//        public static string MensagemFilaMQ = "";

//        private frmGauge frmG = new frmGauge();

//        [DllImport("KERNEL32.DLL", EntryPoint = "GetPrivateProfileStringW", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
//        private static extern int GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, string lpReturnString, int nSize, string lpFilename);

//        [DllImport("KERNEL32.DLL", EntryPoint = "WritePrivateProfileStringW", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
//        private static extern int WritePrivateProfileString(string lpAppName, string lpKeyName, string lpString, string lpFilename);

//        //private static System.Threading.Thread t = null;

//        //static frmGauge g = null;

//        public static bool PodeExcluirFilaMQ(string Fila)
//        {
//            bool result = true;

//            string[] FilasQueNaoPodemExcluir = new string[3];

//            FilasQueNaoPodemExcluir[0] = "PRINTER";
//            FilasQueNaoPodemExcluir[1] = "BPREPLY.MOT";
//            FilasQueNaoPodemExcluir[2] = "BPREPLY.NPFI";


//            for (int i = 0; i < FilasQueNaoPodemExcluir.Length; i++)
//            {
//                if (Fila.ToUpper().Trim().Contains(FilasQueNaoPodemExcluir[i]))
//                {
//                    result = false;
//                    break;
//                }
//            }

//            return result;
//        }

//        #region ... API_ScroolBar_ListView ...
//        const Int32 LVM_FIRST = 0x1000;
//        const Int32 LVM_SCROLL = LVM_FIRST + 20;
//        const int SBS_HORZ = 0;

//        [DllImport("user32.dll")]
//        static extern int GetScrollPos(System.IntPtr hWnd, int nBar);
//        [DllImport("user32.dll")]
//        static extern bool SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);
//        [DllImport("user32.dll")]
//        static extern bool LockWindowUpdate(IntPtr Handle);

//        //private int _hScrollValue = 0;
//        //Store the horizontal scroll value.
//        private int StoreHScrollValue(ref ListView lvwLista)
//        {
//            return GetScrollPos(lvwLista.Handle, SBS_HORZ);
//        }

//        //recover the old scroll position
//        private void RecoverHorizontalScroll(ref ListView lvwLista, int _hScrollValue)
//        {
//            LockWindowUpdate(lvwLista.Handle);
//            //Calculate the value the scroll needs to scroll back.
//            int dx = _hScrollValue - GetScrollPos(lvwLista.Handle, SBS_HORZ);
//            //Send the scroll message.
//            bool b = SendMessage(lvwLista.Handle, LVM_SCROLL, dx, 0);
//            LockWindowUpdate(IntPtr.Zero);
//        }

//        private int GetScrollValue(ref RichTextBox txtBox)
//        {
//            return GetScrollPos(txtBox.Handle, SBS_HORZ);
//        }

//        //recover the old scroll position
//        private void RecoverHorizontalScroll(ref RichTextBox txtBox, int _hScrollValue)
//        {
//            LockWindowUpdate(txtBox.Handle);
//            //Calculate the value the scroll needs to scroll back.
//            int dx = _hScrollValue - GetScrollPos(txtBox.Handle, SBS_HORZ);
//            //Send the scroll message.
//            bool b = SendMessage(txtBox.Handle, LVM_SCROLL, dx, 0);
//            LockWindowUpdate(IntPtr.Zero);
//        }
//        #endregion

//        #region ... Enums ...

//        public enum TipoOperacaoLinha
//        {
//            EntradaLinha = 1,
//            SaidaLinha = 2,
//            FinalLinha = 3,
//            LiberacaoFinal = 4
//        }

//        public enum TipoPesquisaKanban
//        {
//            Normal = 1,
//            SemKanban = 2,
//            Liberados = 3
//        }

//        public enum GearType
//        {
//            EtNormal = 1,
//            Et705 = 2,
//            Patio = 3,
//            Exportacao = 4,
//            NormalQRCode = 5,
//            FormacaoKit = 6
//        }

//        public enum LayoutReport
//        {
//            Frente = 1,
//            Verso = 2,
//            Completo = 3,
//            Previa = 4,
//        }

//        public enum AxisType
//        {
//            Dianteiro = 1,
//            Traseiro = 2,
//            TraseiroVS = 3,
//            TraseiroWLP = 4,
//            Geral = 5
//        }

//        public enum ScaleModeConstants
//        {
//            vbTwips = 0,
//            vbInches = 1,
//            vbCentimeters = 2,
//            vbMillimeters = 3,
//            vbPixels = 4
//        }

//        public enum TipoOperacao
//        {
//            Incluir = 0,
//            Alterar = 1,
//            Excluir = 2,
//            ExcluirTotal = 3,
//            IncluirTotal = 4
//        }

//        public enum PlausiMetodos
//        {
//            plIncluir = 1,
//            plAlterar = 2,
//            plExcluir = 3,
//            plAlterarCodes = 4
//        };

//        public enum AutoOperacoes
//        {
//            autoNormal = 0,
//            autoRepeat = 1,
//            autoCancel = 2,
//            autoNull = 3
//        };

//        public enum TipoDisponiveis
//        {
//            Agregado = 1,
//            PowerTrain = 2
//        };

//        public enum FGTipoLinha
//        {
//            NaoDefinido = -1,
//            Caminhao = 1,
//            Onibus = 2,
//            Agregado = 3,
//            Cabina = 4,
//            Motor = 5,
//            Eixo = 6,
//            Cambio = 7,
//            AgregadoDes = 8,
//            CaminhaoDes = 9,
//            OnibusDes = 10,
//            CabinaDes = 11,
//            Nenhum = 12
//        };

//        public enum PRMSeq
//        {
//            PRMSeq = 0,
//            PRMSeqAcopla = 1,
//            PRMSeqKit = 2,
//            PRMSeqKitEixo = 3,
//            PRMSeqPintura = 4,
//            PRMSeqGalpao = 5,
//            PRMSeqElevZ = 6,
//            PRMSeqMfBus = 7,
//            PRMSeqKitCkd = 8,
//            PRMSeqQuadros = 9,
//            PRMSeqLote = 11,
//            PRMSeqQuadrosOnibus = 12,
//            PRMSeqLogistica = 14,
//            PRMSEQPuffCKD = 15,
//            PRMSeqPufO500 = 16,
//            PRMSeqPufMang = 17,
//            PRMPortal = 99
//        }

//        public enum TipoModoDeMontagem
//        {
//            Sequencia = 1,
//            Linha = 2
//        };

//        public enum Fabrica
//        {
//            SBC = 0,
//            JDF = 1
//        };

//        #endregion

//        public static List<Control> FindAllControls(Control container)
//        {
//            List<Control> controlList = new List<Control>();
//            FindAllControls(container, controlList);
//            return controlList;
//        }

//        public static void FindAllControls(Control container, IList<Control> ctrlList)
//        {
//            foreach (Control ctrl in container.Controls)
//            {
//                if (ctrl.Controls.Count == 0)
//                    ctrlList.Add(ctrl);
//                else
//                    FindAllControls(ctrl, ctrlList);
//            }
//        }

//        public static void AjusteTela2(Form formShow)
//        {
//            float dpiX;
//            Graphics graphics = formShow.CreateGraphics();
//            dpiX = graphics.DpiX;

//            if (dpiX >= 120)
//            {
//                formShow.Font = new Font(formShow.Font.FontFamily, formShow.Font.SizeInPoints * 100 / 96);
//                float fontSize = formShow.Font.Size;

//                List<Control> ctrs = FindAllControls(formShow);
//                for (int i = 0; i < ctrs.Count; i++)
//                {
//                    ctrs[i].Font = new Font(formShow.Font.FontFamily, fontSize);
//                }


//                List<Control> toolStrips = GetAllControls<ToolStrip>(formShow);

//                for (int i = 0; i < toolStrips.Count; i++)
//                {
//                    ((ToolStrip)toolStrips[i]).Font = new Font(formShow.Font.FontFamily, fontSize);

//                    for (int z = 0; z < ((ToolStrip)toolStrips[i]).Items.Count; z++)
//                    {
//                        ((ToolStrip)toolStrips[i]).Items[z].Font = new Font(formShow.Font.FontFamily, fontSize);
//                    }

//                }


//            }
//        }

//        public static void AjusteControles(TabControl formShow)
//        {
//            float dpiX;
//            Graphics graphics = formShow.CreateGraphics();
//            dpiX = graphics.DpiX;

//            if (dpiX >= 120)
//            {
//                //formShow.Font = new Font(formShow.Font.FontFamily, formShow.Font.SizeInPoints * 80 / 96);
//                //float fontSize = formShow.Font.Size;

//                //List<Control> ctrs = FindAllControls(formShow);
//                //for (int i = 0; i < ctrs.Count; i++)
//                //{
//                //    ctrs[i].Font = new Font(formShow.Font.FontFamily, fontSize);
//                //}


//                //List<Control> toolStrips = GetAllControls<ToolStrip>(formShow);

//                //for (int i = 0; i < toolStrips.Count; i++)
//                //{
//                //    ((ToolStrip)toolStrips[i]).Font = new Font(formShow.Font.FontFamily, fontSize);

//                //    for (int z = 0; z < ((ToolStrip)toolStrips[i]).Items.Count; z++)
//                //    {
//                //        ((ToolStrip)toolStrips[i]).Items[z].Font = new Font(formShow.Font.FontFamily, fontSize);
//                //    }

//                //}


//                for (int z = 0; z < formShow.TabPages.Count; z++)
//                {

//                    List<Control> labels = GetAllControls<Label>(formShow);
//                    for (int i = 0; i < labels.Count; i++)
//                    {
//                        //((Label)labels[i]).AutoSize = false;
//                        ((Label)labels[i]).Font = new Font(((Label)labels[i]).Font.FontFamily, ((Label)labels[i]).Font.SizeInPoints * 80 / 96);
//                    }

//                    List<Control> checkboxs = GetAllControls<CheckBox>(formShow.TabPages[z]);
//                    for (int i = 0; i < checkboxs.Count; i++)
//                    {
//                        //((Label)labels[i]).AutoSize = false;
//                        ((CheckBox)checkboxs[i]).Font = new Font(((CheckBox)checkboxs[i]).Font.FontFamily, ((CheckBox)checkboxs[i]).Font.SizeInPoints * 80 / 96);
//                    }

//                    List<Control> radioboxes = GetAllControls<RadioButton>(formShow.TabPages[z]);
//                    for (int i = 0; i < radioboxes.Count; i++)
//                    {
//                        //((Label)labels[i]).AutoSize = false;
//                        ((RadioButton)radioboxes[i]).Font = new Font(((RadioButton)radioboxes[i]).Font.FontFamily, ((RadioButton)radioboxes[i]).Font.SizeInPoints * 80 / 96);
//                    }


//                }



//            }
//        }

//        public static void AjusteControles(UserControl formShow)
//        {
//            float dpiX;
//            Graphics graphics = formShow.CreateGraphics();
//            dpiX = graphics.DpiX;

//            if (dpiX >= 120)
//            {
//                //formShow.Font = new Font(formShow.Font.FontFamily, formShow.Font.SizeInPoints * 80 / 96);
//                //float fontSize = formShow.Font.Size;

//                //List<Control> ctrs = FindAllControls(formShow);
//                //for (int i = 0; i < ctrs.Count; i++)
//                //{
//                //    ctrs[i].Font = new Font(formShow.Font.FontFamily, fontSize);
//                //}


//                //List<Control> toolStrips = GetAllControls<ToolStrip>(formShow);

//                //for (int i = 0; i < toolStrips.Count; i++)
//                //{
//                //    ((ToolStrip)toolStrips[i]).Font = new Font(formShow.Font.FontFamily, fontSize);

//                //    for (int z = 0; z < ((ToolStrip)toolStrips[i]).Items.Count; z++)
//                //    {
//                //        ((ToolStrip)toolStrips[i]).Items[z].Font = new Font(formShow.Font.FontFamily, fontSize);
//                //    }

//                //}

//                List<Control> labels = GetAllControls<Label>(formShow);
//                for (int i = 0; i < labels.Count; i++)
//                {
//                    //((Label)labels[i]).AutoSize = false;
//                    ((Label)labels[i]).Font = new Font(((Label)labels[i]).Font.FontFamily, ((Label)labels[i]).Font.SizeInPoints * 80 / 96);
//                }

//                List<Control> checkboxs = GetAllControls<CheckBox>(formShow);
//                for (int i = 0; i < checkboxs.Count; i++)
//                {
//                    //((Label)labels[i]).AutoSize = false;
//                    ((CheckBox)checkboxs[i]).Font = new Font(((CheckBox)checkboxs[i]).Font.FontFamily, ((CheckBox)checkboxs[i]).Font.SizeInPoints * 80 / 96);
//                }

//                List<Control> radioboxes = GetAllControls<RadioButton>(formShow);
//                for (int i = 0; i < radioboxes.Count; i++)
//                {
//                    //((Label)labels[i]).AutoSize = false;
//                    ((RadioButton)radioboxes[i]).Font = new Font(((RadioButton)radioboxes[i]).Font.FontFamily, ((RadioButton)radioboxes[i]).Font.SizeInPoints * 80 / 96);
//                }

//            }
//        }

//        public static Image resizeImage(Image imgToResize, Size size)
//        {
//            return (Image)(new Bitmap(imgToResize, size));
//        }

//        #region ... CalculoFrame ...
//        private static int CalculaTop(System.Windows.Forms.PictureBox picImPri)
//        {
//            return (picImPri.Height / 2) - (CalcularHeight(picImPri) / 2);
//        }
//        private static int CalcularBottom(System.Windows.Forms.PictureBox picImPri)
//        {
//            return CalculaTop(picImPri) + CalcularHeight(picImPri);
//        }
//        private static int CalcularLeft(System.Windows.Forms.PictureBox picImPri)
//        {
//            return (picImPri.Width / 2) - (CalcularWidth(picImPri) / 2);
//        }
//        private static int CalcularRight(System.Windows.Forms.PictureBox picImPri)
//        {
//            return CalcularLeft(picImPri) + CalcularWidth(picImPri);
//        }
//        private static int CalcularHeight(System.Windows.Forms.PictureBox picImPri)
//        {
//            PropertyInfo pInfo = picImPri.GetType().GetProperty("ImageRectangle",
//            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

//            Rectangle rectangle = (Rectangle)pInfo.GetValue(picImPri, null);

//            return rectangle.Size.Height;
//        }
//        private static int CalcularWidth(System.Windows.Forms.PictureBox picImPri)
//        {
//            PropertyInfo pInfo = picImPri.GetType().GetProperty("ImageRectangle",
//            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

//            Rectangle rectangle = (Rectangle)pInfo.GetValue(picImPri, null);

//            return rectangle.Size.Width;
//        }

//        #endregion

//        public static byte[] MergeImagensPrimaria_Secundaria(Image imgPrimaria, Image imgSecundaria, int posicao_X, int posicao_Y)
//        {
//            int width = imgPrimaria.Width;
//            int height = imgPrimaria.Height;
//            byte[] newfileBytes = null;

//            int iLeft = 0; // CalcularLeft(imgOriginal);
//            int iTopo = 0; // CalculaTop(imgOriginal);

//            Bitmap imgFinal = new Bitmap(width, height);
//            Graphics g = Graphics.FromImage(imgFinal);

//            g.DrawImage(imgPrimaria, new Point(iLeft, iTopo));
//            g.DrawImage(imgSecundaria, new Point(posicao_X, posicao_Y));
//            g.Dispose();

//            using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
//            {
//                imgFinal.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
//                stream.Close();
//                newfileBytes = stream.ToArray();
//            }

//            return newfileBytes;
//        }

//        public static byte[] MergeImagens(Image imgOriginal, Image imgMerge, int posicao_X, int posicao_Y)
//        {
//            int width = imgOriginal.Width;
//            int height = imgOriginal.Height;
//            byte[] newfileBytes = null;

//            int iLeft = 0; // CalcularLeft(imgOriginal);
//            int iTopo = 0; // CalculaTop(imgOriginal);

//            Bitmap imgFinal = new Bitmap(width, height);
//            Graphics g = Graphics.FromImage(imgFinal);

//            int iW = ((imgOriginal.Size.Height * 10) / 100);
//            int iH = iW;

//            Image newImgMerge = resizeImage(imgMerge, new Size(iW, iH));

//            posicao_X = posicao_X + iLeft;
//            posicao_Y = posicao_Y + iTopo;

//            switch (((eDirecaoPonteiro)imgMerge.Tag))
//            {
//                case eDirecaoPonteiro.Direita:
//                    posicao_X = posicao_X - iW;
//                    posicao_Y = posicao_Y - (iH / 2);
//                    break;
//                case eDirecaoPonteiro.Esquerda:
//                    posicao_Y = posicao_Y - (iH / 2);
//                    break;
//                case eDirecaoPonteiro.Cima:
//                    posicao_X = posicao_X - (iW / 2);
//                    break;
//                case eDirecaoPonteiro.Baixo:
//                    posicao_X = posicao_X - (iW / 2);
//                    posicao_Y = posicao_Y - iH;
//                    break;
//                case eDirecaoPonteiro.Circulo:
//                    posicao_X = posicao_X - (iW / 2);
//                    posicao_Y = posicao_Y - (iH / 2);
//                    break;
//            }

//            g.DrawImage(imgOriginal, new Point(iLeft, iTopo));
//            g.DrawImage(newImgMerge, new Point(posicao_X, posicao_Y));
//            g.Dispose();

//            using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
//            {
//                imgFinal.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
//                stream.Close();
//                newfileBytes = stream.ToArray();
//            }

//            return newfileBytes;
//        }

//        public static byte[] MergeImagens(PictureBox imgOriginal, Image imgMerge, int posicao_X, int posicao_Y)
//        {
//            int width = imgOriginal.Width;
//            int height = imgOriginal.Height;
//            byte[] newfileBytes = null;

//            int iLeft = CalcularLeft(imgOriginal);
//            int iTopo = CalculaTop(imgOriginal);

//            Bitmap imgFinal = new Bitmap(width, height);
//            Graphics g = Graphics.FromImage(imgFinal);

//            int iW = ((imgOriginal.Size.Height * 5) / 100);
//            int iH = iW;

//            Image newImgMerge = resizeImage(imgMerge, new Size(iW, iH));

//            posicao_X = posicao_X + iLeft;
//            posicao_Y = posicao_Y + iTopo;

//            switch (((eDirecaoPonteiro)imgMerge.Tag))
//            {
//                case eDirecaoPonteiro.Direita:
//                    posicao_X = posicao_X - iW;
//                    posicao_Y = posicao_Y - (iH / 2);
//                    break;
//                case eDirecaoPonteiro.Esquerda:
//                    posicao_Y = posicao_Y - (iH / 2);
//                    break;
//                case eDirecaoPonteiro.Cima:
//                    posicao_X = posicao_X - (iW / 2);
//                    break;
//                case eDirecaoPonteiro.Baixo:
//                    posicao_X = posicao_X - (iW / 2);
//                    posicao_Y = posicao_Y - iH;
//                    break;
//                case eDirecaoPonteiro.Circulo:
//                    posicao_X = posicao_X - (iW / 2);
//                    posicao_Y = posicao_Y - (iH / 2);
//                    break;
//            }

//            g.DrawImage(imgOriginal.Image, new Point(iLeft, iTopo));
//            g.DrawImage(newImgMerge, new Point(posicao_X, posicao_Y));
//            g.Dispose();

//            using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
//            {
//                imgFinal.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
//                stream.Close();
//                newfileBytes = stream.ToArray();
//            }

//            return newfileBytes;
//        }

//        public static void AjusteTela(Form formShow)
//        {
//            float dpiX;
//            Graphics graphics = formShow.CreateGraphics();
//            dpiX = graphics.DpiX;

//            if (dpiX >= 120)
//            {
//                //formShow.Font = new Font(formShow.Font.FontFamily, formShow.Font.SizeInPoints * 80 / 96);
//                //float fontSize = formShow.Font.Size;

//                //List<Control> ctrs = FindAllControls(formShow);
//                //for (int i = 0; i < ctrs.Count; i++)
//                //{
//                //    ctrs[i].Font = new Font(formShow.Font.FontFamily, fontSize);
//                //}

//                //List<Control> toolStrips = GetAllControls<ToolStrip>(formShow);

//                //for (int i = 0; i < toolStrips.Count; i++)
//                //{
//                //    ((ToolStrip)toolStrips[i]).Font = new Font(formShow.Font.FontFamily, fontSize);

//                //    for (int z = 0; z < ((ToolStrip)toolStrips[i]).Items.Count; z++)
//                //    {
//                //        ((ToolStrip)toolStrips[i]).Items[z].Font = new Font(formShow.Font.FontFamily, fontSize);
//                //    }

//                //}

//                //List<Control> labels = GetAllControls<Label>(formShow);
//                //for (int i = 0; i < labels.Count; i++)
//                //{
//                //    //((Label)labels[i]).AutoSize = false;
//                //    ((Label)labels[i]).Font = new Font(((Label)labels[i]).Font.FontFamily, ((Label)labels[i]).Font.SizeInPoints * 80 / 96);
//                //}

//                //List<Control> checkboxs = GetAllControls<CheckBox>(formShow);
//                //for (int i = 0; i < checkboxs.Count; i++)
//                //{
//                //    //((Label)labels[i]).AutoSize = false;
//                //    ((CheckBox)checkboxs[i]).Font = new Font(((CheckBox)checkboxs[i]).Font.FontFamily, ((CheckBox)checkboxs[i]).Font.SizeInPoints * 80 / 96);
//                //}

//                //List<Control> radioboxes = GetAllControls<RadioButton>(formShow);
//                //for (int i = 0; i < radioboxes.Count; i++)
//                //{
//                //    //((Label)labels[i]).AutoSize = false;
//                //    ((RadioButton)radioboxes[i]).Font = new Font(((RadioButton)radioboxes[i]).Font.FontFamily, ((RadioButton)radioboxes[i]).Font.SizeInPoints * 80 / 96);
//                //}

//                List<Control> TodosControles = Funcoes.FindAllControls(formShow);

//                for (int i = 0; i < TodosControles.Count; i++)
//                {
//                    switch (TodosControles[i].GetType().Name)
//                    {
//                        case "Label":
//                            ((Label)TodosControles[i]).Font = new Font(((Label)TodosControles[i]).Font.FontFamily, ((Label)TodosControles[i]).Font.SizeInPoints * 80 / 96);
//                            break;

//                        case "CheckBox":
//                            ((CheckBox)TodosControles[i]).Font = new Font(((CheckBox)TodosControles[i]).Font.FontFamily, ((CheckBox)TodosControles[i]).Font.SizeInPoints * 80 / 96);
//                            break;

//                        case "RadioButton":
//                            ((RadioButton)TodosControles[i]).Font = new Font(((RadioButton)TodosControles[i]).Font.FontFamily, ((RadioButton)TodosControles[i]).Font.SizeInPoints * 80 / 96);
//                            break;

//                        default:
//                            break;
//                    }
//                }
//            }
//        }

//        public static DateTime Convert_ToDateTime(string Data)
//        {
//            return Convert.ToDateTime(Data, new System.Globalization.CultureInfo("pt-BR", false).DateTimeFormat);
//        }

//        #region ... Objetos ...

//        public static Image ObterImagemSistema(string Sistema)
//        {
//            Image TMP = null;
//            switch (Sistema)
//            {
//                case "PRMAPOIO":
//                    TMP = Properties.Resources.PRMAPOIO;
//                    break;

//                case "PRMALARME":
//                    TMP = Properties.Resources.PRMALARME;
//                    break;

//                case "PRMCHECKCOM":
//                    TMP = Properties.Resources.PRMCHECKCOM;
//                    break;


//                case "PRMCTRACCESS":
//                    TMP = Properties.Resources.PRMCTRACCESS;
//                    break;


//                case "PRMKANBAN":
//                    TMP = Properties.Resources.PRMKANBAN;
//                    break;


//                case "PRMMAPLIN":
//                    TMP = Properties.Resources.PRMMAPLIN;
//                    break;


//                case "PRMMOBILE":
//                    TMP = Properties.Resources.PRMMOBILE;
//                    break;


//                case "PRMPLANLIN":
//                    TMP = Properties.Resources.PRMPLANLIN;
//                    break;


//                case "PRMPRINT":
//                    TMP = Properties.Resources.PRMPRINT;
//                    break;


//                case "PRMSEQ":
//                    TMP = Properties.Resources.PRMSEQ;
//                    break;

//                case "PRMSEQLOG":
//                    TMP = Properties.Resources.PRMSEQLOG;
//                    break;

//                case "PRMSEQPUFFCKD":
//                    TMP = Properties.Resources.PRMSEQPuffCKD;
//                    break;

//                case "PRMSEQPUFO500":
//                    TMP = Properties.Resources.PRMSEQPufO500;
//                    break;

//                case "PRMSEQPUFMANG":
//                    TMP = Properties.Resources.PRMSEQPufMang;
//                    break;

//                case "SGPCLIENT":
//                    TMP = Properties.Resources.SGPClient;
//                    break;

//                case "PRMSEQ1044":
//                case "PRMSEQACOPLA":
//                    TMP = Properties.Resources.PRMSeqAcopla;
//                    break;

//                case "PRMSEQCKD":
//                case "PRMSEQKIT":
//                    TMP = Properties.Resources.PRMSeqKit;
//                    break;

//                case "PRMSEQKITEIXO":
//                    TMP = Properties.Resources.PRMSeqKitEixo;
//                    break;

//                case "PRMSEQMON":
//                    TMP = Properties.Resources.PRMSEQMON;
//                    break;


//                case "PRMTRAMIT":
//                    TMP = Properties.Resources.PRMTRAMIT;
//                    break;


//                case "PRMVARPROD":
//                    TMP = Properties.Resources.PRMVARPROD;
//                    break;


//                case "PRMVINC":
//                    TMP = Properties.Resources.PRMVINC;
//                    break;

//                default:
//                    TMP = Properties.Resources._Caminhao;
//                    break;
//            }

//            return TMP;
//        }

//        public static string GetCodigoGrupo(string GrupoSelecionado)
//        {
//            string GetTipoLinha = "";

//            switch (GrupoSelecionado)
//            {
//                case "Caminhão":
//                case "Caminhões":
//                    GetTipoLinha = "TR";
//                    break;
//                case "Ônibus":
//                    GetTipoLinha = "ON";
//                    break;
//                case "Cabina":
//                case "Cabinas":
//                    GetTipoLinha = "CB";
//                    break;
//                case "Motor":
//                case "Motores":
//                    GetTipoLinha = "MO";
//                    break;
//                case "Eixo":
//                case "Eixos":
//                    GetTipoLinha = "EI";
//                    break;
//                case "Câmbio":
//                case "Câmbios":
//                    GetTipoLinha = "CA";
//                    break;

//            }


//            return GetTipoLinha;
//        }

//        public static string GetTextoTipoLinha(string GrupoSelecionado)
//        {
//            string GetTipoLinha = "";

//            switch (GrupoSelecionado)
//            {
//                case "1":
//                case "TR":
//                    GetTipoLinha = "Caminhão";
//                    break;
//                case "2":
//                case "ON":
//                    GetTipoLinha = "Ônibus";
//                    break;
//                case "4":
//                case "CB":
//                    GetTipoLinha = "Cabina";
//                    break;
//                case "5":
//                case "MO":
//                    GetTipoLinha = "Motor";
//                    break;
//                case "6":
//                case "EI":
//                    GetTipoLinha = "Eixo";
//                    break;
//                case "7":
//                case "CA":
//                    GetTipoLinha = "Câmbio";
//                    break;

//            }


//            return GetTipoLinha;
//        }

//        public static Funcoes.FGTipoLinha GetTipoLinha(string GrupoSelecionado)
//        {
//            Funcoes.FGTipoLinha GetTipoLinha = Funcoes.FGTipoLinha.NaoDefinido;

//            switch (GrupoSelecionado)
//            {
//                case "1":
//                case "TR":
//                    GetTipoLinha = Funcoes.FGTipoLinha.Caminhao;
//                    break;
//                case "2":
//                case "ON":
//                    GetTipoLinha = Funcoes.FGTipoLinha.Onibus;
//                    break;
//                case "4":
//                case "CB":
//                    GetTipoLinha = Funcoes.FGTipoLinha.Cabina;
//                    break;
//                case "5":
//                case "MO":
//                    GetTipoLinha = Funcoes.FGTipoLinha.Motor;
//                    break;
//                case "6":
//                case "EI":
//                    GetTipoLinha = Funcoes.FGTipoLinha.Eixo;
//                    break;
//                case "7":
//                case "CA":
//                    GetTipoLinha = Funcoes.FGTipoLinha.Cambio;
//                    break;
//                case "GE":
//                    GetTipoLinha = Funcoes.FGTipoLinha.Caminhao;
//                    break;

//            }

//            return GetTipoLinha;
//        }

//        public static Funcoes.FGTipoLinha GetTipoLinha(bool pbolDifAgregados, string QualAmbiente)
//        {
//            Funcoes.FGTipoLinha GetTipoLinha = Funcoes.FGTipoLinha.NaoDefinido;

//            if (!pbolDifAgregados)
//            {
//                // ******************************
//                // *************SBC**************
//                // ******************************
//                // Producao
//                if ((QualAmbiente == "GPE1") || (QualAmbiente == "GPE2") || (QualAmbiente == "GPE3") || (QualAmbiente == "GPE4") || (QualAmbiente == "GPE5") || (QualAmbiente == "GPE6") || (QualAmbiente == "GPM1") || (QualAmbiente == "GPM2") || (QualAmbiente == "GPM3") || (QualAmbiente == "GPM4") || (QualAmbiente == "GPM5") || (QualAmbiente == "GPM6") || (QualAmbiente == "GPC1") || (QualAmbiente == "GPC2") || (QualAmbiente == "GPC3") || (QualAmbiente == "GPC4") || (QualAmbiente == "GPC5"))
//                {
//                    GetTipoLinha = Funcoes.FGTipoLinha.Agregado;
//                }
//                else if ((QualAmbiente == "GPV1") || (QualAmbiente == "GPV2") || (QualAmbiente == "GPV3"))
//                {
//                    GetTipoLinha = Funcoes.FGTipoLinha.Caminhao;
//                }
//                else if ((QualAmbiente == "GPV4") || (QualAmbiente == "GPV5"))
//                {
//                    GetTipoLinha = Funcoes.FGTipoLinha.Onibus;
//                }
//                else if ((QualAmbiente == "GPV6") || (QualAmbiente == "GPV7"))
//                {
//                    GetTipoLinha = Funcoes.FGTipoLinha.Cabina;

//                    // Homologacao
//                }
//                else if ((QualAmbiente == "GHE1") || (QualAmbiente == "GHE2") || (QualAmbiente == "GHE3") || (QualAmbiente == "GHE4") || (QualAmbiente == "GHE5") || (QualAmbiente == "GHE6") || (QualAmbiente == "GHM1") || (QualAmbiente == "GHM2") || (QualAmbiente == "GHM3") || (QualAmbiente == "GHM4") || (QualAmbiente == "GHM5") || (QualAmbiente == "GHM6") || (QualAmbiente == "GHC1") || (QualAmbiente == "GHC2") || (QualAmbiente == "GHC3") || (QualAmbiente == "GHC4") || (QualAmbiente == "GHC5"))
//                {
//                    GetTipoLinha = Funcoes.FGTipoLinha.Agregado;
//                }
//                else if ((QualAmbiente == "GHV1") || (QualAmbiente == "GHV2") || (QualAmbiente == "GHV3"))
//                {
//                    GetTipoLinha = Funcoes.FGTipoLinha.Caminhao;
//                }
//                else if ((QualAmbiente == "GHV4") || (QualAmbiente == "GHV5"))
//                {
//                    GetTipoLinha = Funcoes.FGTipoLinha.Onibus;
//                }
//                else if ((QualAmbiente == "GHV6") || (QualAmbiente == "GHV7"))
//                {
//                    GetTipoLinha = Funcoes.FGTipoLinha.Cabina;

//                    // Desenvolvimento
//                }
//                else if ((QualAmbiente == "GDE1") || (QualAmbiente == "GDE2"))
//                {
//                    GetTipoLinha = Funcoes.FGTipoLinha.Agregado;
//                }
//                else if (QualAmbiente == "GDV1")
//                {
//                    GetTipoLinha = Funcoes.FGTipoLinha.Cabina;
//                }
//                else if (QualAmbiente == "GDV2")
//                {
//                    GetTipoLinha = Funcoes.FGTipoLinha.Onibus;
//                    // ******************************
//                    // *************JDV**************
//                    // ******************************
//                    // Producao
//                }
//                else if (QualAmbiente == "JPV1")
//                {
//                    GetTipoLinha = Funcoes.FGTipoLinha.Caminhao;
//                }
//                else if (QualAmbiente == "JPV2")
//                {
//                    GetTipoLinha = Funcoes.FGTipoLinha.Cabina;

//                    // Homologacao
//                }
//                else if (QualAmbiente == "JHV1")
//                {
//                    GetTipoLinha = Funcoes.FGTipoLinha.Caminhao;
//                }
//                else if (QualAmbiente == "JHV2")
//                {
//                    GetTipoLinha = Funcoes.FGTipoLinha.Cabina;

//                    // Desenvolvimento
//                }
//                else if (QualAmbiente == "JDV1")
//                {
//                    GetTipoLinha = Funcoes.FGTipoLinha.Caminhao;
//                }
//                else if (QualAmbiente == "JDV2")
//                {
//                    GetTipoLinha = Funcoes.FGTipoLinha.Cabina;

//                }
//            }
//            else
//            {

//                // ******************************
//                // *************SBC**************
//                // ******************************
//                // Producao
//                if ((QualAmbiente == "GPE1") || (QualAmbiente == "GPE2") || (QualAmbiente == "GPE3") || (QualAmbiente == "GPE4") || (QualAmbiente == "GPE5") || (QualAmbiente == "GPE6"))
//                {
//                    GetTipoLinha = Funcoes.FGTipoLinha.Eixo;
//                }
//                else if ((QualAmbiente == "GPM1") || (QualAmbiente == "GPM2") || (QualAmbiente == "GPM3") || (QualAmbiente == "GPM4") || (QualAmbiente == "GPM5") || (QualAmbiente == "GPM6"))
//                {
//                    GetTipoLinha = Funcoes.FGTipoLinha.Motor;
//                }
//                else if ((QualAmbiente == "GPC1") || (QualAmbiente == "GPC2") || (QualAmbiente == "GPC3") || (QualAmbiente == "GPC4") || (QualAmbiente == "GPC5"))
//                {
//                    GetTipoLinha = Funcoes.FGTipoLinha.Cambio;
//                }
//                else if ((QualAmbiente == "GPV1") || (QualAmbiente == "GPV2") || (QualAmbiente == "GPV3"))
//                {
//                    GetTipoLinha = Funcoes.FGTipoLinha.Caminhao;
//                }
//                else if ((QualAmbiente == "GPV4") || (QualAmbiente == "GPV5"))
//                {
//                    GetTipoLinha = Funcoes.FGTipoLinha.Onibus;
//                }
//                else if ((QualAmbiente == "GPV6") || (QualAmbiente == "GPV7"))
//                {
//                    GetTipoLinha = Funcoes.FGTipoLinha.Cabina;

//                    // Homologacao
//                }
//                else if ((QualAmbiente == "GHE1") || (QualAmbiente == "GHE2") || (QualAmbiente == "GHE3") || (QualAmbiente == "GHE4") || (QualAmbiente == "GHE5") || (QualAmbiente == "GHE6") || (QualAmbiente == "GHM1") || (QualAmbiente == "GHM2") || (QualAmbiente == "GHM3") || (QualAmbiente == "GHM4") || (QualAmbiente == "GHM5") || (QualAmbiente == "GHM6") || (QualAmbiente == "GHC1") || (QualAmbiente == "GHC2") || (QualAmbiente == "GHC3") || (QualAmbiente == "GHC4") || (QualAmbiente == "GHC5"))
//                {
//                    GetTipoLinha = Funcoes.FGTipoLinha.Agregado;
//                }
//                else if ((QualAmbiente == "GHV1") || (QualAmbiente == "GHV2") || (QualAmbiente == "GHV3"))
//                {
//                    GetTipoLinha = Funcoes.FGTipoLinha.Caminhao;
//                }
//                else if ((QualAmbiente == "GHV4") || (QualAmbiente == "GHV5"))
//                {
//                    GetTipoLinha = Funcoes.FGTipoLinha.Onibus;
//                }
//                else if ((QualAmbiente == "GHV6") || (QualAmbiente == "GHV7"))
//                {
//                    GetTipoLinha = Funcoes.FGTipoLinha.Cabina;

//                    // Desenvilvimento
//                }
//                else if ((QualAmbiente == "GDE1") || (QualAmbiente == "GDE2"))
//                {
//                    GetTipoLinha = Funcoes.FGTipoLinha.AgregadoDes;
//                }
//                else if (QualAmbiente == "GDV1")
//                {
//                    GetTipoLinha = Funcoes.FGTipoLinha.CaminhaoDes;
//                }
//                else if (QualAmbiente == "GDV2")
//                {
//                    GetTipoLinha = Funcoes.FGTipoLinha.OnibusDes;

//                    // ******************************
//                    // *************JDV**************
//                    // ******************************
//                }
//                else if (QualAmbiente == "JPV1")
//                {
//                    GetTipoLinha = Funcoes.FGTipoLinha.Caminhao;
//                }
//                else if (QualAmbiente == "JPV2")
//                {
//                    GetTipoLinha = Funcoes.FGTipoLinha.Cabina;

//                    // Homologacao
//                }
//                else if (QualAmbiente == "JHV1")
//                {
//                    GetTipoLinha = Funcoes.FGTipoLinha.Caminhao;
//                }
//                else if (QualAmbiente == "JHV2")
//                {
//                    GetTipoLinha = Funcoes.FGTipoLinha.Cabina;

//                    // Desenvolvimento
//                }
//                else if (QualAmbiente == "JDV1")
//                {
//                    GetTipoLinha = Funcoes.FGTipoLinha.CaminhaoDes;
//                }
//                else if (QualAmbiente == "JDV2")
//                {
//                    GetTipoLinha = Funcoes.FGTipoLinha.CabinaDes;
//                }
//            }

//            return GetTipoLinha;
//        }

//        public static Funcoes.Fabrica ObterFabrica(string QualAmbiente)
//        {


//            Funcoes.Fabrica vObterFabrica = new Funcoes.Fabrica();

//            // Desenvolvimento Juiz de Fora
//            if (Funcoes.Left(QualAmbiente, 2) == "JD")
//            {
//                vObterFabrica = Funcoes.Fabrica.JDF;
//                // Producao Juiz de Fora
//            }
//            else if (Funcoes.Left(QualAmbiente, 2) == "JP")
//            {
//                vObterFabrica = Funcoes.Fabrica.JDF;
//                // Homologacao Juiz de Fora
//            }
//            else if (Funcoes.Left(QualAmbiente, 2) == "JH")
//            {
//                vObterFabrica = Funcoes.Fabrica.JDF;
//                // Desenvolvimento SBC
//            }
//            else if (Funcoes.Left(QualAmbiente, 2) == "GD")
//            {
//                vObterFabrica = Funcoes.Fabrica.SBC;
//                // Homologacao SBC
//            }
//            else if (Funcoes.Left(QualAmbiente, 2) == "GH")
//            {
//                vObterFabrica = Funcoes.Fabrica.SBC;
//                // Producao SBC
//            }
//            else if (Funcoes.Left(QualAmbiente, 2) == "GP")
//            {
//                vObterFabrica = Funcoes.Fabrica.SBC;
//            }

//            return vObterFabrica;
//        }

//        public static ToolStripLabel TituloMenu(string Titulo)
//        {
//            ToolStripLabel tslCaption = new ToolStripLabel(Titulo);
//            tslCaption.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
//            return tslCaption;
//        }

//        public static ToolStripLabel TituloMenu(string Titulo, Image Imagem)
//        {
//            ToolStripLabel tslCaption = new ToolStripLabel(Titulo);
//            tslCaption.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
//            tslCaption.Image = Imagem;
//            return tslCaption;
//        }

//        public static PictureBox CriarPictureBox(ref PictureBox Parent, Image Imagem, string Rotulo, int posicao_X, int posicao_Y)
//        {
//            return CriarPictureBox(ref Parent, Imagem, Rotulo, posicao_X, posicao_Y, 5);
//        }

//        public static PictureBox CriarPictureBox(ref PictureBox Parent, Image Imagem, string Rotulo, int posicao_X, int posicao_Y, int Percentual)
//        {
//            try
//            {
//                PictureBox pic = new PictureBox();
//                pic.BackColor = Color.Transparent;
//                pic.Parent = Parent;

//                int iW = ((Parent.Size.Height * Percentual) / 100);
//                int iH = iW;

//                pic.Size = new Size(iW, iH);

//                var bmp = Imagem;

//                using (Graphics g = Graphics.FromImage(bmp))
//                {
//                    StringFormat strFormat = new StringFormat();
//                    strFormat.Alignment = StringAlignment.Center;
//                    strFormat.LineAlignment = StringAlignment.Center;
//                    g.DrawString(Rotulo, new Font("Tahoma", 55), Brushes.Green, new RectangleF(0, 0, 120, 120), strFormat);
//                }

//                pic.Image = bmp;
//                pic.SizeMode = PictureBoxSizeMode.StretchImage;
//                pic.Anchor = AnchorStyles.Left;
//                pic.Name = "pic" + Rotulo;
//                pic.Visible = true;

//                switch (((eDirecaoPonteiro)Imagem.Tag))
//                {
//                    case eDirecaoPonteiro.Direita:
//                        pic.Location = new Point(posicao_X - iW, posicao_Y - (iH / 2));
//                        break;
//                    case eDirecaoPonteiro.Esquerda:
//                        pic.Location = new Point(posicao_X, posicao_Y - (iH / 2));
//                        break;
//                    case eDirecaoPonteiro.Cima:
//                        pic.Location = new Point(posicao_X - (iW / 2), posicao_Y);
//                        break;
//                    case eDirecaoPonteiro.Baixo:
//                        pic.Location = new Point(posicao_X - (iW / 2), posicao_Y - iH);
//                        break;
//                    case eDirecaoPonteiro.Circulo:
//                        pic.Location = new Point(posicao_X - (iW / 2), posicao_Y - (iH / 2));
//                        break;
//                }

//                pic.Anchor = ((AnchorStyles)(AnchorStyles.Left));

//                return pic;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }

//        public static void ExibirMensagem(MessageBoxIcon Icone, string Titulo, string Mensagem)
//        {
//            ExibirMensagem(Icone, Titulo, Mensagem, 180);
//        }

//        public static void ExibirMensagem(MessageBoxIcon Icone, string Titulo, string Mensagem, int Intervalo)
//        {
//            frmMensagem _frmMensagem = new frmMensagem(Icone, Titulo, Mensagem);

//            _frmMensagem.Intervalo = Intervalo;

//            _frmMensagem.StartPosition = FormStartPosition.CenterScreen;
//            _frmMensagem.Show();

//            Application.DoEvents();

//            _frmMensagem = null;
//        }

//        public static void HabilitarMoveUpDown(ListView LSV, Button Up, Button Down)
//        {
//            if (LSV.SelectedItems.Count == 0)
//            {
//                Up.Enabled = false;
//                Down.Enabled = false;
//                return;
//            }

//            if (LSV.Items.Count == 1)
//            {
//                Up.Enabled = false;
//                Down.Enabled = false;
//                return;
//            }

//            if (LSV.SelectedItems[0].Index == 0)
//            {
//                Up.Enabled = false;
//                Down.Enabled = true;
//                return;
//            }

//            if (LSV.SelectedItems[0].Index != LSV.Items.Count - 1)
//            {
//                Up.Enabled = true;
//                Down.Enabled = true;
//                return;
//            }

//            if (LSV.SelectedItems[0].Index == LSV.Items.Count - 1)
//            {
//                Up.Enabled = true;
//                Down.Enabled = false;
//                return;
//            }
//        }

//        public static void MoveUpDown(ref DataGridView Grid, eUpDown Sentido)
//        {
//            if (Grid.SelectedCells[0].RowIndex == -1) return;

//            DataGridViewRow LinhaAtual;
//            DataGridViewRow LinhaNova;
//            int atualIndex = Grid.SelectedCells[0].RowIndex;
//            int novaIndex = 0;

//            if (Sentido == eUpDown.Up)
//            {
//                novaIndex = atualIndex - 1;
//            }
//            else
//            {
//                novaIndex = atualIndex + 1;
//            }

//            try
//            {
//                // Salva a linha Atual
//                LinhaAtual = (DataGridViewRow)Grid.Rows[atualIndex].Clone();
//                int iCel = 0;
//                foreach (DataGridViewCell Celula in Grid.Rows[atualIndex].Cells)
//                {
//                    LinhaAtual.Cells[iCel].Value = Celula.Value;
//                    iCel++;
//                }
//                // Salva a linha nova
//                LinhaNova = (DataGridViewRow)Grid.Rows[novaIndex].Clone();
//                iCel = 0;
//                foreach (DataGridViewCell Celula in Grid.Rows[novaIndex].Cells)
//                {
//                    LinhaNova.Cells[iCel].Value = Celula.Value;
//                    iCel++;
//                }
//                // Move a linha atual para a nova
//                iCel = 0;
//                foreach (DataGridViewCell Celula in LinhaAtual.Cells)
//                {
//                    Grid.Rows[novaIndex].Cells[iCel].Value = Celula.Value;
//                    iCel++;
//                }
//                // Move a linha nova para a atual 
//                iCel = 0;
//                foreach (DataGridViewCell Celula in LinhaNova.Cells)
//                {
//                    Grid.Rows[atualIndex].Cells[iCel].Value = Celula.Value;
//                    iCel++;
//                }
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//            finally
//            {
//                LinhaNova = null;
//                LinhaAtual = null;
//            }

//            Grid.CurrentCell = Grid.Rows[novaIndex].Cells[(Grid.SelectedCells[0].ColumnIndex == -1) ? 0 : Grid.SelectedCells[0].ColumnIndex];
//        }

//        public static void MoveUpDown(ref ListView LSV, eUpDown Sentido)
//        {
//            if (LSV.SelectedItems.Count == 0) return;

//            ListViewItem ItemAtual;
//            ListViewItem ItemNovo;
//            int atualIndex = LSV.SelectedItems[0].Index;

//            int novaIndex = 0;

//            if (Sentido == eUpDown.Up)
//            {
//                novaIndex = atualIndex - 1;
//            }
//            else
//            {
//                novaIndex = atualIndex + 1;
//            }

//            try
//            {
//                // Salva a linha Atual
//                ItemAtual = (ListViewItem)LSV.Items[atualIndex].Clone();
//                // Salva a linha nova
//                ItemNovo = (ListViewItem)LSV.Items[novaIndex].Clone();
//                // Move a linha atual para a nova
//                if (Sentido == eUpDown.Down)
//                {
//                    LSV.Items[atualIndex].Remove();
//                    LSV.Items.Insert(novaIndex, ItemAtual);
//                    LSV.Items[novaIndex].Selected = true;
//                }
//                else
//                {
//                    LSV.Items[novaIndex].Remove();
//                    LSV.Items.Insert(atualIndex, ItemNovo);
//                    LSV.Items[novaIndex].Selected = true;
//                }
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//            finally
//            {
//                ItemNovo = null;
//                ItemAtual = null;
//            }
//        }

//        public static Point PosicaoMouse(object sender, MouseEventArgs Mouse)
//        {
//            try
//            {
//                return ((Control)sender).PointToScreen(Mouse.Location);
//            }
//            catch
//            {
//                return new Point(0, 0);
//            }
//        }

//        public static void ComboTipoAtividade(ref ComboBox Combo)
//        {
//            Combo.Items.Clear();
//            Combo.Items.Add(new ComboInfo("01", "Manual"));
//            Combo.Items.Add(new ComboInfo("02", "Por Tempo"));
//            Combo.Items.Add(new ComboInfo("03", "Apertadeira"));
//            Combo.Items.Add(new ComboInfo("04", "Consistir Vinculação NPFI"));
//            Combo.Items.Add(new ComboInfo("05", "Vinculação NPFI"));
//            Combo.Items.Add(new ComboInfo("06", "Iniciar Backflush"));
//            Combo.Items.Add(new ComboInfo("07", "Rota AGV"));
//            Combo.Items.Add(new ComboInfo("08", "Outros Equipamentos"));
//            Combo.Items.Add(new ComboInfo("09", "Aguardar Finalizar Backflush"));
//            Combo.Items.Add(new ComboInfo("10", "Validação NP"));
//            Combo.Items.Add(new ComboInfo("11", "Controle Sistema"));
//            Combo.Items.Add(new ComboInfo("12", "Marcar Atendido Posto"));
//            Combo.Items.Add(new ComboInfo("13", "Captar Componente - Posto"));
//            Combo.Items.Add(new ComboInfo("14", "Captar Componente - Produto"));
//        }

//        public static void ComboTipoAtividade(ref DataGridViewComboBoxColumn Combo)
//        {
//            Combo.Items.Clear();
//            Combo.Items.Add("01 - Manual");
//            Combo.Items.Add("02 - Por Tempo");
//            Combo.Items.Add("03 - Apertadeira");
//            Combo.Items.Add("04 - Consistir Vinculação NPFI");
//            Combo.Items.Add("05 - Vinculação NPFI");
//            Combo.Items.Add("06 - Iniciar Backflush");
//            Combo.Items.Add("07 - Rota AGV");
//            Combo.Items.Add("08 - Outros Equipamentos");
//            Combo.Items.Add("09 - Aguardar Finalizar Backflush");
//            Combo.Items.Add("10 - Validação NP");
//            Combo.Items.Add("11 - Controle Sistema");
//            Combo.Items.Add("12 - Marcar Atendido Posto");
//            Combo.Items.Add("13 - Captar Componente - Posto");
//            Combo.Items.Add("14 - Captar Componente - Produto");
//        }

//        public static List<ListaTipoAtividadeModel> ComboTipoAtividade()
//        {
//            List<ListaTipoAtividadeModel> tipoAtividade = new List<ListaTipoAtividadeModel>
//            {
//                new ListaTipoAtividadeModel() { CodigoTipoAtividade = "01", DescricaoTipoAtividade = "Manual" },
//                new ListaTipoAtividadeModel() { CodigoTipoAtividade = "02", DescricaoTipoAtividade = "Por Tempo" },
//                new ListaTipoAtividadeModel() { CodigoTipoAtividade = "03", DescricaoTipoAtividade = "Apertadeira" },
//                new ListaTipoAtividadeModel() { CodigoTipoAtividade = "04", DescricaoTipoAtividade = "Consistir Vinculação NPFI" },
//                new ListaTipoAtividadeModel() { CodigoTipoAtividade = "05", DescricaoTipoAtividade = "Vinculação NPFI" },
//                new ListaTipoAtividadeModel() { CodigoTipoAtividade = "06", DescricaoTipoAtividade = "Iniciar Backflush" },
//                new ListaTipoAtividadeModel() { CodigoTipoAtividade = "07", DescricaoTipoAtividade = "Rota AGV" },
//                new ListaTipoAtividadeModel() { CodigoTipoAtividade = "08", DescricaoTipoAtividade = "Outros Equipamentos" },
//                new ListaTipoAtividadeModel() { CodigoTipoAtividade = "09", DescricaoTipoAtividade = "Aguardar Finalizar Backflush" },
//                new ListaTipoAtividadeModel() { CodigoTipoAtividade = "10", DescricaoTipoAtividade = "Validação NP" },
//                new ListaTipoAtividadeModel() { CodigoTipoAtividade = "11", DescricaoTipoAtividade = "Controle Sistema" },
//                new ListaTipoAtividadeModel() { CodigoTipoAtividade = "12", DescricaoTipoAtividade = "Marcar Atendido Posto" },
//                new ListaTipoAtividadeModel() { CodigoTipoAtividade = "13", DescricaoTipoAtividade = "Captar Componente - Posto" },
//                new ListaTipoAtividadeModel() { CodigoTipoAtividade = "14", DescricaoTipoAtividade = "Captar Componente - Produto" }
//        };

//            return tipoAtividade;
//        }

//        public static void ComboLocalExecucao(ref ComboBox Combo)
//        {
//            Combo.Items.Clear();
//            Combo.Items.Add(new ComboInfo("UMI", "UMI"));
//            Combo.Items.Add(new ComboInfo("SGP", "SGP"));
//            Combo.Items.Add(new ComboInfo("GPX", "GPX"));
//        }

//        public static void ComboLocalExecucao(ref DataGridViewComboBoxColumn Combo)
//        {
//            Combo.Items.Clear();
//            Combo.Items.Add(@"UMI");
//            Combo.Items.Add(@"SGP");
//            Combo.Items.Add(@"GPX");
//        }

//        public static string DescricaoCodigoPlanoPLF(string COPLFA)
//        {
//            // Tabela SQM - não tem replica
//            //COPLFA - DECOPL
//            //ES     - Estudos
//            //ES1    - Estudos Linha 1
//            //ES2    - Estudos linha 2
//            //ES3    - Estudos linha 3
//            //INF    - Informativo
//            //MB     - Montagem Bruta
//            //OPC    - OPCIONAL
//            //PRI    - PRINCIPAL
//            //PRO    - Provisorio
//            //PR1    - Principal Processo 1
//            //PR2    - Principal Processo 2
//            //REF    - Reforma
//            //TER    - Terceiros

//            COPLFA = UFLimitarString(COPLFA, 3);

//            if (COPLFA == "ES ") return "Estudos             ";
//            if (COPLFA == "ES1") return "Estudos Linha 1     ";
//            if (COPLFA == "ES2") return "Estudos linha 2     ";
//            if (COPLFA == "ES3") return "Estudos linha 3     ";
//            if (COPLFA == "INF") return "Informativo         ";
//            if (COPLFA == "MB ") return "Montagem Bruta      ";
//            if (COPLFA == "OPC") return "OPCIONAL            ";
//            if (COPLFA == "PRI") return "PRINCIPAL           ";
//            if (COPLFA == "PRO") return "Provisorio          ";
//            if (COPLFA == "PR1") return "Principal Processo 1";
//            if (COPLFA == "PR2") return "Principal Processo 2";
//            if (COPLFA == "REF") return "Reforma             ";
//            if (COPLFA == "TER") return "Terceiros           ";

//            return COPLFA;
//        }

//        public static string DescricaoSituacaoPlano(string STPLFA)
//        {
//            STPLFA = Funcoes.UFLimitarString(STPLFA, 2);

//            if (STPLFA == "O ") return "Oficial";
//            if (STPLFA == "D ") return "Desenvolvimento";

//            return STPLFA;
//        }

//        public static string ObterStringSemAcentosECaracteresEspeciais(string str)
//        {
//            /** Troca os caracteres acentuados por não acentuados **/
//            string[] acentos = new string[] { "ç", "Ç", "á", "é", "í", "ó", "ú", "ý", "Á", "É", "Í", "Ó", "Ú", "Ý", "à", "è", "ì", "ò", "ù", "À", "È", "Ì", "Ò", "Ù", "ã", "õ", "ñ", "ä", "ë", "ï", "ö", "ü", "ÿ", "Ä", "Ë", "Ï", "Ö", "Ü", "Ã", "Õ", "Ñ", "â", "ê", "î", "ô", "û", "Â", "Ê", "Î", "Ô", "Û" };
//            string[] semAcento = new string[] { "c", "C", "a", "e", "i", "o", "u", "y", "A", "E", "I", "O", "U", "Y", "a", "e", "i", "o", "u", "A", "E", "I", "O", "U", "a", "o", "n", "a", "e", "i", "o", "u", "y", "A", "E", "I", "O", "U", "A", "O", "N", "a", "e", "i", "o", "u", "A", "E", "I", "O", "U" };

//            for (int i = 0; i < acentos.Length; i++)
//            {
//                str = str.Replace(acentos[i], semAcento[i]);
//            }
//            /** Troca os caracteres especiais da string por "" **/
//            string[] caracteresEspeciais = { "\\.", ",", "-", ":", "\\(", "\\)", "ª", "\\|", "\\\\", "°" };

//            for (int i = 0; i < caracteresEspeciais.Length; i++)
//            {
//                str = str.Replace(caracteresEspeciais[i], "");
//            }

//            /** Troca os espaços no início por "" **/
//            str = str.Replace("^\\s+", "");
//            /** Troca os espaços no início por "" **/
//            str = str.Replace("\\s+$", "");
//            /** Troca os espaços duplicados, tabulações e etc por " " **/
//            str = str.Replace("\\s+", " ");
//            return str;
//        }


//        public static bool LinhasDestinoLote(ref ComboBox Combo, string Ambiente)
//        {
//            string strSql = "";
//            DataSet ds = new DataSet();

//            strSql += " SELECT * FROM THK ";
//            strSql += " WHERE TIUTIL = " + PRMLibrary.Web.Comum.Apoio.FormatarVariaveisHost(0);
//            strSql += "   AND NOVIEW = " + PRMLibrary.Web.Comum.Apoio.FormatarVariaveisHost(1);

//            try
//            {
//                ds = PRMLibrary.Web.Comum.Apoio.ExecutarComParametros(strSql, Ambiente, Funcoes.UFLimitarString(AmbienteInfo._TiuTil, 3),
//                                                                                        Funcoes.UFLimitarString("PRMSEQLOTE_LINHA_DESTINO", 30));
//                Combo.Items.Clear();

//                foreach (DataRow dr in ds.Tables[0].Rows)
//                {
//                    Combo.Items.Add(new ComboInfo(dr["NUPOGR"].ToString(), dr["NOCOLU"].ToString()));
//                }

//                return Combo.Items.Count > 0;

//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }

//        #endregion

//        #region ... Numerics ...

//        public static string UFTrataRetornoPGM(string MensagemPGM)
//        {
//            UFTrataRetornoPGM(ref MensagemPGM, false);
//            return MensagemPGM;
//        }

//        public static int UFTrataRetornoPGM(ref string MensagemPGM)
//        {
//            return UFTrataRetornoPGM(ref MensagemPGM, false);
//        }

//        public static int UFTrataRetornoPGM(ref string MensagemPGM, bool ForcaErro)
//        {
//            string OldMensagem = MensagemPGM.Trim();

//            string strDescription = "";
//            //string strSource = "";
//            string strNumber = "";
//            int intPosDoisPontos;
//            int intPosGrade;
//            //int intTamMensagem;
//            int CodigoErro = 0;

//            try
//            {

//                if (MensagemPGM.Trim().Equals(string.Empty))
//                    return 0;

//                intPosDoisPontos = MensagemPGM.IndexOf(":", 1);

//                if (MensagemPGM.Trim().Substring(0, 4) == "ERRO")
//                {
//                    MensagemPGM = MensagemPGM.Substring(intPosDoisPontos + 1).Trim();
//                    intPosDoisPontos = MensagemPGM.IndexOf(":", 1);
//                }

//                intPosGrade = MensagemPGM.IndexOf("#");

//                //if (intPosGrade < 0)
//                //    if (intPosDoisPontos < 0)
//                //        return 0;
//                //    else
//                //        intPosGrade = intPosDoisPontos;


//                if (intPosDoisPontos > 0)
//                {
//                    //MensagemPGM = MensagemPGM.Replace("ERRO", "");
//                    strNumber = MensagemPGM.Substring(intPosDoisPontos - 5, 5);

//                    if (strNumber.Trim().Equals("ERRO"))
//                        strNumber = MensagemPGM.Substring(0, 5);
//                    else
//                        if (strNumber.Trim().Equals(string.Empty))
//                        strNumber = MensagemPGM.Substring(0, 5);


//                    if (intPosGrade > 0)
//                    {
//                        strDescription = Funcoes.Mid(MensagemPGM, intPosDoisPontos + 2);
//                    }
//                    else
//                    {
//                        strDescription = Funcoes.Mid(MensagemPGM, intPosDoisPontos + 2);
//                    }
//                }
//                else
//                {
//                    strNumber = Funcoes.Mid(MensagemPGM, 1, 5);
//                    if (intPosGrade > 0 & MensagemPGM.Length > 10)
//                    {
//                        if (MensagemPGM.Substring(6, 4) == "ORA-")
//                        {
//                            strDescription = MensagemPGM.Substring(6, intPosGrade - 8);
//                        }
//                        else
//                        {
//                            strDescription = MensagemPGM.Substring(6, intPosGrade - 6).Trim();
//                        }
//                    }
//                    else
//                    {
//                        strDescription = MensagemPGM.Trim();
//                    }
//                }

//                //strSource = ".UFLinkMQSeries";

//                int num = 0;

//                if (int.TryParse(strNumber, out num))
//                {
//                    CodigoErro = int.Parse(strNumber);

//                    if (CodigoErro > 0)
//                    {
//                        if (Math.Truncate(Convert.ToDouble(strNumber)) == 0)
//                        {
//                            strDescription = "(" + strNumber.Trim() + ") " + strDescription;
//                        }

//                        MensagemPGM = strDescription;

//                        CodigoErroTrataRetorno = CodigoErro.ToString();

//                        throw new Exception(strDescription.Replace(strNumber, "").Trim());

//                        //MensagemPGM = OldMensagem.Trim();
//                        //throw new Exception(OldMensagem);
//                    }
//                }


//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }

//            return CodigoErro;
//        }

//        public static int UFPesquisaTxtCombo(ComboBox Combo, string Texto)
//        {
//            return UFPesquisaTxtCombo(Combo, Texto, 0);
//        }

//        public static int UFPesquisaTxtCombo(ComboBox Combo, string Texto, int Length)
//        {
//            int UFPesquisaTxtCombo = 0;
//            string TextoAux = "";
//            int nI;

//            UFPesquisaTxtCombo = -1;

//            TextoAux = Texto;
//            if (TextoAux == "") return UFPesquisaTxtCombo;
//            for (nI = 0; nI <= Combo.Items.Count - 1; nI++)
//            {
//                if (Length == 0)
//                {
//                    if (Combo.Items[nI].ToString().Contains(TextoAux))
//                    {
//                        UFPesquisaTxtCombo = Convert.ToInt16(nI);
//                        break;
//                    }
//                }
//                else if (Combo.Items[nI].ToString().Trim().Length >= Length)
//                {
//                    if (Combo.Items[nI].ToString().Substring(0, Length) == TextoAux.Trim())
//                    {
//                        UFPesquisaTxtCombo = Convert.ToInt16(nI);
//                        break;
//                    }
//                }
//            } // nI

//            return UFPesquisaTxtCombo;
//        }

//        public static int TamanhoRegistro(object Origem)
//        {
//            int TamRegistro = 0;
//            PropertyInfo[] ListaPropriedades = Origem.GetType().GetProperties();

//            for (int i = 0; i < ListaPropriedades.Length; i++)
//            {
//                ListaPropriedades[i].SetValue(Origem, string.Empty, null);
//                TamRegistro += ListaPropriedades[i].GetValue(Origem, null).ToString().Length;
//            }

//            return TamRegistro;
//        }

//        public static int InStr(int Start, string Text, string Caracter)
//        {
//            if (Text.Trim().Equals(string.Empty)) return 0;

//            int Posicao = Text.IndexOf(Caracter, Start);

//            if (Posicao >= 0)
//            {
//                return Posicao;
//            }
//            else
//            {
//                return 0;
//            }
//        }

//        public static int LastDayMonth(int iMonth)
//        {
//            return LastDayMonth(iMonth, 0);
//        }

//        public static int LastDayMonth(int iMonth, int iYear)
//        {
//            int LastDayMonth = 0;
//            // VBto upgrade warning: Data As DateTime	OnWrite(string, int)
//            DateTime Data;

//            switch (iMonth)
//            {
//                case 4:
//                case 6:
//                case 9:
//                case 11:
//                    {

//                        LastDayMonth = 30;
//                        break;
//                    }
//                case 1:
//                case 3:
//                case 5:
//                case 7:
//                case 8:
//                case 10:
//                case 12:
//                    {
//                        LastDayMonth = 31;
//                        break;
//                    }
//                default:
//                    {
//                        Data = DateTime.Parse("01/" + iMonth + "/" + (Convert.ToBoolean(iYear == 0) ? DateTime.Today.Year : iYear));
//                        while (Data.Month == iMonth)
//                        {
//                            Data = Data.AddDays(1);
//                        }
//                        LastDayMonth = Convert.ToInt16(DateTime.FromOADate(Data.ToOADate() - 1).Day);
//                        break;
//                    }
//            } //end switch
//            return LastDayMonth;
//        }

//        #endregion

//        #region ... Proposta Montagem ...
//        public static string DataToJuliana(DateTime Data)
//        {
//            string DataToJuliana = "";
//            DataToJuliana = Data.Year.ToString().Substring(2) + "/" + Data.DayOfYear.ToString("D3");
//            return DataToJuliana;
//        }

//        public static string DataJulianaToStringData(string DataJ)
//        {
//            try
//            {
//                if (string.IsNullOrEmpty(DataJ)) return "";

//                DataJ = String.Format("{0:D5}", Convert.ToInt32(DataJ));

//                string strAno;

//                if (DataJ == "00000")
//                {
//                    DataJ = "00001";
//                    strAno = "2000";
//                }
//                else
//                {
//                    strAno = DataJ.PadRight(5, '0').Substring(0, 2);

//                    if (Convert.ToInt16(strAno) > 55)
//                    {
//                        strAno = "19" + strAno;
//                    }
//                    else
//                    {
//                        strAno = "20" + strAno;
//                    }
//                }

//                DateTime data = new DateTime(Convert.ToInt16(strAno), 1, 1);
//                DateTime dataOut = data.AddDays(Convert.ToDouble(DataJ.PadRight(5, '0').Substring(2, 3)) - 1);

//                return dataOut.Day.ToString("00").PadRight(2, '0') + dataOut.Month.ToString("00").PadRight(2, '0') + dataOut.Year.ToString().PadRight(4, '0');

//            }
//            catch (Exception)
//            {
//                throw;
//            }
//        }

//        /// <summary>
//        /// Formata em data recebendo uma string com a data sem formatacao.
//        /// pe. 24122018 ou 241218. Colcando as barras e formatando em data
//        /// </summary>
//        /// <param name="Data"></param>
//        /// <param name="shortDate"></param>
//        /// <returns></returns>
//        public static DateTime FormataData(string Data, Boolean shortDate = false)
//        {
//            try
//            {
//                string strAux = "";

//                if (shortDate)
//                {
//                    strAux = Data.Substring(0, 2) + "/" + Data.Substring(2, 2) + "/" + Data.Substring(4, 2);
//                }
//                else
//                {
//                    strAux = Data.Substring(0, 2) + "/" + Data.Substring(2, 2) + "/" + Data.Substring(4, 4);
//                }

//                return Convert.ToDateTime(strAux);
//            }
//            catch (Exception)
//            {
//                return DateTime.MinValue;
//            }
//        }

//        public static string FormataDataHora(string Data, Boolean shortDate = false)
//        {
//            try
//            {
//                string strData = "";

//                if (Data.Trim() == string.Empty)
//                {
//                    return string.Empty;
//                }

//                if (shortDate)
//                {
//                    strData = Data.Substring(4, 4) + "/" +
//                              Data.PadRight(2, '0').Substring(2, 2) + "/" +
//                              Data.PadRight(2, '0').Substring(0, 2);
//                    return strData;
//                    //FormataDataDB2 = Format$(Data, "yyyy-mm-dd")
//                }
//                else
//                {
//                    strData = Data.Substring(4, 4) + "/" +
//                              Data.PadRight(2, '0').Substring(2, 2) + "/" +
//                              Data.PadRight(2, '0').Substring(0, 2) + " " +
//                              Data.PadRight(2, '0').Substring(8, 2) + " :" +
//                              Data.PadRight(2, '0').Substring(10, 2) + ":" +
//                              Data.PadRight(2, '0').Substring(12, 2);

//                    return strData;
//                    //FormataDataDB2 = Format$(Data, "yyyy-mm-dd-hh.mm.ss.000000")
//                }
//            }
//            catch (Exception)
//            {
//                throw;
//            }
//        }
//        #endregion


//        #region ... Strings ...

//        public static string ObterDescricaoCliente(string Ambiente, string Conta)
//        {
//            string Query = "SELECT DECOMLDA FROM FDA ";
//            Query += " WHERE TIUTILDA = 'DMC' ";
//            Query += "   AND COCONTDA = '" + Conta + "'";

//            DataSet ds = new Conexao(Ambiente).ExecutaSQL(Query);

//            if (ds.Tables[0].Rows.Count > 0)
//            {
//                return Conta + " - " + ds.Tables[0].Rows[0]["DECOMLDA"].ToString().Trim();
//            }
//            else
//            {
//                return Conta + " - Não identificado";
//            }
//        }

//        public static string GetHearder(string Field)
//        {
//            string Retorno = "";

//            switch (Field.ToUpper())
//            {
//                case "DESGLN":
//                    Retorno = "Denominação Segmento";
//                    break;
//                case "DEESSG":
//                    Retorno = "Denominação Estação";
//                    break;
//                case "COGRUP":
//                    Retorno = "Grupo";
//                    break;
//                case "COSGLN":
//                    Retorno = "Segmento";
//                    break;
//                case "COESSG":
//                    Retorno = "Estação";
//                    break;
//                case "NUMVER":
//                    Retorno = "Versão";
//                    break;
//                case "GRSUBC":
//                    Retorno = "Sub-Grupo";
//                    break;
//                case "NUPRVI":
//                    Retorno = "NP Vinculado";
//                    break;
//                case "NUPRBA":
//                    Retorno = "NP Base";
//                    break;
//                case "NUPRFI":
//                    Retorno = "NP Ficticio";
//                    break;

//                case "LOC1":
//                    Retorno = "Apelido Peça";
//                    break;

//                case "DESQPR":
//                    Retorno = "Denominação da Sequencia";
//                    break;

//                case "PLANEJADO":
//                    Retorno = "NP Planejado";
//                    break;
//                case "CAPTADO":
//                    Retorno = "NP Captado";
//                    break;
//                case "COPREM":
//                    Retorno = "Posto";
//                    break;
//                case "DAPLPR":
//                    Retorno = "Data Planejada";
//                    break;
//                case "COLOSQ":
//                    Retorno = "Localização";
//                    break;
//                case "QTDQUC":
//                    Retorno = "Quantidade";
//                    break;
//                case "TIBAUMA":
//                    Retorno = "Tipo Agregado";
//                    break;
//                case "TIBAUMB":
//                    Retorno = "Tipo Veículo";
//                    break;
//                case "NUSQVE":
//                    Retorno = "Kanban Veíc.";
//                    break;
//                case "NUVAVE":
//                    Retorno = "Variante Vei.";
//                    break;
//                case "NUSEPP":
//                    Retorno = "Sequência";
//                    break;
//                case "COLIMO_MOT":
//                    Retorno = "Linha Motor";
//                    break;
//                case "NUPROD_MOT":
//                    Retorno = "NP Motor";
//                    break;
//                case "COLIMO_CAM":
//                    Retorno = "Linha Câmbio";
//                    break;
//                case "NUPROD_CAM":
//                    Retorno = "NP Câmbio";
//                    break;
//                case "COLIMO_EIX":
//                    Retorno = "Linha Eixo";
//                    break;
//                case "NUPROD_EIX":
//                    Retorno = "NP Eixo";
//                    break;
//                case "COLIMO_CAB":
//                    Retorno = "Linha Cabina";
//                    break;
//                case "NUPROD_CAB":
//                    Retorno = "NP Cabina";
//                    break;
//                case "COLONP_MOT":
//                case "COLONP_EIX":
//                case "COLONP_CAM":
//                case "COLONP_CAB":
//                    Retorno = "Status";
//                    break;
//                case "RETRAB_MOT":
//                case "RETRAB_EIX":
//                case "RETRAB_CAM":
//                case "RETRAB_CAB":
//                    Retorno = "Retrabalho";
//                    break;

//                case "DEPREM":
//                    Retorno = "Descrição Posto";
//                    break;

//                case "IDATDN":
//                    Retorno = "Atendido";
//                    break;
//                case "DAATUA":
//                    Retorno = "Atualização";
//                    break;
//                case "COPTRA":
//                    Retorno = "Ponto";
//                    break;
//                case "COTEAT":
//                    Retorno = "Terminal";
//                    break;
//                case "COSQPR":
//                    Retorno = "Sequência";
//                    break;
//                case "STPLPR":
//                    Retorno = "Status Sequência";
//                    break;

//                case "NUVARI_MOT":
//                    Retorno = "Variante Motor";
//                    break;
//                case "NUVARI_CAM":
//                    Retorno = "Variante Câmbio";
//                    break;

//                case "NUVARI_CAB":
//                    Retorno = "Variante Cabina";
//                    break;


//                case "COLIMO_ED1":
//                    Retorno = "Linha Eixo ED1";
//                    break;
//                case "NUPROD_ED1":
//                    Retorno = "NP Eixo ED1";
//                    break;
//                case "COLONP_ED1":
//                    Retorno = "Localização Eixo ED1";
//                    break;
//                case "RETRAB_ED1":
//                    Retorno = "Retrabalho Eixo ED1";
//                    break;
//                case "NUVARI_ED1":
//                    Retorno = "Variante Eixo ED1";
//                    break;

//                case "COLIMO_ED2":
//                    Retorno = "Linha Eixo ED2";
//                    break;
//                case "NUPROD_ED2":
//                    Retorno = "NP Eixo ED2";
//                    break;
//                case "COLONP_ED2":
//                    Retorno = "Localização Eixo ED2";
//                    break;
//                case "RETRAB_ED2":
//                    Retorno = "Retrabalho Eixo ED2";
//                    break;
//                case "NUVARI_ED2":
//                    Retorno = "Variante Eixo ED2";
//                    break;

//                case "COLIMO_ET1":
//                    Retorno = "Linha Eixo ET1";
//                    break;
//                case "NUPROD_ET1":
//                    Retorno = "NP Eixo ET1";
//                    break;
//                case "COLONP_ET1":
//                    Retorno = "Localização Eixo ET1";
//                    break;
//                case "RETRAB_ET1":
//                    Retorno = "Retrabalho Eixo ET1";
//                    break;
//                case "NUVARI_ET1":
//                    Retorno = "Variante Eixo ET1";
//                    break;

//                case "COLIMO_ET2":
//                    Retorno = "Linha Eixo ET2";
//                    break;
//                case "NUPROD_ET2":
//                    Retorno = "NP Eixo ET2";
//                    break;
//                case "COLONP_ET2":
//                    Retorno = "Localização Eixo ET2";
//                    break;
//                case "RETRAB_ET2":
//                    Retorno = "Retrabalho Eixo ET2";
//                    break;
//                case "NUVARI_ET2":
//                    Retorno = "Variante Eixo ET2";
//                    break;

//                case "QTDRES":
//                    Retorno = "Qtd. Est.";
//                    break;
//                case "QTDDIS":
//                    Retorno = "Qtd. Disp.";
//                    break;

//                case "WFCOBA":
//                    Retorno = "Cabina";
//                    break;
//                case "DECOBA":
//                    Retorno = "Obs.Cabina";
//                    break;
//                case "VAR_A":
//                    Retorno = "Var.Cabina";
//                    break;

//                case "WFCOBM":
//                    Retorno = "Motor";
//                    break;
//                case "DECOBM":
//                    Retorno = "Obs.Motor";
//                    break;
//                case "VAR_M":
//                    Retorno = "Var.Motor";
//                    break;

//                case "WFCOBG":
//                    Retorno = "Câmbio";
//                    break;
//                case "DECOBG":
//                    Retorno = "Obs.Câmbio";
//                    break;
//                case "VAR_G":
//                    Retorno = "Var.Câmbio";
//                    break;

//                case "WFCOBV":
//                    Retorno = "Eixo ED1";
//                    break;
//                case "DECOBV":
//                    Retorno = "Obs.Eixo ED1";
//                    break;
//                case "VAR_V":
//                    Retorno = "Var.Eixo ED1";
//                    break;

//                case "WFCOBW":
//                    Retorno = "Eixo ED2";
//                    break;
//                case "DECOBW":
//                    Retorno = "Obs.Eixo ED2";
//                    break;
//                case "VAR_W":
//                    Retorno = "Var.Eixo ED2";
//                    break;


//                case "WFCOBH":
//                    Retorno = "Eixo ET1";
//                    break;
//                case "DECOBH":
//                    Retorno = "Obs.Eixo ET1";
//                    break;
//                case "VAR_H":
//                    Retorno = "Var.Eixo ET1";
//                    break;

//                case "WFCOBJ":
//                    Retorno = "Eixo ET2";
//                    break;
//                case "DECOBJ":
//                    Retorno = "Obs.Eixo ET2";
//                    break;
//                case "VAR_J":
//                    Retorno = "Var.Eixo ET2";
//                    break;

//                case "DAFABR":
//                    Retorno = "Data.Conf.";
//                    break;

//                //case "":
//                //    Retorno = "";
//                //    break;
//                //case "":
//                //    Retorno = "";
//                //    break;
//                //case "":
//                //    Retorno = "";
//                //    break;
//                //case "":
//                //    Retorno = "";
//                //    break;
//                //case "":
//                //    Retorno = "";
//                //    break;
//                //case "":
//                //    Retorno = "";
//                //    break;
//                //case "":
//                //    Retorno = "";
//                //    break;
//                //case "":
//                //    Retorno = "";
//                //    break;
//                //case "":
//                //    Retorno = "";
//                //    break;
//                //case "":
//                //    Retorno = "";
//                //    break;
//                //case "":
//                //    Retorno = "";
//                //    break;
//                //case "":
//                //    Retorno = "";
//                //    break;
//                //case "":
//                //    Retorno = "";
//                //    break;
//                //case "":
//                //    Retorno = "";
//                //    break;
//                //case "":
//                //    Retorno = "";
//                //    break;
//                //case "":
//                //    Retorno = "";
//                //    break;
//                //case "":
//                //    Retorno = "";
//                //    break;
//                //case "":
//                //    Retorno = "";
//                //    break;
//                //case "":
//                //    Retorno = "";
//                //    break;
//                //case "":
//                //    Retorno = "";
//                //    break;
//                //case "":
//                //    Retorno = "";
//                //    break;
//                //case "":
//                //    Retorno = "";
//                //    break;
//                //case "":
//                //    Retorno = "";
//                //    break;
//                //case "":
//                //    Retorno = "";
//                //    break;
//                //case "":
//                //    Retorno = "";
//                //    break;
//                //case "":
//                //    Retorno = "";
//                //    break;



//                case "NUPROD":
//                    Retorno = "Nº Produção";
//                    break;


//                case "NUSEMF":
//                    Retorno = "Seq. M. Final";
//                    break;
//                case "COCORD":
//                    Retorno = "Cor";
//                    break;
//                case "COLONP":
//                    Retorno = "Localização";
//                    break;
//                case "COLIMO":
//                    Retorno = "Linha";
//                    break;
//                case "STSITU":
//                    Retorno = "Status";
//                    break;
//                case "NUPRGP":
//                    Retorno = "Ain/Fz";
//                    break;
//                case "NUVARI":
//                    Retorno = "Variante";
//                    break;
//                case "NUVARP":
//                    Retorno = "Var. Veículo";
//                    break;
//                case "COPAIS":
//                    Retorno = "País";
//                    break;
//                case "COBAUM":
//                    Retorno = "Baumuster";
//                    break;
//                case "DAELIN":
//                    Retorno = "Data Entrada Linha";
//                    break;
//                case "DAFLIN":
//                    Retorno = "Data Final Linha";
//                    break;
//                case "DALICO":
//                    Retorno = "Data Liberação";
//                    break;
//                case "CODTAG":
//                    Retorno = "Destino";
//                    break;
//                case "NUPRGE":
//                case "NUPRVE":
//                    Retorno = "Veículo";
//                    break;
//                case "COLIVE":
//                    Retorno = "Linha";
//                    break;
//                case "STSIGE":
//                    Retorno = "Status Gegador";
//                    break;
//                case "NUSEQM":
//                    Retorno = "Seq. Planej.";
//                    break;
//                case "DENR":
//                    Retorno = "Denominação";
//                    break;
//                case "DACPTO":
//                    Retorno = "Data Captação";
//                    break;
//                case "NUVARR":
//                    Retorno = "Variante Rohbau";
//                    break;
//                case "DASLIN":
//                    Retorno = "Data Saída Linha";
//                    break;
//                case "CODVIN":
//                    Retorno = "Cód. VIN";
//                    break;



//                case "NUMMDS":
//                    Retorno = "Número do MDS";
//                    break;

//                case "DAINCL":
//                    Retorno = "Data/Hora da Inclusão";
//                    break;

//                case "CODOCU":
//                    Retorno = "Código";
//                    break;
//                case "":
//                    Retorno = "Descrição";
//                    break;
//                case "DAINCL1":
//                    Retorno = "Emissão";
//                    break;
//                case "DAINCL2":
//                    Retorno = "Alteração";
//                    break;
//                case "COUSIN":
//                    Retorno = "Usuário Inclusão";
//                    break;
//                case "DEMOTV":
//                    Retorno = "Motivo";
//                    break;

//                case "COUSSO":
//                    Retorno = "Usuário Solic.";
//                    break;

//                case "SIARSO":
//                    Retorno = "Área Solic.";
//                    break;

//                case "COCESO":
//                    Retorno = "Centro de Custo";
//                    break;

//                case "COUSAT":
//                    Retorno = "Usuário Alteração";
//                    break;

//                case "SIAREA":
//                    Retorno = "Área";
//                    break;

//                case "DAHIST":
//                    Retorno = "Data/Hora";
//                    break;

//                case "COPTCO":
//                    Retorno = "Cód. Ponto Contagem";
//                    break;

//                case "TIPHIS":
//                    Retorno = "Tipo Captação";
//                    break;

//                case "COTEIN":
//                    Retorno = "Terminal";
//                    break;

//                case "DEPTRA":
//                    Retorno = "Descrição";
//                    break;

//                case "TICPTE":
//                    Retorno = "Tipo Componente";
//                    break;

//                case "NUMCOM":
//                    Retorno = "Variante/Comp.";
//                    break;

//                case "NUVARI_NUMCOM":
//                    Retorno = "Variante/Peça";
//                    break;

//                case "COORIG":
//                    Retorno = "Origem";
//                    break;

//                case "COFABR":
//                    Retorno = "Cod. Fabr.";
//                    break;

//                case "COLISB":
//                    Retorno = "Linha";
//                    break;

//                case "WFFATE":
//                    Retorno = "Terceiro";
//                    break;

//                case "NUSQAG":
//                    Retorno = "Seq. Veb.";
//                    break;

//                case "NUMCOD":
//                    Retorno = "Item";
//                    break;

//                case "DEDOCU":
//                case "DECPCP":
//                    Retorno = "Denominação";
//                    break;
//                case "DENO":
//                    Retorno = "Descrição";
//                    break;

//                case "COFORN":
//                    Retorno = "Fornecedor";
//                    break;

//                case "NUSEPR":
//                    Retorno = "Série";
//                    break;

//                case "NULTCP":
//                    Retorno = "Lote";
//                    break;

//                case "COBACO":
//                    Retorno = "Cod Barra Componente";
//                    break;

//                case "NUVOEX":
//                    Retorno = "Lote";
//                    break;

//                case "NUVOLU":
//                    Retorno = "Volume";
//                    break;

//                case "NUINTE":
//                    Retorno = "Número Interno";
//                    break;

//                case "NUPOSI":
//                    Retorno = "Posição";
//                    break;

//                case "CRPOSI":
//                    Retorno = "Controle Posição";
//                    break;

//                case "NUITEM":
//                    Retorno = "Peça";
//                    break;

//                default:
//                    Retorno = Field;
//                    break;
//            }

//            return Retorno.Replace("_", " ");
//        }

//        public static string ObterVersaoPRMSeqOutros(string AplicativoEscolhido)
//        {
//            Funcoes.PRMSeq _AplicativoEscolhido = new PRMSeq();

//            switch (AplicativoEscolhido)
//            {
//                case "PRMSEQACOPLA":
//                case "PRMSEQ1044":
//                    _AplicativoEscolhido = PRMSeq.PRMSeqAcopla;
//                    break;
//                case "PRMSEQKIT":
//                    _AplicativoEscolhido = PRMSeq.PRMSeqKit;
//                    break;

//                case "PRMSEQKITEIXO":
//                    _AplicativoEscolhido = PRMSeq.PRMSeqKitEixo;
//                    break;

//                case "PRMSEQPINTURA":
//                    _AplicativoEscolhido = PRMSeq.PRMSeqPintura;
//                    break;

//                case "PRMSEQGALPAO":
//                    _AplicativoEscolhido = PRMSeq.PRMSeqGalpao;
//                    break;

//                case "PRMSEQCKD":
//                    _AplicativoEscolhido = PRMSeq.PRMSeqKitCkd;
//                    break;

//                case "PRMSEQLOTE":
//                    _AplicativoEscolhido = PRMSeq.PRMSeqLote;
//                    break;

//                case "PRMSEQLOG":
//                    _AplicativoEscolhido = PRMSeq.PRMSeqLogistica;
//                    break;

//                case "PRMSEQPUFFCKD":
//                    _AplicativoEscolhido = PRMSeq.PRMSEQPuffCKD;
//                    break;

//                case "PRMSEQPUFO500":
//                    _AplicativoEscolhido = PRMSeq.PRMSeqPufO500;
//                    break;

//                case "PRMSEQPUFMANG":
//                    _AplicativoEscolhido = PRMSeq.PRMSeqPufMang;
//                    break;

//                case "PRMSEQQUADROS":
//                    _AplicativoEscolhido = PRMSeq.PRMSeqQuadros;
//                    break;

//            }

//            return ObterVersaoPRMSeqOutros(_AplicativoEscolhido);
//        }

//        public static string ObterVersaoPRMSeqOutros(Funcoes.PRMSeq AplicativoEscolhido)
//        {
//            string Versao = "1.0.0.0";

//            switch (AplicativoEscolhido)
//            {
//                case Funcoes.PRMSeq.PRMSeqAcopla:
//                case Funcoes.PRMSeq.PRMSeqKit:
//                    Versao = string.Concat("1.6.9.1");
//                    break;

//                case Funcoes.PRMSeq.PRMSeqKitEixo:
//                    Versao = string.Concat("1.0.1.0");
//                    break;

//                case Funcoes.PRMSeq.PRMSeqPintura:
//                    Versao = string.Concat("1.2.2.0");
//                    break;

//                case Funcoes.PRMSeq.PRMSeqGalpao:
//                    Versao = string.Concat("1.4.3.0");
//                    break;

//                case Funcoes.PRMSeq.PRMSeqElevZ:
//                    Versao = string.Concat("1.0.0.0");
//                    break;

//                case Funcoes.PRMSeq.PRMSeqMfBus:
//                    Versao = string.Concat("1.0.0.0");
//                    break;

//                case Funcoes.PRMSeq.PRMSeqQuadros:
//                    Versao = string.Concat("1.2.8.0");
//                    break;

//                case Funcoes.PRMSeq.PRMSeqLote:
//                    Versao = string.Concat("2.2.1.0");
//                    break;

//                case Funcoes.PRMSeq.PRMSeqLogistica:
//                    Versao = string.Concat("1.0.0.0");
//                    break;

//                case Funcoes.PRMSeq.PRMSEQPuffCKD:
//                    Versao = string.Concat("1.0.0.0");
//                    break;

//                case Funcoes.PRMSeq.PRMSeqPufO500:
//                    Versao = string.Concat("1.0.0.0");
//                    break;

//                case Funcoes.PRMSeq.PRMSeqPufMang:
//                    Versao = string.Concat("1.0.0.0");
//                    break;

//                case Funcoes.PRMSeq.PRMPortal:
//                    Versao = string.Concat("1.2.2.0");
//                    break;

//                case Funcoes.PRMSeq.PRMSeqKitCkd:
//                    Versao = string.Concat("1.0.9.7");
//                    break;

//            }

//            return Versao;
//        }

//        static string ObterOraclePooling(string StringConexao)
//        {
//            string ComplementoConexao = "";

//            DataSet ds = new DataSet();
//            OracleConnection conn = new OracleConnection();

//            try
//            {
//                StringConexao = string.Concat(StringConexao, "Pooling=False;Min Pool Size=1;Max Pool Size=1;Connection Lifetime=1;");
//                conn = new OracleConnection(StringConexao);

//                string sql = "SELECT NUMOBJ, COLIE1 FROM TG9 WHERE NUDOCT = 'WEB' AND TIREST = 'WB' ORDER BY COLIE2";

//                conn.Open();
//                OracleDataAdapter da = new OracleDataAdapter(sql, conn);
//                da.Fill(ds);

//                conn.Close();
//                conn.Dispose();

//                if (ds.Tables[0].Rows.Count > 0)
//                {
//                    ComplementoConexao = "";
//                    foreach (DataRow dr in ds.Tables[0].Rows)
//                    {
//                        if (dr["NUMOBJ"].ToString().Trim().Equals("Pooling"))
//                            ComplementoConexao += string.Concat(dr["NUMOBJ"].ToString().Trim(), "=", (dr["COLIE1"].ToString().Trim() == "S" ? "True;" : "False;"));
//                        else
//                            ComplementoConexao += string.Concat(dr["NUMOBJ"].ToString().Trim(), "=", dr["COLIE1"].ToString().Trim() + ";");

//                    }
//                }
//            }
//            catch
//            {

//            }
//            finally
//            {
//                conn = null;
//            }


//            return ComplementoConexao;
//        }

//        public static string ObterCnnString(string Ambiente)
//        {
//            string Retorno = "";
//            string StringConexao = "";

//            if (Ambiente.Substring(1, 1).Equals("D"))
//                StringConexao = string.Concat("User ID=", Ambiente.ToLower(), "cl;Password=", Ambiente.ToLower(), "cl;Data Source=", Ambiente.ToUpper().Substring(0, 3) + "B", ";Persist Security Info=False;");
//            else
//                StringConexao = string.Concat("User ID=", Ambiente.ToLower(), "cl;Password=prmcli5305;Data Source=", Ambiente.ToUpper().Substring(0, 3) + "B", ";Persist Security Info=False;");

//            if (ConfigurationManager.AppSettings["Oracle_Pooling"] == null)
//            {
//                Retorno = StringConexao + ObterOraclePooling(StringConexao);
//            }
//            else if (ConfigurationManager.AppSettings["Oracle_Pooling"].Trim().Equals(""))
//            {
//                ConfigurationManager.AppSettings["Oracle_Pooling"] = ObterOraclePooling(StringConexao);
//                Retorno = StringConexao + ConfigurationManager.AppSettings["Oracle_Pooling"];
//            }
//            else
//            {
//                Retorno = StringConexao + ConfigurationManager.AppSettings["Oracle_Pooling"];
//            }

//            return Retorno;
//        }

//        public static string[] ObterTodosAmbientes(string Ambiente)
//        {
//            string LetraAmbiente = Ambiente.Substring(1, 1); //P - Produção - H - Homo - D - Desenv
//            string[] Ambientes = null;

//            //if (Ambiente.Substring(0, 1) == "G")
//            //    Ambientes = string.Concat("G", LetraAmbiente, "V1,G", LetraAmbiente, "E1,G", LetraAmbiente, "C1,G", LetraAmbiente, "M1").Split(new char[] { ',' });
//            //else
//            //    Ambientes = string.Concat("J", LetraAmbiente, "V1,").Split(new char[] { ',' });

//            if (Ambiente.Substring(1, 1).Equals("D"))
//                Ambientes = string.Concat("G", LetraAmbiente, "V1,G", LetraAmbiente, "E1,J", LetraAmbiente, "V1,J", LetraAmbiente, "V2,").Split(new char[] { ',' });
//            else
//                Ambientes = string.Concat("G", LetraAmbiente, "V1,G", LetraAmbiente, "V5,G", LetraAmbiente, "V6,G", LetraAmbiente, "E1,G", LetraAmbiente, "C1,G", LetraAmbiente, "M1,J", LetraAmbiente, "V1,J", LetraAmbiente, "V2,").Split(new char[] { ',' });

//            return Ambientes;
//        }

//        public static string[] ObterTodosBancosOracle(string Ambiente)
//        {
//            string Fabrica = Ambiente.Substring(0, 1); //G = SBC - J = JDF
//            string LetraAmbiente = Ambiente.Substring(1, 1); //P - Produção - H - Homo - D - Desenv
//            string[] Ambientes = null;
//            string _AmbientesV = "";
//            string _AmbientesM = "";
//            string _AmbientesE = "";
//            string _AmbientesC = "";

//            int[] NumerosVeiculos = null;
//            int[] NumerosMotores = null;
//            int[] NumerosEixos = null;
//            int[] NumerosCambios = null;

//            if (Fabrica == "G")
//            {
//                if (Ambiente == "D")
//                {
//                    NumerosVeiculos = new int[2];
//                    NumerosMotores = new int[2];
//                    NumerosEixos = new int[0];
//                    NumerosCambios = new int[0];
//                }
//                else
//                {
//                    NumerosVeiculos = new int[7];
//                    NumerosMotores = new int[6];
//                    NumerosEixos = new int[6];
//                    NumerosCambios = new int[5];
//                }
//            }
//            else
//            {
//                if (Ambiente == "D")
//                {
//                    NumerosVeiculos = new int[2];
//                    NumerosMotores = new int[0];
//                    NumerosEixos = new int[0];
//                    NumerosCambios = new int[0];
//                }
//                else
//                {
//                    NumerosVeiculos = new int[2];
//                    NumerosMotores = new int[0];
//                    NumerosEixos = new int[0];
//                    NumerosCambios = new int[0];
//                }
//            }


//            for (int i = 0; i < NumerosVeiculos.Length; i++)
//            {
//                if (i == 0)
//                    _AmbientesV += string.Concat(Fabrica, LetraAmbiente, "V", (i + 1).ToString());
//                else
//                    _AmbientesV += string.Concat(",", Fabrica, LetraAmbiente, "V", (i + 1).ToString());
//            }

//            for (int i = 0; i < NumerosMotores.Length; i++)
//            {
//                if (i == 0)
//                    _AmbientesM += string.Concat(Fabrica, LetraAmbiente, "M", (i + 1).ToString());
//                else
//                    _AmbientesM += string.Concat(",", Fabrica, LetraAmbiente, "M", (i + 1).ToString());
//            }

//            for (int i = 0; i < NumerosEixos.Length; i++)
//            {
//                if (i == 0)
//                    _AmbientesE += string.Concat(Fabrica, LetraAmbiente, "E", (i + 1).ToString());
//                else
//                    _AmbientesE += string.Concat(",", Fabrica, LetraAmbiente, "E", (i + 1).ToString());
//            }

//            for (int i = 0; i < NumerosCambios.Length; i++)
//            {
//                if (i == 0)
//                    _AmbientesC += string.Concat(Fabrica, LetraAmbiente, "C", (i + 1).ToString());
//                else
//                    _AmbientesC += string.Concat(",", Fabrica, LetraAmbiente, "C", (i + 1).ToString());
//            }

//            Ambientes = string.Concat(_AmbientesV, ",", _AmbientesM, ",", _AmbientesE, ",", _AmbientesC).Split(',');

//            return Ambientes;
//        }

//        public static string TextoEspacado(string strEntrada, int iTamEsp)
//        {
//            string TextoEspacado = "";
//            int i;
//            int iTam;
//            string strSaida = "";

//            strSaida = "";
//            iTam = strEntrada.Length;

//            for (i = 1; i <= iTam; i++)
//            {
//                strSaida = strSaida + Mid(strEntrada, i, 1) + new string(' ', iTamEsp);
//            } // i

//            TextoEspacado = strSaida.Trim();

//            return TextoEspacado;
//        }

//        public static string ObterDescricaoSistemas(string Sistema)
//        {
//            string Descricao = "";

//            if (Sistema.ToUpper().Trim().Equals("SGP")) Descricao = "Sistema de Gerenciamento da Produção";
//            if (Sistema.ToUpper().Trim().Equals("PRMAPOIO")) Descricao = "Apoio ao Gerenciamento";
//            if (Sistema.ToUpper().Trim().Equals("PRMALARME")) Descricao = "Gerenciador de Alarmes";
//            if (Sistema.ToUpper().Trim().Equals("PRMCHECKCOM")) Descricao = "Plausibilidade/Rastreabilidade e Captação de Componentes";
//            if (Sistema.ToUpper().Trim().Equals("PRMCTRACCESS")) Descricao = "Administração de Controle de Acesso";
//            if (Sistema.ToUpper().Trim().Equals("PRMKANBAN")) Descricao = "Analise e Administração de Disponibilidade de Kanban";
//            if (Sistema.ToUpper().Trim().Equals("PRMMAPLIN")) Descricao = "Mapeamento de Linha de Montagem";
//            if (Sistema.ToUpper().Trim().Equals("PRMMOBILE")) Descricao = "";
//            if (Sistema.ToUpper().Trim().Equals("PRMPLANLIN")) Descricao = "Planejamento e Montagem Final";
//            if (Sistema.ToUpper().Trim().Equals("PRMPRINT")) Descricao = "Gerenciador de Impressão";
//            if (Sistema.ToUpper().Trim().Equals("PRMSEQ")) Descricao = "Sequênciamento de Veículos e Agregados";
//            if (Sistema.ToUpper().Trim().Equals("PRMSEQ1044")) Descricao = "Sequenciamento de Motores e Câmbios para Acoplamento";
//            if (Sistema.ToUpper().Trim().Equals("PRMSEQELEVZ")) Descricao = "Sequenciamento de Cabinas para o Elevador Z";
//            if (Sistema.ToUpper().Trim().Equals("PRMSEQACOPLA")) Descricao = "Sequênciamento e Acoplamento de Veículos e Agregados";
//            if (Sistema.ToUpper().Trim().Equals("PRMSEQMON")) Descricao = "Gerenciamento da Produção de Veículos e Agregados";
//            if (Sistema.ToUpper().Trim().Equals("PRMTRAMIT")) Descricao = "Tramitação de Veículos e Agregados";
//            if (Sistema.ToUpper().Trim().Equals("PRMVARPROD")) Descricao = "Variáveis de Produção";
//            if (Sistema.ToUpper().Trim().Equals("PRMVINC")) Descricao = "Vinculação de Agregados";
//            if (Sistema.ToUpper().Trim().Equals("PRMSEQMFBUS")) Descricao = "Sequenciamento para Montagem Final de Ônibus";
//            if (Sistema.ToUpper().Trim().Equals("PRMSEQQUADROS")) Descricao = "Sequenciamento para Fechamento de Quadros";
//            if (Sistema.ToUpper().Trim().Equals("PRMSEQLOTE")) Descricao = "Sequenciamento em Lotes";
//            if (Sistema.ToUpper().Trim().Equals("PRMSEQLOG")) Descricao = "Sequenciamento de Logistica";
//            if (Sistema.ToUpper().Trim().Equals("PRMSEQPUFFCKD")) Descricao = "Sequenciamento do Puffer CKD";
//            if (Sistema.ToUpper().Trim().Equals("PRMSEQPUFO500")) Descricao = "Sequenciamento do Puffer O500";
//            if (Sistema.ToUpper().Trim().Equals("PRMSEQPUFMANG")) Descricao = "Sequenciamento do Puffer Manga";
//            if (Sistema.ToUpper().Trim().Equals("PRMSEQKITEIXO")) Descricao = "Sequenciamento para Kit de Eixos";
//            if (Sistema.ToUpper().Trim().Equals("PRMSEQJDF")) Descricao = "Sequenciamento para Kit de Cabinas JDF";
//            if (Sistema.ToUpper().Trim().Equals("PRMSEQPINTURA")) Descricao = "Sequenciamento de Cabina para Pintura";
//            if (Sistema.ToUpper().Trim().Equals("PRMSEQCKD")) Descricao = "Sequenciamento Ckd";
//            if (Sistema.ToUpper().Trim().Equals("PRMPROPMONT")) Descricao = "Proposta de Montagem";
//            if (Sistema.ToUpper().Trim().Equals("PRMSEQCABEC") || Sistema.ToUpper().Trim().Equals("PRMSEQCABEÇOTE")) Descricao = "Sequenciamento de Cabeçote";
//            if (Sistema.ToUpper().Trim().Equals("PRMSEQGALPAO") || Sistema.ToUpper().Trim().Equals("PRMSEQGALPÃO"))
//                Descricao = "Sequenciamento de Cabina para Embarque";
//            if (Sistema.ToUpper().Trim().Equals("SGPCLIENT")) Descricao = "Sistema de Gerenciamento da Produção";

//            return Descricao;
//        }

//        public static string UFFormataDataSQL(DateTime Data)
//        {
//            string UFFormataDataSQL = "";
//            // VBto upgrade warning: Ano As int	OnWrite(int)
//            int Ano;
//            // VBto upgrade warning: Mes As int	OnWrite(int)
//            int Mes;
//            // VBto upgrade warning: Dia As int	OnWrite(int)
//            int Dia;
//            Ano = Data.Year;
//            Mes = Data.Month;
//            Dia = Data.Day;

//            UFFormataDataSQL = "TO_DATE('" + new DateTime(Ano, Mes, Dia).ToString("yyyyMMdd") + "','YYYYMMDD')";

//            return UFFormataDataSQL;
//        }

//        public static string UFFormataDataSP(DateTime Data)
//        {
//            string UFFormataDataSP = "";

//            UFFormataDataSP = (Data).ToString("dd\\-MM\\-yyyy");

//            return UFFormataDataSP;
//        }

//        public static string FormataNumProd(string NUPROD)
//        {
//            try
//            {
//                if (NUPROD == "0" || NUPROD.Trim() == "")
//                {
//                    return "";
//                }
//                else
//                {
//                    return string.Concat(NUPROD.PadLeft(9, '0').Substring(0, 2), ".", NUPROD.PadLeft(9, '0').Substring(2, 6), "/", NUPROD.PadLeft(9, '0').Substring(8));
//                }
//            }
//            catch (Exception)
//            {
//                throw;
//            }
//        }

//        public static string GetOnlyNumbers(string NUPROD)
//        {
//            try
//            {
//                //return System.Text.RegularExpressions.Regex.Replace(NUPROD, @"[^0-9]+?", string.Empty).Replace(".", "").Replace(",", "");
//                return NUPROD.Replace(".", "").Replace(",", "").Replace("/", "");
//            }
//            catch (Exception)
//            {
//                throw;
//            }
//        }

//        public static string GetSomenteNumeros(string NUPROD)
//        {
//            try
//            {
//                return System.Text.RegularExpressions.Regex.Replace(NUPROD, @"[^0-9]+?", string.Empty).Replace(".", "").Replace(",", "");
//                //return NUPROD.Replace(".", "").Replace(",", "").Replace("/", "");
//            }
//            catch (Exception)
//            {
//                throw;
//            }
//        }

//        public static string FormataNumPedi(string NUPEDI)
//        {
//            try
//            {
//                if (NUPEDI == "0" || NUPEDI.Trim() == "")
//                {
//                    return "";
//                }
//                else
//                {
//                    return string.Concat(NUPEDI.PadLeft(9, '0').Substring(0, 2), ".", NUPEDI.PadLeft(9, '0').Substring(2, 3), ".", NUPEDI.PadLeft(9, '0').Substring(5));
//                }
//            }
//            catch (Exception)
//            {
//                throw;
//            }
//        }

//        public static string FormataNumeroString(string Numero, int QtInteiros)
//        {
//            return FormataNumeroString(Numero, QtInteiros, 0);
//        }

//        public static string FormataNumeroString(string Numero, int QtInteiros, int QtDecimais)
//        {
//            string strFormatado = "0";
//            string strFornmato = "";
//            string strFator = "1";
//            double dblAux = 0;

//            strFator = strFator.PadRight(QtDecimais + 1, '0');

//            if (UFEhNumerico(Numero.Replace(",", "")))
//            {
//                if (QtDecimais > 0)
//                {
//                    if (Numero.Contains(','))
//                    {
//                        dblAux = Convert.ToInt64(Numero.Replace(",", ""));
//                    }
//                    else
//                    {
//                        dblAux = Convert.ToInt64(Numero) * Convert.ToInt64(strFator);
//                    }
//                }
//                else
//                {
//                    dblAux = Convert.ToInt64(Numero);
//                }
//            }
//            else
//            {
//                dblAux = 0;
//            }

//            dblAux = dblAux / Convert.ToInt64(strFator);
//            strFornmato = "";

//            if (QtDecimais > 0)
//            {
//                strFornmato += ("0").PadRight((QtInteiros - QtDecimais), '0') + ".";
//                strFornmato += ("0").PadRight(QtDecimais, '0');
//            }
//            else
//            {
//                strFornmato = strFornmato.PadRight(QtInteiros, '0');
//            }

//            strFormatado = dblAux.ToString(strFornmato);
//            strFormatado = strFormatado.Replace(",", "");

//            return strFormatado;
//        }

//        public static string FormataNumeroDecimal(string Numero, int CasaDecimal)
//        {
//            string FormataNumeroDecimal = "";
//            string strAux = "";

//            strAux = Right(Numero, CasaDecimal);
//            Numero = Mid(Numero, 1, Numero.Length - CasaDecimal);

//            FormataNumeroDecimal = Numero + "," + strAux;

//            return FormataNumeroDecimal;
//        }

//        public static string LeftText(string Texto, string Delimitador)
//        {
//            int Posicao = InStr(0, Texto, Delimitador);
//            if (Posicao > 0)
//            {
//                return Texto.Substring(0, Posicao);
//            }
//            return "";
//        }

//        public static string RightText(string Texto, string Delimitador)
//        {
//            int Posicao = InStr(0, Texto, Delimitador);
//            if (Posicao > 0)
//            {
//                return Texto.Substring(Posicao + Delimitador.Length);
//            }
//            return "";
//        }



//        public static string ObterValorListView(ListView lvw, ListViewItemSelectionChangedEventArgs e, string NomeCampo)
//        {
//            int QualColuna = -1;
//            for (int c = 0; c < lvw.Columns.Count; c++)
//            {
//                if (lvw.Columns[c].Name.Trim().ToUpper().Equals(NomeCampo.Trim().ToUpper()))
//                {
//                    QualColuna = c;
//                    break;
//                }
//            }

//            if (QualColuna > -1)
//            {
//                string Retorno = e.Item.SubItems[QualColuna].Text;

//                switch (NomeCampo)
//                {
//                    case "NUPROD":
//                    case "NUPRVE":
//                    case "NUPRVA":
//                        {
//                            Retorno = Retorno.Replace(".", "").Replace("/", "");
//                            break;
//                        }
//                }

//                return Retorno;

//            }
//            else
//                return "";
//        }

//        public static int ObterColunaListViewItem(ListViewItem lvi, string NomeCampo)
//        {
//            if (lvi == null)
//                return -1;

//            ListView lvw = lvi.ListView;
//            int QualColuna = -1;
//            for (int c = 0; c < lvw.Columns.Count; c++)
//            {
//                if (lvw.Columns[c].Name.Trim().ToUpper().Equals(NomeCampo.Trim().ToUpper()))
//                {
//                    QualColuna = c;
//                    break;
//                }
//            }

//            return QualColuna;
//        }

//        public static string ObterValorListViewItem(ListViewItem lvi, string NomeCampo)
//        {
//            if (lvi == null)
//                return "";

//            ListView lvw = lvi.ListView;
//            int QualColuna = -1;
//            for (int c = 0; c < lvw.Columns.Count; c++)
//            {
//                if (lvw.Columns[c].Name.Trim().ToUpper().Equals(NomeCampo.Trim().ToUpper()))
//                {
//                    QualColuna = c;
//                    break;
//                }
//            }

//            if (QualColuna > -1)
//                if (lvi.SubItems.Count > 1)
//                {
//                    if (QualColuna > lvi.SubItems.Count)
//                    {
//                        return "";
//                    }
//                    else
//                    {
//                        string Retorno = lvi.SubItems[QualColuna].Text;

//                        switch (NomeCampo)
//                        {
//                            case "NUPROD":
//                            case "NUPRVE":
//                            case "NUPRVA":
//                                {
//                                    Retorno = Retorno.Replace(".", "").Replace("/", "");
//                                    break;
//                                }
//                        }

//                        return Retorno;
//                    }

//                }
//                else
//                    return "";
//            else
//                return "";
//        }

//        public static string ObterValorListView(ListView lvw, string NomeCampo, int LinhaListView)
//        {
//            int QualColuna = -1;
//            for (int c = 0; c < lvw.Columns.Count; c++)
//            {
//                if (lvw.Columns[c].Name.Trim().ToUpper().Equals(NomeCampo.Trim().ToUpper()))
//                {
//                    QualColuna = c;
//                    break;
//                }
//            }

//            if (QualColuna > -1)
//                if (lvw.Items[LinhaListView].SubItems.Count > 1)
//                {
//                    if (QualColuna > lvw.Items[LinhaListView].SubItems.Count)
//                    {
//                        return "";
//                    }
//                    else
//                    {

//                        string Retorno = lvw.Items[LinhaListView].SubItems[QualColuna].Text;

//                        switch (NomeCampo)
//                        {
//                            case "NUPROD":
//                            case "NUPRVE":
//                            case "NUPRVA":
//                                {
//                                    Retorno = Retorno.Replace(".", "").Replace("/", "");
//                                    break;
//                                }
//                        }

//                        return Retorno;
//                    }

//                }
//                else
//                    return "";
//            else
//                return "";
//        }

//        public static string ObterValorListView(ref ListView lvw, string NomeCampo)
//        {
//            if (lvw.SelectedItems == null)
//                return "";

//            int QualColuna = -1;
//            for (int c = 0; c < lvw.Columns.Count; c++)
//            {
//                if (lvw.Columns[c].Name.Trim().ToUpper().Equals(NomeCampo.Trim().ToUpper()))
//                {
//                    QualColuna = c;
//                    break;
//                }
//            }

//            if (QualColuna > -1)
//                if (lvw.SelectedItems.Count > 0)
//                {
//                    string Retorno = lvw.SelectedItems[0].SubItems[QualColuna].Text;

//                    switch (NomeCampo)
//                    {
//                        case "NUPROD":
//                        case "NUPRVE":
//                            {
//                                Retorno = Retorno.Replace(".", "").Replace("/", "");
//                                break;
//                            }
//                    }

//                    return Retorno;
//                }
//                else
//                    return "";
//            else
//                return "";
//        }

//        public static string ObterValorDataGridView(ref DataGridView dgv, string NomeCampo)
//        {
//            if (dgv.SelectedCells == null)
//                return "";

//            int QualColuna = -1;

//            for (int c = 0; c < dgv.Columns.Count; c++)
//            {
//                if (dgv.Columns[c].Name.Trim().ToUpper().Equals(NomeCampo.Trim().ToUpper()))
//                {
//                    QualColuna = c;
//                    break;
//                }
//            }

//            if (QualColuna > -1)
//                if (dgv.SelectedCells.Count > 0)
//                {
//                    string Retorno = dgv.SelectedRows[0].Cells[QualColuna].Value.ToString();

//                    switch (NomeCampo)
//                    {
//                        case "NUPROD":
//                        case "NUPRVE":
//                            {
//                                Retorno = Retorno.Replace(".", "").Replace("/", "");
//                                break;
//                            }
//                    }

//                    return Retorno;
//                }
//                else
//                    return "";
//            else
//                return "";
//        }

//        public static string UFParametrosExecucao(string FilaRetorno)
//        {
//            return UFParametrosExecucao(FilaRetorno, "");
//        }

//        public static string UFParametrosExecucao(string FilaRetorno, string CHV_NUDOCT)
//        {
//            return UFParametrosExecucao(FilaRetorno, CHV_NUDOCT, false);
//        }

//        public static string UFParametrosExecucao(string FilaRetorno, string CHV_NUDOCT, bool FilaPrinter)
//        {
//            return UFParametrosExecucao(FilaRetorno, CHV_NUDOCT, FilaPrinter, "");
//        }

//        public static string UFParametrosExecucao(string FilaRetorno, string CHV_NUDOCT, bool FilaPrinter, string CHV_REFILA)
//        {
//            ParamentroExecucao PARAMETROS_EXECUCAO = new ParamentroExecucao();

//            PARAMETROS_EXECUCAO.Status = "  ";
//            PARAMETROS_EXECUCAO.COEMPR = AmbienteInfo._Empresa;
//            PARAMETROS_EXECUCAO.COMMIT = "S"; // (S/N) Executar Commit
//            PARAMETROS_EXECUCAO.COTERM = AmbienteInfo._LUR;
//            PARAMETROS_EXECUCAO.COUSAR = AmbienteInfo._ChaveUsuario.ToUpper();
//            PARAMETROS_EXECUCAO.NUDOCT = CHV_NUDOCT.Trim() != "" ? CHV_NUDOCT.ToUpper().Trim() : "B22970";
//            if (CHV_REFILA.Trim().ToUpper() == "N")
//            {
//                PARAMETROS_EXECUCAO.REFILA = "N"; // (S/N) Retorno de Fila
//                PARAMETROS_EXECUCAO.Fila_Retorno = "";
//            }
//            else
//            {
//                PARAMETROS_EXECUCAO.REFILA = "S";
//                PARAMETROS_EXECUCAO.Fila_Retorno = FilaRetorno;
//            }
//            PARAMETROS_EXECUCAO.SIAREA = AmbienteInfo._SiglaArea;
//            PARAMETROS_EXECUCAO.TIUTIL = AmbienteInfo._TiuTil;

//            string sPar = PARAMETROS_EXECUCAO.ToString();
//            if (sPar.Length > 86)
//                sPar = sPar.Substring(0, 86);

//            return sPar;
//        }

//        public static string UFParametrosExecucaoBPTVFAG(string FilaRetorno, string CHV_NUDOCT, string CHV_REFILA)
//        {
//            ParamentroExecucao PARAMETROS_EXECUCAO = new ParamentroExecucao();

//            PARAMETROS_EXECUCAO.Status = "  ";
//            PARAMETROS_EXECUCAO.COEMPR = AmbienteInfo._Empresa;
//            PARAMETROS_EXECUCAO.TIUTIL = AmbienteInfo._TiuTil;
//            PARAMETROS_EXECUCAO.SIAREA = AmbienteInfo._SiglaArea;
//            PARAMETROS_EXECUCAO.COUSAR = AmbienteInfo._ChaveUsuario.ToUpper();
//            PARAMETROS_EXECUCAO.COTERM = AmbienteInfo._LUR;
//            PARAMETROS_EXECUCAO.NUDOCT = CHV_NUDOCT.Trim() != "" ? CHV_NUDOCT.ToUpper().Trim() : "B22970";
//            PARAMETROS_EXECUCAO.COMMIT = "S"; // (S/N) Executar Commit

//            if (CHV_REFILA.Trim().ToUpper() == "N")
//            {
//                PARAMETROS_EXECUCAO.REFILA = "N"; // (S/N) Retorno de Fila
//                PARAMETROS_EXECUCAO.Fila_Retorno = "";
//            }
//            else
//            {
//                PARAMETROS_EXECUCAO.REFILA = "S";
//                PARAMETROS_EXECUCAO.Fila_Retorno = FilaRetorno;
//            }

//            string sPar = PARAMETROS_EXECUCAO.ToString();
//            if (sPar.Length > 86)
//                sPar = sPar.Substring(0, 86);

//            return sPar;
//        }

//        public static string UFGeraNome(bool FilaPrinter, string NUMERO_DIFERENCIAL)
//        {
//            string UFGeraNome = "";
//            string strTerminal = "";
//            string CodigoTerminal = AmbienteInfo._LUR;
//            string strGUID = Guid.NewGuid().ToString().Replace("-", "");

//            try
//            {
//                strTerminal = CodigoTerminal;

//                if (FilaPrinter)
//                {
//                    strTerminal = strTerminal.Replace("IL", "");
//                    UFGeraNome = string.Concat("BPMSTAR.MGPV.BPREPLY.REIMPRE.PR.", strTerminal, ".", strGUID);
//                }
//                else
//                {
//                    if (strTerminal.Trim() == string.Empty)
//                        UFGeraNome = string.Concat("BPMSTAR.MGPV.BPREPLY.WW.", strGUID);
//                    else
//                        UFGeraNome = string.Concat("BPMSTAR.MGPV.BPREPLY.CL.", strTerminal, ".", strGUID);
//                }

//                //Informação Douglas Gardelli - 01/07/2016 8:55
//                if (UFGeraNome.Length > 48)
//                    UFGeraNome = UFGeraNome.Substring(0, 48);

//                return UFGeraNome.Replace("-", "0");
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }

//        public static string UFGeraNome(bool FilaPrinter)
//        {
//            return UFGeraNome(FilaPrinter, "");
//        }

//        public static string PCInv5_To_PCInv4(string codigoItem)
//        {
//            if (codigoItem.Trim().Equals(string.Empty)) return string.Empty;

//            string strStatus = "";
//            return PCInv5_To_PCInv4(codigoItem.ToUpper(), ref strStatus);
//        }

//        public static string PCInv5_To_PCInv4(string codigoItem, ref string strStatus)
//        {
//            if (codigoItem.Trim().Equals(string.Empty)) return string.Empty;

//            string Retorno = "";

//            //CL_PCINV.PCINV clPcInv = new CL_PCINV.PCINV();

//            DCX.ITLC.PCInv.Item clPcInv = new Item();

//            try
//            {
//                //string strStatus = "";
//                string strSaida1 = "";
//                string strSaida2 = "";
//                string strSaida3 = "";
//                string strSaida4 = "";
//                string strSaida5 = "";
//                string strSaida6 = "";

//                clPcInv.ConverterNumero(codigoItem.ToUpper(), "5", "NNNSSS", out strStatus, out strSaida1, out strSaida2, out strSaida3, out strSaida4, out strSaida5, out strSaida6);

//                if (strStatus.Trim().Equals(""))
//                {
//                    Retorno = strSaida5;
//                }
//                else
//                {
//                    throw new Exception("Erro na conversão do PCINV - Status: " + strStatus + "#");
//                }
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//            finally
//            {
//                clPcInv = null;
//            }

//            return Retorno.Trim();
//        }

//        public static string PCInv5_To_PCInv3(string codigoItem)
//        {
//            if (codigoItem.Trim().Equals(string.Empty)) return string.Empty;

//            string strStatus = "";
//            string Retorno = "";

//            //CL_PCINV.PCINV clPcInv = new CL_PCINV.PCINV();

//            DCX.ITLC.PCInv.Item clPcInv = new Item();

//            try
//            {
//                //string strStatus = "";
//                string strSaida1 = "";
//                string strSaida2 = "";
//                string strSaida3 = "";
//                string strSaida4 = "";
//                string strSaida5 = "";
//                string strSaida6 = "";

//                clPcInv.ConverterNumero(codigoItem.ToUpper(), "5", "NNNSSS", out strStatus, out strSaida1, out strSaida2, out strSaida3, out strSaida4, out strSaida5, out strSaida6);

//                if (strStatus.Trim().Equals(""))
//                {
//                    Retorno = strSaida4;
//                }
//                else
//                {
//                    throw new Exception("Erro na conversão do PCINV - Status: " + strStatus + "#");
//                }
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//            finally
//            {
//                clPcInv = null;
//            }

//            return Retorno.Trim();
//        }

//        public static string PCInv4_To_PCInv5(string codigoItem)
//        {
//            if (codigoItem.Trim().Equals(string.Empty)) return string.Empty;

//            string Retorno = "";

//            //CL_PCINV.PCINV clPcInv = new CL_PCINV.PCINV();

//            DCX.ITLC.PCInv.Item clPcInv = new Item();

//            try
//            {
//                string strStatus = "";
//                string strSaida1 = "";
//                string strSaida2 = "";
//                string strSaida3 = "";
//                string strSaida4 = "";
//                string strSaida5 = "";
//                string strSaida6 = "";

//                clPcInv.ConverterNumero(codigoItem, "4", "NNNSSS", out strStatus, out strSaida1, out strSaida2, out strSaida3, out strSaida4, out strSaida5, out strSaida6);

//                if (strStatus.Trim().Equals(""))
//                {
//                    //Após convertido - formato 5 - EDITADO TECNICO
//                    Retorno = strSaida6;
//                }
//                else
//                {
//                    throw new Exception("Erro na conversão do PCINV - Status: " + strStatus + "#");
//                }
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//            finally
//            {
//                clPcInv = null;
//            }

//            return Retorno.Trim();
//        }

//        public static string PCInv4_To_PCInv3(string codigoItem)
//        {
//            if (codigoItem.Trim().Equals(string.Empty)) return string.Empty;

//            string Retorno = "";

//            //CL_PCINV.PCINV clPcInv = new CL_PCINV.PCINV();
//            DCX.ITLC.PCInv.Item clPcInv = new Item();
//            try
//            {
//                string strStatus = "";
//                string strSaida1 = "";
//                string strSaida2 = "";
//                string strSaida3 = "";
//                string strSaida4 = "";
//                string strSaida5 = "";
//                string strSaida6 = "";

//                clPcInv.ConverterNumero(codigoItem.ToUpper(), "4", "NNNSSS", out strStatus, out strSaida1, out strSaida2, out strSaida3, out strSaida4, out strSaida5, out strSaida6);

//                if (strStatus.Trim().Equals(""))
//                {
//                    //Após convertido - formato 3 - DESINVERTIDO TECNICO
//                    Retorno = strSaida4;
//                }
//                else
//                {
//                    throw new Exception("Erro na conversão do PCINV - Status: " + strStatus + "#");
//                }
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//            finally
//            {
//                clPcInv = null;
//            }

//            return Retorno.Trim();
//        }

//        public static string PCInv3_To_PCInv4(string codigoItem)
//        {
//            if (codigoItem.Trim().Equals(string.Empty)) return string.Empty;

//            string status = "";
//            return PCInv3_To_PCInv4(codigoItem.ToUpper(), ref status).Trim();
//        }

//        public static string PCInv3_To_PCInv4(string codigoItem, ref string strStatus)
//        {
//            if (codigoItem.Trim().Equals(string.Empty)) return string.Empty;

//            string Retorno = "";

//            //CL_PCINV.PCINV clPcInv = new CL_PCINV.PCINV();
//            DCX.ITLC.PCInv.Item clPcInv = new Item();

//            try
//            {
//                //string strStatus = "";
//                string strSaida1 = "";
//                string strSaida2 = "";
//                string strSaida3 = "";
//                string strSaida4 = "";
//                string strSaida5 = "";
//                string strSaida6 = "";

//                clPcInv.ConverterNumero(codigoItem.ToUpper(), "3", "NNNSSS", out strStatus, out strSaida1, out strSaida2, out strSaida3, out strSaida4, out strSaida5, out strSaida6);

//                if (strStatus.Trim().Equals(""))
//                {
//                    //Após convertido - formato 4 - INVERTIDO TECNICO
//                    Retorno = strSaida5;
//                }
//                else
//                {
//                    throw new Exception("Erro na conversão do PCINV - Status: " + strStatus + "#");
//                }
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//            finally
//            {
//                clPcInv = null;
//            }

//            return Retorno.Trim();
//        }

//        public static string PCInv3_To_PCInv5(string codigoItem)
//        {
//            if (codigoItem.Trim().Equals(string.Empty)) return string.Empty;

//            string Retorno = "";

//            //CL_PCINV.PCINV clPcInv = new CL_PCINV.PCINV();

//            DCX.ITLC.PCInv.Item clPcInv = new Item();

//            try
//            {
//                string strStatus = "";
//                string strSaida1 = "";
//                string strSaida2 = "";
//                string strSaida3 = "";
//                string strSaida4 = "";
//                string strSaida5 = "";
//                string strSaida6 = "";

//                clPcInv.ConverterNumero(codigoItem.ToUpper(), "3", "NNNSSS", out strStatus, out strSaida1, out strSaida2, out strSaida3, out strSaida4, out strSaida5, out strSaida6);

//                if (strStatus.Trim().Equals(""))
//                {
//                    Retorno = strSaida6;
//                }
//                else
//                {
//                    throw new Exception("Erro na conversão do PCINV - Status: " + strStatus + "#");
//                }
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//            finally
//            {
//                clPcInv = null;
//            }

//            return Retorno.Trim();
//        }

//        public static string UFFormatDataOracle(DateTime Data, bool Formato24)
//        {
//            object vUFFormatDataOracle = 0;
//            vUFFormatDataOracle = (Data).ToString("dd\\-MM\\-yyyy HH\\:mm\\:ss");
//            return vUFFormatDataOracle.ToString();
//        }

//        public static string UFFormatDataOracle(DateTime Data)
//        {
//            object vUFFormatDataOracle = 0;
//            vUFFormatDataOracle = (Data).ToString("dd\\-MM\\-yyyy hh\\:mm\\:ss");
//            return vUFFormatDataOracle.ToString();
//        }

//        public static string DescricaoDominio(string Dominio, int Tipo)
//        {
//            string Retorno = "";

//            switch (Dominio)
//            {
//                case "D":
//                    {
//                        if (Tipo == 0) Retorno = "DC";
//                        if (Tipo == 1) Retorno = "LDAP://dc.com.br";
//                        //if (Tipo == 1) Retorno =  "LDAP://OU=Users,OU=T-Systems,DC=dc,DC=com,DC=br";
//                        break;
//                    }
//                case "A":
//                    {
//                        if (Tipo == 0) Retorno = "AMERICAS";
//                        if (Tipo == 1) Retorno = "LDAP://americas.corpdir.net";
//                        //if (Tipo == 1) Retorno =  "LDAP://OU=Users,OU=_GlobalResources,OU=SBC,OU=D154,DC=americas,DC=corpdir,DC=net";
//                        break;
//                    }
//                case "E":
//                    {
//                        if (Tipo == 0) Retorno = "EXTRANET";
//                        if (Tipo == 1) Retorno = "";
//                        break;
//                    }
//                default:
//                    {
//                        Retorno = "";
//                        break;
//                    }
//            }
//            return Retorno;
//        }

//        public static string ObterTerminal()
//        {
//            string ObterTerminal = "";

//            if (LerIni("MainSettings", "TERMINAL_FIXO") == "N")
//            {
//                ObterTerminal = LerIni("MainSettings", "TERMINAL_ID");
//                return ObterTerminal;
//            }

//            return "IL" + Environment.MachineName.Substring(Environment.MachineName.Length - 6, 6);
//        }

//        public static string LerIni(string sSection, string sKey, string sDefault, string Destino)
//        {
//            string Result = "";

//            if (Destino.Equals("PRN"))
//                Result = LerIniPrint(sSection, sKey, sDefault);
//            else
//            {
//                try
//                {
//                    Result = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("MBBras").OpenSubKey(PastaSistemaRegistry).OpenSubKey(sSection).GetValue(sKey).ToString();
//                    if (Result == "")
//                        Result = sDefault;
//                }
//                catch
//                {
//                    Result = sDefault;
//                }
//            }
//            return Result;
//        }

//        public static string LerIni(string sSection, string sKey, string sDefault)
//        {
//            string Result = "";

//            try
//            {
//                Result = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("MBBras").OpenSubKey(PastaSistemaRegistry).OpenSubKey(sSection).GetValue(sKey).ToString();
//                if (Result == "")
//                    Result = sDefault;
//            }
//            catch
//            {
//                Result = sDefault;
//            }
//            return Result;
//        }

//        public static string LerIniPrint(string sSection, string sKey)
//        {
//            return LerIniPrint(sSection, sKey, "");
//        }

//        public static string LerIniPrint(string sSection, string sKey, string sDefault)
//        {
//            return LerIniPrint(sSection, sKey, sDefault, false);
//        }

//        public static string LerIniPrint(string sSection, string sKey, bool UserSettings)
//        {
//            return LerIniPrint(sSection, sKey, "", UserSettings);
//        }

//        public static string LerIniPrint(string sSection, string sKey, string sDefault, bool UserSettings)
//        {
//            string Result = "";
//            string PathAppData = "";

//            System.OperatingSystem os = Environment.OSVersion;
//            Version vs = os.Version;
//            bool Win7 = false;
//            switch (vs.Major)
//            {
//                case 3:
//                case 4:
//                case 5:
//                    Win7 = false;
//                    break;

//                default:
//                    Win7 = true;
//                    break;
//            }
//            vs = null;
//            os = null;

//            if (!UserSettings)
//                PathAppData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
//            else
//                PathAppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);


//            string FileINIPath = "";
//            if (Win7)
//                FileINIPath = string.Concat(PathAppData, @"\MBBRAS\PRM\");
//            else
//                FileINIPath = string.Concat(PathAppData, @"\DCBR IT\PRM\");


//            if (!System.IO.Directory.Exists(FileINIPath))
//                System.IO.Directory.CreateDirectory(FileINIPath);


//            FileINIPath += "PRMPRINT.INI";

//            string sString = new string(' ', 32768);
//            long lngCode = GetPrivateProfileString(sSection, sKey, sDefault, sString, sString.Length, FileINIPath);

//            try
//            {
//                Result = sString.Split('\0')[0];
//            }
//            catch
//            {
//                Result = "";
//            }

//            return Result;
//        }

//        public static string AppDataPath()
//        {
//            string PathAppData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

//            System.OperatingSystem os = Environment.OSVersion;
//            Version vs = os.Version;
//            bool Win7 = false;
//            switch (vs.Major)
//            {
//                case 3:
//                case 4:
//                case 5:
//                    Win7 = false;
//                    break;
//                case 6:
//                    Win7 = true;
//                    break;
//            }
//            vs = null;
//            os = null;

//            string strAppData = "";
//            if (Win7)
//                strAppData = string.Concat(PathAppData, @"\MBBRAS\PRM\");
//            else
//                strAppData = string.Concat(PathAppData, @"\DCBR IT\PRM\");

//            return strAppData;
//        }

//        public static string ProgramFilesPath()
//        {
//            string PathAppData = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);

//            string strAppData = string.Concat(PathAppData, @"\MBBRAS\PRM\");

//            return strAppData;
//        }

//        public static string StartupPathSGP()
//        {
//            string PathAppData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

//            string strAppData = string.Concat(PathAppData, @"\MBBRAS\SGP\SGPClient\");

//            return strAppData;
//        }

//        public static string LerIni(string sSection, string sKey)
//        {
//            string Result = "";

//            try
//            {
//                Result = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("MBBras").OpenSubKey(PastaSistemaRegistry).OpenSubKey(sSection).GetValue(sKey).ToString();
//            }
//            catch
//            {
//                Result = "";
//            }
//            return Result;
//        }

//        public static string LerIni(string sSection, ToolStripComboBox cControl, Form FormOrigem)
//        {
//            string sKey = "";
//            if (FormOrigem.MdiParent == null)
//                sKey = FormOrigem.Name + "." + cControl.Name;
//            else
//                sKey = FormOrigem.MdiParent.Name + "." + FormOrigem.Name + "." + cControl.Name;

//            string sVal = LerIni(sSection, sKey);
//            if (!sVal.Equals(string.Empty))
//                return sVal;
//            else
//                return cControl.Text;
//        }

//        public static string LerIni(string sSection, ToolStripTextBox cControl, Form FormOrigem)
//        {
//            string sKey = "";
//            if (FormOrigem.MdiParent == null)
//                sKey = FormOrigem.Name + "." + cControl.Name;
//            else
//                sKey = FormOrigem.MdiParent.Name + "." + FormOrigem.Name + "." + cControl.Name;

//            string sVal = LerIni(sSection, sKey);
//            if (sVal.Equals(string.Empty))
//                return cControl.Text;
//            else
//                return sVal;
//        }

//        public static void LerIni(string sSection, ref TextBox cControl, Form FormOrigem)
//        {
//            cControl.Text = LerIni(sSection, cControl, FormOrigem);
//        }

//        public static void LerIni(string sSection, ref ToolStripTextBox cControl, Form FormOrigem)
//        {
//            cControl.Text = LerIni(sSection, cControl, FormOrigem);
//        }

//        public static void LerIni(string sSection, ref ToolStripComboBox cControl, Form FormOrigem)
//        {
//            cControl.Text = LerIni(sSection, cControl, FormOrigem);
//        }

//        public static string LerIni(string sSection, TextBox cControl, Form FormOrigem)
//        {
//            string sKey = "";
//            if (FormOrigem.MdiParent == null)
//                sKey = FormOrigem.Name + "." + cControl.Name;
//            else
//                sKey = FormOrigem.MdiParent.Name + "." + FormOrigem.Name + "." + cControl.Name;

//            string sVal = LerIni(sSection, sKey);
//            if (!sVal.Equals(string.Empty))
//                return sVal;
//            else
//                return cControl.Text;
//        }

//        public static void LerIni(string sSection, ref ComboBox cControl, Form FormOrigem)
//        {
//            cControl.Text = LerIni(sSection, cControl, FormOrigem);
//        }

//        public static string LerIni(string sSection, ComboBox cControl, Form FormOrigem)
//        {
//            string sKey = "";
//            if (FormOrigem.MdiParent == null)
//                sKey = FormOrigem.Name + "." + cControl.Name;
//            else
//                sKey = FormOrigem.MdiParent.Name + "." + FormOrigem.Name + "." + cControl.Name;

//            string sVal = LerIni(sSection, sKey);
//            if (!sVal.Equals(string.Empty))
//                return sVal;
//            else
//                return cControl.Text;
//        }



//        public static string ObterBaseVeiculo(string QualAmbiente)
//        {
//            string vObterBaseVeiculo = "";
//            string SiglaAmbiente = QualAmbiente.Substring(0, 2);

//            switch (SiglaAmbiente)
//            {

//                //Desenvolvimento Juiz de Fora
//                case "JD":
//                    vObterBaseVeiculo = "JDV1";
//                    break;
//                //Producao Juiz de Fora
//                case "JP":
//                    vObterBaseVeiculo = "JPV1";
//                    break;
//                //Homologacao Juiz de Fora
//                case "JH":
//                    vObterBaseVeiculo = "JHV1";
//                    break;
//                //Desenvolvimento SBC
//                case "GD":
//                    vObterBaseVeiculo = "GDV1";
//                    break;
//                //Homologacao SBC
//                case "GH":
//                    vObterBaseVeiculo = "GHV1";
//                    break;
//                //Producao SBC
//                case "GP":
//                    vObterBaseVeiculo = "GPV1";
//                    break;

//            }

//            return vObterBaseVeiculo;
//        }

//        public static string ObterBaseCaminhao(string QualAmbiente)
//        {
//            string vObterBaseVeiculo = "";
//            string SiglaAmbiente = QualAmbiente.Substring(0, 2);

//            switch (SiglaAmbiente)
//            {
//                //Desenvolvimento Juiz de Fora
//                case "JD":
//                    vObterBaseVeiculo = "JDV1";
//                    break;
//                //Producao Juiz de Fora
//                case "JP":
//                    vObterBaseVeiculo = "JPV1";
//                    break;
//                //Homologacao Juiz de Fora
//                case "JH":
//                    vObterBaseVeiculo = "JHV1";
//                    break;
//                //Desenvolvimento SBC
//                case "GD":
//                    vObterBaseVeiculo = "GDV1";
//                    break;
//                //Homologacao SBC
//                case "GH":
//                    vObterBaseVeiculo = "GHV1";
//                    break;
//                //Producao SBC
//                case "GP":
//                    vObterBaseVeiculo = "GPV1";
//                    break;

//            }

//            return vObterBaseVeiculo;
//        }

//        public static string ObterBaseOnibus(string QualAmbiente)
//        {
//            string vObterBaseVeiculo = "";
//            string SiglaAmbiente = QualAmbiente.Substring(0, 2);

//            switch (SiglaAmbiente)
//            {
//                //Desenvolvimento Juiz de Fora
//                case "JD":
//                    vObterBaseVeiculo = "JDV2";
//                    break;
//                //Producao Juiz de Fora
//                case "JP":
//                    vObterBaseVeiculo = "JPV2";
//                    break;
//                //Homologacao Juiz de Fora
//                case "JH":
//                    vObterBaseVeiculo = "JHV1";
//                    break;
//                //Desenvolvimento SBC
//                case "GD":
//                    vObterBaseVeiculo = "GDV2";
//                    break;
//                //Homologacao SBC
//                case "GH":
//                    vObterBaseVeiculo = "GHV4";
//                    break;
//                //Producao SBC
//                case "GP":
//                    vObterBaseVeiculo = "GPV4";
//                    break;

//            }

//            return vObterBaseVeiculo;
//        }

//        public static string ObterBaseCabina(string QualAmbiente)
//        {
//            string vObterBaseVeiculo = "";
//            string SiglaAmbiente = QualAmbiente.Substring(0, 2);

//            switch (SiglaAmbiente)
//            {
//                //Desenvolvimento Juiz de Fora
//                case "JD":
//                    vObterBaseVeiculo = "JDV2";
//                    break;
//                //Producao Juiz de Fora
//                case "JP":
//                    vObterBaseVeiculo = "JPV2";
//                    break;
//                //Homologacao Juiz de Fora
//                case "JH":
//                    vObterBaseVeiculo = "JHV2";
//                    break;
//                //Desenvolvimento SBC
//                case "GD":
//                    vObterBaseVeiculo = "GDV2";
//                    break;
//                //Homologacao SBC
//                case "GH":
//                    vObterBaseVeiculo = "GHV6";
//                    break;
//                //Producao SBC
//                case "GP":
//                    vObterBaseVeiculo = "GPV6";
//                    break;

//            }

//            return vObterBaseVeiculo;
//        }

//        public static string ObterBaseCambio(string QualAmbiente)
//        {
//            string vObterBaseVeiculo = "";
//            string SiglaAmbiente = QualAmbiente.Substring(0, 2);

//            switch (SiglaAmbiente)
//            {

//                //Desenvolvimento Juiz de Fora
//                case "JD":
//                    vObterBaseVeiculo = "JDV2";
//                    break;
//                //Producao Juiz de Fora
//                case "JP":
//                    vObterBaseVeiculo = "JPV2";
//                    break;
//                //Homologacao Juiz de Fora
//                case "JH":
//                    vObterBaseVeiculo = "JHV2";
//                    break;
//                //Desenvolvimento SBC
//                case "GD":
//                    vObterBaseVeiculo = "GDE1";
//                    break;
//                //Homologacao SBC
//                case "GH":
//                    vObterBaseVeiculo = "GHC1";
//                    break;
//                //Producao SBC
//                case "GP":
//                    vObterBaseVeiculo = "GPC1";
//                    break;
//            }

//            return vObterBaseVeiculo;
//        }

//        public static string ObterBaseMotor(string QualAmbiente)
//        {
//            string vObterBaseVeiculo = "";
//            string SiglaAmbiente = QualAmbiente.Substring(0, 2);

//            switch (SiglaAmbiente)
//            {

//                //Desenvolvimento Juiz de Fora
//                case "JD":
//                    vObterBaseVeiculo = "JDV2";
//                    break;
//                //Producao Juiz de Fora
//                case "JP":
//                    vObterBaseVeiculo = "JPV2";
//                    break;
//                //Homologacao Juiz de Fora
//                case "JH":
//                    vObterBaseVeiculo = "JHV2";
//                    break;
//                //Desenvolvimento SBC
//                case "GD":
//                    vObterBaseVeiculo = "GDE1";
//                    break;
//                //Homologacao SBC
//                case "GH":
//                    vObterBaseVeiculo = "GHM1";
//                    break;
//                //Producao SBC
//                case "GP":
//                    vObterBaseVeiculo = "GPM1";
//                    break;
//            }

//            return vObterBaseVeiculo;
//        }

//        public static string ObterBaseEixo(string QualAmbiente)
//        {
//            string vObterBaseVeiculo = "";
//            string SiglaAmbiente = QualAmbiente.Substring(0, 2);

//            switch (SiglaAmbiente)
//            {

//                //Desenvolvimento Juiz de Fora
//                case "JD":
//                    vObterBaseVeiculo = "JDV2";
//                    break;
//                //Producao Juiz de Fora
//                case "JP":
//                    vObterBaseVeiculo = "JPV2";
//                    break;
//                //Homologacao Juiz de Fora
//                case "JH":
//                    vObterBaseVeiculo = "JHV2";
//                    break;
//                //Desenvolvimento SBC
//                case "GD":
//                    vObterBaseVeiculo = "GDE1";
//                    break;
//                //Homologacao SBC
//                case "GH":
//                    vObterBaseVeiculo = "GHE1";
//                    break;
//                //Producao SBC
//                case "GP":
//                    vObterBaseVeiculo = "GPE1";
//                    break;
//            }

//            return vObterBaseVeiculo;
//        }

//        public static string QueueAtiva(ToolStripComboBox cboGrupo, string QualAmbiente)
//        {
//            string QueueAtiva = "";
//            Funcoes objFG;


//            try
//            {	// On Error GoTo ErrorHandler

//                objFG = new Funcoes();



//                // QueueAtiva = Funcoes.LerIni("MainSettings", "QM_PADRAO_CAMINHAO", "JPV1");

//                if (ObterFabrica(QualAmbiente) == Funcoes.Fabrica.JDF)
//                {
//                    // ********************************************************
//                    // ********************FABRICA DE JDF**********************
//                    // ********************************************************
//                    if (Funcoes.Mid(QualAmbiente, 1, 2) == "JP")
//                    {
//                        if ((int)cboGrupo.ComboBox.SelectedValue == (int)Funcoes.FGTipoLinha.Caminhao)
//                        {
//                            QueueAtiva = "JPV1";
//                        }
//                        else if ((int)cboGrupo.ComboBox.SelectedValue == (int)Funcoes.FGTipoLinha.Cabina)
//                        {
//                            QueueAtiva = "JPV2";
//                        }
//                        else
//                        {
//                            QueueAtiva = "JPV2";
//                        }
//                    }
//                    else
//                    {
//                        // Homologacao
//                        if (Funcoes.Mid(QualAmbiente, 1, 2) == "JH")
//                        {
//                            if ((int)cboGrupo.ComboBox.SelectedValue == (int)Funcoes.FGTipoLinha.Caminhao)
//                            {
//                                QueueAtiva = "JHV1";
//                            }
//                            else if ((int)cboGrupo.ComboBox.SelectedValue == (int)Funcoes.FGTipoLinha.Cabina)
//                            {
//                                QueueAtiva = "JHV2";
//                            }
//                            else
//                            {
//                                QueueAtiva = "JHV2";
//                            }
//                        }
//                        else
//                        {
//                            // Desenvolvimento
//                            if ((int)cboGrupo.ComboBox.SelectedValue == (int)Funcoes.FGTipoLinha.Caminhao)
//                            {
//                                QueueAtiva = "JDV1";
//                            }
//                            else if ((int)cboGrupo.ComboBox.SelectedValue == (int)Funcoes.FGTipoLinha.Cabina)
//                            {
//                                QueueAtiva = "JDV2";
//                            }
//                            else
//                            {
//                                QueueAtiva = "JDV2";
//                            }
//                        }
//                    }

//                }
//                else
//                {
//                    // ********************************************************
//                    // ********************FABRICA DE SBC**********************
//                    // ********************************************************
//                    if (Funcoes.Mid(QualAmbiente, 1, 2) == "GP")
//                    {
//                        if ((int)cboGrupo.ComboBox.SelectedValue == (int)Funcoes.FGTipoLinha.Cabina)
//                        {
//                            QueueAtiva = "GPV6";
//                        }
//                        else if ((int)cboGrupo.ComboBox.SelectedValue == (int)Funcoes.FGTipoLinha.Cambio)
//                        {
//                            QueueAtiva = "GPC2";
//                        }
//                        else if ((int)cboGrupo.ComboBox.SelectedValue == (int)Funcoes.FGTipoLinha.Eixo)
//                        {
//                            QueueAtiva = "GPE5";
//                        }
//                        else if ((int)cboGrupo.ComboBox.SelectedValue == (int)Funcoes.FGTipoLinha.Motor)
//                        {
//                            QueueAtiva = "GPM3";
//                        }
//                        else if ((int)cboGrupo.ComboBox.SelectedValue == (int)Funcoes.FGTipoLinha.Caminhao)
//                        {
//                            QueueAtiva = "GPV3";
//                        }
//                        else if ((int)cboGrupo.ComboBox.SelectedValue == (int)Funcoes.FGTipoLinha.Onibus)
//                        {
//                            QueueAtiva = "GPV5";
//                        }
//                        else
//                        {
//                            QueueAtiva = QualAmbiente;
//                        }
//                    }
//                    else
//                    {
//                        // Homologacao
//                        if (Funcoes.Mid(QualAmbiente, 1, 2) == "GH")
//                        {
//                            if ((int)cboGrupo.ComboBox.SelectedValue == (int)Funcoes.FGTipoLinha.Cabina)
//                            {
//                                QueueAtiva = "GHV6";
//                            }
//                            else if ((int)cboGrupo.ComboBox.SelectedValue == (int)Funcoes.FGTipoLinha.Cambio)
//                            {
//                                QueueAtiva = "GHC2";
//                            }
//                            else if ((int)cboGrupo.ComboBox.SelectedValue == (int)Funcoes.FGTipoLinha.Eixo)
//                            {
//                                QueueAtiva = "GHE5";
//                            }
//                            else if ((int)cboGrupo.ComboBox.SelectedValue == (int)Funcoes.FGTipoLinha.Motor)
//                            {
//                                QueueAtiva = "GHM3";
//                            }
//                            else if ((int)cboGrupo.ComboBox.SelectedValue == (int)Funcoes.FGTipoLinha.Caminhao)
//                            {
//                                QueueAtiva = "GHV3";
//                            }
//                            else if ((int)cboGrupo.ComboBox.SelectedValue == (int)Funcoes.FGTipoLinha.Onibus)
//                            {
//                                QueueAtiva = "GHV5";
//                            }
//                            else
//                            {
//                                QueueAtiva = QualAmbiente;
//                            }
//                        }
//                        else
//                        {
//                            // Desenvolvimento
//                            if ((int)cboGrupo.ComboBox.SelectedValue == (int)Funcoes.FGTipoLinha.Cabina || (int)cboGrupo.ComboBox.SelectedValue == (int)Funcoes.FGTipoLinha.Onibus || (int)cboGrupo.ComboBox.SelectedValue == (int)Funcoes.FGTipoLinha.Caminhao)
//                            {
//                                QueueAtiva = "GDV1";
//                            }
//                            else if ((int)cboGrupo.ComboBox.SelectedValue == (int)Funcoes.FGTipoLinha.Agregado || (int)cboGrupo.ComboBox.SelectedValue == (int)Funcoes.FGTipoLinha.Eixo || (int)cboGrupo.ComboBox.SelectedValue == (int)Funcoes.FGTipoLinha.Motor || (int)cboGrupo.ComboBox.SelectedValue == (int)Funcoes.FGTipoLinha.Cambio)
//                            {
//                                QueueAtiva = "GDE1";
//                            }
//                            else
//                            {
//                                QueueAtiva = QualAmbiente;
//                            }
//                        }
//                    }

//                }

//                objFG = null;


//            }
//            catch (Exception ex)
//            {	// ErrorHandler:
//                objFG = null;
//                throw ex;
//            }


//            return QueueAtiva;
//        }

//        public static string QueueAtiva(Funcoes.FGTipoLinha TipoLinha, string QualAmbiente)
//        {
//            string QueueAtiva = "";
//            Funcoes objFG;


//            try
//            {	// On Error GoTo ErrorHandler

//                objFG = new Funcoes();



//                //QueueAtiva = Funcoes.LerIni("MainSettings", "QM_PADRAO_CAMINHAO", "JPV1");

//                if (ObterFabrica(QualAmbiente) == Funcoes.Fabrica.JDF)
//                {
//                    // ********************************************************
//                    // ********************FABRICA DE JDF**********************
//                    // ********************************************************
//                    if (Funcoes.Mid(QualAmbiente, 1, 2) == "JP")
//                    {
//                        if (TipoLinha == Funcoes.FGTipoLinha.Caminhao)
//                        {
//                            QueueAtiva = "JPV1";
//                        }
//                        else if (TipoLinha == Funcoes.FGTipoLinha.Cabina)
//                        {
//                            QueueAtiva = "JPV2";
//                        }
//                        else
//                        {
//                            QueueAtiva = "JPV2";
//                        }
//                    }
//                    else
//                    {
//                        // Homologacao
//                        if (Funcoes.Mid(QualAmbiente, 1, 2) == "JH")
//                        {
//                            if (TipoLinha == Funcoes.FGTipoLinha.Caminhao)
//                            {
//                                QueueAtiva = "JHV1";
//                            }
//                            else if (TipoLinha == Funcoes.FGTipoLinha.Cabina)
//                            {
//                                QueueAtiva = "JHV2";
//                            }
//                            else
//                            {
//                                QueueAtiva = "JHV2";
//                            }

//                        }
//                        else
//                        {
//                            // Desenvolvimento
//                            if (TipoLinha == Funcoes.FGTipoLinha.Caminhao)
//                            {
//                                QueueAtiva = "JDV1";
//                            }
//                            else if (TipoLinha == Funcoes.FGTipoLinha.Cabina)
//                            {
//                                QueueAtiva = "JDV2";
//                            }
//                            else
//                            {
//                                QueueAtiva = "JDV2";
//                            }

//                        }
//                    }

//                }
//                else
//                {
//                    // ********************************************************
//                    // ********************FABRICA DE SBC**********************
//                    // ********************************************************
//                    if (Funcoes.Mid(QualAmbiente, 1, 2) == "GP")
//                    {
//                        if (TipoLinha == Funcoes.FGTipoLinha.Cabina)
//                        {

//                            QueueAtiva = "GPV6";
//                        }
//                        else if (TipoLinha == Funcoes.FGTipoLinha.Cambio)
//                        {
//                            QueueAtiva = "GPC2";
//                        }
//                        else if (TipoLinha == Funcoes.FGTipoLinha.Eixo)
//                        {
//                            QueueAtiva = "GPE5";
//                        }
//                        else if (TipoLinha == Funcoes.FGTipoLinha.Motor)
//                        {
//                            QueueAtiva = "GPM3";
//                        }
//                        else if (TipoLinha == Funcoes.FGTipoLinha.Caminhao)
//                        {
//                            QueueAtiva = "GPV1";
//                        }
//                        else if (TipoLinha == Funcoes.FGTipoLinha.Onibus)
//                        {
//                            QueueAtiva = "GPV5";
//                        }
//                        else
//                        {
//                            QueueAtiva = QualAmbiente;
//                        }
//                    }
//                    else
//                    {
//                        // Homologacao
//                        if (Funcoes.Mid(QualAmbiente, 1, 2) == "GH")
//                        {
//                            if (TipoLinha == Funcoes.FGTipoLinha.Cabina)
//                            {

//                                QueueAtiva = "GHV6";

//                            }
//                            else if (TipoLinha == Funcoes.FGTipoLinha.Cambio)
//                            {
//                                QueueAtiva = "GHC2";
//                            }
//                            else if (TipoLinha == Funcoes.FGTipoLinha.Eixo)
//                            {
//                                QueueAtiva = "GHE5";
//                            }
//                            else if (TipoLinha == Funcoes.FGTipoLinha.Motor)
//                            {
//                                QueueAtiva = "GHM3";
//                            }
//                            else if (TipoLinha == Funcoes.FGTipoLinha.Caminhao)
//                            {
//                                QueueAtiva = "GHV1";
//                            }
//                            else if (TipoLinha == Funcoes.FGTipoLinha.Onibus)
//                            {
//                                QueueAtiva = "GHV5";
//                            }
//                            else
//                            {
//                                QueueAtiva = QualAmbiente;
//                            }
//                        }
//                        else
//                        {
//                            // Desenvolvimento
//                            if (TipoLinha == Funcoes.FGTipoLinha.Cabina || TipoLinha == Funcoes.FGTipoLinha.Onibus || TipoLinha == Funcoes.FGTipoLinha.Caminhao)
//                            {
//                                QueueAtiva = "GDV1";
//                            }
//                            else if (TipoLinha == Funcoes.FGTipoLinha.Agregado || TipoLinha == Funcoes.FGTipoLinha.Eixo || TipoLinha == Funcoes.FGTipoLinha.Motor || TipoLinha == Funcoes.FGTipoLinha.Cambio)
//                            {
//                                QueueAtiva = "GDE1";
//                            }
//                        }
//                    }

//                }

//                objFG = null;

//                //return QueueAtiva;

//            }
//            catch (Exception ex)
//            {	// ErrorHandler:
//                objFG = null;
//                throw ex;
//            }


//            return QueueAtiva;
//        }

//        public static string retTIPLIN(Funcoes.FGTipoLinha TipoLinha)
//        {
//            string retTIPLIN = "";
//            if (TipoLinha == Funcoes.FGTipoLinha.Caminhao)
//            {

//                retTIPLIN = "TR";
//            }
//            else if (TipoLinha == Funcoes.FGTipoLinha.Onibus)
//            {
//                retTIPLIN = "ON";
//            }
//            else if (TipoLinha == Funcoes.FGTipoLinha.Agregado)
//            {
//                retTIPLIN = "AG";
//            }
//            else if (TipoLinha == Funcoes.FGTipoLinha.Motor)
//            {
//                retTIPLIN = "AG";
//            }
//            else if (TipoLinha == Funcoes.FGTipoLinha.Cambio)
//            {
//                retTIPLIN = "AG";
//            }
//            else if (TipoLinha == Funcoes.FGTipoLinha.Eixo)
//            {
//                retTIPLIN = "AG";
//            }
//            else if (TipoLinha == Funcoes.FGTipoLinha.Cabina)
//            {
//                retTIPLIN = "AG";
//            }
//            else if (TipoLinha == Funcoes.FGTipoLinha.CabinaDes)
//            {
//                retTIPLIN = "AG";
//                // Desenv quando o parametro pbolDifAgregado for = "true" da function getTipoLinha, usa esses abaixo
//            }
//            else if (TipoLinha == Funcoes.FGTipoLinha.AgregadoDes)
//            {
//                retTIPLIN = "AG";
//            }
//            else if (TipoLinha == Funcoes.FGTipoLinha.OnibusDes)
//            {
//                retTIPLIN = "ON";
//            }
//            else if (TipoLinha == Funcoes.FGTipoLinha.CabinaDes)
//            {
//                retTIPLIN = "AG";
//            }
//            else if (TipoLinha == Funcoes.FGTipoLinha.CaminhaoDes)
//            {
//                retTIPLIN = "TR";
//            }
//            return retTIPLIN;
//        }

//        public static string retGRUSEQ(Funcoes.FGTipoLinha TipoLinha)
//        {
//            string retGRUSEQ = "";
//            if (TipoLinha == Funcoes.FGTipoLinha.Caminhao)
//            {

//                retGRUSEQ = "TR";
//            }
//            else if (TipoLinha == Funcoes.FGTipoLinha.Onibus)
//            {
//                retGRUSEQ = "ON";
//            }
//            else if (TipoLinha == Funcoes.FGTipoLinha.Agregado)
//            {
//                retGRUSEQ = "";
//            }
//            else if (TipoLinha == Funcoes.FGTipoLinha.Motor)
//            {
//                retGRUSEQ = "MO";
//            }
//            else if (TipoLinha == Funcoes.FGTipoLinha.Cambio)
//            {
//                retGRUSEQ = "CA";
//            }
//            else if (TipoLinha == Funcoes.FGTipoLinha.Eixo)
//            {
//                retGRUSEQ = "EI";
//            }
//            else if (TipoLinha == Funcoes.FGTipoLinha.Cabina)
//            {
//                retGRUSEQ = "CB";
//            }
//            return retGRUSEQ;
//        }

//        public static string ObterGrupo_PorAmbiente(string Ambiente)
//        {
//            string Retorno = "";

//            switch (Ambiente.Substring(0, 1)) //Fabrica
//            {

//                case "G": //São Bernardo e Campinas

//                    switch (Ambiente.Substring(2, 1)) //Tipo
//                    {
//                        case "C":
//                            Retorno = "CA";
//                            break;

//                        case "M":
//                            Retorno = "MO";
//                            break;

//                        case "E":
//                            Retorno = "EI";
//                            break;

//                        case "V":
//                            switch (Ambiente.Substring(2, 2)) //Tipo
//                            {
//                                case "V1":
//                                case "V2":
//                                case "V3":
//                                    Retorno = "TR";
//                                    break;

//                                case "V4":
//                                case "V5":
//                                    Retorno = "ON";
//                                    break;

//                                case "V6":
//                                case "V7":
//                                    Retorno = "CB";
//                                    break;

//                            }
//                            break;
//                    }
//                    break;

//                case "J":

//                    switch (Ambiente.Substring(2, 1)) //Tipo
//                    {
//                        case "C":
//                            Retorno = "CA";
//                            break;

//                        case "M":
//                            Retorno = "MO";
//                            break;

//                        case "E":
//                            Retorno = "EI";
//                            break;

//                        case "V":
//                            switch (Ambiente.Substring(2, 2)) //Tipo
//                            {
//                                case "V1":
//                                    Retorno = "TR";
//                                    break;

//                                case "V2":
//                                    Retorno = "CB";
//                                    break;

//                            }
//                            break;
//                    }
//                    break;
//            }


//            return Retorno;

//        }

//        public static string ObterAmbienteParaAcessoTokenApi(string Fabrica, string Ambiente)
//        {
//            string Retorno = "";

//            switch (Fabrica + "_" + Ambiente)
//            {
//                case "SBC_D":
//                    Retorno = "GDV1";
//                    break;

//                case "SBC_H":
//                    Retorno = "GHV1";
//                    break;

//                case "SBC_P":
//                    Retorno = "GPV1";
//                    break;

//                case "JDF_D":
//                    Retorno = "JDV1";
//                    break;

//                case "JDF_H":
//                    Retorno = "JHV1";
//                    break;

//                case "JDF_P":
//                    Retorno = "JPV1";
//                    break;
//            }

//            return Retorno;
//        }

//        public static string ObterAmbiente_Api(string Fabrica, string Ambiente)
//        {
//            string Retorno = "";

//            switch (Fabrica + "_" + Ambiente)
//            {
//                case "SBC_D":
//                    Retorno = "GD";
//                    break;

//                case "SBC_H":
//                    Retorno = "GH";
//                    break;

//                case "SBC_P":
//                    Retorno = "GP";
//                    break;

//                case "JDF_D":
//                    Retorno = "JD";
//                    break;

//                case "JDF_H":
//                    Retorno = "JH";
//                    break;

//                case "JDF_P":
//                    Retorno = "JP";
//                    break;
//            }

//            return Retorno;
//        }

//        public static string ObterAmbiente_PorGrupo(string Ambiente, string Grupo)
//        {
//            string Retorno = "";

//            switch (Grupo)
//            {
//                case "TR":
//                    Retorno = ObterBaseCaminhao(Ambiente);
//                    break;

//                case "ON":
//                    Retorno = ObterBaseOnibus(Ambiente);
//                    break;

//                case "MO":
//                    Retorno = ObterBaseMotor(Ambiente);
//                    break;

//                case "EI":
//                    Retorno = ObterBaseEixo(Ambiente);
//                    break;

//                case "CA":
//                    Retorno = ObterBaseCambio(Ambiente);
//                    break;

//                case "CB":
//                    Retorno = ObterBaseCabina(Ambiente);
//                    break;
//            }

//            return Retorno;
//        }

//        public static string[] ObterTodosQMPRM(string Ambiente)
//        {
//            string[] QMs = new string[14];
//            string[] QMsNovo;
//            string[] QMsTotal = new string[1];
//            string QuebraQM = "XXX";
//            Int32 iOcc = 0;
//            DataSet dtQueues;

//            QMs[0] = Funcoes.ObterBaseCabina("G" + Ambiente);
//            QMs[1] = Funcoes.ObterBaseEixo("G" + Ambiente);
//            QMs[2] = Funcoes.ObterBaseMotor("G" + Ambiente);
//            QMs[3] = Funcoes.ObterBaseOnibus("G" + Ambiente);
//            QMs[4] = Funcoes.ObterBaseVeiculo("G" + Ambiente);
//            QMs[5] = Funcoes.ObterBaseCambio("G" + Ambiente);
//            QMs[6] = Funcoes.ObterBaseCaminhao("G" + Ambiente);
//            QMs[7] = Funcoes.ObterBaseCabina("J" + Ambiente);
//            QMs[8] = Funcoes.ObterBaseEixo("J" + Ambiente);
//            QMs[9] = Funcoes.ObterBaseMotor("J" + Ambiente);
//            QMs[10] = Funcoes.ObterBaseOnibus("J" + Ambiente);
//            QMs[11] = Funcoes.ObterBaseVeiculo("J" + Ambiente);
//            QMs[12] = Funcoes.ObterBaseCambio("J" + Ambiente);
//            QMs[13] = Funcoes.ObterBaseCaminhao("J" + Ambiente);

//            Array.Sort(QMs);
//            QMsNovo = QMs.Distinct().ToArray();

//            foreach (string QM in QMsNovo)
//            {
//                if (QuebraQM.Substring(0, 3) != QM.Substring(0, 3))
//                {
//                    dtQueues = new PerfilUsuario(Ambiente).ObterLinhasQueues(QM);

//                    foreach (DataRow dr in dtQueues.Tables[0].Rows)
//                    {
//                        if (dr["NOQUEU"].ToString().Length > 3)
//                        {
//                            iOcc++;
//                            Array.Resize(ref QMsTotal, iOcc);
//                            QMsTotal[iOcc - 1] = dr["NOQUEU"].ToString();
//                        }
//                    }

//                    QuebraQM = QM.Substring(0, 3);
//                }
//            }

//            Array.Sort(QMsTotal);
//            return QMsTotal.Distinct().ToArray();
//        }

//        public static List<ComboSequenciaInfo> SetarLinhas(GruposInfo Grupo, string Ambiente, string NomeProduto)
//        {
//            ToolStripComboBox cboLinhaMontagem = new ToolStripComboBox();
//            Negocio.Perfil.PerfilUsuario Perfil = new Negocio.Perfil.PerfilUsuario(Ambiente);
//            List<ComboSequenciaInfo> lstAux = new List<ComboSequenciaInfo>();

//            try
//            {
//                Perfil.ObterSequenciasMontagem(ref cboLinhaMontagem, Main.TipoLinha, Funcoes.TipoModoDeMontagem.Linha, Ambiente);

//                for (int iCount = 0; iCount < cboLinhaMontagem.Items.Count; iCount++)
//                {
//                    if (Negocio.Perfil.PerfilUsuario.PermissaoAcesso(5, false, ((ComboSequenciaInfo)cboLinhaMontagem.Items[iCount]).COSQPR,
//                                                                     ((ComboSequenciaInfo)cboLinhaMontagem.Items[iCount]).NOQUEU, NomeProduto, AmbienteInfo._ChaveUsuario))
//                    {
//                        lstAux.Add((ComboSequenciaInfo)cboLinhaMontagem.Items[iCount]);
//                    }
//                }

//                return lstAux;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//            finally
//            {
//                cboLinhaMontagem = null;
//                Perfil = null;
//            }
//        }

//        public static string ObterAmbiente(List<ComboSequenciaInfo> Linhas, string Colimo)
//        {
//            foreach (ComboSequenciaInfo Linha in Linhas)
//            {
//                if (Linha.COLIMO == Colimo) return Linha.NOQUEU;
//            }

//            return "";
//        }

//        public static string ObterPosicaoCKD(string Ambiente, int COSQPR)
//        {
//            try
//            {
//                string _strPosCKD = string.Empty;

//                string strSQL = "select listagg(SUBSTR(THK.NOVIEW, 5),'|') ";
//                strSQL += " within group(order by NOVIEW) strPosCKD ";
//                strSQL += " from THK WHERE NOVIEW LIKE 'DLE_%' AND THK.MECOLU = " + COSQPR + "";

//                DataSet ds = new Conexao(Ambiente).ExecutaSQL(strSQL);

//                if (ds.Tables[0].Rows.Count > 0)
//                {
//                    _strPosCKD = ds.Tables[0].Rows[0]["STRPOSCKD"].ToString().Trim().Replace(" ", "");
//                }
//                else
//                {
//                    _strPosCKD = "^RE|^RC|^RD";
//                }

//                return _strPosCKD;
//            }
//            catch (Exception ex)
//            {
//                throw new Exception("Não foi possível obter a posição do CKD: " + ex.Message);
//            }
//        }

//        public static string ObterPosicaoCKD(OracleConnection conn, int COSQPR)
//        {
//            DataSet ds = new DataSet();
//            string _strPosCKD = string.Empty;

//            try
//            {
//                string strSQL = "select listagg(SUBSTR(THK.NOVIEW, 5),'|') ";
//                strSQL += " within group(order by NOVIEW) strPosCKD ";
//                strSQL += " from THK WHERE NOVIEW LIKE 'DLE_%' AND THK.MECOLU = " + COSQPR + "";

//                OracleDataAdapter da = new OracleDataAdapter(strSQL, conn);

//                da.Fill(ds);

//                if (ds.Tables[0].Rows.Count > 0)
//                {
//                    _strPosCKD = ds.Tables[0].Rows[0]["STRPOSCKD"].ToString().Trim().Replace(" ", "");
//                }
//                else
//                {
//                    _strPosCKD = "^RE|^RC|^RD";
//                }
//            }
//            catch (Exception ex)
//            {
//                throw new Exception("Não foi possível obter a posição do CKD: " + ex.Message);
//            }

//            return _strPosCKD;
//        }

//        public static string ObterSemanaCorrente(DateTime time)
//        {
//            try
//            {
//                DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);

//                if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
//                {
//                    time = time.AddDays(3);
//                }


//            }
//            catch (Exception) { }

//            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday).ToString();
//        }

//        public static string ObterDadosPostosTerceiros(string Parametro)
//        {
//            try
//            {
//                string ret = "";

//                string strSQL = " SELECT NUPOGR ";
//                strSQL += " FROM THK ";
//                strSQL += " WHERE TIUTIL = '" + AmbienteInfo._TiuTil + "'";

//                if (Parametro == "CodPostoVeiculoTer")
//                {
//                    strSQL += " AND NOVIEW = '" + Parametro.ToUpper() + "'";
//                }

//                if (Parametro == "VerPostoVeiculoTer")
//                {
//                    strSQL += " AND NOVIEW = '" + Parametro.ToUpper() + "'";
//                }

//                if (Parametro == "CodPostoMotorTer")
//                {
//                    strSQL += " AND NOVIEW = '" + Parametro.ToUpper() + "'";
//                }

//                if (Parametro == "VerPostoMotorTer")
//                {
//                    strSQL += " AND NOVIEW = '" + Parametro.ToUpper() + "'";
//                }

//                DataSet ds = new Conexao(AmbienteInfo._Ambiente).ExecutaSQL(strSQL);

//                if (ds.Tables[0].Rows.Count > 0)
//                {
//                    return ret = ds.Tables[0].Rows[0]["NUPOGR"].ToString();
//                }
//                else
//                {
//                    return ret = "";
//                }
//            }
//            catch (Exception ex)
//            {
//                throw new Exception("Não foi possível obter (ObterDadosPostosTerceiros): " + ex.Message);
//            }
//        }

//        public static string ObterFilaApedBPMStar(string Ambiente)
//        {
//            try
//            {
//                string strSQL = "";

//                strSQL += " SELECT TRIM(NOCOLU) || '.' || TRIM(NOCACO) || '.' || TRIM(COPICT) FilaAped";
//                strSQL += " FROM THK WHERE NOVIEW = 'FILA_APED_BPMSTAR' ";

//                DataSet ds = new Conexao(Ambiente).ExecutaSQL(strSQL);

//                if (ds.Tables[0].Rows.Count > 0)
//                {
//                    return ds.Tables[0].Rows[0]["FilaAped"].ToString();
//                }
//                else
//                {
//                    return "";
//                }
//            }
//            catch (Exception)
//            {
//                return "";
//            }
//        }

//        public static string FormataString(string texto, int tamanho, bool numerico)
//        {
//            string resultado = "";

//            if (texto.Length > tamanho)
//                texto = texto.Substring(1, tamanho);

//            if (numerico)
//                resultado = texto.PadLeft(tamanho, '0');
//            else
//                resultado = texto;

//            return resultado;
//        }

//        #endregion

//        #region ... List ...

//        public static List<Control> GetAllControls<T>(Control searchWithin)
//        {

//            Control QualForm = null;

//            if (searchWithin.GetType() == typeof(Form))
//            {
//                Form mdi = (Form)searchWithin;


//                if (mdi.ActiveMdiChild != null)
//                {
//                    QualForm = (Control)mdi.ActiveMdiChild;
//                }

//                else
//                {
//                    QualForm = (Control)mdi;
//                }
//            }
//            else
//            {
//                QualForm = (Control)searchWithin;
//            }


//            List<Control> returnList = new List<Control>();

//            if (QualForm.HasChildren == true)
//            {
//                foreach (Control ctrl in QualForm.Controls)
//                {
//                    if (ctrl is T)
//                    {
//                        if (ctrl.Visible)
//                            returnList.Add(ctrl);
//                    }
//                    returnList.AddRange(GetAllControls<T>(ctrl));
//                }
//            }
//            else if (QualForm.HasChildren == false)
//            {
//                foreach (Control ctrl in QualForm.Controls)
//                {
//                    if (ctrl is T)
//                    {
//                        if (ctrl.Visible)
//                            returnList.Add(ctrl);
//                    }
//                    returnList.AddRange(GetAllControls<T>(ctrl));
//                }
//            }
//            return returnList;

//        }

//        #endregion

//        #region ... Void ...

//        public static void SetarValorListViewItem(ListViewItem lvw, string NomeCampo, string Valor)
//        {
//            if (lvw == null)
//                return;

//            int QualColuna = -1;
//            for (int c = 0; c < lvw.ListView.Columns.Count; c++)
//            {
//                if (lvw.ListView.Columns[c].Name.Trim().ToUpper().Equals(NomeCampo.Trim().ToUpper()))
//                {
//                    QualColuna = c;
//                    break;
//                }
//            }

//            if (QualColuna > -1)
//                lvw.SubItems[QualColuna].Text = Valor;
//        }

//        public static void SetarValorListView(ref ListView lvw, string NomeCampo, string Valor)
//        {
//            if (lvw.SelectedItems == null)
//                return;

//            int QualColuna = -1;
//            for (int c = 0; c < lvw.Columns.Count; c++)
//            {
//                if (lvw.Columns[c].Name.Trim().ToUpper().Equals(NomeCampo.Trim().ToUpper()))
//                {
//                    QualColuna = c;
//                    break;
//                }
//            }

//            if (QualColuna > -1)
//                lvw.FocusedItem.SubItems[QualColuna].Text = Valor;
//        }

//        public static void LimparConfigListview(string Sistema, string FormGridName)
//        {
//            Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("MBBras").OpenSubKey(PastaSistemaRegistry).OpenSubKey(Sistema);

//            key.DeleteSubKey(FormGridName);

//            key.Close();
//        }

//        public static void LimparItensRemovidosListView(ref ListView lvwLista, string PalavraTag)
//        {
//            for (int i = lvwLista.Items.Count - 1; i >= 0; i--)
//            {
//                if (lvwLista.Items[i].Selected)
//                {
//                    if (lvwLista.Items[i].Tag.ToString() == PalavraTag)
//                        lvwLista.Items[i].Remove();
//                }
//            }
//        }

//        public static void RemoverItensSelecionadosListView(ref ListView lvwLista)
//        {
//            for (int i = lvwLista.Items.Count - 1; i >= 0; i--)
//            {
//                if (lvwLista.Items[i].Selected)
//                    lvwLista.Items[i].Remove();
//            }
//        }

//        public static Control LocalizarControleContainer(Control Container, string Valor)
//        {
//            try
//            {
//                foreach (Control ctrlAux in Container.Controls)
//                {
//                    if (Valor == ctrlAux.Text)
//                    {
//                        return ctrlAux;
//                    }
//                }
//            }
//            catch (Exception)
//            {
//                return null;
//            }

//            return null;
//        }

//        public static void LocalizarNp(string NuprodKanban, ref ListView lvwLocalizar)
//        {
//            string sNuprod = "";
//            string sKanban = "";
//            //bool Encontrou = false;

//            for (int i = 0; i < lvwLocalizar.Items.Count; i++)
//            {
//                lvwLocalizar.Items[i].Selected = false;

//                sNuprod = Funcoes.ObterValorListView(lvwLocalizar, "NUPROD", i);
//                if (sNuprod.Contains(NuprodKanban))
//                {
//                    lvwLocalizar.Items[i].Selected = true;
//                    lvwLocalizar.Items[i].Focused = true;
//                    lvwLocalizar.Focus();
//                    //Encontrou = true;
//                    break;
//                }

//                sKanban = Funcoes.ObterValorListView(lvwLocalizar, "NUSEQM", i);
//                if (sKanban.Contains(NuprodKanban))
//                {
//                    lvwLocalizar.Items[i].Selected = true;
//                    lvwLocalizar.Items[i].Focused = true;
//                    lvwLocalizar.Focus();
//                    //Encontrou = true;
//                    break;
//                }
//            }

//            if (lvwLocalizar.SelectedItems.Count <= 0)
//            {
//                Funcoes.MessageBox_Exclamacao("Nenhum NP ou Kanban encontrado");
//            }
//        }

//        public static bool LocalizarTextoLvw(string Texto, ref ListView lvwLocalizar)
//        {
//            return LocalizarTextoLvw(Texto, ref lvwLocalizar, false, false);
//        }

//        public static bool LocalizarTextoLvw(string Texto, ref ListView lvwLocalizar, bool Primeiro)
//        {
//            return LocalizarTextoLvw(Texto, ref lvwLocalizar, Primeiro, false);
//        }

//        public static bool LocalizarTextoLvw(string Texto, ref ListView lvwLocalizar, bool Primeiro, bool ExibeMsg)
//        {
//            bool Encontrou = false;

//            string TextoLvw = "";

//            for (int i = 0; i < lvwLocalizar.Items.Count; i++)
//            {
//                lvwLocalizar.Items[i].Selected = false;


//                for (int c = 0; c < lvwLocalizar.Items[i].SubItems.Count; c++)
//                {
//                    switch (lvwLocalizar.Columns[c].Name.Trim().ToUpper())
//                    {
//                        case "NUPRVI":
//                        case "NUPRGE":
//                        case "NUPRVE":
//                        case "NUPROD":
//                        case "NUSEQM":
//                            TextoLvw = GetOnlyNumbers(lvwLocalizar.Items[i].SubItems[c].Text.Trim().ToUpper());
//                            break;

//                        default:
//                            TextoLvw = lvwLocalizar.Items[i].SubItems[c].Text.Trim().ToUpper();
//                            break;
//                    }

//                    if (TextoLvw.Contains(Texto.ToUpper()) || TextoLvw.Contains(Funcoes.GetOnlyNumbers(Texto).ToUpper()))
//                    {
//                        lvwLocalizar.Items[i].Selected = true;
//                        lvwLocalizar.Items[i].Focused = true;
//                        lvwLocalizar.Items[i].EnsureVisible();
//                        lvwLocalizar.Focus();
//                        Encontrou = true;
//                        if (Primeiro) return true;
//                        break;
//                    }
//                }
//            }

//            if (lvwLocalizar.SelectedItems.Count <= 0 && ExibeMsg)
//                Funcoes.MessageBox_Exclamacao("Nenhum valor encontrado");

//            return Encontrou;
//        }

//        public static void NoWrap_GridView(ref System.Web.UI.WebControls.GridView grid)
//        {
//            for (int i = 0; i < grid.Rows.Count; i++)
//            {
//                for (int c = 0; c < grid.Rows[i].Cells.Count; c++)
//                {
//                    grid.Rows[i].Cells[c].Attributes.Add("nowrap", null);
//                }
//            }
//        }

//        public static void EnviaEmailSistema(string Ambiente, string Sistema, string Assunto, string MensagemEmail, string MensagemErroSistema)
//        {
//            string Destinatarios = EmailDestinatarios(Ambiente, Sistema, Assunto);
//            string _AssuntoBase = Assunto;

//            MensagemEmail = MensagemErroSistema + Environment.NewLine + Environment.NewLine + MensagemEmail;
//            MensagemEmail += Environment.NewLine + Environment.NewLine;
//            MensagemEmail += "==> Usuario Logado: " + AmbienteInfo._NomeCompletoUsuario + " [" + AmbienteInfo._ChaveUsuario + "]";
//            Assunto += "(" + Ambiente + ")";

//            if (Destinatarios == "")
//            {
//                MensagemEmail += Environment.NewLine +
//                    "==> Não há destinatários para o Sistema [" + Sistema + "]" +
//                    " assunto [" + _AssuntoBase + "]" + Environment.NewLine +
//                    "==> Verificar cadastro THK. (Select * from THK where NOVIEW LIKE 'GER_EMAIL_%')";

//                EnviaEmailAdms(Ambiente, Sistema, Assunto, MensagemEmail, MensagemErroSistema);
//            }
//            else if (Destinatarios != "NAO_ATIVO")
//            {
//                Email.EnviarEmail(Sistema.ToLower() + "@t-systems.com.br", Sistema, Destinatarios, Assunto, MensagemEmail);
//            }
//        }

//        public static void EnviaEmailAdms(string Ambiente, string Sistema, string Assunto, string MensagemEmail, string MensagemErroSistema)
//        {
//            string Destinatarios = EmailDestinatarios(Ambiente, "ADM_PRM", "ADMINISTRADORES");

//            try
//            {
//                if (Destinatarios.Trim() != "NAO_ATIVO" && Destinatarios.Trim() != "")
//                {
//                    Email.EnviarEmail(Sistema.ToLower() + "@t-systems.com.br", Sistema,
//                        Destinatarios, Assunto, string.Concat(MensagemEmail, Environment.NewLine,
//                        "==> Exception: ", MensagemErroSistema));
//                }
//                else
//                if (Destinatarios.Trim() == "")
//                {
//                    string NOCOLU = "ADM_PRM";
//                    string NOVIEW = String.Concat("GER_EMAIL_", "ADMINISTRADORES");

//                    MensagemErroSistema += Environment.NewLine + "Chave THK NOCOLU>" + NOCOLU + " NOVIEW>" + NOVIEW;
//                    Destinatarios = "narcizo.nobrega@t-systems.com.br";

//                    Email.EnviarEmail(Sistema.ToLower() + "@t-systems.com.br", Sistema,
//                        Destinatarios, Assunto, string.Concat(MensagemEmail, Environment.NewLine,
//                        "==> Exception: ", MensagemErroSistema));
//                }
//            }
//            catch
//            {
//            }
//        }

//        public static string EmailDestinatarios(string Ambiente, string Sistema, string Assunto)
//        {
//            string Sql = "";
//            string NOVIEW = "";
//            string NOCOLU = UFLimitarString(Assunto.ToUpper().Trim(), 30);

//            NOVIEW = UFLimitarString(String.Concat("GER_EMAIL_", Sistema.ToUpper().Trim()), 30);

//            Sql += " SELECT TRIM(NOCACO) || TRIM(COPICT) AS EMAIL , COSTAT";
//            Sql += " FROM THK ";
//            Sql += " WHERE TIUTIL = ' ' ";
//            Sql += "   AND NOVIEW = '" + NOVIEW + "'";
//            Sql += "   AND NOCOLU = '" + NOCOLU + "'";

//            try
//            {
//                DataSet dsMail = new Conexao(Ambiente).ExecutaSQL(Sql);

//                string Destinatarios = "";

//                foreach (DataRow dr in dsMail.Tables[0].Rows)
//                {
//                    if (dr["COSTAT"].ToString().Trim() == "S")
//                    {
//                        if (Destinatarios == "NAO_ATIVO")
//                        {
//                            Destinatarios = string.Concat(dr["Email"].ToString(), ";");
//                        }
//                        else
//                        {
//                            Destinatarios += string.Concat(dr["Email"].ToString(), ";");
//                        }
//                    }
//                    else
//                    {
//                        if (Destinatarios == "") Destinatarios = "NAO_ATIVO";
//                    }
//                }

//                return Destinatarios;
//            }
//            catch (Exception ex)
//            {
//                GravaLog("Erro Email [" + Ambiente + "]", "ErroEnvioEmail.txt", ex.Message, ex.StackTrace, "EmailDestinatarios",
//                    System.AppDomain.CurrentDomain.BaseDirectory.ToString());
//                return "";
//            }
//        }

//        public static void UFGotFocus(TextBox Control)
//        {
//            Control.SelectionStart = 0;
//            Control.SelectionLength = Control.Text.Trim().Length;
//            if (Control.Text.Trim() == "")
//                Control.Text = "";
//        }

//        public static void UFSendKeysTAB(ref TextBox objeto)
//        {
//            if (objeto.Text.Length == objeto.MaxLength)
//                SendKeys.Send("{TAB}");
//        }

//        public static Icon UFIcone(Bitmap Figura)
//        {
//            IntPtr Hicon;

//            try
//            {
//                Hicon = Figura.GetHicon();
//                return Icon.FromHandle(Hicon);
//            }
//            catch (Exception)
//            {
//                Hicon = PRMLibrary.Properties.Resources.error.GetHicon();
//                return null;
//            }
//        }

//        public static void UFCursor(bool Status = false)
//        {
//            if (Status)
//            {
//                Application.DoEvents();
//                Cursor.Current = Cursors.WaitCursor;
//            }
//            else
//            {
//                Cursor.Current = Cursors.Default;
//            }
//        }

//        public static void GravaLog(Exception objErro, string sNomeArquivo, string sMensagem)
//        {
//            GravaLog(objErro.Message, sNomeArquivo, sMensagem, objErro.StackTrace, objErro.Source);
//        }

//        public static void GravaLog(string _Message, string sNomeArquivo, string sMensagem, string _StackTrace, string _Source)
//        {
//            string sPath = "";
//            System.IO.StreamWriter file = null;

//            try
//            {
//                sPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\MBBras\\PRM\\Log\\";

//                if (!System.IO.Directory.Exists(sPath))
//                {
//                    System.IO.Directory.CreateDirectory(sPath);
//                }

//                file = new System.IO.StreamWriter(sPath + sNomeArquivo, true);

//                file.WriteLine("Data: " + (DateTime.Now).ToString("dd-MM-yyyy"));
//                file.WriteLine("Hora: " + (DateTime.Now).ToString("HH:mm:ss"));
//                file.WriteLine("Erro nº " + _StackTrace);
//                file.WriteLine("Fonte: " + _Source);
//                file.WriteLine("Descrição: " + _Message);
//                file.WriteLine("Dados Adic.: " + sMensagem);
//                file.WriteLine(new String('-', 80));

//                file.Close();
//            }
//            catch (Exception ex)
//            {
//                MessageBox_Erro("Erro na Função de Registro do Log." + " - " + ex + " - " + sMensagem.Trim());
//            }
//        }

//        public static void GravaLog(string _Message, string sNomeArquivo, string sMensagem, string _StackTrace, string _Source, string Caminho)
//        {
//            string sPath = "";
//            System.IO.StreamWriter file = null;

//            try
//            {
//                sPath = Caminho;

//                if (!System.IO.Directory.Exists(sPath))
//                {
//                    System.IO.Directory.CreateDirectory(sPath);
//                }

//                file = new System.IO.StreamWriter(sPath + sNomeArquivo, true);

//                file.WriteLine("Data: " + (DateTime.Now).ToString("dd-MM-yyyy"));
//                file.WriteLine("Hora: " + (DateTime.Now).ToString("HH:mm:ss"));
//                file.WriteLine("Erro nº " + _StackTrace);
//                file.WriteLine("Fonte: " + _Source);
//                file.WriteLine("Descrição: " + _Message);
//                file.WriteLine("Dados Adic.: " + sMensagem);
//                file.WriteLine(new String('-', 80));

//                file.Close();
//            }
//            catch (Exception ex)
//            {
//                MessageBox_Erro("Erro na Função de Registro do Log." + " - " + ex + " - " + sMensagem.Trim());
//            }
//        }

//        public static void GravaLogLinha(string _Message, string sNomeArquivo, string sMensagem, string _StackTrace, string _Source)
//        {
//            string sPath = "";
//            System.IO.StreamWriter file = null;

//            try
//            {
//                sPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\MBBras\\PRM\\Log\\";

//                if (!System.IO.Directory.Exists(sPath))
//                {
//                    System.IO.Directory.CreateDirectory(sPath);
//                }

//                file = new System.IO.StreamWriter(sPath + sNomeArquivo, true);

//                file.WriteLine("Data: " + (DateTime.Now).ToString("dd-MM-yyyy"));
//                file.WriteLine("Hora: " + (DateTime.Now).ToString("HH:mm:ss"));
//                file.WriteLine("Erro nº " + _StackTrace);
//                file.WriteLine("Fonte: " + _Source);
//                file.WriteLine("Descrição: " + _Message);
//                file.WriteLine("Dados Adic.: " + sMensagem);
//                file.WriteLine(new String('-', 80));

//                file.Close();
//            }
//            catch (Exception ex)
//            {
//                MessageBox_Erro("Erro na Função de Registro do Log." + " - " + ex + " - " + sMensagem.Trim());
//            }
//        }

//        public static void GravaLogAped(string sConteudo)
//        {
//            string sPath = "";
//            System.IO.StreamWriter objfile = null;
//            bool OK = false;
//            int Seq = 0;

//            while (!OK)
//            {
//                try
//                {
//                    sPath = Path.GetTempPath() + "MBBras\\PRM\\Log\\";
//                    string sNomeArquivo = DateTime.Now.ToString("yyyyMMdd") + "_Processo" + Seq.ToString("00") + ".csv";

//                    if (!System.IO.Directory.Exists(sPath))
//                    {
//                        System.IO.Directory.CreateDirectory(sPath);
//                    }

//                    if (!File.Exists(sPath + sNomeArquivo))
//                    {
//                        objfile = new System.IO.StreamWriter(sPath + sNomeArquivo, true);
//                        objfile.WriteLine("Data;Funcao;Argumento;Direcao;Retorno;Codigo Operacao;Nr. Tela;Tela;");
//                        objfile.Close();
//                    }

//                    objfile = new System.IO.StreamWriter(sPath + sNomeArquivo, true);
//                    objfile.WriteLine(sConteudo);
//                    objfile.Close();
//                    OK = true;
//                }
//                catch (IOException ex)
//                {
//                    Seq++;
//                    if (Seq > 10) throw ex;
//                }
//                catch (Exception ex)
//                {
//                    throw ex;
//                }
//            }
//        }

//        public static void CarregarLogoEtiq(string PrinterName)
//        {
//            string strPortaSerial = String.Empty;
//            string strArqBat = String.Empty;
//            string strLinhaShell = String.Empty;
//            string strNomeArq = "logoestrela.txt";
//            string base64String = "";
//            string PortName = "";
//            int Porta = 9100;
//            bool Local = false;

//            ManagementObjectCollection moReturn = null;
//            ManagementObjectSearcher moSearch = null;

//            System.Net.Sockets.TcpClient client = null;
//            System.IO.StreamWriter writer = null;
//            System.IO.MemoryStream ms = new System.IO.MemoryStream();

//            try
//            {
//                moSearch = new ManagementObjectSearcher("Select * from Win32_Printer Where Name = '" + PrinterName + "'");
//                moReturn = moSearch.Get();
//                if (moReturn.Count <= 0) return;

//                foreach (ManagementObject mo in moReturn)
//                {
//                    PortName = mo["PortName"].ToString();
//                }

//                if ((PortName.ToUpper().Contains("LPT")) || (PortName.ToUpper().Contains("USB")))
//                    Local = true;

//                if (Local)
//                {
//                    try
//                    {
//                        base64String = "~DGESTRELAH,00480,008,gX07FCR07IFCP03DJF8O0HFHEHFCN01FC040HFN07FH0E01F8M0FCH0EH07EL01FI0EH01FL03CI0EI0F8K078I0EI07CK0FJ0EI03EJ01EJ0EI01FJ03CI01FJ0FJ03CI01FJ078I078I01FJ038I0FJ01FJ03CI0FJ01FJ01CI0EJ01FJ01EH01EJ03F8J0EH01CJ03F8J0FH01CJ03F8J0FH03CJ03F8J07H038J03F8J07H038J02F8J078038J06FCJ078038J02FCJ038038J0EF6J038038I01CB7J038038I03HBF8I038038I067HFCI038038H01CIFEI038038H038JF8H038038H063JFCH078038H0CHF3HFEH07H03C031FC07HFH07H01C067EH01HF807H01C0DF8I07FC0FH01E1FCK0HF0EH01E7FL03F8EI0HF8M07DEI0FCN01FCI078O07CI078O078I03CO078I01EO0FJ01FN01EK0F8M03CK07CM078K03EL01FL01F8K03EM07EK0FCM03F8I03F8N0HF803FEO03DJF8P0FHEFEQ01IFgY0";

//                        string PathAppData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
//                        System.OperatingSystem os = Environment.OSVersion;
//                        Version vs = os.Version;

//                        bool Win7 = false;

//                        switch (vs.Major)
//                        {
//                            case 3:
//                            case 4:
//                            case 5:
//                                Win7 = false;
//                                break;
//                            case 6:
//                                Win7 = true;
//                                break;
//                        }

//                        if (Win7)
//                            PathAppData = string.Concat(PathAppData, @"\MBBRAS\PRM\");
//                        else
//                            PathAppData = string.Concat(PathAppData, @"\DCBR IT\PRM\");

//                        //Grava o arquivo com o comando
//                        Funcoes.EscreveArquivoTexto(base64String, PathAppData, strNomeArq, "OUTPUT");

//                        //Verifica se comando será executado pela serial
//                        if (Funcoes.LerIniPrint("PRMPrint", "IMPR_LOGO_SERIAL", "N") == "S")
//                        {
//                            Lpt1Printer.SendFileToPrinter(PrinterName, string.Concat(PathAppData, strNomeArq));
//                        }
//                    }
//                    catch
//                    {
//                        //quando der erro, ignora, apenas nao exibe a imagem
//                    }
//                }
//                else
//                {
//                    client = new System.Net.Sockets.TcpClient();

//                    try
//                    {
//                        client.Connect(PortName, Porta);

//                        Image img = Properties.Resources.mercedes_benz_symbol_96x96;

//                        img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
//                        byte[] imageBytes = ms.ToArray();

//                        foreach (Byte b in imageBytes)
//                        {
//                            string hexRep = String.Format("{0:X}", b);
//                            if (hexRep.Length == 1)
//                                hexRep = "0" + hexRep;
//                            base64String += hexRep;
//                        }

//                        string zplToSend = "^XA" + "^MNN" + "^LL500" + "~DYE:LOGO,P,P," + imageBytes.Length + ",," + base64String + "^XZ";

//                        writer = new System.IO.StreamWriter(client.GetStream(), System.Text.Encoding.UTF8);
//                        writer.Write(zplToSend);
//                        writer.Flush();
//                        writer.Close();
//                        client.Close();
//                    }
//                    catch
//                    {
//                        //quando der erro, ignora, apenas nao exibe a imagem
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//            finally
//            {
//                if (moSearch != null)
//                {
//                    moSearch.Dispose();
//                    moSearch = null;
//                }

//                if (moReturn != null)
//                {
//                    moReturn.Dispose();
//                    moReturn = null;
//                }

//                if (writer != null)
//                {
//                    writer.Dispose();
//                    writer = null;
//                }

//                client = null;

//                if (ms != null)
//                {
//                    ms.Dispose();
//                    ms.Close();
//                }
//            }
//        }

//        public static void LimparArquivosPasta(string Caminho)
//        {
//            LimparArquivosPasta(Caminho, "*");
//        }

//        public static void LimparArquivosPasta(string Caminho, string TipoArquivo)
//        {
//            DirectoryInfo Dir = new DirectoryInfo(Caminho);

//            if (!Dir.Exists)
//            {
//                Dir.Create();
//            }

//            FileInfo[] Files = Dir.GetFiles(TipoArquivo, SearchOption.AllDirectories);

//            try
//            {
//                foreach (FileInfo File in Files)
//                {
//                    try
//                    {
//                        File.Delete();
//                    }
//                    catch (Exception)
//                    {
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }

//        public static bool LimparLogsPastaDias(DirectoryInfo Pasta, int Dias, string TipoArquivo)
//        {
//            if (!Pasta.Exists) return false;

//            FileInfo[] Files = Pasta.GetFiles(TipoArquivo, SearchOption.AllDirectories);

//            try
//            {
//                foreach (FileInfo File in Files)
//                {
//                    int iDia = DateTime.Now.Subtract(File.LastWriteTime).Days;

//                    if (iDia > Dias - 1)
//                    {
//                        try
//                        {
//                            File.Delete();
//                        }
//                        catch (Exception)
//                        {
//                        }
//                    }
//                }

//                return true;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }

//        public static void EscreveArquivoTexto(string pstrTexto, string pstrPath, string pstrNomeArquivo, string pstrModo)
//        {
//            if (pstrModo == "OUTPUT")
//            {
//                System.IO.StreamWriter file = new StreamWriter(pstrPath + "\\" + pstrNomeArquivo);

//                file.Write(pstrTexto);

//                file.Close();
//            }
//        }

//        public static void UFFormatDataOra(DateTime Data, ref string DataDe, ref string DataAte)
//        {
//            UFFormatDataOra(Data, ref DataDe, ref DataAte, false);
//        }

//        public static void UFFormatDataOra(DateTime Data, ref string DataDe, ref string DataAte, bool CurrentDayOnly)
//        {
//            object UFFormatDataOra = 0;
//            string dtAno = "";
//            string dtMes = "";

//            dtAno = (Data.Year).ToString("0000");
//            dtMes = (Data.Month).ToString("00");

//            switch (Convert.ToInt16(double.Parse(dtMes)))
//            {
//                case 1:
//                    {

//                        DataDe = "01-01-";
//                        DataAte = "31-01-";
//                        break;
//                    }
//                case 2:
//                    {
//                        DataDe = "01-02-";
//                        DataAte = LastDayMonth(2, Convert.ToInt16(double.Parse(dtAno))) + "-02-";
//                        break;
//                    }
//                case 3:
//                    {
//                        DataDe = "01-03-";
//                        DataAte = "31-03-";
//                        break;
//                    }
//                case 4:
//                    {
//                        DataDe = "01-04-";
//                        DataAte = "30-04-";
//                        break;
//                    }
//                case 5:
//                    {
//                        DataDe = "01-05-";
//                        DataAte = "31-05-";
//                        break;
//                    }
//                case 6:
//                    {
//                        DataDe = "01-06-";
//                        DataAte = "30-06-";
//                        break;
//                    }
//                case 7:
//                    {
//                        DataDe = "01-07-";
//                        DataAte = "31-07-";
//                        break;
//                    }
//                case 8:
//                    {
//                        DataDe = "01-08-";
//                        DataAte = "31-08-";
//                        break;
//                    }
//                case 9:
//                    {
//                        DataDe = "01-09-";
//                        DataAte = "30-09-";
//                        break;
//                    }
//                case 10:
//                    {
//                        DataDe = "01-10-";
//                        DataAte = "31-10-";
//                        break;
//                    }
//                case 11:
//                    {
//                        DataDe = "01-11-";
//                        DataAte = "30-11-";
//                        break;
//                    }
//                case 12:
//                    {
//                        DataDe = "01-12-";
//                        DataAte = "31-12-";
//                        break;
//                    }
//            } //end switch

//            if (CurrentDayOnly)
//            {
//                DataDe = Data.Day.ToString("00") + DataDe.Substring(2);
//                DataAte = DataDe + dtAno + " 23:59:59";
//                DataDe = DataDe + dtAno + " 00:00:00";

//                DataDe = "TO_DATE('" + DataDe + "', 'DD-MM-YYYY HH24:MI:SS')";
//                DataAte = "TO_DATE('" + DataAte + "', 'DD-MM-YYYY HH24:MI:SS')";
//            }
//            else
//            {
//                DataDe = DataDe + dtAno + " 00:00:00";
//                DataDe = "TO_DATE('" + DataDe + "', 'DD-MM-YYYY HH24:MI:SS')";

//                DataAte = DataAte + dtAno + " 23:59:59";
//                DataAte = "TO_DATE('" + DataAte + "', 'DD-MM-YYYY HH24:MI:SS')";
//            }

//        }

//        public static void CarregarImagensSistema(ref ImageList iml)
//        {
//            iml.Images.Clear();
//            iml.Images.Add("PRMAPOIO", Properties.Resources.PRMAPOIO);
//            iml.Images.Add("PRMALARME", Properties.Resources.PRMALARME);
//            iml.Images.Add("PRMCHECKCOM", Properties.Resources.PRMCHECKCOM);
//            iml.Images.Add("PRMCTRACCESS", Properties.Resources.PRMCTRACCESS);
//            iml.Images.Add("PRMKANBAN", Properties.Resources.PRMKANBAN);
//            iml.Images.Add("PRMMAPLIN", Properties.Resources.internet_explorer);
//            iml.Images.Add("PRMSEQMON", Properties.Resources.internet_explorer);
//            iml.Images.Add("PRMWEB", Properties.Resources.internet_explorer);
//            iml.Images.Add("PRMMOBILE", Properties.Resources.PRMMOBILE);
//            iml.Images.Add("PRMPLANLIN", Properties.Resources.PRMPLANLIN);
//            iml.Images.Add("PRMPRINT", Properties.Resources.PRMPRINT);
//            iml.Images.Add("PRMSEQ", Properties.Resources.PRMSEQ);
//            iml.Images.Add("PRMSEQGALPAO", Properties.Resources.PRMSeqGalpao);
//            iml.Images.Add("PRMSEQPINTURA", Properties.Resources.PRMSeqPintura);
//            iml.Images.Add("PRMSEQ1044", Properties.Resources.PRMSeqAcopla);
//            iml.Images.Add("PRMSEQACOPLA", Properties.Resources.PRMSeqAcopla);
//            iml.Images.Add("PRMSEQKITEIXO", Properties.Resources.PRMSeqKitEixo);
//            iml.Images.Add("PRMTRAMIT", Properties.Resources.PRMTRAMIT);
//            iml.Images.Add("PRMVARPROD", Properties.Resources.PRMVARPROD);
//            iml.Images.Add("PRMVINC", Properties.Resources.PRMVINC);
//            iml.Images.Add("PRMSEQMFBUS", Properties.Resources.PRMSEQ);
//            iml.Images.Add("PRMSEQQUADROS", Properties.Resources.PRMSeqQuadros);
//            iml.Images.Add("PRMSEQLOTE", Properties.Resources.PRMSeqLote);
//            //TODO : Ajustar o resource da logistica.
//            iml.Images.Add("PRMSEQLOG", Properties.Resources.PRMSEQLOG);
//            iml.Images.Add("PRMSEQPUFFCKD", Properties.Resources.PRMSEQLOG);
//            iml.Images.Add("PRMSEQPUFO500", Properties.Resources.PRMSEQLOG);
//            iml.Images.Add("PRMSEQPUFMANG", Properties.Resources.PRMSEQLOG);
//            iml.Images.Add("PRMSEQELEVZ", Properties.Resources.PRMSEQ);
//            iml.Images.Add("PRMSEQCKD", Properties.Resources.PRMSEQ);
//            iml.Images.Add("PRMSEQCABEC", Properties.Resources.PRMSeqLote);
//            //iml.Images.Add("SGPCLIENT", Properties.Resources.PRMSeqLote);
//            iml.Images.Add("SGPCLIENT", Properties.Resources.SGPClient);
//        }

//        public static void CheckItemMenu(ref ToolStripMenuItem Item)
//        {
//            Item.Checked = !Item.Checked;
//        }

//        public static void ExibirUltimoItemListView(ref ListView lvwOrigem)
//        {
//            lvwOrigem.Items[lvwOrigem.Items.Count - 1].Focused = true;
//            lvwOrigem.Items[lvwOrigem.Items.Count - 1].Selected = true;
//            lvwOrigem.Items[lvwOrigem.Items.Count - 1].EnsureVisible();
//        }

//        public static void OrdenarColunaListView(ListView lvwOrdenar, ref int sortColumn, ColumnClickEventArgs e)
//        {

//            if (lvwOrdenar.Items[lvwOrdenar.Items.Count - 1].Text.Trim().ToLower().Contains("mais registros"))
//            {
//                for (int i = 1; i < lvwOrdenar.Columns.Count; i++)
//                {
//                    lvwOrdenar.Items[lvwOrdenar.Items.Count - 1].SubItems.Add("");
//                }
//            }

//            int _hScrollValue = GetScrollPos(lvwOrdenar.Handle, SBS_HORZ);

//            // Determine whether the column is the same as the last column clicked.
//            if (e.Column != sortColumn)
//            {
//                // Set the sort column to the new column.
//                sortColumn = e.Column;
//                // Set the sort order to ascending by default.
//                lvwOrdenar.Sorting = SortOrder.Ascending;
//            }
//            else
//            {
//                // Determine what the last sort order was and change it.
//                if (lvwOrdenar.Sorting == SortOrder.Ascending)
//                    lvwOrdenar.Sorting = SortOrder.Descending;
//                else
//                    lvwOrdenar.Sorting = SortOrder.Ascending;
//            }

//            // Call the sort method to manually sort.
//            lvwOrdenar.Sort();

//            lvwOrdenar.ListViewItemSorter = new ListViewItemComparer(e.Column, lvwOrdenar.Sorting);


//            LockWindowUpdate(lvwOrdenar.Handle);
//            //Calculate the value the scroll needs to scroll back.
//            int dx = _hScrollValue - GetScrollPos(lvwOrdenar.Handle, SBS_HORZ);
//            //Send the scroll message.
//            bool b = SendMessage(lvwOrdenar.Handle, LVM_SCROLL, dx, 0);
//            LockWindowUpdate(IntPtr.Zero);

//        }

//        public static void OrdenarColunaListView(ref ListView lvwOrdenar, ref int sortColumn, ColumnClickEventArgs e)
//        {

//            if (lvwOrdenar.Items[lvwOrdenar.Items.Count - 1].Text.Trim().ToLower().Contains("mais registros"))
//            {
//                for (int i = 1; i < lvwOrdenar.Columns.Count; i++)
//                {
//                    lvwOrdenar.Items[lvwOrdenar.Items.Count - 1].SubItems.Add("");
//                }
//            }

//            int _hScrollValue = GetScrollPos(lvwOrdenar.Handle, SBS_HORZ);

//            // Determine whether the column is the same as the last column clicked.
//            if (e.Column != sortColumn)
//            {
//                // Set the sort column to the new column.
//                sortColumn = e.Column;
//                // Set the sort order to ascending by default.
//                lvwOrdenar.Sorting = SortOrder.Ascending;
//            }
//            else
//            {
//                // Determine what the last sort order was and change it.
//                if (lvwOrdenar.Sorting == SortOrder.Ascending)
//                    lvwOrdenar.Sorting = SortOrder.Descending;
//                else
//                    lvwOrdenar.Sorting = SortOrder.Ascending;
//            }

//            // Call the sort method to manually sort.
//            lvwOrdenar.Sort();

//            lvwOrdenar.ListViewItemSorter = new ListViewItemComparer(e.Column, lvwOrdenar.Sorting);


//            LockWindowUpdate(lvwOrdenar.Handle);
//            //Calculate the value the scroll needs to scroll back.
//            int dx = _hScrollValue - GetScrollPos(lvwOrdenar.Handle, SBS_HORZ);
//            //Send the scroll message.
//            bool b = SendMessage(lvwOrdenar.Handle, LVM_SCROLL, dx, 0);
//            LockWindowUpdate(IntPtr.Zero);

//        }

//        public static void ExportToExcel(ListView lvwExport, bool PlanilhaVisivel)
//        {
//            ExportToExcel(lvwExport, "", PlanilhaVisivel);
//        }

//        public static void ExportToExcel(ListView lvwExport)
//        {
//            ExportToExcel(lvwExport, "", false);
//        }

//        public static void ExportToExcel(ListView lvwExport, string Nome)
//        {
//            ExportToExcel(lvwExport, Nome, false);
//        }

//        public static void ExportToExcel(ListView lvwExport, string Nome, bool PlanilhaVisivel)
//        {

//            if (lvwExport.Items.Count <= 0)
//            {
//                MessageBox_Exclamacao("Lista vazia. Refaça a pesquisa para exportar os dados");
//                return;
//            }

//            Microsoft.Office.Interop.Excel.Application xla = new Microsoft.Office.Interop.Excel.Application();
//            Microsoft.Office.Interop.Excel.Workbook wb = xla.Workbooks.Add(Microsoft.Office.Interop.Excel.XlSheetType.xlWorksheet);
//            Microsoft.Office.Interop.Excel.Worksheet ws = (Microsoft.Office.Interop.Excel.Worksheet)xla.ActiveSheet;
//            SaveFileDialog sf = new SaveFileDialog();

//            try
//            {
//                //Criando um arquvo
//                if (!Nome.Equals(""))
//                {
//                    Nome = Nome.Trim().Replace(" ", ".");

//                    if (Nome.Length > 30)
//                        Nome = Nome.Substring(0, 30);

//                    xla.ActiveSheet.Name = Nome;

//                    if (!Nome.ToLower().Contains(".xls"))
//                        Nome += ".xls";

//                    sf.FileName = Nome;
//                    sf.Filter = "Excel Files(.xlsx)|*.xlsx|Excel Files(.xls)|*.xls|Excel Files(*.xlsm)|*.xlsm";
//                    if (!(sf.ShowDialog() == DialogResult.OK))
//                        return;
//                }

//                if (Main.BarraDeStatus != null)
//                {
//                    Application.DoEvents();
//                    UFCursor(true);
//                    InformacaoStatusBar("Exportando dados para Excel...");
//                }
//                else
//                    Cursor.Current = Cursors.WaitCursor;

//                int Linha = 1;
//                int Coluna = 1;

//                xla.Visible = PlanilhaVisivel;

//                ws.Columns["a:z"].NumberFormat = "@";

//                for (int i = 0; i < lvwExport.Columns.Count; i++)
//                {
//                    ws.Cells[Linha, i + 1] = lvwExport.Columns[i].Text.ToString();
//                }

//                Linha = 2;

//                foreach (ListViewItem comp in lvwExport.Items)
//                {
//                    ws.Cells[Linha, Coluna] = comp.Text.ToString();

//                    foreach (ListViewItem.ListViewSubItem drv in comp.SubItems)
//                    {
//                        ws.Cells[Linha, Coluna] = drv.Text.ToString();
//                        Coluna++;
//                    }
//                    Coluna = 1;
//                    Linha++;
//                }

//                ws.Columns["a:z"].EntireColumn.AutoFit();

//                if (!Nome.Equals(""))
//                {
//                    xla.DisplayAlerts = false;
//                    wb.SaveAs(sf.FileName);
//                    xla.DisplayAlerts = true;
//                }

//                xla.WindowState = Microsoft.Office.Interop.Excel.XlWindowState.xlNormal;
//                xla.Visible = true;
//                xla.ActiveWindow.Activate();
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
//            }
//            finally
//            {
//                if (Main.BarraDeStatus != null)
//                    InformacaoStatusBar();
//                else
//                    Cursor.Current = Cursors.Default;

//                releaseObject(xla);
//                releaseObject(wb);
//                releaseObject(ws);

//                xla = null;
//                wb = null;
//                ws = null;
//                sf = null;
//            }
//        }

//        public static void ExportToExcel(DataGridView lvwExport)
//        {
//            ExportToExcel(lvwExport, "");
//        }

//        public static void ExportToExcel(DataGridView lvwExport, string Nome)
//        {

//            if (lvwExport.Rows.Count <= 0)
//            {
//                MessageBox_Exclamacao("Lista vazia. Refaça a pesquisa para exportar os dados");
//                return;
//            }

//            Microsoft.Office.Interop.Excel.Application xla = new Microsoft.Office.Interop.Excel.Application();
//            Microsoft.Office.Interop.Excel.Workbook wb = xla.Workbooks.Add(Microsoft.Office.Interop.Excel.XlSheetType.xlWorksheet);
//            Microsoft.Office.Interop.Excel.Worksheet ws = (Microsoft.Office.Interop.Excel.Worksheet)xla.ActiveSheet;
//            SaveFileDialog sf = new SaveFileDialog();

//            try
//            {
//                //Criando um arquvo
//                if (!Nome.Equals(""))
//                {
//                    xla.ActiveSheet.Name = Nome;

//                    sf.FileName = Nome;
//                    sf.Filter = "Excel Files(.xlsx)|*.xlsx|Excel Files(.xls)|*.xls|Excel Files(*.xlsm)|*.xlsm";
//                    if (!(sf.ShowDialog() == DialogResult.OK))
//                        return;
//                }

//                if (Main.BarraDeStatus != null)
//                    InformacaoStatusBar("Exportando dados para Excel...");
//                else
//                    Cursor.Current = Cursors.WaitCursor;

//                int Linha = 1;
//                //int Coluna = 1;

//                xla.Visible = false;

//                for (int i = 0; i < lvwExport.Columns.Count; i++)
//                {
//                    ws.Cells[Linha, i + 1] = lvwExport.Columns[i].HeaderText.ToString();
//                }

//                Linha = 2;
//                //Coluna = 1;

//                for (int iLin = 0; iLin < lvwExport.Rows.Count; iLin++)
//                {
//                    for (int iCol = 0; iCol < lvwExport.Columns.Count; iCol++)
//                    {
//                        if (lvwExport.Rows[iLin].Cells[iCol].Value != null)
//                            ws.Cells[Linha, iCol + 1] = lvwExport.Rows[iLin].Cells[iCol].Value.ToString();
//                        else
//                            ws.Cells[Linha, iCol + 1] = "";
//                    }
//                    Linha++;
//                }

//                ws.Columns["a:z"].EntireColumn.AutoFit();

//                if (!Nome.Equals(""))
//                {
//                    xla.DisplayAlerts = false;
//                    wb.SaveAs(sf.FileName);
//                    xla.DisplayAlerts = true;
//                }

//                xla.WindowState = Microsoft.Office.Interop.Excel.XlWindowState.xlNormal;
//                xla.Visible = true;
//                xla.ActiveWindow.Activate();
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
//            }
//            finally
//            {
//                if (Main.BarraDeStatus != null)
//                    InformacaoStatusBar();
//                else
//                    Cursor.Current = Cursors.Default;

//                releaseObject(xla);
//                releaseObject(wb);
//                releaseObject(ws);

//                xla = null;
//                wb = null;
//                ws = null;
//                sf = null;
//            }
//        }

//        public static string ExportToTxt_Web_Tab(DataTable dtExport)
//        {
//            string texto = string.Empty;

//            System.Text.StringBuilder sb = new System.Text.StringBuilder();

//            if (dtExport.Rows.Count > 0)
//            {
//                //if (Spliter.Trim().Equals(""))


//                string Spliter = "\t";

//                string FormatMaisSlipter = string.Concat("{0}", Spliter);

//                //for (int i = 0; i < dtExport.Columns.Count; i++)
//                //{
//                //    sb.Append(string.Format(FormatMaisSlipter, dtExport.Columns[i].ColumnName.ToString()));
//                //}

//                for (int iLin = 0; iLin < dtExport.Rows.Count; iLin++)
//                {

//                    for (int iCol = 0; iCol < dtExport.Columns.Count; iCol++)
//                    {
//                        if (dtExport.Rows[iLin][iCol].ToString().Trim() != "")
//                            sb.Append(string.Format(FormatMaisSlipter, dtExport.Rows[iLin][iCol].ToString()));
//                        else
//                            sb.Append(string.Format(FormatMaisSlipter, ""));
//                    }

//                    sb.AppendLine();
//                }

//            }

//            texto = sb.ToString();

//            return texto;
//        }

//        public static string ExportToTxt_Web_Tab(DataSet dtExport)
//        {
//            string texto = string.Empty;

//            System.Text.StringBuilder sb = new System.Text.StringBuilder();

//            if (dtExport.Tables[0].Rows.Count > 0)
//            {
//                //if (Spliter.Trim().Equals(""))


//                string Spliter = "\t";

//                string FormatMaisSlipter = string.Concat("{0}", Spliter);

//                //for (int i = 0; i < dtExport.Tables[0].Columns.Count; i++)
//                //{
//                //    sb.Append(string.Format(FormatMaisSlipter, dtExport.Tables[0].Columns[i].ColumnName.ToString()));
//                //}

//                for (int iLin = 0; iLin < dtExport.Tables[0].Rows.Count; iLin++)
//                {

//                    for (int iCol = 0; iCol < dtExport.Tables[0].Columns.Count; iCol++)
//                    {
//                        if (dtExport.Tables[0].Rows[iLin][iCol].ToString().Trim() != "")
//                            sb.Append(string.Format(FormatMaisSlipter, dtExport.Tables[0].Rows[iLin][iCol].ToString()));
//                        else
//                            sb.Append(string.Format(FormatMaisSlipter, ""));
//                    }

//                    sb.AppendLine();
//                }

//            }

//            texto = sb.ToString();

//            return texto;
//        }

//        public static void ExportToTxt(DataSet lvwExport, string Nome)
//        {
//            ExportToTxt(lvwExport, Nome, "");
//        }

//        public static void ExportToTxt(DataSet lvwExport, string Nome, string Spliter)
//        {
//            SaveFileDialog dlg = new SaveFileDialog();
//            StringBuilder sb = null;
//            StreamWriter sw = null;

//            try
//            {
//                if (lvwExport.Tables[0].Rows.Count > 0)
//                {
//                    if (Spliter.Trim().Equals(""))
//                        Spliter = "\t";

//                    string FormatMaisSlipter = string.Concat("{0}", Spliter);

//                    dlg.FileName = Nome; // Default file name
//                    dlg.DefaultExt = ".txt"; // Default file extension
//                    dlg.Filter = "Text files (.txt)|*.txt"; // Filter files by extension


//                    // Process save file dialog box results
//                    if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
//                    {
//                        // Save document
//                        string filename = dlg.FileName;

//                        sw = new StreamWriter(filename);

//                        sb = new StringBuilder();

//                        for (int i = 0; i < lvwExport.Tables[0].Columns.Count; i++)
//                        {
//                            sb.Append(string.Format(FormatMaisSlipter, lvwExport.Tables[0].Columns[i].ColumnName.ToString()));
//                        }
//                        sw.WriteLine(sb.ToString());

//                        for (int iLin = 0; iLin < lvwExport.Tables[0].Rows.Count; iLin++)
//                        {
//                            sb = new StringBuilder();

//                            for (int iCol = 0; iCol < lvwExport.Tables[0].Columns.Count; iCol++)
//                            {
//                                if (lvwExport.Tables[0].Rows[iLin][iCol].ToString().Trim() != "")
//                                    sb.Append(string.Format(FormatMaisSlipter, lvwExport.Tables[0].Rows[iLin][iCol].ToString()));
//                                else
//                                    sb.Append(string.Format(FormatMaisSlipter, ""));
//                            }

//                            sw.WriteLine(sb.ToString());
//                        }

//                        sw.WriteLine();

//                        sw.Close();
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox_Erro(ex);
//            }
//            finally
//            {
//                dlg = null;
//                sb = null;
//                sw = null;
//            }
//        }

//        public static void ExportToTxt(ListView lvwExport, string Nome)
//        {
//            ExportToTxt(lvwExport, Nome, "");
//        }

//        public static void ExportToTxt(ListView lvwExport, string Nome, string Spliter)
//        {
//            SaveFileDialog dlg = new SaveFileDialog();
//            StringBuilder sb = null;
//            StreamWriter sw = null;

//            try
//            {
//                //if (lvwExport.Items.Count > 0)
//                //{
//                if (Spliter.Trim().Equals(""))
//                    Spliter = "\t";

//                string FormatMaisSlipter = string.Concat("{0}", Spliter);

//                dlg.FileName = Nome; // Default file name
//                dlg.DefaultExt = ".txt"; // Default file extension
//                dlg.Filter = "Text files (.txt)|*.txt"; // Filter files by extension


//                // Process save file dialog box results
//                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
//                {
//                    // Save document
//                    string filename = dlg.FileName;

//                    sw = new StreamWriter(filename);

//                    // the actual data
//                    foreach (ListViewItem lvi in lvwExport.Items)
//                    {
//                        if (lvi.Text.Trim().ToLower() != "mais registros")
//                        {
//                            sb = new StringBuilder();

//                            foreach (ListViewItem.ListViewSubItem listViewSubItem in lvi.SubItems)
//                            {
//                                sb.Append(string.Format(FormatMaisSlipter, listViewSubItem.Text));
//                            }

//                            sw.WriteLine(sb.ToString());
//                        }
//                    }

//                    //sw.WriteLine();

//                    sw.Close();
//                }
//                //}
//            }
//            catch (Exception ex)
//            {
//                MessageBox_Erro(ex);
//            }
//            finally
//            {
//                dlg = null;
//                sb = null;
//                sw = null;
//            }
//        }

//        public static void ExportToTxt(DataTable dtExport, ListView lvwExport, string Nome)
//        {
//            ExportToTxt(dtExport, lvwExport, Nome, "");
//        }

//        public static void ExportToTxt(DataTable dtExport, ListView lvwExport, string Nome, string Spliter)
//        {
//            SaveFileDialog dlg = new SaveFileDialog();
//            StringBuilder sb = null;
//            StreamWriter sw = null;

//            try
//            {
//                if (Spliter.Trim().Equals(""))
//                    Spliter = "\t";

//                string FormatMaisSlipter = string.Concat("{0}", Spliter);

//                dlg.FileName = Nome; // Default file name
//                dlg.DefaultExt = ".txt"; // Default file extension
//                dlg.Filter = "Text files (.txt)|*.txt"; // Filter files by extension

//                // Process save file dialog box results
//                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
//                {
//                    // Save document
//                    string filename = dlg.FileName;

//                    sw = new StreamWriter(filename);

//                    // the actual data
//                    for (int l = 0; l < dtExport.Rows.Count; l++)
//                    {
//                        sb = new StringBuilder();

//                        for (int cc = 0; cc < lvwExport.Columns.Count; cc++)
//                        {
//                            //for (int c = 0; c < dtExport.Columns.Count; c++)
//                            //{
//                            //    if (dtExport.Columns[c].ColumnName == lvwExport.Columns[cc].Name)
//                            //    {
//                            if (dtExport.Columns[lvwExport.Columns[cc].Name] != null)
//                            {
//                                sb.Append(string.Format(FormatMaisSlipter, dtExport.Rows[l][lvwExport.Columns[cc].Name].ToString()));
//                            }
//                            //        break;
//                            //    }
//                            //}
//                        }

//                        sw.WriteLine(sb.ToString());
//                    }

//                    sw.Close();
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox_Erro(ex, true);
//            }
//            finally
//            {
//                dlg = null;
//                sb = null;
//                sw = null;
//            }
//        }

//        private static void releaseObject(object obj)
//        {
//            try
//            {
//                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
//                obj = null;
//            }
//            catch (Exception ex)
//            {
//                obj = null;
//                MessageBox.Show("Unable to release the Object " + ex.ToString());
//            }
//            finally
//            {
//                GC.Collect();
//            }
//        }

//        public static void ListView_ProcurarTexto(ref ListView datagrid, string Texto)
//        {
//            ListView_ProcurarTexto(ref datagrid, Texto, false);
//        }

//        public static void ListView_ProcurarTexto(ref ListView datagrid, string Texto, bool Todos)
//        {
//            ListView_ProcurarTexto(ref datagrid, Texto, Todos, true);
//        }

//        public static void ListView_ProcurarTexto(ref ListView datagrid, string Texto, bool Todos, bool ExibeMSG)
//        {
//            //int linha = 0;
//            //int coluna = 0;
//            bool Encontrou = false;
//            try
//            {
//                for (int lin = 0; lin < datagrid.Items.Count; lin++)
//                {
//                    datagrid.Items[lin].Selected = false;

//                    if (datagrid.Columns.Count > 0)
//                    {
//                        for (int col = 0; col < datagrid.Columns.Count; col++)
//                        {
//                            if (datagrid.Items[lin].SubItems[col].Text.ToLower().Contains(Texto.Trim().ToLower()))
//                            {
//                                datagrid.Items[lin].Selected = true;
//                                datagrid.Items[lin].EnsureVisible();
//                                Encontrou = true;

//                                if (!Todos)
//                                {
//                                    break;
//                                }
//                            }
//                        }

//                        if (!Todos)
//                        {
//                            if (Encontrou)
//                                break;
//                        }
//                    }
//                    else
//                    {
//                        if (datagrid.Items[lin].Text == Texto)
//                        {
//                            datagrid.Items[lin].Selected = true;
//                            datagrid.Items[lin].EnsureVisible();
//                            Encontrou = true;

//                            if (!Todos)
//                            {
//                                break;
//                            }
//                        }
//                    }
//                }
//            }
//            catch (Exception)
//            {
//                Encontrou = false;
//            }

//            if (!Encontrou && ExibeMSG)
//                MessageBox.Show("Nenhum resultado encontrado", "Localizar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
//            else
//                datagrid.Focus();
//        }

//        public static void GridView_ProcurarTexto(ref DataGridView datagrid, string Texto)
//        {
//            int linha = 0;
//            int coluna = 0;
//            bool Encontrou = false;

//            if (!datagrid.CurrentCell.RowIndex.Equals(0) && !datagrid.CurrentCell.ColumnIndex.Equals(0))
//            {
//                linha = datagrid.CurrentCell.RowIndex;
//                coluna = datagrid.CurrentCell.ColumnIndex;
//            }

//            //if (linha.Equals(datagrid.CurrentCell.RowIndex

//            datagrid.CurrentCell.Selected = false;

//            for (linha = 0; linha < datagrid.Rows.Count - 1; linha++)
//            {
//                for (coluna = 0; coluna < datagrid.Rows[linha].Cells.Count; coluna++)
//                {
//                    datagrid.Rows[linha].Cells[coluna].Selected = false;

//                    if (datagrid.Rows[linha].Cells[coluna].Visible)
//                    {
//                        if (datagrid.Rows[linha].Cells[coluna].Value.ToString().Trim().ToLower().Contains(Texto.ToLower().Trim()))
//                        {
//                            datagrid.Rows[linha].Cells[coluna].Selected = true;
//                            Encontrou = true;
//                            break;
//                        }
//                    }
//                }
//            }

//            if (!Encontrou)
//                MessageBox.Show("Nenhum resultado encontrado", "Localizar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
//        }

//        public static void ConfigGridView_Gravar(string Sistema, DataGridView dataGridView1)
//        {
//            ConfigGridView_Gravar(Sistema, dataGridView1, false, "");
//        }

//        public static void ConfigGridView_Gravar(string Sistema, DataGridView dataGridView1, bool PrimeiraGravacao)
//        {
//            ConfigGridView_Gravar(Sistema, dataGridView1, PrimeiraGravacao, "");
//        }

//        public static void ConfigGridView_Gravar(string Sistema, DataGridView dataGridView1, bool PrimeiraGravacao, string DiferenciadorGrid)
//        {
//            if (dataGridView1.Columns.Count > 0)
//            {
//                string sKey = GetAllTreeParentName(dataGridView1) + DiferenciadorGrid;
//                Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("Software").CreateSubKey("MBBras").CreateSubKey(PastaSistemaRegistry).CreateSubKey(Sistema).CreateSubKey(sKey);

//                //int Contador = 0;
//                for (int i = 0; i < dataGridView1.Columns.Count; i++)
//                {
//                    //if (dataGridView1.Columns[i].Name != "")
//                    {
//                        key.CreateSubKey(dataGridView1.Columns[i].Name.ToUpper()).SetValue("Titulo_Coluna", dataGridView1.Columns[i].HeaderText.ToString());
//                        key.CreateSubKey(dataGridView1.Columns[i].Name.ToUpper()).SetValue("Tamanho_Coluna", dataGridView1.Columns[i].Width.ToString());

//                        if (PrimeiraGravacao)
//                        {
//                            key.CreateSubKey(dataGridView1.Columns[i].Name.ToUpper()).SetValue("Status_Coluna", "S");
//                            key.CreateSubKey(dataGridView1.Columns[i].Name.ToUpper()).SetValue("Posicao_Coluna", i.ToString());
//                        }
//                        else
//                        {
//                            key.CreateSubKey(dataGridView1.Columns[i].Name.ToUpper()).SetValue("Posicao_Coluna", dataGridView1.Columns[i].DisplayIndex.ToString());
//                        }
//                    }
//                }

//                key.Close();
//            }
//        }

//        public static void ConfigListView_Gravar(string Sistema, ListView dataGridView1, bool PrimeiraGravacao)
//        {
//            ConfigListView_Gravar(Sistema, dataGridView1, PrimeiraGravacao, string.Empty);
//        }

//        public static void ConfigListView_Gravar(string Sistema, ListView dataGridView1, bool PrimeiraGravacao, string DiferenciadorListView)
//        {
//            if (dataGridView1.Columns.Count > 0)
//            {
//                //if (dataGridView1.Items.Count > 0)
//                //{
//                string sKey = GetAllTreeParentName(dataGridView1) + DiferenciadorListView;

//                Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("Software").CreateSubKey("MBBras").CreateSubKey(PastaSistemaRegistry).CreateSubKey(Sistema).CreateSubKey(sKey);//dataGridView1.FindForm().Name + "." + dataGridView1.Name);

//                for (int i = 0; i < dataGridView1.Columns.Count; i++)
//                {
//                    //if (dataGridView1.Columns[i].Tag == null)
//                    //{
//                    //    key.CreateSubKey(dataGridView1.Columns[i].Name).SetValue("Titulo_Coluna", dataGridView1.Columns[i].Text.ToString());
//                    //}
//                    //else
//                    //{
//                    //    if (dataGridView1.Columns[i].Tag.Equals(""))
//                    //        key.CreateSubKey(dataGridView1.Columns[i].Name).SetValue("Titulo_Coluna", dataGridView1.Columns[i].Text.ToString());
//                    //    else
//                    //        key.CreateSubKey(dataGridView1.Columns[i].Name).SetValue("Titulo_Coluna", dataGridView1.Columns[i].Tag.ToString());
//                    //}

//                    key.CreateSubKey(dataGridView1.Columns[i].Name.ToUpper()).SetValue("Tamanho_Coluna", dataGridView1.Columns[i].Width.ToString());
//                    key.CreateSubKey(dataGridView1.Columns[i].Name.ToUpper()).SetValue("Posicao_Coluna", dataGridView1.Columns[i].DisplayIndex);

//                    if (PrimeiraGravacao)
//                    {
//                        key.CreateSubKey(dataGridView1.Columns[i].Name.ToUpper()).SetValue("Titulo_Coluna", dataGridView1.Columns[i].Text.ToString());
//                        key.CreateSubKey(dataGridView1.Columns[i].Name.ToUpper()).SetValue("Status_Coluna", "S");
//                    }

//                }

//                key.Close();
//                //}
//            }
//        }

//        public static void ConfigGridView_ColunasGravar(string Sistema, string FormGridName, ref ListView ListaExibe, ref ListView ListaNaoExibe)
//        {
//            Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("Software").CreateSubKey("MBBras").CreateSubKey(PastaSistemaRegistry).CreateSubKey(Sistema).CreateSubKey(FormGridName);

//            //int NovaPosicao = 0;

//            for (int i = 0; i < ListaExibe.Items.Count; i++)
//            {
//                key.CreateSubKey(ListaExibe.Items[i].Tag.ToString()).SetValue("Status_Coluna", "S");
//                key.CreateSubKey(ListaExibe.Items[i].Tag.ToString()).SetValue("Posicao_Coluna", i.ToString());
//            }

//            for (int i = 0; i < ListaNaoExibe.Items.Count; i++)
//            {
//                key.CreateSubKey(ListaNaoExibe.Items[i].Tag.ToString()).SetValue("Status_Coluna", "N");
//                key.CreateSubKey(ListaNaoExibe.Items[i].Tag.ToString()).SetValue("Posicao_Coluna", "999");
//            }

//            key.Close();
//        }

//        public static void ConfigGridView_Restaurar(string Sistema, string FormGridName)
//        {
//            try
//            {
//                Microsoft.Win32.Registry.CurrentUser.CreateSubKey("Software").CreateSubKey("MBBras").CreateSubKey(PastaSistemaRegistry).CreateSubKey(Sistema).DeleteSubKeyTree(FormGridName);
//            }
//            catch
//            {
//            }
//        }

//        public static void ConfigGridView_Atribuir(string Sistema, ref DataGridView dataGridView1, string NomeView, string QualAmbiente)
//        {
//            try
//            {
//                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

//                string sKey = GetAllTreeParentName(dataGridView1);
//                string[] colunas = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("MBBras").OpenSubKey(PastaSistemaRegistry).OpenSubKey(Sistema).OpenSubKey(sKey).GetSubKeyNames();

//                if (colunas.Length != dataGridView1.Columns.Count)
//                    ConfigGridView_Oracle(ref dataGridView1, NomeView, QualAmbiente);

//                for (int i = 0; i < colunas.Length; i++)
//                {
//                    string Header = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("MBBras").OpenSubKey(PastaSistemaRegistry).OpenSubKey(Sistema).OpenSubKey(sKey).OpenSubKey(colunas[i]).GetValue("Titulo_Coluna").ToString();
//                    string Tamanho = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("MBBras").OpenSubKey(PastaSistemaRegistry).OpenSubKey(Sistema).OpenSubKey(sKey).OpenSubKey(colunas[i]).GetValue("Tamanho_Coluna").ToString();
//                    string Status = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("MBBras").OpenSubKey(PastaSistemaRegistry).OpenSubKey(Sistema).OpenSubKey(sKey).OpenSubKey(colunas[i]).GetValue("Status_Coluna").ToString();

//                    if ((colunas[i] == "DAINCL") || (colunas[i] == "DAATUA") || (colunas[i] == "DAPLPR"))
//                    {
//                        dataGridView1.Columns[colunas[i]].ValueType = typeof(System.DateTime);
//                        dataGridView1.Columns[colunas[i]].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm:ss";
//                    }

//                    dataGridView1.Columns[colunas[i]].HeaderText = Header;
//                    dataGridView1.Columns[colunas[i]].Width = int.Parse(Tamanho);
//                    dataGridView1.Columns[colunas[i]].Visible = Status.Equals("S");
//                }
//            }
//            catch
//            {
//                ConfigGridView_Oracle(ref dataGridView1, NomeView, QualAmbiente);
//                ConfigGridView_Gravar(Sistema, dataGridView1, true);
//            }
//        }

//        public static void ConfigGridView_Atribuir(string Sistema, ref DataGridView dataGridView1, string DiferenciadorGrid)
//        {
//            try
//            {
//                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

//                int ColFrozen = 0;
//                for (int c = 0; c < dataGridView1.Columns.Count; c++)
//                {
//                    if (dataGridView1.Columns[c].Frozen)
//                        ColFrozen = c + 1;
//                    else
//                        break;
//                }


//                string sKey = GetAllTreeParentName(dataGridView1) + DiferenciadorGrid;
//                string[] colunas = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("MBBras").OpenSubKey(PastaSistemaRegistry).OpenSubKey(Sistema).OpenSubKey(sKey).GetSubKeyNames();

//                int Contador = 0;

//                List<ColunasGridInfo> listCol = new List<ColunasGridInfo>();

//                for (int i = 0; i < colunas.Length; i++)
//                {
//                    string Header = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("MBBras").OpenSubKey(PastaSistemaRegistry).OpenSubKey(Sistema).OpenSubKey(sKey).OpenSubKey(colunas[i]).GetValue("Titulo_Coluna").ToString();
//                    string Tamanho = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("MBBras").OpenSubKey(PastaSistemaRegistry).OpenSubKey(Sistema).OpenSubKey(sKey).OpenSubKey(colunas[i]).GetValue("Tamanho_Coluna").ToString();
//                    string Status = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("MBBras").OpenSubKey(PastaSistemaRegistry).OpenSubKey(Sistema).OpenSubKey(sKey).OpenSubKey(colunas[i]).GetValue("Status_Coluna").ToString();
//                    string Posicao = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("MBBras").OpenSubKey(PastaSistemaRegistry).OpenSubKey(Sistema).OpenSubKey(sKey).OpenSubKey(colunas[i]).GetValue("Posicao_Coluna").ToString();

//                    if (Posicao == "")
//                        Posicao = i.ToString();

//                    listCol.Add(new ColunasGridInfo(colunas[i], int.Parse(Posicao), Status, int.Parse(Tamanho), Header));
//                }

//                IEnumerable<ColunasGridInfo> query = listCol.OrderBy(coluna => coluna.Posicao);
//                foreach (ColunasGridInfo coluna in query)
//                {
//                    dataGridView1.Columns[coluna.Nome].HeaderText = coluna.Titulo;
//                    dataGridView1.Columns[coluna.Nome].Width = coluna.Tamanho;
//                    dataGridView1.Columns[coluna.Nome].DisplayIndex = (Contador + ColFrozen);
//                    dataGridView1.Columns[coluna.Nome].Visible = coluna.Status.Equals("S");

//                    Contador++;
//                }

//            }
//            catch
//            {
//                ConfigGridView_Gravar(Sistema, dataGridView1, true, DiferenciadorGrid);
//            }
//        }

//        public static void ConfigListView_Atribuir(string Sistema, ref ListView dataGridView1)
//        {
//            ConfigListView_Atribuir(Sistema, ref dataGridView1, string.Empty);
//        }

//        public static void ConfigListView_Atribuir(string Sistema, ref ListView dataGridView1, string DiferenciadorListView)
//        {
//            try
//            {
//                //if (dataGridView1.Items.Count <= 0) return;

//                string sKey = GetAllTreeParentName(dataGridView1) + DiferenciadorListView;//dataGridView1.FindForm().Name + "." + dataGridView1.Name

//                string[] colunas = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("MBBras").OpenSubKey(PastaSistemaRegistry).OpenSubKey(Sistema).OpenSubKey(sKey).GetSubKeyNames();

//                if (colunas.Length == dataGridView1.Columns.Count)
//                {
//                    for (int i = 0; i < colunas.Length; i++)
//                    {
//                        string Header = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("MBBras").OpenSubKey(PastaSistemaRegistry).OpenSubKey(Sistema).OpenSubKey(sKey).OpenSubKey(colunas[i]).GetValue("Titulo_Coluna").ToString();
//                        string Tamanho = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("MBBras").OpenSubKey(PastaSistemaRegistry).OpenSubKey(Sistema).OpenSubKey(sKey).OpenSubKey(colunas[i]).GetValue("Tamanho_Coluna").ToString();
//                        string Status = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("MBBras").OpenSubKey(PastaSistemaRegistry).OpenSubKey(Sistema).OpenSubKey(sKey).OpenSubKey(colunas[i]).GetValue("Status_Coluna").ToString();
//                        string Index = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("MBBras").OpenSubKey(PastaSistemaRegistry).OpenSubKey(Sistema).OpenSubKey(sKey).OpenSubKey(colunas[i]).GetValue("Posicao_Coluna").ToString();

//                        dataGridView1.Columns[colunas[i]].Text = Header;
//                        dataGridView1.Columns[colunas[i]].Width = int.Parse(Tamanho);
//                        if (Status.Equals("N"))
//                            dataGridView1.Columns[colunas[i]].Width = 0;
//                        else
//                        {
//                            dataGridView1.Columns[colunas[i]].DisplayIndex = int.Parse(Index);

//                            if (Tamanho.Equals("0"))
//                                dataGridView1.Columns[colunas[i]].Width = 100;
//                        }
//                    }
//                    dataGridView1.Refresh();
//                }
//                else
//                {
//                    ConfigGridView_Restaurar(Sistema, sKey);
//                    AjustarColunasListView(ref dataGridView1);
//                    ConfigListView_Gravar(Sistema, dataGridView1, true);
//                }
//            }
//            catch
//            {
//                AjustarColunasListViewCompleta(ref dataGridView1);
//                ConfigListView_Gravar(Sistema, dataGridView1, true, DiferenciadorListView);
//            }
//        }

//        public static void ConfigGridView_Oracle(ref DataGridView grid, string NomeView, string QualAmbiente)
//        {
//            bool[] ColunasCongeladas = new bool[grid.Columns.Count];

//            try
//            {
//                for (int i = 0; i < grid.Columns.Count; i++)
//                {
//                    ColunasCongeladas[i] = grid.Columns[i].Frozen;
//                    grid.Columns[i].Frozen = false;
//                }

//                grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

//                string Query = "SELECT NOVIEW As Nome_View, NUPOGR As Posicao_Grid, NOCOLU As Nome_Coluna,    NOCACO As Titulo_Coluna, MECOLU As Tamanho_Coluna, COSTAT As Status_Coluna, COPICT As Picture_Coluna  FROM   THK WHERE NOVIEW = '" + NomeView.ToUpper() + "'  ORDER BY NOVIEW, NUPOGR";
//                DataSet RsetDescColumn = new Conexao(QualAmbiente).ExecutaSQL(Query);

//                for (int i = 0; i < RsetDescColumn.Tables[0].Rows.Count; i++)
//                {
//                    try
//                    {
//                        grid.Columns[RsetDescColumn.Tables[0].Rows[i]["NOME_COLUNA"].ToString().Trim()].HeaderText = RsetDescColumn.Tables[0].Rows[i]["TITULO_COLUNA"].ToString();
//                        grid.Columns[RsetDescColumn.Tables[0].Rows[i]["NOME_COLUNA"].ToString().Trim()].Width = int.Parse(RsetDescColumn.Tables[0].Rows[i]["TAMANHO_COLUNA"].ToString());
//                    }
//                    catch
//                    {
//                    }
//                }



//                for (int i = 0; i < grid.Columns.Count; i++)
//                {
//                    grid.Columns[i].Frozen = ColunasCongeladas[i];
//                }

//                //objColDefAuxP.Clear
//                //Do Until RsetDescColumn.EOF
//                //    Set objColDefAux = objColDefAuxP.Add
//                //    For Each clDescColumn In RsetDescColumn.Fields
//                //        Select Case clDescColumn.Name 'SourceColumn
//                //            Case UCase("Nome_Coluna")
//                //                objColDefAux.ValueName = Trim(clDescColumn.Value)
//                //            Case UCase("Titulo_Coluna")
//                //                objColDefAux.Titulo = Trim(clDescColumn.Value)
//                //            Case UCase("Tamanho_Coluna")
//                //                objColDefAux.Largura = CDbl(clDescColumn.Value)
//                //            Case UCase("Status_Coluna")
//                //                objColDefAux.Status = Trim(clDescColumn.Value)
//                //            Case UCase("Picture_Coluna")
//                //                objColDefAux.Picture = Trim(clDescColumn.Value)
//                //        End Select
//                //    Next clDescColumn
//                //    RsetDescColumn.MoveNext
//                //Loop
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }

//        public static void ConfigListView_Oracle(ref ListView grid, string NomeView, string QualAmbiente)
//        {
//            bool[] ColunasCongeladas = new bool[grid.Columns.Count];

//            string Query = "SELECT NOVIEW As Nome_View, NUPOGR As Posicao_Grid, NOCOLU As Nome_Coluna,    NOCACO As Titulo_Coluna, MECOLU As Tamanho_Coluna, COSTAT As Status_Coluna, COPICT As Picture_Coluna  FROM   THK WHERE NOVIEW = '" + NomeView.ToUpper() + "'  ORDER BY NOVIEW, NUPOGR";
//            DataSet RsetDescColumn = new Conexao(QualAmbiente).ExecutaSQL(Query);

//            for (int i = 0; i < RsetDescColumn.Tables[0].Rows.Count; i++)
//            {
//                grid.Columns[RsetDescColumn.Tables[0].Rows[i][2].ToString().Trim()].Text = RsetDescColumn.Tables[0].Rows[i][3].ToString().Trim();
//                grid.Columns[RsetDescColumn.Tables[0].Rows[i][2].ToString().Trim()].Width = int.Parse(RsetDescColumn.Tables[0].Rows[i][4].ToString());
//            }
//        }

//        public static void ConfigListView_CriarColunas(string Sistema, ref ListView dataGridView1, string DiferenciadorListView)
//        {
//            try
//            {
//                if (dataGridView1.Items.Count <= 0) return;

//                string sKey = GetAllTreeParentName(dataGridView1) + DiferenciadorListView;//dataGridView1.FindForm().Name + "." + dataGridView1.Name

//                string[] colunas = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("MBBras").OpenSubKey(PastaSistemaRegistry).OpenSubKey(Sistema).OpenSubKey(sKey).GetSubKeyNames();

//                if (colunas.Length == dataGridView1.Columns.Count)
//                {

//                    for (int i = 0; i < colunas.Length; i++)
//                    {
//                        string Header = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("MBBras").OpenSubKey(PastaSistemaRegistry).OpenSubKey(Sistema).OpenSubKey(sKey).OpenSubKey(colunas[i]).GetValue("Titulo_Coluna").ToString();
//                        string Tamanho = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("MBBras").OpenSubKey(PastaSistemaRegistry).OpenSubKey(Sistema).OpenSubKey(sKey).OpenSubKey(colunas[i]).GetValue("Tamanho_Coluna").ToString();
//                        string Status = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("MBBras").OpenSubKey(PastaSistemaRegistry).OpenSubKey(Sistema).OpenSubKey(sKey).OpenSubKey(colunas[i]).GetValue("Status_Coluna").ToString();
//                        string Index = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("MBBras").OpenSubKey(PastaSistemaRegistry).OpenSubKey(Sistema).OpenSubKey(sKey).OpenSubKey(colunas[i]).GetValue("Posicao_Coluna").ToString();

//                        dataGridView1.Columns[colunas[i]].Text = Header;
//                        dataGridView1.Columns[colunas[i]].Width = int.Parse(Tamanho);
//                        dataGridView1.Columns[colunas[i]].DisplayIndex = int.Parse(Index);
//                        if (Status.Equals("N"))
//                            dataGridView1.Columns[colunas[i]].Width = 0;
//                        else
//                        {
//                            if (Tamanho.Equals("0"))
//                                dataGridView1.Columns[colunas[i]].Width = 100;
//                        }
//                    }
//                    dataGridView1.Refresh();
//                }
//                else
//                {
//                    ConfigGridView_Restaurar(Sistema, sKey);
//                    AjustarColunasListView(ref dataGridView1);
//                    ConfigListView_Gravar(Sistema, dataGridView1, true);
//                }
//            }
//            catch
//            {
//                AjustarColunasListView(ref dataGridView1);
//                ConfigListView_Gravar(Sistema, dataGridView1, true, DiferenciadorListView);
//            }
//        }

//        public static void ConfigGridView_LerColunas(string Sistema, string GridViewName, ref ListView ListaNaoExibir, ref ListView ListaExibir)
//        {
//            try
//            {
//                ListViewItem item = null;

//                string[] colunas = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("MBBras").OpenSubKey(PastaSistemaRegistry).OpenSubKey(Sistema).OpenSubKey(GridViewName).GetSubKeyNames();


//                //ListaExibir.MultiSelect = false;
//                // ListaNaoExibir.MultiSelect = false;

//                ListaExibir.Items.Clear();
//                ListaNaoExibir.Items.Clear();

//                ListaExibir.Columns.Clear();
//                ListaExibir.Columns.Add("Coluna", 200, HorizontalAlignment.Left);

//                ListaNaoExibir.Columns.Clear();
//                ListaNaoExibir.Columns.Add("Coluna", 200, HorizontalAlignment.Left);


//                List<ColunasGridInfo> listCol = new List<ColunasGridInfo>();

//                for (int i = 0; i < colunas.Length; i++)
//                {
//                    string Header = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("MBBras").OpenSubKey(PastaSistemaRegistry).OpenSubKey(Sistema).OpenSubKey(GridViewName).OpenSubKey(colunas[i]).GetValue("Titulo_Coluna").ToString();
//                    string Exibe = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("MBBras").OpenSubKey(PastaSistemaRegistry).OpenSubKey(Sistema).OpenSubKey(GridViewName).OpenSubKey(colunas[i]).GetValue("Status_Coluna").ToString();
//                    string Posicao = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("MBBras").OpenSubKey(PastaSistemaRegistry).OpenSubKey(Sistema).OpenSubKey(GridViewName).OpenSubKey(colunas[i]).GetValue("Posicao_Coluna").ToString();

//                    if (Posicao == "")
//                        Posicao = i.ToString();

//                    listCol.Add(new ColunasGridInfo(colunas[i], int.Parse(Posicao), Exibe, 100, Header));

//                    //Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("MBBras").OpenSubKey(PastaSistemaRegistry).OpenSubKey(Sistema).OpenSubKey(GridViewName).OpenSubKey(colunas[i]).GetValue("Posicao_Coluna").ToString();
//                }

//                IEnumerable<ColunasGridInfo> query = listCol.OrderBy(coluna => coluna.Posicao);
//                foreach (ColunasGridInfo coluna in query)
//                {

//                    item = new ListViewItem(coluna.Titulo.Trim());
//                    item.Tag = coluna.Nome;

//                    if (coluna.Status.Equals("S"))
//                        ListaExibir.Items.Add(item);

//                    item = null;
//                }

//                IEnumerable<ColunasGridInfo> naoExibe = listCol.OrderBy(coluna => coluna.Titulo);
//                foreach (ColunasGridInfo coluna in naoExibe)
//                {

//                    item = new ListViewItem(coluna.Titulo.Trim());
//                    item.Tag = coluna.Nome;

//                    if (coluna.Status.Equals("N"))
//                        ListaNaoExibir.Items.Add(item);

//                    item = null;
//                }

//                //AjustarColunasListView(ref ListaExibir);
//                //AjustarColunasListView(ref ListaNaoExibir);

//            }
//            catch
//            {
//                throw new Exception("Erro na leitura do registro de colunas");
//            }
//        }

//        public static void ConfigurarTipoPlano(ref ComboBox pCombo)
//        {
//            pCombo.Items.Clear();
//            pCombo.Items.Add(new ComboInfo("01", "01 - Plano de Informação"));
//            pCombo.Items.Add(new ComboInfo("02", "02 - Plano de Montagem"));
//        }

//        public static void ConfiguarTipoPlano(ref ToolStripComboBox ptsCombo)
//        {
//            ComboBox cb = new ComboBox();
//            ConfigurarTipoPlano(ref cb);
//            Funcoes.CopiaToCombo(cb, ref ptsCombo);
//        }

//        public static string TratarMensagemErro(string Texto)
//        {
//            string[] msg;
//            string[] newMsg;

//            if (Texto.Contains("#"))
//            {
//                msg = Texto.Split(new char[] { '#' });

//                if (msg.Length > 0)
//                {
//                    if (msg[0].Contains("ORA-"))
//                    {
//                        newMsg = msg[0].Split(new char[] { ':' });
//                        if (newMsg.Length > 1)
//                            Texto = newMsg[1].Trim();
//                        else
//                            Texto = newMsg[0].Trim();
//                    }
//                    else
//                    {
//                        Texto = msg[0].Trim();
//                    }
//                }
//            }
//            else
//            {
//                Texto = Texto.Replace("\n", ":");
//                msg = Texto.Split(new char[] { ':' });
//                //if (msg.Length > 3)
//                //{
//                //    Texto = msg[3].Trim();
//                //}
//                Texto = "";

//                for (int i = 0; i < msg.Length; i++)
//                {
//                    if (!msg[i].Contains("ORA-"))
//                        Texto += msg[i];
//                }

//            }

//            try
//            {
//                string Codigo = Texto.Substring(0, 5);
//                if (int.Parse(Codigo) > 0)
//                    Texto = Texto.Substring(5).Trim();
//            }
//            catch
//            {
//            }

//            return Texto;
//        }

//        public static void MessageBox_Erro(string Texto, bool TratarMsg)
//        {
//            if (TratarMsg)
//            {
//                ////string[] msg;
//                ////string[] newMsg;

//                ////if (Texto.Contains("#"))
//                ////{
//                ////    msg = Texto.Split(new char[] { '#' });

//                ////    if (msg.Length > 0)
//                ////    {
//                ////        newMsg = msg[0].Split(new char[] { ':' });
//                ////        if (newMsg.Length > 1)
//                ////            Texto = newMsg[1].Trim();
//                ////        else
//                ////            Texto = newMsg[0].Trim();

//                ////    }
//                ////}
//                ////else
//                ////{
//                ////    Texto = Texto.Replace("\n", ":");
//                ////    msg = Texto.Split(new char[] { ':' });
//                ////    if (msg.Length > 3)
//                ////    {
//                ////        Texto = msg[3].Trim();
//                ////    }
//                ////}

//                Texto = TratarMensagemErro(Texto);
//            }

//            MessageBox_Erro(Texto);
//        }

//        public static void MessageBox_Erro(string Texto)
//        {
//            InformacaoStatusBar();
//            MessageBox.Show(Texto.Trim(), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
//        }

//        public static void MessageBox_Erro(Exception ex)
//        {
//            if (ex.Message.Contains("#"))
//            {
//                MessageBox_Erro(TratarMensagemErro(ex.Message));
//            }
//            else
//            {
//                MessageBox_Erro(ex, false);
//            }
//        }

//        public static void MessageBox_Erro(Exception ex, bool TratarMsg)
//        {
//            string InnerException = "";

//            if (ex.InnerException != null)
//            {
//                InnerException = Environment.NewLine + ex.InnerException.ToString();
//            }
//            else
//            {
//                InnerException = "";
//            }

//            string Mensagem = ex.Message + InnerException;
//            InformacaoStatusBar();

//            if (TratarMsg) Mensagem = TratarMensagemErro(ex.Message);

//            MessageBox.Show(Mensagem + Environment.NewLine + ex.StackTrace, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
//        }

//        public static void MessageBox_Erro(string Texto, string Titulo)
//        {
//            InformacaoStatusBar();
//            MessageBox.Show(Texto.Trim(), Titulo.Trim(), MessageBoxButtons.OK, MessageBoxIcon.Error);
//        }

//        public static DialogResult MessageBox_Pergunta(string Texto)
//        {
//            InformacaoStatusBar();
//            return MessageBox.Show(Texto.Trim(), Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
//        }

//        public static DialogResult MessageBox_Pergunta(string Texto, string Titulo)
//        {
//            InformacaoStatusBar();
//            return MessageBox.Show(Texto.Trim(), Titulo, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
//        }

//        public static DialogResult MessageBox_PerguntaCritica(string Texto)
//        {
//            InformacaoStatusBar();
//            return MessageBox.Show(Texto.Trim(), Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2);
//        }

//        public static DialogResult MessageBox_PerguntaCritica(string Texto, string Titulo)
//        {
//            InformacaoStatusBar();
//            return MessageBox.Show(Texto.Trim(), Titulo, MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2);
//        }

//        public static void MessageBox_Exclamacao(string Texto)
//        {
//            InformacaoStatusBar();
//            MessageBox.Show(Texto.Trim(), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
//        }

//        public static void MessageBox_Exclamacao(string Texto, string Titulo)
//        {
//            InformacaoStatusBar();
//            MessageBox.Show(Texto.Trim(), Titulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
//        }

//        public static void MessageBox_Informacao(string Texto)
//        {
//            InformacaoStatusBar();
//            MessageBox.Show(Texto.Trim(), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
//        }

//        public static void MessageBox_Informacao(string Texto, string Titulo)
//        {
//            InformacaoStatusBar();
//            MessageBox.Show(Texto.Trim(), Titulo, MessageBoxButtons.OK, MessageBoxIcon.Information);
//        }

//        public static void UFGotFocus(ref TextBox Control)
//        {
//            Control.SelectionStart = 0;
//            Control.SelectionLength = Control.Text.Trim().Length;
//            if (Control.Text.Trim() == string.Empty)
//            {
//                Control.Text = string.Empty;
//            }
//        }

//        public static void UFGotFocus(ref MaskedTextBox Control)
//        {
//            Control.SelectionStart = 0;
//            Control.SelectionLength = Control.Text.Trim().Length;
//            if (Control.Text.Trim() == string.Empty)
//            {
//                Control.Text = string.Empty;
//            }
//        }

//        public static void GetAmbientesTipo(FGTipoLinha pTipoLinha, ref string[] pstrAmbientes, string QualAmbiente)
//        {
//            string Old_Ambiente = QualAmbiente;

//            if (ObterFabrica(QualAmbiente) == Fabrica.SBC)
//            {
//                QualAmbiente = QualAmbiente.Substring(1, 1);

//                if (!QualAmbiente.Equals("D"))
//                {
//                    // ******************************
//                    // *************SBC**************
//                    // ******************************
//                    if (pTipoLinha == FGTipoLinha.Caminhao)
//                    {
//                        pstrAmbientes = new string[3];
//                        pstrAmbientes[0] = "G" + QualAmbiente + "V1";
//                        pstrAmbientes[1] = "G" + QualAmbiente + "V2";
//                        pstrAmbientes[2] = "G" + QualAmbiente + "V3";
//                    }
//                    else if (pTipoLinha == FGTipoLinha.Onibus)
//                    {
//                        pstrAmbientes = new string[2];
//                        pstrAmbientes[0] = "G" + QualAmbiente + "V4";
//                        pstrAmbientes[1] = "G" + QualAmbiente + "V5";
//                    }
//                    else if (pTipoLinha == FGTipoLinha.Motor)
//                    {
//                        pstrAmbientes = new string[6];
//                        pstrAmbientes[0] = "G" + QualAmbiente + "M1";
//                        pstrAmbientes[1] = "G" + QualAmbiente + "M2";
//                        pstrAmbientes[2] = "G" + QualAmbiente + "M3";
//                        pstrAmbientes[3] = "G" + QualAmbiente + "M4";
//                        pstrAmbientes[4] = "G" + QualAmbiente + "M5";
//                        pstrAmbientes[5] = "G" + QualAmbiente + "M6";
//                    }
//                    else if (pTipoLinha == FGTipoLinha.Cambio)
//                    {
//                        pstrAmbientes = new string[5];
//                        pstrAmbientes[0] = "G" + QualAmbiente + "C1";
//                        pstrAmbientes[1] = "G" + QualAmbiente + "C2";
//                        pstrAmbientes[2] = "G" + QualAmbiente + "C3";
//                        pstrAmbientes[3] = "G" + QualAmbiente + "C4";
//                        pstrAmbientes[4] = "G" + QualAmbiente + "C5";
//                    }
//                    else if (pTipoLinha == FGTipoLinha.Eixo)
//                    {
//                        pstrAmbientes = new string[6];
//                        pstrAmbientes[0] = "G" + QualAmbiente + "E1";
//                        pstrAmbientes[1] = "G" + QualAmbiente + "E2";
//                        pstrAmbientes[2] = "G" + QualAmbiente + "E3";
//                        pstrAmbientes[3] = "G" + QualAmbiente + "E4";
//                        pstrAmbientes[4] = "G" + QualAmbiente + "E5";
//                        pstrAmbientes[5] = "G" + QualAmbiente + "E6";
//                    }
//                    else if (pTipoLinha == FGTipoLinha.Cabina)
//                    {
//                        pstrAmbientes = new string[2];
//                        pstrAmbientes[0] = "G" + QualAmbiente + "V6";
//                        pstrAmbientes[1] = "G" + QualAmbiente + "V7";
//                    }
//                    else if (pTipoLinha == FGTipoLinha.Agregado)
//                    {
//                        pstrAmbientes = new string[17];
//                        pstrAmbientes[0] = "G" + QualAmbiente + "M1";
//                        pstrAmbientes[1] = "G" + QualAmbiente + "M2";
//                        pstrAmbientes[2] = "G" + QualAmbiente + "M3";
//                        pstrAmbientes[3] = "G" + QualAmbiente + "M4";
//                        pstrAmbientes[4] = "G" + QualAmbiente + "M5";
//                        pstrAmbientes[5] = "G" + QualAmbiente + "M6";

//                        pstrAmbientes[6] = "G" + QualAmbiente + "C1";
//                        pstrAmbientes[7] = "G" + QualAmbiente + "C2";
//                        pstrAmbientes[8] = "G" + QualAmbiente + "C3";
//                        pstrAmbientes[9] = "G" + QualAmbiente + "C4";
//                        pstrAmbientes[10] = "G" + QualAmbiente + "C5";

//                        pstrAmbientes[11] = "G" + QualAmbiente + "E1";
//                        pstrAmbientes[12] = "G" + QualAmbiente + "E2";
//                        pstrAmbientes[13] = "G" + QualAmbiente + "E3";
//                        pstrAmbientes[15] = "G" + QualAmbiente + "E4";
//                        pstrAmbientes[15] = "G" + QualAmbiente + "E5";
//                        pstrAmbientes[16] = "G" + QualAmbiente + "E6";
//                    }
//                }
//                else
//                {
//                    pstrAmbientes = new string[1];
//                    pstrAmbientes[0] = Old_Ambiente;

//                    //if (pTipoLinha == FGTipoLinha.Caminhao)
//                    //{
//                    //    pstrAmbientes = new string[1];
//                    //    pstrAmbientes[0] = "G" + QualAmbiente + "V1";
//                    //}
//                    //else if (pTipoLinha == FGTipoLinha.Onibus)
//                    //{
//                    //    pstrAmbientes = new string[1];
//                    //    pstrAmbientes[0] = "G" + QualAmbiente + "V2";
//                    //}
//                    //else if (pTipoLinha == FGTipoLinha.Motor)
//                    //{
//                    //    pstrAmbientes = new string[1];
//                    //    pstrAmbientes[0] = "G" + QualAmbiente + "E2";
//                    //}
//                    //else if (pTipoLinha == FGTipoLinha.Cambio)
//                    //{
//                    //    pstrAmbientes = new string[1];
//                    //    pstrAmbientes[0] = "G" + QualAmbiente + "E2";
//                    //}
//                    //else if (pTipoLinha == FGTipoLinha.Eixo)
//                    //{
//                    //    pstrAmbientes = new string[1];
//                    //    pstrAmbientes[0] = "G" + QualAmbiente + "E1";
//                    //}
//                    //else if (pTipoLinha == FGTipoLinha.Cabina)
//                    //{
//                    //    pstrAmbientes = new string[1];
//                    //    pstrAmbientes[0] = "G" + QualAmbiente + "V2";
//                    //}
//                    //else if (pTipoLinha == FGTipoLinha.AgregadoDes)
//                    //{
//                    //    pstrAmbientes = new string[2];
//                    //    pstrAmbientes[0] = "GDE1";
//                    //    pstrAmbientes[1] = "GDE2";
//                    //}
//                    //else if (pTipoLinha == FGTipoLinha.CaminhaoDes)
//                    //{
//                    //    pstrAmbientes = new string[2];
//                    //    pstrAmbientes[0] = "GDV1";
//                    //    pstrAmbientes[1] = "GDV2";
//                    //}
//                    //else if (pTipoLinha == FGTipoLinha.OnibusDes)
//                    //{
//                    //    pstrAmbientes = new string[2];
//                    //    pstrAmbientes[0] = "GDV1";
//                    //    pstrAmbientes[1] = "GDV2";
//                    //}
//                }
//            }
//            else
//            {
//                QualAmbiente = QualAmbiente.Substring(1, 1);

//                // ******************************
//                // *************JDV**************
//                // ******************************
//                if (pTipoLinha == FGTipoLinha.Caminhao)
//                {
//                    pstrAmbientes = new string[1];
//                    pstrAmbientes[0] = "J" + QualAmbiente + "V1";
//                }
//                else if (pTipoLinha == FGTipoLinha.Cabina)
//                {
//                    pstrAmbientes = new string[1];
//                    pstrAmbientes[0] = "J" + QualAmbiente + "V2";
//                }

//            }
//        }

//        public static string GetAmbienteDB2(string Ambiente)
//        {
//            if (Ambiente == "D") return "IMSD";
//            if (Ambiente == "H") return "IMSR";
//            if (Ambiente == "P") return "IMSP";
//            if (Ambiente == "T") return "IMST";
//            return "IMSP";
//        }

//        public static void CopiaToCombo(ComboBox cboDe, ref ComboBox cboPara)
//        {
//            for (int i = 0; i < cboDe.Items.Count; i++)
//            {
//                cboPara.Items.Add(cboDe.Items[i]);
//            }
//        }

//        public static void CopiaToCombo(ToolStripComboBox cboDe, ref ComboBox cboPara)
//        {
//            for (int i = 0; i < cboDe.Items.Count; i++)
//            {
//                cboPara.Items.Add(cboDe.Items[i]);
//            }
//        }

//        public static void CopiaToCombo(ComboBox cboDe, ref ToolStripComboBox cboPara)
//        {
//            for (int i = 0; i < cboDe.Items.Count; i++)
//            {
//                cboPara.Items.Add(cboDe.Items[i]);
//            }
//        }

//        public static void CopiaToCombo(ToolStripComboBox cboDe, ref ToolStripComboBox cboPara)
//        {
//            for (int i = 0; i < cboDe.Items.Count; i++)
//            {
//                cboPara.Items.Add(cboDe.Items[i]);
//            }
//        }

//        public static void EnableTodosTextBox(Control objContainer, bool valor)
//        {
//            List<Control> controles = Funcoes.GetAllControls<TextBox>(objContainer);
//            for (int i = 0; i < controles.Count; i++)
//            {
//                controles[i].Enabled = valor;
//            }
//        }

//        public static void LimparTodosCampos(Control objContainer)
//        {
//            List<Control> controles = Funcoes.GetAllControls<TextBox>(objContainer);
//            for (int i = 0; i < controles.Count; i++)
//            {
//                controles[i].Text = string.Empty;
//            }

//            List<Control> labels = Funcoes.GetAllControls<Label>(objContainer);
//            for (int i = 0; i < labels.Count; i++)
//            {
//                labels[i].Text = string.Empty;
//            }

//            List<Control> combos = Funcoes.GetAllControls<ComboBox>(objContainer);
//            for (int i = 0; i < combos.Count; i++)
//            {
//                combos[i].Text = string.Empty;
//            }

//            List<Control> chks = Funcoes.GetAllControls<CheckBox>(objContainer);
//            for (int i = 0; i < chks.Count; i++)
//            {
//                ((CheckBox)chks[i]).CheckState = CheckState.Unchecked;
//            }
//        }

//        public static void LimparTodosTextBox(Control objContainer)
//        {
//            LimparTodosTextBox(objContainer, true);
//        }

//        public static void ExcluirControlesFlowPanel(ref FlowLayoutPanel Container)
//        {
//            foreach (Control ctrl in Container.Controls)
//            {
//                Container.Controls.Remove(ctrl);
//                ctrl.Dispose();
//            }

//            Container.Controls.Clear();
//        }

//        public static void LimparTodosTextBox(Control objContainer, bool clearReadOnly)
//        {
//            List<Control> controles = Funcoes.GetAllControls<TextBox>(objContainer);
//            for (int i = 0; i < controles.Count; i++)
//            {
//                if (clearReadOnly)
//                {
//                    controles[i].Text = string.Empty;
//                }
//                else
//                {
//                    if (!((TextBox)controles[i]).ReadOnly)
//                    {
//                        controles[i].Text = string.Empty;
//                    }
//                }
//            }
//        }

//        public static void LimparTodosLabel(Control objContainer)
//        {
//            List<Control> controles = Funcoes.GetAllControls<Label>(objContainer);
//            for (int i = 0; i < controles.Count; i++)
//            {
//                controles[i].Text = string.Empty;
//            }
//        }

//        public static void SelecionarComboInfo1(ref ComboBox cboCombo, string Valor)
//        {
//            ComboInfo gr = null;
//            for (int i = 0; i < cboCombo.Items.Count; i++)
//            {
//                gr = (ComboInfo)cboCombo.Items[i];

//                if (gr.Codigo.Trim().Equals(Valor.Trim()))
//                {
//                    cboCombo.SelectedIndex = i;
//                    break;
//                }
//            }
//            gr = null;
//        }

//        public static void SelecionarDescricaoComboInfo(ref ComboBox cboCombo, string Descricao)
//        {
//            ComboInfo gr = null;

//            for (int i = 0; i < cboCombo.Items.Count; i++)
//            {
//                gr = (ComboInfo)cboCombo.Items[i];

//                if (gr.Descricao.Trim().Equals(Descricao.Trim()))
//                {
//                    cboCombo.SelectedIndex = i;
//                    break;
//                }
//            }
//            gr = null;
//        }

//        public static void SelecionarComboInfo1(ref ToolStripComboBox cboCombo, string Valor)
//        {
//            ComboInfo gr = null;
//            for (int i = 0; i < cboCombo.Items.Count; i++)
//            {
//                gr = (ComboInfo)cboCombo.Items[i];

//                if (gr.Codigo.Trim().Equals(Valor.Trim()))
//                {
//                    cboCombo.SelectedIndex = i;
//                    break;
//                }
//            }
//            gr = null;
//        }

//        public static void SelecionarComboInfo(ref ComboBox cboCombo, string Valor)
//        {
//            ComboInfo2 gr = null;
//            for (int i = 0; i < cboCombo.Items.Count; i++)
//            {
//                gr = (ComboInfo2)cboCombo.Items[i];

//                if (gr.Codigo2.Trim().Equals(Valor.Trim()))
//                {
//                    cboCombo.SelectedIndex = i;
//                    break;
//                }
//            }
//            gr = null;
//        }

//        public static void SelecionarComboPostoTrabalho(ref ToolStripComboBox cboCombo, string Valor)
//        {
//            ComboPostoTrabalho gr = null;
//            for (int i = 0; i < cboCombo.Items.Count; i++)
//            {
//                gr = (ComboPostoTrabalho)cboCombo.Items[i];

//                if (gr.Codigo.Trim().Equals(Valor.Trim()))
//                {
//                    cboCombo.SelectedIndex = i;
//                    break;
//                }
//            }
//            gr = null;
//        }

//        public static void SelecionarComboPostoTrabalho(ref ComboBox cboCombo, string Valor)
//        {
//            ComboPostoTrabalho gr = null;
//            for (int i = 0; i < cboCombo.Items.Count; i++)
//            {
//                gr = (ComboPostoTrabalho)cboCombo.Items[i];

//                if (gr.Codigo.Trim().Equals(Valor.Trim()))
//                {
//                    cboCombo.SelectedIndex = i;
//                    break;
//                }
//            }
//            gr = null;
//        }

//        public static void SelecionarComboInfo(ref ToolStripComboBox cboCombo, string Valor)
//        {
//            ComboInfo2 gr = null;
//            for (int i = 0; i < cboCombo.Items.Count; i++)
//            {
//                gr = (ComboInfo2)cboCombo.Items[i];

//                if (gr.Codigo1.Trim().Equals(Valor.Trim()))
//                {
//                    cboCombo.SelectedIndex = i;
//                    break;
//                }
//            }
//            gr = null;
//        }

//        public static void SelecionarComboGrupoInfo(ref ComboBox cboCombo, string Valor)
//        {
//            GruposInfo gr = null;
//            for (int i = 0; i < cboCombo.Items.Count; i++)
//            {
//                gr = (GruposInfo)cboCombo.Items[i];

//                if (gr.Codigo.ToString().Trim().Equals(Valor.Trim()) || gr.Sigla.ToString().Trim().Equals(Valor.Trim()))
//                {
//                    cboCombo.SelectedIndex = i;
//                    break;
//                }
//            }
//            gr = null;
//        }

//        public static void SelecionarComboGrupoInfo(ref ToolStripComboBox cboCombo, string Valor)
//        {
//            GruposInfo gr = null;
//            for (int i = 0; i < cboCombo.Items.Count; i++)
//            {
//                gr = (GruposInfo)cboCombo.Items[i];

//                if (gr.Codigo.ToString().Trim().Equals(Valor.Trim()) || gr.Sigla.ToString().Trim().Equals(Valor.Trim()))
//                {
//                    cboCombo.SelectedIndex = i;
//                    break;
//                }
//            }
//            gr = null;
//        }

//        public static void SelecionarComboSequenciaInfo(ref ToolStripComboBox cboCombo, string Valor)
//        {
//            ComboSequenciaInfo gr = null;
//            for (int i = 0; i < cboCombo.Items.Count; i++)
//            {
//                gr = (ComboSequenciaInfo)cboCombo.Items[i];

//                if (gr.COSQPR.Trim().Equals(Valor.Trim()))
//                {
//                    cboCombo.SelectedIndex = i;
//                    break;
//                }
//            }
//            gr = null;
//        }

//        public static void SelecionarComboSequenciaInfo(ref ComboBox cboCombo, string Valor)
//        {
//            ComboSequenciaInfo gr = null;
//            for (int i = 0; i < cboCombo.Items.Count; i++)
//            {
//                gr = (ComboSequenciaInfo)cboCombo.Items[i];

//                if (gr.COSQPR.Trim().Equals(Valor.Trim()))
//                {
//                    cboCombo.SelectedIndex = i;
//                    break;
//                }
//            }
//            gr = null;
//        }

//        public static void Ctrl_C(Control controle)
//        {
//            List<Control> textBoxList = GetAllControls<TextBox>(controle);
//            List<Control> rtfBoxList = GetAllControls<RichTextBox>(controle);
//            List<Control> mskBoxList = GetAllControls<MaskedTextBox>(controle);

//            foreach (TextBox t in textBoxList)
//            {
//                if (t.Focused)
//                    Clipboard.SetData(DataFormats.Text, (Object)t.SelectedText);
//            }

//            foreach (RichTextBox t in rtfBoxList)
//            {
//                if (t.Focused)
//                    Clipboard.SetData(DataFormats.Text, (Object)t.SelectedText);
//            }

//            foreach (MaskedTextBox t in mskBoxList)
//            {
//                if (t.Focused)
//                    Clipboard.SetData(DataFormats.Text, (Object)t.SelectedText);
//            }

//        }

//        public static void Ctrl_V(Control controle)
//        {
//            List<Control> textBoxList = GetAllControls<TextBox>(controle);
//            List<Control> rtfBoxList = GetAllControls<RichTextBox>(controle);
//            List<Control> mskBoxList = GetAllControls<MaskedTextBox>(controle);

//            int PosicaoAtual = 0;
//            string Left = "";
//            string Right = "";

//            foreach (TextBox t in textBoxList)
//            {
//                if (t.Focused)
//                {
//                    PosicaoAtual = t.SelectionStart;
//                    if (t.SelectionStart.Equals(0))
//                    {
//                        t.Text = string.Concat(Clipboard.GetText(), t.Text);
//                    }
//                    else
//                    {
//                        Left = t.Text.Substring(0, t.SelectionStart);
//                        Right = t.Text.Substring(t.SelectionStart);
//                        t.Text = string.Concat(Left, Clipboard.GetText(), Right);
//                    }
//                    t.SelectionStart = PosicaoAtual;
//                }
//            }

//            foreach (RichTextBox t in rtfBoxList)
//            {
//                if (t.Focused)
//                    PosicaoAtual = t.SelectionStart;
//                if (t.SelectionStart.Equals(0))
//                {
//                    t.Text = string.Concat(Clipboard.GetText(), t.Text);
//                }
//                else
//                {
//                    Left = t.Text.Substring(0, t.SelectionStart);
//                    Right = t.Text.Substring(t.SelectionStart);
//                    t.Text = string.Concat(Left, Clipboard.GetText(), Right);
//                }
//                t.SelectionStart = PosicaoAtual;
//            }

//            foreach (MaskedTextBox t in mskBoxList)
//            {
//                if (t.Focused)
//                    PosicaoAtual = t.SelectionStart;
//                if (t.SelectionStart.Equals(0))
//                {
//                    t.Text = string.Concat(Clipboard.GetText(), t.Text);
//                }
//                else
//                {
//                    Left = t.Text.Substring(0, t.SelectionStart);
//                    Right = t.Text.Substring(t.SelectionStart);
//                    t.Text = string.Concat(Left, Clipboard.GetText(), Right);
//                }
//                t.SelectionStart = PosicaoAtual;
//            }
//        }

//        public static void UFLimitarNumeros(ref object sender, ref KeyPressEventArgs e, int Tam)
//        {
//            UFCampoTexto(ref sender, ref e, Tam);
//            DigitarSomenteNumeros(ref e);
//        }

//        public static void DigitarSomenteNumeros(ref KeyPressEventArgs e, bool AceitaVirgula)
//        {
//            switch (e.KeyChar)
//            {
//                case (char)3:  //Ctrl+C
//                case (char)22: //Ctrl+V
//                case (char)24: //Ctrl+X
//                case (char)26: //Ctrl+Z
//                case (char)1:  //Ctrl+A
//                case (char)8:  //Backspace
//                case (char)45:  //Menos
//                case (char)43:  //Mais
//                    return;
//                default:
//                    if (e.KeyChar == 44 && AceitaVirgula) return;
//                    if (!char.IsDigit(e.KeyChar)) e.Handled = true;
//                    break;
//            }
//        }

//        public static void DigitarSomenteNumeros(ref KeyPressEventArgs e)
//        {
//            switch (e.KeyChar)
//            {
//                case (char)3:  //Ctrl+C
//                case (char)22: //Ctrl+V
//                case (char)24: //Ctrl+X
//                case (char)26: //Ctrl+Z
//                case (char)1:  //Ctrl+A
//                case (char)8:  //Backspace
//                case (char)45:  //Menos
//                case (char)43:  //Mais
//                    return;
//                default:
//                    if (!char.IsDigit(e.KeyChar)) e.Handled = true;
//                    break;
//            }
//        }

//        public static void DigitarSomenteLetras(ref KeyPressEventArgs e)
//        {
//            switch (e.KeyChar)
//            {
//                case (char)3:  //Ctrl+C
//                case (char)22: //Ctrl+V
//                case (char)24: //Ctrl+X
//                case (char)26: //Ctrl+Z
//                case (char)1:  //Ctrl+A
//                case (char)8:  //Backspace
//                    return;
//                default:
//                    if (char.IsDigit(e.KeyChar)) e.Handled = true;
//                    break;
//            }

//        }

//        public static string GetAllTreeParentName(object Controle)
//        {
//            string sKey = "";

//            object ControleAtual = Controle;

//            while (true)
//            {
//                if (ControleAtual == null)
//                {
//                    sKey = sKey.Substring(0, sKey.Length - 1);
//                    break;
//                }
//                else
//                    if (((Control)ControleAtual).Name != "")
//                    sKey = ((Control)ControleAtual).Name + "." + sKey;

//                ControleAtual = ((Control)ControleAtual).Parent;
//            }

//            return sKey;
//        }

//        //public static string GetAllTreeParentName(ToolStripComboBox Controle)
//        //{
//        //    string sKey = "";

//        //    object ControleAtual = Controle;

//        //    while (true)
//        //    {
//        //        if (ControleAtual == null)
//        //        {
//        //            sKey = sKey.Substring(1, sKey.Length);
//        //            break;
//        //        }
//        //        else
//        //            if (((ToolStripco)ControleAtual).Name != "")
//        //                sKey += "." + ((ToolStripComboBox)ControleAtual).Name;

//        //        ControleAtual = ((ToolStripComboBox)ControleAtual).GetCurrentParent();
//        //    }

//        //    return sKey;
//        //}

//        //public static string GetAllTreeParentName(ToolStripTextBox Controle)
//        //{
//        //    string sKey = "";

//        //    object ControleAtual = Controle;

//        //    while (true)
//        //    {
//        //        if (ControleAtual == null)
//        //        {
//        //            sKey = sKey.Substring(1, sKey.Length);
//        //            break;
//        //        }
//        //        else
//        //            if (((ToolStripTextBox)ControleAtual).Name != "")
//        //                sKey += "." + ((ToolStripTextBox)ControleAtual).Name;

//        //        ControleAtual = ((ToolStripTextBox)ControleAtual).GetCurrentParent();
//        //    }

//        //    return sKey;
//        //}

//        public static void GravarIniPrint(string sSection, string sKey, string sValue, bool UserSettings)
//        {
//            //string Result = "";
//            string PathAppData = "";

//            System.OperatingSystem os = Environment.OSVersion;
//            Version vs = os.Version;

//            bool Win7 = false;

//            switch (vs.Major)
//            {
//                case 3:
//                case 4:
//                case 5:
//                    Win7 = false;
//                    break;
//                default:
//                    Win7 = true;
//                    break;
//            }

//            string FileINIPath = "";

//            if (!UserSettings)
//                PathAppData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
//            else
//                PathAppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

//            if (Win7)
//                FileINIPath = string.Concat(PathAppData, @"\MBBRAS\PRM\PRMPRINT.INI");
//            else
//                FileINIPath = string.Concat(PathAppData, @"\DCBR IT\PRM\PRMPRINT.INI");

//            WritePrivateProfileString(sSection, sKey, sValue, FileINIPath);
//        }

//        public static void GravarIni(string sSection, string sKey, string sValue)
//        {
//            GravarIni(sSection, sKey, sValue, "");
//        }

//        public static void GravarIni(string sSection, string sKey, string sValue, string Destino)
//        {
//            if (Destino.Equals("PRN"))
//            {
//                GravarIniPrint(sSection, sKey, sValue, false);
//            }
//            else
//            {
//                Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("Software").CreateSubKey("MBBras").CreateSubKey(PastaSistemaRegistry).CreateSubKey(sSection);
//                key.SetValue(sKey, sValue);
//                key.Close();
//            }
//        }

//        public static void GravarIni(string sSection, CheckBox cControl, Form FormOrigem)
//        {
//            string sKey = "";
//            if (FormOrigem.MdiParent == null)
//                sKey = FormOrigem.Name + "." + cControl.Name;
//            else
//                sKey = FormOrigem.MdiParent.Name + "." + FormOrigem.Name + "." + cControl.Name;

//            GravarIni(sSection, sKey, cControl.Checked.ToString());
//        }

//        public static void GravarIni(string sSection, RadioButton cControl, Form FormOrigem)
//        {
//            string sKey = "";
//            if (FormOrigem.MdiParent == null)
//                sKey = FormOrigem.Name + "." + cControl.Name;
//            else
//                sKey = FormOrigem.MdiParent.Name + "." + FormOrigem.Name + "." + cControl.Name;

//            GravarIni(sSection, sKey, cControl.Checked.ToString());
//        }

//        public static void GravarIni(string sSection, ToolStripButton cControl, Form FormOrigem)
//        {
//            string sKey = "";
//            if (FormOrigem.MdiParent == null)
//                sKey = FormOrigem.Name + "." + cControl.Name;
//            else
//                sKey = FormOrigem.MdiParent.Name + "." + FormOrigem.Name + "." + cControl.Name;

//            GravarIni(sSection, sKey, cControl.Checked.ToString());
//        }

//        public static void GravarIni(string sSection, ToolStripMenuItem cControl, Form FormOrigem)
//        {
//            string sKey = "";
//            if (FormOrigem.MdiParent == null)
//                sKey = FormOrigem.Name + "." + cControl.Name;
//            else
//                sKey = FormOrigem.MdiParent.Name + "." + FormOrigem.Name + "." + cControl.Name;

//            GravarIni(sSection, sKey, cControl.Checked.ToString());
//        }

//        public static void GravarIni(string sSection, ToolStripTextBox cControl, Form FormOrigem)
//        {
//            string sKey = "";
//            if (FormOrigem.MdiParent == null)
//                sKey = FormOrigem.Name + "." + cControl.Name;
//            else
//                sKey = FormOrigem.MdiParent.Name + "." + FormOrigem.Name + "." + cControl.Name;

//            GravarIni(sSection, sKey, cControl.Text.ToString());
//        }

//        public static void GravarIni(string sSection, ToolStripComboBox cControl, Form FormOrigem)
//        {
//            string sKey = "";
//            if (FormOrigem.MdiParent == null)
//                sKey = FormOrigem.Name + "." + cControl.Name;
//            else
//                sKey = FormOrigem.MdiParent.Name + "." + FormOrigem.Name + "." + cControl.Name;

//            GravarIni(sSection, sKey, cControl.Text.ToString());
//        }

//        public static void GravarIni(string sSection, TextBox cControl, Form FormOrigem)
//        {
//            string sKey = "";
//            if (FormOrigem.MdiParent == null)
//                sKey = FormOrigem.Name + "." + cControl.Name;
//            else
//                sKey = FormOrigem.MdiParent.Name + "." + FormOrigem.Name + "." + cControl.Name;

//            GravarIni(sSection, sKey, cControl.Text);
//        }

//        public static void GravarIni(string sSection, ComboBox cControl, Form FormOrigem)
//        {
//            string sKey = "";
//            if (FormOrigem.MdiParent == null)
//                sKey = FormOrigem.Name + "." + cControl.Name;
//            else
//                sKey = FormOrigem.MdiParent.Name + "." + FormOrigem.Name + "." + cControl.Name;

//            GravarIni(sSection, sKey, cControl.Text);
//        }

//        public static void ConverterParaString(ref string Registro, object Origem)
//        {
//            PropertyInfo[] ListaPropriedades = Origem.GetType().GetProperties();

//            for (int i = 0; i < ListaPropriedades.Length; i++)
//            {
//                if (ListaPropriedades[i].PropertyType.FullName == "System.String")
//                {
//                    Registro += ListaPropriedades[i].GetValue(Origem, null).ToString();  //this.GetType().GetProperty(field.Name).GetValue(this, null).ToString();
//                }
//                //else
//                //{
//                //    for (int z = 0; z < ListaPropriedades[i].Length; z++)
//                //    {
//                //        Registro += ListaPropriedades[i].GetValue(Origem, null).ToString();  //this.GetType().GetProperty(field.Name).GetValue(this, null).ToString();
//                //    }
//                //}
//            }
//        }

//        public static void ConverterParaString(ref string Registro, object Origem, string Separador, params string[] ColunasDelimitadas)
//        {
//            PropertyInfo[] ListaPropriedades = Origem.GetType().GetProperties();
//            Registro = "";

//            for (int i = 0; i < ListaPropriedades.Length; i++)
//            {
//                if (ListaPropriedades[i].PropertyType.FullName == "System.String")
//                {
//                    if (Registro == "")
//                    {

//                        if (Array.IndexOf(ColunasDelimitadas, ListaPropriedades[i].Name) > -1)
//                        {
//                            Registro += '"' + ListaPropriedades[i].GetValue(Origem, null).ToString() + '"';
//                        }
//                        else
//                        {
//                            Registro += ListaPropriedades[i].GetValue(Origem, null).ToString();
//                        }
//                    }
//                    else
//                    {
//                        if (Array.IndexOf(ColunasDelimitadas, ListaPropriedades[i].Name) > -1)
//                        {
//                            Registro += Separador + '"' + ListaPropriedades[i].GetValue(Origem, null).ToString() + '"';
//                        }
//                        else
//                        {
//                            Registro += Separador + ListaPropriedades[i].GetValue(Origem, null).ToString();
//                        }
//                    }
//                }
//            }
//        }

//        public static void ConverterParaString(ref string Registro, object Origem, string Separador, bool Cabecalho)
//        {
//            PropertyInfo[] ListaPropriedades = Origem.GetType().GetProperties();
//            Registro = "";
//            if (!Cabecalho) return;

//            for (int i = 0; i < ListaPropriedades.Length; i++)
//            {
//                if (Registro == "")
//                {
//                    Registro += ListaPropriedades[i].Name;
//                }
//                else
//                {
//                    Registro += Separador + ListaPropriedades[i].Name;
//                }
//            }
//        }

//        public static void ConverterParaRegistro(object Origem, string Dados)
//        {
//            int PosAtual = 0;
//            int Tamanho = 0;
//            Dados = Dados.PadRight(Funcoes.TamanhoRegistro(Origem));

//            PropertyInfo[] ListaPropriedades = Origem.GetType().GetProperties();

//            for (int i = 0; i < ListaPropriedades.Length; i++)
//            {
//                Tamanho = ListaPropriedades[i].GetValue(Origem, null).ToString().Length;
//                ListaPropriedades[i].SetValue(Origem, Dados.Substring(PosAtual, Tamanho), null);
//                PosAtual += Tamanho;
//            }
//        }

//        public static byte[] ConverteStringToByte(string str)
//        {
//            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
//            return encoding.GetBytes(str);
//        }

//        public static void LimparClasseInfo(object Origem)
//        {
//            PropertyInfo[] ListaPropriedades = Origem.GetType().GetProperties();
//            for (int i = 0; i < ListaPropriedades.Length; i++)
//            {
//                ListaPropriedades[i].SetValue(Origem, string.Empty, null);
//            }
//        }

//        public static void ConfigurarColunasListView(ref ListView lsvGen, params string[] Colunas)
//        {
//            ColumnHeader ch = null;

//            lsvGen.Items.Clear();
//            lsvGen.Columns.Clear();
//            lsvGen.View = View.Details;
//            lsvGen.FullRowSelect = true;

//            foreach (string coluna in Colunas)
//            {
//                ch = lsvGen.Columns.Add(coluna);
//                ch.Name = coluna;
//            }

//            AjustarColunasListView(ref lsvGen);
//        }

//        public static void ConfigurarColunasListView(ref ListView lsvGen, params ColunasListView[] Colunas)
//        {
//            ColumnHeader ch = null;

//            lsvGen.Items.Clear();
//            lsvGen.Columns.Clear();
//            lsvGen.View = View.Details;
//            lsvGen.FullRowSelect = true;

//            foreach (ColunasListView coluna in Colunas)
//            {
//                ch = new ColumnHeader();
//                ch.Text = coluna.Texto;
//                ch.Name = coluna.Nome;
//                ch.TextAlign = coluna.Alinhamento;
//                lsvGen.Columns.Add(ch);
//            }
//        }

//        public static void ConfigurarDataGridView(ref DataGridView dgvGen)
//        {
//            dgvGen.EnableHeadersVisualStyles = false;
//            dgvGen.ColumnHeadersDefaultCellStyle.ForeColor = SystemColors.ActiveCaptionText;
//            dgvGen.ColumnHeadersDefaultCellStyle.BackColor = SystemColors.ActiveCaption;
//            dgvGen.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
//            dgvGen.ScrollBars = ScrollBars.Both;
//            dgvGen.ShowCellErrors = false;
//            dgvGen.ShowRowErrors = false;
//            dgvGen.CausesValidation = false;
//            dgvGen.MultiSelect = false;
//            dgvGen.SelectionMode = DataGridViewSelectionMode.CellSelect;
//        }

//        public static void ConfigurarColunasDataGridView(ref DataGridView dgvGen, params ColunasGrid[] Colunas)
//        {
//            DataGridViewColumn dgvc;
//            DataGridViewCellStyle dgvcs;
//            try
//            {
//                // configura ggrid
//                ConfigurarDataGridView(ref dgvGen);
//                //geral
//                dgvGen.Columns.Clear();
//                dgvGen.Rows.Clear();

//                foreach (ColunasGrid Coluna in Colunas)
//                {
//                    switch (Coluna.TipoCelula)
//                    {
//                        case eTipoCelula.Texto:
//                            dgvc = new DataGridViewTextBoxColumn();
//                            break;
//                        case eTipoCelula.Combo:
//                            dgvc = new DataGridViewComboBoxColumn();
//                            break;
//                        case eTipoCelula.Botao:
//                            dgvc = new DataGridViewButtonColumn();
//                            break;
//                        case eTipoCelula.Imagem:
//                            dgvc = new DataGridViewImageColumn();
//                            break;
//                        case eTipoCelula.CheckBox:
//                            dgvc = new DataGridViewCheckBoxColumn();
//                            break;
//                        default:
//                            dgvc = new DataGridViewTextBoxColumn();
//                            break;
//                    }

//                    dgvcs = new DataGridViewCellStyle();

//                    dgvcs.Alignment = Coluna.Alinhamento;
//                    dgvcs.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

//                    dgvc.Name = Coluna.Nome.Trim();
//                    dgvc.AutoSizeMode = Coluna.AutoSize;
//                    dgvc.HeaderText = Coluna.Cabecalho.Trim();
//                    dgvc.SortMode = Coluna.Classificacao;
//                    dgvc.ReadOnly = Coluna.Leitura;
//                    dgvc.Visible = Coluna.Visivel;
//                    dgvc.Width = 50;

//                    dgvc.DefaultCellStyle = dgvcs;

//                    dgvGen.Columns.Add(dgvc);
//                }
//            }
//            catch { }
//        }

//        public static void FormatarDadosDataGridView(ref DataGridView dgv, DataSet ds)
//        {
//            int _QtLinhas = ds.Tables[0].Rows.Count;
//            int _QtColunas = dgv.ColumnCount;
//            int _Linha = 0;

//            try
//            {
//                dgv.Rows.Clear();
//                dgv.Rows.Add(_QtLinhas);

//                foreach (DataRow _dr in ds.Tables[0].Rows)
//                {
//                    foreach (DataGridViewColumn _coluna in dgv.Columns)
//                    {
//                        dgv.Rows[_Linha].Cells[_coluna.Name].Value = _dr[_coluna.Name].ToString();
//                    }
//                    _Linha++;
//                }
//            }
//            catch (Exception ex)
//            {
//                Funcoes.MessageBox_Erro(ex);
//            }
//        }

//        public static void AjustarColunasListView(ref ListView lvwListView)
//        {
//            for (int c = 0; c < lvwListView.Columns.Count; c++)
//            {
//                if (lvwListView.Columns[c].Width > 0)
//                {
//                    lvwListView.AutoResizeColumn(c, ColumnHeaderAutoResizeStyle.ColumnContent);
//                    lvwListView.AutoResizeColumn(c, ColumnHeaderAutoResizeStyle.HeaderSize);
//                }
//            }
//        }

//        public static void AjustarColunasListViewCompleta(ref ListView lvwListView)
//        {
//            for (int c = 0; c < lvwListView.Columns.Count; c++)
//            {

//                lvwListView.AutoResizeColumn(c, ColumnHeaderAutoResizeStyle.ColumnContent);
//                lvwListView.AutoResizeColumn(c, ColumnHeaderAutoResizeStyle.HeaderSize);

//            }
//        }

//        public static void PintarItemSelecionado(TreeView genTVW, Color Fonte, Color Fundo)
//        {
//            foreach (TreeNode node in genTVW.Nodes)
//            {
//                if (node.IsSelected)
//                {
//                    node.ForeColor = Fonte;
//                    node.BackColor = Fundo;
//                }
//                else
//                {
//                    if (node.ImageKey == "EP" || node.ImageKey == "SP")
//                    {
//                        node.ForeColor = Color.Blue;
//                        node.BackColor = SystemColors.Window;
//                    }
//                    else
//                    {
//                        node.ForeColor = SystemColors.WindowText;
//                        node.BackColor = SystemColors.Window;
//                    }
//                }

//                TreeNode auxNode = node;
//                PintarItemSelecionado(auxNode, Fonte, Fundo);
//            }
//        }

//        private static void PintarItemSelecionado(TreeNode genNode, Color Fonte, Color Fundo)
//        {
//            foreach (TreeNode node in genNode.Nodes)
//            {
//                if (node.IsSelected)
//                {
//                    node.ForeColor = Fonte;
//                    node.BackColor = Fundo;
//                }
//                else
//                {
//                    if (node.ImageKey == "EP" || node.ImageKey == "SP")
//                    {
//                        node.ForeColor = Color.Blue;
//                        node.BackColor = SystemColors.Window;
//                    }
//                    else
//                    {
//                        node.ForeColor = SystemColors.WindowText;
//                        node.BackColor = SystemColors.Window;
//                    }
//                }

//                TreeNode auxNode = node;
//                PintarItemSelecionado(auxNode, Fonte, Fundo);
//            }
//        }

//        public static void PintarItemSelecionado(ListView genLVW, Color Fonte, Color Fundo)
//        {
//            foreach (ListViewItem lItem in genLVW.Items)
//            {
//                if (lItem.Selected)
//                {
//                    lItem.Font = new Font(lItem.Font, System.Drawing.FontStyle.Bold);
//                    lItem.ForeColor = Fonte;
//                    lItem.BackColor = Fundo;
//                }
//                else
//                {
//                    lItem.Font = new Font(lItem.Font, System.Drawing.FontStyle.Regular);
//                    lItem.ForeColor = SystemColors.WindowText;
//                    lItem.BackColor = SystemColors.Window; ;
//                }
//            }

//            AjustarColunasListView(ref genLVW);
//        }

//        public static void InformacaoStatusBar()
//        {

//            if (Main.FormGauge != null)
//            {
//                Main.FormGauge.Close();
//                //Main.FormGauge.Dispose();
//                Main.FormGauge = null;
//            }
//            InformacaoStatusBar("");

//            Application.DoEvents();

//        }

//        public static void InformacaoStatusBar(string Msg)
//        {
//            InformacaoStatusBar(Msg, false);
//        }

//        public static void InformacaoStatusBar(string Msg, bool ExibeGauge)
//        {
//            Funcoes.UFCursor(true);

//            try
//            {

//                if (Msg == "")
//                {
//                    if (ExibeGauge)
//                    {
//                        if (Main.FormGauge != null)
//                        {
//                            Main.FormGauge.Close();
//                            Main.FormGauge.Dispose();
//                            Main.FormGauge = null;
//                        }
//                    }

//                    if (Main.BarraDeStatus != null)
//                        Main.BarraDeStatus.Items["Mensagem"].Text = "Pronto";

//                    Cursor.Current = Cursors.Default;
//                }
//                else
//                {
//                    if (ExibeGauge)
//                    {
//                        if (Main.FormGauge == null)
//                        {
//                            Form OldForm = Main.MDIOwner.ActiveMdiChild;


//                            //Button btTeste = new Button();
//                            //btTeste.Width = 200;
//                            //btTeste.Height = 300;
//                            //btTeste.Top = 100;
//                            //btTeste.Left = 100;
//                            //btTeste.Text = "Aqui";
//                            //btTeste.BringToFront();
//                            //OldForm.Controls.Add(btTeste);
//                            //OldForm.Refresh();

//                            if (Main.MDIOwner.ActiveMdiChild != null)
//                                if (Main.MDIOwner.ActiveMdiChild.WindowState == FormWindowState.Maximized)
//                                    AjustarTamanhoFormFilhoMDI(Main.MDIOwner.ActiveMdiChild, Main.MDIOwner);

//                            Main.FormGauge = new frmGauge();
//                            Main.FormGauge.MdiParent = Main.MDIOwner;
//                            Main.FormGauge.Text = Msg;
//                            Main.FormGauge.Show();

//                            if (OldForm != null)
//                                OldForm.Refresh();
//                        }
//                    }

//                    if (Main.BarraDeStatus != null)
//                    {
//                        Main.BarraDeStatus.Items["Mensagem"].Text = Msg;

//                        if (Main.FormGauge != null)
//                            Main.FormGauge.Text = Msg;
//                    }
//                }
//                if (Main.BarraDeStatus != null)
//                    Main.BarraDeStatus.Refresh();
//            }
//            catch (Exception ex)
//            {
//                string MsgErr = ex.Message;
//            }

//            Application.DoEvents();

//        }

//        public static void StatusBarAmbiente(string Ambiente)
//        {
//            Main.BarraDeStatus.Items["Ambiente"].Text = Ambiente;
//        }

//        //public static void StartStatusBarMdi(int NumeroMaximo)
//        //{
//        //    if (NumeroMaximo > 0)
//        //    {
//        //        StatusStrip status = (StatusStrip)Main.MDIOwner.Controls[0];
//        //        ToolStripProgressBar progress = (ToolStripProgressBar)status.Items["progress"];

//        //        progress.Maximum = NumeroMaximo;
//        //    }

//        //    if (Main.FormGauge != null)
//        //        Main.FormGauge.Atualizar();
//        //}

//        //public static void StopStatusBarMdi()
//        //{
//        //    StatusStrip status = (StatusStrip)Main.MDIOwner.Controls[0];
//        //    ToolStripProgressBar progress = (ToolStripProgressBar)status.Items["progress"];

//        //    progress.Value = 0;
//        //}

//        public static void AtualizarStatusBarMdi()
//        {
//            AtualizarStatusBarMdi(true);
//        }

//        public static void AtualizarStatusBarMdi(bool AtualizaGauge)
//        {
//            try
//            {
//                //StatusStrip status = (StatusStrip)Main.MDIOwner.Controls[0];
//                //ToolStripProgressBar progress = (ToolStripProgressBar)status.Items["progress"];

//                //if (progress.Value < progress.Maximum)
//                //    progress.Value += 1;

//                if (AtualizaGauge)
//                {
//                    if (Main.FormGauge != null)
//                        Main.FormGauge.Atualizar();
//                }
//            }
//            catch (Exception ex)
//            {
//                string Msg = ex.Message;
//            }
//        }

//        public static void CarregarCboGrupos(ref ComboBox cboGrupo, string QualAmbiente)
//        {
//            try
//            {
//                List<GruposInfo> lista = ObterListaGrupos(QualAmbiente);

//                cboGrupo.ValueMember = "Codigo";
//                cboGrupo.DisplayMember = "Descricao";
//                cboGrupo.DataSource = lista;

//                return;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }

//        }

//        public static void CarregarCboGrupos(ref ToolStripComboBox cboGrupo, string QualAmbiente)
//        {
//            try
//            {
//                List<GruposInfo> lista = ObterListaGrupos(QualAmbiente);

//                cboGrupo.ComboBox.ValueMember = "Codigo";
//                cboGrupo.ComboBox.DisplayMember = "Descricao";
//                cboGrupo.ComboBox.DataSource = lista;

//                return;

//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }

//        }

//        public static List<GruposInfo> ObterListaGrupos(string QualAmbiente)
//        {
//            try
//            {
//                GruposInfo grupos = new GruposInfo();
//                List<GruposInfo> lista = new List<GruposInfo>();

//                grupos = new GruposInfo();
//                grupos.Codigo = (int)Funcoes.FGTipoLinha.Caminhao;
//                grupos.Descricao = "Caminhões";
//                grupos.Sigla = "TR";
//                grupos.LetraAmbiente = "V";
//                lista.Add(grupos);
//                grupos = null;

//                grupos = new GruposInfo();
//                grupos.Codigo = (int)Funcoes.FGTipoLinha.Cabina;
//                grupos.Descricao = "Cabinas";
//                grupos.Sigla = "CB";
//                grupos.LetraAmbiente = "V";
//                lista.Add(grupos);
//                grupos = null;

//                grupos = new GruposInfo();
//                grupos.Codigo = (int)Funcoes.FGTipoLinha.Cambio;
//                grupos.Descricao = "Câmbios";
//                grupos.Sigla = "CA";
//                grupos.LetraAmbiente = "C";
//                lista.Add(grupos);
//                grupos = null;

//                grupos = new GruposInfo();
//                grupos.Codigo = (int)Funcoes.FGTipoLinha.Eixo;
//                grupos.Descricao = "Eixos";
//                grupos.Sigla = "EI";
//                grupos.LetraAmbiente = "E";
//                lista.Add(grupos);
//                grupos = null;

//                grupos = new GruposInfo();
//                grupos.Codigo = (int)Funcoes.FGTipoLinha.Motor;
//                grupos.Descricao = "Motores";
//                grupos.Sigla = "MO";
//                grupos.LetraAmbiente = "M";
//                lista.Add(grupos);
//                grupos = null;

//                if (Funcoes.ObterFabrica(QualAmbiente) == Funcoes.Fabrica.SBC)
//                {
//                    grupos = new GruposInfo();
//                    grupos.Codigo = (int)Funcoes.FGTipoLinha.Onibus;
//                    grupos.Descricao = "Ônibus";
//                    grupos.LetraAmbiente = "V";
//                    grupos.Sigla = "ON";
//                    lista.Add(grupos);
//                    grupos = null;
//                }

//                return lista;

//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }

//        }

//        public static void AjustarTamanhoFormFilhoMDI(string Aplicacao, Form frmChield)
//        {
//            AjustarTamanhoFormFilhoMDI(Aplicacao, frmChield, "");
//        }

//        public static void AjustarTamanhoFormFilhoMDI(string Aplicacao, Form frmChield, string DiferenciadorForm)
//        {
//            int hMenos = 0;
//            int wMenos = 0;

//            string ChaveForm = string.Concat(frmChield.MdiParent.Name, ".", frmChield.Name, DiferenciadorForm);
//            string vWS = LerIni(Aplicacao, string.Concat(ChaveForm, ".WindowState"));
//            string vFBS = LerIni(Aplicacao, string.Concat(ChaveForm, ".FormBorderStyle"));

//            if (frmChield.FormBorderStyle == FormBorderStyle.SizableToolWindow)
//            {
//                hMenos = 150;
//                wMenos = 20;
//            }

//            if (!vWS.Equals("Maximized"))
//            {
//                string vT = LerIni(Aplicacao, string.Concat(ChaveForm, ".Top"));
//                if (vT.Equals(string.Empty)) vT = "0";

//                string vL = LerIni(Aplicacao, string.Concat(ChaveForm, ".Left"));
//                if (vL.Equals(string.Empty)) vL = "0";

//                string vH = LerIni(Aplicacao, string.Concat(ChaveForm, ".Height"));
//                if (vH.Equals(string.Empty)) vH = (frmChield.MdiParent.Height - hMenos).ToString();

//                string vW = LerIni(Aplicacao, string.Concat(ChaveForm, ".Width"));
//                if (vW.Equals(string.Empty)) vW = (frmChield.MdiParent.Width - wMenos).ToString(); ;

//                frmChield.Top = int.Parse(vT);
//                frmChield.Left = int.Parse(vL);
//                frmChield.Height = int.Parse(vH);
//                frmChield.Width = int.Parse(vW);

//                //if (vFBS.Contains("Sizable"))
//                //    frmChield.FormBorderStyle = FormBorderStyle.Sizable;
//                //else if (vFBS.Contains("None"))
//                //    frmChield.FormBorderStyle = FormBorderStyle.None;
//                //else if (vFBS.Contains("SizableToolWindow"))
//                //    frmChield.FormBorderStyle = FormBorderStyle.SizableToolWindow;
//            }
//            else
//            {
//                frmChield.WindowState = FormWindowState.Maximized;
//            }

//            Application.DoEvents();
//        }

//        public static void AjustarTamanhoFormFilhoMDI(Form frmChield, Form mdiParent)
//        {
//            frmChield.Top = 0;
//            frmChield.Left = 0;
//            frmChield.Height = mdiParent.Height - 105;
//            frmChield.Width = mdiParent.Width - 12;
//        }

//        public static double UFMinutoToDouble(MaskedTextBox mascara)
//        {
//            if (mascara.Text.Length < 5) return 0;
//            if (mascara.Text.Replace('_', ' ').Replace(':', ' ').Trim() == "") return 0;

//            return Convert.ToDouble(Funcoes.Mid(mascara.Text, 1, 2)) + Convert.ToDouble(Funcoes.Mid(mascara.Text, 4, 2)) / 60;
//        }

//        public static double UFMinutoToDouble(string ValorEditado)
//        {
//            if (ValorEditado.Length < 5) return 0;
//            if (ValorEditado.Replace('_', ' ').Replace(':', ' ').Trim() == "") return 0;

//            if (ValorEditado.Length == 5) return Convert.ToDouble(Funcoes.Mid(ValorEditado, 1, 2)) + Convert.ToDouble(Funcoes.Mid(ValorEditado, 4, 2)) / 60;
//            if (ValorEditado.Length == 6) return Convert.ToDouble(Funcoes.Mid(ValorEditado, 1, 3)) + Convert.ToDouble(Funcoes.Mid(ValorEditado, 5, 2)) / 60;

//            return 0;

//        }

//        public static string UFFormatarModoTempo(double TempoCentensimal, eModoTempo Modo, int inteiros)
//        {
//            string TempoConvertido = "";

//            if (Modo == eModoTempo.Sexagesimal)
//            {
//                TempoConvertido = Funcoes.UFFormataMinutos(TempoCentensimal);
//            }
//            else
//            {
//                if (inteiros == 0) inteiros = 1;
//                if (inteiros > 6) inteiros = 6;

//                string strFomato = UFLimitarString("0", inteiros, "0");
//                TempoConvertido = TempoCentensimal.ToString(strFomato + ".00000");
//            }

//            return TempoConvertido;

//        }

//        public static string UFFormataMinutos(double minutos)
//        {
//            int iSegundos = (int)Math.Round((decimal)minutos * 60);
//            int iMinutos = (int)iSegundos / 60;
//            iSegundos = (int)iSegundos % 60;

//            return string.Concat(iMinutos.ToString("00"), ":", iSegundos.ToString("00"));
//        }

//        public static void UFCampoDecimal(ref object Campo, ref KeyPressEventArgs e, int Inteiro, int Decimais)
//        {
//            string strCampo = "";
//            int iLen = Inteiro + Decimais + 1;
//            TextBox txtCampo = Campo as TextBox;

//            DigitarSomenteNumeros(ref e);
//            if (e.Handled == true) return;

//            UFCampoTexto(ref Campo, ref e, iLen);
//            if (e.Handled == true) return;

//            txtCampo.Text = txtCampo.Text.Replace(",", "");

//            if (e.KeyChar < 48 || e.KeyChar > 57) return;

//            strCampo = txtCampo.Text;

//            if (strCampo.Length > Decimais)
//            {
//                strCampo = txtCampo.Text + e.KeyChar.ToString();
//                strCampo = txtCampo.Text;
//                txtCampo.Text = Funcoes.Right(strCampo, Decimais);
//                txtCampo.Text = Funcoes.Left(strCampo, (strCampo.Length - Decimais)) + "," + txtCampo.Text;
//            }
//            else
//            {
//                txtCampo.Text = strCampo;
//            }

//            e.Handled = true;
//        }

//        public static bool UFEhNumerico(string Valor)
//        {
//            Int32 iNumero;
//            return Int32.TryParse(Valor, out iNumero);

//        }

//        public static string AlinharDireita(string Valor, int Digitos)
//        {
//            try
//            {
//                return UFLimitarString(" ", Digitos - Valor.Length) + Valor;
//            }
//            catch (Exception)
//            {
//                return Valor;
//            }
//        }

//        public static void UFCampoTexto(ref object sender, ref KeyPressEventArgs e, int Tamanho)
//        {
//            Encoding ascii = Encoding.ASCII;
//            Encoding unicode = Encoding.Unicode;
//            int iTamanhoTexto = 0;
//            int iTamanhoSelTexto = 0;
//            byte[] unicodeBytes;
//            byte[] asciiBytes;

//            if ((Keys)e.KeyChar == Keys.Return) return;
//            if ((Keys)e.KeyChar == Keys.Back) return;
//            if ((Keys)e.KeyChar == Keys.Delete) return;

//            if (sender.GetType() == typeof(TextBox))
//            {
//                TextBox txt = sender as TextBox;
//                iTamanhoTexto = txt.Text.Length;
//                iTamanhoSelTexto = txt.SelectedText.Length;
//            }
//            if (sender.GetType() == typeof(ComboBox))
//            {
//                ComboBox cbo = sender as ComboBox;
//                iTamanhoTexto = cbo.Text.Length;
//                iTamanhoSelTexto = cbo.SelectedText.Length;
//            }

//            if (iTamanhoTexto < Tamanho || iTamanhoSelTexto > 0)
//            {
//                unicodeBytes = unicode.GetBytes(e.KeyChar.ToString().ToUpper());
//                asciiBytes = Encoding.Convert(unicode, ascii, unicodeBytes);
//                e.KeyChar = (char)asciiBytes[0];
//                return;
//            }

//            e.Handled = true;

//        }



//        public static int UFPesquisarValortsCombo(ToolStripComboBox tsCombo, string Texto)
//        {
//            int i = 0;

//            foreach (object item in tsCombo.Items)
//            {
//                if (item.ToString() == Texto)
//                {
//                    return i;
//                }
//                i++;
//            }

//            return -1;
//        }

//        public static bool UFExibeOcultaPanel(SplitContainer sc, int Panel, bool Exibe)
//        {
//            if (Exibe == true)
//            {
//                if (Panel == 1)
//                {
//                    sc.Panel1Collapsed = false;
//                    sc.Panel1.Show();
//                }
//                else
//                {
//                    sc.Panel2Collapsed = false;
//                    sc.Panel2.Show();
//                }
//            }
//            else
//            {
//                if (Panel == 1)
//                {
//                    sc.Panel1Collapsed = true;
//                    sc.Panel1.Hide();
//                }
//                else
//                {
//                    sc.Panel2Collapsed = true;
//                    sc.Panel2.Hide();
//                }
//            }

//            return !Exibe;

//        }

//        public static void MarcarLinhaListView(ListViewItem itemX)
//        {
//            try
//            {
//                if ((itemX.Index % 2) == 0)
//                    itemX.BackColor = Color.White;
//                else
//                    itemX.BackColor = Color.FromArgb(220, 220, 220);
//            }
//            catch { }
//        }

//        # endregion

//        #region ... Bool ...

//        public static bool ImpressoraLocal(string PrinterName)
//        {
//            bool Local = false;
//            string PortName = "";

//            ManagementObjectCollection moReturn = null;
//            ManagementObjectSearcher moSearch = null;

//            moSearch = new ManagementObjectSearcher("Select * from Win32_Printer Where Name = '" + PrinterName + "'");
//            moReturn = moSearch.Get();
//            if (moReturn.Count > 0)
//            {
//                foreach (ManagementObject mo in moReturn)
//                {
//                    PortName = mo["PortName"].ToString();
//                }

//                if ((PortName.ToUpper().Contains("LPT")) || (PortName.ToUpper().Contains("USB")))
//                    Local = true;

//            }
//            else
//                Local = false;

//            return Local;
//        }

//        public static bool LerIni(string sSection, ToolStripMenuItem cControl, Form FormOrigem)
//        {
//            string sKey = "";
//            if (FormOrigem.MdiParent == null)
//                sKey = FormOrigem.Name + "." + cControl.Name;
//            else
//                sKey = FormOrigem.MdiParent.Name + "." + FormOrigem.Name + "." + cControl.Name;

//            string sVal = LerIni(sSection, sKey);
//            if (!sVal.Equals(string.Empty))
//                return Convert.ToBoolean(sVal);
//            else
//                return false;
//        }

//        public static void LerIni(string sSection, ref CheckBox cControl, Form FormOrigem)
//        {
//            cControl.Checked = LerIni(sSection, cControl, FormOrigem);
//        }

//        public static void LerIni(string sSection, ref RadioButton cControl, Form FormOrigem)
//        {
//            cControl.Checked = LerIni(sSection, cControl, FormOrigem);
//        }

//        public static bool LerIni(string sSection, RadioButton cControl, Form FormOrigem)
//        {
//            string sKey = "";
//            if (FormOrigem.MdiParent == null)
//                sKey = FormOrigem.Name + "." + cControl.Name;
//            else
//                sKey = FormOrigem.MdiParent.Name + "." + FormOrigem.Name + "." + cControl.Name;

//            string sVal = LerIni(sSection, sKey);
//            if (!sVal.Equals(string.Empty))
//                return Convert.ToBoolean(sVal);
//            else
//                return false;
//        }

//        public static bool LerIni(string sSection, CheckBox cControl, Form FormOrigem)
//        {
//            string sKey = "";
//            if (FormOrigem.MdiParent == null)
//                sKey = FormOrigem.Name + "." + cControl.Name;
//            else
//                sKey = FormOrigem.MdiParent.Name + "." + FormOrigem.Name + "." + cControl.Name;

//            string sVal = LerIni(sSection, sKey);
//            if (!sVal.Equals(string.Empty))
//                return Convert.ToBoolean(sVal);
//            else
//                return false;
//        }

//        public static void LerIni(string sSection, ref ToolStripButton cControl, Form FormOrigem)
//        {
//            cControl.Checked = LerIni(sSection, cControl, FormOrigem);
//        }

//        public static bool LerIni(string sSection, ToolStripButton cControl, Form FormOrigem)
//        {
//            string sKey = "";
//            if (FormOrigem.MdiParent == null)
//                sKey = FormOrigem.Name + "." + cControl.Name;
//            else
//                sKey = FormOrigem.MdiParent.Name + "." + FormOrigem.Name + "." + cControl.Name;

//            string sVal = LerIni(sSection, sKey);
//            if (!sVal.Equals(string.Empty))
//                return Convert.ToBoolean(sVal);
//            else
//                return false;
//        }

//        public static bool VerificaCamposEmBranco(Control objContainer)
//        {
//            //List<Control> txtList = Funcoes.GetAllControls<TextBox>(objContainer);
//            //List<Control> mskList = Funcoes.GetAllControls<MaskedTextBox>(objContainer);
//            //List<Control> cboList = Funcoes.GetAllControls<ComboBox>(objContainer);
//            //List<Control> scbList = Funcoes.GetAllControls<ToolStripComboBox>(objContainer);

//            bool ExisteVazio = VerificaEmBranco(Funcoes.GetAllControls<TextBox>(objContainer));
//            if (!ExisteVazio) ExisteVazio = VerificaEmBranco(Funcoes.GetAllControls<MaskedTextBox>(objContainer));
//            if (!ExisteVazio) ExisteVazio = VerificaEmBranco(Funcoes.GetAllControls<ComboBox>(objContainer));
//            if (!ExisteVazio) ExisteVazio = VerificaEmBranco(Funcoes.GetAllControls<ToolStripComboBox>(objContainer));
//            if (!ExisteVazio) ExisteVazio = VerificaEmBranco(Funcoes.GetAllControls<ToolStripTextBox>(objContainer));

//            //if (!ExisteVazio)
//            //{
//            //    for (int i = 0; i < scbList.Count; i++)
//            //    {
//            //        ExisteVazio = scbList[i].Text.Trim().Equals(string.Empty);
//            //        if (ExisteVazio)
//            //        {
//            //            scbList[i].Focus();
//            //            break;
//            //        }
//            //    }
//            //}

//            //if (!ExisteVazio)
//            //{
//            //    for (int i = 0; i < mskList.Count; i++)
//            //    {
//            //        ExisteVazio = mskList[i].Text.Trim().Equals(string.Empty);
//            //        if (ExisteVazio)
//            //        {
//            //            mskList[i].Focus();
//            //            break;
//            //        }
//            //    }
//            //}

//            return ExisteVazio;
//        }

//        public static bool VerificaEmBranco(List<Control> controles)
//        {
//            bool ExisteVazio = false;
//            for (int i = 0; i < controles.Count; i++)
//            {
//                ExisteVazio = controles[i].Text.Trim().Equals(string.Empty);
//                if (ExisteVazio)
//                {
//                    MessageBox.Show("Alguma informação obrigatória não foi preenchida. Verifique!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
//                    controles[i].Focus();
//                    break;
//                }
//            }
//            return ExisteVazio;
//        }

//        public static bool UFTrataRetornoMQSeries(int RCMQSeries)
//        {
//            return UFTrataRetornoMQSeries(RCMQSeries, "");
//        }

//        public static bool UFTrataRetornoMQSeries(int RCMQSeries, string FilaEnvio)
//        {
//            return UFTrataRetornoMQSeries(RCMQSeries, FilaEnvio, "");
//        }

//        public static bool UFTrataRetornoMQSeries(int RCMQSeries, string FilaEnvio, string FilaRetorno)
//        {
//            return UFTrataRetornoMQSeries(RCMQSeries, FilaEnvio, FilaRetorno, AutoOperacoes.autoNull);
//        }

//        public static bool UFTrataRetornoMQSeries(int RCMQSeries, string FilaEnvio, string FilaRetorno, AutoOperacoes AutoOperation)
//        {
//            bool UFTrataRetornoMQSeries = true;

//            string strMSG = "";
//            DialogResult Resposta;
//            MessageBoxButtons EstiloMsg;

//            if (RCMQSeries == 33)
//            {
//                UFTrataRetornoMQSeries = false;
//                strMSG = "A solicitação está pendente no servidor." + "\r\n";
//                strMSG = strMSG + "Processamento ainda não concluído.";
//                //VBtoConverter.ScreenCursor = vbDefault;

//                if (AutoOperation == AutoOperacoes.autoNormal)
//                { // default
//                    EstiloMsg = MessageBoxButtons.RetryCancel;
//                }
//                else
//                {
//                    EstiloMsg = MessageBoxButtons.OK;
//                }

//                Resposta = MessageBox.Show(strMSG, "Atenção", EstiloMsg, MessageBoxIcon.Error);
//                if (Resposta == DialogResult.Cancel || (Resposta == DialogResult.OK && AutoOperation == AutoOperacoes.autoCancel))
//                {
//                    if (FilaEnvio != "" && FilaRetorno != "")
//                    {
//                        //Information.Err().Raise(999 + Constants.vbObjectError, null, "Erro. Entrar em contato com o Help Desk reportando as informações: " + "\r\n" + "Fila envio = " + FilaEnvio + " " + "\r\n" + "Fila Retorno =  " + FilaRetorno, null, null);
//                        throw new Exception("Erro. Entrar em contato com o Help Desk reportando as informações: Fila envio = " + FilaEnvio + " - Fila Retorno =  " + FilaRetorno);
//                    }
//                }
//                else
//                {
//                    //VBtoConverter.ScreenCursor = vbHourglass;
//                }
//            }
//            else
//            {
//                if (RCMQSeries != 0)
//                {
//                    //Information.Err().Raise(10 + Constants.vbObjectError, null, "Erro do MQSeries não esperado nº " + RCMQSeries, null, null);
//                    throw new Exception("Erro do MQSeries não esperado nº " + RCMQSeries);
//                }
//            }

//            return UFTrataRetornoMQSeries;
//        }

//        #endregion

//        #region ... PRMApoio ...
//        static public string ObterDescricaoFuncaoApoio(ePRMApoio Funcao)
//        {
//            switch (Funcao)
//            {
//                case ePRMApoio.a_GRUPO_EMISSAO_DOC:
//                    return "Grupo de Emissão de Documento";
//                case ePRMApoio.b_DOC_GRUPO_EMISSAO:
//                    return "Documento de Grupo de Emissão";
//                case ePRMApoio.c_PARAMETROS_EXEC_GPV:
//                    return "Parâmetros de Execução do PRM";
//                case ePRMApoio.d_GRUPO_FZ:
//                    return "Grupo FZ";
//                case ePRMApoio.e_SITUACAO_ORDEM_MB:
//                    return "Situação de Ordem de Montagem Bruta";
//                case ePRMApoio.f_LOCALIZACAO_NP:
//                    return "Localização do Número de Produção";
//                case ePRMApoio.g_MOTIVO_SITUACAO:
//                    return "Motivo de Situação do Número de Produção";
//                case ePRMApoio.h_SITUACAO_NP:
//                    return "Situação do Número de Produção";
//                case ePRMApoio.i_FZ_DISPONIVEL:
//                    return "FZ Disponível";
//                case ePRMApoio.j_CONFIGURACAO_GRID:
//                    return "Configuração do Gride";
//                case ePRMApoio.k_SITUACAO_ORDEM_PI:
//                    return "Situação da Ordem de Pintura";
//                case ePRMApoio.l_ANTEC_AREA_PRODUCAO:
//                    return "Antecedência da Área de Produção";
//                case ePRMApoio.m_CODIGO_ESPINHA:
//                    return "Código Espinha";
//                case ePRMApoio.n_HISTORICO_NP:
//                    return "Gerador de Histórico do Número de Produção";
//                case ePRMApoio.o_LOCAL_EVENTO:
//                    return "Local do Evento";
//                case ePRMApoio.p_PROCESS_CONTR_EVENTO:
//                    return "Processo do Controle de Eventos";
//                case ePRMApoio.q_MODELO_ANO_VIN:
//                    return "Validade de Code para Cálculo VIN";
//                case ePRMApoio.r_PAIS_ANO_VIN:
//                    return "Restrições de Code para Cálculo VIN";
//                case ePRMApoio.s_CONTROLE_ACESSO:
//                    return "Funcionalidades para Controle de Acesso";
//                case ePRMApoio.t_ADM_PATIO:
//                    return "Administração de Pátio";
//                case ePRMApoio.z_PRMApoio:
//                    return "PRM Apoio";
//                default:
//                    return "Não identificada";
//            }
//        }

//        public static object Pesquisar(string query, string ambiente, object PRMObject)
//        {
//            try
//            {
//                DataTable dt = new Conexao(ambiente).ObterDataTable(query);

//                PropertyInfo[] properties = PRMObject.GetType().GetProperties();

//                foreach (PropertyInfo pr in properties)
//                {
//                    if (dt.Columns.Contains(pr.Name))
//                    {
//                        if (dt.Rows.Count > 0)
//                        {
//                            pr.SetValue(PRMObject, dt.Rows[0][pr.Name].ToString().Trim());
//                        }
//                        else
//                        {
//                            pr.SetValue(PRMObject, "");
//                        }
//                    }
//                }

//                return PRMObject;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }

//        public static DataTable PegarInformacaoCombo(string query, string ambiente)
//        {
//            return new Conexao(ambiente).ObterDataTable(query);
//        }

//        public static string ObterDescricaoSequencia(int COSQPR, string NOQUEU)
//        {
//            string Query = "Select DESQPR From TLO Where COSQPR = " + COSQPR.ToString();
//            DataTable dt = new Conexao(NOQUEU).ExecutaSQL(Query).Tables[0];
//            if (dt.Rows.Count > 0)
//                return dt.Rows[0]["desqpr"].ToString().Trim();
//            else
//                return "";
//        }

//        public static void LinkMQSeries(object PRMObject, int tamanhoDados, string METODO, string ambiente, string fila, string programa)
//        {
//            string strDados = new string(' ', tamanhoDados);

//            try
//            {
//                strDados = METODO;
//                ConverterParaString(ref strDados, PRMObject);

//                (new MonitorMQ(ambiente)).ProcessarMensagem(strDados, fila, true, programa);
//            }
//            catch (Exception ex)
//            {
//                Funcoes.GravaLog(ex, "LinkMQSeries.txt", "Erro ao invocar o método de comunicação com o MQ");
//            }
//        }

//        #endregion

//        #region ... PRMPrint ...
//        public static void VerificarTHK_Posto(string CODOCU, ref string COLIMO, ref string COSQPR, ref string COPREM, ref string NUMVER, ref string COGRUP, ref bool ListarPecas, string NOQUEU)
//        {
//            string Query = "";
//            DataSet ds;

//            ListarPecas = false;

//            try
//            {
//                //Captura configurações para Busca de Peças do Posto
//                ds = null;
//                Query = "";
//                Query += " SELECT ";
//                Query += "   NOVIEW, NOCOLU";
//                Query += " FROM ";
//                Query += "   THK";
//                Query += " WHERE ";
//                Query += "   NOVIEW LIKE 'ETIQUETA_" + CODOCU.Trim() + "%'";

//                ds = new Conexao(NOQUEU).ExecutaSQL(Query);

//                if (ds.Tables[0].Rows.Count > 0)
//                {
//                    string[] parametros = ds.Tables[0].Rows[0]["NOVIEW"].ToString().Trim().Split('_');

//                    if (parametros.Length >= 6)
//                    {
//                        COLIMO = parametros[2];
//                        COPREM = parametros[3];
//                        NUMVER = parametros[4];
//                        COGRUP = parametros[5];

//                        try
//                        {
//                            COSQPR = parametros[6];
//                        }
//                        catch (Exception)
//                        {
//                            COSQPR = "000000";
//                        }
//                        //Captura Código Numérico da Linha
//                        if (COSQPR == "000000")
//                        {
//                            ds = null;
//                            Query = "";
//                            Query += " SELECT ";
//                            Query += "   COSQPR";
//                            Query += " FROM ";
//                            Query += "   TLO";
//                            Query += " WHERE ";
//                            Query += "   COLIMO = '" + COLIMO + "'";

//                            ds = new Conexao(NOQUEU).ExecutaSQL(Query);

//                            if (ds.Tables[0].Rows.Count > 0)
//                            {
//                                COSQPR = ds.Tables[0].Rows[0]["COSQPR"].ToString().Trim();
//                            }
//                        }

//                        ListarPecas = true;
//                    }
//                }

//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }
//        #endregion

//        public static void AjustarDataSet_OC8(DataSet lista, bool Veiculo)
//        {
//            lista.Tables[0].Columns.Add("QTEXTO");

//            for (int L = 0; L < lista.Tables[0].Rows.Count; L++)
//            {
//                string TextoResultado = lista.Tables[0].Rows[L]["DQUERY"].ToString().Trim();

//                //if (Veiculo && (GruItem == "FSA" || GruItem == "FSV"))
//                TextoResultado = TextoResultado.Replace(QueryPadraoVeic, "").Replace(QueryPadraoVeic_old, "");

//                //else
//                TextoResultado = TextoResultado.Replace(QueryPadrao, "").Replace(QueryPadrao_old, "");

//                string Retirar = "";

//                string[] Pecas = ObterPecas(TextoResultado);

//                for (int i = 0; i < Pecas.Length; i++)
//                {
//                    if (i == 0)
//                        Retirar += "'" + Pecas[i] + "'";
//                    else
//                        Retirar += ",'" + Pecas[i] + "'";

//                }

//                //if (Pecas.Length > 2)
//                //    TextoResultado = TextoResultado.Replace(Retirar, "");

//                TextoResultado = TextoResultado.Replace(") > 0)", "");

//                TextoResultado = TextoResultado.Replace(" and numcod in (" + Retirar + ") and (", "");
//                TextoResultado = TextoResultado.Replace("((and numcod = ", "");

//                TextoResultado = TextoResultado.Replace(" and numesa in (" + Retirar + ") and (", "");
//                TextoResultado = TextoResultado.Replace("((and numesa = ", "");

//                //Fazendo Pcinv depois de tudo pronto
//                for (int z = 0; z < Pecas.Length; z++)
//                {
//                    try
//                    {
//                        TextoResultado = TextoResultado.Replace("'" + Pecas[z] + "'", "'" + Funcoes.PCInv4_To_PCInv3(Pecas[z]) + "'");
//                    }
//                    catch
//                    {
//                    }
//                }


//                lista.Tables[0].Rows[L]["QTEXTO"] = TextoResultado.Trim() + " ";
//            }
//            lista.Tables[0].AcceptChanges();
//        }

//        public static string[] ObterPecas(string Texto)
//        {
//            string input = (Texto);
//            var output = String.Join(";", Regex.Matches(input, @"\'(.+?)\'")
//                                                .Cast<Match>()
//                                                .Select(m => m.Groups[1].Value));

//            string[] Pecas = output.Split(';');
//            string txtPeca = "";
//            for (int i = 0; i < Pecas.Length; i++)
//            {
//                if (!txtPeca.Contains(Pecas[i]))
//                {
//                    if (i == 0)
//                        txtPeca += Pecas[i];
//                    else
//                        txtPeca += ";" + Pecas[i];
//                }
//            }

//            string[] newPecas = txtPeca.Split(';');

//            return newPecas;
//        }

//        public static DataTable ObterParametrosLinhas_THK(string Ambiente)
//        {
//            Ambiente = Ambiente.Substring(0, 3) + "1";
//            return new Conexao(Ambiente).ExecutaSQL("Select NUPOGR as COSQPR From THK Where Tiutil = '   ' and NOVIEW = 'PRMSEQLOTE.LINHAESP'").Tables[0];
//        }

//        public static DataTable ObterOrdemLocalizacoes_COLOSQ(string Ambiente)
//        {
//            Ambiente = Ambiente.Substring(0, 3) + "1";
//            return new Conexao(Ambiente).ExecutaSQL("select * from tls").Tables[0];
//        }

//        #region ... Historico ...
//        public static DataSet ListarHistoricoTabela(string Ambiente, string Tabela, params string[] Valores)
//        {
//            string strSql = "";
//            string[] strColunas;
//            DataSet dsColunas = new DataSet();
//            DataSet dsDados = new DataSet();
//            DataSet dsRetorno = new DataSet();
//            Int16 i = 0;

//            strSql += " SELECT COLUMN_POSITION, COLUMN_NAME ";
//            strSql += " FROM ALL_IND_COLUMNS ";
//            strSql += " WHERE TABLE_NAME    = " + Apoio.FormatarVariaveisHost(0);
//            strSql += "   AND INDEX_NAME LIKE 'PK%'";
//            strSql += " ORDER BY COLUMN_POSITION";

//            try
//            {
//                dsColunas = Apoio.ExecutarComParametros(strSql, Ambiente, Trim(Tabela));

//                if (dsColunas.Tables[0].Rows.Count == 0)
//                {
//                    new Exception("Chave da tabela não encontar. Sem histórico para exibir!");
//                }

//                strColunas = new string[dsColunas.Tables[0].Rows.Count];

//                foreach (DataRow dr in dsColunas.Tables[0].Rows)
//                {
//                    strColunas[i] = dr["COLUMN_NAME"].ToString();
//                    i++;
//                }
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }

//            try
//            {
//                strSql = "";
//                strSql += " SELECT NUDOCT, DAINCL, COUSIN, COTEIN, DAATUA, COUSAT, COTEAT, SIAREA ";
//                strSql += " FROM " + Tabela + " ";
//                strSql += Apoio.FormatarClausulaWHERE(strColunas);

//                dsDados = Apoio.ExecutarComParametros(strSql, Ambiente, Valores);

//                dsRetorno.Tables.Add(dsColunas.Tables[0].Copy());
//                dsRetorno.Tables[0].TableName = "Colunas";
//                dsRetorno.Tables.Add(dsDados.Tables[0].Copy());
//                dsRetorno.Tables[1].TableName = "Dados";

//                return dsRetorno;
//            }
//            catch (Exception ex1)
//            {
//                throw ex1;
//            }
//        }

//        #endregion
//    }

//    public class ListViewItemComparer : IComparer
//    {
//        private int coluna;
//        private SortOrder order;
//        public ListViewItemComparer()
//        {
//            coluna = 0;
//            order = SortOrder.Ascending;
//        }
//        public ListViewItemComparer(int column, SortOrder order)
//        {
//            coluna = column;
//            this.order = order;
//        }
//        public int Compare(object x, object y)
//        {
//            int returnVal = -1;

//            returnVal = String.Compare(((ListViewItem)x).SubItems[coluna].Text,
//                                    ((ListViewItem)y).SubItems[coluna].Text);
//            // Determine whether the sort order is descending.
//            if (order == SortOrder.Descending)
//                // Invert the value returned by String.Compare.
//                returnVal *= -1;


//            return returnVal;
//        }
//    }

//    public class Criptografia
//    {
//        public static string Criptografar(string Message)
//        {
//            string senha = "qawszedxrfctgvyhbujnikmol,;:[{]}QAZWSXEDCRFVTGBYHNUJOLPÇ1 !2@3)4M5IK67*8(90_-=+";

//            byte[] Results;
//            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();
//            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
//            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(senha));
//            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();
//            TDESAlgorithm.Key = TDESKey;
//            TDESAlgorithm.Mode = CipherMode.ECB;
//            TDESAlgorithm.Padding = PaddingMode.PKCS7;
//            byte[] DataToEncrypt = UTF8.GetBytes(Message);
//            try
//            {
//                ICryptoTransform Encryptor = TDESAlgorithm.CreateEncryptor();
//                Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);
//            }
//            finally
//            {
//                TDESAlgorithm.Clear();
//                HashProvider.Clear();
//            }
//            return Convert.ToBase64String(Results);
//        }

//        public static string Descriptografar(string Message)
//        {
//            string senha = "qawszedxrfctgvyhbujnikmol,;:[{]}QAZWSXEDCRFVTGBYHNUJOLPÇ1 !2@3)4M5IK67*8(90_-=+";

//            byte[] Results;
//            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();
//            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
//            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(senha));
//            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();
//            TDESAlgorithm.Key = TDESKey;
//            TDESAlgorithm.Mode = CipherMode.ECB;
//            TDESAlgorithm.Padding = PaddingMode.PKCS7;
//            byte[] DataToDecrypt = Convert.FromBase64String(Message);
//            try
//            {
//                ICryptoTransform Decryptor = TDESAlgorithm.CreateDecryptor();
//                Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);
//            }
//            finally
//            {
//                TDESAlgorithm.Clear();
//                HashProvider.Clear();
//            }
//            return UTF8.GetString(Results);
//        }
//    }

//    public class Log
//    {
//        public static DataTable dtPermiteLog = null;

//        public static void GravarLog(string Ambiente, string Coserv, Exception ex)
//        {
//            string Detalhe = "";

//            if (ex.Message.ToString().Trim() != "")
//                Detalhe += string.Concat("Message: ", ex.Message.ToString().Trim());

//            if (ex.Source.ToString().Trim() != "")
//                Detalhe += string.Concat("Source: ", ex.Source.ToString().Trim());

//            if (ex.StackTrace.ToString().Trim() != "")
//                Detalhe += string.Concat(Environment.NewLine, "Trace: ", ex.StackTrace.ToString().Trim());

//            if (ex.TargetSite.ToString().Trim() != "")
//                Detalhe += string.Concat(Environment.NewLine, "Target: ", ex.TargetSite.ToString().Trim());

//            if (ex.InnerException.ToString().Trim() != "")
//                Detalhe += string.Concat(Environment.NewLine, "InnerException: ", ex.InnerException.ToString().Trim());

//            GravarLog(Ambiente, Coserv, "000000000", ex.Message, Detalhe);
//        }

//        public static void GravarLog(string Ambiente, string Coserv, string Message)
//        {
//            GravarLog(Ambiente, Coserv, "000000000", Message, "");
//        }

//        public static void GravarLog(string Ambiente, string Coserv, string Message, string Detalhe)
//        {
//            GravarLog(Ambiente, Coserv, "000000000", Message, Detalhe);
//        }

//        public static void GravarLog(string Ambiente, string Coserv, string Produto, string Message, string Detalhe)
//        {
//            Negocio.Gestao.clsExcecao obj = new Negocio.Gestao.clsExcecao();

//            try
//            {
//                obj.NUEXCE = 0;
//                obj.COUSIN = AmbienteInfo._ChaveUsuario;
//                obj.COTEIN = AmbienteInfo._LUR;
//                obj.SIAREA = AmbienteInfo._SiglaArea;
//                obj.NUDOCT = Coserv.Substring(3).ToUpper();
//                obj.DEEXCE = Message;
//                obj.NUPROD = Produto;
//                obj.NUSEQM = 0;
//                obj.NUSEMF = 0;
//                obj.NUMMDS = "";
//                obj.COTRAN = Coserv.Substring(3).ToUpper();
//                obj.DEPARM = Detalhe;

//                obj.IncluirLog(Ambiente, Coserv);
//            }
//            catch
//            {
//            }
//            finally
//            {
//                obj = null;
//            }
//        }

//        public static void GravarXog(string Grupo, string Ambiente, string Coserv, string Inventario, string Message)
//        {
//            Negocio.Gestao.clsExcecao obj = new Negocio.Gestao.clsExcecao();

//            try
//            {
//                if (Inventario.Trim() == "")
//                    Inventario = Funcoes.ObterTerminal();

//                if (dtPermiteLog == null)
//                    dtPermiteLog = new Conexao(Ambiente).ExecutaSQL("Select * from THK Where Tiutil = '   ' AND NOVIEW = 'XOG_" + Inventario + "'").Tables[0];

//                if (dtPermiteLog.Rows.Count > 0)
//                {
//                    obj.NUEXCE = 0;
//                    obj.COUSIN = AmbienteInfo._ChaveUsuario;
//                    obj.COTEIN = Inventario;
//                    obj.SIAREA = AmbienteInfo._SiglaArea;
//                    obj.NUDOCT = Coserv.Substring(3).ToUpper();
//                    obj.DEEXCE = Message;
//                    obj.NUPROD = "000000000";
//                    obj.NUSEQM = 0;
//                    obj.NUSEMF = 0;
//                    obj.NUMMDS = "";
//                    obj.COTRAN = Coserv.Substring(3).ToUpper();
//                    obj.DEPARM = "";

//                    obj.IncluirXog(Ambiente, Coserv);
//                }
//            }
//            catch
//            {
//            }
//            finally
//            {
//                obj = null;
//            }
//        }
//    }
//}