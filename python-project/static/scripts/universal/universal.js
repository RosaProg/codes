$( document ).ready(function() {

    //
    // Search function - Display
    //
    $("#show-search").click(function(){
        $("#scroll-header-title").text("Searching for:")
        $("#header-search").show();
        $("#header-search").animate({
            marginLeft: "0px",
            opacity: 1
        }, 110, "linear", function() {});
        $("#header-search").focus();
        $("#clear-search").show();
        $(this).hide();
    });
    $('#header-search').blur(function() {

        if($("#header-search").val() == ""){
            $("#show-search").show();
            $("#clear-search").hide();
            $("#scroll-header-title").text("Dashboard > Leads");
            $("#header-search").animate({
                marginLeft: "195px",
                opacity: 0
            }, 110, "linear", function() {});
            setTimeout(function(){
                  $("#header-search").hide();
                  $("#header-search").val("");
            }, 200);
        }
    });



    // Actual Searching
    $('#header-search').keyup(function(event) {
        $(".list-item").hide();

        var search_input = $('#header-search').val();

        $('.list-item').each(function(){
            if($(this).text().toLowerCase().indexOf(search_input) >= 0 ){
                $(this).show();
            }
         });

         if (!$('.list-item:visible').length){
             $("#list-no-results").show();
         }else{
             $("#list-no-results").hide();
         }

    });





    //
    // Turn list item blue when clicked
    //
    $(".list-item").click(function(){
        $(".list-item").removeClass("active");
        $(this).addClass("active");
    })



    //
    // Toggle List/Grid function - Display
    //
    $("#display-list").click(function(){
        $(this).hide();
        $(".view-item").show();
        $("#display-grid").show();
        $(".list-wrapper").toggleClass("list-list list-grid");
    });
    $("#display-grid").click(function(){
        $(this).hide();
        $(".view-item").hide();
        $("#display-list").show();
        $(".list-wrapper").toggleClass("list-list list-grid");
    });



    //
    // Sort List Functions
    //
    $("#sort-by-name").on('click', function(){
        $("#sort-by-name > .sort-icon").toggleClass("ion-arrow-down-c");
        $("#sort-by-name > .sort-icon").toggleClass("ion-arrow-up-c");
        $(this).toggleClass("sorteddown");

        if ( $( this ).hasClass( "sorteddown" ) ) {
            $('.list-item').sortElements(function(a, b){
                return $(a).text() > $(b).text() ? 1 : -1;
            });
        }else{
            $('.list-item').sortElements(function(a, b){
                return $(a).text() < $(b).text() ? 1 : -1;
            });
        }
    })



    //
    // Lead RequestModal Control
    //
    $("#close-modal, #lightbox-background").click(function(){
        $(".popup-modal").slideUp(200);
        setTimeout(function() {
            $("#lightbox-background").fadeOut();
        }, 150);
    })
    $("#show-lead-request").click(function(){
        $("#lightbox-background").fadeIn(200);
        setTimeout(function() {
            $(".popup-modal").slideDown(200);
        }, 150);
    })

    //
    // Email Request Leads
    //
    $("#send-lead-request").click(function () {
		$(this).html("Sending");
        var empty = "";
        var username = $("#lr-username").val();
        var customer = $("#lr-name").val();
        var country = $("#lr-country").val();
        var product = $("#lr-product").val();
            $.ajax({
                type: 'POST',
                url: 'https://mandrillapp.com/api/1.0/messages/send.json',
                data: {
                    'key': 'lq4msOm2tOT7-hPl3fBw6A',
                    'message': {
                        'from_email': 'mitch@exportabroad.com',
                        'to': [
                    {
                        'email': 'austin@exportabroad.com',
                        'name': 'Austin Grandt',
                        'type': 'to'
                    }
                    ],
                    'autotext': 'true',
                    'subject': 'Lead Request - '+username,
                    'html': 'Yo <b>'+username+'</b> needs leads for HS#<b>'+product+'</b> in <b>'+country+'</b> for <b>'+customer+'</b>'
                }
            }
            }).done(function(response) {
				$("#send-lead-request").html("Sent! <i class='ion-checkmark-round'></i>");
                setTimeout(function(){
                        $(".popup-modal").slideUp(200);
                        setTimeout(function() {
                            $("#send-lead-request").html("Already Submitted");
                            $("#another-lead-request").show();
                            $("#send-lead-request, #lr-name, #lr-country, #lr-product").prop("disabled",true);
                            $("#lightbox-background").fadeOut();
                        }, 150);
                  }, 1000);


            });
    });
    //
    // Request Another
    //
    $("#another-lead-request").click(function(){
        $(this).hide();
        $("#send-lead-request").html("Submit");
        $("#send-lead-request, #lr-name, #lr-country, #lr-product").prop("disabled",false);
        $("#lr-name, #lr-country, #lr-product").val("");
    })


});
