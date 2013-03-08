/***************************************************************************
 *    
 *      Copyright (c) 2009 Trading Technologies International, Inc.
 *                     All Rights Reserved Worldwide
 *
 *        * * *   S T R I C T L Y   P R O P R I E T A R Y   * * *
 *
 * WARNING:  This file is the confidential property of Trading Technologies
 * International, Inc. and is to be maintained in strict confidence.  For
 * use only by those with the express written permission and license from
 * Trading Technologies International, Inc.  Unauthorized reproduction,
 * distribution, use or disclosure of this file or any program (or document)
 * derived from it is prohibited by State and Federal law, and by local law
 * outside of the U.S. 
 *
 ***************************************************************************
 * $Date: 2009/12/04 15:00:00EST $
 * $Revision: 1.0 $
 ***************************************************************************/

using System;
using System.Collections;
using System.Windows.Forms;
using System.Threading;


namespace QuickFix
{
    public class QFApplication : QuickFix42.MessageCracker, QuickFix.Application
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
        private QuickFix42.MessageFactory _messageFactory = null;
        private QuickFix.ThreadedSocketInitiator _initiator = null;

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
            double price,
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
            double price,
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

        public delegate void ThreadSafeUpdateSecurity(string SecEx, string symbol, string secID, string cur, double exPointValue);
        private ThreadSafeUpdateSecurity _tsUS;
        public void registerSecurityUpdater(ThreadSafeUpdateSecurity fc)
        {
            _tsUS += fc;
        }
        private void updateSecurity(string SecEx, string symbol, string secID, string cur, double exPointValue)
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

        public delegate void threadsafeUpdatePrices(string SecEx, string symbol, string secID, double bid, double ask);
        private threadsafeUpdatePrices _tsUPrc;
        public void registerPriceUpdater(threadsafeUpdatePrices fc)
        {
            _tsUPrc += fc;
        }
        private void updatePrices(string SecEx, string symbol, string secID, double bid, double ask)
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
                _messageFactory = new QuickFix42.MessageFactory();

                _initiator = new QuickFix.ThreadedSocketInitiator(this,
                    _storeFactory,
                    _settings,
                    _logFactory,
                    _messageFactory);

                _initiator.start();
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
        public void parseMessage(QuickFix.Message message, QuickFix.SessionID sessionID)
        {
            
            try
            {
                updateDisplay(string.Format("{0} {1}", DateTime.Now.ToString("hh:mm:ss.fff"), message.GetType().FullName));

                foreach (QuickFix.Field f in message)
                {
                    updateDisplay(string.Format("TAG: {0} = {1}", f.getField(), f));
                }

                if (message.hasGroup(TT.NoGateways.FIELD))
                { updateDisplay("TT.NoGateways"); }

                if (message.hasGroup(TT.NoGatewayStatus.FIELD))
                { updateDisplay("TT.NoGatewayStatus"); }

                if (message.hasGroup(QuickFix.NoMDEntries.FIELD))
                { updateDisplay("QuickFix.NoMDEntries"); }

                if (message.hasGroup(QuickFix.NoMDEntryTypes.FIELD))
                { updateDisplay("QuickFix.NoMDEntryTypes"); }

            }
            catch (Exception ex)
            {
                updateDisplay("QuickFIX Error");
                log.WriteLog(ex.ToString());
            }
        }

        #region QuickFix Implementation

            public virtual void onCreate(QuickFix.SessionID session)
            {
                updateDisplay(string.Format("onCreate: {0}", session));

                if (string.Equals(session.getSessionQualifier().ToUpperInvariant(), "ORDER"))
                {
                    TT.SendMsg.orderSessionID = session;
                }

                if (string.Equals(session.getSessionQualifier().ToUpperInvariant(), "PRICE"))
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

            public virtual void onLogon(QuickFix.SessionID session)
            {
                log.WriteLog("onLogon: " + session.getTargetCompID()); //FIX4.2:SenderCompID->TargetCompID:SessionQualifier

                if (string.Equals(session.getSessionQualifier().ToUpperInvariant(), "ORDER"))
                {
                    //Add code to clear position table before downloading all trades
                    posReports.Clear();
                    updateClearPOS();

                    TT.SendMsg send = new TT.SendMsg();

                    send.ttGatewayStatusRequest(QuickFix.SubscriptionRequestType.SNAPSHOT_PLUS_UPDATES);

                    send.ttRequestForPosition(TT.PosReqType.SOD);
                    send.ttRequestForPosition(TT.PosReqType.DSOD);
                    send.ttRequestForPosition(TT.PosReqType.MANUAL_FILL);
                    send.ttRequestForPosition(TT.PosReqType.TRADES);
                    
                }

            }

            public virtual void onLogout(QuickFix.SessionID sessionID)
            {
                try
                {
                    updateDisplay("onLogout: " + sessionID.getTargetCompID());

                }
                catch (Exception ex)
                { 
                    updateDisplay("QuickFIX Error");
                    log.WriteLog(ex.ToString());
                }
            }

            public virtual void toAdmin(QuickFix.Message message, QuickFix.SessionID sessionID)
            {
                try
                {
                    QuickFix.MsgType mt = new QuickFix.MsgType();
                    message.getHeader().getField(mt);

                    if (mt.getValue() == QuickFix.MsgType.Logon )
                    {
                        if (!_password.Equals(""))
                        {
                            message.setField(new QuickFix.RawData(_password));
                        }

                        if (_resetSession)
                        {
                            message.setField(new QuickFix.ResetSeqNumFlag(true));
                        }
                    }
                }
                catch (Exception ex)
                {
                    updateDisplay("QuickFIX Error");
                    log.WriteLog(ex.ToString());
                }
            }

            public virtual void toApp(QuickFix.Message message, QuickFix.SessionID sessionID)
            {
                try
                {
                    QuickFix.MsgType msgType = new QuickFix.MsgType();
                    string msg = message.getHeader().getField(msgType).getValue();
                    
                    updateDisplay(string.Format("Message Sent: toApp {0}" , msg ));
                }
                catch (Exception ex)
                { 
                    updateDisplay("QuickFIX Error");
                    log.WriteLog(ex.ToString());
                }
            }

            public virtual void fromAdmin(QuickFix.Message message, QuickFix.SessionID sessionID)
            {
                crack(message, sessionID);
            }

            public virtual void fromApp(QuickFix.Message message, QuickFix.SessionID session)
            {
                try
                {
                    QuickFix.MsgType msgType = new QuickFix.MsgType();
                    message.getHeader().getField(msgType);
                    string msgTypeValue = msgType.getValue();

                    updateDisplay(string.Format("fromApp: {0}", msgTypeValue));
                    
                    switch (msgTypeValue)
                    {
                        case "UAT":
                            onGatewayStatusMessage((QuickFix42.Message)message, session);
                            break;
                        case "UAP":
                            onPositionReportMessage((QuickFix42.Message)message, session);
                            break;

                        default:
                            crack(message, session);
                            break;
                    }
                }
                catch (QuickFix.UnsupportedMessageType umt)
                {
                    updateDisplay("UnsupportedMessageType: " + umt.Message);
                    parseMessage(message, session);
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
        private void posTableUpdate(QuickFix42.Message message, SessionID session)
        {
            string MGT = null;
            string acct = null;

            string SecEx = null;
            string symbol = null;
            string secID = null;
            string gateway = null;

            int tradesize = 0;
            char side = char.MinValue;
            double price = 0.00;

            bool OK2Update = true;

            try
            {
                QuickFix.SenderSubID subID = new QuickFix.SenderSubID();
                if (message.getHeader().isSetField(subID))
                {
                    MGT = message.getHeader().getField(subID).getValue();
                }
                else
                { OK2Update = false; }

                QuickFix.Account a = new QuickFix.Account();
                if (message.isSetField(a))
                {
                    acct = message.getField(a).getValue();
                }
                else
                { OK2Update = false; }

                QuickFix.SecurityExchange se = new QuickFix.SecurityExchange();
                if (message.isSetField(se))
                {
                    SecEx = message.getField(se).getValue();
                }
                else
                { OK2Update = false; }

                QuickFix.Symbol s = new QuickFix.Symbol();
                if (message.isSetField(s))
                {
                    symbol = message.getField(s).getValue();
                }
                else
                { OK2Update = false; }

                QuickFix.SecurityID sid = new QuickFix.SecurityID();
                if (message.isSetField(sid))
                {
                    secID = message.getField(sid).getValue();
                }
                else
                { OK2Update = false; }

                TT.ExchangeGateway eg = new TT.ExchangeGateway();
                if (message.isSetField(eg))
                {
                    gateway = message.getField(eg).getValue();
                }
                else
                { OK2Update = false; }


                QuickFix.LastShares q = new QuickFix.LastShares();
                if (message.isSetField(q))
                {
                    tradesize = (int)message.getField(q).getValue();
                }

                QuickFix.Side bs = new QuickFix.Side();
                if (message.isSetField(bs))
                {
                    side = message.getField(bs).getValue();
                }
                else
                {
                    if (tradesize < 0)
                    {
                        tradesize = Math.Abs(tradesize);
                        side = QuickFix.Side.SELL;
                    }
                    else
                    { side = QuickFix.Side.BUY; }
                }

                QuickFix.LastPx fill = new QuickFix.LastPx();
                if (message.isSetField(fill))
                {
                    price = message.getField(fill).getValue();
                }

                if (OK2Update)
                {
                                 //MGT, acct, SecEx, symbol, secID, tradesize, side, price, gateway
                    updatePosition(MGT, acct, SecEx, symbol, secID, tradesize, side, price, gateway);
                }
                else
                {
                    updateDisplay("Position not updated by following message");
                    parseMessage(message, session);
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
        public override void onMessage(QuickFix42.BusinessMessageReject message, QuickFix.SessionID session)
        {
            parseMessage(message, session);
        }

        //not implemented
        public override void onMessage(QuickFix42.MarketDataRequestReject message, SessionID session)
        {
            parseMessage(message, session);

        }

        //not implemented
        public override void onMessage(QuickFix42.MarketDataIncrementalRefresh message, SessionID session)
        {
            parseMessage(message, session);
        }

        //not implemented
        public override void onMessage(QuickFix42.OrderCancelReject message, SessionID session)
        {
            parseMessage(message, session);

        }

        //not implemented
        public override void onMessage(QuickFix42.SecurityStatus message, QuickFix.SessionID session)
        {
            parseMessage(message, session);
        }

        //receive fills, working orders, order rejects
        public override void onMessage(QuickFix42.ExecutionReport message, QuickFix.SessionID session)
        {
            //parseMessage(message, session);
            try
            {
                QuickFix.ExecType et = new QuickFix.ExecType();
                QuickFix.ExecTransType ett = new QuickFix.ExecTransType();
                QuickFix.ClOrdID coid = new ClOrdID();

                //Friday, December 11, 2009 10:29:27 AM Added parentheses to encapsulate OR
                //remove hold on orders if order was rejected
                if (message.isSetField(coid) && 
                    (message.getField(et).getValue() == QuickFix.ExecType.FILL ||
                    message.getField(et).getValue() == QuickFix.ExecType.REJECTED))
                {
                    updateOrderMgmt(message.getField(coid).getValue().ToString());
                }
                //end modified code

                if (message.isSetField(et))
                {
                    //capture fill to update position
                    if ((message.getField(et).getValue() == QuickFix.ExecType.FILL ||
                         message.getField(et).getValue() == QuickFix.ExecType.PARTIAL_FILL) &&
                         message.getMultiLegReportingType().getValue() != QuickFix.MultiLegReportingType.MULTI_LEG_SECURITY)
                    {
                        updateDisplay("execution received");
                        posTableUpdate(message, session);
                        
                    }

                    //capture working orders and cancel for flatten
                    if (message.getField(et).getValue() == QuickFix.ExecType.RESTATED &&
                        message.getField(ett).getValue() == QuickFix.ExecTransType.STATUS)
                    {

                        string MGT = null;
                        QuickFix.SenderSubID subID = new QuickFix.SenderSubID();
                        if (message.getHeader().isSetField(subID))
                        {
                            MGT = message.getHeader().getField(subID).ToString();
                            orderCancel(MGT, message.getOrderID().ToString());
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
        public override void onMessage(QuickFix42.MarketDataSnapshotFullRefresh message, SessionID session)
        {
            //parseMessage(message, session);
            double _bidPrice = 0.00;
            double _askPrice = 0.00;

            try
            {
                if (message.hasGroup(new QuickFix42.MarketDataSnapshotFullRefresh.NoMDEntries()))
                {
                    QuickFix42.MarketDataSnapshotFullRefresh.NoMDEntries g = new QuickFix42.MarketDataSnapshotFullRefresh.NoMDEntries();
                    QuickFix.NoMDEntries nEntries = new QuickFix.NoMDEntries();
                    message.getField(nEntries);

                    for (uint i = 1; i <= message.groupCount(QuickFix.NoMDEntries.FIELD); i++)
                    {
                        message.getGroup(i, g);

                        if (g.get(new QuickFix.MDEntryType()).getValue() == QuickFix.MDEntryType.BID)
                        { _bidPrice = g.get(new QuickFix.MDEntryPx()).getValue(); }

                        if (g.get(new QuickFix.MDEntryType()).getValue() == QuickFix.MDEntryType.OFFER)
                        { _askPrice = g.get(new QuickFix.MDEntryPx()).getValue(); }
                    }
                }

                string SecEx = null;
                string symbol = null;
                string secID = null;

                QuickFix.SecurityExchange se = new QuickFix.SecurityExchange();
                if (message.isSetField(se))
                { SecEx = message.getField(se).ToString(); }

                QuickFix.Symbol s = new QuickFix.Symbol();
                if (message.isSetField(s))
                { symbol = message.getField(s).ToString(); }

                QuickFix.SecurityID sid = new QuickFix.SecurityID();
                if (message.isSetField(sid))
                { secID = message.getField(sid).ToString(); }

                updatePrices(SecEx, symbol, secID, _bidPrice, _askPrice);
            }
            catch (Exception ex)
            { 
                updateDisplay("QuickFIX Error");
                log.WriteLog(ex.ToString());
            }

        }

        //receive contract parameters
        public override void onMessage(QuickFix42.SecurityDefinition message, QuickFix.SessionID session)
        {
            //parseMessage(message, session);

            string SecEx = null;
            string symbol = null;
            string secID = null;

            try
            {
                QuickFix.SecurityExchange se = new QuickFix.SecurityExchange();
                if (message.isSetField(se))
                { SecEx = message.getField(se).ToString(); }

                QuickFix.Symbol s = new QuickFix.Symbol();
                if (message.isSetField(s))
                { symbol = message.getField(s).ToString(); }

                QuickFix.SecurityID sid = new QuickFix.SecurityID();
                if (message.isSetField(sid))
                { secID = message.getField(sid).ToString(); }

                string cur = null;
                double exPtVal = 0.00;

                TT.ExchPointValue epv = new TT.ExchPointValue();
                if (message.isSetField(epv))
                { exPtVal = message.getField(epv).getValue(); }

                QuickFix.Currency ccy = new QuickFix.Currency();
                if (message.isSetField(ccy))
                { cur = message.getField(ccy).getValue(); }

                updateSecurity(SecEx, symbol, secID, cur, exPtVal);
            }
            catch (Exception ex)
            { 
                updateDisplay("QuickFIX Error");
                log.WriteLog(ex.ToString());
            }

        }

        //receive responses from position request messages
        public void onPositionReportMessage(QuickFix42.Message message, SessionID session)
        {

            TT.PosReqId req = new TT.PosReqId(); 
            string r = message.getField(req).getValue(); //SOD, DSOD, MANUAL_FILL, TRADES
            log.WriteLog(r);

            TT.TotalNumPosReports num = new TT.TotalNumPosReports();
            int numReports = message.getField(num).getValue() ;
            log.WriteLog(numReports.ToString());

            TT.PosMaintRptId uniqueID = new TT.PosMaintRptId();

            if (posReports.Contains(message.getField(uniqueID).getValue()))
            { updateDisplay("Duplicate position report recieved and discarded: " + uniqueID.ToString()); }
            else
            {
                posReports.Add(message.getField(uniqueID).getValue());

                if (message.isSetField(num) && numReports != 0)
                {
                    updateDisplay(string.Format("{0} updates in this {1} report",num, r));
                    posTableUpdate(message, session);
                }
                else
                { updateDisplay(string.Format("No Updates in this {0} position report", r)); }

                switch (r)
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

        //receive gateway status messages
        public void onGatewayStatusMessage(QuickFix42.Message message, SessionID session)
        {

            if (message.hasGroup(TT.NoGatewayStatus.FIELD))
            {
                //updateDisplay(string.Format("Found {0} NoGatewayStatus groups", message.groupCount(TT.NoGatewayStatus.FIELD)));
                QuickFix.Group g = new Group(18201, 18202, new int[] { 18202, 207, 18203, 18204, 58, 0 });

                for (uint i = 1; i <= message.groupCount(TT.NoGatewayStatus.FIELD); i++)
                {
                    message.getGroup(i, g);
                    string exch = g.getField(new TT.ExchangeGateway()).getValue();
                    int server = g.getField(new TT.SubExchangeGateway()).getValue();
                    int status = g.getField(new TT.GatewayStatus()).getValue();
                    string text = null;
                    //updateDisplay(string.Format("ExchangeGateway:{0}, SubExchangeGateway:{1},GatewayStatus:{0}", exch, server, status));

                    if (g.isSetField(new QuickFix.Text()))
                    { 
                        text = g.getField(new QuickFix.Text()).ToString();
                        updateDisplay(string.Format("Text: {0}", text)); 
                    }

                    updateGateway(exch, TT.SubExchangeGateway.getString(server), TT.GatewayStatus.getString(status),text);
                    
                }
            }


        }

        #endregion
       
        #region Session Level Messages - NOT IMPLEMENETED
       
        public override void onMessage(QuickFix42.Heartbeat message, SessionID session)
        {
            parseMessage(message, session);
        }

        public override void onMessage(QuickFix42.TestRequest  message, SessionID session)
        {
           
            parseMessage(message, session);
        }

        public override void onMessage(QuickFix42.ResendRequest message, SessionID session)
        {
            parseMessage(message, session);
        }

        public override void onMessage(QuickFix42.Reject message, SessionID session)
        {
            parseMessage(message, session);
        }

        public override void onMessage(QuickFix42.SequenceReset message, SessionID session)
        {
            //base.onMessage(message, session); runs overidden functionality 
            parseMessage(message, session);
        }
        
        #endregion
    }
}
