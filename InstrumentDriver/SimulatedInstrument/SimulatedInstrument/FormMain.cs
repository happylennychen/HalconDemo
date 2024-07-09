using CommonApi.MyCommunication;

namespace SimulatedInstrument {
    public partial class FormMain : Form {
        public FormMain() {
            InitializeComponent();
            serverSide = new ServerSideDefaultImpl(HandleRequest);
            serverSide.AttachReportMessage(ReportMessage);
            rtbMsgBox.ContextMenuStrip = contextMenu;
        }

        private async void btnInitialize_Click(object sender, EventArgs e) {
            CmdHandler.Initialize(tbIdn.Text.Trim(), ReportMessage);

            string idn = tbIdn.Text.Trim();
            string ip = tbIp.Text.Trim();
            int port = int.Parse(tbPort.Text.Trim());

            this.Text = idn;
            btnListening.Enabled = false;
            tbIdn.Enabled = false;
            tbIp.Enabled = false;
            tbPort.Enabled = false;
            ReportMessage("Start listening...");
            await serverSide.ListenAsync(ip, port, 1);
        }

        private void rtbMsgBox_TextChanged(object sender, EventArgs e) {
            rtbMsgBox.SelectionStart = rtbMsgBox.Text.Length;
            rtbMsgBox.ScrollToCaret();
        }

        private void Form1_Load(object sender, EventArgs e) {
            tbIdn.Text = "Keysight N7745";
            tbIp.Text = "192.168.0.114";
            tbPort.Text = "50001";
        }

        private string HandleRequest(string request) {
            Thread.Sleep(TimeSpan.FromSeconds(1));
            return CmdHandler.Handle(request);
        }

        private void ReportMessage(string message) {
            this.BeginInvoke(new MethodInvoker(delegate {
                rtbMsgBox.AppendText(Environment.NewLine + $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}] : {message}");
            }));
        }

        private readonly ServerSide serverSide;

        private void cmiClearAll_Click(object sender, EventArgs e) {
            rtbMsgBox.Clear();
        }
    }
}