
//$(function() {
//$("div.index_header").flashembed("../swf/index.swf");
//});

$(document).ready(function(){ 
	$('div.page_menu').flashembed({
		src: '../swf/menu.swf', 
		version: [8, 0], 
		wmode: 'transparent', 
		menu: 'false', 
		quality: 'high', 
		allowfullscreen: 'false', 
		allowscriptaccess: 'false'
	}); 
}); 

$(document).ready(function(){ 
	$('div.logo').flashembed({
		src: '../swf/page_top.swf', 
		version: [8, 0], 
		wmode: 'transparent', 
		menu: 'false', 
		quality: 'high', 
		allowfullscreen: 'false', 
		allowscriptaccess: 'false'
	}); 
}); 

