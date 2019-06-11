

/**
 * This method creates insert, update and delete options on jqGrid, this is just to reduce boilerplate code.
 * 
 * @param gridId  (this is gridId with # attached ex : "#gridId")
 * @param pagerId (this is pager id with # attached ex: "#pagerId")
 * @param addFunction ( this is call back function to add an instance)
 * @param editFunction (this is call back function to edit an existing instance)
 * @param deleteFunction (this is a call back function to delete existing instance)
 */
function showCrudOptions(gridId, pagerId, addFunction, editFunction, deleteFunction){
	
		// This is the pager
	    $(gridId).jqGrid('navGrid',pagerId,
	            {edit:false,add:false,del:false,search:false,recreateForm:true}
	    );
	    
	    if(addFunction != null){
	    	 // Custom Add button on the pager
		    $(gridId).navButtonAdd(pagerId,
		            {   caption:"", 
		                buttonicon:"ui-icon-plus", 
		                onClickButton: addFunction,
		                position: "last", 
		                title:"", 
		                cursor: "pointer"
		            } 
		    );
	    }
	
	    if(editFunction != null){
	 	   // Custom Edit button on the pager
		    $(gridId).navButtonAdd(pagerId,
		            {   caption:"", 
		                buttonicon:"ui-icon-pencil", 
		                onClickButton: editFunction,
		                position: "last", 
		                title:"", 
		                cursor: "pointer"
		            } 
		    );

	    }

	    // Custom Delete button on the pager
	    $(gridId).navButtonAdd(pagerId,
	        {   caption:"", 
	            buttonicon:"ui-icon-trash", 
	            onClickButton: deleteFunction,
	            position: "last", 
	            title:"", 
	            cursor: "pointer"
	        } 
	    );
}


//
// Funciton assumes that optionObject has an id property
// optionObject is the object we are taking out of the parsed json response text
// optionLabelField tells us what to display as the selection items

function createOptions(response,optionObject,optionLabelField){
	
		var options = "";
		var responseObject = $.parseJSON(response.responseText);
		var objects = responseObject[optionObject];
		
			for(var index = 0; index < objects.length; index++){
				options += "<option value=" + objects[index].id + ">" + objects[index][optionLabelField] + "</option>";
			}
			
			return options;
}


//a post ajax request
function ajaxRequestPost(url,message,checkDatasetInstance,data,successHandler){
	if(typeof(successHandler) === 'undefined'){
		successHandler = function() {};
	}
	
	checkDatasetInstance = (typeof checkDatasetInstance === "undefined") ? true : checkDatasetInstance;
	var JSONObject = $.ajax({
		  url:url,
		  dataType:'json',
		  type:"POST",
		  data:data,
		  async:true,
		  success:function(data){

			  	successHandler.call();
			  
		  },
		  error:function(e){
			  console.log(e.message);
		  }
	   });
	return JSONObject;
}


//ajaxRequest that checks to make sure datasetInstance is set, by default we check for datsetInstance, 
function ajaxRequest(url,message){
	

	var JSONObject = $.ajax({
		  url:url,
		  dataType:'json',
		  type:"GET",
		  async:false,
		  success:function(data){
			  if(checkDatasetInstance){
				  if(data.hasOwnProperty("datasetInstanceId")){
					  redirectIfDatasetInstanceIsNull(data.datasetInstanceId);
				  }
			  }

			  
		  },
		  error:function(e){
			  console.log(e.message);
		  }
	   });
	return JSONObject;
}

/**
 * This method is called for each row of the typeMetadataGroupGrid, 
 * return value is a string with html tags of type checkbox with an id (rowId + checkBox) and an onClick function having argument as
 * rowId.
 *  
 * @param cellValue
 * @param options
 * @param rowObject
 * @returns {String}
 */
function checkboxFormatter(cellValue, options, rowObject){
	var checkboxId = options.rowId+"checkBox";
	return '<input type="checkbox" id='+checkboxId+' value="false" onclick="checkValueSelected(\'' + options.rowId + '\')" />';
}

function checkboxUnFormat( cellvalue, options, cell){
	return $('input', cell).attr('checked');
}			
/**
 * @param datasetInstanceURL ( spring url variable ex: "${url}")
 * @returns datasetInstance data in json format
 */
function getDatasetInstance(datasetInstanceURL){
	var datasetInstanceJson = $.ajax({
		url: datasetInstanceURL,
		type:"GET",
		async:false,
		success: function(data){
			console.log("datasetInstancejson data retrieved successfully!");
		},
		error:function(e){
			console.log(e.message);
		}
	});
	return datasetInstanceJson;
}

/**
 * 
 * @param typeURL (spring url variable ex: "${typeURL}")
 * @returns type data in json format
 */
function getTypeJson(typeURL){
	var typeDataJson = $.ajax({
		  url:typeURL,
		  dataType:'json',
		  type:"GET",
		  async:false,
		  success:function(data){
			  console.log("types retrieved successfully!" );
		  },
		  error:function(e){
			  console.log(e.message);
		  }
	   });
	return typeDataJson;
}

/**
 * 
 * @param typeMetadatGroupsURL (spring url variable ex: "${typeMetadataGroupURL}")
 * @returns typeMetadataGroups with typemetadata data in json format
 */
function getTypeMetadataGroupJson(typeMetadataGroupURL){
	var typeMetadatGroupJson = $.ajax({
		  url:typeMetadataGroupURL,
		  type:"GET",
		  async:false,
		  success:function(data){
			  console.log("typeMetadataGroups retrieved successfully!");
		  },
		  error:function(e){
			  console.log(e.message);
		  }
	   });
	return typeMetadatGroupJson;
}

function getRelationshipClassJSON(rscURL){
	var rcsDataJson = $.ajax({
		  url:rscURL,
		  type:"GET",
		  async:false,
		  success:function(data){
			  console.log("relationshipClasses retrieved successfully!");
		  },
		  error:function(e){
			  console.log(e.message);
		  }
	   });
	return rcsDataJson;
}


var selectedDatasetInstance;

function getSelectedDatasetInstance(url){
		var urlToUse = url;
		var datasetInstanceSelectResponse = $.ajax({
		url: urlToUse,
		type:"GET",
		async:false,
		success: function(data){
			console.log("selected datasetinsatnce retrieved successfully!");
			//selectedDatasetInstance = $.parseJSON(data);
			//getSelectedDatasetInstance();
		},
		error:function(e){
			console.log(e.message);
		}
		});
		return datasetInstanceSelectResponse;
	}



function selectDatasetInstance(url,ids,selectedUrl){
		var urlToUse = url + "?datasetInstanceId=" + ids;
		var datasetInstanceSelectResponse = $.ajax({
		url: urlToUse,
		type:"GET",
		async:false,
		success: function(data){
			console.log("datasetinsatnce selected successfully!" + data);
			getSelectedDatasetInstance(selectedUrl);
		},
		error:function(e){
			console.log(e.message);
		}
		});
	}


/**
 * following methods are to show a popup div box to show success messages to the user.
 * elementId is the id of div that has to be popped up. This div should be set to display
 * none initially. After the success message is displayed the div box fades out.
 * 
 * @param elementId
 */
function showDivPopup(elementId){
	background_size(elementId);
	window_pos(elementId);
	toggle(elementId);
}

function fadeElement(elementId){
	$("#"+elementId).fadeToggle(3000, "linear");
}

/**
 * This method is to toggle between hiding and showing the popup div.
 * @param elementId
 */
function toggle(elementId) {
	var element = document.getElementById(elementId);
	if ( element.style.display == 'none' ) {	element.style.display = 'block';}
	else {element.style.display = 'none';}
}

/**
 * positioning the height of the popup div to the center of the screen.
 * @param popUpDivVar
 */
function background_size(popUpDivVar) {
	if (typeof window.innerWidth != 'undefined') {
		viewportheight = window.innerHeight;
	} else {
		viewportheight = document.documentElement.clientHeight;
	}
	if ((viewportheight > document.body.parentNode.scrollHeight) && (viewportheight > document.body.parentNode.clientHeight)) {
		blanket_height = viewportheight;
	} else {
        blanket_height = Math.max(document.body.parentNode.clientHeight, document.body.parentNode.scrollHeight);
	}
	var popUpDiv = document.getElementById(popUpDivVar);
	popUpDiv_height=blanket_height/2-200;//200 is half popup's height
	popUpDiv.style.top = popUpDiv_height + 'px';
}

/**
 * positioning the width of the popup div to the center of the screen.
 * @param popUpDivVar
 */
function window_pos(popUpDivVar) {
	if (typeof window.innerWidth != 'undefined') {
		viewportwidth = window.innerHeight;
	} else {
		viewportwidth = document.documentElement.clientWidth;
	}
	if ((viewportwidth > document.body.parentNode.scrollWidth) && (viewportwidth > document.body.parentNode.clientWidth)) {
		window_width = viewportwidth;
	} else {
        window_width = Math.max(document.body.parentNode.clientWidth, document.body.parentNode.scrollWidth);
	}
	var popUpDiv = document.getElementById(popUpDivVar);
	window_width=window_width/2-200;//200 is half popup's width
	popUpDiv.style.left = window_width + 'px';
}


/**
 * This method gets the formId ( the form we are performing the opertion on) , binds the input
 * tags to keypress function. When the keyCode is 13 ( equalent to "ENTER"), get the 
 * submit button tag(in jqGrid, this is a href) and perform the click.
 * 
 * @param formId
 * @returns return false to stop default event propagation.
 */
function submitFormOnEnter(formId){
	$("input", formId).keypress(function(e){
		//checking for enter key press
		if(e.keyCode == 13){
			var submitButton = $("#sData");
			submitButton.trigger("click");
			return false;
		}
	});
}


function createGrid(gridId,baseURL,rootElement,pagerId,colNames,colModel,beforeShowForm,includeCRUD,multiSelect,customListUrl,selectRow,beforeSubmit,tableName){
	if ((typeof(customListUrl) === 'undefined') || (customListUrl == null)){
		customListUrl = "/list";
	}
	if(typeof(selectRow) === 'undefined'){
		selectRow = function(ids) {};	
	}
	
	if(typeof(tableName === 'undefined') || (tableName == null)){
		tableName = rootElement;
	}
	
	 function editRow(){
	 	  	var gr = jQuery("#" + gridId).jqGrid('getGridParam','selrow');
			if( gr != null ){
				jQuery("#" + gridId).jqGrid('editGridRow',gr,{width:350, reloadAfterSubmit:true, closeAfterAdd: false,
	    		// before we show the form we want to get the units that are applied to this users groups for selection
	    		beforeShowForm:beforeShowForm,
				onclickSubmit:function(params, postData){
					/*
					 * attaching id column to the object, server checks for id > 0
					 * when inserting.
					*/
						
					return postData;
				}});	
				
			}
			else { alert("Please Select Row");
			}
			
			
			jQuery("#" + gridId).jqGrid('setGridParam', {datatype: "json",loadonce:false, sortable: true,reloadAfterSubmit:true });


	    }

	
	
	
    function addRow(){
	    		jQuery("#" + gridId).jqGrid('editGridRow',"new",{width:350, reloadAfterSubmit:true, closeAfterAdd: false ,
	    			beforeShowForm:beforeShowForm,
	    			beforeSubmit:beforeSubmit,
	    			onclickSubmit:function(params, postData){
						/*
						 * attaching id column to the object, server checks for id > 0
						 * when inserting.
						*/
						postData.id = 0;
						return postData;
					}});
	    	
		jQuery("#" + gridId).jqGrid('setGridParam', {datatype: "json",loadonce:false, sortable: true,reloadAfterSubmit:true });

	    }
	    
	
	
	function deleteRow(){
			jQuery("#" + gridId).jqGrid('setGridParam', {datatype: "json",loadonce:false, sortable: true,reloadAfterSubmit:true });
			var gr = jQuery("#" + gridId).jqGrid('getGridParam','selrow');
			if( gr != null ) jQuery("#" + gridId).jqGrid('delGridRow',gr,{reloadAfterSubmit:true});
			else alert("Please Select Row to delete!");
 	  }
	
	
	jQuery("#" + gridId).jqGrid({
	   	url:baseURL + customListUrl,
	    datatype: "json",
	    height:400,
	    colNames:colNames,
	    colModel:colModel,
	    multiselect: multiSelect,
		onSortCol : function(index,	iCol,sortorder) { 
			jQuery("#" + gridId).jqGrid('setGridParam', {loadonce:true, sortable: true });
			return "";
		},
		onSelectRow:selectRow,
		
			jsonReader : {
				root:rootElement, 
				page: "currentPage", 
				total: "totalPages", 
				records: "totalRecords", 
				repeatitems: false,
				id: "id" },
		closeAfterEdit:true,
		closeAfterAdd:true,
		pginput : false,
		reloadAfterSubmit:true,
		rowNum:999999,
		mtype:'POST',
		pager: "#" + pagerId,
		sortorder: "asc",
		loadonce:true,
		sortable: true, 
		pgbuttons:false,
		height: 310,
		viewrecords: true,
		sortable:true,
		caption:tableName,
		editurl:baseURL + "/save",
		edit:{ recreateForm:true }
	});
	
	if(includeCRUD == 'd'){
		showCrudOptions("#" + gridId, "#" + pagerId, null, null, deleteRow);	
	}
	else if (includeCRUD == 'e'){
		showCrudOptions("#" + gridId, "#" + pagerId, null, editRow, deleteRow);	
		
		
	}else if(includeCRUD == true){
		showCrudOptions("#" + gridId, "#" + pagerId, addRow, editRow, deleteRow);
	}
	

	
}
/*
**
* This function updates the order and refreshes the list
*/
function updateOrder(gridId, url){
	var rowIds = $("#"+gridId).jqGrid("getDataIDs");
	var choiceList = [];
	/**
	 * iterating through each row of the grid and creating unit and group objects, we do
	 * this so that we can map the json created to the server side unitgroup object.
	 */
	for(var i =0; i < rowIds.length; i++){
		choiceList.push(rowIds[i]);
	}
	var postData = {};
	postData.ordered = JSON.stringify(choiceList);
	console.log(postData.ordered);
	$.ajax({
		url:url+"/updateOrder",
		type:"POST",
		dataType : 'json',
		data : postData,
		success:function(data){
			jQuery("#"+gridId).jqGrid().trigger("reloadGrid");
		},
		error:function(e){
			
		}
	});
}

/**
 * This method checks if a row with "label" exists in the gridId and returns boolean
 * @param label
 * @param gridId
 * @returns {Boolean}
 */
function checkIflabelIsUnique(label, gridId)
 {
	   var labelNames = [];
	   var rowIds = $(gridId).jqGrid('getDataIDs');
	   for ( var rowId in rowIds) {
		    var labelData = $(gridId).jqGrid('getCell',rowIds[rowId],'label');
		    //convert the string to lowerCase and add to the array;
			labelNames.push(labelData.toLowerCase());
	   }
	   /**
	   	* convert the label to lowerCase and check if we have any match in the array we created.
	   	  If we have a match $.inArray returns the array index, or it will return -1. check if
	   	  the returned value is greater then -1 , if it is then the label is not unique return false
	   	  else true.
	   	*/
	   if(($.inArray(label.toLowerCase(), labelNames)) > -1){
		  	return false;   
	   }
	   else
		   return true;
	   
 }
