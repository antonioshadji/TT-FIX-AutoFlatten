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
using System.Windows.Forms;
using System.Threading;


namespace QuickFix
{
    public class QFApplication :  QuickFix.MessageCracker , QuickFix.IApplication 
    {
        private static LOG.LogFiles log = new LOG.LogFiles();
        private bool previousTradesLoaded = false;
        private bool SODLoaded = false;
        private bool ManualFillsLoaded = false;
        private bool PositionLoadedReported = false;
        private ArrayList posReports = new ArrayList();

        int ctrSOD = 0;
        int ctrMAN = 0;
        int ctrTRD = 0;

        private Control _control = null;
        public void setControl(Control c)
        {
            _control = c;
        }

        private string _password = null;
        public void password(string p)
        {
            _password = p;
        }

        private bool _resetSession = false;
        public void resetSession(bool r)
        {
            _resetSession = r;
        }

        private QuickFix.SessionSettings _settings = null;
        private QuickFix.FileStoreFactory _storeFactory = null;
        private QuickFix.FileLogFactory _logFactory = null;
        private QuickFix.FIX42.MessageFactory _messageFactory = null;
        private QuickFix.Transport.SocketInitiator _initiator = null;

        #region Delegates for transferring data to mainForm Thread
        //not used
        public delegate void ThreadSafePassMessage(QuickFix.Message msg, QuickFix.Session s);
        private ThreadSafePassMessage _tspm;
        public void registerMessagePasser(ThreadSafePassMessage method)
        {
            _tspm += method;
        }
        private void passMessage(QuickFix.Message msg, QuickFix.Session s)
        {
            if (_control.InvokeRequired)
            {
                _control.Invoke(_tspm, new Object[] { msg, s });
            }
            else
            {
                _tspm(msg, s);
            }
        }

        public delegate void ThreadSafeFormControl(string text);
        private ThreadSafeFormControl _tsfc;
        public void registerLogUpdater(ThreadSafeFormControl fc)
        {
            _tsfc += fc;
        }
        private void updateDisplay(string s)
        {
            if (_control.InvokeRequired)
            {
                _control.Invoke(_tsfc, new Object[] { s });
            }
            else
            {
                _tsfc(s);
            }
        }

        public delegate void threadSafeOnOff();
        private threadSafeOnOff _tsOnOff;
        public void registerOnOff(threadSafeOnOff fc)
        {
            _tsOnOff += fc;
        }
        private void updateOnOff()
        {
            if (_control.InvokeRequired)
            {
                _control.Invoke(_tsOnOff);
            }
            else
            {
                _tsOnOff();
            }
        }

        public delegate void threadSafeClearPOS();
        private threadSafeClearPOS _tsClearPOS;
        public void registerClearPOS(threadSafeClearPOS fc)
        {
            _tsClearPOS += fc;
        }
        private void updateClearPOS()
        {
            if (_control.InvokeRequired)
            {
                _control.Invoke(_tsClearPOS);
            }
            else
            {
                _tsClearPOS();
            }
        }

        public delegate void ThreadSafeUpdatePositions(
            string MGT,
            string acct,
            string SecEx, 
            string symbol, 
            string secID, 
            int tradesize, 
            char side,
            decimal  price,
            string gateway);
        
        private ThreadSafeUpdatePositions _tsUP;
        public void registerPositionUpdater(ThreadSafeUpdatePositions fc)
        {
            _tsUP += fc;
        }
        
        private void updatePosition(
            string MGT, 
            string acct,
            string SecEx, 
            string symbol, 
            string secID, 
            int tradesize, 
            char side, 
            decimal price,
            string gateway)
        {

            if (_control.InvokeRequired)
            {
                _control.Invoke(_tsUP, new Object[] { MGT, acct, SecEx, symbol, secID, tradesize, side, price, gateway });

            }
            else
            {
                log.WriteLog("No Invoke Required");
                _tsUP(MGT, acct, SecEx, symbol, secID, tradesize, side, price, gateway );
            }
        }

        public delegate void ThreadSafeUpdateSecurity(string SecEx, string symbol, string secID, string cur, decimal exPointValue);
        private ThreadSafeUpdateSecurity _tsUS;
        public void registerSecurityUpdater(ThreadSafeUpdateSecurity fc)
        {
            _tsUS += fc;
        }
        private void updateSecurity(string SecEx, string symbol, string secID, string cur, decimal exPointValue)
        {
            if (_control.InvokeRequired)
            {
                _control.Invoke(_tsUS, new Object[] { SecEx, symbol, secID, cur, exPointValue });
            }
            else
            {
                _tsUS(SecEx, symbol, secID, cur, exPointValue);
            }
        }

        public delegate void threadsafeUpdateGateways(string exch, string sub, string stat, string t);
        private threadsafeUpdateGateways _tsUG;
        public void registerGatewayUpdater(threadsafeUpdateGateways fc)
        {
            _tsUG += fc;
        }
        private void updateGateway(string exch, string sub, string stat, string t)
        {
            if (_control.InvokeRequired)
            {
                _control.Invoke(_tsUG, new Object[] { exch, sub, stat, t });
            }
            else
            {
                _tsUG(exch, sub, stat, t);
            }
        }

        public delegate void threadsafeUpdatePrices(string SecEx, string symbol, string secID, decimal bid, decimal ask);
        private threadsafeUpdatePrices _tsUPrc;
        public void registerPriceUpdater(threadsafeUpdatePrices fc)
        {
            _tsUPrc += fc;
        }
        private void updatePrices(string SecEx, string symbol, string secID, decimal bid, decimal ask)
        {
            if (_control.InvokeRequired)
            {
                _control.Invoke(_tsUPrc, new Object[] { SecEx, symbol, secID, bid, ask });
            }
            else
            {
                _tsUPrc(SecEx, symbol, secID, bid, ask);
            }
        }

        public delegate void threadsafeOrderCancel(string MGT, string TTOrderKey);
        private threadsafeOrderCancel _tsOC;
        public void registerOrderCanceler(threadsafeOrderCancel fc)
        {
            _tsOC += fc;
        }
        private void orderCancel(string MGT, string TTOrderKey)
        {
            if (_control.InvokeRequired)
            {
                _control.Invoke(_tsOC, new Object[] { MGT, TTOrderKey });
            }
            else
            {
                _tsOC(MGT, TTOrderKey);
            }
        }

        public delegate void threadsafeOrderMgmt(string text);
        private threadsafeOrderMgmt _tsom;
        private void updateOrderMgmt(string s)
        {
            if (_control.InvokeRequired)
            {
                _control.Invoke(_tsom, new Object[] { s });
            }
            else
            {
                _tsom(s);
            }
        }
        public void registerOrderMgmt(threadsafeOrderMgmt fc)
        {
            _tsom += fc;
        }

        #endregion

        public QFApplication()
        {
        }

        /// <summary>
        /// Initiate a connection through QuickFix to the TT Fix Adapter
        /// </summary>
        /// <param name="cfg">configuration file name</param>
        /// <param name="p">password</param>
        /// <param name="r">reset sequence numbers - always true for this application</param>
        /// <param name="c">the mainForm control</param>
        public void initiate(string cfg, string p, bool r, Control c)
        {
            log.CreateLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), null);
            

            try
            {
                _password = p;
                _resetSession = r;
                _control = c;

                _settings = new QuickFix.SessionSettings(cfg);
                _storeFactory = new QuickFix.FileStoreFactory(_settings);
                _logFactory = new QuickFix.FileLogFactory(_settings);
                _messageFactory = new QuickFix.FIX42.MessageFactory();

                _initiator = new QuickFix.Transport.SocketInitiator(this,
                    _storeFactory,
                    _settings,
                    _logFactory,
                    _messageFactory);

                _initiator.Start();
            }
            catch (Exception ex)
            { 
                updateDisplay("QuickFIX Error"); 
                log.WriteLog(ex.ToString());
            }

        }

        /// <summary>
        /// Simply parse out message data on the mainForm ListBox control and into a LOG file
        /// </summary>
        /// <param name="message"></param>
        /// <param name="sessionID"></param>
        public void ProcessMessage(QuickFix.Message message, QuickFix.SessionID sessionID)
        {
            
            try
            {
                updateDisplay(string.Format("{0} {1}", DateTime.Now.ToString("hh:mm:ss.fff"), message.GetType().FullName));

                foreach (int i in message.FieldOrder)
                {
                    updateDisplay(string.Format("TAG: {0} = {1}", i, message.GetField(i) ));
                }


                if (message.IsSetField(TT.NoGateways.FIELD))
                { updateDisplay("TT.NoGateways"); }

                if (message.IsSetField(TT.NoGatewayStatus.FIELD))
                { updateDisplay("TT.NoGatewayStatus"); }

                if (message.IsSetField(QuickFix.Fields.Tags.NoMDEntries))
                { updateDisplay("QuickFix.Fields.NoMDEntries"); }

                if (message.IsSetField(QuickFix.Fields.Tags.NoMDEntryTypes))
                { updateDisplay("QuickFix.Fields.NoMDEntryTypes"); }

            }
            catch (Exception ex)
            {
                updateDisplay("QuickFIX Error");
                log.WriteLog(ex.ToString());
            }
        }

        #region QuickFix Implementation

            public virtual void OnCreate(QuickFix.SessionID session)
            {
                updateDisplay(string.Format("onCreate: {0}", session));

                if (string.Equals(session.SessionQualifier.ToUpperInvariant(), "ORDER"))
                {
                    TT.SendMsg.orderSessionID = session;
                }

                if (string.Equals(session.SessionQualifier.ToUpperInvariant(), "PRICE"))
                {
                    TT.SendMsg.priceSessionID = session;
                }

                //log.WriteLog("onCreate: " + session.toString());
                //log.WriteLog("Qualifier: " + session.getSessionQualifier());
                //log.WriteLog("getSenderCompID: " + session.getSenderCompID());
                //log.WriteLog("getTargetCompID: " + session.getTargetCompID());
                
                ////application created with only one SenderCompID 
                //TT.SendMsg.SetSender(session.getSenderCompID());
            }

            public virtual void OnLogon(QuickFix.SessionID session)
            {
                log.WriteLog("onLogon: " + session.TargetCompID ); //FIX4.2:SenderCompID->TargetCompID:SessionQualifier

                if (string.Equals(session.SessionQualifier.ToUpperInvariant(), "ORDER"))
                {
                    //Add code to clear position table before downloading all trades
                    posReports.Clear();
                    updateClearPOS();

                    TT.SendMsg send = new TT.SendMsg();

                    send.ttGatewayStatusRequest(QuickFix.Fields.SubscriptionRequestType.SNAPSHOT_PLUS_UPDATES);

                    send.ttRequestForPosition(QuickFix.Fields.PosReqType.START_OF_DAYS);
                    send.ttRequestForPosition(QuickFix.Fields.PosReqType.DETAILED_START_OF_DAYS);
                    send.ttRequestForPosition(QuickFix.Fields.PosReqType.MANUAL_FILLS);
                    send.ttRequestForPosition(QuickFix.Fields.PosReqType.TRADES);
                    
                }

            }

            public virtual void OnLogout(QuickFix.SessionID sessionID)
            {
                try
                {
                    updateDisplay("onLogout: " + sessionID.TargetCompID);

                }
                catch (Exception ex)
                { 
                    updateDisplay("QuickFIX Error");
                    log.WriteLog(ex.ToString());
                }
            }

            public virtual void ToAdmin(QuickFix.Message message, QuickFix.SessionID sessionID)
            {
                try
                {
                    QuickFix.Fields.MsgType mt = new QuickFix.Fields.MsgType();
                    message.Header.GetField(mt);

                    if (mt.getValue() == QuickFix.Fields.MsgType.LOGON )
                    {
                        if (!String.IsNullOrEmpty(_password)) 
                        {
                            message.SetField(new QuickFix.Fields.RawData(_password));
                        }

                        if (_resetSession)
                        {
                            message.SetField(new QuickFix.Fields.ResetSeqNumFlag(true));
                        }
                    }
                }
                catch (Exception ex)
                {
                    updateDisplay("QuickFIX Error");
                    log.WriteLog(ex.ToString());
                }
            }

            public virtual void ToApp(QuickFix.Message message, QuickFix.SessionID sessionID)
            {
                try
                {
                    QuickFix.Fields.MsgType msgType = new QuickFix.Fields.MsgType();
                    string msg = message.Header.GetField(msgType).getValue();
                    
                    updateDisplay(string.Format("Message Sent: toApp {0}" , msg ));
                }
                catch (Exception ex)
                { 
                    updateDisplay("QuickFIX Error");
                    log.WriteLog(ex.ToString());
                }
            }

            public virtual void FromAdmin(QuickFix.Message message, QuickFix.SessionID sessionID)
            {
                Crack(message, sessionID);
            }

            public virtual void FromApp(QuickFix.Message message, QuickFix.SessionID session)
            {
                try
                {
                    QuickFix.Fields.MsgType msgType = new QuickFix.Fields.MsgType();
                    message.Header.GetField(msgType);
                    string msgTypeValue = msgType.getValue();

                    updateDisplay(string.Format("fromApp: {0}", msgTypeValue));
                    
                    //switch (msgTypeValue)
                    //{
                    //    case "UAT":
                    //        onGatewayStatusMessage((QuickFix.FIX42.Message)message, session);
                    //        break;
                    //    case "UAP":
                    //        onPositionReportMessage((QuickFix.FIX42.Message)message, session);
                    //        break;

                    //    default:
                            Crack(message, session);
                    //        break;
                    //}
                }
                catch (QuickFix.UnsupportedMessageType umt)
                {
                    updateDisplay("UnsupportedMessageType: " + umt.Message);
                    ProcessMessage(message, session);
                }
                catch (Exception ex)
                {
                    updateDisplay("QuickFIX Error");

                    log.WriteLog("Source: " + ex.Source);
                    log.WriteLog("Message: " + ex.Message);
                    log.WriteLog("TargetSite: " + ex.TargetSite);
                    log.WriteLog("InnerException: " + ex.InnerException);
                    log.WriteLog("HelpLink: " + ex.HelpLink);
                    log.WriteLog("Data: " + ex.Data);
                    log.WriteLog("StackTrace: " + ex.StackTrace);
                }
            } 

        #endregion

        #region application-level messages


        /// <summary>
        /// Helper Method to process data from message for updating positions
        /// </summary>
        /// <param name="message"></param>
        /// <param name="session"></param>
        private void posTableUpdate(QuickFix.FIX42.Message message, SessionID session)
        {
            string MGT = null;
            string acct = null;

            string SecEx = null;
            string symbol = null;
            string secID = null;
            string gateway = null;

            int tradesize = 0;
            char side = char.MinValue;
            decimal price = 0.00M;

            bool OK2Update = true;

            try
            {
                QuickFix.Fields.SenderSubID subID = new QuickFix.Fields.SenderSubID();
                if (message.Header.IsSetField(subID))
                {
                    MGT = message.Header.GetField(subID).getValue();
                }
                else
                { OK2Update = false; }

                QuickFix.Fields.Account a = new QuickFix.Fields.Account();
                if (message.IsSetField(a))
                {
                    acct = message.GetField(a).getValue();
                }
                else
                { OK2Update = false; }

                QuickFix.Fields.SecurityExchange se = new QuickFix.Fields.SecurityExchange();
                if (message.IsSetField(se))
                {
                    SecEx = message.GetField(se).getValue();
                }
                else
                { OK2Update = false; }

                QuickFix.Fields.Symbol s = new QuickFix.Fields.Symbol();
                if (message.IsSetField(s))
                {
                    symbol = message.GetField(s).getValue();
                }
                else
                { OK2Update = false; }

                QuickFix.Fields.SecurityID sid = new QuickFix.Fields.SecurityID();
                if (message.IsSetField(sid))
                {
                    secID = message.GetField(sid).getValue();
                }
                else
                { OK2Update = false; }

                QuickFix.Fields.ExchangeGateway eg = new Fields.ExchangeGateway();
                if (message.IsSetField(eg))
                {
                    gateway = message.GetField(eg).getValue();
                }
                else
                { OK2Update = false; }


                QuickFix.Fields.LastShares q = new QuickFix.Fields.LastShares();
                if (message.IsSetField(q))
                {
                    tradesize = (int)message.GetField(q).getValue();
                }

                QuickFix.Fields.Side bs = new QuickFix.Fields.Side();
                if (message.IsSetField(bs))
                {
                    side = message.GetField(bs).getValue();
                }
                else
                {
                    if (tradesize < 0)
                    {
                        tradesize = Math.Abs(tradesize);
                        side = QuickFix.Fields.Side.SELL;
                    }
                    else
                    { side = QuickFix.Fields.Side.BUY; }
                }

                QuickFix.Fields.LastPx fill = new QuickFix.Fields.LastPx();
                if (message.IsSetField(fill))
                {
                    price = message.GetField(fill).getValue();
                }

                if (OK2Update)
                {
                                 //MGT, acct, SecEx, symbol, secID, tradesize, side, price, gateway
                    updatePosition(MGT, acct, SecEx, symbol, secID, tradesize, side, price, gateway);
                }
                else
                {
                    updateDisplay("Position not updated by following message");
                    ProcessMessage(message, session);
                }
            }
            catch (Exception ex)
            {
                updateDisplay("QuickFIX Error");
                log.WriteLog("MGT: " + MGT);
                log.WriteLog("acct: " + acct);
                log.WriteLog("SecEx: " + SecEx);
                log.WriteLog("symbol: " + symbol);
                log.WriteLog("secID: " + secID);
                log.WriteLog("tradesize: " + tradesize.ToString());
                log.WriteLog("side: " + side.ToString());
                log.WriteLog("price: " + price.ToString());
                log.WriteLog("gateway: " + gateway);

                log.WriteLog("Start Exception--------------------");
                log.WriteLog("Source: " + ex.Source);
                log.WriteLog("Message: " + ex.Message);
                log.WriteLog("TargetSite: " + ex.TargetSite);
                log.WriteLog("InnerException: " + ex.InnerException);
                log.WriteLog("HelpLink: " + ex.HelpLink);
                log.WriteLog("Data: " + ex.Data);
                log.WriteLog("StackTrace: " + ex.StackTrace);
                log.WriteLog("Stop Exception details-------------");

            }

        }

        //not implemented
        public void OnMessage(QuickFix.FIX42.BusinessMessageReject message, QuickFix.SessionID session)
        {
            ProcessMessage(message, session);
        }

        //not implemented
        public void OnMessage(QuickFix.FIX42.MarketDataRequestReject message, SessionID session)
        {
            ProcessMessage(message, session);

        }

        //not implemented
        public void OnMessage(QuickFix.FIX42.MarketDataIncrementalRefresh message, SessionID session)
        {
            ProcessMessage(message, session);
        }

        //not implemented
        public void OnMessage(QuickFix.FIX42.OrderCancelReject message, SessionID session)
        {
            ProcessMessage(message, session);

        }

        //not implemented
        public void OnMessage(QuickFix.FIX42.SecurityStatus message, QuickFix.SessionID session)
        {
            ProcessMessage(message, session);
        }

        //receive fills, working orders, order rejects
        public void OnMessage(QuickFix.FIX42.ExecutionReport message, QuickFix.SessionID session)
        {
            //parseMessage(message, session);
            try
            {
                QuickFix.Fields.ExecType et = new QuickFix.Fields.ExecType();
                QuickFix.Fields.ExecTransType ett = new QuickFix.Fields.ExecTransType();
                QuickFix.Fields.ClOrdID coid = new QuickFix.Fields.ClOrdID();

                //Friday, December 11, 2009 10:29:27 AM Added parentheses to encapsulate OR
                //remove hold on orders if order was rejected
                if (message.IsSetField(coid) && 
                    (message.GetField(et).getValue() == QuickFix.Fields.ExecType.FILL ||
                    message.GetField(et).getValue() == QuickFix.Fields.ExecType.REJECTED))
                {
                    updateOrderMgmt(message.GetField(coid).getValue().ToString());
                }
                //end modified code

                if (message.IsSetField(et))
                {
                    //capture fill to update position
                    if ((message.GetField(et).getValue() == QuickFix.Fields.ExecType.FILL ||
                         message.GetField(et).getValue() == QuickFix.Fields.ExecType.PARTIAL_FILL) &&
                         message.MultiLegReportingType.getValue() != QuickFix.Fields.MultiLegReportingType.MULTI_LEG_SECURITY)
                    {
                        updateDisplay("execution received");
                        posTableUpdate(message, session);
                        
                    }

                    //capture working orders and cancel for flatten
                    if (message.GetField(et).getValue() == QuickFix.Fields.ExecType.RESTATED &&
                        message.GetField(ett).getValue() == QuickFix.Fields.ExecTransType.STATUS)
                    {

                        string MGT = null;
                        QuickFix.Fields.SenderSubID subID = new QuickFix.Fields.SenderSubID();
                        if (message.Header.IsSetField(subID))
                        {
                            MGT = message.Header.GetField(subID).ToString();
                            orderCancel(MGT, message.OrderID.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            { 
                updateDisplay("QuickFIX Error");
                log.WriteLog(ex.ToString());
            
            }
        }

        //Receive market data
        public void OnMessage(QuickFix.FIX42.MarketDataSnapshotFullRefresh message, SessionID session)
        {
            decimal _bidPrice = 0.00M;
            decimal _askPrice = 0.00M;

            try
            {
                QuickFix.Group noMDEntriesGrp = new QuickFix.FIX42.MarketDataSnapshotFullRefresh.NoMDEntriesGroup();


                for (int grpIndex = 1; grpIndex <= message.GetInt(QuickFix.Fields.Tags.NoMDEntries); grpIndex += 1)
                {
                    noMDEntriesGrp = message.GetGroup(grpIndex, QuickFix.Fields.Tags.NoMDEntries);

                    if (noMDEntriesGrp.IsSetField(QuickFix.Fields.Tags.BidPx))
                    {
                        _bidPrice = QuickFix.Fields.Converters.DecimalConverter.Convert(noMDEntriesGrp.GetField(QuickFix.Fields.Tags.BidPx));
                    }

                    if (noMDEntriesGrp.IsSetField(QuickFix.Fields.Tags.OfferPx))
                    {
                        _askPrice = QuickFix.Fields.Converters.DecimalConverter.Convert(noMDEntriesGrp.GetField(QuickFix.Fields.Tags.OfferPx));
                    }

                }

                string SecEx = null;
                string symbol = null;
                string secID = null;

                QuickFix.Fields.SecurityExchange se = new QuickFix.Fields.SecurityExchange();
                if (message.IsSetField(se))
                { SecEx = message.GetField(se).ToString(); }

                QuickFix.Fields.Symbol s = new QuickFix.Fields.Symbol();
                if (message.IsSetField(s))
                { symbol = message.GetField(s).ToString(); }

                QuickFix.Fields.SecurityID sid = new QuickFix.Fields.SecurityID();
                if (message.IsSetField(sid))
                { secID = message.GetField(sid).ToString(); }

                updatePrices(SecEx, symbol, secID, _bidPrice, _askPrice);
            }
            catch (Exception ex)
            { 
                updateDisplay(string.Format("QuickFIX Error: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name));
                log.WriteLog(string.Format("{0} : {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString()));
            }

        }

        //receive contract parameters
        public void OnMessage(QuickFix.FIX42.SecurityDefinition message, QuickFix.SessionID session)
        {
            //parseMessage(message, session);

            string SecEx = null;
            string symbol = null;
            string secID = null;

            try
            {
                QuickFix.Fields.SecurityExchange se = new QuickFix.Fields.SecurityExchange();
                if (message.IsSetField(se))
                { SecEx = message.GetField(se).ToString(); }

                QuickFix.Fields.Symbol s = new QuickFix.Fields.Symbol();
                if (message.IsSetField(s))
                { symbol = message.GetField(s).ToString(); }

                QuickFix.Fields.SecurityID sid = new QuickFix.Fields.SecurityID();
                if (message.IsSetField(sid))
                { secID = message.GetField(sid).ToString(); }

                string cur = null;
                decimal exPtVal = 0.00M;

                TT.ExchPointValue epv = new TT.ExchPointValue();
                if (message.IsSetField(epv))
                { exPtVal = message.GetField(epv).getValue(); }

                QuickFix.Fields.Currency ccy = new QuickFix.Fields.Currency();
                if (message.IsSetField(ccy))
                { cur = message.GetField(ccy).getValue(); }

                updateSecurity(SecEx, symbol, secID, cur, exPtVal);
            }
            catch (Exception ex)
            { 
                updateDisplay("QuickFIX Error");
                log.WriteLog(ex.ToString());
            }

        }

        public void OnMessage(QuickFix.FIX42.PositionReport message, QuickFix.SessionID session)
        {
            string reqID = message.PosReqId.ToString();
            log.WriteLog(reqID);
            int numReports = message.TotalNumPosReports.getValue();
            log.WriteLog(numReports.ToString());

            string uniqueID = message.PosMaintRptId.getValue();
            if (posReports.Contains(uniqueID))
            { updateDisplay("Duplicate position report recieved and discarded: " + uniqueID); }
            else
            {
                posReports.Add(uniqueID);

                if (numReports != 0)
                {
                    updateDisplay(string.Format("{0} updates in this {1} report", numReports, reqID));
                    posTableUpdate(message, session);
                }
                else
                { updateDisplay(string.Format("No Updates in this {0} position report", reqID)); }

                switch (reqID)
                {
                    case "SOD":
                        ctrSOD += 1;
                        if (numReports == 0 || ctrSOD == numReports) { SODLoaded = true; }

                        break;
                    case "DSOD":
                        ctrSOD += 1;
                        if (numReports == 0 || ctrSOD == numReports) { SODLoaded = true; }

                        break;
                    case "MANUAL_FILL":
                        ctrMAN += 1;
                        if (numReports == 0 || ctrMAN == numReports) { ManualFillsLoaded = true; }

                        break;
                    case "TRADES":
                        ctrTRD += 1;
                        if (numReports == 0 || ctrTRD == numReports) { previousTradesLoaded = true; }

                        break;
                    default:
                        break;
                }

                log.WriteLog("SOD Loaded: " + SODLoaded.ToString());
                log.WriteLog("MAN Loaded: " + ManualFillsLoaded.ToString());
                log.WriteLog("TRD Loaded: " + previousTradesLoaded.ToString());

                if (SODLoaded && ManualFillsLoaded && previousTradesLoaded && !PositionLoadedReported)
                {
                    updateOnOff();
                    updateDisplay("RISK MANUAL FILLS AND TRADES LOADED");
                    PositionLoadedReported = true;
                }
            }
        }
        
        //receive responses from position request messages
        //public void onPositionReportMessage(QuickFix.FIX42.Message message, SessionID session)
        //{

        //    TT.PosReqId req = new TT.PosReqId(); 
        //    string r = message.GetField(req).getValue(); //SOD, DSOD, MANUAL_FILL, TRADES
        //    log.WriteLog(r);

        //    TT.TotalNumPosReports num = new TT.TotalNumPosReports();
        //    int numReports = message.GetField(num).getValue() ;
        //    log.WriteLog(numReports.ToString());

        //    TT.PosMaintRptId uniqueID = new TT.PosMaintRptId();

        //    if (posReports.Contains(message.GetField(uniqueID).getValue()))
        //    { updateDisplay("Duplicate position report recieved and discarded: " + uniqueID.ToString()); }
        //    else
        //    {
        //        posReports.Add(message.GetField(uniqueID).getValue());

        //        if (message.IsSetField(num) && numReports != 0)
        //        {
        //            updateDisplay(string.Format("{0} updates in this {1} report",num, r));
        //            posTableUpdate(message, session);
        //        }
        //        else
        //        { updateDisplay(string.Format("No Updates in this {0} position report", r)); }

        //        switch (r)
        //        {
        //            case "SOD":
        //                ctrSOD += 1;
        //                if (numReports == 0 || ctrSOD == numReports) { SODLoaded = true; }

        //                break;
        //            case "DSOD":
        //                ctrSOD += 1;
        //                if (numReports == 0 || ctrSOD == numReports) { SODLoaded = true; }

        //                break;
        //            case "MANUAL_FILL":
        //                ctrMAN += 1;
        //                if (numReports == 0 || ctrMAN == numReports) { ManualFillsLoaded = true; }

        //                break;
        //            case "TRADES":
        //                ctrTRD += 1;
        //                if (numReports == 0 || ctrTRD == numReports) { previousTradesLoaded = true; }

        //                break;
        //            default:
        //                break;
        //        }

        //        log.WriteLog("SOD Loaded: " + SODLoaded.ToString());
        //        log.WriteLog("MAN Loaded: " + ManualFillsLoaded.ToString());
        //        log.WriteLog("TRD Loaded: " + previousTradesLoaded.ToString());
 
        //        if (SODLoaded && ManualFillsLoaded && previousTradesLoaded && !PositionLoadedReported)
        //        {
        //            updateOnOff();
        //            updateDisplay("RISK MANUAL FILLS AND TRADES LOADED");
        //            PositionLoadedReported = true;
        //        }
        //    }
        //}

        public void OnMessage(QuickFix.FIX42.GatewayStatus message, SessionID session)
        {
            QuickFix.Group g = new QuickFix.FIX42.GatewayStatus.NoGatewayStatusGroup();
            for (int i = 1; i <= message.GroupCount(message.NoGatewayStatus.getValue()); i++)
            {
                message.GetGroup(i, g);
                string exch = g.GetField(new QuickFix.Fields.ExchangeGateway()).getValue();
                int server = g.GetField(new QuickFix.Fields.SubExchangeGateway()).getValue();
                int status = g.GetField(new QuickFix.Fields.GatewayStatus()).getValue();
                string text = null;

                try
                {
                    text = g.GetField(new QuickFix.Fields.Text()).ToString();
                    updateDisplay(string.Format("Text: {0}", text));
                }
                catch (Exception ex)
                { updateDisplay(string.Format("NO TEXT:{0}", ex.ToString())); }

                updateGateway(exch, 
                    new QuickFix.Fields.SubExchangeGateway(server).toStringField().ToString(), 
                    new QuickFix.Fields.GatewayStatus(status).toStringField().ToString(),
                    text);
            }
        }

        //receive gateway status messages
        //public void onGatewayStatusMessage(QuickFix.FIX42.Message message, SessionID session)
        //{

        //    if (message.IsSetField(TT.NoGatewayStatus.FIELD))
        //    {
        //        //updateDisplay(string.Format("Found {0} NoGatewayStatus groups", message.groupCount(TT.NoGatewayStatus.FIELD)));
        //        QuickFix.Group g = new Group(18201, 18202, new int[] { 18202, 207, 18203, 18204, 58, 0 });

        //        for (int i = 1; i <= message.GroupCount(TT.NoGatewayStatus.FIELD); i++)
        //        {
        //            message.GetGroup(i, g);
        //            string exch = g.GetField(new TT.ExchangeGateway()).getValue();
        //            int server = g.GetField(new TT.SubExchangeGateway()).getValue();
        //            int status = g.GetField(new TT.GatewayStatus()).getValue();
        //            string text = null;
        //            //updateDisplay(string.Format("ExchangeGateway:{0}, SubExchangeGateway:{1},GatewayStatus:{0}", exch, server, status));

        //            if (g.IsSetField(new QuickFix.Fields.Text()))
        //            { 
        //                text = g.GetField(new QuickFix.Fields.Text()).ToString();
        //                updateDisplay(string.Format("Text: {0}", text)); 
        //            }

        //            updateGateway(exch, TT.SubExchangeGateway.getString(server), TT.GatewayStatus.getString(status),text);
                    
        //        }
        //    }


        //}

        #endregion
       
        #region Session Level Messages - NOT IMPLEMENETED
       
        public  void OnMessage(QuickFix.FIX42.Heartbeat message, SessionID session)
        {
            ProcessMessage(message, session);
        }

        public  void OnMessage(QuickFix.FIX42.TestRequest  message, SessionID session)
        {
           
            ProcessMessage(message, session);
        }

        public  void OnMessage(QuickFix.FIX42.ResendRequest message, SessionID session)
        {
            ProcessMessage(message, session);
        }

        public  void OnMessage(QuickFix.FIX42.Reject message, SessionID session)
        {
            ProcessMessage(message, session);
        }

        public  void OnMessage(QuickFix.FIX42.SequenceReset message, SessionID session)
        {
            //base.OnMessage(message, session); runs overidden functionality 
            ProcessMessage(message, session);
        }
        
        #endregion
    }
}
