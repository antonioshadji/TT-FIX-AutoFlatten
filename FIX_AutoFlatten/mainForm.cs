/**
   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
**/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;

namespace FIX_AutoFlatten
{
    public partial class mainForm : Form
    {
        /// <summary>
        /// starting point of application 
        /// </summary>
        public mainForm()
        {
            InitializeComponent();
        }

        private static LOG.LogFiles log = new LOG.LogFiles();
        private QuickFix.QFApplication _qf;
        private DateTime current = DateTime.Now;
      
        private ArrayList stoppedTraders = new ArrayList();
        private bool previousPositionsLoaded = false;
        private bool TimeChanged = false;
        private bool TimerUpdated = false;

        DataSet setINI = new DataSet("INI");
        private string baseCurrency = null;
        private bool isAccountFilterEnabled = false;
        private bool isDebug = false;
        
        //email variables setup
        private DataTable tblParameters = null;
        private bool isEmailEnabled = false;
        private Decimal[] EmailLimits;
        private String[] to;
        private String[] cc;
        private String[] bcc;
        private String[] msg;
        private String[] subject;
        private MailPriority[] priority;

        private void mainForm_Load(object sender, EventArgs e)
        {
            try
            {
                TT.SendMsg.initializeLog();

                log.CreateLog("AUTOFLAT", this.listBox1);
                this.Text += " v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
                log.WriteLog(this.Text);

                _qf = new QuickFix.QFApplication();

                _LoadINI();
                _LoadMGT();
                _LoadCurrency();

                #region Register Delegates for passing data between threads
                //these calls are registering local methods for updating the gui
                //the _qf. methods are delegates that pass the data from the QuickFix thread 
                //to the Gui thread.    
                _qf.registerLogUpdater(log.WriteList);
                _qf.registerGatewayUpdater(UpdateGateway);
                _qf.registerPositionUpdater(UpdatePosition);
                _qf.registerSecurityUpdater(UpdateSecurity);
                _qf.registerPriceUpdater(UpdatePrices);
                _qf.registerOrderCanceler(orderCancel);
                _qf.registerOnOff(activateOnOff);
                _qf.registerOrderMgmt(updateOrderMgmt);
                _qf.registerClearPOS(ClearPositions); 
                #endregion
                
                _qf.initiate("ini.cfg", "12345678", true, this);

            }
            catch (Exception ex)
            {   log.WriteList(ex.ToString()); }


        }

        private void mainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            dsRisk.tblCurrency.WriteXml("CURRENCY.XML");

            if (TimeChanged) 
            {
                setINI.Tables["resetTime"].Rows[0]["time"] = dateTimePicker2.Value.TimeOfDay;
                setINI.WriteXml("INI.XML");
            }
        }

        /// <summary>
        /// Notify application that all positions are loaded
        /// </summary>
        private void activateOnOff()
        { previousPositionsLoaded = true; }

        /// <summary>
        /// Load Trader limits by MGT
        /// </summary>
        private void _LoadMGT()
        {
            try
            {
                if (File.Exists("MGT.XML"))
                {
                    dsRisk.tblTrader.ReadXml("MGT.XML");
                    log.WriteList("MGT.XML: Data Loaded");
                }
                else
                {
                    log.WriteList("MGT.XML: No Trader Limits File loaded!");
                    MessageBox.Show("Application will terminate! Can not run without trader limits file", "FATAL ERROR");
                    Application.Exit();
                }
            }
            catch (Exception ex)
            { log.WriteList(ex.ToString()); }
        
        }

        /// <summary>
        /// Load Currency Conversions
        /// </summary>
        private void _LoadCurrency()
        {
            try
            {
                if (File.Exists("CURRENCY.XML"))
                {
                    dsRisk.tblCurrency.ReadXml("CURRENCY.XML");
                    log.WriteList("CURRENCY.XML: Data Loaded");
                }
                else
                {
                    log.WriteList("CURRENCY.XML: No conversion will be done!");

                }
            }
            catch (Exception ex)
            { log.WriteList(ex.ToString()); }

        }

        /// <summary>
        /// Load email settings
        /// </summary>
        private void _LoadINI()
        {
            log.WriteLog("Load INI Parameters");

            try
            {
                if (File.Exists("INI.XML"))
                {
                    setINI.ReadXml("INI.XML");
                    log.WriteList("INI.XML: File Loaded");

                    if (setINI.Tables.Contains("settings"))
                    {
                        TimeSpan ts;
                        bool worked = TimeSpan.TryParse(setINI.Tables["settings"].Rows[0]["resetTime"].ToString(), out ts);

                        if (worked)
                        {
                            this.dateTimePicker2.Value = DateTime.Today + ts;
                            TimeChanged = false;
                            log.WriteList("Reset Time Set to: " + this.dateTimePicker2.Value.ToShortTimeString());
                        }
                        else
                        { log.WriteList("Reset Time not Set!"); }


                        if (setINI.Tables["settings"].Columns.Contains("baseCurrency"))
                        {
                            baseCurrency = setINI.Tables["settings"].Rows[0]["baseCurrency"].ToString();
                        }
                        else
                        {
                            baseCurrency = "USD";
                        }
                        log.WriteList("Base Currency Set to " + baseCurrency);

                        bool filter = Boolean.TryParse(setINI.Tables["settings"].Rows[0]["AccountFilter"].ToString(), out isAccountFilterEnabled);
                        log.WriteList("Account Filtering Set to "+isAccountFilterEnabled.ToString());

                        bool testing = Boolean.TryParse(setINI.Tables["settings"].Rows[0]["DEBUG"].ToString(), out isDebug);
                        log.WriteList("DEBUG MODE: " + isDebug.ToString());

                    }

                    //setup email 
                    if (setINI.Tables.Contains("parameters"))
                    {
                        tblParameters = setINI.Tables["parameters"];

                        bool[] verifyColumns = new bool[6];

                        DataRow r = tblParameters.Rows[0];

                        if (tblParameters.Columns.Contains("FROM")) { verifyColumns[0] = true; }
                        if (tblParameters.Columns.Contains("SERVER")) { verifyColumns[1] = true; }
                        if (tblParameters.Columns.Contains("PORT")) { verifyColumns[2] = true; }
                        if (tblParameters.Columns.Contains("SSL")) { verifyColumns[3] = true; }
                        if (tblParameters.Columns.Contains("LOGIN")) { verifyColumns[4] = true; }
                        if (tblParameters.Columns.Contains("PWORD")) { verifyColumns[5] = true; }

                        bool IsEmailSetup = true;
                        int i = 0;
                        foreach (bool item in verifyColumns)
                        {
                            if (!item)
                            {
                                log.WriteList("MISSING EMAIL PARAMETER: " + tblParameters.Columns[i].ColumnName);
                                IsEmailSetup = false;
                            }
                            i++;
                        }

                        //this needs to be done on a separate thread to prevent holding up application loading
                        //string test = tblParameters.Rows[0]["SERVER"].ToString();
                        //if (_PingTest(test))
                        //{ log.WriteList("Email Server exists!"); }
                        //else
                        //{ 
                        //    IsEmailSetup = false;
                        //    log.WriteList("Email server not found! Alerts Disabled");
                        //}

                        if (IsEmailSetup)
                        {
                            isEmailEnabled = true;
                            log.WriteList("Email Alerts Enabled");
                            chkBxEmail.Checked = true;
                        }
                        else
                        { chkBxEmail.Checked = false; }

                    }
                    else
                    { 
                        log.WriteList("EMAIL Server parameters not found in INI.XML");
                        chkBxEmail.Checked = false;
                    }

                    DataTable tblAddress = null;
                    //Set up email address table 
                    if (setINI.Tables.Contains("address"))
                    {
                        tblAddress = setINI.Tables["address"];

                        int ctr = 0;
                        to = new String[tblAddress.Rows.Count];
                        cc = new String[tblAddress.Rows.Count];
                        bcc = new String[tblAddress.Rows.Count];
                        EmailLimits = new Decimal[tblAddress.Rows.Count];
                        subject = new String[tblAddress.Rows.Count];
                        msg = new String[tblAddress.Rows.Count];
                        priority = new MailPriority[tblAddress.Rows.Count];

                        foreach (DataRow row in tblAddress.Rows)
                        {
                            to.SetValue(row[tblAddress.Columns["to"]].ToString(), ctr);
                            cc.SetValue(row[tblAddress.Columns["cc"]].ToString(), ctr);
                            bcc.SetValue(row[tblAddress.Columns["bcc"]].ToString(), ctr);

                            EmailLimits[ctr] = Math.Abs(
                                Convert.ToDecimal(
                                row[tblAddress.Columns["limit"]])) * -1;

                            subject.SetValue(row[tblAddress.Columns["subject"]].ToString(), ctr);
                            msg.SetValue(row[tblAddress.Columns["body"]].ToString(), ctr);


                            if (row[tblAddress.Columns["priority"]].ToString().ToUpperInvariant() == "HIGH")
                            {
                                priority[ctr] = MailPriority.High;
                            }
                            else if (row[tblAddress.Columns["priority"]].ToString().ToUpperInvariant() == "NORMAL")
                            {
                                priority[ctr] = MailPriority.Normal;
                            }
                            else if (row[tblAddress.Columns["priority"]].ToString().ToUpperInvariant() == "LOW")
                            {
                                priority[ctr] = MailPriority.Low;
                            }
                            else
                            { priority[ctr] = MailPriority.Normal; }

                            ctr++;
                        }

                        log.WriteList("Email addresses found!");
                    }
                    else
                    { 
                        log.WriteList("No Email addresses found in INI.XML");
                        chkBxEmail.Checked = false;
                    }

                }
                else
                { log.WriteList("INI.XML file does not exist"); }
            }
            catch (Exception ex)
            { log.WriteList(ex.ToString()); }
        }

        /// <summary>
        /// check is email server exists
        /// </summary>
        /// <param name="serverAddress"></param>
        /// <returns></returns>
        //Ping test code must be Set up to run on separate thread to prevent application from hanging
        //private bool _PingTest(string serverAddress)
        //{
        //    try
        //    {
        //        Ping pingSender = new Ping();
        //        PingReply reply = pingSender.Send(serverAddress);

        //        if (reply.Status == IPStatus.Success)
        //        {
        //            log.WriteList(string.Format("{1} RoundTrip time: {0}", reply.RoundtripTime, DateTime.Now.ToString("hh:mm:ss")));
        //            return true;
        //        }
        //        else if (reply.Status == IPStatus.TimedOut)
        //        {
        //            log.WriteList(reply.Status.ToString());
        //            return false;
        //        }
        //        else
        //        {
        //            log.WriteList("Ping Failed!");
        //            log.WriteList(reply.Status.ToString());
        //            return false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        log.WriteList("Ping Failed!");
        //        log.WriteLog(ex.ToString());
        //        return false;
        //    }
        //}

        /// <summary>
        /// calculate P&L;
        /// </summary>
        /// <param name="SecEx">GATEWAY</param>
        /// <param name="product">PRODUCT TICKER</param>
        /// <param name="secID">SECURITY_ID</param>
        /// <param name="bid">Market Bid Price</param>
        /// <param name="ask">Market Ask Price</param>
        private void CalcPnL(string SecEx, string product, string secID, decimal  bid, decimal  ask)
        {
            current = DateTime.Now;
            //log.WriteList("CalcPnL");
            try{

                SortedDictionary<string, decimal > totalPnL = new SortedDictionary<string, decimal >();

                foreach (DataRow dr in dsRisk.tblPositions)
                {
                    if (dr[dsRisk.tblPositions.SecurityExchangeColumn].Equals(SecEx) &&
                        dr[dsRisk.tblPositions.SymbolColumn].Equals(product) &&
                        dr[dsRisk.tblPositions.SecurityIDColumn].Equals(secID) &&
                        dsRisk.tblSecurity.Rows.Find(new object[] { SecEx, product, secID }) != null)
                    {
                        DataRow securityInfo = dsRisk.tblSecurity.Rows.Find(new object[] { SecEx, product, secID });
                        DataRow conversion = dsRisk.tblCurrency.Rows.Find(baseCurrency);

                        int buypos = 0;
                        if (dr[dsRisk.tblPositions.BuyPosColumn]!=null)
                            buypos = (int)dr[dsRisk.tblPositions.BuyPosColumn];

                        int sellpos = 0;
                        if (dr[dsRisk.tblPositions.SellPosColumn]!=null)
                            sellpos = (int)dr[dsRisk.tblPositions.SellPosColumn];

                        int matchQty = Math.Min(buypos, sellpos);
                        int remainQty = buypos - sellpos;

                        decimal  avgbuy = 0.00M;
                        //TODO verify that this works 
                        if ( ((decimal)dr[dsRisk.tblPositions.AvgBuyColumn]).GetType().IsValueType )
                            avgbuy = (decimal )dr[dsRisk.tblPositions.AvgBuyColumn];

                        decimal  avgsell = 0.00M;
                        if ( dr[dsRisk.tblPositions.AvgSellColumn] != null)
                            avgsell = (decimal )dr[dsRisk.tblPositions.AvgSellColumn];

                        decimal  prcdiff = avgsell- avgbuy;
                        
                        decimal  exPtVal = (decimal )securityInfo[dsRisk.tblSecurity.ExchangePointValueColumn];
                        string contractCurrency = securityInfo[dsRisk.tblSecurity.CurrencyColumn].ToString();

                        decimal  currencyConversionrate = 1.00M;
                        if (conversion[contractCurrency] != null)
                        {
                            currencyConversionrate = (decimal )conversion[contractCurrency];
                        }

                        if (matchQty != 0)
                        {
                            dr[dsRisk.tblPositions.RealizedPLColumn] = Math.Round(matchQty * prcdiff * exPtVal * currencyConversionrate, 2);
                        }
                        else
                        { dr[dsRisk.tblPositions.RealizedPLColumn] = 0.00; }

                        if (remainQty < 0)
                        {
                            prcdiff = avgsell - ask;
                        }
                        else if (remainQty > 0)
                        {
                            prcdiff = bid - avgbuy;
                        }
                        else
                        { prcdiff = 0; }

                        dr[dsRisk.tblPositions.UnrealizedPLColumn] = Math.Round(Math.Abs(remainQty) * prcdiff * exPtVal * currencyConversionrate, 2);

                        dr.AcceptChanges();
                    }

                    if (totalPnL.ContainsKey(dr[0].ToString()))
                    { totalPnL[dr[0].ToString()] += ((decimal )dr[dsRisk.tblPositions.RealizedPLColumn] + (decimal )dr[dsRisk.tblPositions.UnrealizedPLColumn]); }
                    else
                    { totalPnL[dr[0].ToString()] = ((decimal )dr[dsRisk.tblPositions.RealizedPLColumn] + (decimal )dr[dsRisk.tblPositions.UnrealizedPLColumn]); }

                }

                foreach (KeyValuePair<string,decimal >  kvp in totalPnL)
                {
                    DataRow dr = dsRisk.tblTrader.Rows.Find(kvp.Key);

                    if (dr != null)
                    {
                        dr[dsRisk.tblTrader.TotalPLColumn] = kvp.Value;
                        dr.AcceptChanges();
                    }
                    else
                    { log.WriteList("TRADER NOT FOUND IN MGT.XML"); }
                }

                if (previousPositionsLoaded)
                {
                    checkPnLAction();
                }
            
            }
            catch (Exception ex)
            { log.WriteList(ex.ToString()); }
        }

        /// <summary>
        /// check what actions must be taken based on limits and P&L
        /// </summary>
        private void checkPnLAction()
        {
            try
            {
                log.WriteLog("mainForm::checkPnLAction: ");

                foreach (DataRow dr in dsRisk.tblTrader)
                {
                    //log.WriteList(string.Format("{0} < {1}", dr[dsRisk.tblTrader.TotalPLColumn], dr[dsRisk.tblTrader.LimitColumn]));

                    decimal  total;
                    bool isValid = decimal .TryParse(dr[dsRisk.tblTrader.TotalPLColumn].ToString(), out total);

                    if (isValid && (decimal )dr[dsRisk.tblTrader.TotalPLColumn] <= (decimal )dr[dsRisk.tblTrader.LimitColumn])
                    {
                        string MGT = dr[dsRisk.tblTrader.MGTColumn].ToString();
                        stoppedTraders.Add(MGT);

                        if (chkBxAUTO.Checked)
                        {
                            TT.SendMsg send = new TT.SendMsg();
                            send.ttOrderStatusRequest(); 
                        }
                        else
                        { log.WriteList("AUTO MODE DISABLED! Orders not canceled"); }

                        FlattenTrader(MGT);
                    }

                    if (isEmailEnabled)
                    {
                        int ActiveAlert = (int)dr[dsRisk.tblTrader.EmailAlertColumn];

                        if (isValid &&
                            ActiveAlert <= EmailLimits.GetUpperBound(0) &&
                            (decimal )dr[dsRisk.tblTrader.TotalPLColumn] < EmailLimits[ActiveAlert])
                        {

                            SendEmail(
                                to[ActiveAlert],
                                cc[ActiveAlert],
                                bcc[ActiveAlert],
                                msg[ActiveAlert],
                                subject[ActiveAlert] + Environment.NewLine + dr[dsRisk.tblTrader.MGTColumn].ToString(),
                                priority[ActiveAlert]);
                           
                            ActiveAlert++;
                            dr[dsRisk.tblTrader.EmailAlertColumn] = ActiveAlert;
                            dr.AcceptChanges();
                        }
                    }

                }
            }
            catch (Exception ex)
            { log.WriteList(ex.ToString()); }
        }

        /// <summary>
        /// Flatten all positions for a trader.
        /// </summary>
        /// <param name="MGT"></param>
        private void FlattenTrader(string MGT)
        {
            try
            {
                foreach (DataRow dr in dsRisk.tblPositions)
                {
                    if (dr[dsRisk.tblPositions.MGTColumn].Equals(MGT) &&
                        !dr[dsRisk.tblPositions.BuyPosColumn].Equals(dr[dsRisk.tblPositions.SellPosColumn]))
                    {
                        string account = dr[dsRisk.tblPositions.AccountColumn].ToString();
                        if (String.IsNullOrEmpty(account))
                        { account = dsRisk.tblTrader.Rows.Find(MGT).Field<string>(dsRisk.tblTrader.AccountColumn); }

                        int buys = (int)dr[dsRisk.tblPositions.BuyPosColumn];
                        int sells = (int)dr[dsRisk.tblPositions.SellPosColumn];
                        char bs = char.MinValue;

                        if (buys < sells)
                        { bs = QuickFix.Fields.Side.BUY; }
                        else
                        { bs = QuickFix.Fields.Side.SELL; }
                        decimal qty = Convert.ToDecimal(Math.Abs(buys - sells));

                        string SecEx = dr[dsRisk.tblPositions.SecurityExchangeColumn].ToString();
                        string symbol = dr[dsRisk.tblPositions.SymbolColumn].ToString();
                        string secID = dr[dsRisk.tblPositions.SecurityIDColumn].ToString();
                        string gateway = dr[dsRisk.tblPositions.ExchangeGatewayColumn].ToString();

                        if (TT.SendMsg.inflightOrders.Count == 0 && chkBxAUTO.Checked) 
                        {
                            TT.SendMsg send = new TT.SendMsg();
                            send.ttNewOrderSingle(account, SecEx,symbol, secID, qty, bs, gateway );
                        }
                        else if (!chkBxAUTO.Checked)
                        { log.WriteList("AUTO MODE DISABLED! No orders sent"); }

                    }
                }
            }
            catch (Exception ex)
            { log.WriteList(ex.ToString()); }
        }

        #region Receive Data or Instructions from QuickFix (running on another thread)
        
        /// <summary>
        /// clear all position information
        /// </summary>
        private void ClearPositions()
        { 
            dsRisk.tblPositions.Clear();
            log.WriteList("Positions Cleared");
        }

        /// <summary>
        /// Update contract prices
        /// </summary>
        /// <param name="SecEx"></param>
        /// <param name="product"></param>
        /// <param name="secID"></param>
        /// <param name="bid"></param>
        /// <param name="ask"></param>
        private void UpdatePrices(string SecEx, string product, string secID, decimal  bid, decimal  ask)
        {
            try
            {
                DataRow dr = dsRisk.tblSecurity.Rows.Find(new object[] { SecEx, product, secID });
                if (dr != null)
                {
                    if (bid != (decimal )dr[dsRisk.tblSecurity.BidPriceColumn] ||
                        ask != (decimal )dr[dsRisk.tblSecurity.AskPriceColumn])
                    {
                        dr[dsRisk.tblSecurity.BidPriceColumn] = bid;
                        dr[dsRisk.tblSecurity.AskPriceColumn] = ask;

                        dr.AcceptChanges();

                        CalcPnL(SecEx, product, secID, bid, ask);
                    }
                }
            }
            catch (Exception ex)
            { log.WriteList(ex.ToString()); }
        }

        /// <summary>
        /// update gateway status
        /// </summary>
        /// <param name="exch"></param>
        /// <param name="sub"></param>
        /// <param name="stat"></param>
        /// <param name="t"></param>
        private void UpdateGateway(string exch, string sub, string stat, string t)
        {
            try
            {
                if (dsRisk.tblGateways.Rows.Find(exch) == null)
                {
                    switch (sub)
                    {
                        case "PRICE":
                            dsRisk.tblGateways.AddtblGatewaysRow(exch, stat, null, null, null);
                            break;
                        case "ORDER":
                            dsRisk.tblGateways.AddtblGatewaysRow(exch, null, stat, null, null);
                            break;
                        case "FILL":
                            dsRisk.tblGateways.AddtblGatewaysRow(exch, null, null, stat, null);
                            break;
                        default:
                            dsRisk.tblGateways.AddtblGatewaysRow(exch, null, null, null, t);
                            break;
                    }
                }
                else
                {
                    DataRow dr = dsRisk.tblGateways.Rows.Find(exch);

                    dr[sub] = stat;
                    if (t != null)
                        dr[dsRisk.tblGateways.TEXTColumn] = t;

                    dr.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                log.WriteList(ex.ToString());
            }
        }

        /// <summary>
        /// Update contract information
        /// </summary>
        /// <param name="SecEx"></param>
        /// <param name="product"></param>
        /// <param name="secID"></param>
        /// <param name="CUR"></param>
        /// <param name="exPointValue"></param>
        private void UpdateSecurity(string SecEx, string product, string secID, string CUR, decimal exPointValue)
        {

            try
            {
                if (dsRisk.tblSecurity.Rows.Find(new object[] { SecEx, product, secID }) == null)
                {

                    dsRisk.tblSecurity.AddtblSecurityRow(SecEx, product, secID,
                    CUR, exPointValue, 0.00M, 0.00M);

                }
                else
                {
                    DataRow dr = dsRisk.tblSecurity.Rows.Find(new object[] { SecEx, product, secID });

                    if (CUR != null) { dr[dsRisk.tblSecurity.CurrencyColumn] = CUR; }
                    //if (exPointValue != decimal .NaN) { 
                    dr[dsRisk.tblSecurity.ExchangePointValueColumn] = exPointValue; 

                    //save changes
                    dr.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                log.WriteList(ex.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="MGT"></param>
        /// <param name="acct"></param>
        /// <param name="SecEx"></param>
        /// <param name="product"></param>
        /// <param name="secID"></param>
        /// <param name="tradesize"></param>
        /// <param name="side"></param>
        /// <param name="price"></param>
        /// <param name="gateway"></param>
        private void UpdatePosition(
            string MGT, 
            string acct,
            string SecEx, 
            string product, 
            string secID, 
            int tradesize, 
            char side,
            decimal  price,
            string gateway)
        {
            log.WriteLog("mainForm::UpdatePosition");

            //added to filter by account
            if (isAccountFilterEnabled)
            {
                string watchedAccount = dsRisk.tblTrader.Rows.Find(MGT).Field<string>(dsRisk.tblTrader.AccountColumn);
                if (watchedAccount == null || watchedAccount != acct)
                {
                    log.WriteList("Execution disregarded because account does not match");
                    return;
                }
            }
            //end account filter code
            
            int currentBuyPos = 0;
            int currentSellPos = 0;
            decimal  currentBuyPrc = 0.00M;
            decimal  currentSellPrc = 0.00M;
            int previousBuyPos = 0;
            int previousSellPos = 0;


            if (side == QuickFix.Fields.Side.SELL )
            {
                currentSellPos = tradesize;
                currentSellPrc = price;
            }
            else if (side == QuickFix.Fields.Side.BUY)
            {
                currentBuyPos = tradesize;
                currentBuyPrc = price;
            }
            else
            {
                log.WriteList("NO BuySell info");
                return;
            }

            try
            {
                if (dsRisk.tblPositions.Rows.Find(new object[] { MGT, SecEx, product, secID }) == null)
                {

                    //TODO why cast to double??
                    dsRisk.tblPositions.AddtblPositionsRow(MGT, SecEx, product, secID,
                    currentBuyPos, currentSellPos, (double)currentBuyPrc, (double)currentSellPrc, 0.00, 0.00, gateway, acct );

                    TT.SendMsg send = new TT.SendMsg();

                    send.ttSecurityDefinitionRequest(SecEx, product, secID);
                    send.ttMarketDataRequest(SecEx, product, secID);

                }
                else 
                {
                    DataRow dr = dsRisk.tblPositions.Rows.Find(new object[] { MGT, SecEx, product, secID });

                    dr[dsRisk.tblPositions.AccountColumn] = acct;

                    //existing buy/sell positions
                    previousBuyPos = (int)dr[dsRisk.tblPositions.BuyPosColumn];
                    previousSellPos = (int)dr[dsRisk.tblPositions.SellPosColumn];

                    dr[dsRisk.tblPositions.BuyPosColumn] = previousBuyPos + currentBuyPos;
                    dr[dsRisk.tblPositions.SellPosColumn] = previousSellPos + currentSellPos;


                    decimal  previousAvgBuyPrc = 0.00M;
                    if ( dr[dsRisk.tblPositions.AvgBuyColumn] != null)
                        previousAvgBuyPrc = (decimal )dr[dsRisk.tblPositions.AvgBuyColumn];


                    decimal  previousAvgSellPrc = 0.00M;
                    if ( dr[dsRisk.tblPositions.AvgSellColumn] != null)
                        previousAvgSellPrc = (decimal )dr[dsRisk.tblPositions.AvgSellColumn];


                    dr[dsRisk.tblPositions.AvgBuyColumn] = ((previousBuyPos * previousAvgBuyPrc) +
                                                            (currentBuyPos * currentBuyPrc)) / (previousBuyPos + currentBuyPos);


                    dr[dsRisk.tblPositions.AvgSellColumn] = ((previousSellPos * previousAvgSellPrc) +
                                                             (currentSellPos * currentSellPrc)) / (previousSellPos + currentSellPos);

                    // using decimals negates the need to check for NaN? 
                    // if (dr[dsRisk.tblPositions.AvgBuyColumn].Equals(decimal. .NaN)) { dr[dsRisk.tblPositions.AvgBuyColumn] = 0.00; }
                    // if (dr[dsRisk.tblPositions.AvgSellColumn].Equals(decimal .NaN)) { dr[dsRisk.tblPositions.AvgSellColumn] = 0.00; }

                    dr.AcceptChanges();


                }

                DataRow drPrc = dsRisk.tblSecurity.Rows.Find(new object[] { SecEx, product, secID });
                if (drPrc != null)
                {
                    decimal  bid = (decimal )drPrc[dsRisk.tblSecurity.BidPriceColumn];
                    decimal  ask = (decimal )drPrc[dsRisk.tblSecurity.AskPriceColumn];

                    CalcPnL(SecEx, product, secID, bid, ask);
                }
            }
            catch (Exception ex)
            {
                log.WriteList("Exception occured mainForm::UpdatePosition");
                log.WriteLog(ex.ToString());
            }


        }

        /// <summary>
        /// Application is designed to only submit one order at a time, when this order is filled 
        /// the application is enabled to send another if needed.  If order is rejected the application will also be 
        /// enabled to resubmit.
        /// </summary>
        /// <param name="ordID"></param>
        private void updateOrderMgmt(string ordID)
        {
            if (TT.SendMsg.inflightOrders.Contains(ordID))
                TT.SendMsg.inflightOrders.Remove(ordID);

        }

        /// <summary>
        /// Receive data from QuickFix to cancel all orders for a shutdown trader
        /// </summary>
        /// <param name="MGT"></param>
        /// <param name="key"></param>
        private void orderCancel(string MGT, string key)
        {
            if (stoppedTraders.Contains(MGT))
            {
                TT.SendMsg send = new TT.SendMsg();
                send.ttOrderCancelRequest(key); 
            }
        }

        #endregion


        /// <summary>
        /// send an email
        /// </summary>
        /// <param name="to">comma separated list of addresses</param>
        /// <param name="cc">comma separated list of addresses</param>
        /// <param name="bcc">comma separated list of addresses</param>
        /// <param name="msgSubject">subject text</param>
        /// <param name="msgBody">body text</param>
        /// <param name="mp">.NET MailPriotity enumeration</param>
        public void SendEmail(
            string to, 
            string cc,
            string bcc,
            string msgSubject, 
            string msgBody, 
            MailPriority mp
            )
        {
            //create the mail message
            System.Net.Mail.MailMessage mail = new MailMessage();

            mail.To.Add(to); 

            mail.CC.Add(cc); 

            mail.Bcc.Add(bcc); 

            mail.Subject = msgSubject;
            log.WriteLog(mail.Subject);

            mail.Body = msgBody;
            log.WriteLog(msgBody);

            mail.Priority = mp;

            try
            {
                DataRow email = tblParameters.Rows[0];

                mail.From = new MailAddress(email["FROM"].ToString());

                System.Net.Mail.SmtpClient smtp = new SmtpClient(email["SERVER"].ToString(), Convert.ToInt32(email["PORT"]));
                smtp.Credentials = new NetworkCredential(email["LOGIN"].ToString(), email["PWORD"].ToString());
                smtp.EnableSsl = Convert.ToBoolean(email["SSL"]);
                smtp.Send(mail);
                log.WriteList("Email Sent!");
            }
            catch (Exception ex)
            { log.WriteList(ex.ToString());            }

            mail.Dispose();
        }

        /// <summary>
        /// This event starts off ticking every second when application starts and at the end of first minute
        /// slows to checking time once per minute.
        /// If current time = the time Set on the application for reset; all p&L and positions are cleared off 
        /// and email alerts are reset to start at first alert.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!TimerUpdated &&
                DateTime.Now.TimeOfDay.Seconds == 0)
            {
                timer1.Interval = 60000;
                TimerUpdated = true;
            }

            if (dateTimePicker2.Value.TimeOfDay.Hours == DateTime.Now.TimeOfDay.Hours  &&
                dateTimePicker2.Value.TimeOfDay.Minutes == DateTime.Now.TimeOfDay.Minutes &&
                checkBox1.Checked)
            {
                dsRisk.tblPositions.Clear();
                

                foreach (DataRow dr in dsRisk.tblTrader)
                {
                    dr[dsRisk.tblTrader.TotalPLColumn] = 0;
                    dr[dsRisk.tblTrader.EmailAlertColumn] = 0;

                    dr.AcceptChanges();
                }

                log.WriteList("Position data cleared: P&L reset to zero.");
            }

            if (TimerUpdated)
            {
                if (current < DateTime.Now.Subtract(TimeSpan.FromSeconds(30)))
                { checkPnLAction(); }

            }
            
        }

        /// <summary>
        /// just a variable to know if we need to save a new time setting for the reset time.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            TimeChanged = true;

        }

        /// <summary>
        /// Disable/enable email alerts 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkBxEmail_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox c = (CheckBox)sender;

            if (c.Checked)
            { isEmailEnabled = true; }
            else
            { isEmailEnabled = false; }
        }


    }
}
