Function.prototype.method = function ( name, func) {

	var prev = this.prototype[ name ];
	
	this.prototype[name] = function()
	{
		if ( func.length == arguments.length )
			return func.apply( this, arguments );
		else if ( typeof prev == 'function' )
			return prev.apply( this, arguments );
    };
	
    return this;
};


Function.method('inherits', function (parent) {
	
	this.prototype = new parent();
    return this;
});





