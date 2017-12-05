mergeInto(LibraryManager.library, {

	SetupGyroscope: function () {
		window.addEventListener("deviceorientation", function(event) { 
		
				var alpha = Math.round(event.alpha*10);
				var beta = Math.round(event.beta*10);
				var gamma = Math.round(event.gamma*10);
				
				SendMessage('Camera', 'setAlpha', alpha);
				SendMessage('Camera', 'setBeta', beta);
				SendMessage('Camera', 'setGamma', gamma);
				
		}, true);
	},


	ALittlePig: function() {
		
		var gn = new GyroNorm();

		gn.init().then(function(){
			gn.start(function(data){

				var alpha = Math.round(data.do.alpha*10);
				var beta = Math.round(data.do.beta*10);
				var gamma = Math.round(data.do.gamma*10);

				SendMessage('Camera', 'setAlpha', alpha);
				SendMessage('Camera', 'setBeta', beta);
				SendMessage('Camera', 'setGamma', gamma);
				
				// data.do.beta		( deviceorientation event beta value )
				// data.do.gamma	( deviceorientation event gamma value )
				// data.do.absolute	( deviceorientation event absolute value )

				// data.dm.alpha	( devicemotion event rotationRate alpha value )
				// data.dm.beta		( devicemotion event rotationRate beta value )
				// data.dm.gamma	( devicemotion event rotationRate gamma value )
			});
		}).catch(function(e){
		  // Catch if the DeviceOrientation or DeviceMotion is not supported by the browser or device
		});
		
	},

	
});
