$( document ).ready(function() {


    $("#techweek").delay(800).slideDown();
    $("#close_techweek").click(function(){
        $("#techweek").slideUp();
    });


    if(window.location.href.indexOf("product/")>-1){
		$("#product_link").addClass("active");
	}else if(window.location.href.indexOf("about/")>-1){
		$("#team_link").addClass("active");
	}else if(window.location.href.indexOf("pricing/")>-1){
		$("#pricing_link").addClass("active");
	}else if(window.location.href.indexOf("contact/")>-1){
		$("#contact_link").addClass("active");
	}




});



$("#show_mobile_nav_btn").click(function(){
    $("#nav_menu").slideToggle(200);
    $(this).toggleClass("ion-navicon")
    $(this).toggleClass("ion-ios-close-empty")
})



$("#contact_form_submit").click(function () {



        var empty = "";
        var name = $("#contact_form_name").val();
        var email = $("#contact_form_email").val();
        var message = $("#contact_form_message").val();
        if (empty == name || empty == email || empty == message)
        {
            $("#demo_form_error").show(400).delay(8000).hide(400);
            e.preventDefault();
        }
        else
       {
            $.ajax({
                  type: 'POST',
                  url: 'https://mandrillapp.com/api/1.0/messages/send.json',
                  data: {
                    'key': 'WKXdbEwmpCgbrFai8a10Ag',
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
                      'subject': 'Contact Us - '+name,
                      'html': '<b>Message From:</b> '+name+'<br/><b>Email Address:</b> '+email+'<br/><br/><b>Message:<br/></b> '+message
                    }
                  }
                 }).done(function(response) {
                     $("#contact_form_submit_icon").removeClass("ion-ios-paperplane-outline");
                     $("#contact_form_submit_icon").addClass("ion-ios-checkmark-empty");
                     $("#contact_form_submit").prop("disabled",true);
            });


        }


});
