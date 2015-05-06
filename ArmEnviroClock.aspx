<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="ArmEnviroClock.aspx.vb" Inherits="ArmEnviroClock" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<img src="http://www.techmeister.net/steve/Armageddon/LogoAnimated.gif" alt="Sample Animated Clock" />
<h2>Armageddon Enviro Clock</h2>
<div style="text-align: left">
    The Armageddon Enviro Clock is a widget intended to sit on your desktop, that gives you a graphical indicator of the current time on Zalanthas.  It has different modes that can be selected, depending on how large you want the clock to be.  The "large image" gives you a fully rendered desert setting that changes with the hour, whereas the "hanging" modes give you a much smaller clock that is intended to float above your open client window.<br><br>
Enviro Clock Instructions:<br />
</div>
<img src="http://www.techmeister.net/steve/Armageddon/envirosample1.jpg" alt="Hanging Logo Sample" style="text-align: center" />  
<ol style="text-align: left">
<li>Download the clock found <a href="http://www.techmeister.net/steve/Armageddon/ArmEnviroTrackerInstall.zip">-here-</a> as an install file (preferred) or <a href="http://www.techmeister.net/steve/Armageddon/ArmEnviroTracker.zip">-here-</a> as an app. (The install file is preferable, as it makes proper use of task bar icons, etc.)</li><br />
<li>Unzip and run the program.</li><br />
<li>Right-click on the form and select "Options" to open the "Options" window below.</li><br />
<img src="http://www.techmeister.net/steve/Armageddon/enviromainform.png" alt="Main Form" /><br /><br />
<li>Set the current hour in Zalanthas (using the website or in-game clock).</li><br />
<li>Also set the number of minutes (not hours) the server is off from your computer, if necessary.</li><br />
<li>The "Clock Style" dropdown box lets you select whether you want a large image clock (last image on this page), a spinning moon phase clock, or the moon phase clock with the Arm logo (first image on this page).</li><br>
<li>The "Always On Top" checkbox allows you to set the clock so that you can use programs in the background without hiding the clock.</li><br>
<li>The "Overlay Label" checkbox allows you to set whether the clock will have the current time overlaid in text, and the dropdown allows you to choose the color of the label.</li><br>
<li>Click "Save" and enjoy!</li><br>
</ol>
<div style="text-align: left">
Note: The clock will remember the difference between the Armageddon server and your computer, so you should only need to reset the hour if something bad happens.  It calculates based on UTC time (~GMT), so you may have to adjust for Daylight Savings.<br><br>
Note 2: The clock will also remember the window size and location if you resize the large image (you can make it any size you want, or maximize it) or move the hanging clocks around.  To reset the image size and location, just select a new clock style in the "Options" and click "Save".  It will reset the window size.<br><br>
</div>
<img src="http://www.techmeister.net/steve/Armageddon/envirosample2.jpg" alt="Large Background Sample" /><br /><br />
<div style="text-align: left">
Note 3: When the clock is in "hanging" modes, the normal window frame and controls will not be visible.  Simply right-click on the clock at these times to change the settings or close the program.  You can also left-click on the frameless clock to drag it around your viewing window.<br><br>
Note 4: Some telnet (MUD) clients also give you the option to set transparency values in the main window.  If you do not set the clock to "always on top" but maximize it in the background, you could turn your MUD window transparent, and always see the desert time image in the background while your text scrolls over it.  (There are instructions to do this for MUSHClient in the 'MUSH Transparency' page on the left)  The "dark" images might be best for this setting, as they will provide better contrast for your text.<br><br>
</div>
<img src="http://www.techmeister.net/steve/Armageddon/envirosample3.jpg" alt="Large Transparent Dark Sample" /><br /><br />
<img src="http://www.techmeister.net/steve/Armageddon/LargeAnimated.gif" alt="Large Background Animation Sample" />
<br /><br /><br /><br />
<h3>Update: Voting Reminders</h3>
<div style="text-align: left">
    Since this clock is already doing a lot of checking against time change, I figured it wouldn't be too much harder to add some little voting reminders to it for people who wanted to use them.  If you check the "Voting Reminders ON" box, the program will pop up a button to remind you to vote, based on the revote period for each of the sites (TMC and TMS).  You can select which you want to be reminded to vote for, and also whether the program will simply show a button to allow you to vote, or actually pop up the voting web page automatically, to remind you.
<br /><br />
Note: The program can't tell when you actually click the link in the web page to vote, only when the page itself is opened, so if you choose not to use "Auto Open Page", and tend to vote right after clicking the button to open the web page, the timing of the reminders will probably be more accurate for you.<br /><br />
</div>
<img src="http://www.techmeister.net/steve/Armageddon/votingreminders.png" alt="Voting Reminders Sample" />
</asp:Content>

