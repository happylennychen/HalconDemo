using CommonApi.MyI18N;

using MyInstruments;
using MyInstruments.MyEnum;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace ProberApi.MyForm {
    public partial class FormCouplingShowInfo : Form {
        public FormCouplingShowInfo(EnumLanguage language, Instrument instrument, string slot, string channel, EnumInstrumentCategory instrumentCategory) {
            InitializeComponent();
            this.ControlBox = false;

            frc = new FormResourceCulture(this.GetType().FullName, Assembly.GetExecutingAssembly());
            frc.Language = language;
            this.instrument = instrument;
            this.slot = slot;
            this.channel = channel;
            this.instrumentCategory = instrumentCategory;
        }
        
        private void FormCouplingShowCouplingInInfo_Load(object sender, EventArgs e) {
            List<TextBox> textBoxes = new List<TextBox> { tbVendorModel, tbVisaResource, tbSlot, tbChannel, tbInstrumentCategory };
            foreach (TextBox textBox in textBoxes) {
                textBox.ReadOnly = true;
                textBox.BackColor = Color.LightGray;
            }

            tbVendorModel.Text = $"{instrument.Vendor} {instrument.CurrentModel}";
            tbVisaResource.Text = instrument.VisaResource;
            tbSlot.Text = slot ;
            tbChannel.Text = channel ;
            tbInstrumentCategory.Text = instrumentCategory.ToString();

            frc.EnumerateControl(this);
        }

        private void btnClose_Click(object sender, EventArgs e) {
            this.Close();
        }

        private readonly FormResourceCulture frc;
        private readonly Instrument instrument;
        private readonly string slot;
        private readonly string channel;
        private readonly EnumInstrumentCategory instrumentCategory;        
    }
}
