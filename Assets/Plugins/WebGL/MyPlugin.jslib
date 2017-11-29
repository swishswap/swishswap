mergeInto(LibraryManager.library, {

	SetupGyroscope: function () {
		window.addEventListener("deviceorientationabsolute", function(event) { 
		
				var alpha = Math.round(event.alpha*10);
				var beta = Math.round(event.beta*10);
				var gamma = Math.round(event.gamma*10);
				
				SendMessage('Watch', 'setAlpha', alpha);
				SendMessage('Watch', 'setBeta', beta);
				SendMessage('Watch', 'setGamma', gamma);
				
		}, true);
	},
	
});
