function GetQueryStringParams(sParam)
{
	/// <summary>
	/// Gets the query string value with the requested name
	/// </summary>
	/// <param name="sParam">The parameter name of the querystring entry requested</param>
    /// <returns type="">The requested querystring value or null if not found</returns>

    var sPageURL = window.location.search.substring(1);
    var sURLVariables = sPageURL.split('&');
    for (var i = 0; i < sURLVariables.length; i++) 
    {
        var sParameterName = sURLVariables[i].split('=');
        if (sParameterName[0] == sParam) 
        {
            return sParameterName[1];
        }
    }
}

function isFunction(functionToCheck) {
    var getType = {};
    return functionToCheck && getType.toString.call(functionToCheck) === '[object Function]';
}

function ajaxLoader() {
    document.getElementById("LoginText").style.display = "none"; document.getElementById("LoaderImg").style.display = "block";
}