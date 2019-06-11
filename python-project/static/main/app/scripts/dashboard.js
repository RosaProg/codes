//==============================
//
//  dashboard.js - widgets js
//  Controls all dashboard elements
//
//==============================
$( document ).ready(function() {





    // TODO: Timezones




    // Dashboard Notes
    $("#dashboard-note-input").keyup(function (e) {
        if (e.keyCode == 13) {
            $("#dashboard-notes-ul").prepend("<li>"+$("#dashboard-note-input").val()+"</li>");
            $(this).val('');
        }
    });



    // Line GRAPH
    var dash_stat_points = {
        labels: [" ", " ", " ", " ", " ", " ", " "],
        datasets: [
            {
                fillColor: "rgba(255,255,255,1)",
                strokeColor: "rgba(255,255,255,1)",
                data: [5,1,2,6,9,0,1]
            }
        ]
    };
    var dash_stat_graph = document.getElementById("dash-stat-graph").getContext("2d");
    new Chart(dash_stat_graph).Line(dash_stat_points, {
      scaleBeginAtZero : false,
      showScaleLabels: true,
      scaleShowGridLines: false,
      showTooltips: false,
      scaleGridLineColor : "rgba(0,0,0,0)",
      pointDot: false,
      bezierCurveTension :0.1,
      scaleLabel: " ",
      tooltipTemplate: " ",
      tooltipCornerRadius: 0,
      pointDotRadius : 4,
      pointDotStrokeWidth : 2,
      responsive:true
    });








    // Close welcome Message
    $("#close-welcome-message").click(function(){
        $("#welcome-message").slideUp();
    });


    // Exchange Rate
    $.get("https://api.fixer.io/latest?base=USD").done(function(exchangeData){
        var getExchangeData = exchangeData;

        $("#ex-AUD").html("<small>AUD</small><br/>"+getExchangeData.rates.AUD);
        // $("#ex-CAD").text("CAD "+getExchangeData.rates.CAD);
        // $("#ex-CHF").text("CHF "+getExchangeData.rates.CHF);
        $("#ex-CNY").html("<small>CNY</small><br/>"+getExchangeData.rates.CNY);
        $("#ex-EUR").html("<small>EUR</small><br/>"+getExchangeData.rates.EUR);
        $("#ex-GBP").html("<small>GBP</small><br/>"+getExchangeData.rates.GBP);
        // $("#ex-HUF").text("HUF "+getExchangeData.rates.HUF);
        $("#ex-JPY").html("<small>JPY</small><br/>"+getExchangeData.rates.JPY);
        // $("#ex-PLN").text("PLN "+getExchangeData.rates.PLN);
        $("#ex-RUB").html("<small>RUB</small><br/>"+getExchangeData.rates.RUB);
        // $("#ex-SEK").text("SEK "+getExchangeData.rates.SEK);

    });


    // Active Link
    $("#link_overview").addClass("active");



    // loadWeather();
    loadNews();
    // loadStocks();

    // WEAHTER LOAD
    function loadWeather(){
        $.simpleWeather({
            location: "New York, New York",
            success: function(weather) {
                $(".w_temp").html(weather.temp+"&deg;F");
                $(".w_current").html(weather.currently);
                $(".w_location").html(weather.city);
            },
            error: function(error) {}
        });
        $.simpleWeather({
            location: "Shanghai",
            success: function(weather) {
                $(".w_temp2").html(weather.temp+"&deg;F");
                $(".w_current2").html(weather.currently);
                $(".w_location2").html(weather.city);
            },
            error: function(error) {}
        });
        $.simpleWeather({
            location: "Sydney",
            success: function(weather) {
                $(".w_temp3").html(weather.temp+"&deg;F");
                $(".w_current3").html(weather.currently);
                $(".w_location3").html(weather.city);
            },
            error: function(error) {}
        });
        $.simpleWeather({
            location: "Duluth, MN",
            success: function(weather) {
                $(".w_temp4").html(weather.temp+"&deg;F");
                $(".w_current4").html(weather.currently);
                $(".w_location4").html(weather.city);
            },
            error: function(error) {}
        });
        $.simpleWeather({
            location: "Dubai",
            success: function(weather) {
                $(".w_temp5").html(weather.temp+"&deg;F");
                $(".w_current5").html(weather.currently);
                $(".w_location5").html(weather.city);
            },
            error: function(error) {}
        });
        $.simpleWeather({
            location: "Hong Kong",
            success: function(weather) {
                $(".w_temp6").html(weather.temp+"&deg;F");
                $(".w_current6").html(weather.currently);
                $(".w_location6").html(weather.city);
            },
            error: function(error) {}
        });
        $.simpleWeather({
            location: "Log Angeles, CA",
            success: function(weather) {
                $(".w_temp7").html(weather.temp+"&deg;F");
                $(".w_current7").html(weather.currently);
                $(".w_location7").html(weather.city);
            },
            error: function(error) {}
        });
    }


    function loadNews(){
        // $("#news_feed").rss("http://mix.chimpfeedr.com/88cc3-World-News", {
        // $("#news_right").rss("http://www.wired.com/category/business/feed/", {
        //     limit: 1,
        //     layoutTemplate: '<span>{entries}</span>',
        //     entryTemplate: '<a href="{url}" target="_blank" id="news_feature_link"><div id="news_teaser" style="background-image:url({teaserImageUrl});"><span id="news_feature_title">{title}</span></div></a>'
        // }).show();

        $("#news-feed").rss("http://mix.chimpfeedr.com/88cc3-World-News", {
            limit: 6,
            layoutTemplate: '<ul>{entries}</ul>',
            entryTemplate: '<li><a href="{url}" target="_blank">{title}</a></li>'
        }).show();

    }


    function loadStocks(){

        var cacheBuster = Math.floor((new Date().getTime()) / 1200 / 1000);

        var sql = "select symbol, price, name, change from csv where url='http://download.finance.yahoo.com/d/quotes.csv?s=GRUB,IBM,YHOO,GOOG,AAPL,BRKA,SEB,NVR,PCLN,TSLA&f=snl1d1t1c1ohgv&e=.csv' and columns='symbol,name,price,date,time,change,col1,high,low,col2'"

        var yqlUrl1= "http://query.yahooapis.com/v1/public/yql?q=" + encodeURIComponent(sql) + "&format=json&diagnostics=true&callback=?&_nocache=" + cacheBuster;

        $.getJSON(yqlUrl1, function(data){
            $.each(data.query.results.row, function(index, item){

                var element = $('<div></div>');

                element.append('<h2 class="ticker_name">' + item.symbol + '<br/><small>' + item.name + '</small></h2>');

                if (item.change.indexOf('+') > -1) {
                    element.append('<h3 class="stock_info">$' + parseFloat(Math.round(item.price * 100) / 100).toFixed(2)  + '<br/><small class="stockUp">' + item.change + ' <i class="ion-arrow-graph-up-right"></i></small></h3>');
                } else {
                    element.append('<h3 class="stock_info">$' + parseFloat(Math.round(item.price * 100) / 100).toFixed(2)  + '<br/><small class="stockDown">' + item.change + ' <i class="ion-arrow-graph-down-right"></i></small></h3>');
                }

                element.appendTo('#stocks_div');
            });
        })
    }




















});




// Todolist Progress check
$("#recommended_todo_list > li").click(function(){
    $(this).remove();
    checkListProgress();
});
function checkListProgress(){
    var listItems = $("#recommended_todo_list").children();
    var count = listItems.length;

    if(count==5){
        $("#todo_progress").val(3);
    }else if(count==4){
        $("#todo_progress").val(20);
    }else if(count==3){
        $("#todo_progress").val(40);
    }else if(count==2){
        $("#todo_progress").val(60);
    }else if(count==1){
        $("#todo_progress").val(80);
    }else if(count==0){
        $("#todo_progress").val(100);
        $("#recommended_todo_list").append("<li>Done!</li>");
    }



}
