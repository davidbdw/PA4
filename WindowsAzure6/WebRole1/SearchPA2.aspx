<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SearchPA2.aspx.cs" Inherits="WebRole1.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <link rel="stylesheet" type="text/css" href="SearchPA2.css" />
    <script src="http://code.jquery.com/jquery-1.11.0.min.js"></script>
    <script src="JavaScriptPA2.js"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.6.2/jquery.min.js"></script>
    <script src="//code.jquery.com/jquery-1.9.1.js"></script>
    <script src="//code.jquery.com/ui/1.10.4/jquery-ui.js"></script>
    <title>NBA Player Searcher</title>
    <script>
        function getSearchQueries() {
            var name = $("#playerName").val();
            if (name.length == 0) {
                $("#suggestions").empty();
            }
            $.ajax({
                url: "WebServicePA2.asmx/searchInTrie",
                type: "POST",
                data: "{userInput:'" + name + "'}",
                contentType: "application/json; charset=utf-8",
                success: function (msg) {
                    $("#suggestions").empty();
                    var msgstring = JSON.stringify(msg.d);
                    $("#suggestions").html(msgstring);
                },
                error: function (msg) {
                    $("#suggestions").html(JSON.stringify(msg));
                }
            });
        }


        function getSearchQueries2() {
            var name = $("#tags").val();
            $.ajax({
                url: "WebServicePA2.asmx/searchInTrie",
                type: "POST",
                data: "{userInput:'" + name + "'}",
                contentType: "application/json; charset=utf-8",
                success: displaySearches2,
                error: function (msg) {
                    $("#suggestions").html(JSON.stringify(msg));
                }
            });
        }

        function displaySearches2(msg) {
            var msgstring = JSON.stringify(msg.d);
            $(function () {
                var availableTags = (msg.d);
                $("#tags").autocomplete({
                    source: availableTags
                });
            });
        }

        function getNBAStats() {
            var name = $("#playerName").val();
            if (name.length == 0) {
                $("#stats").html("No player name was found");
            };
            $.ajax({
                contentType: "application/json; charset=utf-8",
                //crossDomain: true,
                url: "http://ec2-54-201-21-73.us-west-2.compute.amazonaws.com/result.php",
                type: "POST",
                data: "{user:'" + name + "'}",
                dataType: "JSONP",
                success: displayNBAStats,
                error: function (msg) {
                    alert("statfail");
                    $("#stats").html(JSON.stringify(msg)); 
                },
            });
        }
        
        function displayNBAStats(data) {
            alert('jsonp success');
            $("#stats").html(JSON.stringify(data));
        }
        
    </script>

</head>
<body>
    <h1>NBA Player Searcher</h1>
    <h2>Search for your favorite player's stats and associated website URLs</h2>
    <form id="form1" runat="server">
        <div id="searchPlayer">

                Enter NBA Player <input onkeyup="getSearchQueries()" type="text" id="playerName" 
                    name="name" placeholder="Enter player name..." style="width: 200px;" />
                <input type="submit" value="Search" onclick="getNBAStats()"/>


        </div>

        <div class="ui-widget">
            <label for="tags">Enter NBA Player </label>
            <input id="tags" onkeyup="getSearchQueries2()"/>
            <input type="submit" value="Search" onclick="getNBAStats(); getURLS();"/>
        </div>

        <div id="suggestions">
        </div>
    
        <div id="stats">
        </div>

        <div id="urls">
        </div>

    </form>

<script type='text/javascript' src='http://ads1.qadabra.com/t?id=93646a21-fc25-4f92-829e-52aa98e31347&size=300x250'></script>
</body>
</html>
