mergeInto(LibraryManager.library, {

	SetupGyroscope: function () {
		window.addEventListener("deviceorientation", function(event) { 
		
				var alpha = Math.round(event.alpha);
				var beta = Math.round(event.beta);
				var gamma = Math.round(event.gamma);
				
				SendMessage('Cube', 'setAlpha', alpha);
				SendMessage('Cube', 'setBeta', beta);
				SendMessage('Cube', 'setGamma', gamma);
				
		}, true);
	},
	
});
