<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="MUSHClientTransparency.aspx.vb" Inherits="MUSHClientTransparency" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<h2>Setting MUSHClient Transparency</h2>
To set MUSHClient to have a transparent background, follow these steps:<br /><br />
<ol style="text-align: left">
<li>Connect to Armageddon (some MUSHClient settings are not available when disconnected)</li><br>
<li>In the menus, go to: Game->Configure->Scripting</li><br>
<li>Be sure Enable Scripting is checked, and select “Lua” in the 'Scripting Language' drop down box</li><br>
<li>Click 'Ok'</li><br>
<li>In the menus, go to: Game->Configure->Aliases</li><br>
<li>Click 'Add…'</li><br>
<li>Type “gotranspo” (no quotes - another alias you will remember that won't conflict with Arm syntax will also work) in the 'Alias:' box</li><br>
<li>Type “Transparency (-1, 175)” (no quotes) in the 'Send:' box</li><br>
<li>Set 'Send To:' drop down box to 'Script'</li><br>
<li>Click 'Ok'</li><br>
<li>Click 'Add…'</li><br>
<li>Type “stoptranspo” (no quotes - another alias you will remember that won't conflict with Arm syntax will also work) in the 'Alias:' box</li><br>
<li>Type “Transparency (-1, 255)” (no quotes) in the 'Send:' box</li><br>
<li>Set 'Send To:' drop down box to 'Script'</li><br>
<li>Click 'Ok'</li><br>
<li>Click 'Save'</li><br>
</ol>
<div style="text-align: left">
Now that you have the aliases set up, to enter transparecy mode, just type "gotranspo" (no quotes) into your main input window and hit enter.  To exit, do the same with "stoptranspo".  Enjoy!<br /><br />
Note: If you have trouble with the visibility of the background, you can fiddle around with the percentage of transparency by adjusting the value after the comma in step #8.<br /><br />
Note 2: Earlier versions of MUSHClient do not support transparency.  If you are having problems, check to see that your client is updated.
</div>
</asp:Content>

