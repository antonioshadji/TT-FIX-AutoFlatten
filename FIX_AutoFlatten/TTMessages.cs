//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace TT
{
    /// <summary>
    /// 
    /// </summary>
    public class SendMsg
    {
        private static LOG.LogFiles log = new LOG.LogFiles();
        /// <summary>
        /// 
        /// </summary>
        public static void initializeLog()
        { log.CreateLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), null); }

        /// <summary>
        /// 
        /// </summary>
        public static ArrayList inflightOrders = new ArrayList();
        /// <summary>
        /// FIX configuration
        /// </summary>
        public static QuickFix.SessionID priceSessionID;
        /// <summary>
        /// FIX configuration
        /// </summary>
        public static QuickFix.SessionID orderSessionID;

        private static int _uniqueid = 1;
        private string uniqueID()
        { return Convert.ToString(_uniqueid++); }

        /// <summary>
        /// Submit a FIX message to subscribe to gateway status.  Status is displayed but no action is attached.
        /// </summary>
        /// <param name="SubscriptionRequestType"></param>
        public void ttGatewayStatusRequest(char SubscriptionRequestType)
        {
            try
            {
                //Gateway Status Request:: new QuickFix.Fields.MsgType("UAR")
                QuickFix.FIX42.GatewayStatusRequest gsr = new QuickFix.FIX42.GatewayStatusRequest();
                gsr.SubscriptionRequestType.setValue(SubscriptionRequestType);

                gsr.GatewayStatusReqId.setValue(uniqueID()); 

                QuickFix.Session.SendToTarget(gsr, orderSessionID);
            }
            catch (Exception ex)
            { log.WriteLog(ex.ToString()); }
        }

        /// <summary>
        /// Submit FIX request to retreive all the days trades, SODs and manual Fills
        /// </summary>
        /// <param name="reqtype"></param>
        public void ttRequestForPosition(int reqtype)
        {
            try
            {
                //Request for Position :: new QuickFix.Fields.MsgType("UAN")
                QuickFix.FIX42.RequestForPosition rfp = new QuickFix.FIX42.RequestForPosition();

                rfp.SetField(new QuickFix.Fields.PosReqType(reqtype));
                switch (reqtype)
                {
                    case QuickFix.Fields.PosReqType.START_OF_DAYS:
                        rfp.PosReqId.setValue("SOD");
                        break;
                    case QuickFix.Fields.PosReqType.DETAILED_START_OF_DAYS:
                        rfp.PosReqId.setValue("DSOD");
                        break;
                    case QuickFix.Fields.PosReqType.MANUAL_FILLS:
                        rfp.PosReqId.setValue("MANUAL_FILL");
                        break;
                    case QuickFix.Fields.PosReqType.TRADES:
                        rfp.PosReqId.setValue("TRADES");
                        break;
                    default:
                        break;
                }

                QuickFix.Session.SendToTarget(rfp, orderSessionID);
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
        /// <param name="gateway">TT gateway name</param>
        public void ttNewOrderSingle(string _account, string SecEx, string symbol, string secID, decimal qty, char bs, string gateway)
        {
            try
            {
                //log.WriteLog(string.Format("{0} {1} {2} {3} {4} {5} {6}", _account, SecEx, symbol, secID, qty, bs, gateway));

                QuickFix.FIX42.NewOrderSingle nos = new QuickFix.FIX42.NewOrderSingle();

                string id = uniqueID();
                nos.Set(new QuickFix.Fields.ClOrdID(id));
                inflightOrders.Add(id);

                nos.Set(new QuickFix.Fields.SecurityExchange(SecEx));
                nos.Set(new QuickFix.Fields.Symbol(symbol));
                nos.Set(new QuickFix.Fields.SecurityID(secID));

                nos.Set(new QuickFix.Fields.OrderQty(qty));
                nos.Set(new QuickFix.Fields.Side(bs));
                nos.Set(new QuickFix.Fields.OrdType(QuickFix.Fields.OrdType.MARKET));
                nos.Set(new QuickFix.Fields.Account(_account));
                
                //To add a TT custom tag to a QuickFix Message you must use SetString or similar
                //the Set method of the QuickFix.FIX42 message only allows setting standard FIX 4.2 fields
                //SetString(TTAccountType.FIELD, TT.TTAccountType.M1);
                //TODO is ACCOUNT_IS_HOUSE_TRADER_AND_IS_CROSS_MARGINED = M1??
                nos.CustomerOrFirm.setValue(QuickFix.Fields.AccountType.ACCOUNT_IS_HOUSE_TRADER_AND_IS_CROSS_MARGINED);
                
                //Alternative code that can only be used if FA is setup to accept tag 47 and 204 instead of custom tag 18205
                //nos.Set(new QuickFix.Fields.Rule80A(QuickFix.Fields.Rule80A.AGENCY_SINGLE_ORDER));
                //nos.Set(new QuickFix.Fields.CustomerOrFirm(QuickFix.Fields.CustomerOrFirm.CUSTOMER));

                //required for environments with multiple gateways with same products
                nos.ExchangeGateway.setValue(gateway);

                QuickFix.Session.SendToTarget(nos, orderSessionID);

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
                QuickFix.FIX42.SecurityDefinitionRequest sdr = new QuickFix.FIX42.SecurityDefinitionRequest();

                sdr.Set(new QuickFix.Fields.SecurityReqID(string.Concat(SecEx, ":", symbol, ":", secID)));

                //sdr.Set(new QuickFix.Fields.SecurityRequestType(3));

                sdr.Set(new QuickFix.Fields.SecurityExchange(SecEx));
                sdr.Set(new QuickFix.Fields.Symbol(symbol));
                sdr.Set(new QuickFix.Fields.SecurityID(secID));

                //possible values include CS FOR FUT GOVT MLEG OPT NRG
                //sdr.Set(new QuickFix.Fields.SecurityType(QuickFix.Fields.SecurityType.FUTURE));

                //old code was:: SetField(new TT.RequestTickTable("Y"));
                sdr.RequestTickTable.setValue(true); 

                QuickFix.Session.SendToTarget(sdr, priceSessionID);

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
                QuickFix.FIX42.MarketDataRequest mdr = new QuickFix.FIX42.MarketDataRequest();

                mdr.Set(new QuickFix.Fields.MDReqID(string.Concat(SecEx, ":", symbol, ":", secID)));
                mdr.Set(new QuickFix.Fields.SubscriptionRequestType(QuickFix.Fields.SubscriptionRequestType.SNAPSHOT_PLUS_UPDATES));
                mdr.Set(new QuickFix.Fields.MDUpdateType(QuickFix.Fields.MDUpdateType.FULL_REFRESH)); //required if above type is SNAPSHOT_PLUS_UPDATES
                mdr.Set(new QuickFix.Fields.MarketDepth(1));
                mdr.Set(new QuickFix.Fields.AggregatedBook(true));

                QuickFix.FIX42.MarketDataRequest.NoMDEntryTypesGroup tgroup = new QuickFix.FIX42.MarketDataRequest.NoMDEntryTypesGroup();
                tgroup.Set(new QuickFix.Fields.MDEntryType(QuickFix.Fields.MDEntryType.BID));
                mdr.AddGroup(tgroup);
                tgroup.Set(new QuickFix.Fields.MDEntryType(QuickFix.Fields.MDEntryType.OFFER));
                mdr.AddGroup(tgroup);
                //tgroup.Set(new QuickFix.Fields.MDEntryType(QuickFix.Fields.MDEntryType.TRADE));
                //mdr.addGroup(tgroup);

                QuickFix.FIX42.MarketDataRequest.NoRelatedSymGroup sgroup = new QuickFix.FIX42.MarketDataRequest.NoRelatedSymGroup();

                sgroup.Set(new QuickFix.Fields.SecurityExchange(SecEx));
                sgroup.Set(new QuickFix.Fields.Symbol(symbol));
                sgroup.Set(new QuickFix.Fields.SecurityID(secID));
                mdr.AddGroup(sgroup);

                QuickFix.Session.SendToTarget(mdr, priceSessionID);
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
                QuickFix.FIX42.OrderStatusRequest osr = new QuickFix.FIX42.OrderStatusRequest();

                //filter by account - optional
                //osr.Set(new QuickFix.Fields.Account("sl002004"));
                //omit this for order book download
                //osr.Set(new QuickFix.Fields.ClOrdID("uniqueClOrdID"));
                //osr.Set(new QuickFix.Fields.OrderID("TTORDERKEY"));

                //Code modified on 12/15/2009 3:03:42 PM replace hardcoded session name with OrderTargetCompID
                QuickFix.Session.SendToTarget(osr, orderSessionID);


            }
            catch (Exception ex)
            { log.WriteLog(ex.ToString()); }
        }

        /// <summary>
        /// Send order cancel request message for order specified by TTOrderKey (QuickFix.Fields.OrderID)
        /// </summary>
        /// <param name="TTOrderkey"></param>
        public void ttOrderCancelRequest(string TTOrderkey)
        {
            try
            {
                QuickFix.FIX42.OrderCancelRequest ocr = new QuickFix.FIX42.OrderCancelRequest();


                ocr.Set(new QuickFix.Fields.ClOrdID(uniqueID()));
                ocr.Set(new QuickFix.Fields.OrderID(TTOrderkey));

                QuickFix.Session.SendToTarget(ocr, orderSessionID);

            }
            catch (Exception ex)
            { log.WriteLog(ex.ToString());}
        }

    }
}
