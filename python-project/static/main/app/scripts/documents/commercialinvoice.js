//==============================
//
//  commercialinvoice.js - doc
//  Controls invoice JS
//
//==============================
$(document).ready(function() {

	subtotal=0;
	miscTotal=0;
	match;

});



$(".ion-help-circled").click(function(){
	// $("label > b").slideUp();
	$(this).find("b").toggle();

})


function generateInvoice(){
	$("#doc_seller").html($("#seller").val().replace(/\r?\n/g, '<br />'));
	$("#doc_shipto").html($("#shipto").val().replace(/\r?\n/g, '<br />'));
	$("#doc_reference").html($("#reference").val());
	$("#doc_paymentterms").html($("#paymentterms").val());
	$("#doc_shipment").html($("#shipment").val());
	$("#doc_soldto").html($("#soldto").val().replace(/\r?\n/g, '<br />'));
	$("#doc_invoiceno").html($("#invoiceno").val());
	$("#doc_saleterms").html($("#saleterms").val());
	$("#doc_currency").html($("#currency").val());
	$("#doc_packagemarks").html($("#packagemarks").val().replace(/\r?\n/g, '<br />'));
	$("#doc_certifications").html($("#certifications").val().replace(/\r?\n/g, '<br />'));
	$("#doc_misccharges").html($("#misccharges").val().replace(/\r?\n/g, '<br />'));


	if($("#misccharges").val()==""){
		miscTotal=0;
	}else{
		miscCharge = $("#misccharges").val();

		rx=/[+-]?((\.\d+)|(\d+(\.\d+)?)([eE][+-]?\d+)?)/g;
		match = miscCharge.match( rx );
		miscTotal = eval(match.join('+'));
	}

	

	var sum = 0;
	var cells = document.querySelectorAll("td:nth-of-type(6)");
	for (var i = 0; i < cells.length; i++)
      sum+=parseFloat(cells[i].firstChild.data);

  	$("#subtotal").html(sum/2);

  	

  	endTotal = (sum/2)+(miscTotal);

  	$("#totaltotal").html(endTotal);

	window.print();

}

itemCounter = 0;
$("#add_to_invoice").click(function(){
	itemCounter++;

	itemDesc = $("#desc").val();
	if($("#quantity").val() == ""){
		itemQuantity=1;
	}else{
		itemQuantity=$("#quantity").val().replace(/\D/g,'');
	}
	itemPrice = $("#price").val();
	itemUnit = $("#unit").val();
	rowTotal = (itemPrice * itemQuantity).toFixed(2);

	$("#desc, #quantity, #price").val("");
	$("#tableExample").remove();





	$(".invoice_table").append("<tr class='item"+itemCounter+"'><td class='removeitem'>REMOVE</td><td>"+itemQuantity+"</td><td>"+itemDesc+"</td><td>"+itemUnit+"</td><td>"+itemPrice+"</td><td class='rtotal'>"+rowTotal+"</td></tr>");





})
	
//Remove items
$(".invoice_table").on("click", ".removeitem", function(){
	itemToRemove = $(this).parent().attr('class').split(' ')[0];
	$("."+itemToRemove).remove();
})