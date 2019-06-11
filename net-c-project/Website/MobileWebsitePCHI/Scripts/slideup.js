// JavaScript Document
$(document).ready(function () {
    var flag = 1;

    $("#Process").click(function () {
        if (flag == 1) {
            $('#BusyBox').stop(true).animate({ 'margin-bottom': 0, 'opacity': '1' }, { queue: false, duration: 300 });
            flag = 0;
        }
        else {
            $('#BusyBox').stop(true).animate({ 'margin-bottom': -200, 'opacity': '0' }, { queue: false, duration: 300 });
            flag = 1;
        }

        return false;
    });
});