mergeInto(LibraryManager.library, {

	SetupGyroscope: function () {
		window.addEventListener("deviceorientationabsolute", function(event) { 
		
				var alpha = Math.round(event.alpha*10);
				var beta = Math.round(event.beta*10);
				var gamma = Math.round(event.gamma*10);
				
				SendMessage('Camera', 'setAlpha', alpha);
				SendMessage('Camera', 'setBeta', beta);
				SendMessage('Camera', 'setGamma', gamma);
				
		}, true);
	},
	
	TheGreatestFunctionEver: function() {
		var a = actLikeAPig();
		return a;
	},
	
});
