$(function(){
$("#ctl00_content_SearchControls_ctl03_txbProvider").keydown(function(event){
	
if (event.which == 13)
	{

	event.preventDefault(); 
	$("#ctl00_content_SearchControls_ctl03_btnSearch").click();

	
	}
});

});
