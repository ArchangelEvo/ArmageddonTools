<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="LogItemIndexer.aspx.vb" Inherits="LogItemIndexer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<h2>Log Items Indexer</h2>
<div style="text-align: left">
    The Log Item Indexer is a program that will parse your Armageddon log files, extract item short descriptions, prices, colors, wear locations, etc, and place them all into a file.  You can then use this file to plan item sets for characters, see what other items might match an item you already have, etc.<br><br>
Log Item Indexer Instructions:<br>
</div>
<img src="http://www.techmeister.net/steve/Armageddon/MainForm.png" alt="Main Form" />
<ol style="text-align: left">
<li>Download the indexer found <a href="http://www.techmeister.net/steve/Armageddon/LogItemIndexer.zip">-here-</a>.</li><br>
<li>Unzip and run the program.</li><br>
<li>Click the “Select Directory” button, and choose the folder where the files you want indexed are stored.</li><br>
<li>Change the name of the output file in the “New File Name” box as desired.</li><br>
<li>In the “Search Options” box, select whether you want to index ALL folders in the selected directory, or just the top folder, by selecting “Index Subfolders”.</li><br>
<li>In the “Search Options” box, select whether you want to categorize all goods in the output file, or just the definite items, by selecting “Label Misc Goods”. <br>Note: Choosing this option will attempt to label every item in your logs, including crafting materials, herbs, etc.  These items are easily miscategorized, and choosing to label them may render your output slightly confusing.</li><br>
<li>Click the “Index Items” button to begin!</li><br>
</ol>
<div style="text-align: left">
Note: The only file type currently available in the “File Types” box is .txt files.  While the program has the capability to read MS Word logs (.doc/.docx), enabling this feature involves installing the MS Office Interop Assemblies to your Global Assembly Cache (GAC).  If that sounds like gibberish to you, you probably want to stick with the txt only version.<br><br>
Note 2: There is a file called ItemIndex.xls included in the program files that is an MS Excel version of the program output, with the sorting buttons built in already and the headers frozen in place.  If you copy the program output to this file, you will easily be able to sort the results based on type, general color, etc.  The other version, called ItemIndexColorized.xls, <!--found <a href="http://www.techmeister.net/steve/Armageddon/ItemIndexColorized.xls">-here-</a>, which--> will auto-colorize certain cells based on general color type to give you a quick reference as you look through the doc.<br><br />
</div><br />
<img src="http://www.techmeister.net/steve/Armageddon/ColorizedIndex.png" alt="Colorized Index" />
<!--<center><font size=-2>Photo by Zoomion.</font></center>-->
</asp:Content>

