var PARSE_APP = "INSERT PARSE APP ID HERE";
var PARSE_JS = "INSERt PARSE JS ID HERE";
var searchSuburb = "";
var searchCuisine = "";
var cuisineList = new Array;


function getCuisine(){
		/*var dropDown = $('#style');
		cuisineObjects = Parse.Object.extend("Cuisine");
		var query = new Parse.Query(cuisineObjects);
		query.ascending("Description");
		query.find({
			success:function(results) {
				for(var i=0, len=results.length; i<len; i++) {
					var styles = results[i]; 
                    cuisineList[i] = styles.get("Description");
					dropDown.append("<option>"+ cuisineList[i] +"</option>");
				}
			},
			error:function(error) {
				alert("Error when getting food list!");
			}
		});
		dropDown[0].selectedIndex = 0;
		dropDown.selectmenu("refresh");*/
}

function getRestaurants() {
		RestaurantObjects = Parse.Object.extend("Restaurant");
		var query = new Parse.Query(RestaurantObjects);
		query.equalTo("Cuisine1", searchCuisine);
		//query.equalTo("Suburb",  searchSuburb);
		query.find({
			success:function(results) {
				console.dir(results);
				var s = "<ol data-role=\"listview\">";
				for(var i=0, len=results.length; i<len; i++) {
					var restaurants = results[i];
					s += "<li><h3><a href=\"#dealdetail\">"+restaurants.get("Name")+"</h3>";
					s += restaurants.get("Suburb") + "<br/>";
					s += restaurants.get("Cuisine1")+"</p></a>";
					s += "</li>"	
				}	
				s+="</ol>";
				$("#restaurants").html(s);
			},
			error:function(error) {
				alert("Error when getting restaurants!");
			}
		});
}


function saveName(){
    window.localStorage.setItem("userName", document.getElementById("userName").value);
	alert("User Saved");

}

function setNameFormElement(){
	var userName = window.localStorage.getItem("userName");
	document.getElementById("userName").value = userName;
}

function clearName(){
	//      window.localStorage.clear();
	window.localStorage.removeItem("userName");
	document.getElementById("userName").value = "";
	alert("User Cleared");
}


$( document ).ready(function() {
	console.log( "ready!" );
	document.addEventListener("deviceready", onDeviceReady, false);
	//Parse.initialize(PARSE_APP, PARSE_JS);
	//getCuisine();
	//setNameFormElement();
});

function doSearch(){
	//searchSuburb = document.getElementById("location").value;
	searchCuisine= document.getElementById("style").value;
	//alert(searchCuisine);
	getRestaurants();
	window.location = "#searchlist";
}



function onDeviceReady() {
    console.log(navigator.camera);
    alert('Device is ready!');
	//set up a variable to ensure that you can't do anything unless
	//device is actually ready
}


function TakePhotoUsingCamera() {
	TakePhoto(Camera.PictureSourceType.CAMERA);
};
 

function TakePhotoFromLibrary() {
	TakePhoto(Camera.PictureSourceType.PHOTOLIBRARY);
};


function onSuccess(imageData) {
    var image = document.getElementById('myImage');
    image.src = "data:image/jpeg;base64," + imageData;
}

function onFail(message) {
    alert('Failed because: ' + message);
}


function TakePhoto(sourceType) {
var camOptions = {
quality:50,
destinationType: Camera.DestinationType.DATA_URL,
sourceType: sourceType,
correctOrientation: true
};
navigator.camera.getPicture(onSuccess, onFail, camOptions);
}


