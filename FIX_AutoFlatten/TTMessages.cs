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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace TT
{
    public class SendMsg
    {
        private static LOG.LogFiles log = new LOG.LogFiles();
        public static void initializeLog()
        { log.CreateLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), null); }

        public static ArrayList inflightOrders = new ArrayList();

        public static QuickFix.SessionID priceSessionID;
        public static QuickFix.SessionID orderSessionID;

        private static int _uniqueid = 1;
        private string uniqueID()
        { return Convert.ToString(_uniqueid++); }

        /// <summary>
        /// Submit a FIX message to subscribe to gateway status.  Status is displayed but no action is attached.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="SubscriptionRequestType"></param>
        public void ttGatewayStatusRequest(char SubscriptionRequestType)
        {
            try
            {
                //Gateway Status Request
                QuickFix42.Message gsr = new QuickFix42.Message(new QuickFix.MsgType("UAR"));
                gsr.setChar(QuickFix.SubscriptionRequestType.FIELD, SubscriptionRequestType);

                gsr.setField(new TT.GatewayStatusReqId(uniqueID()));

                QuickFix.Session.sendToTarget(gsr, orderSessionID);
            }
            catch (Exception ex)
            { log.WriteLog(ex.ToString()); }
        }

        /// <summary>
        /// Submit FIX request to retreive all the days trades, SODs and manual Fills
        /// </summary>
        /// <param name="session"></param>
        /// <param name="reqtype"></param>
        public void ttRequestForPosition(int reqtype)
        {
            try
            {
                //Request for Position
                QuickFix42.Message rfp = new QuickFix42.Message(new QuickFix.MsgType("UAN"));

                rfp.setField(new TT.PosReqType(reqtype));
                switch (reqtype)
                {
                    case TT.PosReqType.SOD:
                        rfp.setField(new TT.PosReqId("SOD"));
                        break;
                    case TT.PosReqType.DSOD:
                        rfp.setField(new TT.PosReqId("DSOD"));
                        break;
                    case TT.PosReqType.MANUAL_FILL:
                        rfp.setField(new TT.PosReqId("MANUAL_FILL"));
                        break;
                    case TT.PosReqType.TRADES:
                        rfp.setField(new TT.PosReqId("TRADES"));
                        break;
                    default:
                        break;
                }

                QuickFix.Session.sendToTarget(rfp, orderSessionID);
            }
            catch (Exception ex)
            { log.WriteLog(ex.ToString()); }
        }

        /// <summary>
        /// Send a market order to flatten position 
        /// </summary>
        /// <param name="_account">account number must be read in from MGT.XML file</param>
        /// <param name="SecEx">Exchange</param>
        /// <param name="symbol">Exchange symbol for contract</param>
        /// <param name="secID">unique identifier supplied by the exchnage for this contract 
        /// For eurex this is the ticker and expiration</param>
        /// <param name="qty">order quantity</param>
        /// <param name="bs">buy or sell</param>
        public void ttNewOrderSingle(string _account, string SecEx, string symbol, string secID, double qty, char bs, string gateway)
        {
            try
            {
                //log.WriteLog(string.Format("{0} {1} {2} {3} {4} {5} {6}", _account, SecEx, symbol, secID, qty, bs, gateway));

                QuickFix42.NewOrderSingle nos = new QuickFix42.NewOrderSingle();

                string id = uniqueID();
                nos.set(new QuickFix.ClOrdID(id));
                inflightOrders.Add(id);

                nos.set(new QuickFix.SecurityExchange(SecEx));
                nos.set(new QuickFix.Symbol(symbol));
                nos.set(new QuickFix.SecurityID(secID));

                nos.set(new QuickFix.OrderQty(qty));
                nos.set(new QuickFix.Side(bs));
                nos.set(new QuickFix.OrdType(QuickFix.OrdType.MARKET));
                nos.set(new QuickFix.Account(_account));
                
                //To add a TT custom tag to a QuickFix Message you must use setString or similar
                //the set method of the QuickFix42 message only allows setting standard FIX 4.2 fields
                nos.setString(TT.TTAccountType.FIELD, TT.TTAccountType.M1);

                //Alternative code that can only be used if FA is setup to accept tag 47 and 204 instead of custom tag 18205
                //nos.set(new QuickFix.Rule80A(QuickFix.Rule80A.AGENCY_SINGLE_ORDER));
                //nos.set(new QuickFix.CustomerOrFirm(QuickFix.CustomerOrFirm.CUSTOMER));

                //required for environments with multiple gateways with same products
                nos.setString(TT.ExchangeGateway.FIELD, gateway);

                QuickFix.Session.sendToTarget(nos, orderSessionID);

            }
            catch (Exception ex)
            { log.WriteLog(ex.ToString()); }
        }

        /// <summary>
        /// Request security definiton to capture the exchange point value for the contract.
        /// </summary>
        /// <param name="SecEx">Exchange</param>
        /// <param name="symbol">Exchange symbol for contract</param>
        /// <param name="secID">unique identifier supplied by the exchnage for this contract 
        /// For eurex this is the ticker and expiration</param>
        public void ttSecurityDefinitionRequest(string SecEx, string symbol, string secID)
        {
            try
            {
                QuickFix42.SecurityDefinitionRequest sdr = new QuickFix42.SecurityDefinitionRequest();

                sdr.set(new QuickFix.SecurityReqID(string.Concat(SecEx, ":", symbol, ":", secID)));

                //sdr.set(new QuickFix.SecurityRequestType(3));

                sdr.set(new QuickFix.SecurityExchange(SecEx));
                sdr.set(new QuickFix.Symbol(symbol));
                sdr.set(new QuickFix.SecurityID(secID));

                //possible values include CS FOR FUT GOVT MLEG OPT NRG
                //sdr.set(new QuickFix.SecurityType(QuickFix.SecurityType.FUTURE));

                sdr.setField(new TT.RequestTickTable("Y"));

                QuickFix.Session.sendToTarget(sdr, priceSessionID);

            }
            catch (Exception ex)
            { log.WriteLog(ex.ToString()); }
        }

        /// <summary>
        /// Request Bid and offer prices subscription for a specific contract.
        /// </summary>
        /// <param name="SecEx">Exchange</param>
        /// <param name="symbol">Exchange symbol for contract</param>
        /// <param name="secID">unique identifier supplied by the exchnage for this contract 
        /// For eurex this is the ticker and expiration</param>
        public void ttMarketDataRequest(string SecEx, string symbol, string secID)
        {
            try
            {
                QuickFix42.MarketDataRequest mdr = new QuickFix42.MarketDataRequest();

                mdr.set(new QuickFix.MDReqID(string.Concat(SecEx, ":", symbol, ":", secID)));
                mdr.set(new QuickFix.SubscriptionRequestType(QuickFix.SubscriptionRequestType.SNAPSHOT_PLUS_UPDATES));
                mdr.set(new QuickFix.MDUpdateType(QuickFix.MDUpdateType.FULL_REFRESH)); //required if above type is SNAPSHOT_PLUS_UPDATES
                mdr.set(new QuickFix.MarketDepth(1));
                mdr.set(new QuickFix.AggregatedBook(true));

                QuickFix42.MarketDataRequest.NoMDEntryTypes tgroup = new QuickFix42.MarketDataRequest.NoMDEntryTypes();
                tgroup.set(new QuickFix.MDEntryType(QuickFix.MDEntryType.BID));
                mdr.addGroup(tgroup);
                tgroup.set(new QuickFix.MDEntryType(QuickFix.MDEntryType.OFFER));
                mdr.addGroup(tgroup);
                //tgroup.set(new QuickFix.MDEntryType(QuickFix.MDEntryType.TRADE));
                //mdr.addGroup(tgroup);

                QuickFix42.MarketDataRequest.NoRelatedSym sgroup = new QuickFix42.MarketDataRequest.NoRelatedSym();

                sgroup.set(new QuickFix.SecurityExchange(SecEx));
                sgroup.set(new QuickFix.Symbol(symbol));
                sgroup.set(new QuickFix.SecurityID(secID));
                mdr.addGroup(sgroup);

                QuickFix.Session.sendToTarget(mdr, priceSessionID);
            }
            catch (Exception ex)
            { log.WriteLog(ex.ToString()); }
        }

        /// <summary>
        /// Request all working orders so that they may be canceled
        /// </summary>
        public void ttOrderStatusRequest()
        {
            try
            {
                QuickFix42.OrderStatusRequest osr = new QuickFix42.OrderStatusRequest();

                //filter by account - optional
                //osr.set(new QuickFix.Account("sl002004"));
                //omit this for order book download
                //osr.set(new QuickFix.ClOrdID("uniqueClOrdID"));
                //osr.set(new QuickFix.OrderID("TTORDERKEY"));

                //Code modified on 12/15/2009 3:03:42 PM replace hardcoded session name with OrderTargetCompID
                QuickFix.Session.sendToTarget(osr, orderSessionID);


            }
            catch (Exception ex)
            { log.WriteLog(ex.ToString()); }
        }

        /// <summary>
        /// Send order cancel request message for order specified by TTOrderKey (QuickFix.OrderID)
        /// </summary>
        /// <param name="TTOrderkey"></param>
        public void ttOrderCancelRequest(string TTOrderkey)
        {
            try
            {
                QuickFix42.OrderCancelRequest ocr = new QuickFix42.OrderCancelRequest();


                ocr.set(new QuickFix.ClOrdID(uniqueID()));
                ocr.set(new QuickFix.OrderID(TTOrderkey));

                QuickFix.Session.sendToTarget(ocr, orderSessionID);

            }
            catch (Exception ex)
            { log.WriteLog(ex.ToString());}
        }

    }
}
