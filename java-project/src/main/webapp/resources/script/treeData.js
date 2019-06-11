/**
 * This is variable is used to store root objects name
 */
var rootObjectName = "";
/**
 * This variable holds the list of json objects with the properties
 * (level, parent, expanded and isLeaf) attached.
 */
var treeList = [];
/**
 * This variable holds the depth of a tree in the form of array, values are unique.
 * ex(['0','1','2']
 * to get depth of the tree use length property of leaves.
 */
var leaves = [];
/**
 * This variable holds the list of propertyNames that are objects for a given
 * root object (ex for a given typeMetadataGroups we have typeMetadatas as object property,
 * this gets stored in the childPropertyNameList)
 */
var childPropertyNameList = [];

/**
 * This method gets the Json data object , converts the data to tree structure
 * compatible to the jqGrid treegrid and returns json string.
 * 
 * @param dataJson
 * @returns {String}
 */
function generateTreeData(dataJson){
	/**
	 * This variable holds the root object of the json text sent
	 */
	var data = [];
	/**
	 * This variable is used to store the treegrid tree data structure of the
	 * json data root object.
	 */
	var treeJsonText = "";
	
	var jsonObject = {};
	/**
	 * loop through the data received and get the root object and its name.
	 */
	for(var propName in dataJson){
		//if we find the root object end the loop
		if(typeof dataJson[propName] === "object"){
			data = dataJson[propName];
			rootObjectName = propName;
			break;
		}
		else{
			continue;
		}
	}
	
	/**
	 * loop through root object and add the tree structure properties
	 * (level -- to know the depth of the tree, 
	 * parent -- it is null for parents and parentId for leaves,
	 * isLeaf -- boolean specifies if the object is leaf or a parent, 
	 * expanded -- boolean specifies if node had to 
	 * be expanded when the tree is loaded)
	 * 
	 */
	var list = getDepth(data,"");
	/**
	 * looping through the list of tree objects
	 */
	for(var k = 0; k < list.length; k++){
		var listObj = list[k];
		/**
		 * for each object loop through the propertyname list (childPropertyNameList) , if the object
		 * has any of the properties match from the childPropertyNameList, then delete the property from the object.
		 */
		for(var prop in childPropertyNameList){
			if(listObj.hasOwnProperty(childPropertyNameList[prop])){
				delete listObj[childPropertyNameList[prop]];
			}
			/**
			 * if level of object is not equal to the highest level , then set the isLeaf
			 * property of the object to false.
			 */
			if(listObj.level != leaves[leaves.length-1]){
				listObj.isLeaf = false;
			}
		}
	}
	
	jsonObject[rootObjectName] = list; 
	treeJsonText = JSON.stringify(jsonObject);
	//console.log("treeData "+ treeJsonText);
	return treeJsonText;
}	

/**
 * This method gets the root object to traverse through along with the parentId(which is null for the root object)
 * and returns a list of objects constructed with properties specific to jqGrid tree grid(level, parent, isLeaf and expanded)
 * 
 * @param data -- root object
 * @param parentId
 * @returns {Array}
 */
function getDepth(data, parentId){
	/**
	 * looping through list of objects
	 */
	for(var i in data){
		var obj = data[i];
		//this variable holds the level of the tree we are processing.
		var depth = parentId.split("_");
		obj.level = depth.length-1;
		if(parentId == null || parentId == 0)
			obj.parent = "";
		else
			obj.parent = parentId;
		/**
		 * adding level to leaves variable, only if the level doesn't
		 * exist in leaves.
		 */
		if(leaves.indexOf(obj.level) == -1)
			leaves.push(obj.level);
		//setting isLeaf and expanded to true and false for all the objects, respectively.
		obj.isLeaf = true;
		obj.expanded = false;
		/**
		 * adding "_" and parentId to each object we process. The underscore helps us keeping track of the
		 * the level, at which we are processing the object.
		 */
		obj.id = obj.id + "_"+ parentId;
		treeList.push(obj);
		/**
		 * looping through the properties of the obj.
		 */
		for(var propName in obj){
			/**
			 * if obj doesn't have a property typeof 'object' return the list else
			 * add the propertyName of the object to childPropertyNameList(unique values)
			 * and call the getDepth method with object property of "obj" as root object and 
			 * obj.id as parentId.  
			 */
			if(typeof obj[propName] != "object"){
				continue;
			}
			else if(typeof obj[propName] === "object"){
				if(childPropertyNameList.indexOf(propName) == -1){
					childPropertyNameList.push(propName);
				}
				getDepth(obj[propName], obj.id);
			}
		}
	}
	return treeList;
}




function createTreeGrid(gridId, baseUrl, rootElement){
	url = baseUrl+"/listTree";
	var jsonRequest = ajaxRequest(url);
	var jsonObject = $.parseJSON(jsonRequest.responseText);
	var jsonTree = jsonObject;
	jQuery("#"+gridId).jqGrid({
	    datastr: jsonTree,
	    datatype: "jsonstring",
	    height: "auto",
	    loadui: "disable",
	    colNames: ["Id","Label","Selected", "level", "parent", "isLeaf"],
	    colModel: [
	        {name:"id", hidden:true},
	        {name: "label",index:"label", width:150, resizable: false},
	        {name: 'selected', index: 'selected', width: 60, align: 'center',formatter: checkboxFormatter, editoptions: {value: '1:0'},formatoptions: {disabled: false}},
	        {name:"level", index:"level", hidden:true},
	        {name:"parent", index:"parent", hidden:true},
	        {name:"isLeaf", index:"isLeaf",  hidden:true}
	    ],
	    treeGrid: true,
	    treeGridModel: "adjacency",
	    caption: "jqGrid Demos",
	    ExpandColumn: "label",
	    multiselect:true,
	    autowidth: true,
	    rowNum: -1,
	    ExpandColClick: true,
	    jsonReader: {
	        repeatitems: false,
	        root: rootElement
	    }
	});

}

     
