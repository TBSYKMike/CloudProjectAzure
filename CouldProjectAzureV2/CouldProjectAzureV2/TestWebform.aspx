<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestWebform.aspx.cs" Inherits="CouldProjectAzureV2.TestWebform" %>

<%@ Register assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI.DataVisualization.Charting" tagprefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body style="height: 787px; width: 1027px">
    <form id="form1" runat="server">
    <div>
    
        <asp:Chart ID="DataChart" runat="server" Height="347px" Width="625px">
           <Series>

    </Series>
            <chartareas>
                <asp:ChartArea Name="ChartArea1">                  
    <AxisY Title="Sensor Values">
    </AxisY>
     <AxisX Title="Time">
                <LabelStyle Angle="-90" Interval="1" />
      </AxisX>
                </asp:ChartArea>
            </chartareas>
        </asp:Chart>
    
    </div>
    </form>
</body>
</html>
