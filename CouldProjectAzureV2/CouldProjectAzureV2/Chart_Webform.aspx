﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Chart_Webform.aspx.cs" Inherits="CouldProjectAzureV2.Chart_Webform" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div>
    <asp:DropDownList ID="UserList" runat="server" AutoPostBack="True" OnSelectedIndexChanged="UserList_SelectedIndexChanged">
        </asp:DropDownList>

         <asp:DropDownList ID="MeasurementList" runat="server" AutoPostBack="True" OnSelectedIndexChanged="MeasurementList_SelectedIndexChanged">
        </asp:DropDownList>
        
        <asp:Calendar ID="CalendarOne" runat="server" OnSelectionChanged="CalendarOne_SelectionChanged"></asp:Calendar>
    
        <asp:Chart ID="AcclerometerChart" runat="server" Height="347px" Width="625px">
           <Series>

    </Series>
            <chartareas>
                <asp:ChartArea Name="ChartArea1">                  
    <AxisY Title="Acccelerometer values">
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
    <AxisY Title="Light sensor Values">
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
    <AxisY Title="Proximity Values">
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
    <AxisY Title="Battery values">
    </AxisY>
     <AxisX Title="Time">
                <LabelStyle Angle="-90" Interval="1" />
      </AxisX>
                </asp:ChartArea>
            </chartareas>
        </asp:Chart>
    
    </div>
     <div>
         <asp:Table ID="Table1" runat="server"></asp:Table>
     </div>     

</asp:Content>


