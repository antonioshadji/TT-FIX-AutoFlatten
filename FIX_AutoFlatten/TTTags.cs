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

namespace TT
{
    public class OrderEnteredTime : QuickFix.UtcTimeStampField
    {
        public OrderEnteredTime() : base(6038) { }
        public OrderEnteredTime(DateTime data) : base(6038, data) { }

        public const int FIELD = 6038;
    }

    public class ExpectedFillType : QuickFix.IntField
    {
        public ExpectedFillType() : base(10442) { }
        public ExpectedFillType(int data) : base(10442, data) { }

        public const int FIELD = 10442;
    }

    public class SecurityAltId : QuickFix.StringField
    {
        public SecurityAltId() : base(10455) { }
        public SecurityAltId(String data) : base(10455, data) { }

        public const int FIELD = 10455;
    }

    public class UnderlyingSecurityAltID : QuickFix.StringField
    {
        public UnderlyingSecurityAltID() : base(10456) { }
        public UnderlyingSecurityAltID(String data) : base(10456, data) { }

        public const int FIELD = 10456;
        
    }

    public class LotsPerContract : QuickFix.IntField
    {
        public LotsPerContract() : base(10461) { }
        public LotsPerContract(int data) : base(10461, data) { }

        public const int FIELD = 10461;
    }

    public class SecondaryExecID : QuickFix.StringField 
    {
        public SecondaryExecID() : base(10527) { }
        public SecondaryExecID(string data) : base(10527, data) { }

        public const int FIELD = 10527;
    }

    public class FixingDate : QuickFix.StringField
    {
        public FixingDate() : base(10541) { }
        public FixingDate(string data) : base(10541, data) { }

        public const int FIELD = 10541;
    }

    public class TTUsername : QuickFix.StringField
    {
        public TTUsername() : base(10553) { }
        public TTUsername(string data) : base(10553, data) { }

        public const int FIELD = 10553;
    }

    public class LegPrice : QuickFix.DoubleField 
    {
        public LegPrice() : base(10566) { }
        public LegPrice(double data) : base(10566, data) { }

        public const int FIELD = 10566;
    }

    public class SecuritySubType : QuickFix.StringField
    {
        public SecuritySubType() : base(10762) { }
        public SecuritySubType(String data) : base(10762, data) { }

        public const int FIELD = 10762;
    }

    public class FFT2 : QuickFix.StringField
    {
        public FFT2() : base(16102) { }
        public FFT2(String data) : base(16102, data) { }

        public const int FIELD = 16102;
    }

    public class FFT3 : QuickFix.StringField
    {
        public FFT3() : base(16103) { }
        public FFT3(String data) : base(16103, data) { }

        public const int FIELD = 16103;
    }

    public class RealizedPandL : QuickFix.DoubleField
    {
        public RealizedPandL() : base(16210) { }
        public RealizedPandL(double data) : base(16210, data) { }

        public const int FIELD = 16210;
    }

    public class PriceDisplayType : QuickFix.IntField
    {
        public PriceDisplayType() : base(16451) { }
        public PriceDisplayType(int data) : base(16451, data) { }

        public const int FIELD = 16451;

    }

    //16452 TickSize - For internal TT use only.
    //16454 PointValue - For internal TT use only.

    public class NumTickTblEntries : QuickFix.IntField
    {
        public NumTickTblEntries() : base(16456) { }
        public NumTickTblEntries(int data) : base(16456, data) { }

        public const int FIELD = 16456;
    }

    public class NumTicks : QuickFix.IntField
    {
        public NumTicks() : base(16457) { }
        public NumTicks(int data) : base(16457, data) { }

        public const int FIELD = 16457;
    }

    public class MaxPrice : QuickFix.DoubleField
    {
        public MaxPrice() : base(16458) { }
        public MaxPrice(double data) : base(16458, data) { }

        public const int FIELD = 16458;
    }

    public class DeliveryUnit : QuickFix.IntField
    {
        public DeliveryUnit() : base(16460) { }
        public DeliveryUnit(int data) : base(16460, data) { }

        public const int FIELD = 16460;
    }

    public class BaseLots : QuickFix.IntField
    {
        public BaseLots() : base(16461) { }
        public BaseLots(int data) : base(16461, data) { }

        public const int FIELD = 16461;
    }

    public class UnderlyingLotsPerContract : QuickFix.IntField
    {
        public UnderlyingLotsPerContract() : base(16462) { }
        public UnderlyingLotsPerContract(int data) : base(16462, data) { }

        public const int FIELD = 16462;
    }

    public class Blocks : QuickFix.IntField
    {
        public Blocks() : base(16463) { }
        public Blocks(int data) : base(16463, data) { }

        public const int FIELD = 16463;
    }

    public class TradesInFlow : QuickFix.CharField
    {
        public TradesInFlow() : base(16464) { }
        public TradesInFlow(char data) : base(16464, data) { }

        public const int FIELD = 16464;
    }

    public class PassiveAgressive : QuickFix.CharField
    {
        public PassiveAgressive() : base(16480) { }
        public PassiveAgressive(char data) : base(16480, data) { }

        public const int FIELD = 16480;

        public const char PASSIVE = 'P';
        public const char AGGRESSIVE = 'A';
    }

    public class AutoAgressive : QuickFix.CharField
    {
        public AutoAgressive() : base(16481) { }
        public AutoAgressive(char data) : base(16481, data) { }

        public const int FIELD = 16481;

        public const char AUTO_AGRESS = 'Y';
        public const char DO_NOT_AUTO_AGRESS = 'N';
    }

    public class LeaveOrders : QuickFix.CharField
    {
        public LeaveOrders() : base(16482) { }
        public LeaveOrders(char data) : base(16482, data) { }

        public const int FIELD = 16482;

        public const char LEAVE_CONTRACT_ORDERS_IN_THE_MARKET='Y';
        public const char DELETE_ALL_ORDERS_FOR_THE_CONTRACT='N';
    }

    public class MDEntryState : QuickFix.IntField
    {
        public MDEntryState() : base(16486) { }
        public MDEntryState(int data) : base(16486, data) { }

        public const int FIELD = 16486;
        public const int OPEN_WORKUP = 1;
        public const int PUBLIC_WORKUP_HIT_THE_BID = 2;
        public const int PUBLIC_WORKUP_TOOK_THE_ASK = 3;
        public const int PRIVATE_WORKUP_HIT_THE_BID =4;
        public const int PRIVATE_WORKUP_TOOK_THE_ASK =5;
    }

    public class MDEntrySizeType : QuickFix.StringField
    {
        public MDEntrySizeType() : base(16487) { }
        public MDEntrySizeType(string data) : base(16487,data) { }

        public const int FIELD = 16487;
        public const string MINIMUM_AVAILABLE = "1";
        public const string NET_AGGREGATE = "2";

    }

    public class AggressorSide : QuickFix.IntField 
    {
        public AggressorSide() : base(16488) { }
        public AggressorSide(int data) : base(16488, data) { }

        public const int FIELD = 16488;
        public const int BUYER = 1;
        public const int SELLER = 2;
    }

    public class ExchTickSize : QuickFix.DoubleField
    {
        public ExchTickSize() : base(16552) { }
        public ExchTickSize(double data) : base(16552, data) { }

        public const int FIELD = 16552;
    }

    public class ExchPointValue : QuickFix.DoubleField
    {
        public ExchPointValue() : base(16554) { }
        public ExchPointValue(double data) : base(16554, data) { }

        public const int FIELD = 16554;
    }

    public class LegSide : QuickFix.IntField
    {
        public LegSide() : base(16624) { }
        public LegSide(int data) : base(16624, data) { }

        public const int FIELD = 16624;
    }

    public class PosReqId : QuickFix.StringField
    {
        public PosReqId() : base(16710) { }
        public PosReqId(String data) : base(16710, data) { }

        public const int FIELD = 16710;


    }

    public class PosMaintRptId : QuickFix.StringField
    {
        public PosMaintRptId() : base(16721) { }
        public PosMaintRptId(String data) : base(16721, data) { }

        public const int FIELD = 16721;
    }
    
    public class PosReqType : QuickFix.IntField
    {
        public PosReqType() : base(16724) { }
        public PosReqType(int data) : base(16724, data) { }

        public const int FIELD = 16724;

        public const int POSITIONS = 0;
        public const int TRADES =1;
        public const int SOD =4;
        public const int MANUAL_FILL = 5;
        public const int DSOD = 6;
    }

    public class  TotalNumPosReports : QuickFix.IntField
    {
        public TotalNumPosReports() : base(16727) { }
        public TotalNumPosReports(int data) : base(16727, data) { }

        public const int FIELD = 16727;
    }

    public class TotalNumOrders : QuickFix.IntField
    {
        public TotalNumOrders() : base(16728) { }
        public TotalNumOrders(int data) : base(16728, data) { }

        public const int FIELD = 16728;
    }

    public class MarginExcess : QuickFix.DoubleField
    {
        public MarginExcess() : base(16899) { }
        public MarginExcess(double data) : base(16899, data) { }

        public const int FIELD = 16899;
    }

    public class RequestTickTable : QuickFix.StringField
    {
        public RequestTickTable() : base(17000) { }
        public RequestTickTable(String data) : base(17000, data) { }

        public const int FIELD = 17000;

        public const string SEND_TICK_TABLE = "Y";
        public const string DO_NOT_SEND = "N";

    }

    public class ForceLogout : QuickFix.CharField
    {
        public ForceLogout() : base(18000) { }
        public ForceLogout(char data) : base(18000, data) { }

        public const int FIELD = 18000;
    }

    public class ApplicationName : QuickFix.StringField
    {
        public ApplicationName() : base(18011) { }
        public ApplicationName(String data) : base(18011, data) { }

        public const int FIELD = 18011;
    }

    //Version 7.5 removed support for Tag 18103 (OrderAndFillSource).
    //public class OrderAndFillSource : QuickFix.StringField
    //{
    //    public OrderAndFillSource() : base(18103) { }
    //    public OrderAndFillSource(String data) : base(18103, data) { }
    //}

    public class GatewayStatusReqId : QuickFix.StringField
    {
        public GatewayStatusReqId() : base(18200) { }
        public GatewayStatusReqId(String data) : base(18200, data) { }

        public const int FIELD = 18200;
    }

    public class NoGatewayStatus : QuickFix.IntField
    {
        public NoGatewayStatus() : base(18201) { }
        public NoGatewayStatus(int data) : base(18201, data) { }

        public const int FIELD = 18201;
    }

    public class GatewayStatus : QuickFix.IntField
    {
        public GatewayStatus() : base(18202) { }
        public GatewayStatus(int data) : base(18202, data) { }

        public const int FIELD = 18202;

        public const int HALTED =1;
        public const int OPEN=2;
        public const int CLOSED=3;
        public const int PRE_OPEN=4;
        public const int PRE_CLOSE = 5;

        public static string getString(int i)
        {
            switch (i)
            {
                case 1:
                    return "HALTED";
                case 2:
                    return "OPEN";
                case 3:
                    return "CLOSED";
                case 4:
                    return "PRE_OPEN";
                case 5:
                    return "PRE_CLOSE";
                default:
                    return "UNKNOWN";
            }
        }
    }

    public class ExchangeGateway : QuickFix.StringField
    {
        public ExchangeGateway() : base(18203) { }
        public ExchangeGateway(String data) : base(18203, data) { }

        public const int FIELD = 18203;
    }
    
    public class SubExchangeGateway : QuickFix.IntField
    {
        public SubExchangeGateway() : base(18204) { }
        public SubExchangeGateway(int data) : base(18204, data) { }

        public const int FIELD = 18204;

        public const int PRICE=1;
        public const int ORDER=2;
        public const int FILL=3;

        public static string getString(int i)
        {
            switch (i)
            {
                case 1:
                    return "PRICE";
                case 2:
                    return "ORDER";
                case 3:
                    return "FILL";
                default:
                    return "UNKNOWN";
            }
        }
        
        
    }

    public class TTAccountType : QuickFix.StringField
    {
        public TTAccountType() : base(18205) { }
        public TTAccountType(String data) : base(18205, data) { }

        public const int FIELD = 18205;
        
        //First Agent Account (A1): Set 47=A and 204=0, or set 18205=A1
        public const string A1 = "A1";

        //Euronext Products: Customer/Automatic Allocation (A2): Set 47=A and 204=1, or set 18205=A2
        public const string A2 = "A2";
        
        //Euronext Products: Customer/Give-up or System Allocation (A3): Set 47=A and 204=2, or set 18205=A3
        public const string A3 = "A3";
        
        //Pre-Designated Give-up Trade (G1): Set 47=W and 204=0,or set 18205=G1
        public const string G1 = "G1";

        //Designated Give-up Trade/Automatic Allocation (G2): Set 47=W and 204=11, or set 18205=G2
        public const string G2 = "G2";

        //Give-up System Allocation (G3): Set 47=W and 204=2, or set 18205=G3
        public const string G3 = "G3";
        
        //First Market Maker Account (M1): Set 47=E and 204=0, or set 18205=M1
        public const string M1 = "M1";
        
        //Second Market Maker Account (m2): Set 47=E and 204=1,or set 18205=M2
        public const string M2 = "M2";
        
        //Market Maker/Give-up or System Allocation (M3): Set 447=E and 204=2, or set 18205=M3
        public const string M3 = "M3";
        
        //First Principal Account (P1): Set 47=P and 204=0, or set18205=P1
        public const string P1 = "P1";
        
        //Second Principal Account (P2): Set 47=P and 204=1, or set18205=P2
        public const string P2 = "P2";
        
        //Euronext Products: House/Give-up or System Allocation (P3):Set 47=P and 204=2, or set 18205=P3
        public const string P3 = "P3";
        
        //Unallocated (U1): Set 47=0 and 204=0, or set 18205=U1
          //(For orders that have not been allocated to a customer account or where allocation is a middle/back office function)
        public const string U1 = "U1";
        
        //Unallocated/Automatic (U1): Set 47=0 and 204=1, or set 18205=U2
        public const string U2 = "U2";
        
        //Unallocated/System (U3): Set 47=0 and 204=2, or set 18205=U3
        public const string U3 = "U3";
      
    }

    public class NoGateways : QuickFix.IntField
    {
        public NoGateways() : base(18206) { }
        public NoGateways(int data) : base(18206, data) { }

        public const int FIELD = 18206;
    }

    public class SpecialStrategyFillFlag : QuickFix.StringField
    {
        public SpecialStrategyFillFlag() : base(18209) { }
        public SpecialStrategyFillFlag(String data) : base(18209, data) { }

        public const int FIELD = 18209;
    }

    public class PriceFeedStatus : QuickFix.IntField
    {
        public PriceFeedStatus() : base(18210) { }
        public PriceFeedStatus(int data) : base(18210, data) { }

        public const int FIELD = 18210;

        public const int PRICE_FEED_IS_UNAVAILABLE = 0;
        public const int PRICE_FEED_IS_AVAILABLE = 1;
    }

    public class DeliveryTerm : QuickFix.CharField
    {
        public DeliveryTerm() : base(18211) { }
        public DeliveryTerm(char data) : base(18211, data) { }

        public const int FIELD = 18211;

        public const char DAILY_TERM = 'D';
        public const char WEEKLY_TERM = 'W';
        public const char QUARTERLY_TERM ='Q';
        public const char SEASONAL_TERM = 'S';
        public const char YEARLY_CALENDAR_TERM ='C';
        public const char VARIABLE_TERM ='V';
        public const char UNDEFINED_TERM ='U';
    }

    public class UnderlyingDeliveryTerm : QuickFix.CharField
    {
        public UnderlyingDeliveryTerm() : base(18212) { }
        public UnderlyingDeliveryTerm(char data) : base(18212, data) { }

        public const int FIELD = 18212;

        public const char DAILY_TERM = 'D';
        public const char WEEKLY_TERM = 'W';
        public const char QUARTERLY_TERM ='Q';
        public const char SEASONAL_TERM = 'S';
        public const char YEARLY_CALENDAR_TERM ='C';
        public const char VARIABLE_TERM ='V';
        public const char UNDEFINED_TERM ='U';
    }
    


}
