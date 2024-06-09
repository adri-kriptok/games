using Kriptok.Extensions;
using Kriptok.JAEditor.Data;
using Kriptok.JAEditor.Forms;
using Kriptok.Sdk.Tools.Extensions;
using Kriptok.Sdk.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kriptok.JAEditor
{
    public partial class MdiForm : Form
    {
        private byte[] bytes;
        private string fileName;
        private Merc[] mercs;

        public MdiForm(string[] args)
        {
            InitializeComponent();

            FormHelper.InitMainForm(this, "Jagged Alliance Save Editor");

            FormHelper.TryOpenFile(args, "Jagged Alliance files|*.SAV", fileName =>
            {
                Load(fileName, File.ReadAllBytes(fileName));
            }, () =>
            {
                // No se seleccionó archivo.
            }, () =>
            {
                // Canceló el diálogo.
            });
        }

        private void Load(string fileName, byte[] bytes)
        {
            const int initMercs = 28;
            const int lengthMerc = 32 * 16;

            this.fileName = fileName;
            this.bytes = bytes;
            var saveName = string.Join(string.Empty, Encoding.ASCII.GetChars(bytes.Skip(2).Take(24).ToArray()).Select(p => p.ToString()));

            var mercList = new List<Merc>();

            for (int i = 0; i < 8; i++)
            {
                var location = initMercs + i * lengthMerc;

                var merc = new Merc(location, bytes.Skip(location).Take(lengthMerc).ToArray());

                mercList.Add(merc);

                if (!string.IsNullOrWhiteSpace(merc.Name))
                {
                    var form = new MercForm(merc)
                    {
                        MdiParent = this
                    };

                    form.Show();
                }
            }

            this.mercs = mercList.ToArray();

            LoadFoundsCombo(bytes);
        }

        private void LoadFoundsCombo(byte[] bytes)
        {
            var money0 = bytes[20720];
            var money1 = bytes[20721];
            var money2 = bytes[20722];
            var founds = (money2 << 16) | (money1 << 8) | money0;
            var maxFounds = (11 << 16);

            foundsToolStripComboBox.Items.Clear();
            foundsToolStripComboBox.Add(false, founds.ToString("C"));
            foundsToolStripComboBox.Add(true, maxFounds.ToString("C"));
            foundsToolStripComboBox.SelectedItem = foundsToolStripComboBox.Items[0];
            if (maxFounds == founds)
            {
                foundsToolStripComboBox.Enabled = false;
            }

            foundsToolStripComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (mercs.LengthOrDefault() == 0)
            {
                Close();
            }
            else
            {
                this.LayoutMdi(MdiLayout.TileVertical);
                //this.LayoutMdi(MdiLayout.TileHorizontal);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (foundsToolStripComboBox.Enabled &&
                foundsToolStripComboBox.Value<bool>())
            {
                bytes[20720] = 0;
                bytes[20721] = 0;
                bytes[20722] = 11;
                foundsToolStripComboBox.Enabled = false;
            }

            foreach (var merc in mercs)
            {
                merc.SaveTo(bytes);
            }

            File.Copy(fileName, $"{fileName}.bak", true);
            File.WriteAllBytes(fileName, bytes);
        }
    }
}
