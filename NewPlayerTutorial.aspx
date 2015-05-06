<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="NewPlayerTutorial.aspx.vb" Inherits="NewPlayerTutorial" %>

<%@ Register assembly="AjaxControlToolkit" 
namespace="AjaxControlToolkit" 
tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <asp:Literal ID="Literal1" runat="server" />
    <div style="margin-right: auto; margin-left: auto; text-align:center; height: 134px; width: 751px;" 
        align="center">
    <div style="margin-right: auto; margin-left: auto; float: left; width: 553px" align="center">
    <asp:Panel ID="Panel1" runat="server" DefaultButton="btnEnterKey" 
            HorizontalAlign="Center" style="margin-right: auto; margin-left: auto">
    <asp:TextBox ID="txtInputBox" runat="server" AutoCompleteType="Disabled" Height="77px" Width="544px"></asp:TextBox><br />
    <asp:Button ID="btnEnterKey" runat="server" style="display:none" />
    <asp:TextBox ID="txtInstructionsBox" runat="server" Height="49px" 
            ReadOnly="True" textmode="MultiLine" Width="542px"></asp:TextBox>
    </asp:Panel>
    </div>
        <div style="margin-right: auto; margin-left: auto; float: left; width: 197px; height: 134px;" align="center">
           <asp:Panel ID="pnlLinks" runat="server" Height="134px" HorizontalAlign="Left" 
               ScrollBars="Auto" BorderStyle="Solid" BorderWidth="1px" 
                BorderColor="#C8C8C8">
           </asp:Panel>
        </div>
    </div>
    <br />
    <br />
    <asp:TextBox ID="txtReturnMsgBox" runat="server" AutoCompleteType="Disabled" 
            Height="77px" Width="737px" ReadOnly="True" textmode="MultiLine" Font-Bold="True" ForeColor="#0000ff"></asp:TextBox>
        <br />
    <br />
    <asp:Button ID="btnSkipToNext" runat="server" Text="Skip" />
    &nbsp;&nbsp;&nbsp;
    <asp:DropDownList ID="dbxSelectSkipTo" runat="server" Height="20px">
    </asp:DropDownList>
    <asp:Button ID="btnSkipTo" runat="server" Text="Jump To" />
</asp:Content>

