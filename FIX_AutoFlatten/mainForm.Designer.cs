//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.


namespace FIX_AutoFlatten
{
    partial class mainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.exchangeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pRICEDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.oRDERDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FILL = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TEXT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tblGatewaysBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dsRisk = new FIX_AutoFlatten.Properties.DataSources.dsRisk();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageSUM = new System.Windows.Forms.TabPage();
            this.dataGridView5 = new System.Windows.Forms.DataGridView();
            this.mGTDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Account = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.limitDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.totalPLDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tblTraderBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tabPagePOS = new System.Windows.Forms.TabPage();
            this.dataGridView4 = new System.Windows.Forms.DataGridView();
            this.mGTDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.securityExchangeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.securityIDDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buyPosDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sellPosDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.avgBuyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.avgSellDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.realizedPLDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.unrealizedPLDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tblPositionsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tabPageSEC = new System.Windows.Forms.TabPage();
            this.dataGridView3 = new System.Windows.Forms.DataGridView();
            this.SecurityExchange = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Symbol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.securityIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.currencyDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.exchangePointValueDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bidPriceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.askPriceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tblSecurityBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tabPageCUR = new System.Windows.Forms.TabPage();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.currencyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.uSDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eURDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gBPDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.jPYDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cADDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.aUSDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tblCurrencyBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tabPageGW = new System.Windows.Forms.TabPage();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkBxEmail = new System.Windows.Forms.CheckBox();
            this.chkBxAUTO = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblGatewaysBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsRisk)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPageSUM.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblTraderBindingSource)).BeginInit();
            this.tabPagePOS.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblPositionsBindingSource)).BeginInit();
            this.tabPageSEC.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblSecurityBindingSource)).BeginInit();
            this.tabPageCUR.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblCurrencyBindingSource)).BeginInit();
            this.tabPageGW.SuspendLayout();
            this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.exchangeDataGridViewTextBoxColumn,
            this.pRICEDataGridViewTextBoxColumn,
            this.oRDERDataGridViewTextBoxColumn,
            this.FILL,
            this.TEXT});
            this.dataGridView1.DataSource = this.tblGatewaysBindingSource;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 3);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(880, 301);
            this.dataGridView1.TabIndex = 0;
            // 
            // exchangeDataGridViewTextBoxColumn
            // 
            this.exchangeDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.exchangeDataGridViewTextBoxColumn.DataPropertyName = "Exchange";
            this.exchangeDataGridViewTextBoxColumn.HeaderText = "Exchange";
            this.exchangeDataGridViewTextBoxColumn.Name = "exchangeDataGridViewTextBoxColumn";
            this.exchangeDataGridViewTextBoxColumn.ReadOnly = true;
            this.exchangeDataGridViewTextBoxColumn.Width = 80;
            // 
            // pRICEDataGridViewTextBoxColumn
            // 
            this.pRICEDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.pRICEDataGridViewTextBoxColumn.DataPropertyName = "PRICE";
            this.pRICEDataGridViewTextBoxColumn.HeaderText = "PRICE";
            this.pRICEDataGridViewTextBoxColumn.Name = "pRICEDataGridViewTextBoxColumn";
            this.pRICEDataGridViewTextBoxColumn.ReadOnly = true;
            this.pRICEDataGridViewTextBoxColumn.Width = 64;
            // 
            // oRDERDataGridViewTextBoxColumn
            // 
            this.oRDERDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.oRDERDataGridViewTextBoxColumn.DataPropertyName = "ORDER";
            this.oRDERDataGridViewTextBoxColumn.HeaderText = "ORDER";
            this.oRDERDataGridViewTextBoxColumn.Name = "oRDERDataGridViewTextBoxColumn";
            this.oRDERDataGridViewTextBoxColumn.ReadOnly = true;
            this.oRDERDataGridViewTextBoxColumn.Width = 71;
            // 
            // FILL
            // 
            this.FILL.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.FILL.DataPropertyName = "FILL";
            this.FILL.HeaderText = "FILL";
            this.FILL.Name = "FILL";
            this.FILL.ReadOnly = true;
            this.FILL.Width = 53;
            // 
            // TEXT
            // 
            this.TEXT.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.TEXT.DataPropertyName = "TEXT";
            this.TEXT.HeaderText = "TEXT";
            this.TEXT.Name = "TEXT";
            this.TEXT.ReadOnly = true;
            this.TEXT.Width = 60;
            // 
            // tblGatewaysBindingSource
            // 
            this.tblGatewaysBindingSource.DataMember = "tblGateways";
            this.tblGatewaysBindingSource.DataSource = this.dsRisk;
            // 
            // dsRisk
            // 
            this.dsRisk.DataSetName = "dsRisk";
            this.dsRisk.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(0, 413);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(905, 212);
            this.listBox1.TabIndex = 1;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageSUM);
            this.tabControl1.Controls.Add(this.tabPagePOS);
            this.tabControl1.Controls.Add(this.tabPageSEC);
            this.tabControl1.Controls.Add(this.tabPageCUR);
            this.tabControl1.Controls.Add(this.tabPageGW);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(905, 333);
            this.tabControl1.TabIndex = 4;
            // 
            // tabPageSUM
            // 
            this.tabPageSUM.Controls.Add(this.dataGridView5);
            this.tabPageSUM.Location = new System.Drawing.Point(4, 22);
            this.tabPageSUM.Name = "tabPageSUM";
            this.tabPageSUM.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSUM.Size = new System.Drawing.Size(886, 307);
            this.tabPageSUM.TabIndex = 0;
            this.tabPageSUM.Text = "Summary";
            this.tabPageSUM.UseVisualStyleBackColor = true;
            // 
            // dataGridView5
            // 
            this.dataGridView5.AllowUserToAddRows = false;
            this.dataGridView5.AllowUserToDeleteRows = false;
            this.dataGridView5.AutoGenerateColumns = false;
            this.dataGridView5.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView5.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.mGTDataGridViewTextBoxColumn1,
            this.Account,
            this.limitDataGridViewTextBoxColumn,
            this.totalPLDataGridViewTextBoxColumn});
            this.dataGridView5.DataSource = this.tblTraderBindingSource;
            this.dataGridView5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView5.Location = new System.Drawing.Point(3, 3);
            this.dataGridView5.Name = "dataGridView5";
            this.dataGridView5.ReadOnly = true;
            this.dataGridView5.Size = new System.Drawing.Size(880, 301);
            this.dataGridView5.TabIndex = 0;
            // 
            // mGTDataGridViewTextBoxColumn1
            // 
            this.mGTDataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.mGTDataGridViewTextBoxColumn1.DataPropertyName = "MGT";
            this.mGTDataGridViewTextBoxColumn1.HeaderText = "MGT";
            this.mGTDataGridViewTextBoxColumn1.Name = "mGTDataGridViewTextBoxColumn1";
            this.mGTDataGridViewTextBoxColumn1.ReadOnly = true;
            this.mGTDataGridViewTextBoxColumn1.Width = 56;
            // 
            // Account
            // 
            this.Account.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Account.DataPropertyName = "Account";
            this.Account.HeaderText = "Account";
            this.Account.Name = "Account";
            this.Account.ReadOnly = true;
            this.Account.Width = 72;
            // 
            // limitDataGridViewTextBoxColumn
            // 
            this.limitDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.limitDataGridViewTextBoxColumn.DataPropertyName = "Limit";
            this.limitDataGridViewTextBoxColumn.HeaderText = "Limit";
            this.limitDataGridViewTextBoxColumn.Name = "limitDataGridViewTextBoxColumn";
            this.limitDataGridViewTextBoxColumn.ReadOnly = true;
            this.limitDataGridViewTextBoxColumn.Width = 53;
            // 
            // totalPLDataGridViewTextBoxColumn
            // 
            this.totalPLDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.totalPLDataGridViewTextBoxColumn.DataPropertyName = "TotalPL";
            this.totalPLDataGridViewTextBoxColumn.HeaderText = "TotalPL";
            this.totalPLDataGridViewTextBoxColumn.Name = "totalPLDataGridViewTextBoxColumn";
            this.totalPLDataGridViewTextBoxColumn.ReadOnly = true;
            this.totalPLDataGridViewTextBoxColumn.Width = 69;
            // 
            // tblTraderBindingSource
            // 
            this.tblTraderBindingSource.DataMember = "tblTrader";
            this.tblTraderBindingSource.DataSource = this.dsRisk;
            // 
            // tabPagePOS
            // 
            this.tabPagePOS.Controls.Add(this.dataGridView4);
            this.tabPagePOS.Location = new System.Drawing.Point(4, 22);
            this.tabPagePOS.Name = "tabPagePOS";
            this.tabPagePOS.Padding = new System.Windows.Forms.Padding(3);
            this.tabPagePOS.Size = new System.Drawing.Size(897, 307);
            this.tabPagePOS.TabIndex = 1;
            this.tabPagePOS.Text = "Positions";
            this.tabPagePOS.UseVisualStyleBackColor = true;
            // 
            // dataGridView4
            // 
            this.dataGridView4.AllowUserToAddRows = false;
            this.dataGridView4.AllowUserToDeleteRows = false;
            this.dataGridView4.AutoGenerateColumns = false;
            this.dataGridView4.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView4.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.mGTDataGridViewTextBoxColumn,
            this.dataGridViewTextBoxColumn1,
            this.securityExchangeDataGridViewTextBoxColumn,
            this.dataGridViewTextBoxColumn2,
            this.securityIDDataGridViewTextBoxColumn1,
            this.buyPosDataGridViewTextBoxColumn,
            this.sellPosDataGridViewTextBoxColumn,
            this.avgBuyDataGridViewTextBoxColumn,
            this.avgSellDataGridViewTextBoxColumn,
            this.realizedPLDataGridViewTextBoxColumn,
            this.unrealizedPLDataGridViewTextBoxColumn});
            this.dataGridView4.DataSource = this.tblPositionsBindingSource;
            this.dataGridView4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView4.Location = new System.Drawing.Point(3, 3);
            this.dataGridView4.Name = "dataGridView4";
            this.dataGridView4.ReadOnly = true;
            this.dataGridView4.Size = new System.Drawing.Size(891, 301);
            this.dataGridView4.TabIndex = 0;
            // 
            // mGTDataGridViewTextBoxColumn
            // 
            this.mGTDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.mGTDataGridViewTextBoxColumn.DataPropertyName = "MGT";
            this.mGTDataGridViewTextBoxColumn.HeaderText = "MGT";
            this.mGTDataGridViewTextBoxColumn.Name = "mGTDataGridViewTextBoxColumn";
            this.mGTDataGridViewTextBoxColumn.ReadOnly = true;
            this.mGTDataGridViewTextBoxColumn.Width = 56;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Account";
            this.dataGridViewTextBoxColumn1.HeaderText = "Account";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 72;
            // 
            // securityExchangeDataGridViewTextBoxColumn
            // 
            this.securityExchangeDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.securityExchangeDataGridViewTextBoxColumn.DataPropertyName = "SecurityExchange";
            this.securityExchangeDataGridViewTextBoxColumn.HeaderText = "SecurityExchange";
            this.securityExchangeDataGridViewTextBoxColumn.Name = "securityExchangeDataGridViewTextBoxColumn";
            this.securityExchangeDataGridViewTextBoxColumn.ReadOnly = true;
            this.securityExchangeDataGridViewTextBoxColumn.Width = 118;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn2.DataPropertyName = "Symbol";
            this.dataGridViewTextBoxColumn2.HeaderText = "Symbol";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 66;
            // 
            // securityIDDataGridViewTextBoxColumn1
            // 
            this.securityIDDataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.securityIDDataGridViewTextBoxColumn1.DataPropertyName = "SecurityID";
            this.securityIDDataGridViewTextBoxColumn1.HeaderText = "SecurityID";
            this.securityIDDataGridViewTextBoxColumn1.Name = "securityIDDataGridViewTextBoxColumn1";
            this.securityIDDataGridViewTextBoxColumn1.ReadOnly = true;
            this.securityIDDataGridViewTextBoxColumn1.Width = 81;
            // 
            // buyPosDataGridViewTextBoxColumn
            // 
            this.buyPosDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.buyPosDataGridViewTextBoxColumn.DataPropertyName = "BuyPos";
            this.buyPosDataGridViewTextBoxColumn.HeaderText = "BuyPos";
            this.buyPosDataGridViewTextBoxColumn.Name = "buyPosDataGridViewTextBoxColumn";
            this.buyPosDataGridViewTextBoxColumn.ReadOnly = true;
            this.buyPosDataGridViewTextBoxColumn.Width = 68;
            // 
            // sellPosDataGridViewTextBoxColumn
            // 
            this.sellPosDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.sellPosDataGridViewTextBoxColumn.DataPropertyName = "SellPos";
            this.sellPosDataGridViewTextBoxColumn.HeaderText = "SellPos";
            this.sellPosDataGridViewTextBoxColumn.Name = "sellPosDataGridViewTextBoxColumn";
            this.sellPosDataGridViewTextBoxColumn.ReadOnly = true;
            this.sellPosDataGridViewTextBoxColumn.Width = 67;
            // 
            // avgBuyDataGridViewTextBoxColumn
            // 
            this.avgBuyDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.avgBuyDataGridViewTextBoxColumn.DataPropertyName = "AvgBuy";
            this.avgBuyDataGridViewTextBoxColumn.HeaderText = "AvgBuy";
            this.avgBuyDataGridViewTextBoxColumn.Name = "avgBuyDataGridViewTextBoxColumn";
            this.avgBuyDataGridViewTextBoxColumn.ReadOnly = true;
            this.avgBuyDataGridViewTextBoxColumn.Width = 69;
            // 
            // avgSellDataGridViewTextBoxColumn
            // 
            this.avgSellDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.avgSellDataGridViewTextBoxColumn.DataPropertyName = "AvgSell";
            this.avgSellDataGridViewTextBoxColumn.HeaderText = "AvgSell";
            this.avgSellDataGridViewTextBoxColumn.Name = "avgSellDataGridViewTextBoxColumn";
            this.avgSellDataGridViewTextBoxColumn.ReadOnly = true;
            this.avgSellDataGridViewTextBoxColumn.Width = 68;
            // 
            // realizedPLDataGridViewTextBoxColumn
            // 
            this.realizedPLDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.realizedPLDataGridViewTextBoxColumn.DataPropertyName = "RealizedPL";
            this.realizedPLDataGridViewTextBoxColumn.HeaderText = "RealizedPL";
            this.realizedPLDataGridViewTextBoxColumn.Name = "realizedPLDataGridViewTextBoxColumn";
            this.realizedPLDataGridViewTextBoxColumn.ReadOnly = true;
            this.realizedPLDataGridViewTextBoxColumn.Width = 86;
            // 
            // unrealizedPLDataGridViewTextBoxColumn
            // 
            this.unrealizedPLDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.unrealizedPLDataGridViewTextBoxColumn.DataPropertyName = "UnrealizedPL";
            this.unrealizedPLDataGridViewTextBoxColumn.HeaderText = "UnrealizedPL";
            this.unrealizedPLDataGridViewTextBoxColumn.Name = "unrealizedPLDataGridViewTextBoxColumn";
            this.unrealizedPLDataGridViewTextBoxColumn.ReadOnly = true;
            this.unrealizedPLDataGridViewTextBoxColumn.Width = 95;
            // 
            // tblPositionsBindingSource
            // 
            this.tblPositionsBindingSource.DataMember = "tblPositions";
            this.tblPositionsBindingSource.DataSource = this.dsRisk;
            // 
            // tabPageSEC
            // 
            this.tabPageSEC.Controls.Add(this.dataGridView3);
            this.tabPageSEC.Location = new System.Drawing.Point(4, 22);
            this.tabPageSEC.Name = "tabPageSEC";
            this.tabPageSEC.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSEC.Size = new System.Drawing.Size(886, 307);
            this.tabPageSEC.TabIndex = 2;
            this.tabPageSEC.Text = "Securities";
            this.tabPageSEC.UseVisualStyleBackColor = true;
            // 
            // dataGridView3
            // 
            this.dataGridView3.AllowUserToAddRows = false;
            this.dataGridView3.AllowUserToDeleteRows = false;
            this.dataGridView3.AutoGenerateColumns = false;
            this.dataGridView3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView3.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SecurityExchange,
            this.Symbol,
            this.securityIDDataGridViewTextBoxColumn,
            this.currencyDataGridViewTextBoxColumn1,
            this.exchangePointValueDataGridViewTextBoxColumn,
            this.bidPriceDataGridViewTextBoxColumn,
            this.askPriceDataGridViewTextBoxColumn});
            this.dataGridView3.DataSource = this.tblSecurityBindingSource;
            this.dataGridView3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView3.Location = new System.Drawing.Point(3, 3);
            this.dataGridView3.Name = "dataGridView3";
            this.dataGridView3.ReadOnly = true;
            this.dataGridView3.Size = new System.Drawing.Size(880, 301);
            this.dataGridView3.TabIndex = 0;
            // 
            // SecurityExchange
            // 
            this.SecurityExchange.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.SecurityExchange.DataPropertyName = "SecurityExchange";
            this.SecurityExchange.HeaderText = "SecurityExchange";
            this.SecurityExchange.Name = "SecurityExchange";
            this.SecurityExchange.ReadOnly = true;
            this.SecurityExchange.Width = 118;
            // 
            // Symbol
            // 
            this.Symbol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Symbol.DataPropertyName = "Symbol";
            this.Symbol.HeaderText = "Symbol";
            this.Symbol.Name = "Symbol";
            this.Symbol.ReadOnly = true;
            this.Symbol.Width = 66;
            // 
            // securityIDDataGridViewTextBoxColumn
            // 
            this.securityIDDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.securityIDDataGridViewTextBoxColumn.DataPropertyName = "SecurityID";
            this.securityIDDataGridViewTextBoxColumn.HeaderText = "SecurityID";
            this.securityIDDataGridViewTextBoxColumn.Name = "securityIDDataGridViewTextBoxColumn";
            this.securityIDDataGridViewTextBoxColumn.ReadOnly = true;
            this.securityIDDataGridViewTextBoxColumn.Width = 81;
            // 
            // currencyDataGridViewTextBoxColumn1
            // 
            this.currencyDataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.currencyDataGridViewTextBoxColumn1.DataPropertyName = "Currency";
            this.currencyDataGridViewTextBoxColumn1.HeaderText = "Currency";
            this.currencyDataGridViewTextBoxColumn1.Name = "currencyDataGridViewTextBoxColumn1";
            this.currencyDataGridViewTextBoxColumn1.ReadOnly = true;
            this.currencyDataGridViewTextBoxColumn1.Width = 74;
            // 
            // exchangePointValueDataGridViewTextBoxColumn
            // 
            this.exchangePointValueDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.exchangePointValueDataGridViewTextBoxColumn.DataPropertyName = "ExchangePointValue";
            this.exchangePointValueDataGridViewTextBoxColumn.HeaderText = "ExchangePointValue";
            this.exchangePointValueDataGridViewTextBoxColumn.Name = "exchangePointValueDataGridViewTextBoxColumn";
            this.exchangePointValueDataGridViewTextBoxColumn.ReadOnly = true;
            this.exchangePointValueDataGridViewTextBoxColumn.Width = 131;
            // 
            // bidPriceDataGridViewTextBoxColumn
            // 
            this.bidPriceDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.bidPriceDataGridViewTextBoxColumn.DataPropertyName = "BidPrice";
            this.bidPriceDataGridViewTextBoxColumn.HeaderText = "BidPrice";
            this.bidPriceDataGridViewTextBoxColumn.Name = "bidPriceDataGridViewTextBoxColumn";
            this.bidPriceDataGridViewTextBoxColumn.ReadOnly = true;
            this.bidPriceDataGridViewTextBoxColumn.Width = 71;
            // 
            // askPriceDataGridViewTextBoxColumn
            // 
            this.askPriceDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.askPriceDataGridViewTextBoxColumn.DataPropertyName = "AskPrice";
            this.askPriceDataGridViewTextBoxColumn.HeaderText = "AskPrice";
            this.askPriceDataGridViewTextBoxColumn.Name = "askPriceDataGridViewTextBoxColumn";
            this.askPriceDataGridViewTextBoxColumn.ReadOnly = true;
            this.askPriceDataGridViewTextBoxColumn.Width = 74;
            // 
            // tblSecurityBindingSource
            // 
            this.tblSecurityBindingSource.DataMember = "tblSecurity";
            this.tblSecurityBindingSource.DataSource = this.dsRisk;
            // 
            // tabPageCUR
            // 
            this.tabPageCUR.Controls.Add(this.dataGridView2);
            this.tabPageCUR.Location = new System.Drawing.Point(4, 22);
            this.tabPageCUR.Name = "tabPageCUR";
            this.tabPageCUR.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageCUR.Size = new System.Drawing.Size(886, 307);
            this.tabPageCUR.TabIndex = 3;
            this.tabPageCUR.Text = "Currency";
            this.tabPageCUR.UseVisualStyleBackColor = true;
            // 
            // dataGridView2
            // 
            this.dataGridView2.AllowUserToAddRows = false;
            this.dataGridView2.AllowUserToDeleteRows = false;
            this.dataGridView2.AutoGenerateColumns = false;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.currencyDataGridViewTextBoxColumn,
            this.uSDDataGridViewTextBoxColumn,
            this.eURDataGridViewTextBoxColumn,
            this.gBPDataGridViewTextBoxColumn,
            this.jPYDataGridViewTextBoxColumn,
            this.cADDataGridViewTextBoxColumn,
            this.aUSDataGridViewTextBoxColumn});
            this.dataGridView2.DataSource = this.tblCurrencyBindingSource;
            this.dataGridView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView2.Location = new System.Drawing.Point(3, 3);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.Size = new System.Drawing.Size(880, 301);
            this.dataGridView2.TabIndex = 0;
            // 
            // currencyDataGridViewTextBoxColumn
            // 
            this.currencyDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.currencyDataGridViewTextBoxColumn.DataPropertyName = "Currency";
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            this.currencyDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle1;
            this.currencyDataGridViewTextBoxColumn.HeaderText = "Currency";
            this.currencyDataGridViewTextBoxColumn.Name = "currencyDataGridViewTextBoxColumn";
            this.currencyDataGridViewTextBoxColumn.Width = 74;
            // 
            // uSDDataGridViewTextBoxColumn
            // 
            this.uSDDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.uSDDataGridViewTextBoxColumn.DataPropertyName = "USD";
            this.uSDDataGridViewTextBoxColumn.HeaderText = "USD";
            this.uSDDataGridViewTextBoxColumn.Name = "uSDDataGridViewTextBoxColumn";
            this.uSDDataGridViewTextBoxColumn.Width = 55;
            // 
            // eURDataGridViewTextBoxColumn
            // 
            this.eURDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.eURDataGridViewTextBoxColumn.DataPropertyName = "EUR";
            this.eURDataGridViewTextBoxColumn.HeaderText = "EUR";
            this.eURDataGridViewTextBoxColumn.Name = "eURDataGridViewTextBoxColumn";
            this.eURDataGridViewTextBoxColumn.Width = 55;
            // 
            // gBPDataGridViewTextBoxColumn
            // 
            this.gBPDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.gBPDataGridViewTextBoxColumn.DataPropertyName = "GBP";
            this.gBPDataGridViewTextBoxColumn.HeaderText = "GBP";
            this.gBPDataGridViewTextBoxColumn.Name = "gBPDataGridViewTextBoxColumn";
            this.gBPDataGridViewTextBoxColumn.Width = 54;
            // 
            // jPYDataGridViewTextBoxColumn
            // 
            this.jPYDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.jPYDataGridViewTextBoxColumn.DataPropertyName = "JPY";
            this.jPYDataGridViewTextBoxColumn.HeaderText = "JPY";
            this.jPYDataGridViewTextBoxColumn.Name = "jPYDataGridViewTextBoxColumn";
            this.jPYDataGridViewTextBoxColumn.Width = 51;
            // 
            // cADDataGridViewTextBoxColumn
            // 
            this.cADDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cADDataGridViewTextBoxColumn.DataPropertyName = "CAD";
            this.cADDataGridViewTextBoxColumn.HeaderText = "CAD";
            this.cADDataGridViewTextBoxColumn.Name = "cADDataGridViewTextBoxColumn";
            this.cADDataGridViewTextBoxColumn.Width = 54;
            // 
            // aUSDataGridViewTextBoxColumn
            // 
            this.aUSDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.aUSDataGridViewTextBoxColumn.DataPropertyName = "AUS";
            this.aUSDataGridViewTextBoxColumn.HeaderText = "AUS";
            this.aUSDataGridViewTextBoxColumn.Name = "aUSDataGridViewTextBoxColumn";
            this.aUSDataGridViewTextBoxColumn.Width = 54;
            // 
            // tblCurrencyBindingSource
            // 
            this.tblCurrencyBindingSource.DataMember = "tblCurrency";
            this.tblCurrencyBindingSource.DataSource = this.dsRisk;
            // 
            // tabPageGW
            // 
            this.tabPageGW.Controls.Add(this.dataGridView1);
            this.tabPageGW.Location = new System.Drawing.Point(4, 22);
            this.tabPageGW.Name = "tabPageGW";
            this.tabPageGW.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGW.Size = new System.Drawing.Size(886, 307);
            this.tabPageGW.TabIndex = 4;
            this.tabPageGW.Text = "Gateways";
            this.tabPageGW.UseVisualStyleBackColor = true;
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.BottomToolStripPanel
            // 
            this.toolStripContainer1.BottomToolStripPanel.Controls.Add(this.statusStrip1);
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.AutoScroll = true;
            this.toolStripContainer1.ContentPanel.Controls.Add(this.listBox1);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.groupBox1);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.tabControl1);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(921, 604);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(921, 651);
            this.toolStripContainer1.TabIndex = 5;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStrip1.Location = new System.Drawing.Point(0, 0);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(921, 22);
            this.statusStrip1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkBxEmail);
            this.groupBox1.Controls.Add(this.chkBxAUTO);
            this.groupBox1.Controls.Add(this.checkBox1);
            this.groupBox1.Controls.Add(this.dateTimePicker2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 333);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(905, 80);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Controls";
            // 
            // chkBxEmail
            // 
            this.chkBxEmail.AutoSize = true;
            this.chkBxEmail.Checked = true;
            this.chkBxEmail.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBxEmail.Location = new System.Drawing.Point(508, 35);
            this.chkBxEmail.Name = "chkBxEmail";
            this.chkBxEmail.Size = new System.Drawing.Size(116, 17);
            this.chkBxEmail.TabIndex = 35;
            this.chkBxEmail.Text = "Enable Email Alerts";
            this.chkBxEmail.UseVisualStyleBackColor = true;
            this.chkBxEmail.CheckedChanged += new System.EventHandler(this.chkBxEmail_CheckedChanged);
            // 
            // chkBxAUTO
            // 
            this.chkBxAUTO.AutoSize = true;
            this.chkBxAUTO.Checked = true;
            this.chkBxAUTO.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBxAUTO.Location = new System.Drawing.Point(270, 35);
            this.chkBxAUTO.Name = "chkBxAUTO";
            this.chkBxAUTO.Size = new System.Drawing.Size(232, 17);
            this.chkBxAUTO.TabIndex = 34;
            this.chkBxAUTO.Text = "Enable Automatic trading to flatten positions";
            this.chkBxAUTO.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(111, 35);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(153, 17);
            this.checkBox1.TabIndex = 33;
            this.checkBox1.Text = "Enable Reset PositionTime";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.CustomFormat = "";
            this.dateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dateTimePicker2.Location = new System.Drawing.Point(15, 32);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.ShowUpDown = true;
            this.dateTimePicker2.Size = new System.Drawing.Size(90, 20);
            this.dateTimePicker2.TabIndex = 31;
            this.dateTimePicker2.ValueChanged += new System.EventHandler(this.dateTimePicker2_ValueChanged);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(921, 651);
            this.Controls.Add(this.toolStripContainer1);
            this.Name = "mainForm";
            this.Text = "TT Advanced Solutions FIX Risk Manager Sample";
            this.Load += new System.EventHandler(this.mainForm_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.mainForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblGatewaysBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsRisk)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPageSUM.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblTraderBindingSource)).EndInit();
            this.tabPagePOS.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblPositionsBindingSource)).EndInit();
            this.tabPageSEC.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblSecurityBindingSource)).EndInit();
            this.tabPageCUR.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblCurrencyBindingSource)).EndInit();
            this.tabPageGW.ResumeLayout(false);
            this.toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.BottomToolStripPanel.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        //private System.Windows.Forms.DataGridViewTextBoxColumn fILLSDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource tblGatewaysBindingSource;
        private FIX_AutoFlatten.Properties.DataSources.dsRisk dsRisk;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageSUM;
        private System.Windows.Forms.TabPage tabPagePOS;
        private System.Windows.Forms.TabPage tabPageSEC;
        private System.Windows.Forms.TabPage tabPageCUR;
        private System.Windows.Forms.TabPage tabPageGW;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.BindingSource tblCurrencyBindingSource;
        private System.Windows.Forms.DataGridView dataGridView3;
        private System.Windows.Forms.BindingSource tblSecurityBindingSource;
        private System.Windows.Forms.DataGridView dataGridView4;
        private System.Windows.Forms.BindingSource tblPositionsBindingSource;
        private System.Windows.Forms.DataGridView dataGridView5;
        private System.Windows.Forms.BindingSource tblTraderBindingSource;
        //private System.Windows.Forms.DataGridViewTextBoxColumn tOTALDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn exchangeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn pRICEDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn oRDERDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn FILL;
        private System.Windows.Forms.DataGridViewTextBoxColumn TEXT;
        private System.Windows.Forms.DataGridViewTextBoxColumn SecurityExchange;
        private System.Windows.Forms.DataGridViewTextBoxColumn Symbol;
        private System.Windows.Forms.DataGridViewTextBoxColumn securityIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn currencyDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn exchangePointValueDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn bidPriceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn askPriceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn currencyDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn uSDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn eURDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn gBPDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn jPYDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cADDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn aUSDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn mGTDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Account;
        private System.Windows.Forms.DataGridViewTextBoxColumn limitDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn totalPLDataGridViewTextBoxColumn;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox chkBxAUTO;
        private System.Windows.Forms.CheckBox chkBxEmail;
        private System.Windows.Forms.DataGridViewTextBoxColumn mGTDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn securityExchangeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn securityIDDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn buyPosDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sellPosDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn avgBuyDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn avgSellDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn realizedPLDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn unrealizedPLDataGridViewTextBoxColumn;
    }
}

