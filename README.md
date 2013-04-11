TT-FIX-AutoFlatten
==================

This sample application provides the following functionality:
        
**Available Functionality:**
  + Connect to TT Fix Adapter: Last tested against TT FA 7.65
  + Capture all trades for all traders based on config file
  + Monitor P&L
  + At preconfigured Maximum Loss, flatten out all positions.  If used in conjunction with credit limits this effectively locks trader out of trading for the day.

**Master Branch TODO:**  
  + Test against latest production TT FIX Adapter
  + Refactor code to .NET standards based on QuickFix/n  
  + Add QuickFix sample config file

2010 Branch contains code written to use QuickFix C++ DLL with .NET Wrapper

**USE AT YOUR OWN RISK**

This sample code is for demonstration purposes only and is not endorsed by Trading Technologies International, Inc.
