using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Test_Editor
{
    public partial class Form1 : Form
    {
        private const string defaultFont = "Calibri";
        private const int fontsDefaulSize = 14;
        private int countPages = 0;
        private Font fontsSelector;
        public Form1()
        {
            InitializeComponent();
        }
        public RichTextBox getCurrentDoc
        {
            get { return (RichTextBox)tabControl1.SelectedTab.Controls["Page"]; }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.fontsSelector = GetDefaultFont();
            tabControl1.ContextMenuStrip = contextMenuStrip1;
            AddPage();
            GetFontCollection();
            FontSize();
        }
        private void AddPage()
        {
            RichTextBox newpage = new RichTextBox
            {//текстовое поле где писать
                Name = "Page",
                AcceptsTab = true,
                Dock = DockStyle.Fill,
                ContextMenuStrip = contextMenuStrip1,
                Font = this.fontsSelector
            };
            this.countPages++; // счетчик страниц 
            string pageName = "Страница - " + this.countPages; //заготовка имени страницы
            TabPage newPage = new TabPage// страничка = новый файл 
            {
                Name = pageName,
                Text = pageName
            };
            newPage.Controls.Add(newpage); // добавлет текстовое поле в страницу
            tabControl1.TabPages.Add(newPage);  // добавлят страницу в табконтрол
        }
        private void DeletePages()
        {
            if (tabControl1.TabPages.Count != 1)
            {
                tabControl1.TabPages.Remove(tabControl1.SelectedTab);
            }
            else
            {
                tabControl1.TabPages.Remove(tabControl1.SelectedTab);
                this.countPages = 0;
                AddPage();
            }
        }
        private void DeleteAllPages()
        {
            foreach (TabPage Page in tabControl1.TabPages)
            {
                tabControl1.TabPages.Remove(Page);
            }
            this.countPages = 0;
            AddPage();
        }
        // здесь будут еще методы по контролю численности папок табконтроля

        //конец отступа
        private void Open()
        {
            ofdNew.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            ofdNew.Filter = "RTF Document (RTF)|*.rtf";
            if (ofdNew.ShowDialog() == DialogResult.OK)
            {
                if (ofdNew.FileName.Length > 0)
                {
                    try
                    {
                        AddPage();
                        tabControl1.SelectedTab = tabControl1.TabPages["Страница - " + this.countPages];
                        getCurrentDoc.LoadFile(ofdNew.FileName, RichTextBoxStreamType.RichText);
                        string numbArchiv = Path.GetFileName(ofdNew.FileName);
                        tabControl1.SelectedTab.Text = numbArchiv;
                        tabControl1.SelectedTab.Name = numbArchiv;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void SaveDoc()
        {
            saveFDDoc.FileName = tabControl1.SelectedTab.Name;
            saveFDDoc.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            saveFDDoc.Filter = "RTF Document (RTF)|*.rtf";
            saveFDDoc.Title = "Сохранить документ!";
            if (saveFDDoc.ShowDialog() == DialogResult.OK)
            {
                if (saveFDDoc.FileName.Length > 0)
                {
                    try
                    {
                        getCurrentDoc.SaveFile(saveFDDoc.FileName, RichTextBoxStreamType.RichText);
                        string numbArchiv = Path.GetFileName(saveFDDoc.FileName);
                        tabControl1.SelectedTab.Text = numbArchiv;
                        tabControl1.SelectedTab.Name = numbArchiv;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        #region TextEdit
        private void Undo()
        {
            getCurrentDoc.Undo();
        }
        private void Redo()
        {
            getCurrentDoc.Redo();
        }
        private void Cuttext()
        {
            getCurrentDoc.Cut();
        }
        private void Copytext()
        {
            getCurrentDoc.Copy();
        }
        private void Pastetext()
        {
            getCurrentDoc.Paste();
        }
        private void selectall()
        {
            getCurrentDoc.SelectAll();
        }

        #endregion

        #region general methods
        private void создатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddPage();
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Open();
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveDoc();
        }
        private void закрытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }
        private void отменаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Undo();
        }
        private void вырезатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cuttext();
        }
        private void копироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Copytext();
        }
        private void вставитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Pastetext();
        }
        private void выбратьВсеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selectall();
        }
        #endregion
        #region fonts

        private void GetFontCollection()
        {
            InstalledFontCollection Fonts = new InstalledFontCollection();
            foreach (FontFamily font in Fonts.Families)
            {
                fontCollectionComboBox.Items.Add(font.Name);
            }
            fontCollectionComboBox.SelectedIndex = fontCollectionComboBox.FindString(defaultFont);
        }
        private void FontSize()
        {
            for (int i = 0; i <= fontCollectionComboBox.Items.Count; i++)
            {
                fontSizeBtn.Items.Add(i);
            }
            fontSizeBtn.SelectedIndex = fontsDefaulSize;
        }
        private Font GetDefaultFont()
        {
            return new Font(defaultFont, fontsDefaulSize, FontStyle.Regular);
        }
        #endregion

        private void Negrila_Click(object sender, EventArgs e)
        {
            Font regularFont = new Font(getCurrentDoc.SelectionFont.FontFamily,
                getCurrentDoc.SelectionFont.SizeInPoints, FontStyle.Regular);

            Font NegrilaFont = new Font(getCurrentDoc.SelectionFont.FontFamily,
                getCurrentDoc.SelectionFont.SizeInPoints, FontStyle.Bold);
            if (getCurrentDoc.SelectionFont.Bold)
            {
                getCurrentDoc.SelectionFont = regularFont;
            }
            else
            {
                getCurrentDoc.SelectionFont = NegrilaFont;
            }
        }

        private void Italicbutton_Click(object sender, EventArgs e)
        {
            Font regularFont = new Font(getCurrentDoc.SelectionFont.FontFamily,
                getCurrentDoc.SelectionFont.SizeInPoints, FontStyle.Regular);
            Font ItalicFont = new Font(getCurrentDoc.SelectionFont.FontFamily,
                getCurrentDoc.SelectionFont.SizeInPoints, FontStyle.Italic);
            if (getCurrentDoc.SelectionFont.Italic)
            {
                getCurrentDoc.SelectionFont = regularFont;
            }
            else
            {
                getCurrentDoc.SelectionFont = ItalicFont;
            }
        }

        private void UnderlineButton_Click(object sender, EventArgs e)
        {
            Font regularFont = new Font(getCurrentDoc.SelectionFont.FontFamily,
                            getCurrentDoc.SelectionFont.SizeInPoints, FontStyle.Regular);
            Font underline = new Font(getCurrentDoc.SelectionFont.FontFamily,
                getCurrentDoc.SelectionFont.SizeInPoints, FontStyle.Underline);
            if (getCurrentDoc.SelectionFont.Underline)
            {
                getCurrentDoc.SelectionFont = regularFont;
            }
            else
            {
                getCurrentDoc.SelectionFont = underline;
            }
        }

        private void StrikeoutButton_Click(object sender, EventArgs e)
        {
            Font regularFont = new Font(getCurrentDoc.SelectionFont.FontFamily,
                            getCurrentDoc.SelectionFont.SizeInPoints, FontStyle.Regular);
            Font strickeout = new Font(getCurrentDoc.SelectionFont.FontFamily,
                getCurrentDoc.SelectionFont.SizeInPoints, FontStyle.Strikeout);
            if (getCurrentDoc.SelectionFont.Strikeout)
            {
                getCurrentDoc.SelectionFont = regularFont;
            }
            else
            {
                getCurrentDoc.SelectionFont = strickeout;
            }
        }

        private void LeftAlignmentbtn_Click(object sender, EventArgs e)
        {
            getCurrentDoc.SelectionAlignment = HorizontalAlignment.Left;
        }

        private void centerAlignmentBtn_Click(object sender, EventArgs e)
        {
            getCurrentDoc.SelectionAlignment = HorizontalAlignment.Center;
        }

        private void rightAlignmentBtn_Click(object sender, EventArgs e)
        {
            getCurrentDoc.SelectionAlignment = HorizontalAlignment.Right;
        }

        private void delParagraphBtn_Click(object sender, EventArgs e)
        {
            if (getCurrentDoc.SelectionIndent != 0)
            {
                getCurrentDoc.SelectionIndent -= 25;
            }
        }

        private void addParagraphBtn_Click(object sender, EventArgs e)
        {
            getCurrentDoc.SelectionIndent += 25;
        }

        private void selectionBulletBtn_Click(object sender, EventArgs e)
        {
            getCurrentDoc.SelectionBullet = !getCurrentDoc.SelectionBullet;
        }

        private void toUpperBtn_Click(object sender, EventArgs e)
        {
            getCurrentDoc.SelectedText = getCurrentDoc.SelectedText.ToUpper();
        }

        private void toLowerBtn_Click(object sender, EventArgs e)
        {
            getCurrentDoc.SelectedText = getCurrentDoc.SelectedText.ToLower();
        }

        private void incrementFontBtn_Click(object sender, EventArgs e)
        {
            float NewFont = getCurrentDoc.SelectionFont.SizeInPoints + 2;
            Font newFont = new Font(getCurrentDoc.SelectionFont.Name,NewFont, getCurrentDoc.SelectionFont.Style);
            this.fontsSelector = newFont;
            getCurrentDoc.SelectionFont = newFont;
        }

        private void decrementFontBtn_Click(object sender, EventArgs e)
        {
            float NewDecremFont = getCurrentDoc.SelectionFont.SizeInPoints - 2;
            Font NewFont = new Font(getCurrentDoc.SelectionFont.Name, NewDecremFont, getCurrentDoc.SelectionFont.Style);
            this.fontsSelector = NewFont;
            getCurrentDoc.SelectionFont = NewFont;
        }

        private void fontColorBtn_Click(object sender, EventArgs e)
        {
            if(colorDialog1.ShowDialog() == DialogResult.OK)
            {
                getCurrentDoc.SelectionColor = colorDialog1.Color;
            }
        }

        private void YellowBtn_Click(object sender, EventArgs e)
        {
            getCurrentDoc.SelectionBackColor = Color.Yellow;
        }

        private void limeBtn_Click(object sender, EventArgs e)
        {
            getCurrentDoc.SelectionBackColor = Color.Lime;
        }

        private void turquoiseBtn_Click(object sender, EventArgs e)
        {
            getCurrentDoc.SelectionBackColor = Color.Turquoise;
        }

        private void fontCollectionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Font NewFontFromCollection = new Font(
                fontCollectionComboBox.SelectedItem.ToString(),
                getCurrentDoc.SelectionFont.Size,
                getCurrentDoc.SelectionFont.Style);
            this.fontsSelector = NewFontFromCollection;
            getCurrentDoc.SelectionFont = NewFontFromCollection;
        }

        private void fontSizeBtn_SelectedIndexChanged(object sender, EventArgs e)
        {
            float NewSize;
            float.TryParse(fontSizeBtn.SelectedItem.ToString(), out NewSize);
            Font NewFont = new Font(getCurrentDoc.SelectionFont.Name,NewSize,getCurrentDoc.SelectionFont.Style);
            this.fontsSelector = NewFont;
            getCurrentDoc.SelectionFont = NewFont;
        }

        private void deleteTab_Click(object sender, EventArgs e)
        {
            DeletePages();
        }
    }
}
