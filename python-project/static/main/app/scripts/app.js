//==============================
//
//  app.js - universal app javascript
//  Controls all universal requirements
//
//==============================
$(document).ready(function() {


	//Account settings toggle
	$("#account_links").click(function(){
		$("#account_links_popup").toggle();
		$(".down_to_up_icon").toggleClass("ion-ios-arrow-down");
		$(".down_to_up_icon").toggleClass("ion-ios-arrow-up");
	});




	// // Smooth Anchor Scrolling
	$('a').click(function(){
	    $('html, body').animate({
	        scrollTop: $($.attr(this, 'href')).offset().top-100
	    }, 500);
	    return false;
	});





});
