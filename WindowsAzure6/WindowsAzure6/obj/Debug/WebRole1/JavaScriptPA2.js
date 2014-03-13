//function getValues() {
//    var userSearch = $("#userSearch").val();
//    if (userSearch.length == 0) {
//        $("#result").empty();
//        $("#result").html = 'You didn\'t enter in a search value.';
//        return;
//    }

//    $.ajax({
//        type: "POST",
//        url: "WebService1.asmx/getResults",
//        data: "{userInput:" + userSearch + "}",
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        success: function (msg) {
//            $("#result").empty();
//            for (var i = 0; i < msg.d.length; i++) {
//                $("#result").append.(msg.d[i] + "<br/>");
//            }
//            $("#result").html(JSON.stringify(msg));
//        },
//        error: function (msg, status, error) {
//            $("#result").html(JSON.stringify(msg));
//        }
//    });
//};



function myCallback(dataAWS) {
    var stats = '';
    var dataLength = dataAWS.length;
    for (var i = 0; i < dataLength; i++) {
        var eachStat = dataAWS[i];
        stats += eachStat + ', ';
    }
    document.getElementById('stats').innerHTML = stats;

}

//$(document).ready(function () {
//    $("#suggestions").text("Ready");
//    //document.getElementById("suggestions").innerHTML = "ready";
//});
//function getSearchQueries() {
//    var name = $("#playerName").val();
//    if (name.length == 0) {
//        $("#suggestions").empty();
//        return;
//    }
//    $ajax({
//        url: "WebServicePA2.asmx/searchInTrie",
//        type: "POST",
//        data: "{userInput:'" + name + "'}",
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        success: function (msg) {
//            $("#suggestions").empty();
//            for (var i = 0; i < msg.d.length; i++) {
//                $("#suggestions").append(msg.d[i] + "<br/>");
//            }
//        },
//        error: function (msg) {
//            $("#suggestions").html(JSON.stringify(msg));
//        }
//    });
//}