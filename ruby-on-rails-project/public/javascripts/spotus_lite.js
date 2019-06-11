//set the iframe src onload of the widget
function setSrc(url, host){
	var _lite = document.getElementById('_lite');
	if (host!=''){
		_lite.src = url + '?host=' + escape(host);
	} 
	return '';
}

// Resize iframe to full height
function resizeIframe(){
	height = $.cookie("_iframe");
	jQuery('#_lite').attr('height', height);
}

jQuery(document).ready(function($){
	_lite_host = setSrc(_lite_url, _lite_host);
});