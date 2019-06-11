//==============================
//
//  packinglist.js - doc
//  Controls packing list JS
//
//==============================
$(document).ready(function() {



});



$(".ion-help-circled").click(function(){
	// $("label > b").slideUp();
	$(this).find("b").toggle();

})


function generateInvoice(){
	$("#form_to").html($("#in_to").val().replace(/\r?\n/g, '<br />'));
	$("#form_shipto").html($("#in_dest").val().replace(/\r?\n/g, '<br />'));
	$("#form_order_no").html($("#in_orderno").val());
	$("#form_shipped").html($("#in_shipmethod").val());

	$("#cases").html($("#in_cases").val());
	$("#packages").html($("#in_packages").val());
	$("#drums").html($("#in_drums").val());
	$("#reels").html($("#in_reels").val());
	$("#crates").html($("#in_crates").val());
	$("#cartons").html($("#in_cartons").val());
	$("#barrels").html($("#in_barrels").val());

	$("#form_marks").html($("#in_marks").val().replace(/\r?\n/g, '<br />'));


	window.print();

}
