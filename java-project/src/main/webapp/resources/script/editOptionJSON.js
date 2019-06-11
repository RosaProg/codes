/**
 * @param dataJson ( list data in json format)
 * @returns json output in the form id:label;
 */
function getEditOptionJSON(dataJson, nullOption){
	var result = "";
	
	if(typeof(nullOption) != "undefined"){
		result = nullOption.id + ":" + nullOption.label + ";";
	}
	
	var resultArray = [];
	/**
	 * using jQuery pasrJson method to get javascript object from json text
	 */
	var data = $.parseJSON(dataJson.responseText);
	/**
	 * iterating through the objects properties , if find an object then form the
	 * json text for edit option in jqGrid
	 */
	for(var propName in data){
		if(typeof(data[propName]) == "object"){
			/**
			 * iterating through the properties of the object, if the object
			 * has property label then form the string with format id:label
			 */
			
			for(i in data[propName]){
				var obj = data[propName][i];
				resultArray.push(obj);
				if(obj.hasOwnProperty("adminLabel.label")){
					result += obj.id + ":" + obj.adminLabel.label +";";	
				}else if(obj.hasOwnProperty("label")){
					result += obj.id + ":" + obj.label +";";	
				}
			}
		}
	}
	/**
	 * removing semicolon, which is the last character, from the string.
	 */
	result = result.slice(0, result.length-1);
	return result;
}	



/**
 * @param dataJson ( list data in json format)
 * @returns json output in the form id:label;
 */
function getautoCompleteJSON(dataJson){
	var resultArray = [];
	/**
	 * using jQuery pasrJson method to get javascript object from json text
	 */
	var data = $.parseJSON(dataJson.responseText);
	/**
	 * iterating through the objects properties , if find an object then form the
	 * json text for edit option in jqGrid
	 */
	for(var propName in data){
		if(typeof(data[propName]) == "object"){
			/**
			 * iterating through the properties of the object, if the object
			 * has property label then form the string with format id:label
			 */
			for(i in data[propName]){
				var obj = data[propName][i];
				resultArray.push(obj);
			}
		}
	}
	return resultArray;
}	