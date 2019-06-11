$( document ).ready(function() {


    loadWeather();
    loadNews();
    loadStocks();

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
        $("#news_feed").rss("http://mix.chimpfeedr.com/88cc3-World-News", {
            limit: 15,
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
