using Kriptok.Drawing.Shapes;
using Kriptok.Extensions;
using Kriptok.Helpers;
using Kriptok.JAEditor.Data;
using Kriptok.Sdk.Tools.Extensions;
using Kriptok.Sdk.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kriptok.JAEditor.Forms
{
    public partial class MercForm : Form
    {
        /// <summary>
        /// Mercenario para copiar y pegar.
        /// </summary>
        private static Merc paperclipMerc;
        private readonly Merc merc;

        public MercForm(Merc merc)
        {
            this.merc = merc;
            InitializeComponent();

            FormHelper.InitFixedSimple(this, merc.Name);
            ControlBox = false;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            health1NumericUpDown.Bind(merc.Health1, 1, 100, i => merc.Health1 = (byte)i);
            health2NumericUpDown.Bind(merc.Health2, 1, 100, i => merc.Health2 = (byte)i);

            agilityNumericUpDown.Bind(merc.Agility, 0, 100, i => merc.Agility = (byte)i);
            dexterityNumericUpDown.Bind(merc.Dexterity, 0, 100, i => merc.Dexterity = (byte)i);
            wisdomNumericUpDown.Bind(merc.Wisdom, 0, 100, i => merc.Wisdom = (byte)i);
            medicalNumericUpDown.Bind(merc.Medic, 0, 100, i => merc.Medic = (byte)i);
            explosivesNumericUpDown.Bind(merc.Explosives, 0, 100, i => merc.Explosives = (byte)i);
            mechanicalNumericUpDown.Bind(merc.Mechanic, 0, 100, i => merc.Mechanic = (byte)i);
            marksmanshipNumericUpDown.Bind(merc.Marksmanship, 0, 100, i => merc.Marksmanship = (byte)i);

            levelNumericUpDown.Bind(merc.ExperienceLevel, 1, 9, i => merc.ExperienceLevel = (byte)i);

            var items = ItemsEnumHelper.WithoutFlags();

            LoadCombo(rightHandComboBox, rightHandQuantityNumericUpDown, merc.Right, items);
            LoadCombo(leftHandComboBox, leftHandQuantityNumericUpDown, merc.Left, items);

            LoadCombo(headComboBox, merc.Head, items.Where(p => p.HasFlag(ItemsEnum.IsHelmet)).ToArray());
            LoadCombo(earsComboBox, merc.Ears, items.Where(p => p.HasFlag(ItemsEnum.IsEarsItem)).ToArray());
            LoadCombo(faceComboBox, merc.Face, items.Where(p => p.HasFlag(ItemsEnum.IsFaceItem)).ToArray());
            LoadCombo(bodyComboBox, merc.Body, items.Where(p => p.HasFlag(ItemsEnum.IsBodyItem)).ToArray());
            LoadCombo(vestComboBox, merc.Vest, items.Where(p => p.HasFlag(ItemsEnum.IsVest)).ToArray());

            var pocketItems = items.Where(p => !p.HasFlag(ItemsEnum.IsVest)).ToArray();
            LoadCombo(pocket1ComboBox, pocket1QuantityNumericUpDown, merc.Pocket1, pocketItems);
            LoadCombo(pocket2ComboBox, pocket2QuantityNumericUpDown, merc.Pocket2, pocketItems);
            LoadCombo(pocket3ComboBox, pocket3QuantityNumericUpDown, merc.Pocket3, pocketItems);
            LoadCombo(pocket4ComboBox, pocket4QuantityNumericUpDown, merc.Pocket4, pocketItems);
            LoadCombo(pocket5ComboBox, pocket5QuantityNumericUpDown, merc.Pocket5, pocketItems);

            camouflageCheckBox.Checked = merc.Camouflaged;
        }

        private void LoadCombo(ComboBox comboBox, NumericUpDown numeric, ItemSlot slot, ItemsEnum[] itemsEnums)
        {
            LoadCombo(comboBox, slot, itemsEnums);
            LoadNumericUpDown(comboBox, numeric, slot.Quantity);

            comboBox.SelectedValueChanged += (s, e) =>
            {
                var val = comboBox.Value<ItemsEnum>();
                var min = val.GetMin();
                var max = val.GetMax();

                numeric.Minimum = min;
                numeric.Maximum = max;
                numeric.Enabled = min != max;

                if (numeric.Enabled)
                {
                    numeric.Value = max;
                }
            };

            numeric.ValueChanged += (s, e) =>
            {
                var val = comboBox.Value<ItemsEnum>();
                var min = val.GetMin();
                var max = val.GetMax();

                if (min == max && max == 1)
                {
                    slot.Quantity = 99;
                }
                else
                {
                    slot.Quantity = (byte)int.Parse(numeric.Value.ToString());
                }
            };
        }

        private static void LoadNumericUpDown(ComboBox comboBox, NumericUpDown numeric, byte value)
        {
            var val = comboBox.Value<ItemsEnum>();
            var min = val.GetMin();
            var max = val.GetMax();

            numeric.Minimum = min;
            numeric.Maximum = max;

            numeric.Enabled = min != max;
            if (numeric.Enabled)
            {
                numeric.Value = (byte)((int)value).Clamp(min, max);
            }
        }

        private void LoadCombo(ComboBox comboBox, ItemSlot slot, ItemsEnum[] itemsEnums)
        {
            LoadCombo(comboBox, slot.Object, itemsEnums);

            comboBox.SelectedValueChanged += (s, e) =>
            {
                slot.Object = comboBox.Value<ItemsEnum>();
            };
        }

        private static void LoadCombo(ComboBox comboBox, ItemsEnum slotObject, ItemsEnum[] itemsEnums)
        {
            var selected = false;
            comboBox.Items.Clear();
            comboBox.SelectedItem = comboBox.Add(ItemsEnum.Nothing, string.Empty);
            comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            foreach (var item in itemsEnums)
            {
                var it = comboBox.Add(item, item.ToString());

                if (item == slotObject)
                {
                    comboBox.SelectedItem = it;
                    selected = true;
                }
            }

            // Si el item seleccionado no está en la lista, tengo que crear un ítem nuevo.
            if (!selected && slotObject != ItemsEnum.Nothing)
            {
                comboBox.SelectedItem = comboBox.Add((int)slotObject, ((int)slotObject).ToString());
            }
        }

        private void vestComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            var vest = vestComboBox.Value<ItemsEnum>();

            switch (vest)
            {
                case ItemsEnum.Nothing:
                case ItemsEnum.UltraVest:
                    pocket1ComboBox.SetValue(ItemsEnum.Nothing); pocket1ComboBox.Enabled = false;
                    pocket2ComboBox.SetValue(ItemsEnum.Nothing); pocket2ComboBox.Enabled = false;
                    pocket3ComboBox.SetValue(ItemsEnum.Nothing); pocket3ComboBox.Enabled = false;
                    pocket4ComboBox.SetValue(ItemsEnum.Nothing); pocket4ComboBox.Enabled = false;
                    pocket5ComboBox.SetValue(ItemsEnum.Nothing); pocket5ComboBox.Enabled = false;
                    break;
                case ItemsEnum.Vest2Pockets:
                    pocket1ComboBox.Enabled = true;
                    pocket2ComboBox.Enabled = true;
                    pocket3ComboBox.SetValue(ItemsEnum.Nothing); pocket3ComboBox.Enabled = false;
                    pocket4ComboBox.SetValue(ItemsEnum.Nothing); pocket4ComboBox.Enabled = false;
                    pocket5ComboBox.SetValue(ItemsEnum.Nothing); pocket5ComboBox.Enabled = false;
                    break;
                case ItemsEnum.Vest3Pockets:
                    pocket1ComboBox.Enabled = true;
                    pocket2ComboBox.Enabled = true;
                    pocket3ComboBox.Enabled = true;
                    pocket4ComboBox.SetValue(ItemsEnum.Nothing); pocket4ComboBox.Enabled = false;
                    pocket5ComboBox.SetValue(ItemsEnum.Nothing); pocket5ComboBox.Enabled = false;
                    break;
                case ItemsEnum.Vest4Pockets:
                    pocket1ComboBox.Enabled = true;
                    pocket2ComboBox.Enabled = true;
                    pocket3ComboBox.Enabled = true;
                    pocket4ComboBox.Enabled = true;
                    pocket5ComboBox.SetValue(ItemsEnum.Nothing); pocket5ComboBox.Enabled = false;
                    break;
                case ItemsEnum.Vest5Pockets:
                    pocket1ComboBox.Enabled = true;
                    pocket2ComboBox.Enabled = true;
                    pocket3ComboBox.Enabled = true;
                    pocket4ComboBox.Enabled = true;
                    pocket5ComboBox.Enabled = true;
                    break;
            }
        }

        private void maxAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            health1NumericUpDown.Value = 100;
            health2NumericUpDown.Value = 100;

            agilityNumericUpDown.Value = 100;
            dexterityNumericUpDown.Value = 100;
            wisdomNumericUpDown.Value = 100;
            medicalNumericUpDown.Value = 100;
            explosivesNumericUpDown.Value = 100;
            mechanicalNumericUpDown.Value = 100;
            marksmanshipNumericUpDown.Value = 100;

            levelNumericUpDown.Value = 9;

        }

        private void MercForm_Load(object sender, EventArgs e)
        {

        }

        private void rightHandComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            var weapon = rightHandComboBox.Value<ItemsEnum>();
            attachmentComboBox.Enabled = (weapon & ItemsEnum.AllowAttachment) != ItemsEnum.Nothing;
            attachmentComboBox.Items.Clear();

            if (attachmentComboBox.Enabled)
            {
                LoadCombo(attachmentComboBox, merc.Right.Attachment, ItemsEnumHelper.GetAttachments());

                attachmentComboBox.SelectedValueChanged += (s2, e2) =>
                {
                    merc.Right.Attachment = attachmentComboBox.Value<ItemsEnum>();
                };
            }
        }

        private void camouflageCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            merc.Camouflaged = camouflageCheckBox.Checked;
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            paperclipMerc = merc;
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (paperclipMerc.Name != merc.Name)
            {
                rightHandComboBox.SetValue(paperclipMerc.Right.Object);
                TrySet(rightHandQuantityNumericUpDown, paperclipMerc.Right.Quantity);

                attachmentComboBox.SetValue(paperclipMerc.Right.Attachment);

                leftHandComboBox.SetValue(paperclipMerc.Left.Object);
                TrySet(leftHandQuantityNumericUpDown, paperclipMerc.Left.Quantity);

                headComboBox.SetValue(paperclipMerc.Head.Object);
                earsComboBox.SetValue(paperclipMerc.Ears.Object);
                faceComboBox.SetValue(paperclipMerc.Face.Object);
                bodyComboBox.SetValue(paperclipMerc.Body.Object);
                vestComboBox.SetValue(paperclipMerc.Vest.Object);

                pocket1ComboBox.SetValue(paperclipMerc.Pocket1.Object); TrySet(pocket1QuantityNumericUpDown, paperclipMerc.Pocket1.Quantity);
                pocket2ComboBox.SetValue(paperclipMerc.Pocket2.Object); TrySet(pocket2QuantityNumericUpDown, paperclipMerc.Pocket2.Quantity);
                pocket3ComboBox.SetValue(paperclipMerc.Pocket3.Object); TrySet(pocket3QuantityNumericUpDown, paperclipMerc.Pocket3.Quantity);
                pocket4ComboBox.SetValue(paperclipMerc.Pocket4.Object); TrySet(pocket4QuantityNumericUpDown, paperclipMerc.Pocket4.Quantity);
                pocket5ComboBox.SetValue(paperclipMerc.Pocket5.Object); TrySet(pocket5QuantityNumericUpDown, paperclipMerc.Pocket5.Quantity);

                camouflageCheckBox.Checked = paperclipMerc.Camouflaged;
            }
        }

        private void TrySet(NumericUpDown numericUpDown, byte quantity)
        {
            var val = ((int)quantity).Clamp((int)numericUpDown.Minimum, (int)numericUpDown.Maximum);
            numericUpDown.Value = val;
        }

        private void maxAllToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            rightHandQuantityNumericUpDown.Value = rightHandQuantityNumericUpDown.Maximum;
            leftHandQuantityNumericUpDown.Value = leftHandQuantityNumericUpDown.Maximum;
            pocket1QuantityNumericUpDown.Value = pocket1QuantityNumericUpDown.Maximum;
            pocket2QuantityNumericUpDown.Value = pocket2QuantityNumericUpDown.Maximum;
            pocket3QuantityNumericUpDown.Value = pocket3QuantityNumericUpDown.Maximum;
            pocket4QuantityNumericUpDown.Value = pocket4QuantityNumericUpDown.Maximum;
            pocket5QuantityNumericUpDown.Value = pocket5QuantityNumericUpDown.Maximum;
        }
    }
}
