
//$(function() {
//$("div.index_header").flashembed("../swf/index.swf");
//});

$(document).ready(function(){ 
	$('div.index_header').flashembed({
		src: '../swf/index.swf', 
		version: [8, 0], 
		wmode: 'transparent', 
		menu: 'false', 
		quality: 'high', 
		allowfullscreen: 'false', 
		allowscriptaccess: 'false'
	}); 
}); 


