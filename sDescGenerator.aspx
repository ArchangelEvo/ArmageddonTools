<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="sDescGenerator.aspx.vb" Inherits="sDescGenerator" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h2>
    sDesc Generator</h2>
<br />
Gender:
<asp:DropDownList ID="cmbGender" runat="server" AutoPostBack="True">
</asp:DropDownList>
&nbsp;&nbsp;&nbsp; Race:
<asp:DropDownList ID="cmbRace" runat="server" AutoPostBack="True">
</asp:DropDownList>
<br />
<br />
<br />
Height:
<asp:DropDownList ID="cmbHeight" runat="server" AutoPostBack="True">
</asp:DropDownList>
&nbsp;&nbsp;&nbsp; Build:
<asp:DropDownList ID="cmbBuild" runat="server" AutoPostBack="True">
</asp:DropDownList>
&nbsp;&nbsp;&nbsp; Weight:
<asp:DropDownList ID="cmbWeight" runat="server" AutoPostBack="True">
</asp:DropDownList>
<br />
<br />
<br />
Hair Color:
<asp:DropDownList ID="cmbHairColor" runat="server" AutoPostBack="True">
</asp:DropDownList>
&nbsp;&nbsp;&nbsp; Hairstyle:
<asp:DropDownList ID="cmbHairstyle" runat="server" AutoPostBack="True">
</asp:DropDownList>
<br />
<br />
<br />
Eye Color:
<asp:DropDownList ID="cmbEyeColor" runat="server" AutoPostBack="True">
</asp:DropDownList>
<br />
<br />
<br />
Skin Color:
<asp:DropDownList ID="cmbSkinColor" runat="server" AutoPostBack="True">
</asp:DropDownList>
<br />
<br />
<br />
Overall Appearance:
<asp:DropDownList ID="cmbOverall" runat="server" AutoPostBack="True">
</asp:DropDownList>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
<asp:CheckBox ID="cbxIdeal" runat="server" Text="Heroic Ideal" />
<br />
<br />
<br />
<asp:CheckBox ID="cbxTermAgree" runat="server" Text="Force Term Agreement" 
        Checked="True" />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
<asp:Button ID="btnGenerate" runat="server" Height="38px" Text="Generate" 
    Width="178px" />
&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;
<asp:DropDownList ID="nudResults" runat="server">
    <asp:ListItem>1</asp:ListItem>
    <asp:ListItem>2</asp:ListItem>
    <asp:ListItem>3</asp:ListItem>
    <asp:ListItem>4</asp:ListItem>
    <asp:ListItem>5</asp:ListItem>
    <asp:ListItem>6</asp:ListItem>
    <asp:ListItem>7</asp:ListItem>
    <asp:ListItem>8</asp:ListItem>
    <asp:ListItem>9</asp:ListItem>
    <asp:ListItem>10</asp:ListItem>
</asp:DropDownList>
&nbsp;Number of Results<br />
<br />
<asp:TextBox ID="txtDesc" runat="server" Font-Size="Large" Height="19px" 
    Rows="1" TextMode="MultiLine" Width="617px"></asp:TextBox>
    &nbsp;<br />
    <br />
</asp:Content>

