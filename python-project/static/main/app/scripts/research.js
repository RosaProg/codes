$( document ).ready(function() {



	//Show/Hide fake loading. If returning, dont show it.
	if ( document.location.href.indexOf('#return') > -1 ) {$("#fake_loader").remove();}
	setTimeout(function() {$("#pdf_ins_head").text("Calculating...");}, 1000);
	setTimeout(function() {$("#pdf_ins_head").text("Generating Graphs...");}, 3000);
	setTimeout(function() {$("#pdf_ins_head").text("Done!");}, 4500);
	setTimeout(function() {$("#fake_loader").animate({top: "-100%",}, 500 );}, 5500);
	setTimeout(function() {$("#fake_loader").remove();}, 6000);



//	X   X  XXX   XXXX
//	XX XX X   X  X  X
//  X X X XXXXX  XXXX
//  X   X X   X  X
//  X   X X   X  X
//This is the map at the top of the results page
//Edit this

	// var basic = new Datamap({
	//   	element: document.getElementById("results_map_wrapper"),
	// 	fills: {
	// 	    defaultFill: "#333f53",
	// 	    topCountries: "#00BEC3"
	// 	},
	// 	data: {
	// 		// {% for market in top_five %}
	// 		// 	{{ market.country_id }}: { fillKey: "topCountries" },
	// 		// {% endfor %}
	// 	 //    CHN: 
	// 	    JPN: { fillKey: "topCountries" },
	// 	    ITA: { fillKey: "topCountries" },
	// 	    CRI: { fillKey: "topCountries" },
	// 	    KOR: { fillKey: "topCountries" },
	// 	    DEU: { fillKey: "topCountries" },
	// 	},
	// 	geographyConfig: {
	//         borderWidth: 0.2,
	//         borderColor: '#e4e4e4',
	//         popupTemplate: function(geography, data) { //this function should just return a string
	//           return '<div class="hoverinfo"><strong>' + geography.properties.name + '</strong></div>';
	//         },
	//         popupOnHover: true, //disable the popup while hovering
	//         highlightOnHover: true,
	//         highlightFillColor: '#DE6262',
	//         highlightBorderColor: '#e4e4e4',
	//         highlightBorderWidth: 0.3
 //    	},
	// });



//  XXXXX  XXXXX  XXXXX
//  X   X    X    X
//  XXXXX    X    XXX
//  X        X    X
//  X      XXXXX  XXXXX
//Pie graph data
//Edit this

	


//  BBBBB    BBB   BBBB
//  B    B  B   B  B   B 
//  BBBBB   BBBBB  BBBB
//  B    B  B   B  B  B
//  BBBBB   B   B  B   B
//Bar Graph Data
//Edit this


//  L      LLLLL  L   L  LLLLL
//  L        L    LL  L  L
//  L        L    L L L  LLL
//  L        L    L  LL  L
//  LLLLL  LLLLL  L   L  LLLLL
//Line graph data
//Ed





	// Number counter animation. Cool but totally unnecessary
	//Number counter - cool but totally unnecessary
    $('.countIt').each(function () {
      var $this = $(this);
      jQuery({ Counter: 0 }).animate({ Counter: $this.text() }, {
        duration: 1000,
        easing: 'swing',
        step: function () {
          $this.text(Math.ceil(this.Counter));
        }
      });
    });

});