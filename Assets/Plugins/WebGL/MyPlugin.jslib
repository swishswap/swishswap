mergeInto(LibraryManager.library, {

	SetupGyroscope: function () {
		window.addEventListener("deviceorientation", function(event) { 
		
				var alpha = Math.round(event.alpha*10);
				var beta = Math.round(event.beta*10);
				var gamma = Math.round(event.gamma*10);
				
				SendMessage('Cube', 'setAlpha', alpha);
				SendMessage('Cube', 'setBeta', beta);
				SendMessage('Cube', 'setGamma', gamma);
				
		}, true);
	},
	
});
