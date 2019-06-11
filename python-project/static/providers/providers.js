$( document ).ready(function() {
    $('a').click(function(){
	    $('html, body').animate({
	        scrollTop: $($.attr(this, 'href')).offset().top
	    }, 500);
	    return false;
	});

	$("#sign_up, #sign_up2").click(function(){
		$("nav, article").addClass("blurit");
		$("#overlay").slideDown();
	})

	$("#continue_1").click(function(){
		$("#box1").slideUp(250);
		$("#box2").delay(350).slideDown();
	})
	$("#continue_2").click(function(){
		$("#box2").slideUp(250);
		$("#box3").delay(350).slideDown();
	})
	$("#submitit").click(function(){
		name = $("#form_name").val();
		email = $("#form_email").val();
		phone = $("#form_phone").val();
		title = $("#form_title").val();
		company = $("#form_company").val();
		address = $("#form_address").val();
		url = $("#form_url").val();
		revenue = $("#form_revenue").val();
		type = $("#form_type").val();
		services = $("#form_services").val();
		helpus = $("#form_helpus").val();

		$.ajax({
              type: 'POST',
              url: 'https://mandrillapp.com/api/1.0/messages/send.json',
              data: {
                'key': 'WKXdbEwmpCgbrFai8a10Ag',
                'message': {
                  'from_email': 'mitch@exportabroad.com',
                  'to': [
                      {
                        'email': 'austin.grandt@exportabroad.com',
                        'name': 'Austin Grandt',
                        'type': 'to'
                      }
                    ],
                  'autotext': 'true',
                  'subject': 'Export Providers - '+name,
                  'html': "<b>Name:</b> "+name+"<br/><br/><b>Email:</b> "+email+"<br/><br/><b>Phone:</b> "+phone+"<br/><br/><b>Job Title:</b> "+title+"<br/><br/><b>Company Name:</b> "+company+"<br/><br/><b>Company Address:</b> "+address+"<br/><br/><b>Company Website:</b> "+url+"<br/><br/><b>Company Revenue:</b> "+revenue+"<br/><br/><b>Company Type:</b> "+type+"<br/><br/><b>Company Offerings:</b> "+services+"<br/><br/><b>What can we do?</b> "+helpus
                }
              }
             }).done(function(response) {
                    $("#box3").slideUp(250);
					$("#box4").delay(350).slideDown();

        	});
	})


	$("#closeit").click(function(){
		$("#box4").slideUp(250);
		$("#overlay").slideUp();
		$("nav, article").removeClass("blurit");
	})

	


});