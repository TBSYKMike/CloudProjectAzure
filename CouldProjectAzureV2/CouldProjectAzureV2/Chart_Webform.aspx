<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Chart_Webform.aspx.cs" Inherits="CouldProjectAzureV2.Chart_Webform" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div>
    <asp:DropDownList ID="UserList" runat="server">
        </asp:DropDownList>
        
        <asp:Calendar ID="CalendarOne" runat="server" OnSelectionChanged="CalendarOne_SelectionChanged"></asp:Calendar>
    
        <asp:Chart ID="AcclerometerChart" runat="server" Height="347px" Width="625px">
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


           <asp:Chart ID="LightChart" runat="server" Height="347px" Width="625px">
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


           <asp:Chart ID="ProximityChart" runat="server" Height="347px" Width="625px">
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




         




         <asp:Chart ID="BatteryChart" runat="server" Height="347px" Width="625px">
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
         
</asp:Content>


