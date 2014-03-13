<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DashboardPA3.aspx.cs" Inherits="WebRole1.DashboardPA3" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">

    <head id="Head1" runat="server">
        <style type="text/css">
            body {  font: 11pt Trebuchet MS;
                    padding-top: 72px;
                    text-align: center }

            .text { font: 8pt Trebuchet MS }
        </style>

        <title>Simple Web Service</title>

            <script type="text/javascript">

                // This function calls the Web Service method.  
                function EchoUserInput() {
                    var echoElem = document.getElementById("EnteredValue");
                    Samples.AspNet.admin.StartCrawling(echoElem.value,
                        SucceededCallback);
                }

                // This is the callback function that 
                // processes the Web Service return value.
                function SucceededCallback(result) {
                    var RsltElem = document.getElementById("Results");
                    RsltElem.innerHTML = result;
                }

                function ClearIndexes() {
                    var clear = document.getElementById("Clear");
                    Samples.AspNet.admin.ClearIndex();
                }

                function GetTitles() {
                    var title = document.getElementById("Title");
                    Samples.AspNet.admin.GetPageTitle();
                }

        </script>

    </head>

    <body>
        <form id="Form2" runat="server">
         <asp:ScriptManager runat="server" ID="scriptManager">
                <Services>
                    <asp:ServiceReference path="SimpleWebService.asmx" />
                </Services>
            </asp:ScriptManager>
            <div>
                <h2>INFO 344 Web Crawler</h2>
                    <p>Crawl your entered root domain.</p>
                    <input id="EnteredValue" type="text" />
                    <input id="EchoButton" type="button"  
                        value="Start" onclick="EchoUserInput()" />
                    <br />
                    <input id="ClearIndexButton" type="button"
                        value="Clear" onclick="ClearIndexes()" />
                    <input id="GetPageTitle" type="button"
                        value="Title" onclick="GetPageTitle()" />
            </div>
        </form>

        <hr/>

        <div>
            <span id="Results"></span>
        </div>   

    </body>

</html>