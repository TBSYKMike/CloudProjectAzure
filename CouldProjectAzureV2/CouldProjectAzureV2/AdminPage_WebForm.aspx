<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdminPage_WebForm.aspx.cs" Inherits="CouldProjectAzureV2.AdminPage_WebForm" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
        <div class="col-xs-12">
                  <div class="round_corners">  
                <asp:GridView AutoGenerateColumns="false" ID="UserGridView" HeaderStyle-BackColor="#99ccff"
        HeaderStyle-ForeColor="White" RowStyle-BackColor="#e1e1d0" AlternatingRowStyle-BackColor="#f5f5ef"
        RowStyle-ForeColor="#3A3A3A" runat="server" OnRowDataBound="OnRowDataBound" OnSelectedIndexChanged="OnSelectedIndexChanged" class="usersgridviewclass">
                    <Columns>

                        <asp:TemplateField HeaderText="UserName">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" CssClass="noeffect" ID="openProductButton" Text='<%# Eval("UserName") %>' Style="font-size: 12px;"></asp:LinkButton>

                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Email">
                            <ItemTemplate>
                                <asp:Label ID="txtPrice" runat="server">
                                    <%# Eval("Email") %> 
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Accelerometer on/off">
                            <ItemTemplate>
                                <asp:Button ID="acceleroMeterOnButton" runat="server" CssClass="btn btn-success" Text="On"  OnClick="acceleomneterCellClick" />
                                <asp:Button ID="acceleroMeterOffButton" runat="server" CssClass="btn btn-default" Text="Off"  OnClick="acceleomneterCellClick" />

                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Proximity on/off">
                            <ItemTemplate>
                                <asp:Button ID="proximityOnButton" runat="server" CssClass="btn btn-success" Text="On" OnClick="proximityCellClick"/>
                                <asp:Button ID="proximityOffButton" runat="server" CssClass="btn btn-default" Text="Off" OnClick="proximityCellClick"/>
                            </ItemTemplate>
                        </asp:TemplateField>



                        <asp:TemplateField HeaderText="Light on/off">
                            <ItemTemplate>
                                    <asp:Button ID="lightOnButton" runat="server" CssClass="btn btn-success" Text="On" OnClick="lightCellClick"/>
                                <asp:Button ID="lightOffButton" runat="server" CssClass="btn btn-default" Text="Off" OnClick="lightCellClick"/>
                            </ItemTemplate>
                        </asp:TemplateField>

                           <asp:TemplateField HeaderText="Sampling frequency">
                            <ItemTemplate>
                                <asp:Button ID="samplingRateSlowButton" runat="server" CssClass="btn btn-default" Text="Slow" OnClick="changeSamplingFrequencyClick"/>
                                <asp:Button ID="samplingRateMediumButton" runat="server" CssClass="btn btn-default" Text="Medium" OnClick="changeSamplingFrequencyClick"/>
                                <asp:Button ID="samplingRateFastButton" runat="server" CssClass="btn btn-default" Text="Fast" OnClick="changeSamplingFrequencyClick"/>
                            </ItemTemplate>
                        </asp:TemplateField>



                    </Columns>
                </asp:GridView>
                           </div>
             </div>
</asp:Content>
