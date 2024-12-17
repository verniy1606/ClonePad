using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Reflection;

namespace ClonePad {
    public partial class MainForm : Form {
        // private readonly Notepad _notepad; // 将来的にはUIと処理を分離させたい

        private readonly string _appName = $"{Application.ProductName}";
        private readonly string _fileFilter =
            "Text Documents (*.txt)|*.txt;*.text" + "|" +
            "All Files (*.*)|*.*";

        private string _title = "Untitled"; // Path
        private string _savedText = string.Empty;
        private bool _isSaved = true;

        public MainForm() {
            InitializeComponent();

            // _notepad = new Notepad();
        }

        private void MainForm_Load(object sender, EventArgs e) {
            updateTitle();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
            if (_isSaved) return;

            DialogResult result = MessageBox.Show(
                $"Do you want to save changes to {_title}?",
                _appName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

            if (result == DialogResult.Yes) {
                if (saveAs()) return;
            } else if (result == DialogResult.No) {
                return;
            }

            e.Cancel = true;
        }

        private void updateTitle() {
            if (_isSaved)
                this.Text = $"{_title} - {_appName}";
            else
                this.Text = $"*{_title} - {_appName}";
        }

        private void saveDocument(string path) {
            _title = path;
            saveDocument();
        }

        private void saveDocument() {
            _isSaved = true;
            _savedText = textBox.Text;
            updateTitle();
        }

        private bool saveAs() {
            SaveFileDialog saveFileDialog = new SaveFileDialog {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                FileName = @"*.txt",
                Filter = _fileFilter,
                FilterIndex = 1
            };

            DialogResult result = saveFileDialog.ShowDialog();

            if (result == DialogResult.OK) {
                string path = saveFileDialog.FileName;

                File.WriteAllText(path, textBox.Text);
                saveDocument(path);

                return true;
            }

            return false;
        }

        private void textBox_TextChanged(object sender, EventArgs e) {
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);

            bool isSaved = (textBox.Text == _savedText);

            if (_isSaved == isSaved) return;

            _isSaved = isSaved;
            Debug.WriteLine($"_isSaved is {_isSaved}");

            updateTitle();
        }

        private void mnNew_Click(object sender, EventArgs e) {
        }

        private void mnNewWindow_Click(object sender, EventArgs e) {
            Process.Start(Application.ExecutablePath);
        }

        private void mnOpen_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Filter = _fileFilter,
                FilterIndex = 1
            };

            DialogResult result = openFileDialog.ShowDialog();

            if (result == DialogResult.OK) {
                string path = openFileDialog.FileName;

                textBox.Text = File.ReadAllText(path);
                saveDocument(path);
            }
        }

        private void mnSave_Click(object sender, EventArgs e) {
            if (!File.Exists(_title)) {
                Debug.WriteLine(
                    $"{MethodBase.GetCurrentMethod().Name}: Path '{_title}' does not exist. We'll open SaveAs dialog instead."
                );
                saveAs();
                return;
            }

            File.WriteAllText(_title, textBox.Text);
            saveDocument();
        }

        private void mnSaveAs_Click(object sender, EventArgs e) {
            saveAs();
        }

        private void mnExit_Click(object sender, EventArgs e) {
            this.Close();
        }
    }
}
